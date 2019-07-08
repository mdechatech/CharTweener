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
    public partial class CharTweenerUGUI : CharTweener
    {

        protected override Transform CreateProxyTransform(int charIndex)
        {
            var t = new GameObject().AddComponent<RectTransform>();
            t.SetParent(Text.transform.parent, false);
#if UNITY_EDITOR
            t.gameObject.hideFlags = HideFlags.HideAndDontSave;
#endif
            proxyTransforms.Add(charIndex, t);
            return t;
        }

    }
}
