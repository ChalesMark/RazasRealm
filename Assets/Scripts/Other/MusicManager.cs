using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Music { menu,hub,level,death}
public class MusicManager : MonoBehaviour
{
    public AudioClip menuMusic, hubMusic, levelMusic, deathMusic, LevelCompleteMusic;
    AudioSource audioSource;
    public Music currentMusic = Music.menu;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public Music GetMusic()
    {
        return currentMusic;
    }

    public void PlayVictory()
    {
        audioSource.clip = LevelCompleteMusic;
        audioSource.loop = false;
        audioSource.Play();
        Invoke("PlayHubMusic", LevelCompleteMusic.length);
    }

    private void PlayHubMusic()
    {
        Play(Music.hub);
    }

    public void Play(Music music)
    {
        currentMusic = music;
        switch (music)
        {
            case Music.menu:
                audioSource.clip = menuMusic;
                audioSource.loop = false;
                audioSource.Play();
                break;
            case Music.hub:
                audioSource.clip = hubMusic;
                audioSource.loop = true;
                audioSource.Play();
                break;
            case Music.level:
                audioSource.clip = levelMusic;
                audioSource.loop = true;
                audioSource.Play();
                break;
            case Music.death:
                audioSource.clip = deathMusic;
                audioSource.loop = false;
                audioSource.Play();
                break;
        }
    }
}
