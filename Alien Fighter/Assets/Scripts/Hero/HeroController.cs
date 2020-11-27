using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroController : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Text healthUI;

    [Header("Damage Anim")]
    [SerializeField] private Animator getDamageAnim;

    private ScoreController _score;
    private MonetizationSupport _monetization;


    // Static
    private static int health;
    public static bool s_isAlive = true;
    private static int s_countOfLoseForAd;
    private static Text helpHealthUI;

    private void Start()
    {
        helpHealthUI = healthUI;

        s_isAlive = true;

        UpdateUI();

        _score = FindObjectOfType<ScoreController>();
        _monetization = FindObjectOfType<MonetizationSupport>();

        health = 5;
        healthUI.text = health.ToString();
    }

    public void GetDamage(int value)
    {
        getDamageAnim.Play("Damage To Hero Anim");

        health -= value;

        UpdateUI();

        if (health <= 0)
        {
            Dead();
        }
    }

    public static void SetHealthToOne()
    {
        health = 1;
        helpHealthUI.text = health.ToString();
    }

    public int GetHealth()
    {
        return health;
    }

    public void HealthRecovery(int recovery)
    {
        health += recovery;
        UpdateUI();
    }

    private void UpdateUI()
    {
        healthUI.text = health.ToString();
    }

    private void Dead()
    {
        healthUI.text = 0.ToString();
        s_isAlive = false;
        Time.timeScale = 0;

        _score.UpdateUIAfterGame();
        ClearAllEnemeis.ClearAll();

        MonetizationSupport.ShowBannerAd();
    }

    public void ShowAdAfterLose(int countOfLoseNeed)
    {
        s_countOfLoseForAd++;

        if (s_countOfLoseForAd >= countOfLoseNeed)
        {
            s_countOfLoseForAd = 0;
            Time.timeScale = 0;
            _monetization.ShowRegularAd();
        }
    }
}
