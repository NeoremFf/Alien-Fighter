using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EnemyTutorialDamageDeal : MonoBehaviour
{
    [SerializeField] private Text healthUI;
    [SerializeField] private Animator damageAnim;
    [SerializeField] private GameObject deadPref;
    [SerializeField] private SoundPlayer sound;
    [SerializeField] private GameObject exitTutorialPanel;
    private int damage = 1;

    private void Start()
    {
        Invoke("DealDamage", 3.5f);
    }

    private void DealDamage()
    {
        sound.PlayEnemyMakesDamage();

        deadPref.SetActive(true);
        damageAnim.Play("Damage To Hero Anim");
        healthUI.text = (int.Parse(healthUI.text) - damage).ToString();
        exitTutorialPanel.SetActive(true);
        Destroy(gameObject);
    }
}
