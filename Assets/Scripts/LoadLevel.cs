using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadLevel : MonoBehaviour {
    private const string Message = "exit";
    public Image fadeImage;
	public Animator fadeAnim;
    
	// Use this for initialization
	public void onClick () {
		StartCoroutine (Fading());
	}

    public void playAgain() {
       // Debug.Log("Play again");

        Constants.GamePlays++;
        //DontDestroyOnLoad(GameObject.FindGameObjectWithTag("background"));
        //DontDestroyOnLoad(GameObject.FindGameObjectWithTag("audio_source"));
        StartCoroutine(Fading());
    }

    public void mainMenu()
    {
        Debug.Log("mainMenu");

       //DontDestroyOnLoad(GameObject.FindGameObjectWithTag("background"));
        StartCoroutine(FadingMainMenu());
    }

    public void openSettings()
    {
        //Debug.Log("opening settings");
        SceneManager.LoadScene("Scenes/Settings");
        //StartCoroutine(FadingSettings());
    }

    public static void exit() {
        // save the breathing rate list 
        //Constants.SaveListToFile(Constants.breathingRateLog, Constants.brLogFile);
        Debug.Log(Message);
        
        using (AndroidJavaClass cls_UnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            //Debug.Log("log 1 debug");
            using (AndroidJavaObject obj_Activity = cls_UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
            {
                //Debug.Log("log 2 debug");
                string s = getGameDetails(Constants.gamePlayVsRR) + string.Join(",", Constants.breathingRateLogString);
                s = s + "\n";
                s = s + "brAverage :";
                s = s + Constants.getBRAverage();
                obj_Activity.Call("openPostGameSurvey", s);
            }
        }
            //return 50.0;
        

        // call an intent from this part to open the next page here
    }

    public static string getGameDetails(Dictionary<int, PlayDetails> d) {
        string s = "{";
        foreach (KeyValuePair<int, PlayDetails> detail in d)
        {
            Debug.Log(detail.Key + detail.Value.ToString());
            s += detail.Key + ":" + detail.Value.ToString() +",";
        }
        s.TrimEnd(',');
        s += "}";
        Debug.Log("game Details string" + s);
        return s;
    }

	IEnumerator Fading(){
		fadeAnim.SetBool ("Fade", true);
		yield return new WaitUntil (() => fadeImage.color.a == 1);
        //Destroy(GameObject.Find("background"));
        SceneManager.LoadScene ("Scenes/Play Scene");
	}

    IEnumerator FadingMainMenu()
    {
        fadeAnim.SetBool("Fade", true);
        yield return new WaitUntil(() => fadeImage.color.a == 1);
        //Destroy(GameObject.Find("background"));
        SceneManager.LoadScene("Scenes/Menu");
    }

    IEnumerator FadingSettings()
    {
        Debug.Log("coroutine settings");
        fadeAnim.SetBool("Fade", true);
        yield return new WaitUntil(() => fadeImage.color.a == 1);
        Destroy(GameObject.Find("background"));
        SceneManager.LoadScene("Scenes/Settings");
    }

}
