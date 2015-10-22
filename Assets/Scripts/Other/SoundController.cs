using UnityEngine;
using System.Collections;

/// <summary>
/// sound in game : button, effect, win, lose...
/// </summary>
public class SoundController : MonoBehaviour
{

    public static SoundController Sound; // instance of SoundController

    public AudioClip[] SoundClips;      // array sound clips

    public AudioSource audiosource;     // audio source
    void Awake()
    {
        if (Sound == null)
        {
            DontDestroyOnLoad(gameObject);
            Sound = this;
        }
        else if (Sound != this)
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// sound on state
    /// </summary>
    public void SoundON()
    {
        audiosource.mute = false;
    }

    /// <summary>
    /// sound off state
    /// </summary>
    public void SoundOFF()
    {
        audiosource.mute = true;
    }

    public void Click()
    {
        audiosource.PlayOneShot(SoundClips[0]);

    }
    public void JewelCrash()
    {
        audiosource.PlayOneShot(SoundClips[1]);
    }

    public void LockCrash()
    {
        audiosource.PlayOneShot(SoundClips[2]);
    }
    public void IceCrash()
    {
        audiosource.PlayOneShot(SoundClips[3]);
    }

    public void Win()
    {
        audiosource.PlayOneShot(SoundClips[4]);
    }
    public void Lose()
    {
        audiosource.PlayOneShot(SoundClips[5]);
    }

    public void StarIn()
    {
        audiosource.PlayOneShot(SoundClips[6]);
    }
    public void Fire()
    {
        audiosource.PlayOneShot(SoundClips[7]);
    }
    public void Gun()
    {
        audiosource.PlayOneShot(SoundClips[8]);
    }
    public void Boom()
    {
        audiosource.PlayOneShot(SoundClips[9]);
    }

}
