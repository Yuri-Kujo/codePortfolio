using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Photon.Pun;
using UnityEngine.UI;

public class ThirdControllerV2 : MonoBehaviour, IPunObservable
{
    public Transform camara;
    private CinemachineFreeLook vCam;
    public Transform referenciaY;
    private Rigidbody rb;
    private Animator anim;
    public GameObject minimapRef;
    public GameObject minimapCamRef;

    [Header("Referencias UI")]
    public Image hpBar;
    public Image mouse1Image;
    private Image mouse1Cooldown;
    public Image mouse2Image;
    private Image mouse2Cooldown;
    public Sprite mountSprite;

    [Header("Estados online")]
    public bool isLost;
    public bool isWin;

    //Photon
    public TMPro.TextMeshProUGUI nicknameText;
    PhotonView view;

    [Header("Movimiento Terrestre")]
    public float velocidadMovimiento = 5f;
    private bool isSlowed;
    private float originalSpeed;
    public float suavizador = 0.1f;
    public float suavizadorVel;
    public bool hitStun = false;
    public float sprintMultiplier = 1.3f;
    public float sprintCooldown = 6f;
    private float sprintCooldownTimer = 6f;
    private bool canSprint;
    private GameObject vfxRunDust;
    public Image sprintBar;
    public bool canMove = true;

    [Header("Salto y Detección de Suelo")]
    public float poderSalto = 3;
    public bool estaEnSuelo;
    public Transform chequeadorSuelo;
    public float radioChequeador = 0.4f;
    public LayerMask mascaraSuelo;

    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;

    [Header("Detección de objetos (Raycast)")]
    public bool canRaycast = true;
    public float raycastDistance;
    private GolemController golemController;
    RaycastHit hit;

    [Header("Materiales")]
    public Material iceMat;
    private Color rapiColor;

    private void Awake()
    {
        view = GetComponent<PhotonView>();
        originalSpeed = velocidadMovimiento;
        rapiColor = SettingsManager.rapiColor;
        hpBar.fillAmount = 0;
        hpBar.gameObject.SetActive(false);
    }

    private void Start()
    {
        //photon
        if (!view.IsMine)
        {
            Destroy(GetComponentInChildren<CinemachineFreeLook>().gameObject);
            Destroy(referenciaY.gameObject);
            Destroy(minimapRef);
            Destroy(minimapCamRef);
        }
        if(view.IsMine)
        {
            view.RPC("SetColors", RpcTarget.AllBuffered, (int)view.ViewID, (float)rapiColor.r, (float)rapiColor.g, (float)rapiColor.b);
            view.RPC("NicknameUI", RpcTarget.AllBuffered, (string)PhotonNetwork.NickName, (int)view.ViewID);
        }
        rb = GetComponent<Rigidbody>();
        mouse1Cooldown = mouse1Image.transform.GetChild(1).GetComponent<Image>();
        mouse2Cooldown = mouse2Image.transform.GetChild(1).GetComponent<Image>();
        vCam = GetComponentInChildren<CinemachineFreeLook>();
        camara = GameObject.Find("Main Camera").GetComponent<Transform>();
        referenciaY = GetComponentInChildren<ReferenciaY>().transform;
        anim = GetComponentInChildren<Animator>();
        vfxRunDust = GameObject.Find("vfxRunDust");
    }

    private void Update()
    {
        //photon comprueba que es el componente propio
        if (view.IsMine)
        {
            //Salto
            estaEnSuelo = Physics.CheckSphere(chequeadorSuelo.position, radioChequeador, mascaraSuelo);
            Debug.DrawRay(chequeadorSuelo.position, Vector3.down * radioChequeador, Color.yellow);
            anim.SetBool("isGrounded", estaEnSuelo);

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
            //El timer de cooldown se queda entre los valores 0 y el cooldown del sprint.
            sprintCooldownTimer = Mathf.Clamp(sprintCooldownTimer, 0f, sprintCooldown);
            if (direccion.magnitude >= 0.1f)
            {
                float anguloObj = Mathf.Atan2(direccion.x, direccion.z) * Mathf.Rad2Deg + camara.eulerAngles.y;
                float angulo = Mathf.SmoothDampAngle(transform.eulerAngles.y, anguloObj, ref suavizadorVel, suavizador);
                transform.rotation = Quaternion.Euler(0f, angulo, 0f);
                Vector3 movDir = Quaternion.Euler(0f, referenciaY.rotation.y, 0f) * Vector3.forward;

                //Si no está stuneado puede moverse, al apretar shift corres por unos segundos
                if(!hitStun)
                {
                    if(Input.GetKey(KeyCode.LeftShift) && canSprint)
                    {
                        transform.Translate(movDir.normalized * velocidadMovimiento * sprintMultiplier * Time.deltaTime);
                        sprintCooldownTimer -= Time.deltaTime*2;
                        vCam.m_Orbits[1].m_Radius = 16f;
                        if(estaEnSuelo)
                        {
                            vfxRunDust.SetActive(true);
                        }
                        else
                        {
                            vfxRunDust.SetActive(false);
                        }
                        anim.SetFloat("Sprint", 1.3f);
                    }
                    else
                    {
                        transform.Translate(movDir.normalized * velocidadMovimiento * Time.deltaTime);
                        vCam.m_Orbits[1].m_Radius = 14f;
                        vfxRunDust.SetActive(false);
                        anim.SetFloat("Sprint", .75f);
                    }
                }
            }
            //Sprint de Rappi
            if(Input.GetKeyUp(KeyCode.LeftShift) || sprintCooldownTimer <= 0)
            {
                canSprint = false;
            }
            if(sprintCooldownTimer < sprintCooldown && !canSprint)
            {
                sprintBar.color = new Color32(100, 100, 100,255);
                sprintCooldownTimer += Time.deltaTime;
            }
            if(sprintCooldownTimer >= sprintCooldown)
            {
                sprintCooldownTimer = 6f;
                sprintBar.color = Color.white;
                canSprint = true;
            }
            //Llenado de la barra de stamina en la UI
            sprintBar.fillAmount = sprintCooldownTimer / sprintCooldown;
            //Parametro que se encarga de la animacion Idle-Correr.
            anim.SetFloat("Speed", direccion.magnitude);

            //Raycast para subirse al golem
            if(canRaycast) mouse1Cooldown.fillAmount = 1;
            Debug.DrawRay(transform.position + new Vector3(0, 1, 0), camara.forward * raycastDistance, Color.green);
            if (Physics.Raycast(transform.position + new Vector3(0,1,0), camara.forward, out hit, raycastDistance) && hit.transform.tag == "Golem")
            {
                if (!hit.transform.GetComponent<GolemController>().isMounted && !hit.transform.GetComponent<GolemController>().isRegenerating && canRaycast && !hitStun)
                {
                    mouse1Cooldown.fillAmount = 0;
                    if(Input.GetMouseButtonDown(0))
                    {
                        canRaycast = false;
                        GetComponent<CapsuleCollider>().enabled = false;
                        golemController = hit.transform.gameObject.GetComponent<GolemController>();
                        golemController.view.RequestOwnership();
                        this.rb.isKinematic = true;
                        view.RPC("MountGolem", RpcTarget.All, (bool)true, view.ViewID, golemController.view.ViewID);
                    }
                }
            }
        }
        nicknameText.transform.rotation = Quaternion.LookRotation(transform.position - GameObject.Find("Main Camera").GetComponent<Transform>().position);
    }

    //Le quita lo "floaty" al salto y da la posibilidad de hacer saltos cortos si se suelta el boton antes de llegar al peak de la subida.
    private void FixedUpdate()
    {
        if (view.IsMine)
        {
            if (rb.velocity.y < 0)
            {
                rb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
            }
            else if (rb.velocity.y > 0 && !Input.GetButton("Jump") && !hitStun)
            {
                rb.velocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
            }
            if (hitStun)
            {
                hitStun = !estaEnSuelo;
            }
        }
    }

    //Desactiva el movimiento y collider del explorador, reorganiza la camara, activa el movimiento del golem.
    [PunRPC]
    void MountGolem(bool mountedGolem, int pvID, int golemID)
    {
        hpBar.gameObject.SetActive(true);
        vfxRunDust.SetActive(false);
        anim.SetBool("Mounted", true);
        GolemController golemRController = PhotonView.Find(golemID).GetComponent<GolemController>();
        golemRController.playerMounting = PhotonView.Find(pvID);
        golemRController.isMounted = mountedGolem;
        golemController.vCam = vCam;
        golemController.referenciaY = referenciaY;
        vCam.Follow = hit.transform;
        vCam.LookAt = hit.transform;
        vCam.m_Orbits[0].m_Height = 20f;
        vCam.m_Orbits[0].m_Radius = 15f;
        vCam.m_Orbits[1].m_Radius = 30f;
        vCam.m_Orbits[2].m_Height = -4.5f;
        vCam.m_Orbits[2].m_Radius = 15f;
        mouse1Image.sprite = golemController.attackSprite;
        mouse2Image.sprite = golemController.specialSprite;
        golemController.hpBar = hpBar;
        golemController.mouse1Image = mouse1Image;
        mouse2Image.color = Color.white;
        golemController.mouse2Image = mouse2Image;
        golemController.mouse1Cooldown = mouse1Cooldown;
        golemController.mouse2Cooldown = mouse2Cooldown;
        transform.position = new Vector3(hit.transform.position.x, hit.transform.position.y + 4.2f, hit.transform.position.z);
        transform.rotation = hit.transform.rotation;
        transform.parent = hit.transform;
        this.enabled = false;
    }
    //Ralentizacion/Stun de Rappi
    public IEnumerator Slowness(float slowMultiplier)
    {
        if (!isSlowed)
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

    //Setea los colores segun fueron ajustados en el menu principal
    [PunRPC]
    void SetColors(int playerID, float r, float g, float b)
    {
        GameObject obj = PhotonView.Find(playerID).gameObject;
        SkinnedMeshRenderer renderer = obj.GetComponentInChildren<SkinnedMeshRenderer>();
        Material[] mats = renderer.materials;
        mats[2].color = new Color(r, g, b);
        mats[4].color = new Color(r, g, b);
        renderer.materials = mats;
        Debug.Log(gameObject.name + "(ID: " + view.ViewID + ") detectó a" + obj.name + "(ID: " + obj.GetComponent<ThirdControllerV2>().view.ViewID +")");
    }

    [PunRPC]
    void NicknameUI(string name, int playerID)
    {
        GameObject obj = PhotonView.Find(playerID).gameObject;
        obj.GetComponent<ThirdControllerV2>().nicknameText.text = name;
    }

    //ESTA COSA ENVIA Y RECIBE INFORMACION BASICA (ints, bools, strings, floats) QUE QUEREMOS SINCRONIZAR MANUALMENTE. NO SE PUEDEN SINCRONIZAR GAMEOBJECTS ENTEROS PORQUE SI NO LA VIDA SERIA MUY BONITA Y NO ES ASÍ.
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(rb.isKinematic);
        }
        else if (stream.IsReading)
        {
            rb.isKinematic = (bool)stream.ReceiveNext();
        }
    }
}
