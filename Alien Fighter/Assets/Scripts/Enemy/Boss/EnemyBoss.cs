using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoss : EnemyController
{
    #region Variables
    [Header("Times")]
    [SerializeField] private float timeToShield;
    [SerializeField] private float timeToSpawnCommonEnemy;
    private float currentTimerToShield;
    private float currentTimerToSpawnCommonEnemy;

    [Header("Boss Prefabs")]
    [SerializeField] private GameObject shieldPrefab;
    [SerializeField] private GameObject minionPrefab;
    [SerializeField] private int maxCountOfShields;
    [SerializeField] private float radiusOfShieldSpawn = 2.4f;
    private int currentCountOfShields = 0;
    private bool isShieling = false;

    private GameObject container;

    private EnemySpawner _spawner;

    [Header("Sprite")]
    [SerializeField] private Sprite shieldBossSprite;
    private Sprite normalBossSprite;

    private List<GameObject> shields = new List<GameObject>();
    private List<GameObject> minions = new List<GameObject>();

    private float maxX,
        maxY,
        maxXCor;
    #endregion

    private void OnEnable()
    {
        if (isStart)
        {
            StartSettings();

            IsBoos = true;

            maxX = Screen.width;
            maxY = Screen.height;

            maxXCor = Camera.main.ScreenToWorldPoint(new Vector3(maxX, 0, 0)).x;

            return;
        }

        SetUp();

        currentTimerToShield = timeToShield;
        currentTimerToSpawnCommonEnemy = timeToSpawnCommonEnemy;

        currentCountOfShields = 1;
        RemoveShield();

        Shield();
    }

    public void LocalStartSettings(GameObject pool, GameObject container)
    {
        Pool = pool;
        Conteiner = container;
        CreatedGO();
        normalBossSprite = spriteRenderer.sprite;
        _spawner = FindObjectOfType<EnemySpawner>();
    }

    private void CreatedGO()
    {
        /*Shield*/
        for (int i = 0; i < maxCountOfShields + 3; i++)
        {
            shields.Add(Instantiate(shieldPrefab));
            shields[i].transform.SetParent(transform);
            shields[i].GetComponent<EnemyBoss_Shield>().LocalStartSettings(gameObject);
            shields[i].SetActive(false);
        }

        /*Minion*/
        for (int i = 0; i < 2; i++)
        {
            minions.Add(Instantiate(minionPrefab));
            minions[i].transform.SetParent(transform);
            minions[i].GetComponent<EnemyMinion>().LocalStartSettings(gameObject);
            minions[i].SetActive(false);
        }
    }

    #region Shield
    public void AddShield()
    {
        currentCountOfShields++;
        isShieling = true;
    }

    public void RemoveShield()
    {
        currentCountOfShields--;
        if (currentCountOfShields <= 0)
        {
            isShieling = false;
            spriteRenderer.sprite = normalBossSprite;
        }
    }
    #endregion

    protected override void GetDamage()
    {
        hp--;
        if (hp == 0)
        {
            Dead();
            _spawner.isBoss = false;
        }
        else
        {
            sound.PlayEnemyClickSound();
        }
    }

    private void OnMouseDown()
    {
        if (!isShieling)
        {
            GetDamage();
        }
    }

    private void Update()
    {
        MakeDamage();
        Phase();
        SizeUp();
    }

    protected override void MakeDamage()
    {
        if (timer <= 0)
        {
            _spawner.isBoss = false;

            foreach (var item in shields)
            {
                item.SetActive(false);
                item.transform.SetParent(Pool.transform);
            }

            foreach (var item in minions)
            {
                item.SetActive(false);
                item.transform.SetParent(Pool.transform);
            }

            DestroyGO();
            _hero.GetDamage(damage);
            sound.PlayEnemyMakesDamage();
        }
        timer -= Time.deltaTime;
    }

    private void Phase()
    {
        if (!isShieling)
        {
            currentTimerToShield -= Time.deltaTime;
            if (currentTimerToShield <= 0)
            {
                Shield();
            }

            currentTimerToSpawnCommonEnemy -= Time.deltaTime;
            if (currentTimerToSpawnCommonEnemy <= 0)
            {
                SpawnEnemy();
            }
        }
    }

    private void SpawnEnemy()
    {
        currentTimerToSpawnCommonEnemy = timeToSpawnCommonEnemy;

        GameObject temp = null;
        foreach (var item in minions)
        {
            if (!item.activeSelf)
            {
                temp = item;
                break;
            }
        }

        if (temp == null)
        {
            Debug.LogError("GameObject == null!!!");
            return;
        }

        temp.transform.SetParent(transform.parent.transform);

        bool cantSpawn = true;
        Vector3 posToMoveOn = Vector3.zero;
        int countOfRepeated = 0;
        while (cantSpawn &&
            countOfRepeated < 350)
        {
            countOfRepeated++;

            posToMoveOn.x = Random.Range(maxX * 0.3f, maxX * 0.7f);
            posToMoveOn.y = Random.Range(maxY * 0.2f, maxY * 0.7f);
            posToMoveOn.z = transform.position.z + 0.5f;

            posToMoveOn = Camera.main.ScreenToWorldPoint(posToMoveOn);

            if (posToMoveOn.x < maxXCor - maxXCor * 0.2f &&
                posToMoveOn.x > -maxXCor + maxXCor * 0.2f)
            {
                cantSpawn = false;
            }

            cantSpawn = Physics2D.OverlapCircle(posToMoveOn, 1f);
        }

        temp.GetComponent<EnemyMinion>().PosToMoveOn = posToMoveOn;
        temp.transform.position = transform.position;

        temp.SetActive(true);
    }

    private void Shield()
    {
        spriteRenderer.sprite = shieldBossSprite;

        currentTimerToShield = timeToShield;

        int currentLoopShieldSpwn = Random.Range(maxCountOfShields - 3, maxCountOfShields + 3);

        for (int i = 0; i < currentLoopShieldSpwn; i++)
        {
            GameObject temp = shields[i];
            temp.transform.SetParent(transform.parent.transform);

            Vector3 pos;
            pos.x = transform.position.x + radiusOfShieldSpawn * Mathf.Sin((360 / currentLoopShieldSpwn * i) * Mathf.Deg2Rad);
            pos.y = transform.position.y + radiusOfShieldSpawn * Mathf.Cos((360 / currentLoopShieldSpwn * i) * Mathf.Deg2Rad);
            pos.z = transform.position.z + 0.5f;
            temp.transform.position = pos;

            temp.SetActive(true);
        }
    }
}