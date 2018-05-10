using TMPro;
using UnityEngine;

namespace CharTween
{
    public partial class CharTweener : MonoBehaviour
    {
        public float GetAlpha(int charIndex)
        {
            return GetColor(charIndex).a;
        }

        public void SetAlpha(int charIndex, float value)
        {
            var proxy = GetProxyColor(charIndex);
            proxy.Color = WithAlpha(proxy.Color, value);
            _updateColorsPending = true;
        }

        public Color GetColor(int charIndex)
        {
            return proxyColors == null ? default(Color) : GetProxyColor(charIndex).Color;
        }

        public void SetColor(int charIndex, Color value)
        {
            GetProxyColor(charIndex).Color = value;
            _updateColorsPending = true;
        }

        public VertexGradient GetGradient(int charIndex)
        {
            return proxyColors == null ? default(VertexGradient) : GetProxyColor(charIndex).VertexGradient;
        }

        public void SetGradient(int charIndex, VertexGradient value)
        {
            GetProxyColor(charIndex).VertexGradient = value;
            _updateColorsPending = true;
        }

        public Vector3 GetPositionOffset(int charIndex)
        {
            return proxyTransforms == null ? Vector3.zero : GetProxyTransform(charIndex).position;
        }

        public void SetPositionOffset(int charIndex, Vector3 positionOffset)
        {
            GetProxyTransform(charIndex).position = positionOffset;
            _updateVerticesPending = true;
        }

        public Vector3 GetLocalEulerAngles(int charIndex)
        {
            return proxyTransforms == null ? Vector3.zero : GetProxyTransform(charIndex).localEulerAngles;
        }

        public void SetLocalEulerAngles(int charIndex, Vector3 localEulerAngles)
        {
            GetProxyTransform(charIndex).localEulerAngles = localEulerAngles;
            _updateVerticesPending = true;
        }

        public Vector3 GetEulerAngles(int charIndex)
        {
            return proxyTransforms == null ? Vector3.zero : GetProxyTransform(charIndex).eulerAngles;
        }

        public void SetEulerAngles(int charIndex, Vector3 eulerAngles)
        {
            GetProxyTransform(charIndex).eulerAngles = eulerAngles;
            _updateVerticesPending = true;
        }

        public Quaternion GetRotation(int charIndex)
        {
            return proxyTransforms == null ? Quaternion.identity : GetProxyTransform(charIndex).rotation;
        }

        public void SetRotation(int charIndex, Quaternion rotation)
        {
            GetProxyTransform(charIndex).rotation = rotation;
            _updateVerticesPending = true;
        }

        public Quaternion GetLocalRotation(int charIndex)
        {
            return proxyTransforms == null ? Quaternion.identity : GetProxyTransform(charIndex).localRotation;
        }

        public void SetLocalRotation(int charIndex, Quaternion localRotation)
        {
            GetProxyTransform(charIndex).localRotation = localRotation;
            _updateVerticesPending = true;
        }

        public Vector3 GetLocalScale(int charIndex)
        {
            return proxyTransforms == null ? Vector3.one : GetProxyTransform(charIndex).localScale;
        }

        public void SetLocalScale(int charIndex, Vector3 localScale)
        {
            GetProxyTransform(charIndex).localScale = localScale;
            _updateVerticesPending = true;
        }

        public Vector3 GetLossyScale(int charIndex)
        {
            return proxyTransforms == null ? Vector3.one : GetProxyTransform(charIndex).lossyScale;
        }
    }
}
