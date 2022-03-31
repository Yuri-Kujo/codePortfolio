using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SockActive : MonoBehaviour
{
    public float spinSpeed;
    private AudioSource bgm;
    // Start is called before the first frame update
    void Start()
    {
        bgm = GameObject.Find("AudioManager").GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(transform.up * spinSpeed * Time.deltaTime,Space.Self);
    }
    public IEnumerator Invincibility()
    {
        yield return new WaitForSeconds(9f);
        bgm.UnPause();
        gameObject.SetActive(false);
        yield break;
    }
}
