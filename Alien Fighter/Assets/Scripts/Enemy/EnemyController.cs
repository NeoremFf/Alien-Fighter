using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Resources Folder Name")]
    [SerializeField] private string resourcesName;
    [SerializeField] private string resourcesTypeForBoss;

    public GameObject Pool { get; set; }
    public GameObject Conteiner { get; set; }

    [Header("Dead Prefab")]
    [SerializeField] private GameObject deadPrefab;
    protected GameObject deadAnimGO; 

    [Header("Value")]
    [SerializeField] protected int maxHp;
    [SerializeField] protected int damage;
    protected int hp;

    [SerializeField] private int scoreAddCount;

    [SerializeField] private float timeToDamage;
    protected float timer;

    protected Vector3 normalSize;

    protected SoundPlayer sound;

    protected bool isStart = true;

    protected HeroController _hero;
    private ScoreController _score;

    //Sprites
    protected SpriteRenderer spriteRenderer;
    private Sprite[] skins;

    // Is it boss
    public bool IsBoos { get; set; }

    protected virtual void StartSettings()
    {
        isStart = false;

        IsBoos = false;

        sound = FindObjectOfType<SoundPlayer>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        _hero = FindObjectOfType<HeroController>().GetComponent<HeroController>();
        _score = FindObjectOfType<ScoreController>().GetComponent<ScoreController>();

        normalSize = transform.localScale;

        deadAnimGO = Instantiate(deadPrefab);
        deadAnimGO.transform.SetParent(transform);
        deadAnimGO.SetActive(false);

        SetSprite();
    }

    private void SetSprite()
    {
        if (resourcesTypeForBoss != null)
        {
            skins = Resources.LoadAll<Sprite>("Enemy/" + resourcesName + "/" + resourcesTypeForBoss);
        }
        else
        {
            skins = Resources.LoadAll<Sprite>("Enemy/" + resourcesName);
        }
        spriteRenderer.sprite = skins[PlayerPrefs.GetInt("SelectedSkin")];
    }

    protected void SetMaxHp(int value)
    {
        maxHp = value;
    }

    protected void SetDamage(int value)
    {
        damage = value;
    }

    protected virtual void SetUp()
    {
        hp = maxHp;

        timer = timeToDamage;

        transform.localScale = normalSize / 3;
    }
    
    protected virtual void GetDamage()
    {
        hp--;
        if (hp == 0)
        {
            Dead();
        }
        else
        {
            sound.PlayEnemyClickSound();
            transform.localScale = normalSize / 3;
        }
    }

    public virtual void Dead()
    {
        DestroyGO();
        AddScore();
        sound.PlayEnemyDeadSound();
    }

    protected virtual void MakeDamage()
    {
        if (timer <= 0)
        {
            DestroyGO();
            _hero.GetDamage(damage);
            sound.PlayEnemyMakesDamage();
        }
        timer -= Time.deltaTime;
    }

    protected void DestroyGO()
    {
        deadAnimGO.SetActive(true);
        deadAnimGO.transform.SetParent(transform.parent.transform);
        deadAnimGO.transform.position = transform.position;

        gameObject.transform.SetParent(Pool.transform);
        gameObject.SetActive(false);
    }

    protected virtual void SizeUp()
    {
        if (transform.localScale.x < normalSize.x)
        {
            transform.localScale += new Vector3 (0.1f, 0.1f, 0.1f);
        }
    }

    protected virtual void AddScore()
    {
        _score.AddScore(scoreAddCount);
    }
}