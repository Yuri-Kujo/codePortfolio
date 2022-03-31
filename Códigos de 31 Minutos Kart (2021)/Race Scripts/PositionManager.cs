using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PositionManager : MonoBehaviour
{
    public List <GameObject> karts;
    public List<GameObject> ordenLlegada;
    public TextMeshProUGUI positionUI;
    public TextMeshProUGUI lapsUI;

    private CheckpointCheck cpc;
    private CheckpointList cpl;

    private void OnEnable()
    {

    }
    private void Start()
    {
        karts.Add(GameObject.FindGameObjectWithTag("Player"));
        karts.AddRange(GameObject.FindGameObjectsWithTag("AIPlayer"));
        cpc = GameObject.FindGameObjectWithTag("Player").GetComponent<CheckpointCheck>();
        cpl = GameObject.Find("GameManager").GetComponent<CheckpointList>();
    }

    void Update()
    {
        for(int i= 0; i < karts.Count; i++)
        {
            karts[i].GetComponent<CheckpointCheck>().kartPosition = karts.Count;
            for (int a = 0; a < karts.Count; a++)
            {
                if (karts[i].GetComponent<CheckpointCheck>().positionScore > karts[a].GetComponent<CheckpointCheck>().positionScore)
                {
                    karts[i].GetComponent<CheckpointCheck>().kartPosition--;
                }
            }
        }
        //ACTUALIZAR UI
        positionUI.text = karts[0].GetComponent<CheckpointCheck>().kartPosition.ToString();
        lapsUI.text = cpc.lapCounter + "/" + cpl.maxLaps;
    }
}
