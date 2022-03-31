using KartGame.KartSystems;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TImeCountDown : MonoBehaviour
{
    [Header("UI Elements")]
    public Image tennyson;
    public List<Sprite> tennysons;

    [Header("Audios")]
    public AudioSource audioManager;

    public AudioSource sfxManager;
    public AudioClip sound1, sound2;

    // Start is called before the first frame update
    void Start()
    {
        List<GameObject> kartList = GameObject.Find("GameManager").GetComponent<PositionManager>().karts;
        for (int i = 0; i < kartList.Count; i++)
        {
            kartList[i].GetComponent<ArcadeKart>().m_CanMove = false;
        }
        StartCoroutine("CounterDownToStart");
    }

    IEnumerator CounterDownToStart()
    {
        List<GameObject> kartList = GameObject.Find("GameManager").GetComponent<PositionManager>().karts;
        yield return new WaitForSeconds(3.06f);
        sfxManager.PlayOneShot(sound1);
        tennyson.enabled = true;
        tennyson.sprite = tennysons[0];
        yield return new WaitForSeconds(1f);
        sfxManager.PlayOneShot(sound1);
        tennyson.sprite = tennysons[1];
        yield return new WaitForSeconds(1f);
        sfxManager.PlayOneShot(sound2);
        tennyson.sprite = tennysons[2];

        for (int i = 0; i < kartList.Count; i++)
        {
            kartList[i].GetComponent<ArcadeKart>().m_CanMove = true;
        }
        GameObject.Find("GameManager").GetComponent<LapTimeManager>().timeRunning = true;
        yield return new WaitForSeconds(0.5f);

        audioManager.Play();

        yield return new WaitForSeconds(1f);
        tennyson.enabled = false;

        yield break;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
