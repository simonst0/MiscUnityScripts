using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;

public class Dragable : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public string dragTag = "";
    private Vector3 originalPosition;
    private GraphicRaycaster raycaster;
    private Transform dragRoot;
    private Transform originalParent;
    public UnityEvent onBeginDrag = new UnityEvent();
    public UnityEvent onDragEnd = new UnityEvent();

    private void Start()
    {
        Transform current = transform;
        while (current.parent != null && current.GetComponent<GraphicRaycaster>() == null)
            current = current.parent;
        raycaster = current.GetComponent<GraphicRaycaster>();
        dragRoot = GameObject.FindGameObjectWithTag("DragRoot").transform;
    }

    public void OnDrag(PointerEventData eventData)
    {
        GetComponent<RectTransform>().position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        List<RaycastResult> hits = new List<RaycastResult>();
        raycaster.Raycast(eventData, hits);
        onDragEnd.Invoke();
        foreach (var hit in hits)
        {
            DropHandler handler;
            if (hit.gameObject.TryGetComponent<DropHandler>(out handler))
            {
                transform.SetParent(originalParent);
                originalParent = null;
                handler.OnDrop(eventData);
                return;
            }
        }
        AbortDrag();
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        RectTransform rect = GetComponent<RectTransform>();
        originalPosition = rect.position;
        originalParent = rect.parent;
        rect.transform.SetParent(dragRoot);
        onBeginDrag.Invoke();
    }

    public void AbortDrag()
    {
        transform.SetParent(originalParent);
        originalParent = null;
        transform.GetComponent<RectTransform>().position = originalPosition;
    }
}