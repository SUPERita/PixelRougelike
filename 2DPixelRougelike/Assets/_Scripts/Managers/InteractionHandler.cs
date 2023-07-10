using Febucci.UI;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InteractionHandler : MonoBehaviour
{
    [SerializeField] private Image keyPrompt;
    [SerializeField] private TextMeshProUGUI promptText;
    [SerializeField] private TextAnimator promptAnimator;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private float checkRate = .2f;
    [SerializeField] private float checkRange = 3f;
    [SerializeField] private float fadeSpeed = .5f;
    IInteractible currentInteractible = null;
    IInteractible lastInteractible = null;
    private void Start()
    {
        InvokeRepeating(nameof(SearchLoop), 0, checkRate);
    }


    private float targetOpacity = 0f;
    private void Update()
    {
        canvasGroup.alpha = Mathf.Lerp(
            canvasGroup.alpha,
            targetOpacity,
            fadeSpeed*Time.deltaTime*25f*(2-canvasGroup.alpha));
    }
    
    private void SearchLoop()
    {
        currentInteractible = GetClosestInteractible();

        //handle switches 
        if (lastInteractible != currentInteractible)
        {
            lastInteractible?.OnExitRange();
            if (currentInteractible != null)
            {
                //promptText.text = "";
                currentInteractible.OnEnterRange();
                promptAnimator.SetText(currentInteractible.GetInteractionPrompt(), false);
            }
        }
        lastInteractible = currentInteractible;

        //hanlde refresh prompt requests
        if (currentInteractible != null && currentInteractible.prompRefreshRequest)
        {
            currentInteractible.prompRefreshRequest = false;
            currentInteractible.OnEnterRange();
            promptAnimator.SetText(currentInteractible.GetInteractionPrompt(), false);
        }

        //if no interactibles near hide promp
        if (currentInteractible == null) {
            targetOpacity = 0f;
            return; 
        }

        //if should show promp show,
        if(currentInteractible.GetInteractionPrompt() != "-1")
        {
            //if one is near show key prompt
            keyPrompt.enabled = true;
            //promptText.text = currentInteractible.GetInteractionPrompt();
            targetOpacity = .85f;
        }
        //else 
        else
        {
            keyPrompt.enabled = false;
            targetOpacity = 0f;
        }

        

    }

    Collider2D[] collidersInRange = new Collider2D[20];
    List<Collider2D> InteractiblesInRange = new List<Collider2D>();
    /// <summary>
    /// 
    /// </summary>
    /// <returns>closest Interactible / null</returns>
    private IInteractible GetClosestInteractible()
    {
        //works
        int numColliders = Physics2D.OverlapCircleNonAlloc(transform.position, checkRange, collidersInRange); // the int is important // works

        //filter them for only IInteractible
        InteractiblesInRange.Clear();
        for (int i = 0; i < numColliders; i++)
        {
            if (collidersInRange[i].gameObject.GetComponent<IInteractible>() == null) { continue; }
            InteractiblesInRange.Add(collidersInRange[i]);
        }
        //foreach (Collider2D _collider in collidersInRange)//thats a sneaky one, watch out for checking empty slots in collidrs in range, it has 20
        //{
        //    if(_collider.gameObject.GetComponent<IInteractible>() == null) { continue; }
        //    InteractiblesInRange.Add(_collider);
        //}

        //if no interactibles no go
        if(InteractiblesInRange.Count == 0){ return null; }

        //get the closest one
        float minDis = Mathf.Infinity;
        int closestIndex = -1;
        IInteractible target = null;
        for (int i = 0; i < InteractiblesInRange.Count; i++)
        {
            float crntDis = (transform.position - InteractiblesInRange[i].transform.position).sqrMagnitude;
            if (crntDis < minDis)
            {
                minDis = crntDis;
                closestIndex = i;
            }
        }

        target = closestIndex == -1 ? null : InteractiblesInRange[closestIndex].GetComponent<IInteractible>();
        return target;
    }

    public void OnInteract(InputValue _value)
    {
        //in another menu
        if(GameStateManager.Instance.GetCurrentGameState() != GameState.GameLoop) { return; }

        if(currentInteractible != null)
        {
            currentInteractible.OnInteract();
        } else
        {
            Debug.Log("no one in range");
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, checkRange);
    }
}
