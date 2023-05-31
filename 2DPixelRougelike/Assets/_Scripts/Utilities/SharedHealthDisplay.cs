using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SharedHealthDisplay : MonoBehaviour
{
    [SerializeField] private Health health;
    [SerializeField] private GameObject healthBarRoot;
    [SerializeField] private Vector3 offset = Vector3.up;
    private Image healthImage;
    private Transform healthRoot = null;
    private Camera _cam = null;
    SharedCanvas sharedCanvas = null;
    private void Start()
    {
        health.OnTakeDamage += Health_OnTakeDamage;
        health.OnDie += Health_OnDie;

        sharedCanvas = FindObjectOfType<SharedCanvas>();
        _cam = sharedCanvas._mainCameraRef;

        healthRoot = Instantiate(healthBarRoot, sharedCanvas.transform).transform;
        healthImage = healthRoot.GetComponentInChildren<Image>();

        healthImage.fillAmount = 1f;
    }
    private void OnDisable()
    {
        health.OnTakeDamage -= Health_OnTakeDamage;
        health.OnDie -= Health_OnDie;
    }

    private IEnumerator DoNextFrame(UnityAction _a)
    {
        yield return new WaitForEndOfFrame();
        _a.Invoke();
    }


    private void Health_OnDie()
    {
        Destroy(healthRoot.gameObject);
        //Debug.LogError("RemoveFromCanvas");//can add a canvas group to the image to fade it
    }
    private void Health_OnTakeDamage(int obj)
    {
        healthImage.fillAmount = (float)health.GetCurrrentHealth() / (float)health.GetBaseHealth();
    }

    private void LateUpdate()
    {
        if (!healthRoot) { return; }
        //if(_cam == null)
        //{
        //    //Debug.Log(SharedCanvas.Instance.name);
        //    //_cam = SharedCanvas.Instance._mainCameraRef;
        //    DoNextFrame(()=> _cam = Camera.main);
        //    //_cam = Camera.main;
        //}
        //else
        //{
        //}

            healthRoot.position = _cam.WorldToScreenPoint(transform.position) + offset;
    }
}