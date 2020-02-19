# CharTweener
Got [DOTween](http://dotween.demigiant.com/index.php)? Got [TextMeshPro](https://assetstore.unity.com/packages/essentials/beta-projects/textmesh-pro-84126)?

Using CharTweener, you can do stuff like this...
![Example](https://github.com/mdechatech/CharTweener/blob/master/Content/example_simple.gif)

Using code like this! (Full script [here](https://github.com/mdechatech/CharTweener/blob/master/Assets/CharTween/Examples/CharTweenExampleSimple.cs))
```c#
private void Start()
{
    // Set text
    var text = GetComponent<TMP_Text>();
    text.text = "DETERMINATION";

    var tweener = text.GetCharTweener();    
    for (var i = 0; i < tweener.CharacterCount; ++i)
    {
        // Move characters in a circle
        var circleTween = tweener.DOCircle(i, 0.05f, 0.5f)
            .SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Restart);

        // Oscillate character color between yellow and white
        var colorTween = tweener.DOColor(i, Color.yellow, 0.5f)
            .SetLoops(-1, LoopType.Yoyo);

        // Offset animations based on character index in string
        var timeOffset = Mathf.Lerp(0, 1, i / (float)(tweener.CharacterCount - 1));
        circleTween.fullPosition = timeOffset;
        colorTween.fullPosition = timeOffset;
    }
}
```
#### Features
- Tween position, rotation, scale, and color. Includes the special transform extensions like DOPunch, DOShake
- Controlled like normal tweens. Kill, SetDelay, SetLoops, etc. work. Sequences work.
- If you tween a character at an index that doesn't exist, the tween still happens. For example, you could oscillate the first 100 characters of an empty input field and the animation will happen as the user types in characters. 
- Compatibility with TextMeshProUGUI

#### Limitations
- Performance overhead; my laptop dips below 60FPS when tweening 3000 characters.
- Doesn't work with per-material properties such as Outline, Glow, Underlay.
- **Only works once Start is called; you cannot tween characters in the same frame that the TextMeshPro component is enabled for the first time!**

## Installation
- Have [DOTween](http://dotween.demigiant.com/index.php) and [TextMeshPro](https://assetstore.unity.com/packages/essentials/beta-projects/textmesh-pro-84126) installed in your project.
- Get the .unitypackage from the [latest release](https://github.com/mdechatech/CharTweener/releases).
- Open and import, uncheck the Examples folder if needed.
- Go crazy!

![Example](https://github.com/mdechatech/CharTweener/blob/master/Content/example_full.gif)

[script](https://github.com/mdechatech/CharTweener/blob/master/Assets/CharTween/Examples/CharTweenExampleFull.cs)
