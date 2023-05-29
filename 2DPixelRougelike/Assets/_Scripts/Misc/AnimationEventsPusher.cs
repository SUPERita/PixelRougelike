using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class AnimationEventsPusher : MonoBehaviour
{
     
    private IAnimationEventsReciever reciever = null;
    [SerializeField] private GameObject recieverObject = null;

    private void Start()
    {
        reciever = recieverObject.GetComponent<IAnimationEventsReciever>();
    }
#if UNITY_EDITOR
    // Code to exclude from the build
    [Button]
    private void LocateParentReciever()
    {
        Selection.activeGameObject = recieverObject;
        EditorGUIUtility.PingObject(recieverObject);
    }
#endif

    

    public void Animator_CallEvent(string _eventName)
    {
        reciever.OnEventCalled(_eventName);
    }

}

public interface IAnimationEventsReciever
{
    void OnEventCalled(string _eventName);
}
