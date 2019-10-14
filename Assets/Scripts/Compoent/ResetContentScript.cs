using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class ResetContentScript : MonoBehaviour
{
    public enum ResetType
    {
        TEXT,
    }

    public ResetType resetType = ResetType.TEXT;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Reset()
    {
        this.ResetWidthAndHeight();
    }

    void ResetWidthAndHeight()
    {
        switch (this.resetType)
        {
            case ResetType.TEXT:
                RectTransform rect = GetComponent<RectTransform>();
                var Text = this.GetComponent<UnityEngine.UI.Text>();
                // 获取Text的Size
                Vector2 v2 = rect.rect.size;

                // width保持不变
                rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Text.preferredWidth);
                // 动态设置height
                rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Text.preferredHeight);
                break;
            
        }
    }
}
