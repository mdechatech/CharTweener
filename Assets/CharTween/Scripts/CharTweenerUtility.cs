using System.Collections.Generic;
using TMPro;

namespace CharTween
{
    public static class CharTweenerUtility
    {
        private static readonly Dictionary<TMP_Text, CharTweener> Tweeners = new Dictionary<TMP_Text, CharTweener>();

        /// <summary>
        /// Returns a <see cref="CharTweener"/> guaranteeing the same instance is used for the same text.
        /// </summary>
        public static CharTweener GetCharTweener(this TMP_Text text)
        {
            CharTweener tweener;
            if (Tweeners.TryGetValue(text, out tweener))
                return tweener;

            tweener = text.gameObject.AddComponent<CharTweener>();
            tweener.Initialize(text);
            Tweeners.Add(text, tweener);
            return tweener;
        }
    }
}
