using KartGame.KartSystems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ActiveBalon : MonoBehaviour
{
    [SerializeField]
    private float minVelocity = 10f;
    public GameObject mesh;
    public AudioSource ballSFX;

    private IEnumerator Start()
    {
        GetComponent<BoxCollider>().enabled = false;
        yield return new WaitForFixedUpdate();
        GetComponent<BoxCollider>().enabled = true;
        yield break;
    }

    private void Update()
    {
        transform.Translate(gameObject.transform.forward * minVelocity * Time.deltaTime,Space.World);
    }

    private IEnumerator OnTriggerEnter(Collider other)
    {
        if(other.transform.parent.parent.CompareTag("Player") || other.transform.parent.parent.CompareTag("AIPlayer"))
        {
            Rigidbody otherRb = other.transform.parent.parent.GetComponent<Rigidbody>();
            Animator anim = other.transform.parent.parent.GetComponent<KartPlayerAnimator>().PlayerAnimator;
            anim.SetBool("Hit", true);
            otherRb.isKinematic = true;
            ballSFX.Play();
            yield return new WaitForSeconds(1.5f);
            otherRb.isKinematic = false;
            anim.SetBool("Hit", false);
        }
        if(other.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
        yield break;
    }
}

