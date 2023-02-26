using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;

public class BehavAdd : MonoBehaviour {

	public static void AddBehaviour(Dictionary<string, List<KeyValuePair<string, string>>> behaviour, GameObject go)
    {
        foreach (var df in behaviour.Keys)
        {
            var type = Type.GetType(df);

            if (go.GetComponent(type) == null)
                go.AddComponent(type);
            
            //parse params
            foreach (var ds in behaviour[df])
            {
                //ds.key is parameter
                //ds.value is value
                var t1 = ReflAll.GetTp(go.GetComponent(df), ds.Key);
                //Debug.Log(t1);
                Type f1 = typeof(System.Single);
                Type f2 = typeof(System.Int32);
                Type f3 = typeof(UnityEngine.Vector3);
                Type f4 = typeof(System.Boolean);
                Type f5 = typeof(System.String);



                if (t1.Equals(f2))
                {
                    ReflAll.SetField(go.GetComponent(df), ds.Key, int.Parse(ds.Value));
                }
                else if (t1.Equals(f1))
                {
                    ReflAll.SetField(go.GetComponent(df), ds.Key, float.Parse(ds.Value));
                }
                else if (t1.Equals(f3))
                {
                    var dt = ds.Value.Split(',');
                    ReflAll.SetField(go.GetComponent(df), ds.Key, new Vector3(float.Parse(dt[0]), float.Parse(dt[1]), float.Parse(dt[2])));
                }
                else if (t1.Equals(f4))
                {
                    ReflAll.SetField(go.GetComponent(df), ds.Key, bool.Parse(ds.Value));
                }
                else if (t1.Equals(f5))
                {
                    ReflAll.SetField(go.GetComponent(df), ds.Key, ds.Value);
                }



            }
        }
    }
}
