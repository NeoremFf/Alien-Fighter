using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;

public class GPSManager : MonoBehaviour
{
    private static bool successConnected = false;

    public static void AuthenticateUser()
    {
        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().Build();
        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.Activate();
        Social.localUser.Authenticate((bool success) =>
        {
            successConnected = success;
        });
    }

    public static void PostToLeaderBoard(long newScore)
    {
        Social.ReportScore(newScore, GPGSIds.leaderboard_high_score, (bool success) =>
        {
            if (success)
            {
                // do if leaderBoard update is success
                Debug.Log("LeaderBoard update is success!");
            }
            else
            {
                // do if leaderBoard update is not success
                Debug.LogError("LeaderBoard update is not success!");
            }
        });
    }

    public void ShowLeaderBoard()
    {
        PlayGamesPlatform.Instance.ShowLeaderboardUI(GPGSIds.leaderboard_high_score);
    }

    private void OnApplicationQuit()
    {
        PlayGamesPlatform.Instance.SignOut();
    }
}
