using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{

    public float speed;
    private float prevSpeed;
    private int startX;
    private int startY;
    private float x;
    private float y;

    private int level;

    private System.String gameAdaptation;
    public float br;
    public float speed1, speed2, speed3,speed0, speed4;

    // Use this for initialization
    void Start()
    {
        //Debug.Log("Ball start");
        level = GameObject.FindGameObjectWithTag("game_field").GetComponent<FieldController>().level;
        //startX = 0;
        //startY = -1;
        //x = 0;
        //y = 1;
        startX = Random.Range(0, 2) == 0 ? -1 : 1;
        startY = Random.Range(0, 2) == 0 ? -1 : 1;
        x = Random.value;
        y = Mathf.Sqrt(1.0f - Mathf.Pow(x, 2));

        gameAdaptation = "br";
        speed = 6.5f; //8.0 for pace and only game
        speed0 = speed * (1 / (Constants.bs0 * Constants.castingSpeed));
        speed1 = speed * (1 / (Constants.bs1 * Constants.castingSpeed));
        speed2 = speed * (1 / (Constants.bs2 * Constants.castingSpeed));
        speed3 = speed * (1 / (Constants.bs3 * Constants.castingSpeed));
        speed4 = speed * (1 / (Constants.bs4 * Constants.castingSpeed));
        //Debug.Log(Constants.bs1);
        //Debug.Log(Constants.bs2);
        //Debug.Log(Constants.bs3);
        //Debug.Log(Constants.castingSpeed);
        //Debug.Log(speed1);
        //Debug.Log(speed2);
        //Debug.Log(speed3);

        prevSpeed = speed;
        GetComponent<Rigidbody2D>().velocity = (new Vector3(x * startX, y * startY)) * speed;
        //GetComponent<Rigidbody2D>().velocity = (new Vector3(x * startX, y * startY)) * speed * LevelSpeed();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("Ball update");
        level = GameObject.FindGameObjectWithTag("game_field").GetComponent<FieldController>().level;

        /**
        if (prevSpeed != speed && speed >= 1)
        {
            //Debug.Log("Ball update prev speed != speed & speed >= 1,  speed = " + speed);
            GetComponent<Rigidbody2D>().velocity /= prevSpeed;
            GetComponent<Rigidbody2D>().velocity *= speed * LevelSpeed();
            prevSpeed = speed;
         }
        **/
        if (GetComponent<Rigidbody2D>().velocity.magnitude < 1f)
        {
            if (GetComponent<Rigidbody2D>().velocity.magnitude < 0.01)
            {
                //Debug.Log("Ball update velocity < 0.01f,  speed = " + speed);
                startX = Random.Range(0, 2) == 0 ? -1 : 1;
                startY = Random.Range(0, 2) == 0 ? -1 : 1;
                x = Random.value;
                y = Mathf.Sqrt(1.0f - Mathf.Pow(x, 2));
                prevSpeed = speed;
                //GetComponent<Rigidbody2D>().velocity = (new Vector3(x * startX, y * startY)) * speed * LevelSpeed();
                GetComponent<Rigidbody2D>().velocity = (new Vector3(x * startX, y * startY)) * speed ;
            }
            else {
                //Debug.Log("Ball update velocity < 1f, velocity > 0.01f,  speed = " + speed);
            }
        }
        
        //gameAdaptation = GetGameAdaptation();
        if (gameAdaptation.Equals("br"))
        {
            Debug.Log("bR");
            trial2Adaptation();
            // for game pace
            Debug.Log(speed);
            speed = speed;
            GetComponent<Rigidbody2D>().velocity /= prevSpeed;
            GetComponent<Rigidbody2D>().velocity *= speed;
            prevSpeed = speed;
        }
        else if (gameAdaptation.Equals("ie"))
        {
            // gameAdaptationInspExp();
        }
        else if (gameAdaptation.Equals("dyna"))
        {
            //gameAdaptationDynamic();
        }
        
        //GetComponent<Rigidbody2D>().velocity = GetComponent<Rigidbody2D>().velocity.normalized * LevelSpeed();
        //GetComponent<Rigidbody2D>().velocity = GetComponent<Rigidbody2D>().velocity.normalized ;


        //else
        // {
        //     speed = 3;
        //     GetComponent<Rigidbody2D>().velocity /= prevSpeed;
        //     GetComponent<Rigidbody2D>().velocity *= speed;
        //     prevSpeed = speed;
        //    Debug.Log("BioFeedback ball speed non = " + speed);

        //    GetComponent<FieldController>().castingSpeed = 0.07f;
        // }

        

        
    }

    float LevelSpeed()
    {
        //Debug.Log("Ball LevelSpeed");
        // modifying level speed also with breathing rate
        speed = Mathf.Sqrt(Mathf.Pow(level * 1.0f, (1.0f / 3)));
        return speed;
    }


    private void trial2Adaptation() {

        br = GetCurrentBR();
        if (br <= Constants.br1)
        {
            Debug.Log("BioFeedback Control update br < 6");
            Debug.Log(speed);
            speed = speed0;
            GetComponent<Rigidbody2D>().velocity /= prevSpeed;
            GetComponent<Rigidbody2D>().velocity *= speed;
            prevSpeed = speed;
            //GetComponent<FieldController>().castingSpeed = 0.1f;
            //Debug.Log("BioFeedback ball speed 1= ");
        }
        else if (br >= Constants.br1 && br <= Constants.br2)
        {
            Debug.Log("BioFeedback Control update br > 6.1 && br <= 8 ");
            Debug.Log(speed);

            speed = (((br - Constants.br1) * (speed2 - speed1)) / (Constants.br2 - Constants.br1)) + speed1;
            //speed = ((3 * br) / 2) - 6;
            GetComponent<Rigidbody2D>().velocity /= prevSpeed;
            GetComponent<Rigidbody2D>().velocity *= speed;
            prevSpeed = speed;
            //Debug.Log("BioFeedback ball speed 2= " + speed);
        }
        else if (br > Constants.br2 && br < Constants.br3)
        {
            //speed = br / 2;
            speed = (((br - Constants.br2) * (speed3 - speed2)) / (Constants.br3 - Constants.br2)) + speed2;
            //speed = ((3 * br) / 8) + 3;
            GetComponent<Rigidbody2D>().velocity /= prevSpeed;
            GetComponent<Rigidbody2D>().velocity *= speed;
            prevSpeed = speed;
            //Debug.Log("BioFeedback ball speed 3= " + speed);
        }
        /*
        else if (br > Constants.br3 && br < Constants.br4)
        {
            //speed = br / 2;
            speed = (((br - Constants.br3) * (speed4 - speed3)) / (Constants.br4 - Constants.br3)) + speed3;
            //speed = ((3 * br) / 8) + 3;
            GetComponent<Rigidbody2D>().velocity /= prevSpeed;
            GetComponent<Rigidbody2D>().velocity *= speed;
            prevSpeed = speed;
            //Debug.Log("BioFeedback ball speed 3= " + speed);
        }
        */
        else
        {
            //Debug.Log("BioFeedback Control update br > 24");
            speed = speed3;
            GetComponent<Rigidbody2D>().velocity /= prevSpeed;
            GetComponent<Rigidbody2D>().velocity *= speed;
            prevSpeed = speed;
            //Debug.Log("BioFeedback ball speed 4= " + speed);

        }
    }
    private void newGameAdaptation()
    {

        br = GetCurrentBR();

        if (br <= Constants.br1)
        {
            Debug.Log("BioFeedback Control update br < 6");
            Debug.Log(speed);
            speed = speed0;
            GetComponent<Rigidbody2D>().velocity /= prevSpeed;
            GetComponent<Rigidbody2D>().velocity *= speed;
            prevSpeed = speed;
            //GetComponent<FieldController>().castingSpeed = 0.1f;
            //Debug.Log("BioFeedback ball speed 1= ");
        }
        else if (br >= Constants.br1 && br <= Constants.br2)
        {
            Debug.Log("BioFeedback Control update br > 6.1 && br <= 8 ");
            Debug.Log(speed);

            speed = (((br - Constants.br1) * (speed2 - speed1)) / (Constants.br2 - Constants.br1)) + speed1;
            //speed = ((3 * br) / 2) - 6;
            GetComponent<Rigidbody2D>().velocity /= prevSpeed;
            GetComponent<Rigidbody2D>().velocity *= speed;
            prevSpeed = speed;
            //Debug.Log("BioFeedback ball speed 2= " + speed);
        }
        else if (br > Constants.br2 && br < Constants.br3)
        {
            //speed = br / 2;
            speed = (((br - Constants.br2) * (speed3 - speed2)) / (Constants.br3 - Constants.br2)) + speed2;
            //speed = ((3 * br) / 8) + 3;
            GetComponent<Rigidbody2D>().velocity /= prevSpeed;
            GetComponent<Rigidbody2D>().velocity *= speed;
            prevSpeed = speed;
            //Debug.Log("BioFeedback ball speed 3= " + speed);
        }
        else if (br > Constants.br3 && br < Constants.br4)
        {
            //speed = br / 2;
            speed = (((br - Constants.br3) * (speed4 - speed3)) / (Constants.br4 - Constants.br3)) + speed3;
            //speed = ((3 * br) / 8) + 3;
            GetComponent<Rigidbody2D>().velocity /= prevSpeed;
            GetComponent<Rigidbody2D>().velocity *= speed;
            prevSpeed = speed;
            //Debug.Log("BioFeedback ball speed 3= " + speed);
        }
        else
        {
            //Debug.Log("BioFeedback Control update br > 24");
            speed = speed3;
            GetComponent<Rigidbody2D>().velocity /= prevSpeed;
            GetComponent<Rigidbody2D>().velocity *= speed;
            prevSpeed = speed;
            //Debug.Log("BioFeedback ball speed 4= " + speed);

        }
        //GameObject.FindGameObjectWithTag("hrv").GetComponent<Text>().text = "" + (float)br;
    }


    private void gameAdaptionBreathingRate()
    {
        
        br = GetCurrentBR();
       
        if (br <= Constants.br1)
        {
            Debug.Log("BioFeedback Control update br < 6");
            Debug.Log(speed);
            speed = speed1;
            GetComponent<Rigidbody2D>().velocity /= prevSpeed;
            GetComponent<Rigidbody2D>().velocity *= speed;
            prevSpeed = speed;
            //GetComponent<FieldController>().castingSpeed = 0.1f;
            //Debug.Log("BioFeedback ball speed 1= ");
        }
        else if (br >= Constants.br1 && br <= Constants.br2)
        {
            Debug.Log("BioFeedback Control update br > 6.1 && br <= 8 ");
            Debug.Log(speed);
            
            speed = (((br - Constants.br1) * (speed2 - speed1)) / (Constants.br2 - Constants.br1)) + speed1;
            //speed = ((3 * br) / 2) - 6;
            GetComponent<Rigidbody2D>().velocity /= prevSpeed;
            GetComponent<Rigidbody2D>().velocity *= speed;
            prevSpeed = speed;
            //Debug.Log("BioFeedback ball speed 2= " + speed);
        }
        else if (br > Constants.br2 && br < Constants.br3)
        {
            //speed = br / 2;
            speed = (((br - Constants.br2) * (speed3 - speed2)) / (Constants.br3 - Constants.br2)) + speed2;
            //speed = ((3 * br) / 8) + 3;
            GetComponent<Rigidbody2D>().velocity /= prevSpeed;
            GetComponent<Rigidbody2D>().velocity *= speed;
            prevSpeed = speed;
            //Debug.Log("BioFeedback ball speed 3= " + speed);
        }
        else
        {
            //Debug.Log("BioFeedback Control update br > 24");
            speed = speed4;
            GetComponent<Rigidbody2D>().velocity /= prevSpeed;
            GetComponent<Rigidbody2D>().velocity *= speed;
            prevSpeed = speed;
            //Debug.Log("BioFeedback ball speed 4= " + speed);

        }
        //GameObject.FindGameObjectWithTag("hrv").GetComponent<Text>().text = "" + (float)br;
    }

    private void gameAdaptationInspExp()
    {
        List<short> brWaveForm = GetBreathingWaveForm();
    }
    private void gameAdaptationDynamic()
    {
        List<short> brWaveForm = GetBreathingWaveForm();
    }

    System.String GetGameAdaptation()
    {
        using (AndroidJavaClass cls_UnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            using (AndroidJavaObject obj_Activity = cls_UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
            {
                return obj_Activity.Call<System.String>("getGameBioFeedBack");
            }
        }
    }

    public float GetCurrentBR()
    {
        using (AndroidJavaClass cls_UnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            using (AndroidJavaObject obj_Activity = cls_UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
            {
                float breathingRate = (float)obj_Activity.Call<double>("getCurrentBR");
                Constants.breathingRateLog.Add((System.Double)breathingRate);
                Constants.breathingRateLogString.Add(breathingRate + "");
                return breathingRate;
            }
        }
        //return 50.0;
    }

    public List<short> GetBreathingWaveForm()
    {
        using (AndroidJavaClass cls_UnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            using (AndroidJavaObject obj_Activity = cls_UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
            {
                return obj_Activity.Call<List<short>>("getBreathingWaveForm");
            }
        }
        //return 50.0;
    }


}
