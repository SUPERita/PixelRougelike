using System.Collections;
using System.Collections.Generic;
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
    private SpriteRenderer sr;
    [SerializeField] private ParticleSystem openParticles;
    private bool working = true;

    public void SetIsWorking(bool _b)
    {
        working = _b;
        lockSR.gameObject.SetActive(!_b);
    }

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = close;
        openParticles.Stop();
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
    [field: SerializeField] public KeyCode InteractionKey { get; set; } = KeyCode.None;

    public string GetInteractionPrompt()
    {
        return "<wiggle>" +"Room " + sceneNumber + "</>";
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
}
