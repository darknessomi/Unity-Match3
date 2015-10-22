using UnityEngine;
using System.Collections;

public class LoseUI : MonoBehaviour
{
    public UnityEngine.UI.Text Score;   // current score

    public UnityEngine.UI.Text Best;    // best score

    private int playerScore;            // score tmp

    private int star;                   // star number

    void Start()
    {
        if (PLayerInfo.MODE != 1)
            playerScore = PLayerInfo.Info.Score + (PLayerInfo.MapPlayer.Level - 1) * 5000;
        else
            playerScore = PLayerInfo.Info.Score;
        // display score text
        Score.text = playerScore.ToString();
        // display best score text
        Best.text = getBestScore(playerScore).ToString();

    }

    /// <summary>
    /// compare score with best score
    /// </summary>
    /// <param name="score">score by player</param>
    /// <returns>best score</returns>
    int getBestScore(int score)
    {
        if (PLayerInfo.MODE != 1)
        {
            if (score > PLayerInfo.MapPlayer.HightScore)
            {
                PLayerInfo.MapPlayer.HightScore = score;
                PlayerPrefs.SetInt(PLayerInfo.KEY_CLASSIC_HISCORE, PLayerInfo.MapPlayer.HightScore);
            }
        }
        return PLayerInfo.MapPlayer.HightScore;
    }

}
