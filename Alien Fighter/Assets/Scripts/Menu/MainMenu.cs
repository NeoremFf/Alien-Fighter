using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Animator anim;

    [SerializeField] private GameObject dailyPanel;

    [SerializeField] private GameObject donutShop;
    [SerializeField] private GameObject shop;

    [SerializeField] private GameObject panelGetGoldAfterGold;

    private bool isSetting = false;
    private bool isShop = false;

    private void Start()
    {
        shop.SetActive(false);
    }

    public void StartGame()
    {
        /// <summary>
        /// It is first entering in game - need to show tutorial
        /// </summary>
        if (!PlayerPrefs.HasKey("WasTutorial"))
        {
            PlayerPrefs.SetInt("WasTutorial", 1);
            for (int i = 0; i < 5; i++)
            {
                ScoreController.AddClearCharge();
            }
            MonetizationSupport.HideBannerAd();
            SceneManager.LoadScene(3); // Load Tutoria-scene
            return;
        }

        MonetizationSupport.HideBannerAd();
        SceneManager.LoadScene(1);
    }

    private IEnumerator CloseShope()
    {
        yield return new WaitForSeconds(1f);
        shop.SetActive(false);
    }

    public void EnterMenu()
    {
        if (isSetting)
        {
            anim.Play("UI_BackSettings_MainMenu");
            isSetting = false;
        }
        else if (isShop)
        {
            anim.Play("UI_BackShop");
            isShop = false;
            StartCoroutine(CloseShope());
        }
    }

    public void EnterSettings()
    {
        anim.Play("UI_Settings_MainMenu");
        isSetting = true;
    }

    public void EnterShop()
    {
        shop.SetActive(true);
        anim.Play("UI_Shop");
        isShop = true;
    }

    public void OpenDonutShop()
    {
        donutShop.SetActive(true);
    }

    public void CloseDonutShop()
    {
        donutShop.SetActive(false);
    }

    public void ClosePanelGetGoldAfterAd()
    {
        panelGetGoldAfterGold.SetActive(false);
    }

    public void OpenDailyBonusPanel()
    {
        dailyPanel.SetActive(true);
    }

    public void CloseDailyBonusPanel()
    {
        dailyPanel.SetActive(false);
    }

    public void EnterDailyBonusScene()
    {
        SceneManager.LoadScene(2);
    }

    public void ExitFromAplication()
    {
        Application.Quit();
    }
}