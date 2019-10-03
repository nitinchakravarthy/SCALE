using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    private GameObject pieceGenerator;

    // Update is called once per frame
    void Update()
    {

    }

    public void rotatePieceRight()
    {
        Debug.Log("RotatePieceRight ran");
        pieceGenerator = GameObject.Find("piece_generator");
        pieceGenerator.GetComponent<PieceGeneration>().triggerRotateRightAnimation();
    }
}
