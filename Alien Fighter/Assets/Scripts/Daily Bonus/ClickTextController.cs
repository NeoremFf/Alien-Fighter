using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClickTextController : MonoBehaviour
{
    [SerializeField] [Range(0, 1)] private float lerpTime = 0.9f;
    [SerializeField] private Color newColor;
    private Text sprite;

    private void OnEnable()
    {
        if (!sprite)
        {
            sprite = GetComponent<Text>();
        }

        sprite.color = Color.white;
    }

    private void Update()
    {
        transform.Translate(Vector3.up * Time.deltaTime * 5);
        sprite.color = Color.Lerp(sprite.color, newColor, lerpTime * Time.deltaTime);

        if (sprite.color.a < 0.1f)
        {
            gameObject.SetActive(false);
        }
    }
}
