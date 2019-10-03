using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading;
using UnityEngine.SceneManagement;

public class tileTimer : MonoBehaviour
{

    public int time;//Seconds Overall
    public Text tiletime; //UI Text Object
    public static tileTimer instance;
    public GameObject field;
    public Coroutine tileTimeCoroutine;
    public GameObject piecegen;

    // Start is called before the first frame update
    void Start()
    {
        time = Constants.tileTime;
        tileTimeCoroutine = StartCoroutine("LoseTileTime");
        Time.timeScale = 1; //Just making sure that the timeScale is right
        //field = GameObject.FindGameObjectWithTag("game_field");
        //piecegen = GameObject.FindGameObjectWithTag("piecegen");
        //Debug.Log("piecegen");
        //Debug.Log(piecegen);
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(time);
        //Debug.Log(field);
        tiletime.text = ("Time Left :" + time); //Showing the Score on the Canvas

        //if (time == 0)
        //{
        //    Debug.Log("came here");
        //    stopTimer();
        //    time = Constants.tileTime;
        //    tileTimeCoroutine = StartCoroutine("LoseTileTime");
        //}
        

    }

    public void stopTimer()
    {
        if (tileTimeCoroutine != null)
        {
            Debug.Log("stopping couroutine 1");
            StopCoroutine(tileTimeCoroutine);
        }
    }

    public void restartTimer() {
        time = Constants.tileTime;
        Debug.Log("restarting couroutine 1");
        tileTimeCoroutine = StartCoroutine("LoseTileTime");
    }

    //Simple Coroutine
    IEnumerator LoseTileTime()
    {
        while (time >= 0 )
        {
            if (time == 0)
            {
                
                yield return new WaitForSeconds(1);
                changePieceAndRemoveLife();
                stopTimer();
                restartTimer();
            }
            else {
                yield return new WaitForSeconds(1);
                time--;
            }
            
        }

    }

    public void changePieceAndRemoveLife() {
        Debug.Log("trying to change piece");
        GameObject currentpiece = piecegen.GetComponent<PieceGeneration>().getCurrentPiece();
        if (currentpiece != null)
        {
            Destroy(currentpiece);
        }
        piecegen.GetComponent<PieceGeneration>().GeneratePiece();
        field.GetComponent<FieldController>().lives--;

    }
}
