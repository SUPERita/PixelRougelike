using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class MusicStarter : StaticInstance<MusicStarter>
{
    [SerializeField] private bool isLobby = false;
    private string curretSessionMusic = string.Empty;
    private void Start()
    {
        StartCoroutine(Helpers.DoNextFrame(()=> PlayCorrectMusic()));
        
    }

    private void PlayCorrectMusic()
    {
        if (isLobby)
        {
            //play lobby music
            curretSessionMusic = "music_lobby1";
        }
        else
        {
            //play fight music
            if (Random.value > .7f) curretSessionMusic = "music_battle1";
            else if (Random.value > .7f) curretSessionMusic = "music_battle2";
            else curretSessionMusic = "music_battle3";
        }

        AudioSystem.Instance.PlayMusic(curretSessionMusic);
    }
    public void PlayShopMusic()
    {
        //Debug.Log("helo");
        AudioSystem.Instance.PlayMusic("music_shop1");
    }
    public void ReturnToCurrentMusic()
    {
        AudioSystem.Instance.PlayMusic(curretSessionMusic);
    }
}
