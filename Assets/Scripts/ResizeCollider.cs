using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResizeCollider : MonoBehaviour {
	//public float ballScale;
	private Vector2 spriteSize;

	// Use this for initialization
	void Start () {
		spriteSize = gameObject.GetComponent<SpriteRenderer> ().size;
		gameObject.GetComponent<BoxCollider2D> ().size = spriteSize;
		//gameObject.GetComponent<BoxCollider2D> ().size = new Vector2 (spriteSize.x - ballScale, spriteSize.y - ballScale);
	}
	
	// Update is called once per frame
	void Update () {
		spriteSize = gameObject.GetComponent<SpriteRenderer> ().size;
		gameObject.GetComponent<BoxCollider2D> ().size = spriteSize;
		//gameObject.GetComponent<BoxCollider2D> ().size = new Vector2 (spriteSize.x - ballScale, spriteSize.y - ballScale);
	}
}
