using KartGame.KartSystems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SockCollision : MonoBehaviour
{
    public IEnumerator OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("BodyPlayer")||other.CompareTag("BodyAI"))
        {
            Rigidbody otherRb = other.transform.parent.parent.GetComponent<Rigidbody>();
            Animator anim = other.transform.parent.parent.GetComponent<KartPlayerAnimator>().PlayerAnimator;
            anim.SetBool("Hit", true);
            otherRb.isKinematic = true;
            yield return new WaitForSeconds(1.5f);
            otherRb.isKinematic = false;
            anim.SetBool("Hit", false);
        }
        else if(other.CompareTag("Item"))
        {
            Destroy(other.gameObject);
        }
        yield break;
    }
    
}
