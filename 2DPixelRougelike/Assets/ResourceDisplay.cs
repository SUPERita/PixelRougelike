using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class ResourceDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI goldText;
    [SerializeField] private TextMeshProUGUI energyNuggetText;
    [SerializeField] private CanvasGroup canvasGroup;

    
    void Start()
    {
        ResourceSystem.Instance.OnResourceChanged += Instance_OnResourceChanged;

        //first time load for the event
        //needs to be delayed at start so the resource system initializes before it
        StartCoroutine(DoNextFrame(()=>
            ResourceSystem.Instance.ResourceDisplay_StartCallChangeEvent())
        );
        
    }
    private void OnDisable()
    {
        if (ResourceSystem.Instance == null) return;
        ResourceSystem.Instance.OnResourceChanged -= Instance_OnResourceChanged;
    }

    private IEnumerator DoNextFrame(UnityAction _a)
    {
        yield return new WaitForEndOfFrame();
        _a.Invoke();
    }
    private void Instance_OnResourceChanged(ResourceType arg1, int _change)
    {
        switch (arg1)
        {
            case ResourceType.Gold:
                goldText.SetText(""+ ResourceSystem.Instance.GetResourceAmount(arg1));
                break;
            case ResourceType.EnergyNugget:
                energyNuggetText.SetText("" + ResourceSystem.Instance.GetResourceAmount(arg1));
                break;
        }
        //Debug.Log(ResourceSystem.Instance.GetResourceAmount(arg1));
    }


    

    [Button]
    public void ToggleCanvasGroup()
    {
        canvasGroup.alpha = canvasGroup.alpha == 1 ? 0 : 1;
    }
}
