using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Cinemachine;
using UnityEngine.Audio;
using TMPro;

public class MenuManager : MonoBehaviour
{
    public GameObject[] panel;

    public bool isOnSplashScreen = true;
    public CinemachineVirtualCamera[] vCamera;
    public GameObject[] timeLine;
    public GameObject audioManager;
    public TextMeshProUGUI presionaCualquierTxt;
    public GameObject rappiOnMenu;

    // Start is called before the first frame update
    void Start()
    {

        audioManager.SetActive(true);
        switch (isOnSplashScreen)
        {
            case true:
                panel[0].SetActive(true);
                panel[1].SetActive(false);
                if(presionaCualquierTxt != null) presionaCualquierTxt.LeanAlphaTextMesh(1, 2).setLoopType(LeanTweenType.pingPong);
                break;
            case false:
                panel[0].SetActive(false);
                panel[1].SetActive(true);
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown && panel[0].activeInHierarchy)
        {
            panel[0].SetActive(false);
            panel[1].SetActive(true);
            isOnSplashScreen = false;

            vCamera[0].gameObject.SetActive(false);
            timeLine[0].SetActive(false);
            vCamera[1].gameObject.SetActive(true);

            rappiOnMenu.SetActive(true);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    public void ProfileSettingsCamera()
    {
        vCamera[1].gameObject.SetActive(false);
        vCamera[2].gameObject.SetActive(true);
    }

    public void MainMenuCamera()
    {
        vCamera[2].gameObject.SetActive(false);
        vCamera[1].gameObject.SetActive(true);
    }
}
