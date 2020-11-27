using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Startup : MonoBehaviour
{
    [SerializeField] GameObject loadingScreen;

    [Header("UI")]
    [SerializeField] GameObject dailyBonusButton;
    [SerializeField] GameObject shopButton;
    [SerializeField] GameObject leaderboardButton;

    private void Start()
    {
        loadingScreen.SetActive(true);

        Screen.orientation = ScreenOrientation.Portrait;

        GPSManager.AuthenticateUser();
        MonetizationSupport.InitializeAds();

        CheckDaily.CheckDayStrike();

        ScoreController.LoadClearCharge();

        if (!PlayerPrefs.HasKey("WasTutorial"))
        {
            dailyBonusButton.SetActive(false);
            shopButton.SetActive(false);
            leaderboardButton.SetActive(false);
        }
        else
        {
            dailyBonusButton.SetActive(PlayerPrefs.GetInt("DailyBonusEarned") == 1 ? false : true);
        }

        loadingScreen.SetActive(false);
    }
}
