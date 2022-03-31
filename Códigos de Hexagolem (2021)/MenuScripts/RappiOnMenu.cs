using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

//------------------------------------------------------------------
// Script correspondiente a Rapi que se encuentra en el menú principal.
// Hace un baile aleatorio cada vez que se entra al menú principal, y tambien cambia de color dependiendo del ajuste del perfil.
//------------------------------------------------------------------
public class RappiOnMenu : MonoBehaviour
{
    private Color rapiColor;
    private new SkinnedMeshRenderer renderer;
    private Material[] mats;
    public Animator anim;
    public List<AudioClip> dances;
    public AudioSource sfxM;

    private void Awake()
    {
        rapiColor = SettingsManager.rapiColor;
    }

    private void Start()
    {
        anim = GetComponent<Animator>();
        renderer = GetComponentInChildren<SkinnedMeshRenderer>();
        mats = renderer.materials;
        StartCoroutine("Dance");
    }

    private void Update()
    {
        mats[2].color = SettingsManager.rapiColor;
        mats[4].color = SettingsManager.rapiColor;
        renderer.materials = mats;
    }
    IEnumerator Dance()
    {
        int random = Random.Range(1, 13);
        Debug.Log(random);
        yield return new WaitForSeconds(1.5f);
        anim.SetTrigger("dance" + random);
        if (dances[random] != null) sfxM.PlayOneShot(dances[random]);
        yield break;
    }
}
