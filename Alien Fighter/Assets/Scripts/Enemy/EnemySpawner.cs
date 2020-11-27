using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EnemySpawner : MonoBehaviour
{
    #region Variables
    [Header("Times")]
    [SerializeField] private float timeBtwSpawnCommon;
    [SerializeField] private float timeBtwSpawnRareMin;
    [SerializeField] private float timeBtwSpawnRareMax;
    [SerializeField] private float timeToSpawnBoss;
    [SerializeField] private float timeToAddSpeed;

    [Header("Score to spawn boss")]
    [SerializeField] private int scoreToSpawnBoss;

    [Header("Speed Spawn Index")]
    [SerializeField] private float speedIndex = 2f;

    private float currentTimerBtwSpawnCommon;
    private float currentTimerBtwSpawnRare;
    private float currentTimerToSpawnBoss;
    private float timerToAddSpeed;

    [Header("Prefabs")]
    [SerializeField] private GameObject enemyCommonPrefab;
    [SerializeField] private GameObject[] enemiesRarePrefab;
    [SerializeField] private GameObject[] bossesPrefab;

    private List<GameObject> enemyCommon = new List<GameObject>();
    private List<GameObject> enemyRare01 = new List<GameObject>();
    private List<GameObject> enemyRare02 = new List<GameObject>();
    private List<GameObject> enemyRare03 = new List<GameObject>();
    private List<GameObject> enemyRare04 = new List<GameObject>();
    private List<GameObject> bosses = new List<GameObject>();

    private int witchBoss = 0;

    [Header("Container")]
    [SerializeField] private GameObject container;
    [SerializeField] private GameObject pool;

    public bool isBoss { get; set; }
    private int countBossesWere = 0;

    private int tempScoreToSpawnBoss;
    private int tempCountAdd = 0;

    private float maxX,
        maxY;

    private ScoreController score;
    #endregion

    private void Awake()
    {
        maxX = Screen.width;
        maxY = Screen.height;

        currentTimerBtwSpawnCommon = timeBtwSpawnCommon * speedIndex;
        currentTimerBtwSpawnRare = Random.Range(timeBtwSpawnRareMin, timeBtwSpawnRareMax) * speedIndex;

        isBoss = false;

        timerToAddSpeed = timeToAddSpeed;
    }

    private void Start()
    {
        score = FindObjectOfType<ScoreController>().GetComponent<ScoreController>();

        CreateEnemeisGO();
    }

    private void CreateEnemeisGO()
    {
        // create 3 common enemies
        for (int i = 0; i < 3; i++)
        {
            enemyCommon.Add(Instantiate(enemyCommonPrefab));
            enemyCommon[i].transform.SetParent(pool.transform);
            enemyCommon[i].GetComponent<EnemyController>().Pool = pool;
            enemyCommon[i].GetComponent<EnemyController>().Conteiner = container;
            enemyCommon[i].SetActive(false);
        }

        // create 2 rare enemies (01, 02, 03, 04)
        for (int i = 0; i < 2; i++)
        {
            enemyRare01.Add(Instantiate(enemiesRarePrefab[0]));
            enemyRare02.Add(Instantiate(enemiesRarePrefab[1]));
            enemyRare03.Add(Instantiate(enemiesRarePrefab[2]));
            enemyRare03[i].GetComponent<Enemy03>().LocalStartSettings(pool, container);
            for (int k = 0; k < 2; k++)
            {
                GameObject clone1 = Instantiate(enemiesRarePrefab[2]);
                enemyRare03[i].GetComponent<Enemy03>().childrenClone.Add(clone1);
                clone1.GetComponent<Enemy03>().LocalStartSettings(enemyRare03[i], container);
                for (int j = 0; j < 2; j++)
                {
                    GameObject clone2 = Instantiate(enemiesRarePrefab[2]);
                    clone1.GetComponent<Enemy03>().childrenClone.Add(clone2);
                    clone2.GetComponent<Enemy03>().LocalStartSettings(clone1, container);
                    clone2.SetActive(false);
                }
                clone1.SetActive(false);
            }
            enemyRare04.Add(Instantiate(enemiesRarePrefab[3]));

            enemyRare01[i].transform.SetParent(pool.transform);
            enemyRare02[i].transform.SetParent(pool.transform);
            enemyRare03[i].transform.SetParent(pool.transform);
            enemyRare04[i].transform.SetParent(pool.transform);

            enemyRare01[i].GetComponent<EnemyController>().Pool = pool;
            enemyRare02[i].GetComponent<EnemyController>().Pool = pool;
            enemyRare03[i].GetComponent<EnemyController>().Pool = pool;
            enemyRare04[i].GetComponent<EnemyController>().Pool = pool;

            enemyRare01[i].GetComponent<EnemyController>().Conteiner = container;
            enemyRare02[i].GetComponent<EnemyController>().Conteiner = container;
            enemyRare03[i].GetComponent<EnemyController>().Conteiner = container;
            enemyRare04[i].GetComponent<EnemyController>().Conteiner = container;

            enemyRare01[i].SetActive(false);
            enemyRare02[i].SetActive(false);
            enemyRare03[i].SetActive(false);
            enemyRare04[i].SetActive(false);
        }

        // create 1 bosses all types
        for (int i = 0; i < bossesPrefab.Length; i++)
        {
            bosses.Add(Instantiate(bossesPrefab[i]));
            bosses[i].transform.SetParent(pool.transform);
            EnemyBoss _bossController = bosses[i].GetComponent<EnemyBoss>();
            _bossController.LocalStartSettings(pool, container);
            bosses[i].SetActive(false);
        }
    }

    private void Update()
    {
        if (!isBoss)
        {
            Timers();
        }
    }

    public void Timers()
    {
        if (currentTimerToSpawnBoss == 0)
        {
            currentTimerBtwSpawnCommon -= Time.deltaTime;
            if (currentTimerBtwSpawnCommon <= 0)
            {
                currentTimerBtwSpawnCommon = timeBtwSpawnCommon * speedIndex;
                Spawn(0);
            }

            currentTimerBtwSpawnRare -= Time.deltaTime;
            if (currentTimerBtwSpawnRare <= 0)
            {
                currentTimerBtwSpawnRare = Random.Range(timeBtwSpawnRareMin, timeBtwSpawnRareMax) * speedIndex;
                Spawn(1);
            }

            tempScoreToSpawnBoss += (int)ScoreController.score - tempScoreToSpawnBoss - scoreToSpawnBoss * tempCountAdd;
            if (tempScoreToSpawnBoss >= scoreToSpawnBoss)
            {
                tempScoreToSpawnBoss -= scoreToSpawnBoss;
                tempCountAdd++;
                currentTimerToSpawnBoss = timeToSpawnBoss;
            }
        }

        if (currentTimerToSpawnBoss > 0)
        {
            currentTimerToSpawnBoss -= Time.deltaTime;
            if (currentTimerToSpawnBoss <= 0)
            {
                currentTimerToSpawnBoss = 0;
                SpawnBoss();
            }
        }

        timerToAddSpeed -= Time.deltaTime;
        if (timerToAddSpeed <= 0)
        {
            AddSpeedOfGame();
        }
    }

    private void AddSpeedOfGame()
    {
        timerToAddSpeed = timeToAddSpeed;
        if (speedIndex > (0.6f  /*- 0.05f * countBossesWere*/))
        {
            speedIndex -= 0.05f /*+ (0.05f * countBossesWere)*/;
        }
    }

    private void Spawn(int n)
    {
        GameObject temp = null;
        float x,
            y;
        Vector3 screenPos,
            realPos = Vector3.zero;
        bool isIllegalToSpawnHere = true;
        int countOfRepeated = 0;

        switch (n)
        {
            case 0:
                foreach (var item in enemyCommon)
                {
                    if (!item.activeSelf)
                    {
                        temp = item;
                        break;
                    }
                }

                if (temp == null)
                {
                    Debug.LogError("Common enemy clone" + " == null!!!");
                    return;
                }

                temp.transform.SetParent(container.transform);

                while (isIllegalToSpawnHere &&
                    countOfRepeated < 350)
                {
                    countOfRepeated++;
                    x = Random.Range(maxX * 0.3f, maxX * 0.7f);
                    y = Random.Range(maxY * 0.2f, maxY * 0.7f);

                    screenPos = new Vector3(x, y, 5);
                    realPos = Camera.main.ScreenToWorldPoint(screenPos);

                    isIllegalToSpawnHere = Physics2D.OverlapCircle(realPos, 0.8f);

                    temp.transform.position = realPos;
                }

                temp.SetActive(true);
                break;

            case 1:
                int witchClone = Random.Range(0, enemiesRarePrefab.Length);
                /*************************************/
                //witchClone = 1; 
                /*************************************/
                List<GameObject> collection = null;

                switch (witchClone)
                {
                    case 0:
                        collection = enemyRare01;
                        break;

                    case 1:
                        collection = enemyRare02;
                        break;

                    case 2:
                        collection = enemyRare03;
                        break;

                    case 3:
                        collection = enemyRare04;
                        break;
                }

                foreach (var item in collection)
                {
                    if (!item.activeSelf)
                    {
                        temp = item;
                        break;
                    }
                }

                if (temp == null)
                {
                    Debug.LogError(witchClone + ") clone" + " == null!!!");
                    return;
                }

                temp.transform.SetParent(container.transform);

                Enemy02 enemy02 = temp.GetComponent<Enemy02>();

                if (enemy02)
                {
                    x = Random.Range(maxX * 0.3f, maxX * 0.7f);
                    y = maxY * 0.2f;

                    screenPos = new Vector3(x, y, 5);
                    realPos = Camera.main.ScreenToWorldPoint(screenPos);
                }
                else
                {
                    while (isIllegalToSpawnHere &&
                        countOfRepeated < 350)
                    {
                        countOfRepeated++;

                        x = Random.Range(maxX * 0.3f, maxX * 0.7f);
                        y = Random.Range(maxY * 0.2f, maxY * 0.7f);

                        screenPos = new Vector3(x, y, 5);
                        realPos = Camera.main.ScreenToWorldPoint(screenPos);

                        isIllegalToSpawnHere = Physics2D.OverlapCircle(realPos, 0.8f);
                    }

                }

                temp.transform.position = realPos;

                temp.SetActive(true);
                break;
        }
    }

    private void SpawnBoss()
    {
        isBoss = true;

        countBossesWere++;

        if (scoreToSpawnBoss < 1000)
        {
            scoreToSpawnBoss += 250;
        }

        GameObject temp = bosses[witchBoss];

        if (witchBoss != bossesPrefab.Length - 1)
        {
            witchBoss++;
        }
        else
        {
            witchBoss = 0;
        }

        temp.transform.SetParent(container.transform);

        float x = maxX * 0.5f;
        float y = maxY * 0.4f;

        Vector3 screenPos = new Vector3(x, y, 5);
        Vector3 realPos = Camera.main.ScreenToWorldPoint(screenPos);
        temp.transform.position = realPos;

        temp.SetActive(true);
    }
}
