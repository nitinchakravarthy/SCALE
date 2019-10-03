using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading;
using UnityEngine.SceneManagement;

public class Countdown : MonoBehaviour
{
    public int timeLeft;//Seconds Overall
    public Text countdown; //UI Text Object
    public static Countdown instance;


    public GameObject field;
        void Start()
    {
        timeLeft = Constants.timeLeft;
        Constants.timeCoroutine = StartCoroutine("LoseTime");
        Time.timeScale = 1; //Just making sure that the timeScale is right
        field = GameObject.FindGameObjectWithTag("game_field");
    }
    
    void Update()
    {   
     
        if (Constants.timeLeft <= 0)
        {
            field.GetComponent<FieldController>().setGameSummary();
            //SceneManager.LoadScene("Scenes/Exit Scene");
            stopTimer();
            LoadLevel.exit();
        }
        else {
            //Debug.Log(Constants.timeLeft);
            //Debug.Log(field);
            countdown.text = ("" + timeLeft); //Showing the Score on the Canvas
        }
    }
        //return 50.0;


        // call an intent from this part to open the next page here


    public void stopTimer() {
        if (Constants.timeCoroutine != null) {
            Debug.Log("stopping couroutine 1");
            StopCoroutine(Constants.timeCoroutine);
        }
    }
    //Simple Coroutine
    IEnumerator LoseTime()
    {
        while (timeLeft > 0 && true)
        {
            yield return new WaitForSeconds(1);
            timeLeft--;
            Constants.timeLeft = timeLeft;
        }
    }
}