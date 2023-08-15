using Febucci.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCSpeechBox : MonoBehaviour
{
    [SerializeField] private NPC _npc = null;
    [SerializeField] private TextAnimatorPlayer textAnimatorPlayer = null;
    [SerializeField] string[] helloSentences = null;
    [SerializeField] string[] byeSentences = null;
    [SerializeField] private bool endSighSound = false;
    void Start()
    {

        _npc.OnEnterNotify += _npc_OnEnterNotify;
        _npc.OnExitNotify += _npc_OnExitNotify;
        _npc.OnInteractNotify += _npc_OnInteractNotify;
    }
    private void OnDisable()
    {
        _npc.OnEnterNotify -= _npc_OnEnterNotify;
        _npc.OnExitNotify -= _npc_OnExitNotify;
        _npc.OnInteractNotify -= _npc_OnInteractNotify;
    }

    private void _npc_OnInteractNotify()
    {
        
    }
    private void _npc_OnExitNotify()
    {
        SetSpeechText(byeSentences[Random.Range(0, byeSentences.Length)]);
        //SetSpeechText("Good luck!");
    }
    private void _npc_OnEnterNotify()
    {
        SetSpeechText(helloSentences[Random.Range(0, helloSentences.Length)]);
        //SetSpeechText("They Wishlisted, my king");
    }

    private void SetSpeechText(string _text = "")
    {
        textAnimatorPlayer.textAnimator.SetText(_text, true);
        textAnimatorPlayer.StartShowingText(true);
        CancelInvoke(nameof(ClearSpeechText));
        Invoke(nameof(ClearSpeechText), 3f);
    }
    private void ClearSpeechText()
    {
        textAnimatorPlayer.StartDisappearingText();
    }

    int _countSpeech = 0;
    public void MakeTalkingSound(char _c)
    {
        _countSpeech++;
        if(_countSpeech == 2) {
            AudioSystem.Instance.PlaySound("wizard_talk1", .7f, 1 + Random.value / 20);
            _countSpeech = 0;
        }
    }
    public void MakeSighSound()
    {
        if (!endSighSound) { return; }
        AudioSystem.Instance.PlaySound("wizard_sigh", .5f, .85f + Random.value / 5);
    }

}
