using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//visit when add resource
public enum ResourceType
{
   Gold,
   EnergyNugget
}

public class ResourceSystem : Singleton<ResourceSystem>
{
    private string resourceLocRoot = "resourcesLoc";
    public event Action<ResourceType, int> OnResourceChanged;// type and change
    //visit when add resource
    private PlayerResource goldResource = new PlayerResource(ResourceType.Gold, 0);
    private PlayerResource energyNuggetResource = new PlayerResource(ResourceType.EnergyNugget, 0);

    // on start because the displays are delayed by a frame
    private void Start()
    {
        LoadResources();
        InvokeRepeating(nameof(SaveLoop),1,1);
    }
    private void SaveLoop()
    {
        //Debug.Log("saveloop");
        SaveResources();
    }

    public int GetResourceAmount(ResourceType _t)
    {
        return TypeToResource(_t)._amount;
    }
    private void SetResourceAmount(ResourceType _t, int _new)
    {
        TypeToResource(_t).SetAmount(_new);
    }
    public void AddResourceAmount(ResourceType _t, int _add)
    {
        int _tmp = _add;
        if (_t == ResourceType.Gold) { 
            _tmp = (int)(_add * (1f+PlayerStatsHolder.Instance.TryGetStat(StatType.MoneyGain)/100f));
            //Debug.Log(_tmp);
        }

        TypeToResource(_t).SetAmount(TypeToResource(_t)._amount + _tmp);
        OnResourceChanged?.Invoke(_t, _add);
    }
    public void TakeResourceAmount(ResourceType _t, int _take)
    {
        TypeToResource(_t).SetAmount(TypeToResource(_t)._amount - _take);
        OnResourceChanged?.Invoke(_t, -_take);
    }
    public bool HasEnougthResources(ResourceType _t, int _val)
    {
        return TypeToResource(_t)._amount - _val >= 0;
    }
    [Button]
    public void ResetResource(ResourceType _t)
    {
        TypeToResource(_t).SetAmount(0);
    }
    [Button]
    public void SetResource(ResourceType _t, int amt)
    {
        TypeToResource(_t).SetAmount(amt);
    }

    //visit when add resource
    private void SaveResources()
    {
        //no saving gold
        //SaveSystem.SaveIntAtLocation(GetResourceAmount(ResourceType.Gold), resourceLocRoot + ResourceTypeToString(ResourceType.Gold));
        SaveSystem.SaveIntAtLocation(GetResourceAmount(ResourceType.EnergyNugget), resourceLocRoot + ResourceTypeToString(ResourceType.EnergyNugget));
    }
    //visit when add resource
    //only call this once per application Open!
    private void LoadResources()
    {
        //no saving gold
        //SetResourceAmount(
        //    ResourceType.Gold,
        //    SaveSystem.LoadIntFromLocation(resourceLocRoot + ResourceTypeToString(ResourceType.Gold))
        //    );
        //Debug.Log("resource load");

        SetResourceAmount(
            ResourceType.EnergyNugget,
            SaveSystem.LoadIntFromLocation(resourceLocRoot + ResourceTypeToString(ResourceType.EnergyNugget))
            );
    }

    private void OnDisable()
    {
        CancelInvoke(nameof(SaveLoop));
        //actually save
        SaveResources();
        Debug.Log("Application ending after " + Time.time + " seconds");
    }
    

    /*
    public int GetResource(ResourceType _t)
    {
        return SaveSystem.LoadIntFromLocation(resourceLocRoot+ResourceTypeToString(_t), 0);
    }
    public void SetResource(ResourceType _t, int _val)
    {
        SaveSystem.SaveIntAtLocation(_val, resourceLocRoot + ResourceTypeToString(_t));
        //OnResourceChanged?.Invoke(_t, 0);
    }
    public void AddResource(ResourceType _t, int _add)
    {
        SetResource(_t, GetResource(_t) + _add);
        OnResourceChanged?.Invoke(_t, _add);
    }
    public void TakeResource(ResourceType _t, int _take)
    {
        SetResource(_t, GetResource(_t) - _take);
        OnResourceChanged?.Invoke(_t, -_take);
    }

    public void ResetResource(ResourceType _t)
    {
        OnResourceChanged?.Invoke(_t, -GetResource(_t));
        SetResource(_t, 0);
    }
    */


    //utils
    //visit when add resource
    private PlayerResource TypeToResource(ResourceType _t)
        {
            switch (_t)
            {
                case ResourceType.EnergyNugget:
                    return energyNuggetResource;
                case ResourceType.Gold:
                    return goldResource;
            }
            Debug.LogError("very wierd this happened");
            return new PlayerResource(ResourceType.Gold, -1);
        }
    //visit when add resource
    public void ResourceDisplay_StartCallChangeEvent()
    {
        //Debug.Log("resource update");
        OnResourceChanged?.Invoke(ResourceType.EnergyNugget, TypeToResource(ResourceType.EnergyNugget)._amount);
        OnResourceChanged?.Invoke(ResourceType.Gold, TypeToResource(ResourceType.Gold)._amount);
    }
    //visit when add resource
    public string ResourceTypeToString(ResourceType _t)
    {
        switch(_t)
        {
            case ResourceType.Gold:
                return "Gold";
            case ResourceType.EnergyNugget:
                return "EnergyNugget";
        }
        return "N/A";

    }



}

//when it was a struct i couldent change it
[Serializable]
public class PlayerResource
{
    public ResourceType _type { get; private set; }
    public int _amount { get; private set; }

    public PlayerResource(ResourceType _t, int _val)
    {
        _type = _t;
        _amount = _val;
    }

    public void SetAmount(int _new)
    {
        //Debug.Log(_amount);
        _amount = _new;
        //Debug.Log(_amount);
    }
}
