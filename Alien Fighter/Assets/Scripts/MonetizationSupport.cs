using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;

public class MonetizationSupport : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private static bool testMode = false;
    private static string gameId = "3519207";
    private static string regularPlacementId = "video";
    private static string rewardedVideoPlacementId = "rewardedVideo";
    private static string bannerPlacementId = "banner";

    [Header("Gold")]
    [SerializeField] private int countToAddGoldForAd;
    [SerializeField] private GameObject panelGetGoldAfterGold;

    private static int countToShowAd = 0;
    private static bool canShowAd = true;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public static void InitializeAds()
    {
        Advertisement.Initialize(gameId, testMode);

        if (Advertisement.isInitialized)
        {
            CheckRemoveAd();

            StartShowBanner();
        }
    }

    public static void CheckRemoveAd()
    {
        if (PurchaseManager.CheckBuyState("remove_ad"))
        {
            canShowAd = false;
            Debug.Log("<color=pink>Bought Remove Ads - dont show ads.</color>");
            Advertisement.Banner.Hide();
        }
    }

    private static void StartShowBanner()
    {
        if (canShowAd)
        {
            if (Advertisement.IsReady(bannerPlacementId))
            {
                ShowBannerAd();
            }
        }
        else
        {
            Debug.Log("<color=pink>Bought Remove Ads - dont show ads.</color>");
        }
    }

    #region ShowAds
    public void ShowRegularAd()
    {
        if (canShowAd)
        {
            countToShowAd++;
            if (countToShowAd >= 3)
            {
                Time.timeScale = 0;
                countToShowAd = 0;
                if (Advertisement.IsReady(regularPlacementId))
            {
                var showOptions = new ShowOptions
                {
                    resultCallback = HandleShowResult
                };
                Advertisement.Show(regularPlacementId, showOptions);
            }
            }
        }
        else
        {
            Debug.Log("<color=pink>Bought Remove Ads - dont show ads.</color>");
        }
    }

    public void ShowRewardedAd()
    {
        if (Advertisement.IsReady(rewardedVideoPlacementId))
            {
                var showOptions = new ShowOptions();
                showOptions.resultCallback = HandleShowResult;

                Advertisement.Show(rewardedVideoPlacementId, showOptions);
            }
    }
    public static void ShowBannerAd()
    {
        if (canShowAd)
        {
            if (Advertisement.IsReady(bannerPlacementId))
            {
                Advertisement.Banner.SetPosition(BannerPosition.BOTTOM_CENTER);
                Advertisement.Banner.Show(bannerPlacementId);
            }
        }
        else
        {
            Debug.Log("<color=pink>Bought Remove Ads - dont show ads.</color>");
        }
    }
    #endregion

    public static void HideBannerAd()
    {
        if (canShowAd)
        {
            Advertisement.Banner.Hide();
        }
    }

    private void HandleShowResult(ShowResult result)
    {
        switch (result)
        {
            case ShowResult.Finished:
                Debug.Log("<color=green>The ad was successfully shown.</color>");

                if (panelGetGoldAfterGold)
                {
                    ScoreController.AddPurchasedGold(countToAddGoldForAd);
                    SkinSelected.UpdateGoldUI();
                    panelGetGoldAfterGold.SetActive(true);
                }
                else if(HeroController.s_isAlive == false)
                {
                    HeroController.SetHealthToOne();
                    HeroController.s_isAlive = true;
                    HideBannerAd();
                    FindObjectOfType<Menu>().AnimHelpToAddHealth.Play("Home");
                }
                else
                {}

                Time.timeScale = 1;
                break;

            case ShowResult.Skipped:
                Debug.Log("<color=yellow>The ad was skipped before reaching the end.</color>");
                Time.timeScale = 1;
                break;

            case ShowResult.Failed:
                Debug.LogError("<color=red>The ad failed to be shown.</color>");
               
                if (HeroController.s_isAlive == false)
                {
                    Time.timeScale = 0;
                }
                else
                {
                    Time.timeScale = 1;
                }
                break;
        }
    }

    public void GetFreeGold()
    {
        Time.timeScale = 0;
        ShowRewardedAd();
    }
}