using Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PreviewSystem : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] public GameObject Menu;
    [SerializeField] public GameObject Previews;
    [SerializeField] public GameObject SightsRootObject;

    private bool _isMenuVisible;
    private bool _isComponentVisible;

    private List<GameObject> _menus = new List<GameObject>();

    private string GetMenuNameForSight(string sight)
    {
        return $"Menu_{sight}";
    }

    private string GetPreviewNameForSight(string sight)
    {
        return $"Preview_{sight}";
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        _isMenuVisible = !_isMenuVisible;
        Menu.SetActive(_isMenuVisible);
    }

    void Start()
    {
        Menu.SetActive(_isMenuVisible);

        EventTrigger.TriggerEvent trigev = new EventTrigger.TriggerEvent();
        trigev.AddListener((eventData) => OnMenuItemClick((PointerEventData)eventData));

        foreach (Transform child in Menu.transform)
        {
            var childGameObject = child.gameObject;
            if (childGameObject != null)
            {
                childGameObject.GetComponent<EventTrigger>().triggers.Add(new EventTrigger.Entry() { callback = trigev, eventID = EventTriggerType.PointerDown });
                Debug.Log(childGameObject.name);
            }
        }
    }

    private void OnMenuItemClick(PointerEventData eventData)
    {
        string menuName = string.Empty;

        if (eventData.pointerEnter.gameObject.name.Contains("Menu_"))
        {
            menuName = eventData.pointerEnter.gameObject.name;
        }else if (eventData.pointerEnter.gameObject.transform.parent.gameObject.name.Contains("Menu_"))
        {
            menuName = eventData.pointerEnter.gameObject.transform.parent.gameObject.name;
        }

        if (menuName == string.Empty)
        {
            return;
        }

        switch (menuName)
        {
            case "Menu_S0":
                HandleSightChange("S0");
                break;

            case "Menu_S1":
                HandleSightChange("S1");
                break;
        }
    }

    private void HandleSightChange(string sightName)
    {
        var sightMenuName = GetMenuNameForSight(sightName);
        var sightPreviewName = GetPreviewNameForSight(sightName);

        foreach(Transform child in Menu.transform)
        {
            if (child.name != sightMenuName)
            {
                foreach (Transform childOfChild in child.transform)
                {
                    if (childOfChild.name == "Panel")
                    {
                        Color c = Color.black;
                        c.a = 0.4f;
                        childOfChild.GetComponent<Image>().color = c;
                    }
                }
            }
            else
            {
                foreach (Transform childOfChild in child.transform)
                {
                    if (childOfChild.name == "Panel")
                    {
                        Color c = Color.red;
                        c.a = 0.4f;
                        childOfChild.GetComponent<Image>().color = c;
                    }
                }
            }
        }

        foreach (Transform child in Previews.transform)
        {
            if (child.name != sightPreviewName)
            {
                child.gameObject.SetActive(false);
            }
            else
            {
                child.gameObject.SetActive(true);
            }
        }

        foreach (Transform child in SightsRootObject.transform)
        {
            if (child.name != sightName)
            {
                child.gameObject.SetActive(false);
            }
            else
            {
                child.gameObject.SetActive(true);
            }
        }
    }
}
