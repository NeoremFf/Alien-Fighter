using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    [SerializeField] private AudioClip enemyDeadSound;
    [SerializeField] private AudioClip enemyMakesDamageSound;
    [SerializeField] private AudioClip enemyClickSound;
    [SerializeField] private AudioClip soundHealthRecoveryDestroy;
    private bool isSoundActive;
    private AudioSource sound;

    private void Awake()
    {
        sound = GetComponent<AudioSource>();

        if (PlayerPrefs.GetInt("Sound_State") == 1 )
        {
            isSoundActive = true;
        }
        else
        {
            isSoundActive = false;
        }
    }

    public void SetSoundActive(bool value)
    {
        isSoundActive = value;
    }

    // button click
    public void PlaySound()
    {
        if (isSoundActive)
        {
            sound.Play(); // sound of button click
        }
    }

    #region Enemy
    public void PlayEnemyMakesDamage()
    {
        if (isSoundActive)
        {
            sound.PlayOneShot(enemyMakesDamageSound);
        }
    }

    public void PlayEnemyDeadSound()
    {
        if (isSoundActive)
        {
            sound.PlayOneShot(enemyDeadSound);
        }
    }

    public void PlayEnemyClickSound()
    {
        if (isSoundActive)
        {
            sound.PlayOneShot(enemyClickSound);
        }
    }
    #endregion

    #region Good
    public void PlayHealthRecoveryDestrouSound()
    {
        if (isSoundActive)
        {
            sound.PlayOneShot(soundHealthRecoveryDestroy);
        }
    }
    #endregion
}
