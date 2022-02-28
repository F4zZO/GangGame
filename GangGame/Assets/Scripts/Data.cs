using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Data
{
    public int selectedHat;
    public float[] selectedColor;
    public bool[] hasHats;
    public float[] highScore;

    public Data(GameManager gm)
    {
        this.selectedHat = gm.playerHat;

        this.selectedColor = new float[3];
        this.selectedColor[0] = gm.playerColor.r;
        this.selectedColor[1] = gm.playerColor.g;
        this.selectedColor[2] = gm.playerColor.b;

        this.hasHats = new bool[3];
        if(gm.hasHat == null)
        {
            this.hasHats[0] = false;
            this.hasHats[1] = false;
            this.hasHats[2] = false;
        }
        else
        {
            this.hasHats = gm.hasHat;
        }

        this.highScore = new float[3];
        if (gm.highScores == null)
        {
            this.highScore[0] = 0;
            this.highScore[1] = 0;
            this.highScore[2] = 0;
        }
        else
        {
            this.highScore = gm.highScores;
        }
    }

}
