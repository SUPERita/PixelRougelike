using Febucci.UI;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DeathPanel : StaticInstance<DeathPanel>
{
    [SerializeField] private CanvasGroup canvasGroup = null;

    private void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();

        Helpers.ToggleCanvas(canvasGroup, false);
    }

    //enter
    [ButtonGroup("a")]
    [Button]
    public void OpenDeathPanel()
    {
        //game state
        GameStateManager.Instance.SetState(GameState.Dead);
        Time.timeScale = 0f;

        //fade in
        Helpers.ToggleCanvas(canvasGroup, true);

        // set selected
        EventSystem.current.SetSelectedGameObject(GetComponentInChildren<Button>().gameObject);

        GetComponentInChildren<TextAnimator>().SetText("<rainb><pend>GAME OVER</pend></rainb>", true);// need to be true for the typewriter
        GetComponentInChildren<TextAnimatorPlayer>().StartShowingText();

    }

    //exit
    [ButtonGroup("a")]
    [Button]
    public void CloseCanvas()
    {
        GameStateManager.Instance.ReturnToBaseState();
        Time.timeScale = 1f;


        //fade out
        Helpers.ToggleCanvas(canvasGroup, false);
    }



    //utils
    [Button]
    public void ToggleCanvasGroup()
    {
        Helpers.ToggleCanvas(canvasGroup);
    }

    public void LoadLobby()
    {
        CloseCanvas();
        SceneManager.LoadScene("Lobby");
    }

}
