using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DailyBossManager : MonoBehaviour
{
    [Header("End's Panel")]
    [SerializeField] private GameObject endsPanel;
    [SerializeField] private GameObject day5RewardPanel;

    [Header("Timer Sprite")]
    [SerializeField] private GameObject timerSprite;
    private float maxTime;

    [Header("Click Setup")]
    [SerializeField] private GameObject parent;
    [SerializeField] private GameObject prefabTextAfterClick;
    [SerializeField] private Text textCountGoldAdd;
    private int countOfGold;
    private List<GameObject> listText = new List<GameObject>();

    [Header("Sound")]
    [SerializeField] private SoundPlayer sound;

    private float timeToDestroy;

    private bool isWaiting = true;

    private Vector3 normalScale;

    private void Start()
    {
        PlayerPrefs.SetInt("DailyBonusEarned", 1);

        for (int i = 0; i < 20; i++)
        {
            listText.Add(Instantiate(prefabTextAfterClick, parent.transform));
            listText[i].transform.SetParent(parent.transform);
            listText[i].SetActive(false);
        }

        Setup();
    }

    private void Setup()
    {
        // Get current time of live the boss from daily revards script manager
        timeToDestroy = CheckDaily.DayStrike * 5;
        maxTime = timeToDestroy;

        textCountGoldAdd.text = "+" + countOfGold;

        normalScale = transform.localScale;
    }

    private void Update()
    {
        if (isWaiting)
        {
            return;
        }

        timeToDestroy -= Time.deltaTime;
        if (timeToDestroy <= 0)
        {
            EndOfTime();
        }

        timerSprite.transform.localScale = new Vector3(timeToDestroy / maxTime, 1, 1);
        if (timerSprite.transform.localScale.x < 0.01f)
        {
            timerSprite.SetActive(false);
        }

        if (transform.localScale.x < normalScale.x)
        {
            transform.localScale += new Vector3(0.1f, 0.1f, 0.1f);
        }
    }

    private void OnMouseDown()
    {
        if (isWaiting)
        {
            isWaiting = !isWaiting;
        }

        //Set gold Add
        countOfGold++;
        textCountGoldAdd.text = "+" + countOfGold;

        //Set temp clickText
        GameObject temp = null;

        foreach (var item in listText)
        {
            if (!item.activeSelf)
            {
                temp = item;
                break;
            }
        }

        if (!temp)
        {
            Debug.LogError("Temp 'ClickText' is null!");
            listText.Add(Instantiate(prefabTextAfterClick, parent.transform));
            listText[listText.Count - 1].transform.SetParent(parent.transform);
            temp = listText[listText.Count - 1];
        }

        float x = Random.Range(-1.68f, 1.68f) + transform.position.x;
        float y = Random.Range(-2.32f, 0) + transform.position.y;
        Vector3 pos = new Vector3(x, y, parent.transform.position.z);
        temp.transform.position = pos;
        temp.SetActive(true);

        sound.PlayEnemyClickSound();

        transform.localScale = normalScale * 0.9f;
    }

    private void EndOfTime()
    {
        Destroy(gameObject);
        endsPanel.SetActive(true);
        if (CheckDaily.DayStrike == 5)
        {
            day5RewardPanel.SetActive(true);
            ScoreController.AddClearCharge();
        }
        else
        {
            day5RewardPanel.SetActive(false);
        }
        day5RewardPanel.SetActive(CheckDaily.DayStrike == 5 ? true : false);
        endsPanel.GetComponentInChildren<Text>().text = "+" + countOfGold;
        PlayerPrefs.SetInt("Gold", PlayerPrefs.GetInt("Gold") + countOfGold);
    }
}
