using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour, IInteractible
{

    [SerializeField] private string interactionPrompt = "";
    Vector2 startScale;
    private void Start()
    {
        startScale = transform.localScale;
    }

    public virtual string GetInteractionPrompt()
    {
        return interactionPrompt;
    }
    public virtual void OnInteract()
    {
        Debug.Log("hello im " + name);
        //Vector2 startScale = transform.localScale;
        AudioSystem.Instance.PlaySound("s2");
        transform.DOShakeScale(.3f,1,15).SetUpdate(true).OnComplete(()=>transform.localScale = startScale);
    }

    public virtual void OnStopInteract()
    {
        Debug.Log("good bye");
    }

    public virtual void OnEnterRange()
    {
        GetComponent<SpriteRenderer>().color = Color.red;
    }

    public virtual void OnExitRange()
    {
        GetComponent<SpriteRenderer>().color = Color.white;
    }
}
