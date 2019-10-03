using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FieldController : MonoBehaviour
{
    public bool playing;

    public float ballScale;
    public float castingSpeed;

    private Vector3 ballFieldAreaRatio;
    private Vector3 halfStartingBallScale;

    private float origArea; //original size of the shape for each level
    private Vector2 levelSize; //Play region
    private Vector3 levelCenter; //center of play region
    public float levelGoalSizePercentage; //number between 0 and 1 that has the percentage of area needed to move on
    public bool nextLevel;
    public int level;
    private float score;
    private float scoreBarWidth;
    private float scoreBarHeight;
    public int lives;
    private float startTime;
    public int pointsTotal;

    private Vector2[] startPoints;
    private Vector2 realCenter;

    public bool casting;

    private RaycastHit2D hit1;
    private RaycastHit2D hit2;
    private Vector2 currCastDir1;
    private Vector2 currCastDir2;
    private float castDist1;
    private float castDist2;
    public float textScale;

    private Collider2D currCastingColl;
    private PolygonCollider2D fieldColl;

    private ArrayList pieces = new ArrayList();
    public GameObject[] balls = new GameObject[1];

    private GameObject levelLabel;
    private GameObject pointsLabel;
    private GameObject livesLabel;
    private GameObject scoreLabel;
    private GameObject hrvLabel;
    private GameObject timerLabel;
    public Slider progressSlider;

    public Image fadeImage;
    public Animator fadeAnim;

    private float floatingPointError;
    public float startRR;

    public string startTimeStamp;
    public string endTimeStamp;
    public float percentage = 0.0f;
    Vector3[] ballPos;

    void Awake()
    {
        playing = true;
        //balls [0] = (GameObject)Instantiate (Resources.Load ("ball"), transform.position, Quaternion.identity);
    }

    // Use this for initialization
    void Start()
    {
        //DontDestroyOnLoad(GameObject.FindGameObjectWithTag("audio_source"));

        fieldColl = GetComponent<PolygonCollider2D>();
        float xLength = fieldColl.points[1].x - fieldColl.points[0].x;
        float yLength = fieldColl.points[1].y - fieldColl.points[2].y;
        origArea = PolygonArea(fieldColl);
        levelSize = new Vector2(xLength, yLength);
        levelCenter = transform.position;
        nextLevel = false;
        level = 1;
        pointsTotal = Constants.points;
        score = 0.0f;
        scoreBarWidth = 182f;
        scoreBarHeight = 10f;
        lives = Constants.lives;
        startTime = Time.time;
        balls[0] = (GameObject)Instantiate(Resources.Load("ball"), transform.position, Quaternion.identity);

        Transform ballTrans = GameObject.FindGameObjectWithTag("Ball").GetComponent<Transform>();
        ballFieldAreaRatio = ballTrans.localScale / 2 / PolygonArea(fieldColl);
        halfStartingBallScale = ballTrans.localScale / 2;

        startPoints = fieldColl.points;

        levelLabel = GameObject.FindGameObjectWithTag("level");
        pointsLabel = GameObject.FindGameObjectWithTag("points");
        livesLabel = GameObject.FindGameObjectWithTag("lives");
        scoreLabel = GameObject.FindGameObjectWithTag("score");
        //hrvLabel = GameObject.FindGameObjectWithTag("hrv");
        timerLabel = GameObject.FindGameObjectWithTag("timer");
        progressSlider = GameObject.FindGameObjectWithTag("progress_slider").GetComponent<Slider>();

        levelLabel.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width / textScale, Screen.height / textScale);

        scoreBarWidth = Screen.width / 2f;
        scoreBarHeight = Screen.height / 40f;

        livesLabel.GetComponent<RectTransform>().sizeDelta = new Vector2(scoreBarWidth, Screen.height / textScale / 4);
        //hrvLabel.GetComponent<RectTransform>().sizeDelta = new Vector2(scoreBarWidth, Screen.height / textScale / 4);
        timerLabel.GetComponent<RectTransform>().sizeDelta = new Vector2(scoreBarWidth, Screen.height / textScale / 4);

        RectTransform scoreBackLabel = GameObject.FindGameObjectWithTag("score_back").GetComponent<RectTransform>();
        scoreBackLabel.sizeDelta = new Vector2(scoreBarWidth, scoreBarHeight);

        casting = false;
        castDist1 = 0f;
        castDist2 = 0f;
        Physics2D.queriesHitTriggers = false;
        gameObject.GetComponent<LineRenderer>().sortingLayerName = "play objects";

        // multiplying it with castingspeed in constants to change it's value respectively
        castingSpeed = 0.13f * (1 / Constants.castingSpeed);   // half second per half field on average (0.13f)

        floatingPointError = 0.00001f;
        startRR = GetComponent<BiofeedbackControl>().GetCurrentBR();
        startTimeStamp = GetComponent<BiofeedbackControl>().getTimeStamp();
        
        double currRR = GetComponent<BiofeedbackControl>().GetCurrentBR();
        // if BR >6.0f and BR is increasing then add anew ball
        // commenting out for pace
        
        if (currRR > 6.0f || !getIsBRdecreasing())
        {
        GameObject newBall = (GameObject)Instantiate(Resources.Load("ball"), transform.position, Quaternion.identity);
        balls = GameObject.FindGameObjectsWithTag("Ball");
        Debug.Log(balls.Length);
        }
       
       

    }

    // Updates the labels on the screen
    void Update()
    {
        //Text levelLabel = GameObject.FindGameObjectWithTag ("level").GetComponent<Text>();
        levelLabel.GetComponent<Text>().text = "Level: " + level;
        pointsLabel.GetComponent<Text>().text = "" + pointsTotal;
        //RectTransform scoreLabel = GameObject.FindGameObjectWithTag ("score").GetComponent<RectTransform> ();
        //max width should be 200
        //scoreLabel.GetComponent<RectTransform>().sizeDelta = new Vector2(score, scoreBarHeight);
        progressSlider.value = percentage;
        livesLabel.GetComponent<Text>().text = "" + lives;

        if (lives <= 0)
        {
            SceneManager.LoadScene("Scenes/Exit Scene");
            // time continues even on the exit scene
            //if (Constants.timeCoroutine != null)
            //{
            //    Debug.Log("stopping couroutine 2");
            //    StopCoroutine(Constants.timeCoroutine);
            //}
            //StartCoroutine (Fading ());

            //Debug.Log("lives ");
            // Destroy(GameObject.Find("background"));
            Constants.points = pointsTotal;
            PlayDetails playDetails = new PlayDetails();
            playDetails.LevelsFinished = level - 1;
            playDetails.PointsTotal = pointsTotal;
            playDetails.StartRR = startRR;
            playDetails.StartTime = startTimeStamp;
            endTimeStamp = GetComponent<BiofeedbackControl>().getTimeStamp();
            playDetails.EndTime = endTimeStamp;
            playDetails.EndRR = GetComponent<BiofeedbackControl>().GetCurrentBR();
            Constants.gamePlayVsRR.Add(Constants.GamePlays, playDetails);
            //Debug.Log("playdetails " + playDetails);

        }

    }

    public void setGameSummary()
    {
        //StartCoroutine (Fading ());

        //Debug.Log("lives ");
        // Destroy(GameObject.Find("background"));
        Constants.points = pointsTotal;
        PlayDetails playDetails = new PlayDetails();
        playDetails.LevelsFinished = level - 1;
        playDetails.PointsTotal = pointsTotal;
        playDetails.StartRR = startRR;
        playDetails.StartTime = startTimeStamp;
        endTimeStamp = GetComponent<BiofeedbackControl>().getTimeStamp();
        playDetails.EndTime = endTimeStamp;
        playDetails.EndRR = GetComponent<BiofeedbackControl>().GetCurrentBR();
        Constants.gamePlayVsRR.Add(Constants.GamePlays, playDetails);
        //Debug.Log("playdetails " + playDetails);
    }

    IEnumerator Fading()
    {
        fadeAnim.SetBool("Fade", true);
        yield return new WaitUntil(() => fadeImage.color.a == 1);
        Destroy(GameObject.Find("background"));
        SceneManager.LoadScene("Scenes/Menu");
    }

    void FixedUpdate()
    {
        if (!nextLevel)
        {
            // Raycasting when piece is placed on the field
            GameObject playPiece = FindObjectWithLayer(11);
            if (!casting && (playPiece != null) && !Input.GetMouseButton(0))
            {
                Collider2D tempColl = playPiece.GetComponent<Collider2D>();
                if (PolyContains((Vector2)tempColl.transform.position))
                {
                    currCastingColl = tempColl;
                    casting = true;
                }
            }

            GameObject[] mainBalls = GameObject.FindGameObjectsWithTag("Ball");
            ballPos = new Vector3[mainBalls.Length];
            
           
            for (int i = 0; i < mainBalls.Length; i++)
            {
                if (PolyContains(mainBalls[i].transform.position))
                {
                    Debug.Log("Ball in the hole");
                    ballPos[i] = mainBalls[i].transform.position;
                }
                else
                {
                    //Debug.Log("Catch the ball");
                    Destroy(mainBalls[i]);
                    mainBalls[i] = (GameObject)Instantiate(Resources.Load("ball"), ballPos[i], Quaternion.identity);
                    ScaleBalls();
                    Debug.Log("New Ball instantiated");
                }
            }
            
            if (casting)
            {
                Vector2 piecePosition = new Vector2(currCastingColl.transform.position.x, currCastingColl.transform.position.y);
                if (PolyContains(piecePosition))
                {
                    string pieceType = currCastingColl.tag;

                    switch (pieceType)
                    {
                        case "horizontal":
                            currCastDir1 = Vector2.left;
                            currCastDir2 = Vector2.right;
                            break;
                        case "vertical":
                            currCastDir1 = Vector2.up;
                            currCastDir2 = Vector2.down;
                            break;
                        case "top_left":
                            currCastDir1 = Vector2.up;
                            currCastDir2 = Vector2.left;
                            break;
                        case "top_right":
                            currCastDir1 = Vector2.up;
                            currCastDir2 = Vector2.right;
                            break;
                        case "bottom_right":
                            currCastDir1 = Vector2.down;
                            currCastDir2 = Vector2.right;
                            break;
                        case "bottom_left":
                            currCastDir1 = Vector2.down;
                            currCastDir2 = Vector2.left;
                            break;
                    }

                    Vector3 castPos1 = piecePosition + currCastDir1 * castDist1;
                    Vector3 castPos2 = piecePosition + currCastDir2 * castDist2;
                    if ((currCastDir1 == Vector2.up) && (castPos1.y < transform.TransformPoint(FindUpIntersect(piecePosition)).y))
                    {
                        castDist1 += castingSpeed;
                    }
                    if ((currCastDir1 == Vector2.down) && (castPos1.y > transform.TransformPoint(FindDownIntersect(piecePosition)).y))
                    {
                        castDist1 += castingSpeed;
                    }
                    if ((currCastDir1 == Vector2.left) && (castPos1.x > transform.TransformPoint(FindLeftIntersect(piecePosition)).x))
                    {
                        castDist1 += castingSpeed;
                    }

                    if ((currCastDir2 == Vector2.left) && (castPos2.x > transform.TransformPoint(FindLeftIntersect(piecePosition)).x))
                    {
                        castDist2 += castingSpeed;
                    }
                    if ((currCastDir2 == Vector2.right) && (castPos2.x < transform.TransformPoint(FindRightIntersect(piecePosition)).x))
                    {
                        castDist2 += castingSpeed;
                    }
                    if ((currCastDir2 == Vector2.down) && (castPos2.y > transform.TransformPoint(FindDownIntersect(piecePosition)).y))
                    {
                        castDist2 += castingSpeed;
                    }

                    hit1 = Physics2D.Raycast(piecePosition, currCastDir1, castDist1);
                    hit2 = Physics2D.Raycast(piecePosition, currCastDir2, castDist2);

                    Vector3[] points = new Vector3[3];
                    points[0] = new Vector3(piecePosition.x + currCastDir1.x * castDist1, piecePosition.y + currCastDir1.y * castDist1);
                    points[1] = piecePosition;
                    points[2] = new Vector3(piecePosition.x + currCastDir2.x * castDist2, piecePosition.y + currCastDir2.y * castDist2);

                    gameObject.GetComponent<LineRenderer>().positionCount = 3;
                    gameObject.GetComponent<LineRenderer>().SetPositions(points);

                    if (hit1.collider != null || hit2.collider != null)
                    {
                        if (hit1.collider != null)
                        {
                            string colliderTag = hit1.collider.tag;
                            if (colliderTag.Equals("Ball"))
                            {
                                hit1.collider.gameObject.GetComponent<ParticleSystem>().Emit(1);
                                RemovePiece();
                                lives--;
                            }
                        }
                        if (hit2.collider != null)
                        {
                            string colliderTag = hit2.collider.tag;
                            if (colliderTag.Equals("Ball"))
                            {
                                hit2.collider.gameObject.GetComponent<ParticleSystem>().Emit(5);
                                RemovePiece();
                                lives--;
                            }
                        }

                        castPos1 = piecePosition + currCastDir1 * castDist1;
                        castPos2 = piecePosition + currCastDir2 * castDist2;

                        switch (pieceType)
                        {
                            case "horizontal":
                                if ((castPos1.x <= transform.TransformPoint(FindLeftIntersect(piecePosition)).x) &&
                                    (castPos2.x >= transform.TransformPoint(FindRightIntersect(piecePosition)).x))
                                {
                                    Chop(currCastingColl);
                                    startTime = Time.time;
                                    startPoints = fieldColl.points;
                                    RemovePiece();

                                    if (PolygonArea(fieldColl) / origArea <= levelGoalSizePercentage)
                                    {
                                        nextLevel = true;
                                        pointsTotal += level;
                                        level++;
                                    }
                                }
                                break;
                            case "vertical":
                                if ((castPos1.y >= transform.TransformPoint(FindUpIntersect(piecePosition)).y) &&
                                    (castPos2.y <= transform.TransformPoint(FindDownIntersect(piecePosition)).y))
                                {
                                    Chop(currCastingColl);
                                    startTime = Time.time;
                                    startPoints = fieldColl.points;
                                    RemovePiece();

                                    if (PolygonArea(fieldColl) / origArea <= levelGoalSizePercentage)
                                    {
                                        nextLevel = true;
                                        pointsTotal += level;
                                        level++;
                                    }
                                }
                                break;
                            case "top_left":
                                if ((castPos1.y >= transform.TransformPoint(FindUpIntersect(piecePosition)).y) &&
                                    (castPos2.x <= transform.TransformPoint(FindLeftIntersect(piecePosition)).x))
                                {
                                    Chop(currCastingColl);
                                    startTime = Time.time;
                                    startPoints = fieldColl.points;
                                    RemovePiece();

                                    if (PolygonArea(fieldColl) / origArea <= levelGoalSizePercentage)
                                    {
                                        nextLevel = true;
                                        pointsTotal += level;
                                        level++;
                                    }
                                }
                                break;
                            case "top_right":
                                if ((castPos1.y >= transform.TransformPoint(FindUpIntersect(piecePosition)).y) &&
                                    (castPos2.x >= transform.TransformPoint(FindRightIntersect(piecePosition)).x))
                                {
                                    Chop(currCastingColl);
                                    startTime = Time.time;
                                    startPoints = fieldColl.points;
                                    RemovePiece();

                                    if (PolygonArea(fieldColl) / origArea <= levelGoalSizePercentage)
                                    {
                                        nextLevel = true;
                                        pointsTotal += level;
                                        level++;
                                    }
                                }
                                break;
                            case "bottom_right":
                                if ((castPos1.y <= transform.TransformPoint(FindDownIntersect(piecePosition)).y) &&
                                    (castPos2.x >= transform.TransformPoint(FindRightIntersect(piecePosition)).x))
                                {
                                    Chop(currCastingColl);
                                    startTime = Time.time;
                                    startPoints = fieldColl.points;
                                    RemovePiece();

                                    if (PolygonArea(fieldColl) / origArea <= levelGoalSizePercentage)
                                    {
                                        nextLevel = true;
                                        pointsTotal += level;
                                        level++;
                                    }
                                }
                                break;
                            case "bottom_left":
                                if ((castPos1.y <= transform.TransformPoint(FindDownIntersect(piecePosition)).y) &&
                                    (castPos2.x <= transform.TransformPoint(FindLeftIntersect(piecePosition)).x))
                                {
                                    Chop(currCastingColl);
                                    startTime = Time.time;
                                    startPoints = fieldColl.points;
                                    RemovePiece();

                                    if (PolygonArea(fieldColl) / origArea <= levelGoalSizePercentage)
                                    {
                                        nextLevel = true;
                                        pointsTotal += level;
                                        level++;
                                    }
                                }
                                break;
                        }
                    }
                }
            }
            
            // if the breathing rate is <6.0 or decreasing ball number will remain 1
            // if the breathing rate is >6.0 or increasing the ball number will be 2
            double instRR = GetComponent<BiofeedbackControl>().GetCurrentBR();
            balls = GameObject.FindGameObjectsWithTag("Ball");
            if ((instRR < 6.0f || getIsBRdecreasing() ))
            {
                int number = numberofBallsinPoly();
                if (number > 1) {
                    for (int i = 1; i < balls.Length; i++)
                    {
                        Debug.Log("destroying ball");
                        //ParticleSystem pSys = balls[i].GetComponent<ParticleSystem>();
                        Destroy(balls[i]);
                    }
                }
                
            }
            else if ((instRR > 6.0f || !getIsBRdecreasing()) ) {
                int number = numberofBallsinPoly();
                if (number <= 1)
                {
                    Instantiate(Resources.Load("ball"), transform.position, Quaternion.identity);
                    ScaleBalls();
                }
            }
            

            
            //Ball deletion
            balls = GameObject.FindGameObjectsWithTag("Ball");
            for (int i = 0; i < balls.Length; i++)
                if (!PolyContains((Vector2)balls[i].transform.position))
                {
                    //ParticleSystem pSys = balls[i].GetComponent<ParticleSystem>();
                    //pSys.Emit(1);
                    Destroy(balls[i]);
                }

            /*
            if (ballpos.Length == 1)
            {
                if (!PolyContains(ballpos[0]))
                {
                    mainBalls[0] = (GameObject)Instantiate(Resources.Load("ball"), transform.position, Quaternion.identity);
                    ScaleBalls();
                }
            }  
            */
            int ballNumber = numberofBallsinPoly();
            if (ballNumber < 1) {
                for (int i = 0; i < balls.Length; i++) {
                    Destroy(balls[i]);
                }
                
                GameObject ball = (GameObject)Instantiate(Resources.Load("ball"), transform.position, Quaternion.identity);
                ScaleBalls();
            }

        }
        
        // Next Level updates
        if (nextLevel)
        {
            ScaleField();
            if (Vector2.Distance(realCenter, (Vector2)levelCenter) < Vector2.kEpsilon)
            {
                origArea = PolygonArea(fieldColl);
                GameObject mainBall = GameObject.FindGameObjectWithTag("Ball");
                /*
                double currRR = GetComponent<BiofeedbackControl>().GetCurrentBR();
                if (currRR > 6.0f) {
                    Instantiate(Resources.Load("ball"), mainBall.transform.position, Quaternion.identity);
                }
                */
                ScaleBalls();
                balls = GameObject.FindGameObjectsWithTag("Ball");

                nextLevel = false;
            }
            score = 0;
        }

        //Score Updating
        percentage = ((PolygonArea(fieldColl) / origArea) - levelGoalSizePercentage) / (1.0f - levelGoalSizePercentage);
        score = percentage * scoreBarWidth;


    }

    int numberofBallsinPoly() {
        GameObject[] balls = GameObject.FindGameObjectsWithTag("Ball");
        int number = 0;
        for (int i = 0; i < balls.Length; i++) {
            if (PolyContains((Vector2)balls[i].transform.position)) {
                number = number + 1;
            }
        }
        return number;
    }
    //Removes the piece from the field and resets related variables
    void RemovePiece()
    {
        gameObject.GetComponent<LineRenderer>().positionCount = 0;
        pieces.Remove(currCastingColl);
        casting = false;
        Destroy(currCastingColl.gameObject);

        castDist1 = 0;
        castDist2 = 0;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (!pieces.Contains(collider) && (collider.gameObject.layer == 11))
        {
            pieces.Add(collider);
            //Debug.Log ("Added");
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (pieces.Contains(collider))
        {
            pieces.Remove(collider);
            //Debug.Log ("Removed");
        }
    }


    // Cuts the field based off piece type
    void Chop(Collider2D collider)
    {
        Vector2 piecePosition = new Vector2(collider.transform.position.x, collider.transform.position.y);
        Vector3 fieldCenter = transform.position;
        if (collider.gameObject.layer == 11 && !Input.GetMouseButton(0))
        {
            string pieceType = collider.tag;

            GameObject mainBall = GameObject.FindGameObjectWithTag("Ball");
            Vector3 ballPos = mainBall.transform.position;

            switch (pieceType)
            {
                case "horizontal":
                    int castInd1 = 0;
                    int castInd2 = 0;
                    ArrayList pointList = new ArrayList();
                    ArrayList poly1 = new ArrayList();
                    ArrayList poly2 = new ArrayList();

                    for (int i = 0; i < fieldColl.points.Length; i++)
                    {
                        pointList.Add(fieldColl.points[i]);
                    }

                    for (int i = 0; i < pointList.Count; i++)
                    {
                        if (Mathf.Abs(Vector2.Distance((Vector2)pointList[i], (Vector2)pointList[(i + 1) % pointList.Count]) -
                            Vector2.Distance((Vector2)pointList[i], FindRightIntersect(piecePosition)) -
                            Vector2.Distance(FindRightIntersect(piecePosition), (Vector2)pointList[(i + 1) % pointList.Count])) < floatingPointError)
                        {
                            pointList.Insert(i + 1, FindRightIntersect(piecePosition));
                            castInd1 = i + 1;
                            i++;
                        }
                    }
                    for (int i = 0; i < pointList.Count; i++)
                    {
                        if (Mathf.Abs(Vector2.Distance((Vector2)pointList[i], (Vector2)pointList[(i + 1) % pointList.Count]) -
                            Vector2.Distance((Vector2)pointList[i], FindLeftIntersect(piecePosition)) -
                            Vector2.Distance(FindLeftIntersect(piecePosition), (Vector2)pointList[(i + 1) % pointList.Count])) < floatingPointError)
                        {
                            pointList.Insert(i + 1, FindLeftIntersect(piecePosition));
                            castInd2 = i + 1;
                            if (i + 1 < castInd1)
                                castInd1++;
                            i++;
                        }
                    }

                    int tempInd = castInd1;
                    while (tempInd % pointList.Count != (castInd2 + 1) % pointList.Count)
                    {
                        poly1.Add(pointList[tempInd % pointList.Count]);
                        tempInd++;
                    }
                    tempInd = castInd2;
                    while (tempInd % pointList.Count != (castInd1 + 1) % pointList.Count)
                    {
                        poly2.Add(pointList[tempInd % pointList.Count]);
                        tempInd++;
                    }

                    Vector2 localBallPos = (Vector2)fieldColl.transform.InverseTransformPoint(ballPos);
                    Vector2 localPiecePos = (Vector2)fieldColl.transform.InverseTransformPoint((Vector3)piecePosition);
                    Vector2[] newPoints = new Vector2[1];
                    if (localBallPos.y > localPiecePos.y)
                    {
                        newPoints = new Vector2[poly2.Count];
                        for (int i = 0; i < newPoints.Length; i++)
                        {
                            newPoints[i] = (Vector2)poly2[i];
                        }
                    }
                    else
                    {
                        newPoints = new Vector2[poly1.Count];
                        for (int i = 0; i < newPoints.Length; i++)
                        {
                            newPoints[i] = (Vector2)poly1[i];
                        }
                    }

                    fieldColl.points = newPoints;

                    break;
                case "vertical":
                    castInd1 = 0;
                    castInd2 = 0;
                    pointList = new ArrayList();
                    poly1 = new ArrayList();
                    poly2 = new ArrayList();

                    for (int i = 0; i < fieldColl.points.Length; i++)
                    {
                        pointList.Add(fieldColl.points[i]);
                    }

                    for (int i = 0; i < pointList.Count; i++)
                    {
                        if (Mathf.Abs(Vector2.Distance((Vector2)pointList[i], (Vector2)pointList[(i + 1) % pointList.Count]) -
                            Vector2.Distance((Vector2)pointList[i], FindUpIntersect(piecePosition)) -
                            Vector2.Distance(FindUpIntersect(piecePosition), (Vector2)pointList[(i + 1) % pointList.Count])) < floatingPointError)
                        {
                            pointList.Insert(i + 1, FindUpIntersect(piecePosition));
                            castInd1 = i + 1;
                            i++;
                        }
                    }
                    for (int i = 0; i < pointList.Count; i++)
                    {
                        if (Mathf.Abs(Vector2.Distance((Vector2)pointList[i], (Vector2)pointList[(i + 1) % pointList.Count]) -
                            Vector2.Distance((Vector2)pointList[i], FindDownIntersect(piecePosition)) -
                            Vector2.Distance(FindDownIntersect(piecePosition), (Vector2)pointList[(i + 1) % pointList.Count])) < floatingPointError)
                        {
                            pointList.Insert(i + 1, FindDownIntersect(piecePosition));
                            castInd2 = i + 1;
                            if (i + 1 < castInd1)
                                castInd1++;
                            i++;
                        }
                    }

                    tempInd = castInd1;
                    while (tempInd % pointList.Count != (castInd2 + 1) % pointList.Count)
                    {
                        poly1.Add(pointList[tempInd % pointList.Count]);
                        tempInd++;
                    }
                    tempInd = castInd2;
                    while (tempInd % pointList.Count != (castInd1 + 1) % pointList.Count)
                    {
                        poly2.Add(pointList[tempInd % pointList.Count]);
                        tempInd++;
                    }

                    localBallPos = (Vector2)fieldColl.transform.InverseTransformPoint(ballPos);
                    localPiecePos = (Vector2)fieldColl.transform.InverseTransformPoint((Vector3)piecePosition);
                    newPoints = new Vector2[1];
                    if (localBallPos.x < localPiecePos.x)
                    {
                        newPoints = new Vector2[poly2.Count];
                        for (int i = 0; i < newPoints.Length; i++)
                        {
                            newPoints[i] = (Vector2)poly2[i];
                        }
                    }
                    else
                    {
                        newPoints = new Vector2[poly1.Count];
                        for (int i = 0; i < newPoints.Length; i++)
                        {
                            newPoints[i] = (Vector2)poly1[i];
                        }
                    }

                    fieldColl.points = newPoints;
                    break;
                case "top_left":
                    castInd1 = 0;
                    castInd2 = 0;
                    pointList = new ArrayList();
                    poly1 = new ArrayList();
                    poly2 = new ArrayList();

                    for (int i = 0; i < fieldColl.points.Length; i++)
                    {
                        pointList.Add(fieldColl.points[i]);
                    }

                    for (int i = 0; i < pointList.Count; i++)
                    {
                        if (Mathf.Abs(Vector2.Distance((Vector2)pointList[i], (Vector2)pointList[(i + 1) % pointList.Count]) -
                            Vector2.Distance((Vector2)pointList[i], FindUpIntersect(piecePosition)) -
                            Vector2.Distance(FindUpIntersect(piecePosition), (Vector2)pointList[(i + 1) % pointList.Count])) < floatingPointError)
                        {
                            pointList.Insert(i + 1, FindUpIntersect(piecePosition));
                            castInd1 = i + 1;
                            i++;
                        }
                    }
                    for (int i = 0; i < pointList.Count; i++)
                    {
                        if (Mathf.Abs(Vector2.Distance((Vector2)pointList[i], (Vector2)pointList[(i + 1) % pointList.Count]) -
                            Vector2.Distance((Vector2)pointList[i], FindLeftIntersect(piecePosition)) -
                            Vector2.Distance(FindLeftIntersect(piecePosition), (Vector2)pointList[(i + 1) % pointList.Count])) < floatingPointError)
                        {
                            pointList.Insert(i + 1, FindLeftIntersect(piecePosition));
                            castInd2 = i + 1;
                            if (i + 1 < castInd1)
                                castInd1++;
                            i++;
                        }
                    }

                    localPiecePos = (Vector2)fieldColl.transform.InverseTransformPoint((Vector3)piecePosition);

                    tempInd = castInd1;
                    while (tempInd % pointList.Count != (castInd2 + 1) % pointList.Count)
                    {
                        poly1.Add(pointList[tempInd % pointList.Count]);
                        tempInd++;
                    }
                    poly1.Add(localPiecePos);
                    tempInd = castInd2;
                    while (tempInd % pointList.Count != (castInd1 + 1) % pointList.Count)
                    {
                        poly2.Add(pointList[tempInd % pointList.Count]);
                        tempInd++;
                    }
                    poly2.Add(localPiecePos);

                    localBallPos = (Vector2)fieldColl.transform.InverseTransformPoint(ballPos);

                    newPoints = new Vector2[1];
                    if ((localBallPos.x > localPiecePos.x) || (localBallPos.y < localPiecePos.y))
                    {
                        newPoints = new Vector2[poly1.Count];
                        for (int i = 0; i < newPoints.Length; i++)
                        {
                            newPoints[i] = (Vector2)poly1[i];
                        }
                    }
                    else
                    {
                        newPoints = new Vector2[poly2.Count];
                        for (int i = 0; i < newPoints.Length; i++)
                        {
                            newPoints[i] = (Vector2)poly2[i];
                        }
                    }
                    fieldColl.points = newPoints;
                    break;
                case "top_right":
                    castInd1 = 0;
                    castInd2 = 0;
                    pointList = new ArrayList();
                    poly1 = new ArrayList();
                    poly2 = new ArrayList();

                    for (int i = 0; i < fieldColl.points.Length; i++)
                    {
                        pointList.Add(fieldColl.points[i]);
                    }

                    for (int i = 0; i < pointList.Count; i++)
                    {
                        if (Mathf.Abs(Vector2.Distance((Vector2)pointList[i], (Vector2)pointList[(i + 1) % pointList.Count]) -
                            Vector2.Distance((Vector2)pointList[i], FindUpIntersect(piecePosition)) -
                            Vector2.Distance(FindUpIntersect(piecePosition), (Vector2)pointList[(i + 1) % pointList.Count])) < floatingPointError)
                        {
                            pointList.Insert(i + 1, FindUpIntersect(piecePosition));
                            castInd1 = i + 1;
                            i++;
                        }
                    }
                    for (int i = 0; i < pointList.Count; i++)
                    {
                        if (Mathf.Abs(Vector2.Distance((Vector2)pointList[i], (Vector2)pointList[(i + 1) % pointList.Count]) -
                            Vector2.Distance((Vector2)pointList[i], FindRightIntersect(piecePosition)) -
                            Vector2.Distance(FindRightIntersect(piecePosition), (Vector2)pointList[(i + 1) % pointList.Count])) < floatingPointError)
                        {
                            pointList.Insert(i + 1, FindRightIntersect(piecePosition));
                            castInd2 = i + 1;
                            if (i + 1 < castInd1)
                                castInd1++;
                            i++;
                        }
                    }

                    localPiecePos = (Vector2)fieldColl.transform.InverseTransformPoint((Vector3)piecePosition);

                    tempInd = castInd1;
                    while (tempInd % pointList.Count != (castInd2 + 1) % pointList.Count)
                    {
                        poly1.Add(pointList[tempInd % pointList.Count]);
                        tempInd++;
                    }
                    poly1.Add(localPiecePos);
                    tempInd = castInd2;
                    while (tempInd % pointList.Count != (castInd1 + 1) % pointList.Count)
                    {
                        poly2.Add(pointList[tempInd % pointList.Count]);
                        tempInd++;
                    }
                    poly2.Add(localPiecePos);

                    localBallPos = (Vector2)fieldColl.transform.InverseTransformPoint(ballPos);

                    newPoints = new Vector2[1];
                    if ((localBallPos.x < localPiecePos.x) || (localBallPos.y < localPiecePos.y))
                    {
                        newPoints = new Vector2[poly2.Count];
                        for (int i = 0; i < newPoints.Length; i++)
                        {
                            newPoints[i] = (Vector2)poly2[i];
                        }
                    }
                    else
                    {
                        newPoints = new Vector2[poly1.Count];
                        for (int i = 0; i < newPoints.Length; i++)
                        {
                            newPoints[i] = (Vector2)poly1[i];
                        }
                    }

                    fieldColl.points = newPoints;
                    break;
                case "bottom_right":
                    castInd1 = 0;
                    castInd2 = 0;
                    pointList = new ArrayList();
                    poly1 = new ArrayList();
                    poly2 = new ArrayList();

                    for (int i = 0; i < fieldColl.points.Length; i++)
                    {
                        pointList.Add(fieldColl.points[i]);
                    }

                    for (int i = 0; i < pointList.Count; i++)
                    {
                        if (Mathf.Abs(Vector2.Distance((Vector2)pointList[i], (Vector2)pointList[(i + 1) % pointList.Count]) -
                            Vector2.Distance((Vector2)pointList[i], FindDownIntersect(piecePosition)) -
                            Vector2.Distance(FindDownIntersect(piecePosition), (Vector2)pointList[(i + 1) % pointList.Count])) < floatingPointError)
                        {
                            pointList.Insert(i + 1, FindDownIntersect(piecePosition));
                            castInd1 = i + 1;
                            i++;
                        }
                    }
                    for (int i = 0; i < pointList.Count; i++)
                    {
                        if (Mathf.Abs(Vector2.Distance((Vector2)pointList[i], (Vector2)pointList[(i + 1) % pointList.Count]) -
                            Vector2.Distance((Vector2)pointList[i], FindRightIntersect(piecePosition)) -
                            Vector2.Distance(FindRightIntersect(piecePosition), (Vector2)pointList[(i + 1) % pointList.Count])) < floatingPointError)
                        {
                            pointList.Insert(i + 1, FindRightIntersect(piecePosition));
                            if (i + 1 < castInd1)
                                castInd1++;
                            castInd2 = i + 1;
                            i++;
                        }
                    }

                    localPiecePos = (Vector2)fieldColl.transform.InverseTransformPoint((Vector3)piecePosition);
                    tempInd = castInd1;
                    while (tempInd % pointList.Count != (castInd2 + 1) % pointList.Count)
                    {
                        poly1.Add(pointList[tempInd % pointList.Count]);
                        tempInd++;
                    }
                    poly1.Add(localPiecePos);
                    tempInd = castInd2;
                    while (tempInd % pointList.Count != (castInd1 + 1) % pointList.Count)
                    {
                        poly2.Add(pointList[tempInd % pointList.Count]);
                        tempInd++;
                    }
                    poly2.Add(localPiecePos);

                    localBallPos = (Vector2)fieldColl.transform.InverseTransformPoint(ballPos);

                    newPoints = new Vector2[1];
                    if ((localBallPos.x < localPiecePos.x) || (localBallPos.y > localPiecePos.y))
                    {
                        newPoints = new Vector2[poly1.Count];
                        for (int i = 0; i < newPoints.Length; i++)
                        {
                            newPoints[i] = (Vector2)poly1[i];
                        }
                    }
                    else
                    {
                        newPoints = new Vector2[poly2.Count];
                        for (int i = 0; i < newPoints.Length; i++)
                        {
                            newPoints[i] = (Vector2)poly2[i];
                        }
                    }
                    fieldColl.points = newPoints;
                    break;
                case "bottom_left":
                    castInd1 = 0;
                    castInd2 = 0;
                    pointList = new ArrayList();
                    poly1 = new ArrayList();
                    poly2 = new ArrayList();

                    for (int i = 0; i < fieldColl.points.Length; i++)
                    {
                        pointList.Add(fieldColl.points[i]);
                    }

                    for (int i = 0; i < pointList.Count; i++)
                    {
                        if (Mathf.Abs(Vector2.Distance((Vector2)pointList[i], (Vector2)pointList[(i + 1) % pointList.Count]) -
                            Vector2.Distance((Vector2)pointList[i], FindDownIntersect(piecePosition)) -
                            Vector2.Distance(FindDownIntersect(piecePosition), (Vector2)pointList[(i + 1) % pointList.Count])) < floatingPointError)
                        {
                            pointList.Insert(i + 1, FindDownIntersect(piecePosition));
                            castInd1 = i + 1;
                            if (i + 1 < castInd1)
                                castInd1++;
                            i++;
                        }
                    }
                    for (int i = 0; i < pointList.Count; i++)
                    {
                        if (Mathf.Abs(Vector2.Distance((Vector2)pointList[i], (Vector2)pointList[(i + 1) % pointList.Count]) -
                            Vector2.Distance((Vector2)pointList[i], FindLeftIntersect(piecePosition)) -
                            Vector2.Distance(FindLeftIntersect(piecePosition), (Vector2)pointList[(i + 1) % pointList.Count])) < floatingPointError)
                        {
                            pointList.Insert(i + 1, FindLeftIntersect(piecePosition));
                            castInd2 = i + 1;
                            if (i + 1 < castInd1)
                                castInd1++;
                            i++;
                        }
                    }

                    localPiecePos = (Vector2)fieldColl.transform.InverseTransformPoint((Vector3)piecePosition);

                    tempInd = castInd1;
                    while (tempInd % pointList.Count != (castInd2 + 1) % pointList.Count)
                    {
                        poly1.Add(pointList[tempInd % pointList.Count]);
                        tempInd++;
                    }
                    poly1.Add(localPiecePos);
                    tempInd = castInd2;
                    while (tempInd % pointList.Count != (castInd1 + 1) % pointList.Count)
                    {
                        poly2.Add(pointList[tempInd % pointList.Count]);
                        tempInd++;
                    }
                    poly2.Add(localPiecePos);

                    localBallPos = (Vector2)fieldColl.transform.InverseTransformPoint(ballPos);

                    newPoints = new Vector2[1];
                    if ((localBallPos.x > localPiecePos.x) || (localBallPos.y > localPiecePos.y))
                    {
                        newPoints = new Vector2[poly2.Count];
                        for (int i = 0; i < newPoints.Length; i++)
                        {
                            newPoints[i] = (Vector2)poly2[i];
                        }
                    }
                    else
                    {
                        newPoints = new Vector2[poly1.Count];
                        for (int i = 0; i < newPoints.Length; i++)
                        {
                            newPoints[i] = (Vector2)poly1[i];
                        }
                    }

                    fieldColl.points = newPoints;
                    break;


            }
        }

        GetComponentInChildren<SetColliders>().ResetMesh();
    }

    // Rescales the field to resize the shape into the field dimensions
    void ScaleField()
    {
        balls = GameObject.FindGameObjectsWithTag("Ball");
        Vector2[] ballDistRatios = new Vector2[balls.Length];
        int[] closestCornerInds = new int[balls.Length];

        Vector2[] newPoints = new Vector2[fieldColl.points.Length];
        int numPoints = fieldColl.points.Length;
        float centerX = 0;
        float centerY = 0;
        float minX = float.PositiveInfinity;
        float maxX = float.NegativeInfinity;
        float minY = float.PositiveInfinity;
        float maxY = float.NegativeInfinity;

        for (int i = 0; i < numPoints; i++)
        {
            if (fieldColl.points[i].x < minX)
                minX = fieldColl.points[i].x;
            if (fieldColl.points[i].y < minY)
                minY = fieldColl.points[i].y;
            if (fieldColl.points[i].x > maxX)
                maxX = fieldColl.points[i].x;
            if (fieldColl.points[i].y > maxY)
                maxY = fieldColl.points[i].y;
        }

        float polyWidth = maxX - minX;
        float polyHeight = maxY - minY;

        for (int i = 0; i < balls.Length; i++)
        {
            balls[i].GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
            int closestCorner = ClosestCornerInd(fieldColl, balls[i].transform.position);
            closestCornerInds[i] = closestCorner;
            float xFromClose = balls[i].transform.position.x -
                               fieldColl.transform.TransformPoint((Vector3)fieldColl.points[closestCorner]).x;
            float yFromClose = balls[i].transform.position.y -
                               fieldColl.transform.TransformPoint((Vector3)fieldColl.points[closestCorner]).y;
            ballDistRatios[i] = new Vector2(xFromClose / polyWidth, yFromClose / polyHeight);
        }

        float scale = (maxX - minX) > (maxY - minY) ? levelSize.x / (maxX - minX) : levelSize.y / (maxY - minY);
        //gameObject.GetComponent<SpriteRenderer> ().size = fieldSprite.size * scale;
        for (int i = 0; i < startPoints.Length; i++)
        {
            newPoints[i] = Vector2.Lerp(startPoints[i],
                fieldColl.points[i] * scale, (Time.time - startTime) / 1f);
        }
        //fieldColl.points = newPoints;


        //Movement of field
        centerX = (minX + maxX) / 2f;
        centerY = (minY + maxY) / 2f;
        realCenter = (Vector2)fieldColl.transform.TransformPoint(new Vector3(centerX, centerY, 0));
        float deltaX = levelCenter.x - realCenter.x;
        float deltaY = levelCenter.y - realCenter.y;

        //newPoints = new Vector2[fieldColl.points.Length];
        for (int i = 0; i < numPoints; i++)
        {
            newPoints[i] = Vector2.Lerp(startPoints[i], new Vector2(newPoints[i].x + deltaX, newPoints[i].y + deltaY), (Time.time - startTime) / 1f);
        }
        fieldColl.points = newPoints;

        minX = float.PositiveInfinity;
        maxX = float.NegativeInfinity;
        minY = float.PositiveInfinity;
        maxY = float.NegativeInfinity;

        for (int i = 0; i < numPoints; i++)
        {
            if (fieldColl.points[i].x < minX)
                minX = fieldColl.points[i].x;
            if (fieldColl.points[i].y < minY)
                minY = fieldColl.points[i].y;
            if (fieldColl.points[i].x > maxX)
                maxX = fieldColl.points[i].x;
            if (fieldColl.points[i].y > maxY)
                maxY = fieldColl.points[i].y;
        }
        polyWidth = maxX - minX;
        polyHeight = maxY - minY;
        for (int i = 0; i < balls.Length; i++)
        {
            float xFromClose = ballDistRatios[i].x * polyWidth;
            float yFromClose = ballDistRatios[i].y * polyHeight;
            Vector3 newBallPos = new Vector3(
                                     xFromClose + fieldColl.transform.TransformPoint((Vector3)fieldColl.points[closestCornerInds[i]]).x,
                                     yFromClose + fieldColl.transform.TransformPoint((Vector3)fieldColl.points[closestCornerInds[i]]).y);
            balls[i].transform.position = newBallPos;
        }


        GetComponentInChildren<SetColliders>().ResetMesh();
    }

    void ScaleBalls()
    {
        //Instantiate (Resources.Load ("ball"), transform.position, Quaternion.identity);
        balls = GameObject.FindGameObjectsWithTag("Ball");
        if (balls.Length > 0)
            for (int i = 0; i < balls.Length; i++)
            {
                GameObject ball = (GameObject)balls[i];
                //ball.transform.position = transform.position;
                ball.transform.localScale = (ballFieldAreaRatio * PolygonArea(fieldColl)) + halfStartingBallScale;
            }
    }

    //returns the index of the closest corner of the polygon collider to the position given
    int ClosestCornerInd(PolygonCollider2D polyColl, Vector3 pos)
    {
        float minDist = Mathf.Infinity;
        int minInd = 0;

        pos = polyColl.transform.InverseTransformPoint(pos);

        for (int i = 0; i < polyColl.points.Length; i++)
        {
            if (Vector2.Distance((Vector2)pos, polyColl.points[i]) < minDist)
            {
                minInd = i;
                minDist = Vector2.Distance((Vector2)pos, polyColl.points[i]);
            }
        }

        return minInd;
    }

    //Only for non self-intersecting polygons
    float PolygonArea(PolygonCollider2D polyColl)
    {
        int numPoints = polyColl.points.Length;
        float[] x = new float[numPoints];
        float[] y = new float[numPoints];

        for (int i = 0; i < numPoints; i++)
        {
            x[i] = polyColl.points[i].x;
            y[i] = polyColl.points[i].y;
        }

        float area = 0f;
        int j = numPoints - 1;

        for (int i = 0; i < numPoints; i++)
        {
            area = area + (x[j] + x[i]) * (y[j] - y[i]);
            j = i;
        }

        return area / 2f;
    }

    Vector2 FindRightIntersect(Vector2 pos)
    {
        ArrayList pairs = new ArrayList();

        //Creates array of points of the polygon with world points
        Vector2[] realPoints = new Vector2[fieldColl.points.Length];
        for (int i = 0; i < realPoints.Length; i++)
            realPoints[i] = (Vector2)fieldColl.transform.TransformPoint((Vector3)fieldColl.points[i]);

        //Finds all pairs of points that are both on the right side of the piece and that also have the same x-value
        for (int i = 0; i < realPoints.Length; i++)
        {
            if ((realPoints[i].x > pos.x) && (realPoints[(i + 1) % realPoints.Length].x > pos.x))
            {
                if (Mathf.Abs(realPoints[i].x - realPoints[(i + 1) % realPoints.Length].x) < floatingPointError)
                {
                    pairs.Add(new Vector2[2] { realPoints[i], realPoints[(i + 1) % realPoints.Length] });
                }
            }
        }

        //Removes pairs of points that don't contain the y-value of pos and find the closest pair to pos
        int index = 0;
        int minInd = 0;
        float minDist = float.PositiveInfinity;

        while (index < pairs.Count)
        {
            Vector2 p1 = ((Vector2[])pairs[index])[0];
            Vector2 p2 = ((Vector2[])pairs[index])[1];

            if (p1.y > p2.y)
            {
                if ((p1.y < pos.y) || (p2.y > pos.y))
                {
                    pairs.RemoveAt(index);
                    index--;
                }
                else
                {
                    if (Mathf.Abs(p1.x - pos.x) < minDist)
                    {
                        minDist = Mathf.Abs(p1.x - pos.x);
                        minInd = index;
                    }
                }
            }
            else
            {
                if ((p2.y < pos.y) || (p1.y > pos.y))
                {
                    pairs.RemoveAt(index);
                    index--;
                }
                else
                {
                    if (Mathf.Abs(p1.x - pos.x) < minDist)
                    {
                        minDist = Mathf.Abs(p1.x - pos.x);
                        minInd = index;
                    }
                }
            }

            index++;
        }

        //Sets the intersection point and turns it back into local space
        Vector2 returnPoint = new Vector2(((Vector2[])pairs[minInd])[0].x, pos.y);
        returnPoint = (Vector2)fieldColl.transform.InverseTransformPoint((Vector3)returnPoint);
        return returnPoint;
    }

    Vector2 FindLeftIntersect(Vector2 pos)
    {
        ArrayList pairs = new ArrayList();

        //Creates array of points of the polygon with world points
        Vector2[] realPoints = new Vector2[fieldColl.points.Length];
        for (int i = 0; i < realPoints.Length; i++)
            realPoints[i] = (Vector2)fieldColl.transform.TransformPoint((Vector3)fieldColl.points[i]);

        //Finds all pairs of points that are both on the left side of the piece and that also have the same x-value
        for (int i = 0; i < realPoints.Length; i++)
        {
            if ((realPoints[i].x < pos.x) && (realPoints[(i + 1) % realPoints.Length].x < pos.x))
            {
                if (Mathf.Abs(realPoints[i].x - realPoints[(i + 1) % realPoints.Length].x) < floatingPointError)
                {
                    pairs.Add(new Vector2[2] { realPoints[i], realPoints[(i + 1) % realPoints.Length] });
                }
            }
        }

        //Removes pairs of points that don't contain the y-value of pos and find the closest pair to pos
        int index = 0;
        int minInd = 0;
        float minDist = float.PositiveInfinity;

        while (index < pairs.Count)
        {
            Vector2 p1 = ((Vector2[])pairs[index])[0];
            Vector2 p2 = ((Vector2[])pairs[index])[1];

            if (p1.y > p2.y)
            {
                if ((p1.y < pos.y) || (p2.y > pos.y))
                {
                    pairs.RemoveAt(index);
                    index--;
                }
                else
                {
                    if (Mathf.Abs(p1.x - pos.x) < minDist)
                    {
                        minDist = Mathf.Abs(p1.x - pos.x);
                        minInd = index;
                    }
                }
            }
            else
            {
                if ((p2.y < pos.y) || (p1.y > pos.y))
                {
                    pairs.RemoveAt(index);
                    index--;
                }
                else
                {
                    if (Mathf.Abs(p1.x - pos.x) < minDist)
                    {
                        minDist = Mathf.Abs(p1.x - pos.x);
                        minInd = index;
                    }
                }
            }

            index++;
        }

        //Sets the intersection point and turns it back into local space
        Vector2 returnPoint = new Vector2(((Vector2[])pairs[minInd])[0].x, pos.y);
        returnPoint = (Vector2)fieldColl.transform.InverseTransformPoint((Vector3)returnPoint);
        return returnPoint;
    }

    Vector2 FindUpIntersect(Vector2 pos)
    {
        ArrayList pairs = new ArrayList();

        //Creates array of points of the polygon with world points
        Vector2[] realPoints = new Vector2[fieldColl.points.Length];
        for (int i = 0; i < realPoints.Length; i++)
            realPoints[i] = (Vector2)fieldColl.transform.TransformPoint((Vector3)fieldColl.points[i]);

        //Finds all pairs of points that are both above the piece and that also have the same y-value
        for (int i = 0; i < realPoints.Length; i++)
        {
            if ((realPoints[i].y > pos.y) && (realPoints[(i + 1) % realPoints.Length].y > pos.y))
            {
                if (Mathf.Abs(realPoints[i].y - realPoints[(i + 1) % realPoints.Length].y) < floatingPointError)
                {
                    pairs.Add(new Vector2[2] { realPoints[i], realPoints[(i + 1) % realPoints.Length] });
                }
            }
        }

        //Removes pairs of points that don't contain the x-value of pos and find the closest pair to pos
        int index = 0;
        int minInd = 0;
        float minDist = float.PositiveInfinity;

        while (index < pairs.Count)
        {
            Vector2 p1 = ((Vector2[])pairs[index])[0];
            Vector2 p2 = ((Vector2[])pairs[index])[1];

            if (p1.x > p2.x)
            {
                if ((p1.x < pos.x) || (p2.x > pos.x))
                {
                    pairs.RemoveAt(index);
                    index--;
                }
                else
                {
                    if (Mathf.Abs(p1.y - pos.y) < minDist)
                    {
                        minDist = Mathf.Abs(p1.y - pos.y);
                        minInd = index;
                    }
                }
            }
            else
            {
                if ((p2.x < pos.x) || (p1.x > pos.x))
                {
                    pairs.RemoveAt(index);
                    index--;
                }
                else
                {
                    if (Mathf.Abs(p1.y - pos.y) < minDist)
                    {
                        minDist = Mathf.Abs(p1.y - pos.y);
                        minInd = index;
                    }
                }
            }

            index++;
        }

        //Sets the intersection point and turns it back into local space
        Vector2 returnPoint = new Vector2(pos.x, ((Vector2[])pairs[minInd])[0].y);
        returnPoint = (Vector2)fieldColl.transform.InverseTransformPoint((Vector3)returnPoint);
        return returnPoint;
    }

    Vector2 FindDownIntersect(Vector2 pos)
    {
        ArrayList pairs = new ArrayList();

        //Creates array of points of the polygon with world points
        Vector2[] realPoints = new Vector2[fieldColl.points.Length];
        for (int i = 0; i < realPoints.Length; i++)
            realPoints[i] = (Vector2)fieldColl.transform.TransformPoint((Vector3)fieldColl.points[i]);

        //Finds all pairs of points that are both below the piece and that also have the same y-value
        for (int i = 0; i < realPoints.Length; i++)
        {
            if ((realPoints[i].y < pos.y) && (realPoints[(i + 1) % realPoints.Length].y < pos.y))
            {
                if (Mathf.Abs(realPoints[i].y - realPoints[(i + 1) % realPoints.Length].y) < floatingPointError)
                {
                    pairs.Add(new Vector2[2] { realPoints[i], realPoints[(i + 1) % realPoints.Length] });
                }
            }
        }

        //Removes pairs of points that don't contain the x-value of pos and find the closest pair to pos
        int index = 0;
        int minInd = 0;
        float minDist = float.PositiveInfinity;

        while (index < pairs.Count)
        {
            Vector2 p1 = ((Vector2[])pairs[index])[0];
            Vector2 p2 = ((Vector2[])pairs[index])[1];

            if (p1.x > p2.x)
            {
                if ((p1.x < pos.x) || (p2.x > pos.x))
                {
                    pairs.RemoveAt(index);
                    index--;
                }
                else
                {
                    if (Mathf.Abs(p1.y - pos.y) < minDist)
                    {
                        minDist = Mathf.Abs(p1.y - pos.y);
                        minInd = index;
                    }
                }
            }
            else
            {
                if ((p2.x < pos.x) || (p1.x > pos.x))
                {
                    pairs.RemoveAt(index);
                    index--;
                }
                else
                {
                    if (Mathf.Abs(p1.y - pos.y) < minDist)
                    {
                        minDist = Mathf.Abs(p1.y - pos.y);
                        minInd = index;
                    }
                }
            }

            index++;
        }

        //Sets the intersection point and turns it back into local space
        Vector2 returnPoint = new Vector2(pos.x, ((Vector2[])pairs[minInd])[0].y);
        returnPoint = (Vector2)fieldColl.transform.InverseTransformPoint((Vector3)returnPoint);
        return returnPoint;
    }

    bool PolyContains(Vector2 pos)
    {
        int numIntersects = 0;
        ArrayList pairs = new ArrayList();

        //Creates array of points of the polygon with world points
        Vector2[] realPoints = new Vector2[fieldColl.points.Length];
        for (int i = 0; i < realPoints.Length; i++)
            realPoints[i] = (Vector2)fieldColl.transform.TransformPoint((Vector3)fieldColl.points[i]);

        //Finds all pairs of points that are both on the left side of the piece and that also have the same x-value
        for (int i = 0; i < realPoints.Length; i++)
        {
            if ((realPoints[i].x < pos.x) && (realPoints[(i + 1) % realPoints.Length].x < pos.x))
            {
                if (Mathf.Abs(realPoints[i].x - realPoints[(i + 1) % realPoints.Length].x) < floatingPointError)
                {
                    pairs.Add(new Vector2[2] { realPoints[i], realPoints[(i + 1) % realPoints.Length] });
                }
            }
        }

        //Removes pairs of points that don't contain the y-value of pos and find the closest pair to pos
        int index = 0;

        while (index < pairs.Count)
        {
            Vector2 p1 = ((Vector2[])pairs[index])[0];
            Vector2 p2 = ((Vector2[])pairs[index])[1];

            if (p1.y > p2.y)
            {
                if ((p1.y < pos.y) || (p2.y > pos.y))
                {
                    pairs.RemoveAt(index);
                    index--;
                }
                else
                {
                    numIntersects++;
                }
            }
            else
            {
                if ((p2.y < pos.y) || (p1.y > pos.y))
                {
                    pairs.RemoveAt(index);
                    index--;
                }
                else
                {
                    numIntersects++;
                }
            }

            index++;
        }
        return numIntersects % 2 != 0;
    }

    GameObject FindObjectWithLayer(int layer)
    {
        GameObject[] objects = GameObject.FindObjectsOfType<GameObject>();
        if (objects.Length > 0)
            for (int i = 0; i < objects.Length; i++)
            {
                if (objects[i].layer == layer)
                    return objects[i];
            }
        return null;
    }

    public System.Boolean getIsBRdecreasing()
    {
        using (AndroidJavaClass cls_UnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            using (AndroidJavaObject obj_Activity = cls_UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
            {
                return obj_Activity.Call<System.Boolean>("getIsBRdecreasing");
            }
        }
        // return "50.0";
    }
}
