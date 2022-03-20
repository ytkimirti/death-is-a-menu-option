using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Option : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
{
    public Window childWindow;
    public bool isHorizontal;
    public bool IsActive => isHovering || (childWindow != null && childWindow.IsChildActive);
    public bool isHovering;
    public bool isMouseOver;
    public bool isDisabled;
    public UnityEvent onClick;
    private Image _image;
    [SerializeField] private TextMeshProUGUI textMesh;
    private Window _parentWindow;
    [SerializeField] private TextMeshProUGUI arrowTextMesh;
    
    [Space]
    public float closeDelay;
    private float _closeTimer;

    private void Awake()
    {
        _image = GetComponent<Image>();
        isHovering = false;
        isMouseOver = false;
        _image.color = Color.clear;
        textMesh.color = GameManager.Instance.textBlackColor;
        if (!_parentWindow)
            _parentWindow = GetComponentInParent<Window>();
    }

    public void Close()
    {
        if (_parentWindow)
            _parentWindow.gameObject.SetActive(false);
        _closeTimer = 0;
        isHovering = false;
        isMouseOver = false;
    }

    private void Update()
    {
        arrowTextMesh.gameObject.SetActive(!isHorizontal && childWindow != null);
        if (isDisabled)
            textMesh.color = GameManager.Instance.textDisabledColor;
        if (childWindow)
            childWindow.gameObject.SetActive(IsActive);
        arrowTextMesh.color = textMesh.color;
        
        if (isMouseOver)
            _closeTimer = closeDelay;
        else
        {
            _closeTimer -= Time.deltaTime;
            if (_closeTimer <= 0)
            {
                isHovering = false;
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (childWindow)
        {
            Gizmos.color = Color.red;
            
            Gizmos.DrawWireSphere(transform.position, 0.2f);
            Gizmos.DrawWireSphere(childWindow.transform.position, 0.5f);
            Gizmos.DrawLine(transform.position, childWindow.transform.position);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        
        if (isDisabled)
            return;
        isHovering = true;
        isMouseOver = true;
        _image.color = GameManager.Instance.hoverColor;
            textMesh.color = GameManager.Instance.textWhiteColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (isDisabled)
            return;
        isMouseOver = false;
        _image.color = Color.clear;
            textMesh.color = GameManager.Instance.textBlackColor;
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        if (isDisabled)
            return;
        Close();
        onClick.Invoke();
        GameManager.Instance.OnClick(this);
    }

}
