using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressBar : MonoBehaviour
{
    public RectTransform progressBar;
    protected RectTransform ownTransfrom;

    [SerializeField]
    protected float _progress = 0;

    [SerializeField]
    public float progress
    {
        get
        {
            return _progress;
        }
        set
        {
            value = Mathf.Clamp(value, 0.0f, 1.0f);
            _progress = value;
            Layout();
        }
    }

    private void Awake()
    {
        ownTransfrom = GetComponent<RectTransform>();
        progress = _progress;
    }

    protected void Layout()
    {
        float fullWidth = ownTransfrom.rect.width;
        progressBar.sizeDelta = new Vector2(fullWidth * _progress, progressBar.sizeDelta.y);;
    }
}
