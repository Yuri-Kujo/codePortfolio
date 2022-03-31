using KartGame.KartSystems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntiKinematic : MonoBehaviour
{
    private float stunTime = 1.6f;
    private bool playerIsKinematic;
    private Rigidbody rb;
    private Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<KartPlayerAnimator>().PlayerAnimator;
    }

    // Update is called once per frame
    void Update()
    {
        playerIsKinematic = rb.isKinematic;

        if (playerIsKinematic)
        {
            stunTime -= Time.deltaTime;
            if(stunTime <= 0)
            {
                rb.isKinematic = false;
                playerIsKinematic = false;
                anim.SetBool("Hit", false);
                stunTime = 1.6f;
            }
        }
        else
        {
            stunTime = 1.6f;
        }
    }
}
