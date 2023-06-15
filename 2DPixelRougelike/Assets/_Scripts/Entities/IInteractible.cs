using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractible 
{
    void OnExitRange();
    void OnEnterRange();
    void OnInteract();
    void OnStopInteract();
    string GetInteractionPrompt();

}
