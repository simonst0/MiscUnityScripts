using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropHandler : MonoBehaviour
{
    public GameObjectIntEvent OnDropEvent = new GameObjectIntEvent();
    public IntIntEvent OnSwapEvent = new IntIntEvent();
    public string dragTag = "";
    public int index = 0;

    bool isFree { get { return GetComponentInChildren<Dragable>() == null; } }

    public void OnDrop(PointerEventData eventData)
    {
        Transform target = eventData.pointerDrag.transform;
        while (target.parent && target.parent.GetComponent<Dragable>())
            target = target.parent;
        Dragable dragable = target.GetComponent<Dragable>();
        if (!dragable)
            return;
        if (dragable.dragTag == dragTag && isFree)
        {
            AcceptDrag(dragable);
        }
        else if (dragable.dragTag == dragTag && !isFree) //Switch 
        {
            DropHandler otherParent = target.GetComponentInParent<DropHandler>();
            Switch(otherParent);
        }
        else
        {
            dragable.AbortDrag();
        }
    }

    public void Switch(DropHandler other, bool notify = true)
    {
        Dragable ownDragable = GetComponentInChildren<Dragable>();
        ownDragable.transform.SetParent(other.transform);
        ownDragable.GetComponent<RectTransform>().localPosition = Vector3.zero;
        Dragable otherDragable = other.GetComponentInChildren<Dragable>();
        otherDragable.transform.SetParent(transform);
        otherDragable.GetComponent<RectTransform>().localPosition = Vector3.zero;
        if (notify)
        {
            OnDropEvent.Invoke(other.transform.gameObject, index);
            other.OnDropEvent.Invoke(ownDragable.gameObject, other.index);
            OnSwapEvent.Invoke(other.index, index);
        }
    }

    public void AcceptDrag(Dragable target, bool notify = true)
    {
        target.transform.SetParent(transform);
        target.GetComponent<RectTransform>().localPosition = Vector3.zero;
        if (notify)
            OnDropEvent.Invoke(target.gameObject, index);
    }
}
