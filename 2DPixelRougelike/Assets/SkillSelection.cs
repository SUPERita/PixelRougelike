using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillSelection : StaticInstance<SkillSelection>, SubButtonListener
{
    [SerializeField] private RectTransform skillChoicesRoot;
    [AssetsOnly][SerializeField] private GameObject prefabSkillChoice;
    [SerializeField] private CanvasGroup canvasGroup = null;

    [SerializeField] private SkillCollection skillsCollection = null;
    private static string selectedSkillSaveLoc = "selectedSkills";

    private void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        
        Helpers.ToggleCanvas(canvasGroup, false);
    }

    //enter
    [ButtonGroup("a")]
    [Button]
    public void OpenSkillSelection()
    {
        GameStateManager.Instance.SetState(GameState.Shop);
        //fade in
        Time.timeScale = 0f;
        Helpers.ToggleCanvas(canvasGroup, true);
        // set selected
        EventSystem.current.SetSelectedGameObject(GetComponentInChildren<Button>().gameObject);

        //Choices SetUp
        SetUpChoices();

        //start highlight
        HighlightSelectedSavedButtons();

    }


    private void SetUpChoices()
    {
        //create and "subscribe"
        foreach (SkillNamePair skill in skillsCollection.skillNamePairs)
        {
            Instantiate(prefabSkillChoice, skillChoicesRoot)
                .GetComponent<SubButton>().InitializeButton(
                    this, 
                    skill._skillIcon, 
                    skill._skillName);
        }


    }
    //exit
    [ButtonGroup("a")]
    [Button]
    public void CloseCanvas()
    {
        GameStateManager.Instance.ReturnToBaseState();

        //destroy previous
        foreach (Transform child in skillChoicesRoot) Destroy(child.gameObject);

        //fade out
        Time.timeScale = 1f;
        //canvasGroup.alpha = 0;
        Helpers.ToggleCanvas(canvasGroup, false);

    }

    //buttons
    public void OnClicked(SubButton _button)
    {

        //can extend to 2+ simultanious skills but manipulating the saved array maybe making it into a queue or something
        //save
        SaveSystem.SaveStringArrayAtLocation(
            new string[] { _button._string1 },
            selectedSkillSaveLoc);

        HighlightSelectedSavedButtons();
        //SetHighlightedButtons(new SubButton[] {_button});
    }
    private void HighlightSelectedSavedButtons()
    {
        List<SubButton> _subButtons = new List<SubButton>(); ;
        foreach (string _s in GetSavedSelectedSkills())
        {
            _subButtons.Add(GetSubButtonFromSkillName(_s));
        }
        SetHighlightedButtons(_subButtons.ToArray());
    }



    //utils
    [Button]
    public void ToggleCanvasGroup()
    {
        Helpers.ToggleCanvas(canvasGroup);
    }

    private void SetHighlightedButtons(SubButton[] _buttons)
    {
        foreach(var _btn in GetComponentsInChildren<SubButton>()) {
            _btn.SetHighlight(false);
        }
        foreach (var _btn in _buttons)
        {
            _btn.SetHighlight(true);
        }
    }
    private SubButton GetSubButtonFromSkillName(string _skillName)
    {
        foreach (var _btn in GetComponentsInChildren<SubButton>())
        {
            if(_btn._string1 == _skillName) return _btn;
        }
        return null;
    }
    private static string[] GetSavedSelectedSkills()
    {
        string[] _s = SaveSystem.LoadStringArrayFromLocation(selectedSkillSaveLoc);

        return _s;
    }
    public static GameObject[] GetSelectedSavedSkills(SkillCollection _skillCollectionContext)
    {
        List<GameObject> _skills = new List<GameObject>();

        string[] _s = GetSavedSelectedSkills();
        foreach (string _skillname in _s)
        {
            _skills.Add(_skillCollectionContext.GetSkillFromName(_skillname));
        }

        return _skills.ToArray();
    }
}
