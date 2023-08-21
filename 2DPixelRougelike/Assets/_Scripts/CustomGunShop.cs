using System.Collections;
using System.Collections.Generic;
using System.Configuration;

using UnityEngine;
using UnityEngine.UI;

public class CustomGunShop : MonoBehaviour, SubButtonListener
{
    [SerializeField] private GameObject _c = null;
    [SerializeField] private RectTransform root = null;
    [SerializeField] private WeaponCollection weaponCollection = null;

    public void OnClicked(SubButton _button)
    {
        WeaponManager.Instance.TryAddWeapon((Weapon)_button.behav1);
    }

    public void ToggleCanvas()
    {
        _c.gameObject.SetActive(!_c.activeSelf);
    }

    private void Start()
    {
        GameObject _template = new GameObject();
        _template.AddComponent<Image>();
        _template.AddComponent<SubButton>();
        _template.AddComponent<Button>();
       

        foreach (var weapon in weaponCollection.weaponIconPairs)
        {
           GameObject _inst = Instantiate(_template, root);
            _inst.GetComponent<Image>().sprite = weapon._weaponIcon;
            _inst.GetComponent<SubButton>().InitializeButton(this, null, "empty", -1);
            _inst.GetComponent<SubButton>().AddAdditionalData(weapon._weapon);

            _inst.GetComponent<Button>().onClick.AddListener(() => _inst.GetComponent<SubButton>().Button_OnClick());
        }

        Destroy(_template );
    }
}
