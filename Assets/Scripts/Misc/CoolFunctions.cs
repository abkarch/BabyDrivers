using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public static class CoolFunctions
{
    static int circleDetailLevel = 40;
    public static float halfSqrt2 = 0.70710678118f;

    public static void DrawCircle(Vector3 position, float radius, bool rainbow = false)
    {
        for (int i = 0; i < circleDetailLevel; i++)
        {
            float angle = (float)i / circleDetailLevel * 360;
            float angle2 = (float)(i + 1) / circleDetailLevel * 360;
            if (rainbow)
            {
                Gizmos.color = CoolFunctions.HSVToRGB(angle, 1, 1);
            }

            angle *= Mathf.Deg2Rad;
            angle2 *= Mathf.Deg2Rad;
            Gizmos.DrawLine(
                position + new Vector3(Mathf.Sin(angle) * radius, 0, Mathf.Cos(angle) * radius),
                position + new Vector3(Mathf.Sin(angle2) * radius, 0, Mathf.Cos(angle2) * radius));
        }
    }

    public static void DrawCircleXY(Vector3 position, float radius, bool rainbow = false)
    {
        for (int i = 0; i < circleDetailLevel; i++)
        {
            float angle = (float)i / circleDetailLevel * 360;
            float angle2 = (float)(i + 1) / circleDetailLevel * 360;
            if (rainbow)
            {
                Gizmos.color = CoolFunctions.HSVToRGB(angle, 1, 1);
            }

            angle *= Mathf.Deg2Rad;
            angle2 *= Mathf.Deg2Rad;
            Gizmos.DrawLine(
                position + new Vector3(Mathf.Sin(angle) * radius, Mathf.Cos(angle) * radius, 0),
                position + new Vector3(Mathf.Sin(angle2) * radius, Mathf.Cos(angle2) * radius, 0));
        }
    }

    public static void DrawCircleYZ(Vector3 position, float radius, bool rainbow = false)
    {
        for (int i = 0; i < circleDetailLevel; i++)
        {
            float angle = (float)i / circleDetailLevel * 360;
            float angle2 = (float)(i + 1) / circleDetailLevel * 360;
            if (rainbow)
            {
                Gizmos.color = CoolFunctions.HSVToRGB(angle, 1, 1);
            }

            angle *= Mathf.Deg2Rad;
            angle2 *= Mathf.Deg2Rad;
            Gizmos.DrawLine(
                position + new Vector3(0, Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius),
                position + new Vector3(0, Mathf.Cos(angle2) * radius, Mathf.Sin(angle2) * radius));
        }
    }    

    public static float XZDist(Vector3 v1, Vector3 v2)
    {
        v1.y = 0;
        v2.y = 0;
        return Vector3.Distance(v1, v2);
    }

    public static float XZSquareDist(Vector3 v1, Vector3 v2)
    {
        v1.y = 0;
        v2.y = 0;
        return Vector3.SqrMagnitude(v1 - v2);
    }

    public static Vector3 Clamp(Vector3 target, Vector3 lower, Vector3 upper)
    {
        target.x = Mathf.Clamp(target.x, lower.x, upper.x);
        target.y = Mathf.Clamp(target.y, lower.y, upper.y);
        target.z = Mathf.Clamp(target.z, lower.z, upper.z);

        return target;
    }

    public static float SquaredDistance(Vector2 v1, Vector2 v2)
    {
        Vector2 ret;
        ret = v1 - v2;
        return ret.sqrMagnitude;
    }

    public static float SquaredDistance(Vector3 v1, Vector3 v2)
    {
        Vector3 ret;
        ret = v1 - v2;
        return ret.sqrMagnitude;
    }

    public static Vector3 GetLookEulerAngles(Transform thisTrans, Transform target, float lerp, bool ignoreXZ = true)
    {
        return GetLookEulerAngles(thisTrans, target.position, lerp, ignoreXZ);
    }

    public static Vector3 GetLookEulerAngles(Transform thisTrans, Vector3 target, float lerp, bool ignoreXZ = true)
    {
        if (thisTrans.position == target)
            return thisTrans.eulerAngles;
        Quaternion lookAtThatMan = Quaternion.LookRotation(target - thisTrans.position);
        Quaternion rot = Quaternion.Lerp(thisTrans.rotation, lookAtThatMan, lerp);

        if (ignoreXZ)
            return new Vector3(0, rot.eulerAngles.y, 0);
        else
            return rot.eulerAngles;
    }

    public static Vector3 GetLookEulerAngles(Transform thisTrans, Vector3 target, bool ignoreXZ = true)
    {
        if (thisTrans.position == target)
            return thisTrans.eulerAngles;
        Quaternion rot = Quaternion.LookRotation(target - thisTrans.position);

        if (ignoreXZ)
            return new Vector3(0, rot.eulerAngles.y, 0);
        else
            return rot.eulerAngles;
    }

    public static Vector3 GetLookEulerAngles(Vector3 thisPos, Vector3 target, bool ignoreXZ = true)
    {
        Quaternion rot = Quaternion.LookRotation(target - thisPos);

        if (ignoreXZ)
            return new Vector3(0, rot.eulerAngles.y, 0);
        else
            return rot.eulerAngles;
    }

    public static string colOrange = "<color=#d4641d>";
    public static string colLime = "<color=#b7ef0b>";

    //inclusive-exclusive
    public static float WrapValue(this float val, float lowerBound, float upperBound)
    {
        while (val < lowerBound || val >= upperBound)
        {
            if (val < lowerBound)
                val += upperBound;
            else if (val >= upperBound)
                val -= upperBound;
        }

        return val;
    }

    public static void LookAtYOnly(this Transform val, Transform targetTrans)
    {
        LookAtYOnly(val, targetTrans.position);
    }

    public static void LookAtYOnly(this Transform val, Vector3 targetPos)
    {
        val.eulerAngles = GetLookEulerAngles(val, targetPos, 1, true);
    }    

    public static bool Chance(int first, int second)
    {
        float f = first;
        float s = second;

        if (Random.Range(0f, 1f) < f / s)
            return true;

        return false;
    }

    public static bool Coinflip
    {
        get
        {
            return Random.Range(0, 2) == 1;
        }
    }

    public static Vector3 LerpAngle(Vector3 from, Vector3 to, float lerpy)
    {
        Vector3 vec = new Vector3();

        vec.x = Mathf.LerpAngle(from.x, to.x, lerpy);
        vec.y = Mathf.LerpAngle(from.y, to.y, lerpy);
        vec.z = Mathf.LerpAngle(from.z, to.z, lerpy);

        return vec;
    }

    public static Color CLerp(Color c1, Color c2, float lerpVal)
    {
        Color retVal = new Color();

        retVal.r = Mathf.Lerp(c1.r, c2.r, lerpVal);
        retVal.g = Mathf.Lerp(c1.g, c2.g, lerpVal);
        retVal.b = Mathf.Lerp(c1.b, c2.b, lerpVal);
        retVal.a = Mathf.Lerp(c1.a, c2.a, lerpVal);

        return retVal;
    }

    public static Color CMoveTowards(Color current, Color towards, float maxDelta)
    {
        if (towards.r > current.r)
        {
            current.r += maxDelta;
            if (current.r > towards.r)
                current.r = towards.r;
        }
        else if (towards.r < current.r)
        {
            current.r -= maxDelta;
            if (current.r < towards.r)
                current.r = towards.r;
        }

        if (towards.g > current.g)
        {
            current.g += maxDelta;
            if (current.g > towards.g)
                current.g = towards.g;
        }
        else if (towards.g < current.g)
        {
            current.g -= maxDelta;
            if (current.g < towards.g)
                current.g = towards.g;
        }

        if (towards.b > current.b)
        {
            current.b += maxDelta;
            if (current.b > towards.b)
                current.b = towards.b;
        }
        else if (towards.b < current.b)
        {
            current.b -= maxDelta;
            if (current.b < towards.b)
                current.b = towards.b;
        }

        if (towards.a > current.a)
        {
            current.a += maxDelta;
            if (current.a > towards.a)
                current.a = towards.a;
        }
        else if (towards.a < current.a)
        {
            current.a -= maxDelta;
            if (current.a < towards.a)
                current.a = towards.a;
        }

        return current;
    }
    /**
	 * Converts an HSV color to an RGB color.
	 * According to the algorithm described at http://en.wikipedia.org/wiki/HSL_and_HSV
	 * 
	 * @author Wikipedia
	 * @return the RGB representation of the color.
	 */
    public static Color HSVToRGB(float h, float s, float v)
    {
        float Min;
        float Chroma;
        float Hdash;
        float X;
        float r = 0, g = 0, b = 0;

        Chroma = s * v;
        Hdash = h / 60.0f;
        X = Chroma * (1.0f - System.Math.Abs((Hdash % 2.0f) - 1.0f));

        if (Hdash < 1.0f)
        {
            r = Chroma;
            g = X;
        }
        else if (Hdash < 2.0f)
        {
            r = X;
            g = Chroma;
        }
        else if (Hdash < 3.0f)
        {
            g = Chroma;
            b = X;
        }
        else if (Hdash < 4.0f)
        {
            g = X;
            b = Chroma;
        }
        else if (Hdash < 5.0f)
        {
            r = X;
            b = Chroma;
        }
        else if (Hdash < 6.0f)
        {
            r = Chroma;
            b = X;
        }

        Min = v - Chroma;

        r += Min;
        g += Min;
        b += Min;

        return new Color(r, g, b);
    }
    public static GameObject Find(this GameObject go, string nameToFind, bool bSearchInChildren)
    {
        if (bSearchInChildren)
        {
            var transform = go.transform;
            var childCount = transform.childCount;
            for (int i = 0; i < childCount; ++i)
            {
                var child = transform.GetChild(i);
                if (child.gameObject.name == nameToFind)
                    return child.gameObject;
                GameObject result = child.gameObject.Find(nameToFind, bSearchInChildren);
                if (result != null) return result;
            }
            return null;
        }
        else
        {
            return GameObject.Find(nameToFind);
        }
    }

    /// <summary>
    /// Finds the rigidbody from something that is hit.
    /// </summary>
    /// <param name="hit">The hit being checked.</param>
    /// <returns>The rigidbody that was hit.</returns>
    public static Rigidbody getRigidbodyFromHit(RaycastHit hit)
    {
        return hit.rigidbody;
    }

}

public static class FunExtensions
{
    public static void SetLayerRecursively(this GameObject inst, int layer, int layerMustMatch = -1)
    {
        if (layerMustMatch != -1)
        {
            if (inst.layer == layerMustMatch)
            {
                inst.layer = layer;
            }
            foreach (Transform child in inst.transform)
                child.gameObject.SetLayerRecursively(layer, layerMustMatch);
        }
        else
        {
            inst.layer = layer;
            foreach (Transform child in inst.transform)
                child.gameObject.SetLayerRecursively(layer);
        }
    }


    /// <summary>compares the squared magnitude of target - second to given float value</summary>
    public static bool AlmostEquals(this Vector3 target, Vector3 second, float sqrMagnitudePrecision)
    {
        return (target - second).sqrMagnitude < sqrMagnitudePrecision;  // TODO: inline vector methods to optimize?
    }

    /// <summary>compares the squared magnitude of target - second to given float value</summary>
    public static bool AlmostEquals(this Vector2 target, Vector2 second, float sqrMagnitudePrecision)
    {
        return (target - second).sqrMagnitude < sqrMagnitudePrecision;  // TODO: inline vector methods to optimize?
    }

    /// <summary>compares the angle between target and second to given float value</summary>
    public static bool AlmostEquals(this Quaternion target, Quaternion second, float maxAngle)
    {
        return Quaternion.Angle(target, second) < maxAngle;
    }

    /// <summary>compares two floats and returns true of their difference is less than floatDiff</summary>
    public static bool AlmostEquals(this float target, float second, float floatDiff)
    {
        return Mathf.Abs(target - second) < floatDiff;
    }

    /// <summary>
    /// Merges all keys from addHash into the target. Adds new keys and updates the values of existing keys in target.
    /// </summary>
    /// <param name="target">The IDictionary to update.</param>
    /// <param name="addHash">The IDictionary containing data to merge into target.</param>
    public static void Merge(this IDictionary target, IDictionary addHash)
    {
        if (addHash == null || target.Equals(addHash))
        {
            return;
        }

        foreach (object key in addHash.Keys)
        {
            target[key] = addHash[key];
        }
    }

    /// <summary>
    /// Merges keys of type string to target Hashtable.
    /// </summary>
    /// <remarks>
    /// Does not remove keys from target (so non-string keys CAN be in target if they were before).
    /// </remarks>
    /// <param name="target">The target IDicitionary passed in plus all string-typed keys from the addHash.</param>
    /// <param name="addHash">A IDictionary that should be merged partly into target to update it.</param>
    public static void MergeStringKeys(this IDictionary target, IDictionary addHash)
    {
        if (addHash == null || target.Equals(addHash))
        {
            return;
        }

        foreach (object key in addHash.Keys)
        {
            // only merge keys of type string
            if (key is string)
            {
                target[key] = addHash[key];
            }
        }
    }

    /// <summary>
    /// This method copies all string-typed keys of the original into a new Hashtable.
    /// </summary>
    /// <remarks>
    /// Does not recurse (!) into hashes that might be values in the root-hash. 
    /// This does not modify the original.
    /// </remarks>
    /// <param name="original">The original IDictonary to get string-typed keys from.</param>
    /// <returns>New Hashtable containing only string-typed keys of the original.</returns>
    public static Hashtable StripToStringKeys(this IDictionary original)
    {
        Hashtable target = new Hashtable();
        foreach (DictionaryEntry pair in original)
        {
            if (pair.Key is string)
            {
                target[pair.Key] = pair.Value;
            }
        }

        return target;
    }

    /// <summary>
    /// This removes all key-value pairs that have a null-reference as value.
    /// Photon properties are removed by setting their value to null.
    /// Changes the original passed IDictionary!
    /// </summary>
    /// <param name="original">The IDictionary to strip of keys with null-values.</param>
    public static void StripKeysWithNullValues(this IDictionary original)
    {
        object[] keys = new object[original.Count];
        original.Keys.CopyTo(keys, 0);

        for (int index = 0; index < keys.Length; index++)
        {
            var key = keys[index];
            if (original[key] == null)
            {
                original.Remove(key);
            }
        }
    }

    /// <summary>
    /// Checks if a particular integer value is in an int-array.
    /// </summary>
    /// <remarks>This might be useful to look up if a particular actorNumber is in the list of players of a room.</remarks>
    /// <param name="target">The array of ints to check.</param>
    /// <param name="nr">The number to lookup in target.</param>
    /// <returns>True if nr was found in target.</returns>
    public static bool Contains(this int[] target, int nr)
    {
        if (target == null)
        {
            return false;
        }

        for (int index = 0; index < target.Length; index++)
        {
            if (target[index] == nr)
            {
                return true;
            }
        }

        return false;
    }

}