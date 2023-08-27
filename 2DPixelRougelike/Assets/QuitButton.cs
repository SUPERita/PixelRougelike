using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuitButton : MonoBehaviour
{
    private void Start()
    {
        checkIfShouldBeActive(SceneManager.GetActiveScene());
        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
    }
    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= SceneManager_sceneLoaded;
    }


    private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        checkIfShouldBeActive(arg0);
    }

    private void checkIfShouldBeActive(Scene arg0)
    {
        gameObject.SetActive((arg0.name == "Lobby"));
    }

    public void Button_Quit()
    {
        Application.Quit();
    }
}
