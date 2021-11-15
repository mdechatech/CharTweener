using System;
using UnityEngine;
using DG.Tweening;
using TMPro;
using Random = UnityEngine.Random;

namespace CharTween.Examples
{
    public class CharTweenExampleFull : MonoBehaviour
    {
        private Transform[] slides;
        private int currentSlideIndex;
        private Transform currentSlide;
        private Action[] slideActions;

        private void Feature_Transform_OffsetLocalWorldPosition()
        {
            TweenChars("Text 1", (textMesh, tweener, i) =>
                tweener.DOOffsetMoveZ(i, 1, 1).SetEase(Ease.InOutCubic).SetLoops(-1, LoopType.Yoyo));
            TweenChars("Text 2", (textMesh, tweener, i) =>
                tweener.DOLocalMove(i, Vector3.zero, 1).SetEase(Ease.InOutCubic).SetLoops(-1, LoopType.Yoyo));
            TweenChars("Text 3", (textMesh, tweener, i) =>
                tweener.DOMove(i, Vector3.zero, 1).SetEase(Ease.InOutCubic).SetLoops(-1, LoopType.Yoyo));
        }

        private void Feature_Transform_PositionRotationScale()
        {
            TweenChars("Text 1", (textMesh, tweener, i) =>
                tweener.DOShakePosition(i, 1, 0.03f, 100, 90, false, false).SetLoops(-1, LoopType.Restart));
            TweenChars("Text 2", (textMesh, tweener, i) =>
                tweener.DOPunchRotation(i, new Vector3(-90, 0, 0), 1, 5).SetLoops(-1, LoopType.Restart));
            TweenChars("Text 3", (textMesh, tweener, i) =>
                tweener.DOScale(i, 0, 1).SetLoops(-1, LoopType.Yoyo));
        }

        private void Feature_Color_FadeColorGradient()
        {
            TweenChars("Text 1", (textMesh, tweener, i) =>
                tweener.DOFade(i, 0, 1).SetLoops(-1, LoopType.Yoyo));
            TweenChars("Text 2", (textMesh, tweener, i) =>
                tweener.DOColor(i, Color.magenta, 1).SetLoops(-1, LoopType.Yoyo));
            TweenChars("Text 3", (textMesh, tweener, i) =>
                tweener.DOGradient(i, new VertexGradient(Color.cyan, Color.cyan, Color.magenta, Color.magenta), 1).SetLoops(-1, LoopType.Yoyo));
        }

        private void Feature_Sequence()
        {
            TweenText("Text 1", (textMesh, tweener) =>
            {
                for (int i = 0; i < tweener.CharacterCount; i++)
                {
                    tweener.SetColor(i, new Color(0, 1, 1, 0));
                    tweener.SetLocalScale(i, 0);
                }
                tweener.UpdateCharProperties();

                Sequence sequence = DOTween.Sequence();
                for (int i = 0; i < tweener.CharacterCount; i++)
                {
                    float timeOffset = Mathf.Lerp(0, 1, i / (float) tweener.CharacterCount);
                    Sequence charSequence = DOTween.Sequence();
                    charSequence
                        .Append(tweener.DOOffsetMoveY(i, 0.5f, 0.5f).SetEase(Ease.InOutCubic))
                        .Join(tweener.DOFade(i, 1, 0.5f))
                        .Join(tweener.DOScale(i, 1, 0.5f).SetEase(Ease.OutBack, overshoot: 10))
                        .Append(tweener.DOOffsetMoveY(i, 0, 0.5f).SetEase(Ease.OutBounce))
                        .Join(tweener.DOColor(i, textMesh.color, 0.5f));
                    sequence.Insert(timeOffset, charSequence);
                }

                sequence.SetLoops(-1, LoopType.Yoyo);
                return sequence;
            });
            TweenText("Text 2", (textMesh, tweener) =>
            {
                Vector3[,] offsets = new [,]
                {
                    {Vector3.up, Vector3.right, Vector3.left, Vector3.down},
                    {Vector3.right+Vector3.up, Vector3.down+Vector3.right, Vector3.up+Vector3.left, Vector3.left+Vector3.down},
                    {Vector3.up, Vector3.right, Vector3.left, Vector3.down},
                    {Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero}
                };

                Sequence sequence = DOTween.Sequence();
                for (int i = 0; i < tweener.CharacterCount; i++)
                {
                    float timeOffset = Mathf.Lerp(0, 1, i / (float) tweener.CharacterCount);
                    Sequence charSequence = DOTween.Sequence();
                    for (int j = 0; j < 4; j++)
                    {
                        charSequence.Append(tweener.DOOffsetMove(i, offsets[j, i % 4] * 0.5f, 0.25f)
                            .SetEase(Ease.OutQuint));
                        charSequence.Join(tweener.DOScale(i, j == 3 ? 1 : Random.Range(0.25f, 2f), 0.25f)
                            .SetEase(Ease.OutQuint));
                        charSequence.Join(tweener
                            .DOLocalRotate(i, j == 3 ? Vector3.zero : Vector3.forward * 90 * Random.Range(-1, 2), 0.25f)
                            .SetEase(Ease.OutQuint));
                    }
                    sequence.Insert(timeOffset, charSequence);
                }

                sequence.SetLoops(-1, LoopType.Restart);
                return sequence;
            });
            TweenText("Text 3", (textMesh, tweener) =>
            {
                for (int i = 0; i < tweener.CharacterCount; i++)
                {
                    tweener.SetColor(i, new Color(textMesh.color.r, textMesh.color.g, textMesh.color.b, 0));
                }
                tweener.UpdateCharProperties();
                Sequence sequence = DOTween.Sequence();
                sequence.AppendInterval(1);
                for (int i = 0; i < tweener.CharacterCount; i++)
                {
                    Sequence charSequence = DOTween.Sequence();
                    charSequence.Append(tweener.DOFade(i, 1, 1).SetEase(Ease.Flash, 5 + 2*Random.Range(1, 5)));
                    sequence.Insert(1, charSequence);
                }

                sequence.AppendInterval(1);
                sequence.SetLoops(-1, LoopType.Yoyo);
                return sequence;
            });
        }

        private delegate Tween CharTweenOutput(TMP_Text textMesh, CharTweener tweener, int i);
        private delegate Tween TextTweenOutput(TMP_Text textMesh, CharTweener tweener);
        private void TweenChars(string childName, CharTweenOutput output)
        {
            TMP_Text textMesh = currentSlide.Find(childName).GetComponent<TMP_Text>();
            CharTweener tweener = textMesh.GetCharTweener();
            for (int charIndex = 0; charIndex < tweener.CharacterCount; charIndex++)
            {
                Tween tween = output(textMesh, tweener, charIndex);
                tween.SetId(currentSlide);
                tween.fullPosition = Mathf.Lerp(0, tween.Duration(includeLoops: false), charIndex / (float) tweener.CharacterCount);
            }
        }

        private Tween TweenText(string childName, TextTweenOutput output)
        {
            TMP_Text textMesh = currentSlide.Find(childName).GetComponent<TMP_Text>();
            CharTweener tweener = textMesh.GetCharTweener();
            Tween tween = output(textMesh, tweener);
            tween.SetId(currentSlide);
            return tween;
        }

        void Awake()
        {
            slides = new Transform[transform.childCount];
            for (int i = 0; i < slides.Length; i++)
            {
                Transform slide = transform.GetChild(i);
                slide.gameObject.SetActive(false);
                slides[i] = slide;
            }

            slideActions = new Action[]
            {
                Feature_Transform_OffsetLocalWorldPosition,
                Feature_Transform_PositionRotationScale,
                Feature_Color_FadeColorGradient,
                Feature_Sequence
            };

            currentSlideIndex = -1;
            NextSlide();
        }

        void Update()
        {
            if (Input.GetMouseButtonDown(0))
                NextSlide();

            if (Input.GetMouseButtonDown(1))
                PreviousSlide();
        }

        private void NextSlide()
        {
            StopCurrentSlide();

            currentSlideIndex++;
            if (currentSlideIndex >= slides.Length)
                currentSlideIndex = 0;

            RunCurrentSlide();
        }

        private void PreviousSlide()
        {
            StopCurrentSlide();

            currentSlideIndex--;
            if (currentSlideIndex < 0)
                currentSlideIndex = slides.Length - 1;

            RunCurrentSlide();
        }

        private void StopCurrentSlide()
        {
            if (currentSlide)
            {
                DOTween.Rewind(currentSlide);
                DOTween.Kill(currentSlide);
                currentSlide.gameObject.SetActive(false);
            }
        }

        private void RunCurrentSlide()
        {
            currentSlide = slides[currentSlideIndex];
            if (currentSlide)
            {
                currentSlide.gameObject.SetActive(true);
                slideActions[currentSlideIndex]();
            }
        }
    }
}
