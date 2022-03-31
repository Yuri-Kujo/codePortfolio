using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public AudioMixer audioMixer;

    public Dropdown resolucionDropDown;

    Resolution[] resoluciones;

    void Start()
    {
        resoluciones = Screen.resolutions;

        resolucionDropDown.ClearOptions();

        List<string> opciones = new List<string>();

        int resolucionActualIndex = 0;

        for (int i = 0; i < resoluciones.Length; i++)
        {
            string opcion = resoluciones[i].width + " x " + resoluciones[i].height;

            opciones.Add(opcion);

            if (resoluciones[i].width == Screen.currentResolution.width &&
                resoluciones[i].height == Screen.currentResolution.height)
            {
                resolucionActualIndex = i;
            }
        }

        resolucionDropDown.AddOptions(opciones);
        resolucionDropDown.value = resolucionActualIndex;
        resolucionDropDown.RefreshShownValue();
    }

    public void PonerResolucion(int resolucionIndex)
    {
        Resolution resolucion = resoluciones[resolucionIndex];

        Screen.SetResolution(resolucion.width, resolucion.height, Screen.fullScreen);
    }

    public void PonerVolumen(float volumen)
    {
        audioMixer.SetFloat("volumen", volumen);
    }

    public void PonerFullScreen(bool esFullScreen)
    {
        Screen.fullScreen = esFullScreen;
    }
}
