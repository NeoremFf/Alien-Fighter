using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy01 : EnemyController
{
    private float maxX,
        maxY;

    private Vector3 realPos;

    //private void Start()
    //{
    //    realPos = transform.position;

    //    SetUp();

    //    maxX = Screen.width;
    //    maxY = Screen.height;
    //}

    private void OnEnable()
    {
        if (isStart)
        {
            StartSettings();
            return;
        }

        realPos = transform.position;

        SetUp();

        maxX = Screen.width;
        maxY = Screen.height;
    }

    protected override void GetDamage()
    {
        hp--;
        if (hp == 0)
        {
            Dead();
        }
        else
        {
            bool isIllegalToSpawnHere = true;
            Vector3 screenPos = Vector3.zero,
                checkPos = Vector3.zero;
            while (isIllegalToSpawnHere)
            {
                float x = Random.Range(maxX * 0.1f, maxX * 0.9f);
                float y = Random.Range(maxY * 0.2f, maxY * 0.8f);
                screenPos = new Vector3(x, y, 0.5f);
                checkPos = Camera.main.ScreenToWorldPoint(screenPos);
                isIllegalToSpawnHere = Physics2D.OverlapCircle(checkPos, 0.7f);
            }

            realPos = checkPos;
            realPos.z = 0;

            sound.PlayEnemyClickSound();
            transform.localScale = normalSize / 3;
        }
    }

    private void OnMouseDown()
    {
        GetDamage();
    }

    private void Update()
    {
        MakeDamage();
        SizeUp();

        Move();
    }

    private void Move()
    {
        Vector3 dir = realPos - transform.position;
        if (dir.magnitude > 0.5f)
        {
            transform.position += dir.normalized * 20f * Time.deltaTime;
        }
    }
}
