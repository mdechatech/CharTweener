using DG.Tweening;
using TMPro;
using UnityEngine;

namespace CharTween
{
    public partial class CharTweenerUGUI
    {
        // Override for UGUI
        public Tweener DOCircleUGUI(Camera viewCamera, int charIndex, float radius, float duration, int pathPoints = 8, PathType pathType = PathType.CatmullRom,
            PathMode pathMode = PathMode.Full3D, int resolution = 10, Color? gizmoColor = null)
        {
            RectTransform rectTransform = GetComponent<RectTransform>();

            var tweenPath = new Vector3[pathPoints + 1];
            for (var i = 0; i < tweenPath.Length; ++i)
            {
                var theta = Mathf.Lerp(0, 2 * Mathf.PI, i / (float)(tweenPath.Length - 1));

                Vector3 charWorldPosition = viewCamera.WorldToScreenPoint(transform.position);;
                Vector3 worldPosition;
                
                RectTransformUtility.ScreenPointToWorldPointInRectangle(rectTransform, new Vector3(radius*Mathf.Cos(theta)+charWorldPosition.x, radius*Mathf.Sin(theta)+charWorldPosition.y, 0), viewCamera, out worldPosition);
                tweenPath[i] = worldPosition;
            }

            SetPositionOffset(charIndex, tweenPath[0]);
            return DOPath(charIndex, tweenPath, duration, pathType, pathMode, resolution, gizmoColor);
        }

        // Tween for UGUI
        public Tweener DOAnchorPos( int charIndex, Vector2 endValue, float duration, bool snapping = false )
        {
            return MonitorTransformTween( (GetProxyTransform( charIndex ) as RectTransform).DOAnchorPos( endValue, duration, snapping ) );
        }

        public Tweener DOAnchorPosX( int charIndex, float endValue, float duration, bool snapping = false )
        {
            return MonitorTransformTween( (GetProxyTransform( charIndex ) as RectTransform).DOAnchorPosX( endValue, duration, snapping ) );
        }

        public Tweener DOAnchorPosY( int charIndex, float endValue, float duration, bool snapping = false )
        {
            return MonitorTransformTween( (GetProxyTransform( charIndex ) as RectTransform).DOAnchorPosY( endValue, duration, snapping ) );
        }

    }
}
