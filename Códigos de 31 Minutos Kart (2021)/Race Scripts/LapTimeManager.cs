using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LapTimeManager : MonoBehaviour
{
    public bool timeRunning;
    public int minuteCount;
    public int secondCount;
    public float miliCount;
    public string miliDisplay;

    public TextMeshProUGUI minuteBox;
    public TextMeshProUGUI secondBox;
    public TextMeshProUGUI miliBox;

    // Update is called once per frame
    void Update()
    {
        if(timeRunning)
        {
            miliCount += Time.deltaTime * 10;
        }
        miliDisplay = miliCount.ToString("F0");
        miliBox.text = "" + miliDisplay;

        if(miliCount >= 9)
        {
            miliCount = 0;
            secondCount += 1;
        }

        if(secondCount <= 9)
        {
            secondBox.text = "0" + secondCount;
        }
        else
        {
            secondBox.text = "" + secondCount;
        }

        if(secondCount >= 60)
        {
            secondCount = 0;
            minuteCount += 1;
        }

        if(minuteCount <= 9)
        {
            minuteBox.text = "0" + minuteCount;
        }
        else
        {
            minuteBox.text = "" + minuteCount;
        }
    }
}
