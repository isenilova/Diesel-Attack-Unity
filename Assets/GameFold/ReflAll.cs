using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;


public class ReflAll : MonoBehaviour {

    //blah blah


    public static T GetReference<T>(object inObj, string fieldName) where T : class
    {
        return GetField(inObj, fieldName) as T;
    }


    public static T GetValue<T>(object inObj, string fieldName) where T : struct
    {
        return (T)GetField(inObj, fieldName);
    }


    public static void SetField(object inObj, string fieldName, object newValue)
    {
        FieldInfo info = inObj.GetType().GetField(fieldName);
        if (info != null)
            info.SetValue(inObj, newValue);
    }


    public static object GetField(object inObj, string fieldName)
    {
        object ret = null;
        FieldInfo info = inObj.GetType().GetField(fieldName);
        if (info != null)
            ret = info.GetValue(inObj);
        return ret;
    }

    public static Type GetTp(object inObj, string fieldName)
    {
        FieldInfo info = inObj.GetType().GetField(fieldName);

        return info.FieldType;
    }

}
