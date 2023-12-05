using Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PreviewSystem : MonoBehaviour, IPointerClickHandler
{
    private Sight _currentSight = Sight.Default;
    private const string SightMenuName = "SightMenu";
    private const string SightPreviewsName = "SightPreviews";

    private GameObject _sightMenu;
    private GameObject _sightPreviews;
    private GameObject _sightModsView;

    private bool _isMenuVisible;
    private bool _isComponentVisible;

    private GameObject _sightMenuS0;
    private GameObject _sightMenuS1;


    private string GetMenuNameForSight(Sight sight)
    {
        return $"Menu_{sight.GetDescription()}";
    }

    private string GetMenuNameForSight(string sight)
    {
        return $"Menu_{sight}";
    }

    private string GetPreviewNameForSight(Sight sight)
    {
        return $"Preview_{sight.GetDescription()}";
    }

    private string GetPreviewNameForSight(string sight)
    {
        return $"Preview_{sight}";
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        _isMenuVisible = !_isMenuVisible;
        _sightMenu.SetActive(_isMenuVisible);
    }

    void Start()
    {
        _sightMenu = GameObject.Find(SightMenuName);
        _sightPreviews = GameObject.Find(SightPreviewsName);
        _sightMenuS0 = GameObject.Find(GetMenuNameForSight("S0"));
        _sightMenuS1 = GameObject.Find(GetMenuNameForSight("S1"));
        _sightModsView = GameObject.Find("SightMods");
        _sightMenu.SetActive(_isMenuVisible);

        EventTrigger.TriggerEvent trigev = new EventTrigger.TriggerEvent();
        trigev.AddListener((eventData) => OnMenuItemClick((PointerEventData)eventData));
        _sightMenuS0.GetComponent<EventTrigger>().triggers.Add(new EventTrigger.Entry() { callback = trigev, eventID = EventTriggerType.PointerDown });
        _sightMenuS1.GetComponent<EventTrigger>().triggers.Add(new EventTrigger.Entry() { callback = trigev, eventID = EventTriggerType.PointerDown });
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

        foreach(Transform child in _sightMenu.transform)
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

        foreach (Transform child in _sightPreviews.transform)
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

        foreach (Transform child in _sightModsView.transform)
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
