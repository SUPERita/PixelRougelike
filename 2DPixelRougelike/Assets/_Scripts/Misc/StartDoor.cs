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
    private SpriteRenderer sr;
    [SerializeField] private ParticleSystem openParticles;
    public bool working = true;

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

    public string GetInteractionPrompt()
    {
        return "-1";
    }

    public void OnEnterRange()
    {
        if (!working) { return; }
        sr.sprite = open;
        openParticles.Play();
    }

    public void OnExitRange()
    {
        if (!working) { return; }
        sr.sprite = close;
        openParticles.Stop();
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
