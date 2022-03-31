using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Photon.Pun;
using UnityEngine.UI;

//------------------------------------------------------------------
// Script base del movimiento del golem, funciona similar al explorador en ciertos sentidos.
//------------------------------------------------------------------
public class GolemController : MonoBehaviour, IPunObservable
{
    public Transform camara;
    public CinemachineFreeLook vCam;
    public Transform referenciaY;
    private Rigidbody rb;
    private Animator anim;

    public PhotonView view;

    public enum GOLEM_TYPE
    {
        ROCKGOLEM, FIREGOLEM, ICEGOLEM
    }
    [Header("Variables de Golem")]
    public GOLEM_TYPE golemType;
    public bool isMounted;
    public bool isRegenerating;
    public float healthPoints;
    public Image healthBar;
    public int attackDamage;
    public float pushForce;
    public PhotonView playerMounting;
    private int playerMountingID;
    private GameObject attackHitbox;
    private float attackCooldown;
    public float specialCooldown;
    public float maxSpecialCooldown;
    private GameObject iceHitbox;

    [Header("Movimiento Terrestre")]
    public float velocidadMovimiento = 5f;
    private bool isSlowed;
    private float originalSpeed;
    public float suavizador = 0.1f;
    public float suavizadorVel;
    public bool canMove;

    [Header("Salto y Detección de Suelo")]
    public float poderSalto = 3;
    public bool estaEnSuelo;
    public Transform chequeadorSuelo;
    public float radioChequeador = 0.4f;
    public LayerMask mascaraSuelo;

    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;

    [Header("Detección de objetos (Raycast)")]
    public float raycastDistance;
    RaycastHit hit;

    [Header("Material de H I E L O")]
    public Material iceMat;
    [Header("UI")]
    public Sprite attackSprite;
    public Sprite specialSprite;
    [HideInInspector] public Image hpBar;
    [HideInInspector] public Image mouse1Image;
    [HideInInspector] public Image mouse1Cooldown;
    [HideInInspector] public Image mouse2Image;
    [HideInInspector] public Image mouse2Cooldown;

    public AudioSource golpesfx;

    private void Awake()
    {
        view = GetComponent<PhotonView>();
        if (golemType == GOLEM_TYPE.ICEGOLEM)
        {
            iceHitbox = transform.GetChild(2).gameObject;
            iceHitbox.SetActive(false);
        }
        originalSpeed = velocidadMovimiento;
    }
    private void Start()
    {
        view.TransferOwnership(view.ViewID);
        rb = GetComponent<Rigidbody>();
        attackHitbox = transform.GetChild(1).gameObject;
        attackHitbox.SetActive(false);
        anim = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        /*if (playerMounting != null)
        {
            isMounted = true;
        }
        else
        {
            isMounted = false;
        }*/

        if (view.IsMine)
        {
            if (isMounted)
            {
                //Salto
                estaEnSuelo = Physics.CheckSphere(chequeadorSuelo.position, radioChequeador, mascaraSuelo);

                if (Input.GetButtonDown("Jump") && estaEnSuelo)
                {
                    rb.AddForce(Vector3.up * poderSalto, ForceMode.Impulse);
                    Debug.Log(gameObject.name + "jumping");
                }

                //Caminar
                if (!canMove) return;
                float vX = Input.GetAxisRaw("Horizontal");
                float vZ = Input.GetAxisRaw("Vertical");
                Vector3 direccion = new Vector3(vX, 0f, vZ).normalized;

                if (golemType == GOLEM_TYPE.ICEGOLEM)
                {
                    rb.AddRelativeForce(direccion * velocidadMovimiento * Time.deltaTime, ForceMode.Impulse);
                    if(rb.velocity.magnitude > velocidadMovimiento * 1.2f)
                    {
                        rb.velocity = rb.velocity.normalized * velocidadMovimiento * 1.2f;
                    }
                    
                }
                else
                {
                    transform.Translate(direccion * velocidadMovimiento * Time.deltaTime);
                }
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0f, referenciaY.eulerAngles.y, 0f), suavizador);
                anim.SetFloat("Speed", direccion.magnitude);

                //Desmontado de Golem
                if (Input.GetKeyDown(KeyCode.LeftControl))
                {
                    view.RPC("DismountGolem", RpcTarget.All, (bool)false, (int)view.ViewID);
                }

                //Ataque
                if (Input.GetMouseButtonDown(0) && attackCooldown <= 0)
                {
                    //IMPORTANTE: La hitbox de ataque debe ser el segundo child del Golem, seguido del GroundCheck.
                    StartCoroutine("GolemAttackCoroutine");
                    anim.SetBool("Attack", true);
                    attackCooldown = 2f;
                    if(specialCooldown < 2)specialCooldown = 2f;
                }
                //Ataque
                if (Input.GetMouseButtonDown(1) && specialCooldown <= 0)
                {
                    GolemSpecial();
                }
            }
            if (!isMounted)
            {
                rb.velocity = new Vector3(0, rb.velocity.y, 0);
            }
        }
        
        //Cooldown de ataque, se restablece estando montado o no.
        if(attackCooldown > 0)
        {
            attackCooldown -= Time.deltaTime;
        }
        if (specialCooldown > 0)
        {
            specialCooldown -= Time.deltaTime;
        }
        if(mouse1Cooldown != null && mouse2Cooldown != null)
        {
            mouse1Cooldown.fillAmount = attackCooldown / 2f;
            mouse2Cooldown.fillAmount = specialCooldown / maxSpecialCooldown;
        }
        //Desmontado y regen.
        if (healthPoints <= 0)
        {
            if(isMounted)
            {
                view.RPC("DismountGolem",RpcTarget.All,(bool)false,(int)view.ViewID);
            }
            healthPoints = 0.1f;
            isRegenerating = true;
        }
        if(isRegenerating)
        {
            healthPoints += 10 * Time.deltaTime;
            if(healthPoints >= 100)
            {
                isRegenerating = false;
                healthPoints = 100;
            }
        }
        //Lifebar
        healthBar.fillAmount = healthPoints / 100f;
        if(hpBar != null) hpBar.fillAmount = healthPoints / 100f;

        //Raycast
        if(camara != null)
        Debug.DrawRay(transform.position + new Vector3(0, 1, 0), camara.forward * raycastDistance, Color.green);

        playerMounting.GetComponent<ThirdControllerV2>().nicknameText.transform.rotation = Quaternion.LookRotation(transform.position - GameObject.Find("Main Camera").GetComponent<Transform>().position);

        

    }

    //Le quita lo "floaty" al salto y da la posibilidad de hacer saltos cortos si se suelta el boton antes de llegar al peak de la subida.
    private void FixedUpdate()
    {
        if(view.IsMine)
        {
            if (rb.velocity.y < 0)
            {
                rb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
            }
            else if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
            {
                rb.velocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
            }
        }
    }
    [PunRPC]
    public void GolemHPLoss(float damage)
    {
        healthPoints -= damage;
    }
    //Desmonta al golem. El estado de montado es falso, el explorador vuelve a ser controlado, la camara se reorganiza, y el explorador es impulsado hacia atras del golem.
    [PunRPC]
    public void DismountGolem(bool mountedGolem, int golemID)
    {
        isMounted = mountedGolem;
        GolemController _golemController = PhotonView.Find(golemID).GetComponent<GolemController>();
        anim.SetFloat("Speed", 0);
        hpBar.fillAmount = 0;
        hpBar = null;
        mouse2Image.color = Color.black;
        mouse2Image = null;
        mouse1Cooldown = null;
        mouse2Cooldown = null;
        playerMounting.transform.parent = null;
        view.TransferOwnership(view.ViewID);
        playerMounting.GetComponent<CapsuleCollider>().enabled = true;
        playerMounting.GetComponent<ThirdControllerV2>().enabled = true;
        playerMounting.GetComponent<ThirdControllerV2>().hpBar.gameObject.SetActive(false);
        mouse1Image.sprite = playerMounting.GetComponent<ThirdControllerV2>().mountSprite;
        mouse1Image = null;
        playerMounting.GetComponent<ThirdControllerV2>().canRaycast = true;
        playerMounting.GetComponent<Rigidbody>().isKinematic = false;
        vCam.Follow = playerMounting.transform;
        vCam.LookAt = playerMounting.transform;
        vCam.m_Orbits[0].m_Height = 7.63f;
        vCam.m_Orbits[0].m_Radius = 4.4f;
        vCam.m_Orbits[1].m_Radius = 14.67f;
        vCam.m_Orbits[2].m_Height = .16f;
        vCam.m_Orbits[2].m_Radius = 4.54f;
        referenciaY = null;
        vCam = null;
        playerMounting.GetComponent<Rigidbody>().AddForce(playerMounting.transform.forward * -10, ForceMode.Impulse);
        playerMounting.GetComponentInChildren<Animator>().SetBool("Mounted", false);
        _golemController.playerMounting = null;
    }
    //ESPECIALES DE LOS GOLEM (CLIC DERECHO)
    void GolemSpecial()
    {
        switch (golemType)
        {
            //BOLA DE FUEGO
            case GOLEM_TYPE.FIREGOLEM:
                Debug.Log("firegolemSpecial");
                maxSpecialCooldown = 4f;
                specialCooldown = 4f;
                attackCooldown = 1.25f;
                GameObject goFireball = PhotonNetwork.Instantiate("Fireball", attackHitbox.transform.position, Quaternion.identity);
                goFireball.GetComponent<Rigidbody>().AddForce(transform.forward * 20, ForceMode.Impulse);
                goFireball.GetComponent<GolemFireball>().golemController = this;
                break;
            case GOLEM_TYPE.ROCKGOLEM:
                Debug.Log("ROCLgolemSpecial");
                if(Physics.Raycast(transform.position + new Vector3(0, 1, 0), camara.forward, out hit, raycastDistance, 1 << LayerMask.NameToLayer("Suelo")))
                {
                    GameObject goWall = PhotonNetwork.Instantiate("RockWall", hit.point, transform.rotation * Quaternion.Euler(-90,0,0));
                    specialCooldown = 10f;
                }
                break;
            case GOLEM_TYPE.ICEGOLEM:
                Debug.Log("ICEgolemSpecial");
                StartCoroutine("IceBreathCoroutine");
                specialCooldown = 10f;
                attackCooldown = 1.25f;
                //FindObjectOfType<AudioManager>().Play("Aliento Hielo");
                break;
        }
    }
    //FUNCION RPC QUE ENVIA LA CORRUTINA DE GOLPE,ANIMACION, ACTIVA/DESACTIVA HITBOX
    [PunRPC]
    void GolemAttacking(bool activeAttack)
    {
        attackHitbox.SetActive(activeAttack);
        if(activeAttack)
        {
            golpesfx.Play();
        }
    }
    IEnumerator GolemAttackCoroutine()
    {
        yield return new WaitForSeconds(0.7f);
        view.RPC("GolemAttacking", RpcTarget.All, (bool)true);
        anim.SetBool("Attack", false);
        yield return new WaitForSeconds(0.16f);
        yield return new WaitForFixedUpdate();
        view.RPC("GolemAttacking", RpcTarget.All, (bool)false);
        yield break;
    }
    //ALIENTO DE HIELO
    [PunRPC]
    void GolemIceBreath(bool activeBreath)
    {
        iceHitbox.SetActive(activeBreath);
    }
    IEnumerator IceBreathCoroutine()
    {
        view.RPC("GolemIceBreath", RpcTarget.All, (bool)true);
        yield return new WaitForSeconds(2.8f);
        view.RPC("GolemIceBreath", RpcTarget.All, (bool)false);
        yield break;
    }
    public IEnumerator Slowness(float slowMultiplier)
    {
        if(!isSlowed)
        {
            isSlowed = true;
            SkinnedMeshRenderer meshR = GetComponentInChildren<SkinnedMeshRenderer>();
            Material[] originalMats = meshR.materials;
            Material[] iceMats = new Material[originalMats.Length];
            for (int i = 0; i < meshR.materials.Length; i++)
            {
                iceMats[i] = iceMat;
            }
            meshR.materials = iceMats;
            Debug.Log("slowness activado");
            velocidadMovimiento = velocidadMovimiento * (1 - slowMultiplier);
            yield return new WaitForSeconds(2.5f);
            meshR.materials = originalMats;
            velocidadMovimiento = originalSpeed;
            isSlowed = false;
        }
        yield break;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(isMounted);
        }
        else if (stream.IsReading)
        {
            isMounted = (bool)stream.ReceiveNext();
        }
    }
}
