using KartGame.KartSystems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OilCollision : MonoBehaviour
{

    public IEnumerator OnTriggerEnter(Collider other)
    {
        if(other.transform.parent.parent.CompareTag("Player")||other.transform.parent.parent.CompareTag("AIPlayer"))
        {
            Rigidbody otherRb = other.transform.parent.parent.GetComponent<Rigidbody>();
            Animator anim = other.transform.parent.parent.GetComponent<KartPlayerAnimator>().PlayerAnimator;
            GetComponent<MeshRenderer>().enabled = false;
            GetComponent<CapsuleCollider>().enabled = false;
            anim.SetBool("Hit", true);
            otherRb.isKinematic = true;
            yield return new WaitForSeconds(1.5f);
            otherRb.isKinematic = false;
            anim.SetBool("Hit", false);
            Destroy(gameObject);
        }
        yield break;
    }
}
