using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class CheckDaily : MonoBehaviour
{
    #region Varabels
    [Header("Colors")]
    [SerializeField] private Color helpNormalColor;
    [SerializeField] private Color helpCurrentDayColor;
    [SerializeField] private Color helpLastDayColor;
    private static Color normalColor;
    private static Color currrentDayColor;
    private static Color lastDayColor;

    [Header("Panel of Days")]
    [SerializeField] private GameObject helpDay1GO;
    [SerializeField] private GameObject helpDay2GO;
    [SerializeField] private GameObject helpDay3GO;
    [SerializeField] private GameObject helpDay4GO;
    [SerializeField] private GameObject helpDay5GO;
    private static GameObject day1GO;
    private static GameObject day2GO;
    private static GameObject day3GO;
    private static GameObject day4GO;
    private static GameObject day5GO;

    [SerializeField] private GameObject helpPanelDailyBonus;
    private static GameObject panelDailyBonus;

    private static DateTime today; // date today
    private static DateTime nextDay; // date of next day to daily strike

    public static int DayStrike { get; set; } // current day of strike
    #endregion

    private void Awake()
    {
        DayStrike = 1;

        panelDailyBonus = helpPanelDailyBonus;

        normalColor = helpNormalColor;
        currrentDayColor = helpCurrentDayColor;
        lastDayColor = helpLastDayColor;

        day1GO = helpDay1GO;
        day2GO = helpDay2GO;
        day3GO = helpDay3GO;
        day4GO = helpDay4GO;
        day5GO = helpDay5GO;
    }

    /// <summary>
    /// Check that the first day or more
    /// </summary>
    public static void CheckDayStrike()
    {
        //PlayerPrefs.DeleteAll();
        //return;

        /// <summary>
        /// It is first entering in game - need to show tutorial first (exit daily bonus)
        /// </summary>
        if (!PlayerPrefs.HasKey("WasTutorial"))
        {
            return;
        }

        // if it is the first entering in game
        if (!PlayerPrefs.HasKey("NextDay"))
        {
            today = DateTime.Now.AddDays(-1);
            nextDay = today.AddDays(2);
            PlayerPrefs.SetString("TodayDate", today.ToString());
            PlayerPrefs.SetString("NextDay", nextDay.ToString());
            PlayerPrefs.SetInt("DailyBonusEarned", 0);
            PlayerPrefs.SetInt("DayStrike", 1);
        }

        // check that it will the first entering in game today
        today = Convert.ToDateTime(PlayerPrefs.GetString("TodayDate"));
        if (today.Day == DateTime.Now.Day &&
            PlayerPrefs.GetInt("DailyBonusEarned") == 1)
        {
            // Exit
            Debug.LogWarning("<color=blue>It is not the first entering in game today.</color>");
            return;
        }
        else // if it is first entering in game today
        {
            PlayerPrefs.SetInt("DailyBonusEarned", 0);
        }

        //Work:
        today = DateTime.Now;
        nextDay = Convert.ToDateTime(PlayerPrefs.GetString("NextDay"));
        DayStrike = PlayerPrefs.GetInt("DayStrike");
        if (today.Date == nextDay.Date) //It is not first day
        {
            DayStrike++; // add one day to strike

            // if was 5-days-strike than reset to first day
            if (DayStrike > 5)
            {
                DayStrike = 1;
            }

            for (int i = 1; i < DayStrike; i++)
            {
                switch (i)
                {
                    case 1:
                        day1GO.GetComponent<Image>().color = lastDayColor;
                        break;

                    case 2:
                        day2GO.GetComponent<Image>().color = lastDayColor;
                        break;

                    case 3:
                        day3GO.GetComponent<Image>().color = lastDayColor;
                        break;

                    case 4:
                        day4GO.GetComponent<Image>().color = lastDayColor;
                        break;

                    default:
                        break;
                }
            }
        }
        else
        {
            //It is first day
            DayStrike = 1; // reset to start
        }

        // Set Current day Plane color
        switch (DayStrike)
        {
            case 1:
                day1GO.GetComponent<Image>().color = currrentDayColor;
                break;

            case 2:
                day2GO.GetComponent<Image>().color = currrentDayColor;
                break;

            case 3:
                day3GO.GetComponent<Image>().color = currrentDayColor;
                break;

            case 4:
                day4GO.GetComponent<Image>().color = currrentDayColor;
                break;

            case 5:
                day5GO.GetComponent<Image>().color = currrentDayColor;
                break;

            default:
                break;
        }

        PlayerPrefs.SetInt("DayStrike", DayStrike); // save dayStrike

        //Set today to check
        PlayerPrefs.SetString("TodayDate", today.ToString());
        // Set date of next day to check
        nextDay = today.AddDays(1);
        PlayerPrefs.SetString("NextDay", nextDay.ToString());

        //Open Daily Bonus Scene:
        panelDailyBonus.SetActive(true);
    }
}
