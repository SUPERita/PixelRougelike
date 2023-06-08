using UnityEngine;

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
}
