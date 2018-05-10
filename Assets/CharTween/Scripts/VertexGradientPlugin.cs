using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Core.Easing;
using DG.Tweening.Core.Enums;
using DG.Tweening.Plugins.Core;
using DG.Tweening.Plugins.Options;
using TMPro;

namespace CharTween
{
    /// <summary>
    /// Handles tweening of <see cref="VertexGradient"/>.
    /// </summary>
    public class VertexGradientPlugin : ABSTweenPlugin<VertexGradient, VertexGradient, NoOptions>
    {
        public override void Reset(TweenerCore<VertexGradient, VertexGradient, NoOptions> t)
        {

        }

        public override void SetFrom(TweenerCore<VertexGradient, VertexGradient, NoOptions> t, bool isRelative)
        {
            var prevEndVal = t.endValue;
            t.endValue = t.getter();
            t.startValue = isRelative ? Add(t.endValue, prevEndVal) : prevEndVal;
            t.startValue = prevEndVal;
            t.setter(t.startValue);
        }

        public override VertexGradient ConvertToStartValue(TweenerCore<VertexGradient, VertexGradient, NoOptions> t, VertexGradient value)
        {
            return value;
        }

        public override void SetRelativeEndValue(TweenerCore<VertexGradient, VertexGradient, NoOptions> t)
        {
            t.endValue = Subtract(t.startValue, t.changeValue);
        }

        public override void SetChangeValue(TweenerCore<VertexGradient, VertexGradient, NoOptions> t)
        {
            t.changeValue = Subtract(t.endValue, t.startValue);
        }

        public override float GetSpeedBasedDuration(NoOptions options, float unitsXSecond, VertexGradient changeValue)
        {
            return unitsXSecond;
        }

        public override void EvaluateAndApply(NoOptions options, Tween t, bool isRelative, DOGetter<VertexGradient> getter, DOSetter<VertexGradient> setter, float elapsed,
            VertexGradient startValue, VertexGradient changeValue, float duration, bool usingInversePosition,
            UpdateNotice updateNotice)
        {
            var easeVal = EaseManager.Evaluate(t, elapsed, duration, t.easeOvershootOrAmplitude, t.easePeriod);
            setter(Add(startValue, Multiply(changeValue, easeVal)));
        }

        private static VertexGradient Add(VertexGradient left, VertexGradient right)
        {
            return new VertexGradient(
                left.topLeft + right.topLeft,
                left.topRight + right.topRight,
                left.bottomLeft + right.bottomLeft,
                left.bottomRight + right.bottomRight);
        }

        private static VertexGradient Subtract(VertexGradient left, VertexGradient right)
        {
            return new VertexGradient(
                left.topLeft - right.topLeft,
                left.topRight - right.topRight,
                left.bottomLeft - right.bottomLeft,
                left.bottomRight - right.bottomRight);
        }

        private static VertexGradient Multiply(VertexGradient left, float right)
        {
            return new VertexGradient(
                left.topLeft * right,
                left.topRight * right,
                left.bottomLeft * right,
                left.bottomRight * right);
        }
    } // Class
} // Namespace
