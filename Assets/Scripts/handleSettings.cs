using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class handleSettings : MonoBehaviour
{
    GameObject playTime_dd;
    GameObject linecastSpeed_inp;
    GameObject lives_dd;
    GameObject tilerotPecent_dd;
    GameObject tiletime_inp;
    GameObject br1_inp;
    GameObject br2_inp;
    GameObject br3_inp;
    GameObject bs1_inp;
    GameObject bs2_inp;
    GameObject bs3_inp;
    GameObject applyChanges;

    // Start is called before the first frame update
    void Start()
    {
        playTime_dd = GameObject.FindGameObjectWithTag("pt_dd");
        linecastSpeed_inp = GameObject.FindGameObjectWithTag("cast_inp");
        lives_dd = GameObject.FindGameObjectWithTag("lives_dd");
        tilerotPecent_dd = GameObject.FindGameObjectWithTag("rotation_dd");
        tiletime_inp = GameObject.FindGameObjectWithTag("tiletime_inp");
        br1_inp = GameObject.FindGameObjectWithTag("br1_inp");
        br2_inp = GameObject.FindGameObjectWithTag("br2_inp"); 
        br3_inp = GameObject.FindGameObjectWithTag("br3_inp"); 
        bs1_inp = GameObject.FindGameObjectWithTag("bs1_inp");
        bs2_inp = GameObject.FindGameObjectWithTag("bs2_inp");
        bs3_inp = GameObject.FindGameObjectWithTag("bs3_inp");
        applyChanges = GameObject.FindGameObjectWithTag("applyChanges");
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void savePlayTime(int value) {
        Debug.Log(value);
        
        Constants.playTime = (value+ 3) * 60;
        PlayerPrefs.SetFloat("playTime", Constants.playTime);

    }

    public void saveLives(int value)
    {
        Debug.Log(value);

        Constants.lives = (value + 3);
        PlayerPrefs.SetInt("lives", Constants.lives);

    }

    public void saveTileRotationPercent(int value)
    {
        Debug.Log(value);

        Constants.rotatePercentile = (value + 4) * 5;
        PlayerPrefs.SetInt("tileRotation", Constants.rotatePercentile);

    }

    public void saveCastingSpeed(string value)
    {
        Debug.Log(value);
        float castSpeed = float.Parse(value);
        Constants.castingSpeed = castSpeed;
        PlayerPrefs.SetFloat("castingSpeed", Constants.castingSpeed);

    }

    public void saveTileTime(string value)
    {
        Debug.Log(value);
        int tileTime = int.Parse(value);
        Constants.tileTime = tileTime;
        PlayerPrefs.SetInt("tiletime", Constants.tileTime);

    }
    public void savebr1(string value)
    {
        Debug.Log(value);
        int br1 = int.Parse(value);
        Constants.br1 = br1;
        PlayerPrefs.SetInt("br1", Constants.br1);

    }
    public void savebr2(string value)
    {
        Debug.Log(value);
        int br2 = int.Parse(value);
        Constants.br2 = br2;
        PlayerPrefs.SetInt("br2", Constants.br2);

    }
    public void savebr3(string value)
    {
        Debug.Log(value);
        int br3 = int.Parse(value);
        Constants.br3 = br3;
        PlayerPrefs.SetInt("br3", Constants.br3);

    }

    public void savebs1(string value)
    {
        Debug.Log(value);
        float bs1 = float.Parse(value);
        Constants.bs1 = bs1;
        PlayerPrefs.SetFloat("bs1", Constants.bs1);

    }
    public void savebs2(string value)
    {
        Debug.Log(value);
        float bs2 = float.Parse(value);
        Constants.bs2 = bs2;
        PlayerPrefs.SetFloat("bs2", Constants.bs2);

    }
    public void savebs3(string value)
    {
        Debug.Log(value);
        float bs3 = float.Parse(value);
        Constants.bs3 = bs3;
        PlayerPrefs.SetFloat("bs3", Constants.bs3);

    }

    public void back() {
        SceneManager.LoadScene("Scenes/Menu");
    }

    public void clearPrefs()
    {
        Constants.clearPrefs();
    }

}
