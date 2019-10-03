using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuLayout : MonoBehaviour {

	private GameObject playButton;
    private GameObject settingsButton;

	// Use this for initialization
	void Start () {
		playButton = GameObject.Find ("Play");
        settingsButton = GameObject.Find("settings");
		playButton.GetComponent<RectTransform> ().sizeDelta = new Vector2 (Screen.width / 1.5f, Screen.height / 15f);
        //settingsButton.GetComponent<RectTransform>().position = playButton.transform.position + Vector3.up * 300.0f + Vector3.right * 500.0f; ;

    }

    // Update is called once per frame
    void Update () {
		
	}
}
