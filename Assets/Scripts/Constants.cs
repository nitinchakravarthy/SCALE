using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public static class Constants 
{
    public static int GamePlays = 1;
    public static Dictionary<int, PlayDetails> gamePlayVsRR = new Dictionary<int, PlayDetails>();
    public static List<double> breathingRateLog = new List<double>();
    public static List<String> breathingRateLogString = new List<String>();
    public static string brLogFile = "brLog";
    // settings
    public static int playTime = 300;
    public static float castingSpeed = 1; // 1 implies 1 second to cross the entire field
    public static int lives =3;
    public static int rotatePercentile =50;
    public static int tileTime = 10;
    public static int br1 = 6;
    public static int br2 =  7;
    public static int br3 = 24;
    public static int br4 = 24;
    public static float bs0 = 1.5f;
    public static float bs1 = 1.5f;
    public static float bs2 = 0.75f;
    public static float bs3 = 0.3f;
    public static float bs4 = 0.25f;


    //public static int playTime = PlayerPrefs.GetInt("playTime", 180); //   180;
    //public static float castingSpeed = PlayerPrefs.GetFloat("castingSpeed", 1);  //1; // 1 implies 1 second to cross the entire field
    //public static int lives = PlayerPrefs.GetInt("lives", 3);   // 3;
    //public static int rotatePercentile = PlayerPrefs.GetInt("tileRotation", 25);   // 25;
    //public static int tileTime = PlayerPrefs.GetInt("tiletime", 10);  // 10;
    //public static int br1 = PlayerPrefs.GetInt("br1", 6);  // 6;
    //public static int br2 = PlayerPrefs.GetInt("br2", 7);  // 7;
    //public static int br3 = PlayerPrefs.GetInt("br3", 24);  // 24;
    //public static float bs1 = PlayerPrefs.GetFloat("bs1", 3.0f); //3.0f;
    //public static float bs2 = PlayerPrefs.GetFloat("bs2", 6.0f); //6.0f;
    //public static float bs3 = PlayerPrefs.GetFloat("bs3", 12.0f); //12.0f; 

    public static float startRR;
    public static float endRR;
    public static int levelsFinished;
    public static int timeLeft = playTime;
    public static Coroutine timeCoroutine;
    public static int points = 0;
    public static float baseSpeed = 6.5f;



    public static double getBRAverage() {
        int length = breathingRateLog.Count;
        double sum = 0;
        foreach (double br in breathingRateLog) {
            sum += br;
        }
        return sum / (double)length;
    }
    public static Text AddTextToCanvas(string textString, GameObject canvasGameObject)
    {
        Debug.Log("adding timer text");
        Text text = canvasGameObject.AddComponent<Text>();
        text.text = textString;
        text.fontSize = 30;
        Font ArialFont = (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");
        text.font = ArialFont;
        text.material = ArialFont.material;
        return text;
    }

    public static bool percentile(int percent) {

        int num = UnityEngine.Random.Range(1, 100);
        //Debug.Log("Random Number is " + num);
        if (num <= percent) {
            return true;
        }
        return false;
    }

    [System.Serializable]
    public struct SaveStuff
    {
        public List<float> brs;
    }

    public static void SaveListToFile(List<float> temp, string fileName)
    {
        var stream = new System.IO.StreamWriter(fileName);
        foreach (float item in temp) {
            stream.Write(item);
            stream.Write("\n");
        }
        stream.Close();

    }

    public static void clearPrefs() {
        PlayerPrefs.DeleteAll();
    }
}


