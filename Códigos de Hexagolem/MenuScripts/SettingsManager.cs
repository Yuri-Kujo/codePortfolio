using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;

//------------------------------------------------------------------
// Permite personalizar el color del jugador.
// Edita la resolución, volumen, y la sensibilidad de la cámara.
//------------------------------------------------------------------
public class SettingsManager : MonoBehaviour
{
    public FlexibleColorPicker fcp;
    public static Color rapiColor = Color.red;
    public static float camSensitivity = 0.5f;

    [Header("Opciones")]
    public AudioMixer audioMixer;
    public TMP_Dropdown resolucionDropDown;
    Resolution[] resoluciones;

    private void Start()
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

    private void Update()
    {
        if(fcp != null) rapiColor = fcp.color;
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void PonerResolucion(int resolucionIndex)
    {
        Resolution resolucion = resoluciones[resolucionIndex];

        Screen.SetResolution(resolucion.width, resolucion.height, Screen.fullScreen);
    }

    public void PonerBGMVolumen(float volumen)
    {
        audioMixer.SetFloat("BGMvolumen", volumen);
    }

    public void PonerSFXVolumen(float volumen)
    {
        audioMixer.SetFloat("SFXvolumen", volumen);
    }
    public void PonerSensibilidad(float sensitivity)
    {
        camSensitivity = sensitivity;
    }

    public void PonerFullScreen(bool esFullScreen)
    {
        Screen.fullScreen = esFullScreen;
    }
}
