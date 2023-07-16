
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectionTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{
    [SerializeField] private Vector3 personalOffset = Vector3.zero;
    public virtual string GetDescription()
    {
        return "helo";
    }
    public void OnDeselect(BaseEventData eventData)
    {
        TooltipManager.Instance.ReleaseTooltip();
    }
    public void OnSelect(BaseEventData eventData)
    {
        TooltipManager.Instance.RequestTooltip(GetComponent<RectTransform>(), GetDescription(), personalOffset);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        TooltipManager.Instance.RequestTooltip(GetComponent<RectTransform>(), GetDescription(), personalOffset);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TooltipManager.Instance.ReleaseTooltip();
    }

}

