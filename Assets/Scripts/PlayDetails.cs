using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayDetails
{
    private float startRR;
    private  float endRR;
    private  int levelsFinished;
    private string startTime;
    private string endTime;
    private int pointsTotal;
    public PlayDetails()
    {

    }

    public PlayDetails(float startRR, float endRR, int levelsFinished, string startTime, string endTime, int pointsTotal)
    {
        this.startRR = startRR;
        this.endRR = endRR;
        this.levelsFinished = levelsFinished;
        this.startTime = startTime;
        this.endTime = endTime;
        this.pointsTotal = pointsTotal;

    }

    public int LevelsFinished { get => levelsFinished; set => levelsFinished = value; }
    public float EndRR { get => endRR; set => endRR = value; }
    public float StartRR { get => startRR; set => startRR = value; }
    public string EndTime { get => endTime; set => endTime = value; }
    public string StartTime { get => startTime; set => startTime = value; }
    public int PointsTotal { get => pointsTotal; set => pointsTotal = value; }

    public override string ToString()
    {
        return "{" + "levelsFinished : " + levelsFinished + ", startRR: " + startRR + ", endRR: " + endRR +
                            ", startTIme: " + startTime + ", endTime: " + endTime + ", pointsTotal: " + pointsTotal + "}"; 
        
    }
}
