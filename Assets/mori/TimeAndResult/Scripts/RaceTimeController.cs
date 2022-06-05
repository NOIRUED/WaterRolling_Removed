using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class RaceTimeController : MonoBehaviour
{
    [SerializeField] Text timeText;
    public int minutes = 0;
    public float seconds = 0.0f;
    bool isInRace = false;

    // Start is called before the first frame update
    void Start()
    {
        //StartRace();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!isInRace) return;
        
        seconds += Time.deltaTime;
        if (seconds >= 60.0f)
        {
            minutes++;
            seconds -= 60.0f;
        }
        timeText.text =  String.Format("{0:00}",minutes)+":"+String.Format("{0:00.00}",seconds);
        
    }

    public void StartRace()
    {
        isInRace = true;
    }

    public void EndRace()
    {
        isInRace = false;

    }

}
