using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PositionLevel : MonoBehaviour {

	private GameObject field;
	private GameObject levelLabel;
	private GameObject scoreLabel;
	private GameObject scoreBackLabel;
	private GameObject livesLabel;
    private GameObject livesHeart;
    private GameObject hrvLabel;
    private GameObject timerLabel;
    //private GameObject tileTimerLabel;
    private GameObject pointsLabel;
    private GameObject pointsStar;
    private GameObject levelProgress;
    private GameObject brStateImage;

    public float scoreLabelBuffer;
	public float levelLabelBuffer;

    //public GameObject background;
    public GameObject audioSource;
	// Use this for initialization
	void Start () {
        //set up settings from the preferences 
        //Constants.playTime = PlayerPrefs.GetInt("playTime", Constants.playTime); //   180;
        //Constants.castingSpeed = PlayerPrefs.GetFloat("castingSpeed", Constants.castingSpeed);  //1; // 1 implies 1 second to cross the entire field
        //Constants.lives = PlayerPrefs.GetInt("lives", Constants.lives);   // 3;
        //Constants.rotatePercentile = PlayerPrefs.GetInt("tileRotation", Constants.rotatePercentile);   // 25;
        //Constants.tileTime = PlayerPrefs.GetInt("tiletime", Constants.tileTime);  // 10;
        //Constants.br1 = PlayerPrefs.GetInt("br1", Constants.br1);  // 6;
        //Constants.br2 = PlayerPrefs.GetInt("br2", Constants.br2);  // 7;
        //Constants.br3 = PlayerPrefs.GetInt("br3", Constants.br3);  // 24;
        //Constants.bs1 = PlayerPrefs.GetFloat("bs1", Constants.bs1); //3.0f;
        //Constants.bs2 = PlayerPrefs.GetFloat("bs2", Constants.bs2); //6.0f;
        //Constants.bs3 = PlayerPrefs.GetFloat("bs3", Constants.bs3); //12.0f; 

        field = GameObject.FindGameObjectWithTag ("game_field");
		levelLabel = GameObject.FindGameObjectWithTag ("level");
		scoreLabel = GameObject.FindGameObjectWithTag ("score");
		scoreBackLabel = GameObject.FindGameObjectWithTag ("score_back");
		livesLabel = GameObject.FindGameObjectWithTag ("lives");
        livesHeart = GameObject.FindGameObjectWithTag("heart");
        hrvLabel = GameObject.FindGameObjectWithTag ("hrv");
        timerLabel = GameObject.FindGameObjectWithTag("timer");
        pointsLabel = GameObject.FindGameObjectWithTag("points");
        pointsStar = GameObject.FindGameObjectWithTag("star");
        //tileTimerLabel = GameObject.FindGameObjectWithTag("tileTimer");
        levelProgress = GameObject.FindGameObjectWithTag("progress_slider");
        brStateImage = GameObject.FindGameObjectWithTag("brState");


        //Debug.Log("timer label");  
        //Debug.Log(pointsLabel);
        // scoreLabel and scorebacklabel are not needed now....
        // replaced it with a levelProgres progressbar
        scoreLabel.GetComponent<RectTransform> ().position = field.transform.position + Vector3.up * scoreLabelBuffer ;
        scoreBackLabel.GetComponent<RectTransform> ().position = field.transform.position + Vector3.up * scoreLabelBuffer;

        levelLabel.GetComponent<RectTransform> ().position = field.transform.position + Vector3.up * (levelLabelBuffer - 2.5f);

		livesLabel.GetComponent<RectTransform> ().position = field.transform.position + Vector3.up * (levelLabelBuffer + 0.5f) ;
        livesHeart.GetComponent<RectTransform>().position = field.transform.position + Vector3.up * (levelLabelBuffer + 0.5f) + Vector3.left * 3.0f;

        timerLabel.GetComponent<RectTransform>().position = field.transform.position + Vector3.up * (levelLabelBuffer + 1.5f);
        pointsLabel.GetComponent<RectTransform>().position = field.transform.position + Vector3.up * (levelLabelBuffer + 0.5f);
        pointsStar.GetComponent<RectTransform>().position = field.transform.position + Vector3.up * (levelLabelBuffer + 0.5f) + Vector3.right * 0.4f;

        //tileTimerLabel.GetComponent<RectTransform>().position = field.transform.position + Vector3.down * (scoreLabelBuffer - 0.5f);

        levelProgress.GetComponent<RectTransform>().position = field.transform.position + Vector3.up * scoreLabelBuffer;
        hrvLabel.GetComponent<RectTransform> ().position = field.transform.position + Vector3.up * (levelLabelBuffer + 0.5f) + Vector3.left* (0.75f);

       //background = GameObject.FindGameObjectWithTag("progress_slider");
        audioSource = GameObject.FindGameObjectWithTag("audio_source");
        
    }

    // Update is called once per frame
    void Update () {

    }
}
