using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using TMPro.SpriteAssetUtilities;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class StartDoor : MonoBehaviour, IInteractible
{
    [SerializeField] private int sceneNumber = 0;
    [SerializeField] private string doorName = "door";
    [SerializeField] private Sprite open;
    [SerializeField] private Sprite close;
    [SerializeField] private SpriteRenderer lockSR;
    [SerializeField] private Canvas highscoreCanvas;
    [SerializeField] private GameObject crownSR;
    private SpriteRenderer sr;
    [SerializeField] private ParticleSystem openParticles;
    private bool working = true;
    [field: SerializeField] public KeyCode InteractionKey { get; set; } = KeyCode.None;
    [Header("demo stuff")]
    [InfoBox("uncheck for real build")]
    [SerializeField] private bool lockedForDemo = false;

    public readonly static string roomProgressSaveLocBase = "roomProgress";
    public readonly static string roomFinishedSavedLoc = "roomFinished";
    public void SetIsWorking(bool _b)
    {
        working = _b;
        RefreshDoorVisuals();
    }

    public void RefreshDoorVisuals()
    {
        //lock
        lockSR.gameObject.SetActive(!working);

        //highscore
        int _highscore = SaveSystem.LoadIntFromLocation(roomProgressSaveLocBase + sceneNumber);
        highscoreCanvas.enabled = working;
        highscoreCanvas.GetComponentInChildren<TextMeshProUGUI>().SetText(_highscore.ToString());

        //crown
        crownSR.gameObject.SetActive(SaveSystem.LoadBoolFromLocation(roomFinishedSavedLoc + sceneNumber));
    }

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = close;
        openParticles.Stop();

        if (lockedForDemo)
        {
            GameObject _d = new GameObject();
            _d.transform.parent = transform;
            _d.AddComponent<TextMeshPro>();
            _d.AddComponent<MeshRenderer>();
            _d.GetComponent<TextMeshPro>().SetText("Demo");
            _d.GetComponent<TextMeshPro>().color = Color.red;
            _d.GetComponent<TextMeshPro>().fontSize = 25f;
            _d.GetComponent<TextMeshPro>().sortingOrder = 100;
            _d.GetComponent<TextMeshPro>().alignment = TextAlignmentOptions.Center;
            _d.transform.rotation = Quaternion.Euler(0, 0, -45);
            _d.transform.position = transform.position;
            GetComponent<Collider2D>().enabled = false;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!working) { return; }
        if (collision.gameObject.GetComponent<PlayerMovement>() != null)
        {
            SceneManager.LoadScene(""+sceneNumber);
        }
    }

    #region interface
    public bool prompRefreshRequest { get; set; }

    public string GetInteractionPrompt()
    {
        return "<wiggle>" +"Room " + (sceneNumber+1) + "</>";
    }

    public void OnEnterRange()
    {
        if (!working) { return; }
        sr.sprite = open;
        openParticles.Play();

        AudioSystem.Instance.PlaySound("door_open", .75f);
    }

    public void OnExitRange()
    {
        if (!working) { return; }
        sr.sprite = close;
        openParticles.Stop();
        AudioSystem.Instance.PlaySound("squeak", .75f);
    }

    public void OnInteract()
    {
        if (!working) { return; }
        Debug.Log("talked to door");
    }

    public void OnStopInteract()
    {
        if (!working) { return; }
    }
    #endregion

    public void ResetRoomProgress()
    {
        SaveSystem.SaveIntAtLocation(0, roomProgressSaveLocBase + sceneNumber);
        SaveSystem.SaveBoolAtLocation(false, roomFinishedSavedLoc + sceneNumber);

    }
}
