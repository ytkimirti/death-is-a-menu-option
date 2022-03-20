using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Window : MonoBehaviour, IPointerEnterHandler , IPointerExitHandler
{
    public bool isHovering;
    public bool IsChildActive => isHovering || CheckIsChildActive();
    private List<Option> _options;

    private void Awake()
    {
        _options = new List<Option>();
        foreach (Transform t in transform)
        {
            Option option = t.GetComponent<Option>();
            
            if (option == null)
                continue;
            _options.Add(option);
        }
    }

    public bool CheckIsChildActive()
    {
        if (_options == null || _options.Count == 0)
            return false;
        foreach (Option option in _options)
        {
            if (option.IsActive)
                return true;
        }
        return false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovering = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovering = false;
    }
}
