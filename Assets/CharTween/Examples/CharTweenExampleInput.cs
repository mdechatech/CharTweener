using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CharTween.Examples {

    public class CharTweenExampleInput : MonoBehaviour
    {
        private enum State
        {
            InputName,
            ConfirmName,
            Done,
        }

        [Header("Enter Name Screen")]
        public TMP_Text EnterNamePrompt;
        public TMP_Text CharLimitPrompt;
        public TMP_Text PressEnterPrompt;
        public TMP_InputField NameInput;

        [Header("Confirm Name Screen")]
        public GameObject ConfirmNameScreen;
        public TMP_Text ConfirmNamePrompt;
        public TMP_Text ConfirmNameValue;
        public TMP_Text ConfirmNameInputPrompt;

        [Header("Done Screen")]
        public TMP_Text WelcomePrompt;

        private string lastNameValue; // For tweening in typed characters and animating error once length limit is hit
        private State currentState;

        // These are used as ids to group related tweens
        private object driftTweenId = new object();
        private object stateTransitionId = new object();
        private object errorId = new object();

        void Start()
        {
            NameInput.ActivateInputField();
            NameInput.onValueChanged.AddListener(Name_OnValueChanged);
            lastNameValue = NameInput.text;
            UpdateCharLimitPrompt();

            // Enter InputName
            DOTween.Sequence()
                .Join(BounceIn(EnterNamePrompt).OnComplete(() => Drift(EnterNamePrompt)))
                .Join(BounceIn(CharLimitPrompt, 20).SetDelay(0.5f).OnComplete(() => Drift(CharLimitPrompt, 1.5f)))
                .Join(BounceIn(PressEnterPrompt, 20).SetDelay(2).OnComplete(() => Drift(PressEnterPrompt, 1.5f)))
                .Join(BounceIn(NameInput.textComponent, 50, NameInput.characterLimit)
                    .OnComplete(() => Drift(NameInput.textComponent, 2, NameInput.characterLimit)))
                .SetId(stateTransitionId);
            currentState = State.InputName;
        }

        void Update()
        {
            if (currentState == State.InputName)
            {
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    if (string.IsNullOrEmpty(NameInput.text.Trim()))
                    {
                        DOTween.Complete(stateTransitionId, withCallbacks: true);
                        if (!DOTween.IsTweening(errorId))
                            Error(EnterNamePrompt);
                    }
                    else
                    {
                        currentState = State.ConfirmName;

                        DOTween.Complete(stateTransitionId, withCallbacks: true);
                        DOTween.Complete(errorId, withCallbacks: true);
                        DOTween.Kill(driftTweenId);
                        Sequence transition = DOTween.Sequence()
                            .SetId(stateTransitionId);

                        // Exit InputName
                        NameInput.interactable = false;
                        NameInput.DeactivateInputField();
                        transition
                            .Append(ScaleOut(EnterNamePrompt))
                            .Join(ScaleOut(CharLimitPrompt))
                            .Join(ScaleOut(PressEnterPrompt))
                            .Join(ScaleOut(NameInput.textComponent, 100, NameInput.characterLimit));

                        // Enter ConfirmName
                        ConfirmNamePrompt.alpha = 1;
                        ConfirmNameValue.alpha = 1;
                        ConfirmNameInputPrompt.alpha = 1;
                        ConfirmNameValue.text = NameInput.text;
                        transition
                            .Append(BounceIn(ConfirmNamePrompt).OnComplete(() => Drift(ConfirmNamePrompt)))
                            .Join(BounceIn(ConfirmNameValue, 50, NameInput.characterLimit)
                                .SetDelay(0.25f)
                                .OnComplete(() => Drift(ConfirmNameValue, 2, NameInput.characterLimit)))
                            .Join(BounceIn(ConfirmNameInputPrompt).SetDelay(0.25f)
                                .OnComplete(() => Drift(ConfirmNameInputPrompt)));
                    }
                }
                
                NameInput.ActivateInputField();
            }
            else if (currentState == State.ConfirmName)
            {
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    currentState = State.Done;

                    DOTween.Complete(stateTransitionId, withCallbacks: true);
                    DOTween.Kill(driftTweenId);
                    Sequence transition = DOTween.Sequence()
                        .SetId(stateTransitionId);

                    // Exit ConfirmName
                    transition
                        .Append(ScaleOut(ConfirmNamePrompt))
                        .Join(ScaleOut(ConfirmNameValue, 100, NameInput.characterLimit))
                        .Join(ScaleOut(ConfirmNameInputPrompt));

                    // Enter Done
                    WelcomePrompt.alpha = 1;
                    WelcomePrompt.text = string.Format("Welcome, {0}!", NameInput.text);
                    transition
                        .Append(BounceIn(WelcomePrompt).OnComplete(() => Drift(WelcomePrompt)))
                        .AppendInterval(5)
                        .Append(ScaleOut(WelcomePrompt))
                        .AppendCallback(() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex));
                }
                else if (Input.GetKeyDown(KeyCode.Escape))
                {
                    currentState = State.InputName;

                    DOTween.Complete(stateTransitionId, withCallbacks: true);
                    DOTween.Kill(driftTweenId);
                    Sequence transition = DOTween.Sequence()
                        .SetId(stateTransitionId);

                    // Exit ConfirmName
                    transition
                        .Append(ScaleOut(ConfirmNamePrompt))
                        .Join(ScaleOut(ConfirmNameValue, 100, NameInput.characterLimit))
                        .Join(ScaleOut(ConfirmNameInputPrompt));

                    // Enter InputName
                    NameInput.interactable = true;
                    transition
                        .Append(BounceIn(EnterNamePrompt).OnComplete(() => Drift(EnterNamePrompt)))
                        .Join(BounceIn(CharLimitPrompt, 20).SetDelay(0.5f)
                            .OnComplete(() => Drift(CharLimitPrompt, 1.5f)))
                        .Join(BounceIn(PressEnterPrompt, 20).SetDelay(2)
                            .OnComplete(() => Drift(PressEnterPrompt, 1.5f)))
                        .Join(BounceIn(NameInput.textComponent, 50, NameInput.characterLimit)
                            .OnComplete(() => Drift(NameInput.textComponent, 2, NameInput.characterLimit)));
                }
            }
        }

        void OnGUI()
        {
            if (currentState != State.InputName)
                return;

            if (Event.current.type == EventType.KeyDown)
            {
                Event evt = Event.current;
                if (evt.keyCode == KeyCode.None || evt.keyCode == KeyCode.Backspace || evt.keyCode == KeyCode.Return)
                    return;

                if (NameInput.text.Length == NameInput.characterLimit)
                {
                    if (DOTween.IsTweening(errorId))
                        return;

                    DOTween.Complete(stateTransitionId, withCallbacks: true);
                    Error(NameInput.textComponent);
                    Error(CharLimitPrompt);
                }
            }
        }

        private void Name_OnValueChanged(string value)
        {
            if (value.Length > lastNameValue.Length)
            {
                CharTweener tweener = NameInput.textComponent.GetCharTweener();
                for (int i = lastNameValue.Length; i < value.Length; i++)
                {
                    tweener.SetLocalScale(i, Vector3.one);
                    NameInput.textComponent.GetCharTweener().DOPunchScale(i, Vector3.up * 0.25f, 0.5f);
                }
            }

            lastNameValue = value;
            UpdateCharLimitPrompt();
        }

        private void UpdateCharLimitPrompt()
        {
            CharLimitPrompt.text = string.Format("{0:00}/{1:00}", NameInput.text.Length, NameInput.characterLimit);
        }

        private Sequence BounceIn(TMP_Text text, float distance = 50, int? charsAhead = null)
        {
            Sequence textSequence = DOTween.Sequence();
            CharTweener tweener = text.GetCharTweener();

            int count = charsAhead ?? tweener.CharacterCount;
            for (int i = 0; i < count; i++)
            {
                tweener.SetAlpha(i, 0);
                tweener.SetLocalEulerAngles(i, Vector3.forward * 45);
                tweener.SetLocalScale(i, 1);
                tweener.ResetPosition(i);
                tweener.UpdateCharProperties();
            }

            for (int i = 0; i < count; i++)
            {
                Sequence charSequence = DOTween.Sequence();
                charSequence.Insert(0, tweener.DOFade(i, 1, 1));
                charSequence.Insert(0, tweener.DOOffsetMoveY(i, distance, 0.25f).SetEase(Ease.OutCubic));
                charSequence.Insert(0.25f, tweener.DOOffsetMoveY(i, 0, 0.75f).SetEase(Ease.OutBounce));
                charSequence.Insert(0.25f, tweener.DOLocalRotate(i, Vector3.zero, 0.75f).SetEase(Ease.OutBounce));
                textSequence.Insert((float) i / count, charSequence);
            }

            textSequence.SetTarget(text);
            return textSequence;
        }

        private Sequence ScaleOut(TMP_Text text, float distance = 100, int? charsAhead = null)
        {
            Sequence textSequence = DOTween.Sequence();
            CharTweener tweener = text.GetCharTweener();

            int count = charsAhead ?? tweener.CharacterCount;
            for (int i = 0; i < count; i++)
            {
                Sequence charSequence = DOTween.Sequence();
                charSequence.Insert(0, tweener.DOFade(i, 0, 0.5f).SetEase(Ease.InCubic));
                charSequence.Insert(0, tweener.DOScale(i, 0, 0.5f).SetEase(Ease.InBack));
                textSequence.Insert((float)i / count*0.5f, charSequence);
            }

            textSequence.SetTarget(text);
            return textSequence;
        }

        private Sequence Drift(TMP_Text text, float strength = 2, int? charsAhead = null)
        {
            Sequence textSequence = DOTween.Sequence();
            CharTweener tweener = text.GetCharTweener();

            int count = charsAhead ?? tweener.CharacterCount;
            for (int i = 0; i < count; i++)
            {
                textSequence.Insert(0, tweener.DODriftPosition(i, Vector3.one * strength, Vector3.one * 0.5f, 5));
            }

            textSequence.SetId(driftTweenId);
            return textSequence;
        }

        private void Error(TMP_Text text, int? charsAhead = null)
        {
            Sequence textSequence = DOTween.Sequence();
            CharTweener tweener = text.GetCharTweener();

            DOTween.Kill(tweener, complete: true);
            int count = charsAhead ?? tweener.CharacterCount;
            for (int i = 0; i < count; i++)
            {
                Sequence charSequence = DOTween.Sequence();
                Color oldColor = tweener.GetColor(i);
                tweener.SetColor(i, Color.red);
                charSequence.Insert(0, tweener.DOColor(i, oldColor, 0.5f).SetTarget(tweener));
                charSequence.Insert(0, tweener.DOShakeRotation(i, 0.5f, Vector3.forward * 25, 20).SetTarget(tweener));
                textSequence.Insert((float) i / count * 0.25f, charSequence);
            }

            textSequence.SetId(errorId);
        }
    }
}
