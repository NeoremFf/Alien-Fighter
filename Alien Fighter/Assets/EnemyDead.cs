using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDead : MonoBehaviour
{
    private Animator anim;
    private bool isStart = true;
    private GameObject parent;

    private void Start()
    {
        anim = GetComponent<Animator>();
        parent = transform.parent.gameObject;
    }

    private void OnEnable()
    {
        if (isStart)
        {
            isStart = false;
            return;
        }

        StartCoroutine(PlayAnim());
    }

    private IEnumerator PlayAnim()
    {
        yield return new WaitForSeconds(0.5f);
        transform.SetParent(parent.transform);
        gameObject.SetActive(false);
    }
}
