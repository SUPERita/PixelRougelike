using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.Universal.Internal;

public class DoorManager : MonoBehaviour
{
    private static readonly string progressSaveLoc = "doorProgress";
    [SerializeField] private StartDoor[] doors = null;
    private static int doorAmount = 0;
    private void Start()
    {
        doorAmount = doors.Length;
        if (GetDoorProgress() == null) { SetStartDoors(); }
        if (GetDoorProgress().Length != doorAmount) { SetStartDoors(); }

        for (int i = 0; i < doors.Length; i++)
        {
            Debug.Log(i);
            if (GetDoorProgress()[i])
            {
                doors[i].SetIsWorking(true);
            } 
            else
            {
                doors[i].SetIsWorking(false);
            }
        }
    }

    private static bool[] GetDoorProgress()
    {
        return SaveSystem.LoadBoolArrayFromLocation(progressSaveLoc);
    }

    
    
    /// <summary>
    /// only unlocks the next level, not the ones behind or in between the levels
    /// </summary>
    /// <param name="_index"></param>
    [Button]
    
    public static void CompleatedLevel(int _index)
    {
        ////debug
        //string _s = "";
        //_s = "";
        //foreach (bool _b in GetDoorProgress())
        //{
        //    _s += _b.ToString() + ", ";
        //}
        //Debug.Log(_s);

        //actually funciton
        bool[] bools = GetDoorProgress();
        bools[_index+1] = true;

        SaveSystem.SaveArrayBoolAtLocation(bools, progressSaveLoc);

        //debug
        //_s = "";
        //foreach (bool _b in GetDoorProgress())
        //{
        //    _s += _b.ToString() + ", ";
        //} 
        //Debug.Log(_s);
    }
    [Button]
    private static void SetStartDoors()
    {
        bool[] bools = new bool[doorAmount];
        for (int i = 0; i < bools.Length; i++)
        {
            bools[i] = false;
        }
        bools[0] = true;

        SaveSystem.SaveArrayBoolAtLocation(bools, progressSaveLoc);
    }
}