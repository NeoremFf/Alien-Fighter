using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy03 : EnemyController
{
    private GameObject parent;

    public List<GameObject> childrenClone = new List<GameObject>();

    private int startHp,
        startDamage;

    private void OnEnable()
    {
        if (isStart)
        {
            StartSettings();
            return;
        }
        SetUp();
    }

    public void LocalStartSettings(GameObject parentTemp, GameObject containerParenTemp)
    {
        Conteiner = containerParenTemp;
        parent = parentTemp;

        Enemy03 parentController = parent.GetComponent<Enemy03>();
        if (!parentController)
        {
            return;
        }
        transform.SetParent(parent.transform);
        SetMaxHp(parentController.maxHp / 2);
        SetDamage(parentController.damage / 2);

        transform.localScale = parent.transform.localScale * 0.9f;
        normalSize = transform.localScale;
    }

    private void OnMouseDown()
    {
        GetDamage();
    }

    protected override void GetDamage()
    {
        hp--;

        if (hp <= 0)
        {
            if (childrenClone.Count > 0)
            {
                for (int i = 0; i < 2; i++)
                {
                    GameObject clone = childrenClone[i];
                    clone.transform.SetParent(Conteiner.transform);
                    clone.transform.position = Random.insideUnitCircle * Random.Range(0.5f, 2f);
                    clone.SetActive(true);
                }
            }

            Dead();
        }
        else
        {
            sound.PlayEnemyClickSound();
            transform.localScale = normalSize / 3;
        }
    }

    protected override void MakeDamage()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            transform.SetParent(parent.transform);
            gameObject.SetActive(false);
            _hero.GetDamage(damage);
            sound.PlayEnemyMakesDamage();
        }
    }

    public override void Dead()
    {
        AddScore();
        sound.PlayEnemyDeadSound();

        transform.SetParent(parent.transform);
        gameObject.SetActive(false);
    }

    private void Update()
    {
        MakeDamage();
        SizeUp();
    }
}
