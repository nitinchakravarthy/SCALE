using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class BiofeedbackControl : MonoBehaviour {



    //breathing rate controll
    public static String gameSummary;
    public float br;
    public static Boolean gameAdaptation;
    public Boolean showBR;
    public string toastString;
    private float targetBr;
    public float ballSpeed;
    //private float castingSpeedDelta;
    private GameObject ball;
    public Sprite green;
    public Sprite red;
    //private float baseCastingSpeed;
    // Use this for initialization
    void Start() {
        br = 0.0f;
        targetBr = 6.0f;
        //castingSpeedDelta = 0.02f;
        //baseCastingSpeed = GetComponent<FieldController>().castingSpeed;
        //baseCastingSpeed = 0.1f;
        //Debug.Log("BiofeedbackController start");
        br = 6;
        ballSpeed = 5;
        //gameAdaptation = GetGameAdaptation();
        
    }

    // Update is called once per frame
    void Update() {
        Debug.Log("BioFeedback Control update");

        // GetComponent<FieldController>().castingSpeed = baseCastingSpeed;
        br = GetCurrentBR();
        GameObject.FindGameObjectWithTag("hrv").GetComponent<Text>().text = "" + (float)br;
        //Debug.Log(GameObject.FindGameObjectWithTag("brState"));
        
        if (getIsBRdecreasing())
        {
            GameObject.FindGameObjectWithTag("brState").GetComponent<SpriteRenderer>().sprite = green; 
        }
        else {
            GameObject.FindGameObjectWithTag("brState").GetComponent<SpriteRenderer>().sprite = red;
        }
        /**
        if (gameAdaptation)
        {
            //ballSpeed = 
            if (br <= 6)
            {
                // Debug.Log("BioFeedback Control update br < 6");

                //GetComponent<FieldController>().castingSpeed = 0.1f;
                //Debug.Log("BioFeedback ball speed 1= " );
            }
            else if (br > 6 && br <= 12)
            {
                //Debug.Log("BioFeedback Control update 6 < br < 24");
                // GetComponent<FieldController>().castingSpeed = 0.15f;
                //GetComponent<FieldController>().castingSpeed = BR612(br);
                //Debug.Log("BioFeedback ball speed 2= " );
            }
            else if (br > 12 && br < 24)
            {
                //GetComponent<FieldController>().castingSpeed = BR1224(br);
                //Debug.Log("BioFeedback ball speed 3= " );
            }
            else
            {
                //Debug.Log("BioFeedback Control update br > 24");
                //GetComponent<FieldController>().castingSpeed = 0.01f;
                //Debug.Log("BioFeedback ball speed 4= " );

            }
        }
        else {
            //GetComponent<FieldController>().castingSpeed = 0.07f;
        }
        **/
    }



    public float GetCurrentBR()
    {
        using (AndroidJavaClass cls_UnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            using (AndroidJavaObject obj_Activity = cls_UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
            {
                return (float)obj_Activity.Call<double>("getCurrentBR");
            }
        }
        //return 50.0;
    }

    public string getTimeStamp()
    {
        using (AndroidJavaClass cls_UnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            using (AndroidJavaObject obj_Activity = cls_UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
            {
                return obj_Activity.Call<string>("getTimeStamp");
            }
        }
        // return "50.0";
    }

    public Boolean getIsBRdecreasing()
    {
        using (AndroidJavaClass cls_UnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            using (AndroidJavaObject obj_Activity = cls_UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
            {
                return obj_Activity.Call<Boolean>("getIsBRdecreasing");
            }
        }
        // return "50.0";
    }


    Boolean GetGameAdaptation()
    {
        using (AndroidJavaClass cls_UnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            using (AndroidJavaObject obj_Activity = cls_UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
            {
                return obj_Activity.Call<Boolean>("getGameBioFeedBack");
            }
        }
    }

	public void Save(){
		BinaryFormatter bf = new BinaryFormatter ();
		FileStream file;
		Data data;
		if (File.Exists (Application.persistentDataPath + "/gameData.dat")) {
			file = File.Open(Application.persistentDataPath + "/gameData.dat", FileMode.Open);
			data = (Data)bf.Deserialize (file);
			file.Close ();
		} else {
			data = new Data ();
		}
		file = File.Create (Application.persistentDataPath + "/gameData.dat");

		bf.Serialize (file, data);
		file.Close ();
	}

	public void Load(){
		if (File.Exists (Application.persistentDataPath + "/gameData.dat")) {
			BinaryFormatter bf = new BinaryFormatter ();
			FileStream file = File.Open (Application.persistentDataPath + "/gameData.dat", FileMode.Open);
			Data data = (Data)bf.Deserialize (file);
			file.Close ();

		}
		
	}

	void OnApplicationPause(bool pauseStatus){
		Save ();
	}

	void OnApplicationQuit(){
		Save ();
	}
}

[Serializable]
class Data {
	public float minHRVRMSSD;
	public float maxHRVRMSSD;
	public float minHRVSDNN;
	public float maxHRVSDNN;
}
