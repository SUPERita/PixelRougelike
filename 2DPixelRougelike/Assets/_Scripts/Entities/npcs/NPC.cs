using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour, IInteractible
{

    [SerializeField] private string interactionPrompt = "";
    Vector2 startScale;

    public bool prompRefreshRequest { get; set; } = false;

    protected virtual void Start()
    {
        startScale = transform.localScale;
    }

    public virtual string GetInteractionPrompt()
    {
        return interactionPrompt;
    }
    public virtual void OnInteract()
    {
        //Debug.Log("hello im " + name);
        //Vector2 startScale = transform.localScale;
        AudioSystem.Instance.PlaySound("s2");
        transform.DOShakeScale(.2f,.25f,1).SetUpdate(true).OnComplete(()=>transform.localScale = startScale);
    }

    public virtual void OnStopInteract()
    {
        Debug.Log("good bye");
    }

    public virtual void OnEnterRange()
    {
        // Get the current material of the renderer
        Material material = GetComponent<SpriteRenderer>().material;

        // Set the new value for outbase_on directly on the material
        material.SetFloat("_OutlineAlpha", 1);
        //material.EnabledKeyword("OUTBASE_ON");
    }

    public virtual void OnExitRange()
    {
        //GetComponent<SpriteRenderer>().color = Color.white;
        // Get the current material of the renderer
        Material material = GetComponent<SpriteRenderer>().material;
        material.SetFloat("_OutlineAlpha", 0);
        // Set the new value for outbase_on directly on the material
        //material.SetInt("OUTBASE_ON", 0);
    }

    //public bool prompRefreshRequest { get; private set; } = false;

}
