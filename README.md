# CharTweener - Animate characters in text

![Example](https://github.com/mdechatech/CharTweener/blob/master/Content/example_simple.gif)

```c#
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
```
Use DOTween tweens on TextMeshPro characters the same way you would on a game object:
```c#
transform.DOMove(Vector3.zero, 1) // Move the transform to (0,0,0) over 1 second
// becomes...
tweener.DOMove(2, Vector3.zero, 1); // Move the 3rd character in the text mesh to (0,0,0) over 1 second
```
You can also set properties on characters:
```c#
transform.localPosition = new Vector3(0,1,0) // Move the transform to (0,1,0) locally
// becomes...
tweener.SetOffsetPosition(2, new Vector3(0,1,0)); // Move the 3rd character in the text mesh to (0,1,0) above its original position
```

# Features
CharTween extends existing features for use on text:
- Use `Transform` tweens on characters `(DOMove, DORotate, DOLookAt)`
- Use `Transform` properties and methods on characters `(Get/SetPosition, Get/SetRotation, LookAt)`
- Use `Color` and `VertexGradient` tweens on characters `(DOColor, DOFade, DOGradient)`
- Use `Color` and `VertexGradient` properties on characters `(Get/SetColor, Get/SetAlpha, Get/SetColorGradient)`
- Use `Tween` properties and methods on character tweens `(Pause, OnComplete, timeScale)`

CharTween also provides some extras:
- Use extra tweens that aren't in DOTween `(DOMoveCircle, DODriftPosition/Rotation)`
- Edit character position as offset from start position `(DOOffsetMove, GetStartPosition, Get/SetOffsetPosition)`
- Tween "ahead of time". For example, run tweens on a `TMP_InputField` for characters that haven't been typed yet, and they will show up animated
    - Does not work with `DOMove` or `DOLocalMove`. To tween position ahead of time, use `DOOffsetMove`. `DoShakePosition` and `DOPunchPosition` also work

## Limitations
- **Cannot tween a text mesh before its `Awake()` has been called, must wait until `Start()` or later**
- Material effects such as Outline, Glow, and Underlay cannot be changed per-character
- Text mesh effects such as Underline, Strikethrough cannot be changed per-character
- Performance overhead, creates one `Transform` per modified character.

# Requirements
- Unity 2018.1.0f2 or newer
- TextMesh Pro 1.3.0 (in Unity 2018.1.0f2 package manager) or newer
- DOTween 1.1.695 (February 02, 2018) or newer
    - **DOTween 1.2.320 (January 07, 2020) and older require small changes, see below**

# Installation
1. Have [DOTween](http://dotween.demigiant.com/download.php) and TextMeshPro in your project
2. Get the .unitypackage from the [latest release](https://github.com/mdechatech/CharTweener/releases)
3. Open and import, exclude the Examples folder if needed
4. See fix for old DOTween version if needed

## Fix for old DOTween version
Applies to DOTween 1.2.320 (January 07, 2020) and older
```c#
/*** VertexGradientPlugin.cs, lines 30-34 ***/

        // COMMENT this method if DOTween version is 1.2.320 or older
        public override void SetFrom(TweenerCore<VertexGradient, VertexGradient, NoOptions> t, VertexGradient fromValue, bool setImmediately, bool isRelative) { SetFrom(t, isRelative); }

        // UNCOMMENT this method if DOTween version is 1.2.320 to 1.2.235
        //public override void SetFrom(TweenerCore<VertexGradient, VertexGradient, NoOptions> t, VertexGradient fromValue, bool setImmediately) { SetFrom(t, false); }
```

## License
MIT
