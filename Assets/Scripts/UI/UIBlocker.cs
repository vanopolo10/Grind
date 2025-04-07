using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;

public static class UIBlocker
{
    public static bool IsPointerOverUI(Vector2 screenPosition, GraphicRaycaster raycaster, string ignoreTag = null)
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current)
        {
            position = screenPosition
        };

        List<RaycastResult> results = new List<RaycastResult>();
        raycaster.Raycast(eventData, results);

        foreach (var result in results)
        {
            if (!string.IsNullOrEmpty(ignoreTag) && result.gameObject.CompareTag(ignoreTag))
                continue;

            return true;
        }

        return false;
    }
}