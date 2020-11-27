using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClearAllEnemeis : MonoBehaviour
{
    [SerializeField] private GameObject allEnemies;
    [SerializeField] private Text chargeClearText;
    [SerializeField] private Animator animWavw;

    private static GameObject helpAllEnemies;

    private void Start()
    {
        chargeClearText.text = ScoreController.s_chargeOfClear.ToString();

        helpAllEnemies = allEnemies;
    }

    public void Activated()
    {
        if (ScoreController.s_chargeOfClear <= 0)
        {
            return;
        }

        List<GameObject> collection = new List<GameObject>();
        int countOfEnemeis = allEnemies.transform.childCount;

        if (countOfEnemeis <= 0)
        {
            return;
        }

        int newCountOfEnemeis = 0;
        for (int i = 0, k = 0; i < countOfEnemeis; i++, k++)
        {
            collection.Add(allEnemies.transform.GetChild(i).gameObject);
            if (collection[k].GetComponent<EnemyBoss_Shield>())
            {
                collection.Remove(collection[k]);
                k--;
            }
            else if (!collection[k].GetComponent<EnemyController>())
            {
                collection.Remove(collection[k]);
                k--;
            }
            else if (collection[k].GetComponent<EnemyController>().IsBoos)
            {
                collection.Remove(collection[k]);
                k--;
            }
            newCountOfEnemeis = k;
        }

        newCountOfEnemeis++;

        if (newCountOfEnemeis <= 0)
        {
            return;
        }

        for (int i = 0; i < newCountOfEnemeis; i++)
        {
            collection[i].GetComponent<EnemyController>().Dead();
        }

        animWavw.Play("ClearWave");

        ScoreController.RemoveClearCharge();
        chargeClearText.text = ScoreController.s_chargeOfClear.ToString();
    }

    public static void ClearAll()
    {
        List<GameObject> collection = new List<GameObject>();
        int countOfEnemeis = helpAllEnemies.transform.childCount;

        long curruntScore = ScoreController.score;

        if (countOfEnemeis <= 0)
        {
            return;
        }


        for (int i = 0; i < countOfEnemeis; i++)
        {
            collection.Add(helpAllEnemies.transform.GetChild(i).gameObject);
        }

        for (int i = 0; i < countOfEnemeis; i++)
        {
            EnemyController _enemy = collection[i].GetComponent<EnemyController>();

            if (!_enemy)
            {
                continue;
            }

            _enemy.Dead();
        }

        ScoreController.SetScoreBck(curruntScore);
    }
}
