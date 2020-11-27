using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class Menu : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private GameObject enemies;
    [SerializeField] private GameObject audioUpdate;

    public Animator AnimHelpToAddHealth { get; set; }

    private void Awake()
    {

    }

    private void Start()
    {
        AnimHelpToAddHealth = anim;
    }

    public void OpenMenu()
    {
        if (HeroController.s_isAlive)
        {
            //audioUpdate.SetActive(false);
            //audioUpdate.SetActive(true);
            anim.Play("Menu");
            MonetizationSupport.ShowBannerAd();

            if (enemies)
            {
                enemies.transform.position -= new Vector3(10, 0, 0);
            }

            Time.timeScale = 0;
        }
    }

    public void Continue()
    {
        anim.Play("BackMenu");
        MonetizationSupport.HideBannerAd();

        if (enemies)
        {
            enemies.transform.position += new Vector3(10, 0, 0);
        }

        Time.timeScale = 1;
    }

    public void Restart()
    {
        MonetizationSupport.HideBannerAd();
        SceneManager.LoadScene(1);

        Time.timeScale = 1;
    }

    public void RefreashHealthToOneByAd()
    {
        EventSystem.current.currentSelectedGameObject.SetActive(false);

        ClearAllEnemeis.ClearAll();

        FindObjectOfType<MonetizationSupport>().ShowRewardedAd();
    }

    public void ExitMainMenu()
    {
        MonetizationSupport.ShowBannerAd();
        SceneManager.LoadScene(0);
        //Destroy(audioController.gameObject);

        Time.timeScale = 1;
    }
}
