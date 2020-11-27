using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreController : MonoBehaviour
{
    [SerializeField] private Animator loseMenuAnim;
    [SerializeField] private Text scoreUI;
    [SerializeField] private Text scoreLoseUI;
    [SerializeField] private Text bestScoreUI;
    [SerializeField] private Text goldUI;

    public static long score { get; set; }
    private long bestScore;
    private long newBestScore;
    private int currentGoldAdd;
    private int lastScoreToAddGold;
    private int countAdd;

    private static Text helpScoreUI;

    // Consumable
    public static int s_chargeOfClear;

    private void Start()
    {
        helpScoreUI = scoreUI;

        score = 0;

        UpdateScoreUI();

        if (!PlayerPrefs.HasKey("Best_Score"))
        {
            PlayerPrefs.SetInt("Best_Score", 0);
            bestScore = PlayerPrefs.GetInt("Best_Score");
        }
        else
        {
            bestScore = PlayerPrefs.GetInt("Best_Score");
            newBestScore = bestScore;
        }

        if (!PlayerPrefs.HasKey("Gold"))
        {
            PlayerPrefs.SetInt("Gold", 0);
        }
    }

    public void UpdateUIAfterGame()
    {
        loseMenuAnim.Play("Lose");

        UpdateGoldUI();
        UpdateScoreAfterGame();
    }

    public static void SetScoreBck(long newScore)
    {
        score = newScore;
        helpScoreUI.text = score.ToString();
    }

    #region Score
    public void AddScore(int value)
    {
        score += value;
        UpdateScoreUI();

        lastScoreToAddGold += value;
        if (lastScoreToAddGold / 100 > 0)
        {
            AddGold();
            lastScoreToAddGold -= 100 * countAdd;
            countAdd = 0;
        }

        if (score > bestScore)
        {
            bestScore = score;
            PlayerPrefs.SetInt("Best_Score", (int)bestScore);
        }
    }

    private void UpdateScoreUI()
    {
        scoreUI.text = score.ToString();
    }

    private void UpdateScoreAfterGame()
    {
        bestScoreUI.text = bestScore.ToString();
        scoreLoseUI.text = score.ToString();

        //update LeaderBoard
        if (newBestScore < bestScore)
        {
            GPSManager.PostToLeaderBoard(bestScore);
        }
    }
    #endregion

    #region Gold
    public void AddGold()
    {
        countAdd = lastScoreToAddGold / 100;
        currentGoldAdd += countAdd;
        PlayerPrefs.SetInt("Gold", PlayerPrefs.GetInt("Gold") + currentGoldAdd);
    }

    public static void AddPurchasedGold(int addCount)
    {
        PlayerPrefs.SetInt("Gold", PlayerPrefs.GetInt("Gold") + addCount);
    }

    private void UpdateGoldUI()
    {
        goldUI.text = "+" + currentGoldAdd.ToString();
    }
    #endregion

    #region Consumable
    public static void LoadClearCharge()
    {
        s_chargeOfClear = PlayerPrefs.GetInt("Clear");
    }

    public static void AddClearCharge()
    {
        s_chargeOfClear++;
        PlayerPrefs.SetInt("Clear", s_chargeOfClear);
    }

    public static void RemoveClearCharge()
    {
        s_chargeOfClear--;
        PlayerPrefs.SetInt("Clear", s_chargeOfClear);
    }
    #endregion
}
