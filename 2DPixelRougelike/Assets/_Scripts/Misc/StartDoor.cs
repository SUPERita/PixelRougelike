using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class StartDoor : MonoBehaviour, IInteractible
{
    [SerializeField] private string doorName = "door";
    [SerializeField] private Sprite open;
    [SerializeField] private Sprite close;
    private SpriteRenderer sr;
    [SerializeField] private ParticleSystem openParticles;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = close;
        openParticles.Stop();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.GetComponent<PlayerMovement>() != null)
        {
            SceneManager.LoadScene("GameLoop");
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
        sr.sprite = open;
        openParticles.Play();
    }

    public void OnExitRange()
    {
        sr.sprite = close;
        openParticles.Stop();
    }

    public void OnInteract()
    {
        Debug.Log("talked to door");
    }

    public void OnStopInteract()
    {

    }
    #endregion
}
