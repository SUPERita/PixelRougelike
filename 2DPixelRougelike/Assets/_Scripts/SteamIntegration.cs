using Sirenix.OdinInspector;
using Steamworks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SteamIntegration : MonoBehaviour
{
    [SerializeField] private bool showUtils = false;
    [SerializeField, ShowIf("showUtils")] private string ach = "ACH_ENTER";
    private void Start()
    {
        try
        {
            Steamworks.SteamClient.Init(2498620);
        } 
        catch (System.Exception e) 
        {
            Debug.Log(e);
        }
    }
    private void Update()
    {
        Steamworks.SteamClient.RunCallbacks();
    }
    private void OnApplicationQuit()
    {
        Steamworks.SteamClient.Shutdown();
    }

    #region Achievments
    [Button]
    public static void UnlockAchievment(string _achname)
    {
        var _ach = new Steamworks.Data.Achievement(_achname);
        _ach.Trigger();
//#if UNITY_EDITOR
//        Debug.Log(_ach + ": has been called");
//#endif
        //Steamworks.Data.Achievement()
    }
    [Button, ShowIf("showUtils")]
    public void PrintSteamName()
    {
        Debug.Log(Steamworks.SteamClient.Name);
    }
    [Button, ShowIf("showUtils")]
    public void ClearAchievment()
    {
        var _ach = new Steamworks.Data.Achievement(ach);
        _ach.Clear();
    }
    [Button, ShowIf("showUtils")]
    private void IsAchivementUnlocked()
    {
        var _ach = new Steamworks.Data.Achievement(ach);
        Debug.Log(_ach.Name + ": "+_ach.State);
    }
    [Button, ShowIf("showUtils")]
    private void ClearAllAchivements()
    {
        foreach (var a in SteamUserStats.Achievements)
        {
            a.Clear();
        }
    }
    #endregion



}
