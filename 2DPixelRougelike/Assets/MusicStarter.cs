using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicStarter : MonoBehaviour
{
    [SerializeField] private bool isLobby = false;

    private void Start()
    {
        if (isLobby)
        {
            //play lobby music
            AudioSystem.Instance.PlayMusic("music_lobby1");
        } else
        {
            //play fight music
            if(Random.value > .7f) AudioSystem.Instance.PlayMusic("music_battle1");
            else if (Random.value > .7f) AudioSystem.Instance.PlayMusic("music_battle2");
            else AudioSystem.Instance.PlayMusic("music_battle3");
        }
    }
}
