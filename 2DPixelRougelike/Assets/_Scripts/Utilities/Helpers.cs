using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Events;
using System.Collections;

/// <summary>
/// A static class for general helpful methods
/// </summary>
public static class Helpers
{
    /// <summary>
    /// Destroy all child objects of this transform (Unintentionally evil sounding).
    /// Use it like so:
    /// <code>
    /// transform.DestroyChildren();
    /// </code>
    /// </summary>
    public static void DestroyChildren(this Transform t)
    {
        if(t == null) { Debug.LogWarning("idk it just sometimes says that on non null object just let it be"); return; }
        foreach (Transform child in t) Object.Destroy(child.gameObject);
    }

    public static void ToggleCanvas(CanvasGroup _c)
    {
        if (_c == null) return;
        _c.alpha = 1-_c.alpha;
        _c.interactable = !_c.interactable;
        _c.blocksRaycasts = !_c.blocksRaycasts;
    }
    public static void ToggleCanvas(CanvasGroup _c, bool _state)
    {
        if (_c == null) return;
        _c.alpha = _state? 1:0;
        _c.interactable = _state;
        _c.blocksRaycasts = _state;
    }

    public static void SelectSomethingUnder(Transform _t)
    {
        foreach (Button _b in _t.GetComponentsInChildren<Button>())
        {
            if (_b.IsInteractable())
            {
                EventSystem.current.SetSelectedGameObject(_b.gameObject);
                break;
            }
        }
    }

    /// <summary>
    /// % out of 100
    /// </summary>
    /// <param name="_f"></param>
    /// <returns></returns>
    public static bool RollChance(float _f)
    {
        //Debug.Log(_f);
        if (_f == 0) return false;

        return _f >= Random.Range(0f, 100f);
    }


    public static IEnumerator DoInTime(UnityAction _A, float time = 0f)
    {
        Debug.Log("triple check this works good");
        yield return new WaitForSeconds(time);
        _A.Invoke();
    }

    public static int FilterLetters(string input)
    {
        string digitsOnly = string.Empty;

        for (int i = 0; i < input.Length; i++)
        {
            if (char.IsDigit(input[i]))
            {
                digitsOnly += input[i];
            }
        }

        int result;
        if (int.TryParse(digitsOnly, out result))
        {
            return result;
        }
        else
        {
            Debug.LogWarning("Failed to convert the filtered string to an integer.");
            return 0;
        }
    }

}
