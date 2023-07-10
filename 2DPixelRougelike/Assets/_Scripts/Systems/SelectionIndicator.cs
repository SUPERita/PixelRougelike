using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectionIndicator : StaticInstance<SelectionIndicator> 
{

    [SerializeField] private Image indicator;
    [SerializeField] private Vector2 offset = Vector2.zero;

    GameObject selectedObject;
    GameObject lastSelectedObject;
    RectTransform selectedRectTransform;
    void Update()
    {
        selectedObject = EventSystem.current.currentSelectedGameObject;

        //on switch sfx
        if(selectedObject != lastSelectedObject) { AudioSystem.Instance.PlaySound("s1"); }
        lastSelectedObject = selectedObject;

        if (selectedObject != null && selectedObject.GetComponent<Selectable>().IsInteractable())
        {
            indicator.gameObject.SetActive(true);
            // Get the RectTransform component of the selected GameObject
            selectedRectTransform = selectedObject.GetComponent<RectTransform>();

            // Do something with the RectTransform if needed
            if (selectedRectTransform != null)
            {

                indicator.rectTransform.position = 
                    (Vector2)selectedRectTransform.position + //target pos
                    Vector2.right * selectedRectTransform.sizeDelta.x + //target width
                    offset; //offset
                    
            }
        } else
        {
            indicator.gameObject.SetActive(false);
        }
    }

}

