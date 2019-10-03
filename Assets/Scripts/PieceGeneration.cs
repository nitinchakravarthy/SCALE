using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;

public class PieceGeneration : MonoBehaviour
{
    private bool pointerDown;
    private float pointerDownTimer;
    public float requiredholdTime;


    public GameObject currentPiece;
    GameObject game_field;
    GameObject canvas;
    GameObject rotateRight;
    //GameObject tileTimer;
    //Text tileTimerText;
    Animator anim;
    public RuntimeAnimatorController controller;

    //int time = Constants.tileTime;

    public GameObject getCurrentPiece() {
        return currentPiece;
    }

    //ball = (GameObject) Instantiate(Resources.Load("ball"), transform.position, Quaternion.identity);
    // Use this for initialization
    void Start () {
        canvas = GameObject.FindGameObjectWithTag("canvas");
        game_field = GameObject.FindGameObjectWithTag("game_field");
        rotateRight = GameObject.FindGameObjectWithTag("rotateRight");
        rotateRight.SetActive(false);
        //Debug.Log(canvas);
        GeneratePiece ();
	}
	
	// Update is called once per frame
	void Update () {
        //Debug.Log(Input.mousePosition.y);
        

        if (currentPiece == null)
        {
            //canvas.GetComponent<tiletimerHandler>().timerReset();
            GeneratePiece();
            LongPress lp = currentPiece.AddComponent<LongPress>() as LongPress;
           // Debug.Log("Piece generated");
        }
        /*
        else {
            if (pointerDown )
            {   
                
                game_field = GameObject.FindGameObjectWithTag("game_field");
                //Debug.Log("got mouse click");
                //Debug.Log(currentPiece.name);
                rotatePiece();
            }
            //Debug.Log("Nada generated");
        }
        */
	}

    public void rotatePieceRight() {
        Debug.Log("Rotating Piece after animation");
        switch (currentPiece.name) {
            case "horizontal(Clone)":
                Destroy(currentPiece);
                currentPiece = (GameObject)Instantiate(Resources.Load("vertical"), transform.position, Quaternion.identity);
                break;
            case "vertical(Clone)":
                Destroy(currentPiece);
                currentPiece = (GameObject)Instantiate(Resources.Load("horizontal"), transform.position, Quaternion.identity);
                break;
            case "top_left(Clone)":
                Destroy(currentPiece);
                currentPiece = (GameObject)Instantiate(Resources.Load("top_right"), transform.position, Quaternion.identity);
                break;
            case "top_right(Clone)":
                Destroy(currentPiece);
                currentPiece = (GameObject)Instantiate(Resources.Load("bottom_right"), transform.position, Quaternion.identity);
                break;
            case "bottom_right(Clone)":
                Destroy(currentPiece);
                currentPiece = (GameObject)Instantiate(Resources.Load("bottom_left"), transform.position, Quaternion.identity);
                break;
            case "bottom_left(Clone)":
                Destroy(currentPiece);
                currentPiece = (GameObject)Instantiate(Resources.Load("top_left"), transform.position, Quaternion.identity);
                break;


        }
        anim = currentPiece.AddComponent<Animator>();
        Debug.Log("Item Rotated and replaced");
    }

    public void rotatePieceLeft()
    {
        Debug.Log("Rotating Piece after animation");
        switch (currentPiece.name)
        {
            case "horizontal(Clone)":
                Destroy(currentPiece);
                currentPiece = (GameObject)Instantiate(Resources.Load("vertical"), transform.position, Quaternion.identity);
                break;
            case "vertical(Clone)":
                Destroy(currentPiece);
                currentPiece = (GameObject)Instantiate(Resources.Load("horizontal"), transform.position, Quaternion.identity);
                break;
            case "top_left(Clone)":
                Destroy(currentPiece);
                currentPiece = (GameObject)Instantiate(Resources.Load("bottom_left"), transform.position, Quaternion.identity);
                break;
            case "top_right(Clone)":
                Destroy(currentPiece);
                currentPiece = (GameObject)Instantiate(Resources.Load("top_left"), transform.position, Quaternion.identity);
                break;
            case "bottom_right(Clone)":
                Destroy(currentPiece);
                currentPiece = (GameObject)Instantiate(Resources.Load("top_right"), transform.position, Quaternion.identity);
                break;
            case "bottom_left(Clone)":
                Destroy(currentPiece);
                currentPiece = (GameObject)Instantiate(Resources.Load("bottom_right"), transform.position, Quaternion.identity);
                break;


        }
        anim = currentPiece.AddComponent<Animator>();
        Debug.Log("Item Rotated and replaced");
    }




    public void GeneratePiece(){
		int rand = Random.Range (0, 6);
		switch (rand) {
		case 0:
			currentPiece = (GameObject)Instantiate (Resources.Load ("horizontal"), transform.position, Quaternion.identity);
			break;
		case 1:
			currentPiece = (GameObject)Instantiate (Resources.Load ("vertical"), transform.position, Quaternion.identity);
			break;
		case 2:
			currentPiece = (GameObject)Instantiate (Resources.Load ("top_left"), transform.position, Quaternion.identity);
			break;
		case 3:
			currentPiece = (GameObject)Instantiate (Resources.Load ("top_right"), transform.position, Quaternion.identity);
			break;
		case 4:
			currentPiece = (GameObject)Instantiate (Resources.Load ("bottom_right"), transform.position, Quaternion.identity);
			break;
		case 5:
			currentPiece = (GameObject)Instantiate (Resources.Load ("bottom_left"), transform.position, Quaternion.identity);
			break;
		}
        anim = currentPiece.AddComponent<Animator>();
        setRotateActive();

    }

    public void triggerRotateRightAnimation() {
        Debug.Log("Trigger Animation");
        controller = Resources.Load("Animations/RotateRightController") as RuntimeAnimatorController;
        Debug.Log("controller");
        Debug.Log(controller);
        currentPiece.GetComponent<Animator>().runtimeAnimatorController = controller;
        //rotatePiece();
    }

    public void triggerRotateLeftAnimation()
    {
        Debug.Log("Trigger Animation");
        controller = Resources.Load("Animations/RotateLeftController") as RuntimeAnimatorController;
        Debug.Log("controller");
        Debug.Log(controller);
        currentPiece.GetComponent<Animator>().runtimeAnimatorController = controller;
        //rotatePiece();
    }

    public void setRotateActive() {
        if (Constants.percentile(Constants.rotatePercentile))
        {
            rotateRight.SetActive(true);
        }
        else {
            rotateRight.SetActive(false);
        }
    }


}
