using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapIcon : MonoBehaviour
{
    public string playerName; 
    public string botName;
    private CinemachineVirtualCamera vCamera;
    // Start is called before the first frame update
    void Start()
    {
        vCamera = GetComponent<CinemachineVirtualCamera>();
        if(GameObject.Find(playerName+"(Clone)"))
        {
            vCamera.Follow = GameObject.Find(playerName+"(Clone)").transform;
        }
        else
        {
            vCamera.Follow = GameObject.Find(botName+"(Clone)").transform;
        }
    }
}
