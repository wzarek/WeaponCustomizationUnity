using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StickerPreviewSystem : MonoBehaviour
{
    [SerializeField] public GameObject StickersRootObject;

    void Start()
    {
        EventTrigger.TriggerEvent trigev = new EventTrigger.TriggerEvent();
        trigev.AddListener((eventData) => OnMenuItemClick((PointerEventData)eventData));

        foreach (Transform child in transform)
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
        }
        else if (eventData.pointerEnter.gameObject.transform.parent.gameObject.name.Contains("Menu_"))
        {
            menuName = eventData.pointerEnter.gameObject.transform.parent.gameObject.name;
        }

        if (menuName == string.Empty)
        {
            return;
        }

        var menuNameSplitted = menuName.Split('_');
        var stickerName = menuNameSplitted.Last();

        HandleStickerToggle(stickerName);
    }

    private void HandleStickerToggle(string stickerName)
    {
        var stickerMenuName = $"Menu_{stickerName}";
        var isCurrentlyActive = false;

        foreach (Transform child in StickersRootObject.transform)
        {
            if (child.name == stickerName)
            {
                child.gameObject.SetActive(!child.gameObject.activeSelf);
                isCurrentlyActive = child.gameObject.activeSelf;
            }
        }

        foreach (Transform child in transform)
        {
            if (child.name == stickerMenuName)
            {
                foreach (Transform childOfChild in child.transform)
                {
                    if (childOfChild.name == "Panel")
                    {
                        if (isCurrentlyActive)
                        {
                            Color c = Color.red;
                            c.a = 0.4f;
                            childOfChild.GetComponent<Image>().color = c;
                        }
                        else
                        {
                            Color c = Color.black;
                            c.a = 0.4f;
                            childOfChild.GetComponent<Image>().color = c;
                        }
                    }
                }
            }
        }
    }
}
