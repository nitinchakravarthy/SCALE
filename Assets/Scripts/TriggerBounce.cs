using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerBounce : MonoBehaviour {
	public double cornerBuffer;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerExit2D(Collider2D collider){
		GameObject collidedObject = collider.gameObject;
		float xComp = collider.attachedRigidbody.velocity.x;
		float yComp = collider.attachedRigidbody.velocity.y;

		float triggerCenterX = gameObject.GetComponent<Collider2D> ().bounds.center.x;
		float triggerCenterY = gameObject.GetComponent<Collider2D> ().bounds.center.y;

		float xLength = gameObject.GetComponent<Collider2D> ().bounds.extents.x;
		float yLength = gameObject.GetComponent<Collider2D> ().bounds.extents.y;

		float closestX = gameObject.GetComponent<Collider2D> ().bounds.ClosestPoint (collidedObject.transform.position).x;
		float closestY = gameObject.GetComponent<Collider2D> ().bounds.ClosestPoint (collidedObject.transform.position).y;
		Vector2 closestPoint = new Vector2(closestX,closestY);

		Vector2[] corners = new Vector2[4];
		corners [0] = new Vector2 (triggerCenterX + xLength, triggerCenterY + yLength);
		corners [1] = new Vector2 (triggerCenterX + xLength, triggerCenterY - yLength);
		corners [2] = new Vector2 (triggerCenterX - xLength, triggerCenterY + yLength);
		corners [3] = new Vector2 (triggerCenterX - xLength, triggerCenterY - yLength);

		float minMag = Mathf.Infinity;
		int minInd = -1;
		for (int i = 0; i < corners.Length; i++) {
			if (Vector2.Distance (corners [i], closestPoint) < minMag) {
				minMag = Vector2.Distance (corners [i], closestPoint);
				minInd = i;
			}
		}

		float xDistance = Mathf.Abs (closestX - corners[minInd].x);
		float yDistance = Mathf.Abs (closestY - corners[minInd].y);

		if (Mathf.Abs (xDistance - yDistance) < cornerBuffer) {
			collidedObject.GetComponent<Rigidbody2D> ().velocity = new Vector2 (-xComp, -yComp);
		} else {
			float chance = xDistance<yDistance ? -1 : 1;
			collidedObject.GetComponent<Rigidbody2D>().velocity = new Vector2(chance*xComp,-chance*yComp);
		}
	}
}
