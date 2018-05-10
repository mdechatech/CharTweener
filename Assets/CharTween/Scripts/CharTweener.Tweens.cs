using DG.Tweening;
using TMPro;
using UnityEngine;

namespace CharTween
{
    public partial class CharTweener : MonoBehaviour
    {
        // Extra goodies
        public Tweener DOCircle(int charIndex, float radius, float duration, int pathPoints = 8, PathType pathType = PathType.CatmullRom,
            PathMode pathMode = PathMode.Full3D, int resolution = 10, Color? gizmoColor = null)
        {
            var tweenPath = new Vector3[pathPoints + 1];
            for (var i = 0; i < tweenPath.Length; ++i)
            {
                var theta = Mathf.Lerp(0, 2 * Mathf.PI, i / (float)(tweenPath.Length - 1));
                tweenPath[i] = new Vector3(radius*Mathf.Cos(theta), radius*Mathf.Sin(theta), 0);
            }

            tweenPath[tweenPath.Length - 1] = new Vector3(radius, 0, 0);
            SetPositionOffset(charIndex, tweenPath[0]);
            return DOPath(charIndex, tweenPath, duration, pathType, pathMode, resolution, gizmoColor);
        }

        // Color tweens
        public Tweener DOFade(int charIndex, float endValue, float duration)
        {
            return MonitorColorTween(GetProxyColor(charIndex).DOFade(endValue, duration));
        }

        public Tweener DOColor(int charIndex, Color endValue, float duration)
        {
            return MonitorColorTween(GetProxyColor(charIndex).DOColor(endValue, duration));
        }

        public Tweener DOGradient(int charIndex, VertexGradient endValue, float duration)
        {
            return MonitorColorTween(GetProxyColor(charIndex).DOVertexGradient(endValue, duration));
        }

        // Transform tweens
        public Tweener DOMove(int charIndex, Vector3 endValue, float duration, bool snapping = false)
        {
            return MonitorTransformTween(GetProxyTransform(charIndex).DOMove(endValue, duration, snapping));
        }

        public Tweener DOMoveX(int charIndex, float endValue, float duration, bool snapping = false)
        {
            return MonitorTransformTween(GetProxyTransform(charIndex).DOMoveX(endValue, duration, snapping));
        }

        public Tweener DOMoveY(int charIndex, float endValue, float duration, bool snapping = false)
        {
            return MonitorTransformTween(GetProxyTransform(charIndex).DOMoveY(endValue, duration, snapping));
        }

        public Tweener DOMoveZ(int charIndex, float endValue, float duration, bool snapping = false)
        {
            return MonitorTransformTween(GetProxyTransform(charIndex).DOMoveZ(endValue, duration, snapping));
        }

        public Tweener DOLocalMove(int charIndex, Vector3 endValue, float duration, bool snapping = false)
        {
            return MonitorTransformTween(GetProxyTransform(charIndex).DOLocalMove(endValue, duration, snapping));
        }

        public Tweener DOLocalMoveX(int charIndex, float endValue, float duration, bool snapping = false)
        {
            return MonitorTransformTween(GetProxyTransform(charIndex).DOLocalMoveX(endValue, duration, snapping));
        }

        public Tweener DOLocalMoveY(int charIndex, float endValue, float duration, bool snapping = false)
        {
            return MonitorTransformTween(GetProxyTransform(charIndex).DOLocalMoveY(endValue, duration, snapping));
        }

        public Tweener DOLocalMoveZ(int charIndex, float endValue, float duration, bool snapping = false)
        {
            return MonitorTransformTween(GetProxyTransform(charIndex).DOLocalMoveZ(endValue, duration, snapping));
        }

        public Sequence DOJump(int charIndex, Vector3 endValue, float jumpPower, int numJumps, float duration, bool snapping = false)
        {
            return MonitorTransformTween(GetProxyTransform(charIndex).DOJump(endValue, jumpPower, numJumps, duration, snapping));
        }

        public Tweener DORotate(int charIndex, Vector3 endValue, float duration, RotateMode mode)
        {
            return MonitorTransformTween(GetProxyTransform(charIndex).DORotate(endValue, duration, mode));
        }

        public Tweener DORotateQuaternion(int charIndex, Quaternion endValue, float duration)
        {
            return MonitorTransformTween(GetProxyTransform(charIndex).DORotateQuaternion(endValue, duration));
        }

        public Tweener DOLocalRotate(int charIndex, Vector3 endValue, float duration, RotateMode mode = RotateMode.Fast)
        {
            return MonitorTransformTween(GetProxyTransform(charIndex).DOLocalRotate(endValue, duration, mode));
        }

        public Tweener DOLocalRotateQuaternion(int charIndex, Quaternion endValue, float duration)
        {
            return MonitorTransformTween(GetProxyTransform(charIndex).DOLocalRotateQuaternion(endValue, duration));
        }

        public Tweener DOLookAt(int charIndex, Vector3 towards, float duration, AxisConstraint axisConstraint = AxisConstraint.None,
            Vector3? up = null)
        {
            return MonitorTransformTween(GetProxyTransform(charIndex).DOLookAt(towards, duration, axisConstraint, up));
        }

        public Tweener DOScale(int charIndex, float endValue, float duration)
        {
            return MonitorTransformTween(GetProxyTransform(charIndex).DOScale(endValue, duration));
        }

        public Tweener DOScale(int charIndex, Vector3 endValue, float duration)
        {
            return MonitorTransformTween(GetProxyTransform(charIndex).DOScale(endValue, duration));
        }

        public Tweener DOScaleX(int charIndex, float endValue, float duration)
        {
            return MonitorTransformTween(GetProxyTransform(charIndex).DOScaleX(endValue, duration));
        }

        public Tweener DOScaleY(int charIndex, float endValue, float duration)
        {
            return MonitorTransformTween(GetProxyTransform(charIndex).DOScaleY(endValue, duration));
        }

        public Tweener DOScaleZ(int charIndex, float endValue, float duration)
        {
            return MonitorTransformTween(GetProxyTransform(charIndex).DOScaleZ(endValue, duration));
        }

        public Tweener DOPunchPosition(int charIndex, Vector3 punch, float duration, int vibrato = 10, float elasticity = 1, bool snapping = false)
        {
            return MonitorTransformTween(GetProxyTransform(charIndex).DOPunchPosition(punch, duration, vibrato, elasticity, snapping));
        }

        public Tweener DOPunchRotation(int charIndex, Vector3 punch, float duration, int vibrato = 10, float elasticity = 1)
        {
            return MonitorTransformTween(GetProxyTransform(charIndex).DOPunchRotation(punch, duration, vibrato, elasticity));
        }

        public Tweener DOPunchScale(int charIndex, Vector3 punch, float duration, int vibrato = 10, float elasticity = 1)
        {
            return MonitorTransformTween(GetProxyTransform(charIndex).DOPunchScale(punch, duration, vibrato, elasticity));
        }

        public Tweener DOShakePosition(int charIndex, float duration, float strength = 1, int vibrato = 10, float randomness = 90,
            bool snapping = false, bool fadeOut = true)
        {
            return MonitorTransformTween(GetProxyTransform(charIndex).DOShakePosition(duration, strength, vibrato, randomness, snapping, fadeOut));
        }

        public Tweener DOShakePosition(int charIndex, float duration, Vector3 strength, int vibrato = 10, float randomness = 90,
            bool snapping = false, bool fadeOut = true)
        {
            return MonitorTransformTween(GetProxyTransform(charIndex).DOShakePosition(duration, strength, vibrato, randomness, snapping, fadeOut));
        }

        public Tweener DOShakeRotation(int charIndex, float duration, float strength = 1, int vibrato = 10, float randomness = 90,
            bool fadeOut = true)
        {
            return MonitorTransformTween(GetProxyTransform(charIndex).DOShakeRotation(duration, strength, vibrato, randomness, fadeOut));
        }

        public Tweener DOShakeRotation(int charIndex, float duration, Vector3 strength, int vibrato = 10, float randomness = 90,
            bool fadeOut = true)
        {
            return MonitorTransformTween(GetProxyTransform(charIndex).DOShakeRotation(duration, strength, vibrato, randomness, fadeOut));
        }
        public Tweener DOShakeScale(int charIndex, float duration, float strength = 1, int vibrato = 10, float randomness = 90,
            bool fadeOut = true)
        {
            return MonitorTransformTween(GetProxyTransform(charIndex).DOShakeScale(duration, strength, vibrato, randomness, fadeOut));
        }

        public Tweener DOShakeScale(int charIndex, float duration, Vector3 strength, int vibrato = 10, float randomness = 90,
            bool fadeOut = true)
        {
            return MonitorTransformTween(GetProxyTransform(charIndex).DOShakeScale(duration, strength, vibrato, randomness, fadeOut));
        }

        public Tweener DOPath(int charIndex, Vector3[] path, float duration, PathType pathType = PathType.Linear,
            PathMode pathMode = PathMode.Full3D, int resolution = 10, Color? gizmoColor = null)
        {
            return MonitorTransformTween(GetProxyTransform(charIndex).DOPath(path, duration, pathType, pathMode, resolution, gizmoColor));
        }

        public Tweener DOLocalPath(int charIndex, Vector3[] path, float duration, PathType pathType = PathType.Linear,
            PathMode pathMode = PathMode.Full3D, int resolution = 10, Color? gizmoColor = null)
        {
            return MonitorTransformTween(GetProxyTransform(charIndex).DOLocalPath(path, duration, pathType, pathMode, resolution, gizmoColor));
        }

        public Tweener DOBlendableMoveBy(int charIndex, Vector3 byValue, float duration, bool snapping = false)
        {
            return MonitorTransformTween(GetProxyTransform(charIndex).DOBlendableMoveBy(byValue, duration, snapping));
        }

        public Tweener DOBlendableLocalMoveBy(int charIndex, Vector3 byValue, float duration, bool snapping = false)
        {
            return MonitorTransformTween(GetProxyTransform(charIndex).DOBlendableLocalMoveBy(byValue, duration, snapping));
        }

        public Tweener DOBlendableRotateBy(int charIndex, Vector3 byValue, float duration,
            RotateMode rotateMode = RotateMode.Fast)
        {
            return MonitorTransformTween(GetProxyTransform(charIndex).DOBlendableRotateBy(byValue, duration, rotateMode));
        }

        public Tweener DOBlendableLocalRotateBy(int charIndex, Vector3 byValue, float duration,
            RotateMode rotateMode = RotateMode.Fast)
        {
            return MonitorTransformTween(GetProxyTransform(charIndex).DOBlendableLocalRotateBy(byValue, duration, rotateMode));
        }

        public Tweener DOBlendableScaleBy(int charIndex, Vector3 byValue, float duration)
        {
            return MonitorTransformTween(GetProxyTransform(charIndex).DOBlendableScaleBy(byValue, duration));
        }
    }
}
