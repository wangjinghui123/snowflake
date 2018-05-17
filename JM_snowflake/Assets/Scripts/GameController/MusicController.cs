using UnityEngine;
using System.Collections;
using DG.Tweening;

public class MusicController : MonoBehaviour
{
    public AudioSource startMusic;



    public void PlayeStartMusic()
    {
        startMusic.volume = 0;
        startMusic.Play();
        startMusic.DOFade(1, 3f);
    }
    public void StopStartMusic()
    {
        startMusic.DOFade(0, 2f)
        .OnComplete(
            () =>
            {
                startMusic.Stop();
            });
    }



    public AudioSource backGroundMusic;
    public void PlayeBackGroundMusic()
    {
        backGroundMusic.Play();
    }
    public void StopBackGroundMusic()
    {
        backGroundMusic.volume = 1;
        backGroundMusic.DOFade(0, 3f)
            .OnComplete(
            () =>
            {
                backGroundMusic.Stop();
            }
            );
    }



    public AudioSource CheerMusic;
    public void PlayeCheerMusicMusic()
    {
        CheerMusic.Play();
    }
    public void StopCheerMusicMusic()
    {
        CheerMusic.volume = 1;
        CheerMusic.DOFade(0, 3f)
            .OnComplete(
            () =>
            {
                CheerMusic.Stop();
            }
            );
    }



}
