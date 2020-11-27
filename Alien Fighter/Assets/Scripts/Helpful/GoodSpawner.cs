using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoodSpawner : MonoBehaviour
{
    [Header("Times")]
    [SerializeField] private float timeToStartSpawn;

    [SerializeField] private float timeBtwSpawn;
    private float currentTimerBtwSpawn;

    [Header("Prefabs")]
    [SerializeField] private GameObject recoveryPrefab;

    [Header("Container")]
    [SerializeField] private GameObject pool;
    [SerializeField] private GameObject container;

    private List<GameObject> recovery = new List<GameObject>();

    private SpriteRenderer spriteRenderer;
    private Sprite[] sprites;

    private float maxX,
        maxY;

    private void Start()
    {
        CreatedGO();
        SetSprites();

        maxX = Screen.width;
        maxY = Screen.height;

        currentTimerBtwSpawn = timeBtwSpawn;
    }

    private void CreatedGO()
    {
        for (int i = 0; i < 3; i++)
        {
            recovery.Add(Instantiate(recoveryPrefab));
            HealthRecovery controller = recovery[i].GetComponent<HealthRecovery>();
            controller.Pool = pool;
            controller.Container = container;
            recovery[i].transform.SetParent(pool.transform);
            recovery[i].SetActive(false);
        }
    }

    private void SetSprites()
    {
        sprites = Resources.LoadAll<Sprite>("Buffs/");
    }

    private void Update()
    {
        Timer();
    }

    public void Timer()
    {
        currentTimerBtwSpawn -= Time.deltaTime;
        if (currentTimerBtwSpawn <= 0)
        {
            currentTimerBtwSpawn = timeBtwSpawn;

            Spawn();
        }
    }

    public void Spawn()
    {
        GameObject temp = null;
        float x,
            y;
        Vector3 screenPos,
            rotateVec,
            realPos = Vector3.zero;
        bool isIllegalToSpawnHere = true;

        foreach (var item in recovery)
        {
            if (!item.activeSelf)
            {
                temp = item;
                break;
            }
        }

        if (temp == null)
        {
            Debug.LogError("Health Recovery == null");
            return;
        }

        spriteRenderer = temp.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprites[Random.Range(0, sprites.Length)];

        int countOfRepeated = 0;
        while (isIllegalToSpawnHere ||
            countOfRepeated < 350)
        {
            countOfRepeated++;

            x = Random.Range(maxX * 0.1f, maxX * 0.9f);
            y = Random.Range(maxY * 0.2f, maxY * 0.8f);
            screenPos = new Vector3(x, y, 5);
            realPos = Camera.main.ScreenToWorldPoint(screenPos);

            isIllegalToSpawnHere = Physics2D.OverlapCircle(realPos, 0.6f);
        }

        temp.transform.position = realPos;
        rotateVec = new Vector3(0, 0, Random.Range(0, 360));
        temp.transform.Rotate(rotateVec);

        temp.SetActive(true);
    }
}
