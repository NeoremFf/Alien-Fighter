using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthRecovery : MonoBehaviour
{
    public GameObject Pool { get; set; }
    public GameObject Container { get; set; }

    [SerializeField] private int recovery;
    [SerializeField] private int addScore;

    [SerializeField] private float timeToDestroy;
    private float timerToDestroy;

    private SoundPlayer sound;

    private bool isStart = true;

    private void Start()
    {
        sound = FindObjectOfType<SoundPlayer>();
    }

    private void OnEnable()
    {
        if (isStart)
        {
            isStart = false;
            return;
        }

        timerToDestroy = timeToDestroy;
        transform.SetParent(Container.transform);
    }

    private void Update()
    {
        timerToDestroy -= Time.deltaTime;
        if (timerToDestroy <= 0)
        {
            DestroyGO();
            sound.PlayHealthRecoveryDestrouSound();
        }
    }

    private void DestroyGO()
    {
        gameObject.transform.SetParent(Pool.transform);
        gameObject.SetActive(false);
    }

    private void OnMouseDown()
    {
        ScoreController _score = GameObject.FindObjectOfType<ScoreController>().GetComponent<ScoreController>();
        HeroController _hero = GameObject.FindObjectOfType<HeroController>().GetComponent<HeroController>();

        if (_hero.GetHealth() < 5)
        {
            _hero.HealthRecovery(recovery);
        }
        else
        {
            _score.AddScore(addScore);
        }
        sound.PlayHealthRecoveryDestrouSound();
        DestroyGO();
    }
}
