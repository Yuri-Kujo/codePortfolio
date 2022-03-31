using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ThirdController : MonoBehaviour
{
    public CharacterController control;
    public Transform camara;
    private CinemachineFreeLook vCam;
    public Transform chequeadorSuelo;

    public float velocidad = 5f;
    public float gravedad = -9.8f;

    public float altoS = 3;
    public Vector3 caida;
    public bool estaEnSuelo;
    public float sueltoD = 0.4f;
    public LayerMask mascaraSuelo;

    public float suavizador = 0.1f;
    public float suavizadorVel;

    private Rigidbody rb;

    public float raycastDistance;

    private void Start()
    {
        control = GetComponent<CharacterController>();
        rb = GetComponent<Rigidbody>();
        vCam = GameObject.Find("PlayerCamera").GetComponent<CinemachineFreeLook>();
    }

    private void Update()
    {
        //Salto
        estaEnSuelo = Physics.CheckSphere(chequeadorSuelo.position, sueltoD, mascaraSuelo);

        if (estaEnSuelo && caida.y < 0)
        {
            caida.y = -2f;
        }

        if (Input.GetButtonDown("Jump") && estaEnSuelo)
        {
            caida.y = Mathf.Sqrt(altoS * -2 * gravedad);
            Debug.Log(gameObject.name + "jumping");
        }

        //gravedad
        caida.y += gravedad * Time.deltaTime;
        control.Move(caida * Time.deltaTime);

        //Caminar
        float vX = Input.GetAxisRaw("Horizontal");
        float vZ = Input.GetAxisRaw("Vertical");
        Vector3 direccion = new Vector3(vX, 0f, vZ).normalized;

        if (direccion.magnitude >= 0.1f)
        {
            float anguloObj = Mathf.Atan2(direccion.x, direccion.z) * Mathf.Rad2Deg + camara.eulerAngles.y;
            float angulo = Mathf.SmoothDampAngle(transform.eulerAngles.y, anguloObj, ref suavizadorVel, suavizador);
            transform.rotation = Quaternion.Euler(0f, angulo, 0f);

            Vector3 movDir = Quaternion.Euler(0f, anguloObj, 0f) * Vector3.forward;

            control.Move(movDir.normalized * velocidad * Time.deltaTime);
        }

        //Raycast para subirse al golem

        RaycastHit hit;

        if(Physics.Raycast(transform.position, camara.forward, out hit, raycastDistance))
        {
            //Debug.Log(hit.collider.gameObject.name);
            if (Input.GetMouseButtonDown(0) && hit.transform.tag == "Golem")
            {
                hit.transform.gameObject.GetComponent<ThirdController>().enabled = true;

                vCam.Follow = hit.transform;
                vCam.LookAt = hit.transform;
                vCam.m_Orbits[0].m_Height = 15f;
                vCam.m_Orbits[0].m_Radius = 4.4f;
                vCam.m_Orbits[1].m_Radius = 23f;
                vCam.m_Orbits[2].m_Height = .75f;
                vCam.m_Orbits[2].m_Radius = 15f;
                transform.position = new Vector3(hit.transform.position.x, hit.transform.position.y + 5f, hit.transform.position.z);
                rb.isKinematic = true;
                transform.parent = hit.transform;
                this.enabled = false;
            }
        }
    }
}
