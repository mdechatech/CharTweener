using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace CharTween
{
    /// <summary>
    /// Wraps around a <see cref="TextMeshPro"/> or <see cref="TextMeshProUGUI"/> to provide methods for changing the properties
    /// of their individual characters.
    /// </summary>
    [AddComponentMenu("")]
    public partial class CharTweener : MonoBehaviour
    {
        private class CharColor
        {
            private static readonly VertexGradientPlugin VertexGradientPlugin = new VertexGradientPlugin();

            public bool IsModified;
            public VertexGradient VertexGradient;

            public Color Color { get { return GetColor(); } set { SetColor(value); } }

            public Tweener DOFade(float endValue, float duration)
            {
                IsModified = true;
                return DOTween.To(GetAlpha, SetAlpha, endValue, duration);
            }

            public Tweener DOColor(Color endValue, float duration)
            {
                IsModified = true;
                return DOTween.To(GetColor, SetColor, endValue, duration);
            }

            public Tweener DOVertexGradient(VertexGradient endValue, float duration)
            {
                IsModified = true;
                return DOTween.To(VertexGradientPlugin,
                    GetVertexGradient, SetVertexGradient,
                    endValue, duration);
            }

            // Accessors passed in as lambdas to avoid allocation
            private Color GetColor()
            {
                return VertexGradient.topLeft;
            }

            private void SetColor(Color value)
            {
                IsModified = true;
                Color32 color32 = value;
                VertexGradient.bottomLeft = color32;
                VertexGradient.topLeft = color32;
                VertexGradient.bottomRight = color32;
                VertexGradient.topRight = color32;
            }

            private float GetAlpha()
            {
                return VertexGradient.topLeft.a;
            }

            private void SetAlpha(float value)
            {
                IsModified = true;
                VertexGradient.bottomLeft = WithAlpha(VertexGradient.bottomLeft, value);
                VertexGradient.bottomRight = WithAlpha(VertexGradient.bottomRight, value);
                VertexGradient.topLeft = WithAlpha(VertexGradient.topLeft, value);
                VertexGradient.topRight = WithAlpha(VertexGradient.topRight, value);
            }

            private VertexGradient GetVertexGradient()
            {
                return VertexGradient;
            }

            private void SetVertexGradient(VertexGradient value)
            {
                IsModified = true;
                VertexGradient = value;
            }
        }

        private List<Tween> _activeVertexTweens;
        private List<Tween> _activeColorTweens;

        private Dictionary<int, Transform> proxyTransforms;
        private Dictionary<int, CharColor> proxyColors;
        private TMP_MeshInfo[] _meshCache;

        private bool _updateVerticesPending;
        private bool _updateColorsPending;

#if UNITY_EDITOR
        private void Awake()
        {
            hideFlags = HideFlags.HideInInspector;
        }
#endif
        private void Update()
        {
            ApplyChanges();
        }

        private void OnDestroy()
        {
            TMPro_EventManager.TEXT_CHANGED_EVENT.Remove(OnTextChanged);

            if (proxyTransforms != null)
            {
                foreach (var pair in proxyTransforms)
                    Destroy(pair.Value.gameObject);
            }

            foreach (var tween in _activeColorTweens)
                tween.Kill();

            foreach (var tween in _activeVertexTweens)
                tween.Kill();
        }

        public TMP_Text Text { get; set; }

        public int CharacterCount { get { return Text ? Text.textInfo.characterCount : 0; } }

        /// <summary>
        /// Must be called after <see cref="Text"/> is assigned. This is handled automatically when calling 
        /// <see cref="CharTweenerUtility.GetCharTweener"/>.
        /// </summary>
        public void Initialize()
        {
            Text.ForceMeshUpdate(true);
            RefreshCache();

            _activeVertexTweens = new List<Tween>();
            _activeColorTweens = new List<Tween>();

            TMPro_EventManager.TEXT_CHANGED_EVENT.Add(OnTextChanged);
        }

        /// <summary>
        /// This happens automatically on Update().
        /// </summary>
        public void ApplyChanges()
        {
            if (!Text && gameObject)
            {
                Destroy(this);
                return;
            }

            if (!Text.enabled)
                return;

            if (_updateVerticesPending || _activeVertexTweens.Count > 0)
            {
                UpdateVertexDataFromProxies();
                Text.UpdateVertexData(TMP_VertexDataUpdateFlags.Vertices);

                _updateVerticesPending = false;
            }

            if (_updateColorsPending || _activeColorTweens.Count > 0)
            {
                UpdateColorFromProxies();
                Text.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);

                _updateColorsPending = false;
            }

            for (var i = _activeVertexTweens.Count - 1; i >= 0; --i)
            {
                if (!_activeVertexTweens[i].IsActive())
                    _activeVertexTweens.RemoveAt(i);
            }

            for (var i = _activeColorTweens.Count - 1; i >= 0; --i)
            {
                if (!_activeColorTweens[i].IsActive())
                    _activeColorTweens.RemoveAt(i);
            }
        }

        private void RefreshCache()
        {
            _meshCache = Text.textInfo.CopyMeshInfoVertexData();
        }

        private void OnTextChanged(Object text)
        {
            if (!Text)
            {
                TMPro_EventManager.TEXT_CHANGED_EVENT.Remove(OnTextChanged);
                return;
            }

            if (text == Text)
            {
                RefreshCache();
                ApplyChanges();
            }
        }

        // Helper for method chaining
        private T MonitorTransformTween<T>(T tween) where T : Tween
        {
            _activeVertexTweens.Add(tween);
            return tween;
        }

        // Helper for method chaining
        private T MonitorColorTween<T>(T tween) where T : Tween
        {
            _activeColorTweens.Add(tween);
            return tween;
        }

        // Helper for method chaining
        private Transform GetProxyTransform(int charIndex)
        {
            // The modifier works by creating a proxy transform for each character in the text

            // Modifications are applied to the proxies rather than the characters themselves
            // On every update, the modifier copies the transform matrix on each proxy and
            // applies it to the corresponding character

            // This is done because DOTween exposes many additional methods for transforms that are not
            // available for plain Vector3s - for example, the DORotate method

            if (proxyTransforms == null)
                proxyTransforms = new Dictionary<int, Transform>(Text.textInfo.characterCount);

            Transform proxy;
            if (!proxyTransforms.TryGetValue(charIndex, out proxy))
                proxy = CreateProxyTransform(charIndex);

            return proxy;
        }

        private Transform CreateProxyTransform(int charIndex)
        {
            var t = new GameObject().transform;
            t.SetParent(Text.transform.parent, false);
#if UNITY_EDITOR
            t.gameObject.hideFlags = HideFlags.HideAndDontSave;
#endif
            proxyTransforms.Add(charIndex, t);
            return t;
        }

        private CharColor GetProxyColor(int charIndex)
        {
            if (proxyColors == null)
                proxyColors = new Dictionary<int, CharColor>(Text.textInfo.characterCount);

            CharColor color;
            if (!proxyColors.TryGetValue(charIndex, out color))
                color = CreateProxyColor(charIndex);

            return color;
        }


        private CharColor CreateProxyColor(int charIndex)
        {
            var color = new CharColor
            {
                Color = Text.color,
                VertexGradient = Text.colorGradient
            };

            proxyColors.Add(charIndex, color);
            return color;
        }

        private void UpdateVertexDataFromProxies()
        {
            if (proxyTransforms == null)
                return;

            foreach (var entry in proxyTransforms)
            {
                if (entry.Key >= Text.textInfo.characterCount)
                    continue;

                var charInfo = Text.textInfo.characterInfo[entry.Key];
                var proxy = entry.Value;

                if (!charInfo.isVisible || !proxy)
                    continue;
                
                var materialIndex = charInfo.materialReferenceIndex;
                var vertexIndex = charInfo.vertexIndex;
                var sourceVertices = _meshCache[materialIndex].vertices;

                // Getting this from charInfo.vertex_TL, etc. yields the wrong values
                var sourceTopLeft = sourceVertices[vertexIndex + 1];
                var sourceTopRight = sourceVertices[vertexIndex + 2];
                var sourceBottomLeft = sourceVertices[vertexIndex + 0];
                var sourceBottomRight = sourceVertices[vertexIndex + 3];

                var offset = (sourceTopLeft + sourceBottomRight) * 0.5f;

                var destinationVertices = Text.textInfo.meshInfo[materialIndex].vertices;
                var matrix = Matrix4x4.TRS(proxy.localPosition, proxy.localRotation, proxy.localScale);

                var destinationTopLeft = matrix.MultiplyPoint3x4(sourceTopLeft - offset) + offset;
                var destinationTopRight = matrix.MultiplyPoint3x4(sourceTopRight - offset) + offset;
                var destinationBottomLeft = matrix.MultiplyPoint3x4(sourceBottomLeft - offset) + offset;
                var destinationBottomRight = matrix.MultiplyPoint3x4(sourceBottomRight - offset) + offset;

                destinationVertices[vertexIndex + 1] = destinationTopLeft;
                destinationVertices[vertexIndex + 2] = destinationTopRight;
                destinationVertices[vertexIndex + 0] = destinationBottomLeft;
                destinationVertices[vertexIndex + 3] = destinationBottomRight;
            }
        }

        private static Color WithAlpha(Color color, float alpha)
        {
            return new Color(color.r, color.g, color.b, alpha);
        }

        private void UpdateColorFromProxies()
        {
            if (proxyColors == null)
                return;

            foreach (var entry in proxyColors)
            {
                if (entry.Key >= Text.textInfo.characterCount)
                    continue;

                var charInfo = Text.textInfo.characterInfo[entry.Key];
                var proxy = entry.Value;

                if (!charInfo.isVisible || !proxy.IsModified)
                    continue;

                var materialIndex = charInfo.materialReferenceIndex;
                var vertexIndex = charInfo.vertexIndex;

                var destinationColors = Text.textInfo.meshInfo[materialIndex].colors32;
                destinationColors[vertexIndex + 1] = proxy.VertexGradient.topLeft;
                destinationColors[vertexIndex + 2] = proxy.VertexGradient.topRight;
                destinationColors[vertexIndex + 0] = proxy.VertexGradient.bottomLeft;
                destinationColors[vertexIndex + 3] = proxy.VertexGradient.bottomRight;
            }
        }
    }
}
