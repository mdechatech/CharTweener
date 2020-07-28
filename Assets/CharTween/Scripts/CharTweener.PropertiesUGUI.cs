using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CharTween
{
    public partial class CharTweener
    {
        public void SetAnchorPos(int charIndex, Vector2 anchoredPosition) {
            (GetProxyTransform( charIndex ) as RectTransform).anchoredPosition = anchoredPosition;
        }

        public Vector2 GetAnchorPos(int charIndex) {
            return proxyTransforms == null ? Vector2.zero : (GetProxyTransform( charIndex ) as RectTransform).anchoredPosition;
        }
    }
}
