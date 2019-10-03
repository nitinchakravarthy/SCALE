using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleRegionController : MonoBehaviour {

	private Vector3 regionPos;
	private Vector2 regionSize;

	// Use this for initialization
	void Start () {
		GameObject field = GameObject.FindGameObjectWithTag ("game_field");
		regionPos = field.transform.position;
		regionSize = field.GetComponent<SpriteRenderer> ().size;

		transform.position = regionPos;
		gameObject.GetComponent<BoxCollider2D> ().size = regionSize;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
