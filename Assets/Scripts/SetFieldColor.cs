using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetFieldColor : MonoBehaviour {

	private Color darkgreen;
	private Color mutedblue;
	private Color royalblue;
	private Color mutedpurple;
	private Color orangebrown;

	private Color chosenColor;

	// Use this for initialization
	void Awake () {
		//fieldSprite = gameObject.GetComponent<SpriteRenderer> ();

		darkgreen = new Color (0, 198f/255f, 29f/255f);
		mutedblue = new Color (101f/255f, 161f/255f, 207f/255f);
		royalblue = new Color (0, 92f/255f, 255f/255f);
		mutedpurple = new Color (226f/255f, 127f/255f, 162f/255f);
		orangebrown = new Color (226f/255f, 155f/255f, 88f/255f);

		int field = Random.Range (0, 5);
		switch (field) {
		case 0:
			chosenColor = darkgreen;
			break;
		case 1:
			chosenColor = mutedblue;
			break;
		case 2:
			chosenColor = royalblue;
			break;
		case 3:
			chosenColor = mutedpurple;
			break;
		case 4:
			chosenColor = orangebrown;
			break;
		}
	}

	void Update(){
			Mesh fieldMesh = GetComponent<MeshFilter> ().mesh;
			Vector3[] vertices = fieldMesh.vertices;
			Color[] colors = new Color[vertices.Length];
			for (int i = 0; i < vertices.Length; i++) {
				colors [i] = chosenColor;
			}
			fieldMesh.colors = colors;
	}
}
