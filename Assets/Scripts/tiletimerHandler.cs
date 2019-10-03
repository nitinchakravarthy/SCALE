using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tiletimerHandler : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject tileTimer;

    void Start()
    {
        tileTimer = GameObject.FindGameObjectWithTag("tileTimer");
        //Debug.Log(tileTimer);
        //tileTimer.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void timerReset() {
        //Debug.Log("timer reset");
        tileTimer.GetComponent<tileTimer>().stopTimer();
        tileTimer.GetComponent<tileTimer>().restartTimer();
        //tileTimer.SetActive(true);
    }

    //public void setTimerinActive()
    //{
    //    Debug.Log("set timer inActive");
    //    tileTimer.SetActive(false);
    //}
}
