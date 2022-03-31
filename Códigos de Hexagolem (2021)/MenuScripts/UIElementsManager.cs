using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIElementsManager : MonoBehaviour
{
    public void PopUpOpen(GameObject gameObject)
    {
        LeanTween.move(gameObject.GetComponent<RectTransform>(), new Vector2(0, 0), 0.5f);
    }

    public void PopUpClose(GameObject gameObject)
    {
        StartCoroutine(ClosePopUp(gameObject));
    }

    IEnumerator ClosePopUp(GameObject gameObject)
    {
        LeanTween.move(gameObject.GetComponent<RectTransform>(), new Vector2(0, -1084), 0.5f);
        yield return new WaitForSeconds(0.5f);
        gameObject.SetActive(false);
        yield return null;
    }

    public void ProfileSettingsPopUp(GameObject gameObject)
    {
        LeanTween.move(gameObject.GetComponent<RectTransform>(), new Vector2(-300, 0), 0.5f);
    }

    public void ProfileSettingsPopClose(GameObject gameObject)
    {
        StartCoroutine(ProfileSettingsClose(gameObject));
    }

    IEnumerator ProfileSettingsClose(GameObject gameObject)
    {
        LeanTween.move(gameObject.GetComponent<RectTransform>(), new Vector2(-300, -1084), 0.5f);
        yield return new WaitForSeconds(0.5f);
        gameObject.SetActive(false);
        yield return null;
    }
}
