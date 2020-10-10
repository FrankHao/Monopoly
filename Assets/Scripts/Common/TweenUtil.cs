using DigitalRuby.Tween;
using UnityEngine;

public class TweenUtil : MonoBehaviour
{


    public enum TweenType
    {
        TWEEN_ALPHA,
        TWEEN_POSITION,
        TWEEN_ROTATION,
        TWEEN_SIZE
    }

    [SerializeField]
    Vector3 TargetVector = Vector3.zero;

    [SerializeField]
    TweenType tweenType = TweenType.TWEEN_POSITION;
    [SerializeField]
    float tweenTime = 1f;


    private void OnEnable()
    {
        switch (tweenType)
        {
            case TweenType.TWEEN_POSITION:
                TweenUtil.TweenPosition(this.transform, TargetVector, tweenTime);
                break;
            case TweenType.TWEEN_SIZE:
                TweenUtil.TweenSize(this.transform, TargetVector.x, tweenTime);
                break;
        }

    }

    public static void TweenRotate(Transform transform, float endAngle, float duration = 1f)
    {
        System.Action<ITween<float>> objectRotate = (t) =>
        {
            // start rotation from identity to ensure no stuttering
            transform.rotation = Quaternion.identity;
            transform.Rotate(Camera.main.transform.forward, t.CurrentValue);
        };

        float startAngle = transform.rotation.eulerAngles.z;

        // completion defaults to null if not passed in
        transform.gameObject.Tween("RotateTrans_" + transform.name, startAngle, endAngle, duration, TweenScaleFunctions.CubicEaseInOut, objectRotate);
    }

    public static void TweenSize(Transform transform, float endSize, float duration = 1f)
    {
        System.Action<ITween<float>> objectSize = (t) =>
        {
            // start rotation from identity to ensure no stuttering
            transform.rotation = Quaternion.identity;
            transform.localScale = Vector3.one * t.CurrentValue;
        };

        float startSize = transform.localScale.x;

        // completion defaults to null if not passed in
        transform.gameObject.Tween("ResizeTrans_" + transform.name, startSize, endSize, duration, TweenScaleFunctions.CubicEaseIn, objectSize);
    }

    public static void TweenAlpha(Transform transform, float endAlpha, float duration = 1f)
    {
        SpriteRenderer sprite = transform.GetComponent<SpriteRenderer>();
        if (sprite == null) return;

        System.Action<ITween<float>> objectAlpha = (t) =>
        {
            sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, t.CurrentValue);
        };

        float startAlpha = sprite.color.a;

        // completion defaults to null if not passed in
        transform.gameObject.Tween("ResizeTrans_" + transform.name, startAlpha, endAlpha, duration, TweenScaleFunctions.CubicEaseOut, objectAlpha);
    }

    public static void TweenPosition(Transform transform, Vector3 endPosition, float duration = 1f)
    {
        System.Action<ITween<Vector3>> objectPos = (t) =>
        {
            // start rotation from identity to ensure no stuttering
            transform.rotation = Quaternion.identity;
            transform.localPosition = t.CurrentValue;
        };

        Vector3 startPos = transform.localPosition;

        // completion defaults to null if not passed in
        transform.gameObject.Tween("ResizeTrans_" + transform.name, startPos, endPosition, duration, TweenScaleFunctions.CubicEaseIn, objectPos);
    }
}
