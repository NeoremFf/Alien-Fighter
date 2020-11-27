using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTutorial : MonoBehaviour
{
    [SerializeField] private GameObject animFingerClick;
    [SerializeField] private GameObject animHealthTutorial;
    [SerializeField] private GameObject enemyDamage;
    [SerializeField] private SoundPlayer sound;

    private Vector3 normalScale;

    private int health = 5;

    private float timeToShowAnimAgain = 1.5f;
    private float timer;

    void Start()
    {
        normalScale = transform.localScale;
    }

    private void OnMouseDown()
    {
        timer = timeToShowAnimAgain;

        if (animFingerClick.activeSelf)
        {
            animFingerClick.SetActive(false);
        }

        transform.localScale = normalScale / 3;

        health--;
        if (health <= 0)
        {
            sound.PlayEnemyDeadSound();

            Destroy(gameObject);
            animHealthTutorial.SetActive(true);
            enemyDamage.SetActive(true);
            animFingerClick.SetActive(false);
        }
        else
        {
            sound.PlayEnemyClickSound();
        }
    }

    void Update()
    {
        if (transform.localScale.x < normalScale.x)
        {
            transform.localScale += new Vector3(0.1f, 0.1f, 0.1f);
        }

        if (timer <= 0)
        {
            animFingerClick.SetActive(true);
        }
        else
        {
            timer -= Time.deltaTime;
        }
    }
}
