using TMPro;
using UnityEngine;

namespace CharTween
{
    // Contains getters and setters for character properties. Characters can be modified using the same methods as are in
    // Unity's Transform.
    public partial class CharTweener : MonoBehaviour
    {
        public float GetAlpha(int charIndex)
        {
            ProxyColor proxyColor = TryGetProxyColor(charIndex);
            return proxyColor != null ? proxyColor.Alpha : Text.alpha;
        }

        public void SetAlpha(int charIndex, float value)
        {
            GetProxyColor(charIndex).SetAlpha(value);
            colorsChangedByProperties = true;
        }

        public Color GetColor(int charIndex)
        {
            ProxyColor proxyColor = TryGetProxyColor(charIndex);
            return proxyColor != null ? proxyColor.Color : Text.color;
        }

        public void SetColor(int charIndex, Color value)
        {
            GetProxyColor(charIndex).SetColor(value);
            colorsChangedByProperties = true;
        }

        public VertexGradient GetColorGradient(int charIndex)
        {
            ProxyColor proxyColor = TryGetProxyColor(charIndex);
            return proxyColor != null ? proxyColor.ColorGradient : Text.colorGradient;
        }

        public void SetColorGradient(int charIndex, VertexGradient value)
        {
            GetProxyColor(charIndex).SetColorGradient(value);
            colorsChangedByProperties = true;
        }

        /// <summary>
        /// Returns the character's original position relative to the text mesh's position
        /// </summary>
        public Vector3 GetLocalStartPosition(int charIndex)
        {
            if (charIndex >= characterCount)
                return Vector3.zero;

            ProxyTransform proxy;
            if (proxyTransformDict != null && proxyTransformDict.TryGetValue(charIndex, out proxy))
                return proxy.LocalStartPosition;

            TMP_CharacterInfo charInfo = Text.textInfo.characterInfo[charIndex];
            return new Vector3(
                (charInfo.topLeft.x + charInfo.bottomRight.x) * 0.5f,
                (charInfo.topLeft.y + charInfo.bottomRight.y) * 0.5f,
                (charInfo.topLeft.z + charInfo.bottomRight.z) * 0.5f);
        }

        /// <summary>
        /// Returns the character's original position in world space
        /// </summary>
        public Vector3 GetStartPosition(int charIndex)
        {
            return proxyTransformParent.TransformPoint(GetLocalStartPosition(charIndex));
        }

        /// <summary>
        /// Returns the character's current position relative to its original position
        /// </summary>
        public Vector3 GetOffsetPosition(int charIndex)
        {
            ProxyTransform proxyTransform = TryGetProxyTransform(charIndex);
            return proxyTransform != null ? proxyTransform.OffsetPosition : Vector3.zero;
        }

        /// <summary>
        /// Sets the character's position relative to its original position
        /// </summary>
        public void SetOffsetPosition(int charIndex, Vector3 value)
        {
            GetProxyTransform(charIndex).OffsetPosition = value;
            transformsChangedByProperties = true;
        }

        /// <summary>
        /// Returns the character to its original position
        /// </summary>
        public void ResetPosition(int charIndex)
        {
            GetProxyTransform(charIndex).OffsetPosition = Vector3.zero;
            transformsChangedByProperties = true;
        }

        /// <summary>
        /// Returns the character's position relative to the text mesh's position
        /// </summary>
        public Vector3 GetLocalPosition(int charIndex)
        {
            // We must create the proxy here to calculate the value
            return GetProxyTransform(charIndex).LocalPosition;
        }

        /// <summary>
        /// Sets the character's position relative to the text mesh's position
        /// </summary>
        public void SetLocalPosition(int charIndex, Vector3 localPosition)
        {
            GetProxyTransform(charIndex).LocalPosition = localPosition;
            transformsChangedByProperties = true;
        }

        /// <summary>
        /// Returns the character's position in world space
        /// </summary>
        public Vector3 GetPosition(int charIndex)
        {
            // We must create the proxy here to calculate the value
            return GetProxyTransform(charIndex).Position;
        }

        /// <summary>
        /// Sets the character's position in world space
        /// </summary>
        public void SetPosition(int charIndex, Vector3 position)
        {
            GetProxyTransform(charIndex).Position = position;
            transformsChangedByProperties = true;
        }

        public Vector3 GetLocalEulerAngles(int charIndex)
        {
            ProxyTransform proxyTransform = TryGetProxyTransform(charIndex);
            return proxyTransform != null ? proxyTransform.Target.localEulerAngles : Vector3.zero;
        }

        public void SetLocalEulerAngles(int charIndex, Vector3 localEulerAngles)
        {
            GetProxyTransform(charIndex).Target.localEulerAngles = localEulerAngles;
            transformsChangedByProperties = true;
        }

        public Vector3 GetEulerAngles(int charIndex)
        {
            ProxyTransform proxyTransform = TryGetProxyTransform(charIndex);
            return proxyTransform != null ? proxyTransform.Target.eulerAngles : proxyTransformParent.eulerAngles;
        }

        public void SetEulerAngles(int charIndex, Vector3 eulerAngles)
        {
            GetProxyTransform(charIndex).Target.eulerAngles = eulerAngles;
            transformsChangedByProperties = true;
        }

        public Quaternion GetLocalRotation(int charIndex)
        {
            ProxyTransform proxyTransform = TryGetProxyTransform(charIndex);
            return proxyTransform != null ? proxyTransform.Target.localRotation : Quaternion.identity;
        }

        public void SetLocalRotation(int charIndex, Quaternion localRotation)
        {
            GetProxyTransform(charIndex).Target.localRotation = localRotation;
            transformsChangedByProperties = true;
        }

        public Quaternion GetRotation(int charIndex)
        {
            ProxyTransform proxyTransform = TryGetProxyTransform(charIndex);
            return proxyTransform != null ? proxyTransform.Target.rotation : proxyTransformParent.rotation;
        }

        public void SetRotation(int charIndex, Quaternion rotation)
        {
            GetProxyTransform(charIndex).Target.rotation = rotation;
            transformsChangedByProperties = true;
        }

        public void ResetRotation(int charIndex)
        {
            // Setting localRotation because we want to keep the textmesh's rotation
            GetProxyTransform(charIndex).Target.localRotation = Quaternion.identity;
            transformsChangedByProperties = true;
        }

        public Vector3 GetLocalScale(int charIndex)
        {
            ProxyTransform proxyTransform = TryGetProxyTransform(charIndex);
            return proxyTransform != null ? GetProxyTransform(charIndex).Target.localScale : Vector3.one;
        }

        public void SetLocalScale(int charIndex, float localScale)
        {
            GetProxyTransform(charIndex).Target.localScale = new Vector3(localScale, localScale, localScale);
            transformsChangedByProperties = true;
        }

        public void SetLocalScale(int charIndex, Vector3 localScale)
        {
            GetProxyTransform(charIndex).Target.localScale = localScale;
            transformsChangedByProperties = true;
        }

        public void ResetScale(int charIndex)
        {
            GetProxyTransform(charIndex).Target.localScale = Vector3.one;
            transformsChangedByProperties = true;
        }

        public void LookAt(int charIndex, Transform target)
        {
            GetProxyTransform(charIndex).Target.LookAt(target);
            transformsChangedByProperties = true;
        }

        public void LookAt(int charIndex, Transform target, Vector3 worldUp)
        {
            GetProxyTransform(charIndex).Target.LookAt(target, worldUp);
            transformsChangedByProperties = true;
        }

        public void LookAt(int charIndex, Vector3 worldPosition)
        {
            GetProxyTransform(charIndex).Target.LookAt(worldPosition);
            transformsChangedByProperties = true;
        }

        public void LookAt(int charIndex, Vector3 worldPosition, Vector3 worldUp)
        {
            GetProxyTransform(charIndex).Target.LookAt(worldPosition, worldUp);
            transformsChangedByProperties = true;
        }

        public void Rotate(int charIndex, Vector3 eulerAngles)
        {
            GetProxyTransform(charIndex).Target.Rotate(eulerAngles);
            transformsChangedByProperties = true;
        }

        public void Rotate(int charIndex, Vector3 eulerAngles, Space relativeTo)
        {
            GetProxyTransform(charIndex).Target.Rotate(eulerAngles, relativeTo);
            transformsChangedByProperties = true;
        }

        public void Rotate(int charIndex, Vector3 axis, float angle)
        {
            GetProxyTransform(charIndex).Target.Rotate(axis, angle);
            transformsChangedByProperties = true;
        }

        public void Rotate(int charIndex, Vector3 axis, float angle, Space relativeTo)
        {
            GetProxyTransform(charIndex).Target.Rotate(axis, angle, relativeTo);
            transformsChangedByProperties = true;
        }

        public void Rotate(int charIndex, float xAngle, float yAngle, float zAngle)
        {
            GetProxyTransform(charIndex).Target.Rotate(xAngle, yAngle, zAngle);
            transformsChangedByProperties = true;
        }

        public void Rotate(int charIndex, float xAngle, float yAngle, float zAngle, Space relativeTo)
        {
            GetProxyTransform(charIndex).Target.Rotate(xAngle, yAngle, zAngle, relativeTo);
            transformsChangedByProperties = true;
        }

        public void RotateAround(int charIndex, Vector3 point, Vector3 axis, float angle)
        {
            GetProxyTransform(charIndex).Target.RotateAround(point, axis, angle);
            transformsChangedByProperties = true;
        }

        public void Translate(int charIndex, Vector3 translation)
        {
            GetProxyTransform(charIndex).Target.Translate(translation);
            transformsChangedByProperties = true;
        }

        public void Translate(int charIndex, Vector3 translation, Space relativeTo)
        {
            GetProxyTransform(charIndex).Target.Translate(translation, relativeTo);
            transformsChangedByProperties = true;
        }

        public void Translate(int charIndex, Vector3 translation, Transform relativeTo)
        {
            GetProxyTransform(charIndex).Target.Translate(translation, relativeTo);
            transformsChangedByProperties = true;
        }

        public void Translate(int charIndex, float x, float y, float z)
        {
            GetProxyTransform(charIndex).Target.Translate(x, y, z);
            transformsChangedByProperties = true;
        }

        public void Translate(int charIndex, float x, float y, float z, Space relativeTo)
        {
            GetProxyTransform(charIndex).Target.Translate(x, y, z, relativeTo);
            transformsChangedByProperties = true;
        }

        // There is no SetLossyScale because it's not available in Transform
        public Vector3 GetLossyScale(int charIndex)
        {
            ProxyTransform proxyTransform = TryGetProxyTransform(charIndex);
            return proxyTransform != null ? proxyTransform.Target.lossyScale : proxyTransformParent.lossyScale;
        }

        public Matrix4x4 GetLocalToWorldMatrix(int charIndex)
        {
            ProxyTransform proxyTransform = TryGetProxyTransform(charIndex);
            return proxyTransform != null ? proxyTransform.Target.localToWorldMatrix : proxyTransformParent.localToWorldMatrix;
        }

        public Matrix4x4 GetWorldToLocalMatrix(int charIndex)
        {
            ProxyTransform proxyTransform = TryGetProxyTransform(charIndex);
            return proxyTransform != null ? proxyTransform.Target.worldToLocalMatrix : proxyTransformParent.worldToLocalMatrix;
        }

        public Vector3 GetUpVector(int charIndex)
        {
            ProxyTransform proxyTransform = TryGetProxyTransform(charIndex);
            return proxyTransform != null ? proxyTransform.Target.up : proxyTransformParent.up;
        }

        public Vector3 GetForwardVector(int charIndex)
        {
            ProxyTransform proxyTransform = TryGetProxyTransform(charIndex);
            return proxyTransform != null ? proxyTransform.Target.forward : proxyTransformParent.forward;
        }

        public Vector3 GetRightVector(int charIndex)
        {
            ProxyTransform proxyTransform = TryGetProxyTransform(charIndex);
            return proxyTransform != null ? proxyTransform.Target.right : proxyTransformParent.right;
        }

        public Vector3 InverseTransformDirection(int charIndex, Vector3 direction)
        {
            ProxyTransform proxyTransform = TryGetProxyTransform(charIndex);
            return proxyTransform != null
                ? proxyTransform.Target.InverseTransformDirection(direction)
                : proxyTransformParent.InverseTransformDirection(direction);
        }

        public Vector3 InverseTransformDirection(int charIndex, float x, float y, float z)
        {
            ProxyTransform proxyTransform = TryGetProxyTransform(charIndex);
            return proxyTransform != null
                ? proxyTransform.Target.InverseTransformDirection(x, y, z)
                : proxyTransformParent.InverseTransformDirection(x, y, z);
        }

        public Vector3 InverseTransformPoint(int charIndex, Vector3 Point)
        {
            ProxyTransform proxyTransform = TryGetProxyTransform(charIndex);
            return proxyTransform != null
                ? proxyTransform.Target.InverseTransformPoint(Point)
                : proxyTransformParent.InverseTransformPoint(Point);
        }

        public Vector3 InverseTransformPoint(int charIndex, float x, float y, float z)
        {
            ProxyTransform proxyTransform = TryGetProxyTransform(charIndex);
            return proxyTransform != null
                ? proxyTransform.Target.InverseTransformPoint(x, y, z)
                : proxyTransformParent.InverseTransformPoint(x, y, z);
        }

        public Vector3 InverseTransformVector(int charIndex, Vector3 Vector)
        {
            ProxyTransform proxyTransform = TryGetProxyTransform(charIndex);
            return proxyTransform != null
                ? proxyTransform.Target.InverseTransformVector(Vector)
                : proxyTransformParent.InverseTransformVector(Vector);
        }

        public Vector3 InverseTransformVector(int charIndex, float x, float y, float z)
        {
            ProxyTransform proxyTransform = TryGetProxyTransform(charIndex);
            return proxyTransform != null
                ? proxyTransform.Target.InverseTransformVector(x, y, z)
                : proxyTransformParent.InverseTransformVector(x, y, z);
        }

        public Vector3 TransformDirection(int charIndex, Vector3 direction)
        {
            ProxyTransform proxyTransform = TryGetProxyTransform(charIndex);
            return proxyTransform != null
                ? proxyTransform.Target.TransformDirection(direction)
                : proxyTransformParent.TransformDirection(direction);
        }

        public Vector3 TransformDirection(int charIndex, float x, float y, float z)
        {
            ProxyTransform proxyTransform = TryGetProxyTransform(charIndex);
            return proxyTransform != null
                ? proxyTransform.Target.TransformDirection(x, y, z)
                : proxyTransformParent.TransformDirection(x, y, z);
        }

        public Vector3 TransformPoint(int charIndex, Vector3 Point)
        {
            ProxyTransform proxyTransform = TryGetProxyTransform(charIndex);
            return proxyTransform != null
                ? proxyTransform.Target.TransformPoint(Point)
                : proxyTransformParent.TransformPoint(Point);
        }

        public Vector3 TransformPoint(int charIndex, float x, float y, float z)
        {
            ProxyTransform proxyTransform = TryGetProxyTransform(charIndex);
            return proxyTransform != null
                ? proxyTransform.Target.TransformPoint(x, y, z)
                : proxyTransformParent.TransformPoint(x, y, z);
        }

        public Vector3 TransformVector(int charIndex, Vector3 Vector)
        {
            ProxyTransform proxyTransform = TryGetProxyTransform(charIndex);
            return proxyTransform != null
                ? proxyTransform.Target.TransformVector(Vector)
                : proxyTransformParent.TransformVector(Vector);
        }

        public Vector3 TransformVector(int charIndex, float x, float y, float z)
        {
            ProxyTransform proxyTransform = TryGetProxyTransform(charIndex);
            return proxyTransform != null
                ? proxyTransform.Target.TransformVector(x, y, z)
                : proxyTransformParent.TransformVector(x, y, z);
        }
    }
}
