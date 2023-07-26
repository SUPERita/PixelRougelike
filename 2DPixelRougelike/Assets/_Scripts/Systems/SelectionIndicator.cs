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
        if(selectedObject != lastSelectedObject) {
            if(Random.value > .5f) AudioSystem.Instance.PlaySound("click1", .5f, 1f);
            else AudioSystem.Instance.PlaySound("click1", .5f, 1.1f);
        }
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
                    Vector2.right * selectedRectTransform.rect.width * Helpers.CurrentScreenSizeRelativeTo1920() / 2//target width
                    + offset //offset
                    + (Vector2.right * indicator.GetComponent<RectTransform>().rect.width * Helpers.CurrentScreenSizeRelativeTo1920() / 2); //self width; 


            }
        } else
        {
            indicator.gameObject.SetActive(false);
        }
    }

}

