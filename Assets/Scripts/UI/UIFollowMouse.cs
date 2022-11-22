using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TTW.UI
{
    public class UIFollowMouse : MonoBehaviour
    {
        public Canvas parentCanvas;
        public Vector3 offset;
        public void Start()
        {
            Vector2 pos;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                parentCanvas.transform as RectTransform, Input.mousePosition,
                parentCanvas.worldCamera,
                out pos);
        }

        public void LateUpdate()
        {
            Vector2 movePos;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                parentCanvas.transform as RectTransform,
                Input.mousePosition, parentCanvas.worldCamera,
                out movePos);

            transform.position = parentCanvas.transform.TransformPoint(movePos) + offset;
        }
    }
}
