using System.Collections;
using System.Collections.Generic;
using System.Security.Permissions;
using UnityEngine;

public interface IInteractible 
{
    void OnExitRange();
    void OnEnterRange();
    void OnInteract();
    void OnStopInteract();
    string GetInteractionPrompt();

    [field: SerializeField] public KeyCode InteractionKey { get; set; }
    bool prompRefreshRequest { get; set; }


}
