using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using TMPro;

public class PlayerUI : MonoBehaviour
{
    [Header("Variables")]
    public bool isInMenu;
    public int placeinLost;
    [Header("Componentes")]
    public GameObject pauseInGame;
    public GameObject lostPanel;
    public GameObject winPanel;
    public UIElementsManager uiManager;
    public CinemachineFreeLook playerCamera;
    public TextMeshProUGUI lostPlaceText, lostPlaceText2;

    [Header("Scripts")]
    public ThirdControllerV2 pyController;
    public NetworkManager netManager;
    public GameManager gm;

    // Start is called before the first frame update
    void Start()
    {
        playerCamera = GetComponentInChildren<CinemachineFreeLook>();
        netManager = GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<NetworkManager>();
        pyController = GetComponentInParent<ThirdControllerV2>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (pyController.isLost)
        {
            case true:
                lostPanel.SetActive(true);
                isInMenu = true;
                break;
            case false:
                lostPanel.SetActive(false);
                break;
        }

        switch (pyController.isWin)
        {
            case true:
                winPanel.SetActive(true);
                isInMenu = true;
                break;
            case false:
                winPanel.SetActive(false);
                break;
        }

        switch (isInMenu)
        {
            case true:
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                playerCamera.m_YAxis.m_MaxSpeed = 0;
                playerCamera.m_XAxis.m_MaxSpeed = 0;
                pyController.canMove = false;
                break;

            case false:
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                playerCamera.m_YAxis.m_MaxSpeed = SettingsManager.camSensitivity * 10 + 2.5f;
                playerCamera.m_XAxis.m_MaxSpeed = SettingsManager.camSensitivity * 1000 + 250;
                pyController.canMove = true;
                break;
        }
        PressMenu();
    }

    public void PressMenu()
    {
        if (isInMenu) return;
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isInMenu = true;
            pauseInGame.SetActive(true);
            uiManager.PopUpOpen(pauseInGame);
        }
    }

    public void ExitMatch()
    {
        netManager.DesconectarJugador();
        gm.playersInGame.Remove(this.gameObject);
        gm.playersLost.Remove(this.gameObject);
        Time.timeScale = 1f;
    }

    public void ChangeIsMenu()
    {
        isInMenu = false;
    }

    public void OnSpecterMode()
    {
        pyController.isLost = false;
        playerCamera.gameObject.SetActive(false);
    }
}
