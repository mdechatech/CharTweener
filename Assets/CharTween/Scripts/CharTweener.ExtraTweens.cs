using System;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CharTween
{
    // Contains tweens that don't exist in DOTween and are included as extra utilities
    public partial class CharTweener : MonoBehaviour
    {
        /// <summary>Tweens a character's offset position to the given value. Offset position is position relative to the character's starting position.
        /// Also stores the transform as the tween's target so it can be used for filtered operations</summary>
        /// <param name="charIndex">The index of the character to tween</param>
        /// <param name="endValue">The end value to reach</param>
        /// <param name="duration">The duration of the tween</param>
        /// <param name="snapping">If TRUE the tween will smoothly snap all values to integers</param>
        public Tweener DOOffsetMove(int charIndex, Vector3 endValue, float duration, bool snapping = false)
        {
            NotifyActiveTransformTween();
            ProxyTransform proxy = GetProxyTransform(charIndex);
            return proxy.AddTween(proxy.Target.DOLocalMove(endValue, duration, snapping));
        }

        /// <summary>Tweens a character's x offset position to the given value. Offset position is position relative to the character's starting position.
        /// Also stores the transform as the tween's target so it can be used for filtered operations</summary>
        /// <param name="charIndex">The index of the character to tween</param>
        /// <param name="endValue">The end value to reach</param>
        /// <param name="duration">The duration of the tween</param>
        /// <param name="snapping">If TRUE the tween will smoothly snap all values to integers</param>
        public Tweener DOOffsetMoveX(int charIndex, float endValue, float duration, bool snapping = false)
        {
            NotifyActiveTransformTween();
            ProxyTransform proxy = GetProxyTransform(charIndex);
            return proxy.AddTween(proxy.Target.DOLocalMoveX(endValue, duration, snapping));
        }

        /// <summary>Tweens a character's y offset position to the given value. Offset position is position relative to the character's starting position.
        /// Also stores the transform as the tween's target so it can be used for filtered operations</summary>
        /// <param name="charIndex">The index of the character to tween</param>
        /// <param name="endValue">The end value to reach</param>
        /// <param name="duration">The duration of the tween</param>
        /// <param name="snapping">If TRUE the tween will smoothly snap all values to integers</param>
        public Tweener DOOffsetMoveY(int charIndex, float endValue, float duration, bool snapping = false)
        {
            NotifyActiveTransformTween();
            ProxyTransform proxy = GetProxyTransform(charIndex);
            return proxy.AddTween(proxy.Target.DOLocalMoveY(endValue, duration, snapping));
        }

        /// <summary>Tweens a character's z offset position to the given value. Offset position is position relative to the character's starting position.
        /// Also stores the transform as the tween's target so it can be used for filtered operations</summary>
        /// <param name="charIndex">The index of the character to tween</param>
        /// <param name="endValue">The end value to reach</param>
        /// <param name="duration">The duration of the tween</param>
        /// <param name="snapping">If TRUE the tween will smoothly snap all values to integers</param>
        public Tweener DOOffsetMoveZ(int charIndex, float endValue, float duration, bool snapping = false)
        {
            NotifyActiveTransformTween();
            ProxyTransform proxy = GetProxyTransform(charIndex);
            return proxy.AddTween(proxy.Target.DOLocalMoveZ(endValue, duration, snapping));
        }

        /// <summary>
        /// Moves character in a circle around its start position.
        /// </summary>
        /// <param name="charIndex">The index of the character to tween</param>
        /// <param name="radius">The radius of the path for the character to follow</param>
        /// <param name="duration">The duration of the tween</param>
        /// <returns></returns>
        public Tweener DOMoveCircle(int charIndex, float radius, float duration)
        {
            NotifyActiveTransformTween();
            ProxyTransform proxy = GetProxyTransform(charIndex);
            DOMoveCircleHandler handler = new DOMoveCircleHandler(proxy.Target, radius, duration);
            return proxy.AddTween(DOTween.To(handler.GetTime, handler.SetTime, duration, duration)
                .SetEase(Ease.Linear)
                .SetTarget(proxy.Target));
        }

        /// <summary>
        /// <para>Use <see cref="DOMoveCircle"/> instead, this method is from an old CharTweener version.</para>
        /// <para>Moves character in a circle around its start position.</para>
        /// </summary>
        [Obsolete("DOMoveCircle is more performant")]
        public Tweener DOCircle(int charIndex, float radius, float duration, int pathPoints = 8, PathType pathType = PathType.CatmullRom,
            PathMode pathMode = PathMode.Full3D, int resolution = 10, Color? gizmoColor = null)
        {
            NotifyActiveTransformTween();
            Vector3[] tweenPath = new Vector3[pathPoints + 1];
            Vector3 origin = Vector3.zero;

            for (int i = 0; i < pathPoints; ++i)
            {
                float theta = Mathf.Lerp(0, 2 * Mathf.PI, i / (float)pathPoints);
                tweenPath[i] = new Vector3(radius * Mathf.Cos(theta), radius * Mathf.Sin(theta), 0) + origin;
            }

            tweenPath[pathPoints] = new Vector3(radius, 0, 0) + origin;
            SetOffsetPosition(charIndex, tweenPath[0]);
            return DOLocalPath(charIndex, tweenPath, duration, pathType, pathMode, resolution, gizmoColor);
        }

        /// <summary>
        /// <para>Drifts character around start position, like DOShakePosition but can go more slowly.</para>
        /// <para>Infinite duration. To stop, call <see cref="DOTween.Kill()"/> on the <see cref="CharTweener"/> or Kill() on the returned tween.</para>
        /// <para>Creates garbage.</para>
        /// </summary>
        /// <param name="charIndex">The index of the character to tween.</param>
        /// <param name="strength">Max (x, y, z) distance from start position.</param>
        /// <param name="vibrato">Drift speed in (x, y, z) directions.</param>
        /// <param name="fadeInDuration">Time over which the tween ramps up to full strength, in seconds.</param>
        /// <returns></returns>
        public Tweener DODriftPosition(int charIndex, Vector3 strength, Vector3 vibrato, float fadeInDuration = 1)
        {
            NotifyActiveTransformTween();
            ProxyTransform proxy = GetProxyTransform(charIndex);
            DODriftPositionHandler handler = new DODriftPositionHandler(proxy.Target, strength, vibrato);
            return proxy.AddTween(DOTween.To(handler.GetTime, handler.SetTime, DriftTime, DriftTime)
                .SetEase(Ease.Linear)
                .SetTarget(proxy.Target));
        }

        /// <summary>
        /// <para>Drifts character rotation, like DOShakeRotation but can go more slowly.</para>
        /// <para>Infinite duration. To stop, call <see cref="DOTween.Kill()"/> on the <see cref="CharTweener"/> or Kill() on the returned tween.</para>
        /// <para>Creates garbage.</para>
        /// </summary>
        /// <param name="charIndex"></param>
        /// <param name="strength">Max (x, y, z) euler angles.</param>
        /// <param name="vibrato">Drift speed on (x, y, z) axes</param>
        /// <param name="fadeInDuration">Time over which the tween ramps up to full strength, in seconds.</param>
        /// <returns></returns>
        public Tweener DODriftRotation(int charIndex, Vector3 strength, Vector3 vibrato, float fadeInDuration = 1)
        {
            NotifyActiveTransformTween();
            ProxyTransform proxy = GetProxyTransform(charIndex);
            DODriftRotationHandler handler = new DODriftRotationHandler(proxy.Target, strength, vibrato);
            return proxy.AddTween(DOTween.To(handler.GetTime, handler.SetTime, DriftTime, DriftTime)
                .SetEase(Ease.Linear)
                .SetTarget(proxy.Target));
        }

        // Helper classes 
        private class DOMoveCircleHandler
        {
            public Transform Target;
            public float Radius;
            public float Duration;

            public float Time;

            public DOMoveCircleHandler(Transform target, float radius, float duration)
            {
                Target = target;
                Radius = radius;
                Duration = duration;
            }

            public float GetTime()
            {
                return Time;
            }

            public void SetTime(float value)
            {
                Time = value;
                float angle = Mathf.Lerp(0, 2 * Mathf.PI, Time / Duration);
                Target.localPosition = new Vector3(Mathf.Cos(angle) * Radius, Mathf.Sin(angle) * Radius, 0);
            }
        }

        private const float DriftTime = 10000f;
        private class DODriftPositionHandler
        {
            public Transform Target;
            public Vector3 Strength;
            public Vector3 Vibrato;
            public float FadeInDuration;

            public float Time;

            private float seedX;
            private float seedY;
            private float seedZ;

            public DODriftPositionHandler(Transform target, Vector3 strength, Vector3 vibrato, float fadeInDuration = 1)
            {
                Target = target;
                Strength = strength;
                Vibrato = vibrato;
                FadeInDuration = fadeInDuration;

                seedX = Random.Range(0f, 1000f);
                seedY = Random.Range(0f, 1000f);
                seedZ = Random.Range(0f, 1000f);
            }

            public float GetTime()
            {
                return Time;
            }

            public void SetTime(float time)
            {
                Time = time;
                float fadeIn = Mathf.Clamp01(Time / Mathf.Max(0.0001f, FadeInDuration));
                Target.localPosition = new Vector3(
                    (Mathf.PerlinNoise(Time * Vibrato.x, seedX) * 2 - 1) * Strength.x * fadeIn,
                    (Mathf.PerlinNoise(Time * Vibrato.y, seedY) * 2 - 1) * Strength.y * fadeIn,
                    (Mathf.PerlinNoise(Time * Vibrato.z, seedZ) * 2 - 1) * Strength.z * fadeIn
                );

                // If completing the tween, return character to start position
                if (time == DriftTime)
                    Target.localPosition = Vector3.zero;
            }
        }

        private class DODriftRotationHandler
        {
            public Transform Target;
            public Vector3 Strength;
            public Vector3 Vibrato;
            public float FadeInDuration;

            public float Time;

            private float seedX;
            private float seedY;
            private float seedZ;

            public DODriftRotationHandler(Transform target, Vector3 strength, Vector3 vibrato, float fadeInDuration = 1)
            {
                Target = target;
                Strength = strength;
                Vibrato = vibrato;
                FadeInDuration = fadeInDuration;

                seedX = Random.Range(0f, 1000f);
                seedY = Random.Range(0f, 1000f);
                seedZ = Random.Range(0f, 1000f);
            }

            public float GetTime()
            {
                return Time;
            }

            public void SetTime(float time)
            {
                Time = time;
                float fadeIn = Mathf.Clamp01(Time / Mathf.Max(0.0001f, FadeInDuration));
                Target.localEulerAngles = new Vector3(
                    (Mathf.PerlinNoise(Time * Vibrato.x, seedX) * 2 - 1) * Strength.x * fadeIn,
                    (Mathf.PerlinNoise(Time * Vibrato.y, seedY) * 2 - 1) * Strength.y * fadeIn,
                    (Mathf.PerlinNoise(Time * Vibrato.z, seedZ) * 2 - 1) * Strength.z * fadeIn
                );

                // If completing the tween, return character to original rotation
                if (time == DriftTime)
                    Target.localEulerAngles = Vector3.zero;
            }
        }
    }
}
