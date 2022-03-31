using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public int scene;
    public bool pressSpace;
    public GameObject mainMenu;

    //Cargar Escena

    public void LoadScene()
    {
        SceneManager.LoadScene(scene);
    }

    //salir del juego

    public void ExitGame()
    {
        Application.Quit();
    }

    //Pausa, continuar nivel y reiniciar

    public void Pausa()
    {
        Time.timeScale = 0f;
    }

    public void Continuar()
    {
        Time.timeScale = 1f;
    }

    public void Reiniciar()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void Update()
    {
        if(pressSpace && Input.GetKeyDown(KeyCode.Space))
        {
            gameObject.SetActive(false);
            mainMenu.SetActive(true);
        }
    }
}
