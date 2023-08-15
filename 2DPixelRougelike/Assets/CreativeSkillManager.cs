using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CreativeSkillManager : MonoBehaviour
{
    [SerializeField] private Transform root = null;
    [SerializeField] private SkillCollection _skillCollection;
    [SerializeField] private GameObject _InScenePrefab = null;
    [SerializeField] private GameObject _InScenePlusPrefab = null;
    private void Start()
    {
        SkillNamePair[] _selectedSkills = SkillSelection.GetSelectedSavedSkillsData(_skillCollection);
        int _i = 0;
        GameObject _lastPlus = null;
        foreach (SkillNamePair _pair in _selectedSkills)
        {
            GameObject _g = Instantiate(_InScenePrefab, root);
            _g.GetComponentsInChildren<Image>()[2].sprite = _pair._skillIcon;
            _g.GetComponentInChildren<TextMeshProUGUI>().text = _pair._skillName;
            _g.GetComponentInChildren<TextMeshProUGUI>().gameObject.transform.localPosition =
                _g.GetComponentInChildren<TextMeshProUGUI>().gameObject.transform.localPosition +
                Vector3.up * -35*_i;

            _g.SetActive(true);

            _i = _i == 1? 0 : 1;

            //plus 
            _lastPlus = Instantiate(_InScenePlusPrefab, root);
            _lastPlus.SetActive(true);
        }
        //GetComponentInChildren<HorizontalLayoutGroup>().spacing = ;
        Destroy(_lastPlus.gameObject);
    }
}
