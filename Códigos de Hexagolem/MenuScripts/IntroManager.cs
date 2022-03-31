using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//------------------------------------------------------------------
// Saltar intro al apretar espacio.
//------------------------------------------------------------------
public class IntroManager : MonoBehaviour
{

    private void Update()
    {
        if (Input.anyKeyDown)
        {
            CargarEscena();
        }
    }

    public void CargarEscena()
    {
        SceneManager.LoadScene(1);
    }
}
