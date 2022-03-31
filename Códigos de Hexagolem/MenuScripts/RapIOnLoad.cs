using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//------------------------------------------------------------------
// Script utilizado para que Rapi corriera en la pantalla de carga.
//------------------------------------------------------------------
public class RapIOnLoad : MonoBehaviour
{
    public Transform rapiRunOnLoad;
    public AnimationCurve rapiCurve;
    public LeanTweenType easyType;
    // Start is called before the first frame update
    void Start()
    {
        rapiRunOnLoad = GetComponent<Transform>();
        rapiRunOnLoad.LeanMoveLocalX(-947.14f, 2.5f).setLoopClamp().setEase(rapiCurve);
    }
}
