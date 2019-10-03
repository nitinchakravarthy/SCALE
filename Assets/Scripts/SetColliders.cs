using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetColliders : MonoBehaviour {

	public float colliderWidth;

	private ArrayList borderColls;

	private PolygonCollider2D fieldColl;
	private LineRenderer borderLineRend;
    public GameObject field;
    public GameObject ball;

    private PolygonCollider2D fieldCollider;

    //private int numPoints;

    // Use this for initialization
    void Start () {
		borderColls = new ArrayList ();
		fieldColl = GetComponentInParent<PolygonCollider2D> ();
		borderLineRend = GetComponent<LineRenderer> ();
		borderLineRend.sortingLayerName = "Game Field";

		//Setting mesh to polygon
		MeshRenderer fieldMesh = GetComponentInParent<MeshRenderer>();
		fieldMesh.sortingLayerName = "Game Field";
		ResetMesh();

		//Adding initial colliders to border of polygon collider
		for (int num = 0; num < fieldColl.points.Length; num++) {
			borderColls.Add(gameObject.AddComponent<BoxCollider2D> ());
		}
		//numPoints = fieldColl.points.Length;

		SetColliderPos ();
	}
	
	// Update is called once per frame
	void Update () {
        ball = GameObject.FindGameObjectWithTag("Ball");
        //Debug.Log(ball);
        //Debug.Log(field);

        GetComponentInParent<Transform> ().position = fieldColl.bounds.center;
		SetColliderPos ();

		borderLineRend.positionCount = fieldColl.points.Length+1;
		Vector3[] linePoints = new Vector3[fieldColl.points.Length+1];
		for (int i = 0; i <= fieldColl.points.Length; i++) {
			linePoints [i] = fieldColl.transform.TransformPoint((Vector3)fieldColl.points [i%fieldColl.points.Length]);
		}
		borderLineRend.SetPositions (linePoints);

	}
		
	public void SetColliderPos(){
		// Adds/Deletes colliders based off of number of points
		//if (fieldColl.points.Length != numPoints) {
			borderColls.Clear ();
			foreach(BoxCollider2D coll in gameObject.GetComponents<BoxCollider2D>()){
				Destroy (coll);
			}
			//borderColls.Clear ();
			for (int num = 0; num < fieldColl.points.Length; num++) {
				borderColls.Add(gameObject.AddComponent<BoxCollider2D> ());
			}
		//}

		//assumes that adjacent points are either on the same x or y plane
		for (int i1 = 0; i1 < fieldColl.points.Length; i1++) {
			int i2 = (i1+1) % fieldColl.points.Length;
			Vector2 point1 = fieldColl.transform.TransformPoint (fieldColl.points [i1]);
			Vector2 point2 = fieldColl.transform.TransformPoint (fieldColl.points [i2]);

			Vector2 length = point2 - point1;
			float x = length.x;
			float y = length.y;
			Vector2 collSize = Mathf.Abs(x) > Mathf.Abs(y) ? new Vector2 (Mathf.Abs(x)-0.01f, colliderWidth) : new Vector2 (colliderWidth, Mathf.Abs(y)-0.01f);

			if (Mathf.Abs(y) < 0.01f) {
				y = point1.x < point2.x ? y + colliderWidth : y - colliderWidth;
			}
			if (Mathf.Abs(x) < 0.01f) {
				x = point1.y < point2.y ? x - colliderWidth : x + colliderWidth;
			}
			Vector2 collPos = new Vector2 (point1.x + (x / 2f), point1.y + (y / 2f));

			BoxCollider2D tempColl = (BoxCollider2D)borderColls [i1];
			tempColl.size = collSize;
			tempColl.offset = collPos - (Vector2)fieldColl.transform.position;
		}

		transform.position = fieldColl.transform.position;

	}

	public void ResetMesh(){
		int pointCount = 0;
		pointCount = fieldColl.points.Length;
		MeshFilter mf = GetComponentInParent<MeshFilter> ();
		Mesh mesh = new Mesh ();

		Vector2[] points = fieldColl.points;
		Vector3[] vertices = new Vector3[pointCount];
		for (int j = 0; j < pointCount; j++) {
			Vector2 actual = points [j];
			vertices [j] = new Vector3 (actual.x, actual.y, 2);
		}

		Triangulator tr = new Triangulator (points);
		int[] triangles = tr.Triangulate ();
		mesh.vertices = vertices;
		mesh.triangles = triangles;
		mf.mesh = mesh;
	}

}
