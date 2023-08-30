using Febucci.UI;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Resources;
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
        //GameStateManager.Instance.SetState(GameState.Dead);
        Time.timeScale = 0f;

        //fade in
        Helpers.ToggleCanvas(canvasGroup, true);

        // set selected
        EventSystem.current.SetSelectedGameObject(GetComponentInChildren<Button>().gameObject);

        GetComponentInChildren<TextAnimator>().SetText("<gold><pend>GAME OVER</pend></gold>", true);// need to be true for the typewriter
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
        ResourceSystem.Instance.SetResource(ResourceType.Gold, 0);
        CloseCanvas();
        SceneManager.LoadScene("Lobby");
    }

}
