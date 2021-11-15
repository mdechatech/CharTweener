using DG.Tweening;
using TMPro;
using UnityEngine;

namespace CharTween.Examples
{
    [RequireComponent(typeof(TMP_Text))]
    public class CharTweenExampleSimple : MonoBehaviour
    {
        void Start()
        {
            // Set text
            TMP_Text textMesh = GetComponent<TMP_Text>();
            textMesh.text = "DETERMINATION";

            CharTweener tweener = textMesh.GetCharTweener();
            for (int i = 0; i < tweener.CharacterCount; i++)
            {
                // Move characters in a circle
                Tween circleTween = tweener.DOMoveCircle(i, 0.05f, 0.5f)
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
