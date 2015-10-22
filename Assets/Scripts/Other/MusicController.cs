using UnityEngine;
using System.Collections;

/// <summary>
/// background music in game
/// </summary>
public class MusicController : MonoBehaviour {

    public static MusicController Music;

    public AudioClip[] MusicClips;

    public AudioSource audiosource;

    void Awake()
    {
        if (Music == null)
        {
            DontDestroyOnLoad(gameObject);
            Music = this;
        }
        else if (Music != this)
        {
            Destroy(gameObject);
        }
        
    }

    public void MusicON()
    {
        audiosource.mute = false; 
    }

    public void MusicOFF(){
        audiosource.mute = true; 
        
    }

    public void BG_menu()
    {
        audiosource.clip = MusicClips[0];
        audiosource.Play();
    }

    public void BG_play()
    {
        audiosource.clip = MusicClips[1];
        audiosource.Play();
    }

}
