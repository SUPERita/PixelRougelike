using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEditor;

public class SkillSelection : StaticInstance<SkillSelection>, SubButtonListener
{
    [SerializeField] private RectTransform skillChoicesRoot;
    [AssetsOnly][SerializeField] private GameObject prefabSkillChoice;
    [SerializeField] private CanvasGroup canvasGroup = null;

    [SerializeField] private SkillCollection skillsCollection = null;
    private static string selectedSkillSaveLoc = "selectedSkills";

    private static Stack<string> skillStack = new Stack<string>();

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

        //skillStack initialazation
        ConvertSkillSaveToStack();

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
                    skill._skillName)
                .AddAdditionalData(skill._skillCost);
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
        SaveChoiceClicked(_button._string1);

        HighlightSelectedSavedButtons();
        //SetHighlightedButtons(new SubButton[] {_skillName});
    }
    private void HighlightSelectedSavedButtons()
    {
        List<SubButton> _subButtons = new List<SubButton>();
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
    private void SetHighlightedButtons(SubButton[] _selectedBtns)
    {
        
        foreach(var _btn in GetComponentsInChildren<SubButton>()) {
            if(Contains(_selectedBtns, _btn)) { continue; }
            _btn.SetHighlight(false);
        }
        foreach (var _btn in _selectedBtns)
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
    [Button]
    //"reset skills"
    public void ResetLockedSubButtons()
    {
        foreach(LockedSubButton _b in GetComponentsInChildren<LockedSubButton>())
        {
            _b.SetIsUnlocked(false);
            _b.SetVisual(false);
        }

        //reset save
        SaveSystem.SaveStringArrayAtLocation(new string[0], selectedSkillSaveLoc);

        //rehighlight
        HighlightSelectedSavedButtons();
    }

    public bool Contains(SubButton[] _arr, SubButton _btn)
    {
        foreach(SubButton _b in _arr)
        {
            if(_b ==  _btn) return true;
        }
        return false;
    }


    //save interaction
    private static bool IsInCurrentSave(string _skillName)
    {
        foreach (var _s in GetSavedSelectedSkills())
        {
            if (_s==_skillName)
            {
                return true;
            }
        }
        return false;
    }
    private static void ConvertSkillSaveToStack()
    {
        skillStack = new Stack<string>();
        foreach (string _s in GetSavedSelectedSkills())
        {
            skillStack.Push(_s);
        }
    }
    private static string[] GetSavedSelectedSkills()
    {
        string[] _s = SaveSystem.LoadStringArrayFromLocation(selectedSkillSaveLoc);

        int _numberOfAllowedSkills = PlayerStatsHolder.Instance.TryGetStat(StatType.SkillCap);

        //if more skills are saved then are alowed
        if(_s.Length > _numberOfAllowedSkills)
        {
            List<string> _cutSavedSkills = new List<string>();
            //add skills until its the right amount
            for (int i = 0; i < _numberOfAllowedSkills; i++)
            {
                _cutSavedSkills.Add(_s[i]);
                //Debug.Log(_s[i]);
            }

            //fix the save
            SaveSystem.SaveStringArrayAtLocation(_cutSavedSkills.ToArray(), selectedSkillSaveLoc);
            _s = _cutSavedSkills.ToArray();
        }

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
    private static void SaveChoiceClicked(string _skillName)
    {
        //if already choosen ignore
        if (IsInCurrentSave(_skillName)) { return; }


        //recreate the stack
        ConvertSkillSaveToStack();

        //copy the stack
        Stack<string> _skillStackCopy = new Stack<string>(skillStack);

        //add to the stack
        _skillStackCopy.Push(_skillName);

        //get the last n skills
        List<string> _result = new List<string>();
        int _numberOfSkills = PlayerStatsHolder.Instance.TryGetStat(StatType.SkillCap);
        for (int i = 0; i < _numberOfSkills; i++)
        {
            if(_skillStackCopy.Count == 0) { continue; }
            _result.Add(_skillStackCopy.Pop());
            //Debug.Log(_result[_result.Count-1]);
        }
        

        //save the last n elements of the stack?!
        SaveSystem.SaveStringArrayAtLocation(_result.ToArray(), selectedSkillSaveLoc);
                    
                   
    }

}
