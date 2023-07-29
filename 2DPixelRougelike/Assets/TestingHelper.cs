using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingHelper : MonoBehaviour
{
    [SerializeField] private RectTransform root = null;
    private void Start()
    {
#if !UNITY_EDITOR
        gameObject.SetActive(false);
        return;
#else 
        gameObject.SetActive(true);
        return;
#endif 
        
    }
    public void AddLevel()
    {
        XPManager.Instance.AddXP(100000);
    }
    public void removeWeapons()
    {
        WeaponManager.Instance.Test_ClearWeaponSockets();
    }
    public void HideCanvas()
    {
        root.anchoredPosition = new Vector3 (-root.anchoredPosition.x, root.anchoredPosition.y, 0);
    }
}
