using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class getTotalPoints : MonoBehaviour
{
    public Text pointsText;
    // Start is called before the first frame update
    void Start()
    {
        pointsText.text = "Total Points: " + Constants.points;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
