using DG.Tweening;
using TMPro;
using UnityEngine;

namespace CharTween
{
    // Contains tweens that are exact copies of DOTween methods
    public partial class CharTweener : MonoBehaviour
    {
        // Color tweens
        public Tweener DOFade(int charIndex, float endValue, float duration)
        {
            NotifyActiveColorTween();
            ProxyColor proxy = GetProxyColor(charIndex);
            return proxy.AddTween(proxy.DOFade(endValue, duration));
        }

        public Tweener DOColor(int charIndex, Color endValue, float duration)
        {
            NotifyActiveColorTween();
            ProxyColor proxy = GetProxyColor(charIndex);
            return proxy.AddTween(proxy.DOColor(endValue, duration));
        }

        public Tweener DOGradient(int charIndex, VertexGradient endValue, float duration)
        {
            NotifyActiveColorTween();
            ProxyColor proxy = GetProxyColor(charIndex);
            return proxy.AddTween(proxy.DOColorGradient(endValue, duration));
        }

        // Transform tweens
        public Tweener DOMove(int charIndex, Vector3 endValue, float duration, bool snapping = false)
        {
            NotifyActiveTransformTween();
            ProxyTransform proxy = GetProxyTransform(charIndex);
            return proxy.AddTween(proxy.DOMove(endValue, duration, snapping));
        }

        public Tweener DOMoveX(int charIndex, float endValue, float duration, bool snapping = false)
        {
            NotifyActiveTransformTween();
            ProxyTransform proxy = GetProxyTransform(charIndex);
            return proxy.AddTween(proxy.DOMoveX(endValue, duration, snapping));
        }

        public Tweener DOMoveY(int charIndex, float endValue, float duration, bool snapping = false)
        {
            NotifyActiveTransformTween();
            ProxyTransform proxy = GetProxyTransform(charIndex);
            return proxy.AddTween(proxy.DOMoveY(endValue, duration, snapping));
        }

        public Tweener DOMoveZ(int charIndex, float endValue, float duration, bool snapping = false)
        {
            NotifyActiveTransformTween();
            ProxyTransform proxy = GetProxyTransform(charIndex);
            return proxy.AddTween(proxy.DOMoveZ(endValue, duration, snapping));
        }

        public Tweener DOLocalMove(int charIndex, Vector3 endValue, float duration, bool snapping = false)
        {
            NotifyActiveTransformTween();
            ProxyTransform proxy = GetProxyTransform(charIndex);
            return proxy.AddTween(proxy.DOLocalMove(endValue, duration, snapping));
        }

        public Tweener DOLocalMoveX(int charIndex, float endValue, float duration, bool snapping = false)
        {
            ProxyTransform proxy = GetProxyTransform(charIndex);
            return proxy.AddTween(proxy.DOLocalMoveX(endValue, duration, snapping));
        }

        public Tweener DOLocalMoveY(int charIndex, float endValue, float duration, bool snapping = false)
        {
            NotifyActiveTransformTween();
            ProxyTransform proxy = GetProxyTransform(charIndex);
            return proxy.AddTween(proxy.DOLocalMoveY(endValue, duration, snapping));
        }

        public Tweener DOLocalMoveZ(int charIndex, float endValue, float duration, bool snapping = false)
        {
            NotifyActiveTransformTween();
            ProxyTransform proxy = GetProxyTransform(charIndex);
            return proxy.AddTween(proxy.DOLocalMoveZ(endValue, duration, snapping));
        }

        public Sequence DOJump(int charIndex, Vector3 endValue, float jumpPower, int numJumps, float duration, bool snapping = false)
        {
            NotifyActiveTransformTween();
            ProxyTransform proxy = GetProxyTransform(charIndex);
            return proxy.AddTween(proxy.Target.DOJump(endValue, jumpPower, numJumps, duration, snapping));
        }

        public Sequence DOLocalJump(int charIndex, Vector3 endValue, float jumpPower, int numJumps, float duration, bool snapping = false)
        {
            NotifyActiveTransformTween();
            ProxyTransform proxy = GetProxyTransform(charIndex);
            return proxy.AddTween(proxy.Target.DOLocalJump(endValue, jumpPower, numJumps, duration, snapping));
        }

        public Tweener DORotate(int charIndex, Vector3 endValue, float duration, RotateMode mode)
        {
            NotifyActiveTransformTween();
            ProxyTransform proxy = GetProxyTransform(charIndex);
            return proxy.AddTween(proxy.Target.DORotate(endValue, duration, mode));
        }

        public Tweener DORotateQuaternion(int charIndex, Quaternion endValue, float duration)
        {
            NotifyActiveTransformTween();
            ProxyTransform proxy = GetProxyTransform(charIndex);
            return proxy.AddTween(proxy.Target.DORotateQuaternion(endValue, duration));
        }

        public Tweener DOLocalRotate(int charIndex, Vector3 endValue, float duration, RotateMode mode = RotateMode.Fast)
        {
            NotifyActiveTransformTween();
            ProxyTransform proxy = GetProxyTransform(charIndex);
            return proxy.AddTween(proxy.Target.DOLocalRotate(endValue, duration, mode));
        }

        public Tweener DOLocalRotateQuaternion(int charIndex, Quaternion endValue, float duration)
        {
            NotifyActiveTransformTween();
            ProxyTransform proxy = GetProxyTransform(charIndex);
            return proxy.AddTween(proxy.Target.DOLocalRotateQuaternion(endValue, duration));
        }

        public Tweener DOLookAt(int charIndex, Vector3 towards, float duration, AxisConstraint axisConstraint = AxisConstraint.None,
            Vector3? up = null)
        {
            NotifyActiveTransformTween();
            ProxyTransform proxy = GetProxyTransform(charIndex);
            return proxy.AddTween(proxy.Target.DOLookAt(towards, duration, axisConstraint, up));
        }

        public Tweener DOScale(int charIndex, float endValue, float duration)
        {
            NotifyActiveTransformTween();
            ProxyTransform proxy = GetProxyTransform(charIndex);
            return proxy.AddTween(proxy.Target.DOScale(endValue, duration));
        }

        public Tweener DOScale(int charIndex, Vector3 endValue, float duration)
        {
            NotifyActiveTransformTween();
            ProxyTransform proxy = GetProxyTransform(charIndex);
            return proxy.AddTween(proxy.Target.DOScale(endValue, duration));
        }

        public Tweener DOScaleX(int charIndex, float endValue, float duration)
        {
            NotifyActiveTransformTween();
            ProxyTransform proxy = GetProxyTransform(charIndex);
            return proxy.AddTween(proxy.Target.DOScaleX(endValue, duration));
        }

        public Tweener DOScaleY(int charIndex, float endValue, float duration)
        {
            NotifyActiveTransformTween();
            ProxyTransform proxy = GetProxyTransform(charIndex);
            return proxy.AddTween(proxy.Target.DOScaleY(endValue, duration));
        }

        public Tweener DOScaleZ(int charIndex, float endValue, float duration)
        {
            NotifyActiveTransformTween();
            ProxyTransform proxy = GetProxyTransform(charIndex);
            return proxy.AddTween(proxy.Target.DOScaleZ(endValue, duration));
        }

        public Tweener DOPunchPosition(int charIndex, Vector3 punch, float duration, int vibrato = 10, float elasticity = 1, bool snapping = false)
        {
            NotifyActiveTransformTween();
            ProxyTransform proxy = GetProxyTransform(charIndex);
            return proxy.AddTween(proxy.Target.DOPunchPosition(punch, duration, vibrato, elasticity, snapping));
        }

        public Tweener DOPunchRotation(int charIndex, Vector3 punch, float duration, int vibrato = 10, float elasticity = 1)
        {
            NotifyActiveTransformTween();
            ProxyTransform proxy = GetProxyTransform(charIndex);
            return proxy.AddTween(proxy.Target.DOPunchRotation(punch, duration, vibrato, elasticity));
        }

        public Tweener DOPunchScale(int charIndex, Vector3 punch, float duration, int vibrato = 10, float elasticity = 1)
        {
            NotifyActiveTransformTween();
            ProxyTransform proxy = GetProxyTransform(charIndex);
            return proxy.AddTween(proxy.Target.DOPunchScale(punch, duration, vibrato, elasticity));
        }

        public Tweener DOShakePosition(int charIndex, float duration, float strength = 1, int vibrato = 10, float randomness = 90,
            bool snapping = false, bool fadeOut = true)
        {
            NotifyActiveTransformTween();
            ProxyTransform proxy = GetProxyTransform(charIndex);
            return proxy.AddTween(proxy.Target.DOShakePosition(duration, strength, vibrato, randomness, snapping, fadeOut));
        }

        public Tweener DOShakePosition(int charIndex, float duration, Vector3 strength, int vibrato = 10, float randomness = 90,
            bool snapping = false, bool fadeOut = true)
        {
            NotifyActiveTransformTween();
            ProxyTransform proxy = GetProxyTransform(charIndex);
            return proxy.AddTween(proxy.Target.DOShakePosition(duration, strength, vibrato, randomness, snapping, fadeOut));
        }

        public Tweener DOShakeRotation(int charIndex, float duration, float strength = 1, int vibrato = 10, float randomness = 90,
            bool fadeOut = true)
        {
            NotifyActiveTransformTween();
            ProxyTransform proxy = GetProxyTransform(charIndex);
            return proxy.AddTween(proxy.Target.DOShakeRotation(duration, strength, vibrato, randomness, fadeOut));
        }

        public Tweener DOShakeRotation(int charIndex, float duration, Vector3 strength, int vibrato = 10, float randomness = 90,
            bool fadeOut = true)
        {
            NotifyActiveTransformTween();
            ProxyTransform proxy = GetProxyTransform(charIndex);
            return proxy.AddTween(proxy.Target.DOShakeRotation(duration, strength, vibrato, randomness, fadeOut));
        }
        public Tweener DOShakeScale(int charIndex, float duration, float strength = 1, int vibrato = 10, float randomness = 90,
            bool fadeOut = true)
        {
            NotifyActiveTransformTween();
            ProxyTransform proxy = GetProxyTransform(charIndex);
            return proxy.AddTween(proxy.Target.DOShakeScale(duration, strength, vibrato, randomness, fadeOut));
        }

        public Tweener DOShakeScale(int charIndex, float duration, Vector3 strength, int vibrato = 10, float randomness = 90,
            bool fadeOut = true)
        {
            NotifyActiveTransformTween();
            ProxyTransform proxy = GetProxyTransform(charIndex);
            return proxy.AddTween(proxy.Target.DOShakeScale(duration, strength, vibrato, randomness, fadeOut));
        }

        public Tweener DOPath(int charIndex, Vector3[] path, float duration, PathType pathType = PathType.Linear,
            PathMode pathMode = PathMode.Full3D, int resolution = 10, Color? gizmoColor = null)
        {
            NotifyActiveTransformTween();
            ProxyTransform proxy = GetProxyTransform(charIndex);
            return proxy.AddTween(proxy.Target.DOPath(path, duration, pathType, pathMode, resolution, gizmoColor));
        }

        public Tweener DOLocalPath(int charIndex, Vector3[] path, float duration, PathType pathType = PathType.Linear,
            PathMode pathMode = PathMode.Full3D, int resolution = 10, Color? gizmoColor = null)
        {
            NotifyActiveTransformTween();
            ProxyTransform proxy = GetProxyTransform(charIndex);
            return proxy.AddTween(proxy.Target.DOLocalPath(path, duration, pathType, pathMode, resolution, gizmoColor));
        }

        public Tweener DOBlendableMoveBy(int charIndex, Vector3 byValue, float duration, bool snapping = false)
        {
            NotifyActiveTransformTween();
            ProxyTransform proxy = GetProxyTransform(charIndex);
            return proxy.AddTween(proxy.Target.DOBlendableMoveBy(byValue, duration, snapping));
        }

        public Tweener DOBlendableLocalMoveBy(int charIndex, Vector3 byValue, float duration, bool snapping = false)
        {
            NotifyActiveTransformTween();
            ProxyTransform proxy = GetProxyTransform(charIndex);
            return proxy.AddTween(proxy.Target.DOBlendableLocalMoveBy(byValue, duration, snapping));
        }

        public Tweener DOBlendableRotateBy(int charIndex, Vector3 byValue, float duration,
            RotateMode rotateMode = RotateMode.Fast)
        {
            NotifyActiveTransformTween();
            ProxyTransform proxy = GetProxyTransform(charIndex);
            return proxy.AddTween(proxy.Target.DOBlendableRotateBy(byValue, duration, rotateMode));
        }

        public Tweener DOBlendableLocalRotateBy(int charIndex, Vector3 byValue, float duration,
            RotateMode rotateMode = RotateMode.Fast)
        {
            NotifyActiveTransformTween();
            ProxyTransform proxy = GetProxyTransform(charIndex);
            return proxy.AddTween(proxy.Target.DOBlendableLocalRotateBy(byValue, duration, rotateMode));
        }

        public Tweener DOBlendableScaleBy(int charIndex, Vector3 byValue, float duration)
        {
            NotifyActiveTransformTween();
            ProxyTransform proxy = GetProxyTransform(charIndex);
            return proxy.AddTween(proxy.Target.DOBlendableScaleBy(byValue, duration));
        }

        public Tweener DOBlendablePunchRotation(int charIndex, Vector3 punch, float duration, int vibrato = 10, float elasticity = 1)
        {
            NotifyActiveTransformTween();
            ProxyTransform proxy = GetProxyTransform(charIndex);
            return proxy.AddTween(proxy.Target.DOBlendablePunchRotation(punch, duration, vibrato, elasticity));
        }
    }
}
