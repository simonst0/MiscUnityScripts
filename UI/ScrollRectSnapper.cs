using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

[RequireComponent(typeof(ScrollRect))]
public class ScrollRectSnapper : MonoBehaviour
{
    public ScrollRect scrollrect;
    private Vector2 desiredPosition = Vector2.zero;
    private bool changed = false;
    void Start()
    {
        scrollrect = GetComponent<ScrollRect>();
        scrollrect.onValueChanged.AddListener(OnScroll);
    }

    void OnScroll(Vector2 normalizedPosition)
    {
        desiredPosition = new Vector2((int)(normalizedPosition.x + 0.5f), (int)(normalizedPosition.y + 0.5f));
        changed = true;
    }

    private void Update()
    {
        if (!changed)
            return;
#if UNITY_EDITOR
        if (!Input.GetMouseButton(0))
        {
            SnapToDesiredPosition();
            changed = false;
            return;
        }
#else
          if (Input.touchCount == 0)
        {
            SnapToDesiredPosition();
            changed = false;
        }
#endif
    }

    private void SnapToDesiredPosition()
    {
        scrollrect.verticalNormalizedPosition = desiredPosition.y;
        scrollrect.horizontalNormalizedPosition = desiredPosition.x;
    }
}
