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
        /// <summary>
        /// The target text mesh.
        /// </summary>
        public TMP_Text Text { get; set; }

        /// <summary>
        /// Number of characters in the text mesh.
        /// </summary>
        public int CharacterCount { get { return characterCount; } }

        /// <summary>
        /// Interval between updates to the text mesh. If zero, the text mesh updates every frame.
        /// </summary>
        public float VisualUpdateInterval { get { return visualUpdateInterval; }}

        // Maintain both dict and list for both fast lookup (when getting/setting properties)
        // and fast iteration (when applying tweens)
        private Dictionary<int, ProxyColor> proxyColorDict;
        private List<ProxyColor> proxyColorList;
        private Dictionary<int, ProxyTransform> proxyTransformDict;
        private List<ProxyTransform> proxyTransformList;

        private Transform proxyTransformParent;
        private int characterCount;
        private bool transformTweensActive;
        private bool transformsChangedByProperties;
        private bool colorTweensActive;
        private bool colorsChangedByProperties;
        private float tweenDisposeTimer;
        private string lastValue;

        private float visualUpdateInterval;
        private float visualUpdateTimer;

        /// <summary>
        /// Must be called after creation. This is handled automatically when calling 
        /// <see cref="CharTweenerUtility.GetCharTweener"/>.
        /// </summary>
        public void Initialize(TMP_Text text)
        {
            Text = text;
            Text.ForceMeshUpdate(true);
            characterCount = text.textInfo.characterCount;
            lastValue = Text.text;
            TMPro_EventManager.TEXT_CHANGED_EVENT.Add(OnTextChanged);
        }

        /// <summary>
        /// Makes it so that the text mesh is only updated between the given update interval.
        /// <para>For example, if the update interval is 0.1 then the text mesh will visually update 10 times per second.</para>
        /// <para>The default value is zero, which means the text mesh visually updates every frame.</para>
        /// </summary>
        /// <param name="updateInterval">Update interval in seconds. Set to zero to update every frame.</param>
        public void SetVisualUpdateInterval(float updateInterval)
        {
            this.visualUpdateInterval = updateInterval;
        }

        /// <summary>
        /// Updates the text mesh with the current character changes from tweens and properties.
        /// Called automatically on Update().
        /// </summary>
        public void UpdateCharProperties()
        {
            TMP_VertexDataUpdateFlags updateFlags = 0;
            if (transformTweensActive || transformsChangedByProperties)
            {
                UpdateCharPositions();
                updateFlags |= TMP_VertexDataUpdateFlags.Vertices;
                transformsChangedByProperties = false;
            }

            if (colorTweensActive || colorsChangedByProperties)
            {
                UpdateCharColors();
                updateFlags |= TMP_VertexDataUpdateFlags.Colors32;
                colorsChangedByProperties = false;
            }

            if (updateFlags != 0)
                Text.UpdateVertexData(updateFlags);
        }

        /// <summary>Completes all character tweens on the text mesh</summary>
        /// <param name="withCallbacks">For Sequences only: if TRUE also internal Sequence callbacks will be fired,
        /// otherwise they will be ignored</param>
        public void CompleteAll(bool withCallbacks = false)
        {
            if (proxyTransformList != null)
            {
                for (var i = 0; i < proxyTransformList.Count; i++)
                {
                    ProxyTransform proxy = proxyTransformList[i];
                    for (int j = 0; j < proxy.Tweens.Count; j++)
                        proxy.Tweens[j].Complete(withCallbacks);
                    proxy.Tweens.Clear();
                }
            }

            if (proxyColorList != null)
            {
                for (var i = 0; i < proxyColorList.Count; i++)
                {
                    ProxyColor proxy = proxyColorList[i];
                    for (int j = 0; j < proxy.Tweens.Count; j++)
                        proxy.Tweens[j].Complete(withCallbacks);
                    proxy.Tweens.Clear();
                }
            }
        }

        /// <summary>
        /// Completes all tweens on the character in the text mesh
        /// </summary>
        /// <param name="charIndex">Character index</param>
        /// <param name="withCallbacks">For Sequences only: if TRUE also internal Sequence callbacks will be fired,
        /// otherwise they will be ignored</param>
        public void CompleteAll(int charIndex, bool withCallbacks = false)
        {
            ProxyColor proxyColor;
            if (proxyColorDict != null && proxyColorDict.TryGetValue(charIndex, out proxyColor))
                proxyColor.CompleteAll(withCallbacks);

            ProxyTransform proxyTransform;
            if (proxyTransformDict != null && proxyTransformDict.TryGetValue(charIndex, out proxyTransform))
                proxyTransform.CompleteAll(withCallbacks);
        }

        /// <summary>Kills all character tweens on the text mesh</summary>
        /// <param name="complete">If TRUE completes the tweens before killing them</param>
        public void KillAll(bool complete = false)
        {
            if (proxyTransformList != null)
            {
                for (var i = 0; i < proxyTransformList.Count; i++)
                    proxyTransformList[i].KillAll(complete);
            }

            if (proxyColorList != null)
            {
                for (var i = 0; i < proxyColorList.Count; i++)
                    proxyColorList[i].KillAll(complete);
            }
        }

        /// <summary>
        /// Kills all tweens on the character in the text mesh
        /// </summary>
        /// <param name="charIndex">Character index</param>
        /// <param name="withCallbacks">For Sequences only: if TRUE also internal Sequence callbacks will be fired,
        /// otherwise they will be ignored</param>
        public void KillAll(int charIndex, bool complete = false)
        {
            ProxyColor proxyColor;
            if (proxyColorDict != null && proxyColorDict.TryGetValue(charIndex, out proxyColor))
                proxyColor.KillAll(complete);

            ProxyTransform proxyTransform;
            if (proxyTransformDict != null && proxyTransformDict.TryGetValue(charIndex, out proxyTransform))
                proxyTransform.KillAll(complete);
        }

        /// <summary>
        /// Cleans up tweens that are not active. 
        /// </summary>
        public void DisposeInactiveTweens()
        {
            transformTweensActive = false;
            if (proxyTransformList != null)
            {
                for (var i = proxyTransformList.Count - 1; i >= 0; i--)
                {
                    ProxyTransform proxy = proxyTransformList[i];
                    proxy.DisposeFinishedTweens();
                    if (proxy.Tweens.Count > 0)
                        transformTweensActive = true;
                }
            }

            colorTweensActive = false;
            if (proxyColorList != null)
            {
                for (var i = proxyColorList.Count - 1; i >= 0; i--)
                {
                    ProxyColor proxy = proxyColorList[i];
                    proxy.DisposeFinishedTweens();
                    if (proxy.Tweens.Count > 0)
                        colorTweensActive = true;
                }
            }
        }

        /// <summary>
        /// Same as <see cref="UpdateCharProperties"/>, from old CharTween version
        /// </summary>
        public void ApplyChanges()
        {
            UpdateCharProperties();
        }

        private void OnTextChanged(Object changedText)
        {
            if (!Text)
            {
                TMPro_EventManager.TEXT_CHANGED_EVENT.Remove(OnTextChanged);
                return;
            }

            if (changedText == Text)
            {
                characterCount = Text.textInfo.characterCount;
                if (lastValue != Text.text)
                {
                    UpdateStartPositions();
                    lastValue = Text.text;
                }

                UpdateCharProperties();
            }
        }

        private void NotifyActiveTransformTween()
        {
            transformTweensActive = true;
        }

        private void NotifyActiveColorTween()
        {
            colorTweensActive = true;
        }

        void Awake()
        {
            proxyTransformParent = new GameObject("Proxy Transforms").transform;
            proxyTransformParent.SetParent(transform);
            proxyTransformParent.localPosition = Vector3.zero;
        }

        void Update()
        {
            if (!Text && gameObject)
            {
                Destroy(this);
                return;
            }

            if (!Text.enabled)
                return;

            tweenDisposeTimer += Time.deltaTime;
            if (tweenDisposeTimer >= 1)
            {
                tweenDisposeTimer = 0;
                DisposeInactiveTweens();
            }

            if (visualUpdateInterval > 0)
            {
                visualUpdateTimer += Time.deltaTime;
                if (visualUpdateTimer >= visualUpdateInterval)
                {
                    visualUpdateTimer = 0;
                    UpdateCharProperties();
                }
            }
            else
            {
                UpdateCharProperties();
            }
        }

        void OnDestroy()
        {
            TMPro_EventManager.TEXT_CHANGED_EVENT.Remove(OnTextChanged);
            Destroy(proxyTransformParent.gameObject);
        }

        private void UpdateStartPositions()
        {
            if (proxyTransformDict == null)
                return;

            TMP_CharacterInfo[] charInfo = Text.textInfo.characterInfo;
            int oldLength = lastValue.Length;
            int newLength = Text.text.Length;

            foreach (var pair in proxyTransformDict)
            {
                int charIndex = pair.Key;
                ProxyTransform proxy = pair.Value;

                if (charIndex >= oldLength)
                {
                    // Character was not present in old text and is present in new text
                    if (charIndex < newLength)
                        proxy.AssignCharInfo(charInfo[pair.Key]);
                }
                else // pair.Key < oldLength
                {
                    // Character was present in old text and is not present in new text
                    if (charIndex >= newLength)
                        proxy.AssignCharIndex(charIndex);
                    else
                        proxy.AssignCharInfo(charInfo[pair.Key]);
                }
            }
        }

        // Returns null if proxy does not exist
        private ProxyTransform TryGetProxyTransform(int charIndex)
        {
            if (proxyTransformDict == null)
                return null;

            ProxyTransform proxyTransform;
            return proxyTransformDict.TryGetValue(charIndex, out proxyTransform) ? proxyTransform : null;
        }

        // Will create proxy if does not exist
        private ProxyTransform GetProxyTransform(int charIndex)
        {
            if (proxyTransformDict == null)
            {
                proxyTransformDict = new Dictionary<int, ProxyTransform>(CharacterCount);
                proxyTransformList = new List<ProxyTransform>(CharacterCount);
            }

            ProxyTransform proxy;
            if (!proxyTransformDict.TryGetValue(charIndex, out proxy))
            {
                Transform t = new GameObject(charIndex.ToString()).transform;
                t.SetParent(proxyTransformParent);

                proxy = charIndex >= CharacterCount
                    ? new ProxyTransform(t, proxyTransformParent, charIndex)
                    : new ProxyTransform(t, proxyTransformParent, Text.textInfo.characterInfo[charIndex]);
                proxyTransformDict.Add(charIndex, proxy);
                proxyTransformList.Add(proxy);
            }

            return proxy;
        }

        // Returns null if proxy does not exist
        private ProxyColor TryGetProxyColor(int charIndex)
        {
            if (proxyColorDict == null)
                return null;

            ProxyColor proxyColor;
            return proxyColorDict.TryGetValue(charIndex, out proxyColor) ? proxyColor : null;
        }

        // Will create proxy if does not exist
        private ProxyColor GetProxyColor(int charIndex)
        {
            if (proxyColorDict == null)
            {
                proxyColorDict = new Dictionary<int, ProxyColor>(CharacterCount);
                proxyColorList = new List<ProxyColor>(CharacterCount);
            }

            ProxyColor proxy;
            if (!proxyColorDict.TryGetValue(charIndex, out proxy))
            {
                proxy = new ProxyColor(Text.color, Text.colorGradient, Text.enableVertexGradient, charIndex);
                proxyColorDict.Add(charIndex, proxy);
                proxyColorList.Add(proxy);
                return proxy;
            }

            return proxy;
        }

        private void UpdateCharPositions()
        {
            // If no transform tweens have been created then the list of proxies will not exist
            if (proxyTransformList == null)
                return;

            for (var i = proxyTransformList.Count - 1; i >= 0; i--)
            {
                ProxyTransform proxy = proxyTransformList[i];
                if (proxy.CharIndex >= characterCount)
                    continue;

                int charIndex = proxy.CharIndex;
                TMP_CharacterInfo charInfo = Text.textInfo.characterInfo[charIndex];

                if (!charInfo.isVisible || !proxy.Target || !proxy.Target.hasChanged)
                    continue;

                int materialIndex = charInfo.materialReferenceIndex;
                int vertexIndex = charInfo.vertexIndex;

                Vector3 origin = (charInfo.topLeft + charInfo.bottomRight) * 0.5f;

                Vector3[] destinationVertices = Text.textInfo.meshInfo[materialIndex].vertices;
                Matrix4x4 matrix = Matrix4x4.TRS(proxy.OffsetPosition, proxy.Target.localRotation, proxy.Target.localScale);

                destinationVertices[vertexIndex + 1] = matrix.MultiplyPoint3x4(charInfo.topLeft - origin) + origin;
                destinationVertices[vertexIndex + 2] = matrix.MultiplyPoint3x4(charInfo.topRight - origin) + origin;
                destinationVertices[vertexIndex + 0] = matrix.MultiplyPoint3x4(charInfo.bottomLeft - origin) + origin;
                destinationVertices[vertexIndex + 3] = matrix.MultiplyPoint3x4(charInfo.bottomRight - origin) + origin;
            }
        }

        private void UpdateCharColors()
        {
            // If no color tweens have been created then the list of proxies will not exist
            if (proxyColorList == null)
                return;

            for (var i = proxyColorList.Count - 1; i >= 0; i--)
            {
                ProxyColor proxy = proxyColorList[i];
                if (proxy.CharIndex >= characterCount)
                    continue;

                TMP_CharacterInfo charInfo = Text.textInfo.characterInfo[proxy.CharIndex];
                if (!charInfo.isVisible || !proxy.IsModified)
                    continue;

                int materialIndex = charInfo.materialReferenceIndex;
                int vertexIndex = charInfo.vertexIndex;

                Color32[] destinationColors = Text.textInfo.meshInfo[materialIndex].colors32;
                destinationColors[vertexIndex + 1] = proxy.ColorGradient.topLeft;
                destinationColors[vertexIndex + 2] = proxy.ColorGradient.topRight;
                destinationColors[vertexIndex + 0] = proxy.ColorGradient.bottomLeft;
                destinationColors[vertexIndex + 3] = proxy.ColorGradient.bottomRight;
            }
        }

        private static Color WithAlpha(Color color, float alpha)
        {
            return new Color(color.r, color.g, color.b, alpha);
        }

        // Stores color data for a character. CharTweener updates vertex colors in the text mesh using this data.
        private class ProxyColor
        {
            // This handles DOColorGradient
            private static readonly VertexGradientPlugin VertexGradientPlugin = new VertexGradientPlugin();

            public bool IsModified;

            public VertexGradient ColorGradient; // Setting this via field will not change dirty flag
            public bool UseColorGradient; // Setting this via field will not change dirty flag
            public int CharIndex;
            public Color Color { get { return GetColor(); } set { SetColor(value); } }
            public float Alpha { get { return GetAlpha(); } set { SetAlpha(value); } }

            public List<Tween> Tweens;

            public ProxyColor(Color color, VertexGradient colorGradient, bool useColorGradient, int charIndex)
            {
                Tweens = new List<Tween>();
                UseColorGradient = useColorGradient;
                if (UseColorGradient)
                    ColorGradient = colorGradient;
                else
                    Color = color;
                CharIndex = charIndex;
            }

            public void CompleteAll(bool withCallbacks = false)
            {
                for (var i = 0; i < Tweens.Count; i++)
                {
                    Tweens[i].Complete(withCallbacks);
                }
            }

            public void KillAll(bool complete = false)
            {
                for (var i = 0; i < Tweens.Count; i++)
                {
                    Tweens[i].Kill(complete);
                }
            }

            public T AddTween<T>(T tween) where T : Tween
            {
                Tweens.Add(tween);
                return tween;
            }

            public void DisposeFinishedTweens()
            {
                if (Tweens.Count == 0)
                    return;

                for (int i = Tweens.Count - 1; i >= 0; i--)
                {
                    Tween tween = Tweens[i];
                    if (!tween.active)
                        Tweens.RemoveAt(i);
                }
            }

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

            public Tweener DOColorGradient(VertexGradient endValue, float duration)
            {
                IsModified = true;
                return DOTween.To(VertexGradientPlugin,
                    GetColorGradient, SetColorGradient,
                    endValue, duration);
            }

            public Color GetColor()
            {
                return ColorGradient.topLeft;
            }

            public void SetColor(Color value)
            {
                IsModified = true;
                UseColorGradient = false;
                ColorGradient.bottomLeft = value;
                ColorGradient.topLeft = value;
                ColorGradient.bottomRight = value;
                ColorGradient.topRight = value;
            }

            public float GetAlpha()
            {
                return ColorGradient.topLeft.a;
            }

            public void SetAlpha(float value)
            {
                IsModified = true;
                ColorGradient.bottomLeft = WithAlpha(ColorGradient.bottomLeft, value);
                ColorGradient.bottomRight = WithAlpha(ColorGradient.bottomRight, value);
                ColorGradient.topLeft = WithAlpha(ColorGradient.topLeft, value);
                ColorGradient.topRight = WithAlpha(ColorGradient.topRight, value);
            }

            public VertexGradient GetColorGradient()
            {
                return ColorGradient;
            }

            public void SetColorGradient(VertexGradient value)
            {
                IsModified = true;
                UseColorGradient = true;
                ColorGradient = value;
            }
        }

        // Stores transform data for a character. CharTweener updates vertex positions in the text mesh using this data.
        private class ProxyTransform
        {
            public Transform Target { get { return target; } }
            public int CharIndex { get { return charIndex; } }
            public Vector3 LocalStartPosition { get { return localStartPosition; } }
            public Vector3 StartPosition { get { return parent.TransformPoint(LocalStartPosition); } }
            public Vector3 OffsetPosition { get { return target.localPosition; } set { target.localPosition = value; } }

            public Vector3 LocalPosition
            {
                get { return target.localPosition + localStartPosition; }
                set { target.localPosition = value - localStartPosition; }
            }

            public Vector3 Position
            {
                get { return target.position + LocalStartPosition; }
                set { target.position = value - LocalStartPosition; }
            }

            public List<Tween> Tweens;
            private Transform target;
            private Transform parent;
            private int charIndex;
            private Vector3 localStartPosition;

            public ProxyTransform(Transform target, Transform parent, int charIndex)
            {
                Tweens = new List<Tween>();
                this.target = target;
                this.parent = parent;
                AssignCharIndex(charIndex);
                Target.localPosition = Vector3.zero;
            }

            public ProxyTransform(Transform target, Transform parent, TMP_CharacterInfo charInfo)
            {
                Tweens = new List<Tween>();
                this.target = target;
                this.parent = parent;
                AssignCharInfo(charInfo);
                Target.localPosition = Vector3.zero;
            }

            public T AddTween<T>(T tween) where T : Tween
            {
                Tweens.Add(tween);
                return tween;
            }

            public void CompleteAll(bool withCallbacks = false)
            {
                for (var i = 0; i < Tweens.Count; i++)
                    Tweens[i].Complete(withCallbacks);
                Tweens.Clear();
            }

            public void KillAll(bool complete = false)
            {
                for (var i = 0; i < Tweens.Count; i++)
                    Tweens[i].Kill(complete);
                Tweens.Clear();
            }

            public void DisposeFinishedTweens()
            {
                if (Tweens.Count == 0)
                    return;

                for (int i = Tweens.Count - 1; i >= 0; i--)
                {
                    Tween tween = Tweens[i];
                    if (!tween.active)
                        Tweens.RemoveAt(i);
                }
            }

            public void AssignCharInfo(TMP_CharacterInfo charInfo)
            {
                charIndex = charInfo.index;
                if (charInfo.isVisible)
                {
                    localStartPosition = new Vector3(
                        (charInfo.topLeft.x + charInfo.bottomRight.x) * 0.5f,
                        (charInfo.topLeft.y + charInfo.bottomRight.y) * 0.5f,
                        (charInfo.topLeft.z + charInfo.bottomRight.z) * 0.5f
                    );
                }
                else
                {
                    localStartPosition = Vector3.zero;
                }
            }

            public void AssignCharIndex(int charIndex)
            {
                this.charIndex = charIndex;
                localStartPosition = Vector3.zero;
            }

            public Tweener DOMove(Vector3 endValue, float duration, bool snapping = false)
            {
                return snapping
                    ? DOTween.To(GetPosition, SetPositionAndSnap, endValue, duration).SetTarget(target)
                    : DOTween.To(GetPosition, SetPosition, endValue, duration).SetTarget(target);
            }

            public Tweener DOMoveX(float endValue, float duration, bool snapping = false)
            {
                return snapping
                    ? DOTween.ToAxis(GetPosition, SetPositionAndSnap, endValue, duration, AxisConstraint.X).SetTarget(target)
                    : DOTween.ToAxis(GetPosition, SetPosition, endValue, duration, AxisConstraint.X).SetTarget(target);
            }

            public Tweener DOMoveY(float endValue, float duration, bool snapping = false)
            {
                return snapping
                    ? DOTween.ToAxis(GetPosition, SetPositionAndSnap, endValue, duration, AxisConstraint.Y).SetTarget(target)
                    : DOTween.ToAxis(GetPosition, SetPosition, endValue, duration, AxisConstraint.Y).SetTarget(target);
            }

            public Tweener DOMoveZ(float endValue, float duration, bool snapping = false)
            {
                return snapping
                    ? DOTween.ToAxis(GetPosition, SetPositionAndSnap, endValue, duration, AxisConstraint.Z).SetTarget(target)
                    : DOTween.ToAxis(GetPosition, SetPosition, endValue, duration, AxisConstraint.Z).SetTarget(target);
            }

            public Tweener DOLocalMove(Vector3 endValue, float duration, bool snapping = false)
            {
                return snapping
                    ? DOTween.To(GetLocalPosition, SetLocalPositionAndSnap, endValue, duration).SetTarget(target)
                    : DOTween.To(GetLocalPosition, SetLocalPosition, endValue, duration).SetTarget(target);
            }

            public Tweener DOLocalMoveX(float endValue, float duration, bool snapping = false)
            {
                return snapping
                    ? DOTween.ToAxis(GetLocalPosition, SetLocalPositionAndSnap, endValue, duration, AxisConstraint.X).SetTarget(target)
                    : DOTween.ToAxis(GetLocalPosition, SetLocalPosition, endValue, duration, AxisConstraint.X).SetTarget(target);
            }

            public Tweener DOLocalMoveY(float endValue, float duration, bool snapping = false)
            {
                return snapping
                    ? DOTween.ToAxis(GetLocalPosition, SetLocalPositionAndSnap, endValue, duration, AxisConstraint.Y).SetTarget(target)
                    : DOTween.ToAxis(GetLocalPosition, SetLocalPosition, endValue, duration, AxisConstraint.Y).SetTarget(target);
            }

            public Tweener DOLocalMoveZ(float endValue, float duration, bool snapping = false)
            {
                return snapping
                    ? DOTween.ToAxis(GetLocalPosition, SetLocalPositionAndSnap, endValue, duration, AxisConstraint.Z).SetTarget(target)
                    : DOTween.ToAxis(GetLocalPosition, SetLocalPosition, endValue, duration, AxisConstraint.Z).SetTarget(target);
            }

            // These getters and setters are for use with DOTween.To
            public Vector3 GetLocalPosition() { return LocalPosition; }
            public void SetLocalPosition(Vector3 value) { LocalPosition = value; }

            public void SetLocalPositionAndSnap(Vector3 value)
            {
                LocalPosition = new Vector3(Mathf.Round(value.x), Mathf.Round(value.y), Mathf.Round(value.z));
            }

            public Vector3 GetPosition() { return Position; }
            public void SetPosition(Vector3 value) { Position = value; }

            public void SetPositionAndSnap(Vector3 value)
            {
                Position = new Vector3(Mathf.Round(value.x), Mathf.Round(value.y), Mathf.Round(value.z));
            }
        }
    }
}
