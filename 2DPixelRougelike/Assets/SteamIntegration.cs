using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteamIntegration : MonoBehaviour
{
    [SerializeField] private string ach = "ACH_ENTER";
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
    public void PrintSteamName()
    {
        Debug.Log(Steamworks.SteamClient.Name);
    }
    [Button]
    public void UnlockAchievment()
    {
        var _ach = new Steamworks.Data.Achievement(ach);
        _ach.Trigger();
    }
    [Button]
    public void ClearAchievment()
    {
        var _ach = new Steamworks.Data.Achievement(ach);
        _ach.Clear();
    }

    [Button]
    private void IsAchivementUnlocked()
    {
        var _ach = new Steamworks.Data.Achievement(ach);
        Debug.Log(_ach.Name + ": "+_ach.State);
    }
    #endregion

    #region GUI
    //// Define the position and size of the buttons
    //private Rect buttonRect1 = new Rect(0, 50, 100, 50);
    //private Rect buttonRect2 = new Rect(0, 100, 100, 50);

    //private void OnGUI()
    //{
    //    // Draw the first button
    //    if (GUI.Button(buttonRect1, "UnlockAchievment"))
    //    {
    //        // Button 1 is clicked, do something
    //        UnlockAchievment(ach);
    //    }

    //    // Draw the second button
    //    if (GUI.Button(buttonRect2, "ClearAchievment"))
    //    {
    //        // Button 2 is clicked, do something
    //        ClearAchievment(ach);
    //    }

    //}
    #endregion

}
