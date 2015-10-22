using UnityEngine;
using System.Collections;

public class WinUI : MonoBehaviour {

    public GameObject[] Stargold;

    public UnityEngine.UI.Text Score;

    public UnityEngine.UI.Text TimeBonus;

    public UnityEngine.UI.Text Best;

    private int playerScore;

    private int star;

    void Start()
    {
        TimeBonus.text = ((int)Mathf.Abs(Timer.timer.GameTime)).ToString();

        playerScore = getGameScore(PLayerInfo.Info.Score,Timer.timer.GameTime);

        Score.text = playerScore.ToString();

        Best.text = getBestScore(playerScore).ToString();

        star = getGameStar(playerScore);

        StartCoroutine(StarAnimation(star));

        SaveData();

    }

    /// <summary>
    /// get best score
    /// </summary>
    /// <param name="score">current score</param>
    /// <returns>best score</returns>
    int getBestScore(int score)
    {
        if (score > PLayerInfo.MapPlayer.HightScore)
            PLayerInfo.MapPlayer.HightScore = score;

        return PLayerInfo.MapPlayer.HightScore;

    }

    /// <summary>
    /// calculate score
    /// </summary>
    /// <param name="playerscore">score</param>
    /// <param name="gametime">time</param>
    /// <returns>score when caculated</returns>
    int getGameScore(int playerscore, float gametime)
    {
        return playerscore + (int)Mathf.Abs(gametime) * 500;
    }

    /// <summary>
    /// caculate star number by score
    /// </summary>
    /// <param name="score">score</param>
    /// <returns>number of star</returns>
    int getGameStar(int score)
    {
        if (score >= 80000)
        {
            PLayerInfo.MapPlayer.Stars = 3;
            return 3;
        }
        else if (score >= 60000)
        {
            if (PLayerInfo.MapPlayer.Stars < 2)
                PLayerInfo.MapPlayer.Stars = 2;
            return 2;
        }
        else
        {
            PLayerInfo.MapPlayer.Stars = 1;
            return 1;
        }
    }


    /// <summary>
    /// animation star
    /// </summary>
    /// <param name="star">number of star</param>
    /// <returns></returns>
    IEnumerator StarAnimation(int star)
    {
        for (int i = 0; i < star ; i++)
        {
            Stargold[i].SetActive(true);
            yield return new WaitForSeconds(0.7f);
        }
    }


    /// <summary>
    /// sava data
    /// </summary>
    void SaveData()
    {
        int index = PLayerInfo.MapPlayer.Level - 1;
        DataLoader.MyData[index] = PLayerInfo.MapPlayer;
        if (PLayerInfo.MapPlayer.Level < 297)
            DataLoader.MyData[index + 1].Locked = false;  
        PlayerUtils p = new PlayerUtils();
        p.Save(DataLoader.MyData);
    }



}
