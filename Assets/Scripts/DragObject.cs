using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragObject : MonoBehaviour {
	public float offset;
	private Vector3 screenPoint;
	private Vector3 mouseOffset;
	private float mouseX;
	private float mouseY;
	private Vector3 mousePos;
	private bool draggable;

	private float touchScale;
	private Vector3 initSpriteSize;
	private Vector3 finalSpriteSize;
	private float startTime;
	private int mouseDownCount;
	private int mouseUpCount;

	private PolygonCollider2D fieldCollider;
	private FieldController field;

	private GameObject pieceGenerator;

    private float holdTime;
    private float holdMouseX;
    private float holdMouseY;
    private bool rotating;
    public float rotSpeed = 5.0f;
	// Use this for initialization
	void Start () {
        //Debug.Log("Drag Object start");
		field = GameObject.FindGameObjectWithTag ("game_field").GetComponent<FieldController>();
		fieldCollider = GameObject.FindGameObjectWithTag ("game_field").GetComponent<PolygonCollider2D> ();

		pieceGenerator = GameObject.Find ("piece_generator");

		draggable = false;
		offset = Screen.height / 7f;

		touchScale = 1.2f;
		initSpriteSize = transform.localScale;
		finalSpriteSize = initSpriteSize * touchScale;
		startTime = Time.time;
		mouseDownCount = 0;
		mouseUpCount = 0;
	}

    // Update is called once per frame

	void Update () {

        mouseX = Input.mousePosition.x;
		mouseY = Input.mousePosition.y;

        if (Input.GetMouseButtonDown(0) && ContainsPoint((Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition))) {
            holdTime = Time.time;
            holdMouseX = Input.mousePosition.x;
            holdMouseY = Input.mousePosition.y;
        }
        
        if (Input.GetMouseButton(0) && !field.casting) {
            

            if (mouseDownCount == 0) {
				startTime = Time.time;
				mouseDownCount++;
				mouseUpCount = 0;
                //
                if (ContainsPoint((Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition)))
                {
                    draggable = true;
                }
			}

			if (draggable) {
				OnMouseDrag ();
				gameObject.GetComponent<SpriteRenderer> ().color = new Color (1f, 1f, 1f, 0.5f);
				//if (Vector2.Distance (transform.localScale, finalSpriteSize) > Vector3.kEpsilon) {
				//	transform.localScale = Vector3.Lerp (initSpriteSize, finalSpriteSize, (Time.time - startTime) / 0.1f);
				//}
			}
		} else {
			if (mouseUpCount == 0) {
				startTime = Time.time;
				mouseUpCount++;
				mouseDownCount = 0;
			}
			if (!PolyContains ((Vector2)transform.position)) {
				transform.position = pieceGenerator.transform.position;
			}
			draggable = false;
			gameObject.GetComponent<SpriteRenderer> ().color = new Color (1f, 1f, 1f, 1f);
			//if (Vector3.Distance (transform.localScale, initSpriteSize) > Vector3.kEpsilon) {
			//	transform.localScale = Vector3.Lerp (finalSpriteSize, initSpriteSize, (Time.time - startTime) / 0.25f);
			//}
		}
	}

    void replaceRightPiece() {
        pieceGenerator.GetComponent<PieceGeneration>().rotatePieceRight();
    }

    //float rotationAmount = 45f;
    //float delaySpeed = 0.1f;

    void OnMouseDrag() {
			transform.position = Camera.main.ScreenToWorldPoint (new Vector3 (mouseX, mouseY, 1.0f));
	}


	bool ContainsPoint(Vector2 point){
        //Debug.Log("inside Contains Point");
		float buffer = 0.25f;

		Vector2 objCenter = (Vector2)transform.position;
		Vector2 minX = objCenter + Vector2.left * (gameObject.GetComponent<BoxCollider2D> ().bounds.extents.x + buffer);
		Vector2 maxX = objCenter + Vector2.right*(gameObject.GetComponent<BoxCollider2D> ().bounds.extents.x + buffer);
		Vector2 minY = objCenter + Vector2.down*(gameObject.GetComponent<BoxCollider2D> ().bounds.extents.y + buffer);
		Vector2 maxY = objCenter + Vector2.up*(gameObject.GetComponent<BoxCollider2D> ().bounds.extents.y + buffer);

		if (point.x >= minX.x && point.x <= maxX.x && point.y >= minY.y && point.y <= maxY.y) {
			return true;
		} else {
			return false;
		}
	}

	bool PolyContains(Vector2 pos){
        //Debug.Log("inside poly Contains");
        int numIntersects = 0;
		ArrayList pairs = new ArrayList ();

		//Creates array of points of the polygon with world points
		Vector2[] realPoints = new Vector2[fieldCollider.points.Length];
		for (int i = 0; i < realPoints.Length; i++)
			realPoints [i] = (Vector2)fieldCollider.transform.TransformPoint ((Vector3)fieldCollider.points [i]);

		//Finds all pairs of points that are both on the left side of the piece and that also have the same x-value
		for (int i = 0; i < realPoints.Length; i++) {
			if((realPoints[i].x < pos.x) && (realPoints[(i+1)%realPoints.Length].x < pos.x)){
				if (Mathf.Abs (realPoints [i].x - realPoints [(i + 1) % realPoints.Length].x) < 0.00005f) {
					pairs.Add (new Vector2[2] {realPoints [i], realPoints [(i + 1) % realPoints.Length]});
				}
			}
		}

		//Removes pairs of points that don't contain the y-value of pos and find the closest pair to pos
		int index = 0;

		while (index < pairs.Count) {
			Vector2 p1 = ((Vector2[])pairs [index]) [0];
			Vector2 p2 = ((Vector2[])pairs [index]) [1];

			if (p1.y > p2.y) {
				if ((p1.y < pos.y) || (p2.y > pos.y)) {
					pairs.RemoveAt (index);
					index--;
				} else {
					numIntersects++;
				}
			} else {
				if ((p2.y < pos.y) || (p1.y > pos.y)) {
					pairs.RemoveAt (index);
					index--;
				} else {
					numIntersects++;
				}
			}

			index++;
		}
		return numIntersects % 2 != 0;
	}



}
