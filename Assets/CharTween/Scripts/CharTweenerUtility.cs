using System.Collections.Generic;
using TMPro;

namespace CharTween
{
    public static class CharTweenerUtility
    {
        private static readonly Dictionary<TMP_Text, CharTweener> CharModifiers = new Dictionary<TMP_Text, CharTweener>();

        /// <summary>
        /// Returns a <see cref="CharTweener"/> guaranteeing the same instance is used for the same text.
        /// </summary>
        public static CharTweener GetCharTweener(this TMP_Text text)
        {
            if (CharModifiers.ContainsKey(text))
                return CharModifiers[text];

            var modifier = CharModifiers[text] = text.gameObject.AddComponent<CharTweener>();
            modifier.Text = text;
            modifier.Initialize();
            return modifier;
        }

        public static CharTweenerUGUI GetCharTweenerUGUI(this TextMeshProUGUI text)
        {
            if (CharModifiers.ContainsKey(text))
                return CharModifiers[text] as CharTweenerUGUI;

            var modifier = CharModifiers[text] = text.gameObject.AddComponent<CharTweenerUGUI>();
            modifier.Text = text;
            modifier.Initialize();
            return modifier as CharTweenerUGUI;
        }

    }
}
