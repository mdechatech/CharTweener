using DG.Tweening;
using TMPro;
using UnityEngine;

namespace CharTween.Examples
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class CharTweenExampleSimpleUGUI : MonoBehaviour
    {
        void Start()
        {
            RectTransform r;
            
            // Set text
            TMP_Text textMesh = GetComponent<TextMeshProUGUI>();
            textMesh.text = "DETERMINATION";

            CharTweener tweener = textMesh.GetCharTweener();
            for (int i = 0; i < tweener.CharacterCount; i++)
            {
                // Move characters in a circle
                Tween circleTween = tweener.DOMoveCircle(i, 5f, 0.5f)
                    .SetLoops(-1, LoopType.Restart);

                // Fade character color between yellow and white
                Tween colorTween = tweener.DOColor(i, Color.yellow, 0.5f)
                    .SetLoops(-1, LoopType.Yoyo);

                // Offset animations based on character index in string
                float timeOffset = (float)i / tweener.CharacterCount;
                circleTween.fullPosition = timeOffset;
                colorTween.fullPosition = timeOffset;
            }
        }
    }
}