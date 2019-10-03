using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetRemainingTime : MonoBehaviour
{
    public Text countdown; //UI Text Object

    // Start is called before the first frame update
    void Start()
    {
        

    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(Constants.timeLeft);
        countdown.text = "Time Remaining - " + Constants.timeLeft;
        if (Constants.timeLeft <= 1) {
            LoadLevel.exit();
        }
    }
}
