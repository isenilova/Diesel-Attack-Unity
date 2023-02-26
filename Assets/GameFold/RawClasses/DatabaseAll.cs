using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;

public class DatabaseAll : MonoBehaviour {

    public Dictionary<string, Dictionary<string, BaseObj>> data;
    public Dual[] files;
    public string className;

    public static DatabaseAll instance;

    int loadMax = 0;
    int load = 0;
    public bool isLoaded = false;

    private void Awake()
    {
        instance = this;
    }

    public BaseObj GetObj(string className, string id)
    {
        return data[className][id];
    }

    public List<BaseObj> GetListObj(string className)
    {
        List<BaseObj> bl = new List<BaseObj>();
        foreach (var d in data[className].Keys)
        {
            bl.Add(data[className][d]);
        }

        return bl;
    }

    [ContextMenu("GetSample")]
    public void GetSample()
    {
        //var t = System.Activator.CreateInstance(Type.GetType(className));

        var t = new OneDialog();
        t.id = "begd0";
        t.idA = "troll1";
        t.idB = "troll2";
        t.phrase = "Hi ! nice to meet you in our kingdom !";
        t.myPhrases = new List<TripleS>();
        TripleS sf = new TripleS();
        sf.phrase = "Glad to hear it ! Who are you ?";
        sf.leadID = "begd1";
        sf.action = "c";
        t.myPhrases.Add(sf);
        TripleS sf1 = new TripleS();
        sf1.phrase = "Ok, thats enough";
        sf1.leadID = "begd1";
        sf1.action = "end";
        t.myPhrases.Add(sf1);


             
        /*
        var t = new ExpCurve();
        //t.damage = new Damage();
        t.expVals = new Dictionary<int, int>();
        t.expVals.Add(1, 100);
        t.expVals.Add(2, 200);
        */

        string f = JsonSerializer.Serialize(t, false);
        

        Debug.Log(f);
    }

    [ContextMenu("PrintAll")]
    public void PrintAll()
    {
        var ds = data["Skill"]["1"];
        Debug.Log(((Skill)ds).skillname);
        /*
        foreach (var sd in data)
        {
            Debug.Log(((Skill)sd).skillname);
        }
        */
    }

    IEnumerator GetMe(int j)
    {
        loadMax++;

        var t = System.Activator.CreateInstance(Type.GetType(files[j].className));

        data.Add(files[j].className, new Dictionary<string, BaseObj>());

        var www = new WWW(files[j].url);

        yield return www;

        var u = t.GetType();

        Debug.Log(files[j].url);
        //Debug.Log(www.text);

        var tt = Type.GetType(files[j].className);
        var tf = JsonConvert.DeserializeObject<JObject>(www.text);

        JArray items = (JArray)tf["main"];
        int length = items.Count;
        Debug.Log(length);

        for (int i = 0; i < items.Count; i++)
        {
            var f = JsonConvert.DeserializeObject(tf["main"][i].ToString(), tt);
            data[files[j].className].Add(((BaseObj)f).id, (BaseObj)f);
        }

        load++;
        Debug.Log("Done " + files[j].url);
    }

    public void Reload()
    {
        load = 0;
        isLoaded = false;
        data.Clear();
        Start();
    }

    public void Start()
    {
        // var a = new Monster();
        data = new Dictionary<string, Dictionary<string, BaseObj>>();

        //var t = System.Activator.CreateInstance(Type.GetType(className));

        for (int j = 0; j < files.Length; j++)
        {
            if (files[j].useUrl)
            {
                StartCoroutine(GetMe(j));
                continue;
            }


            var tt = Type.GetType(files[j].className);


            JObject tf;

            if (!files[j].useFile)
            {
                tf = JsonConvert.DeserializeObject<JObject>(files[j].fl.text);
            }
            else
            {
                StreamReader sr = new StreamReader("Config/" + files[j].fl.name + ".txt");
                var txt = sr.ReadToEnd();
                tf = JsonConvert.DeserializeObject<JObject>(txt);
            }

            data.Add(files[j].className, new Dictionary<string, BaseObj>());

            JArray items = (JArray)tf["main"];
            int length = items.Count;
            Debug.Log(files[j].className);
            Debug.Log(files[j].fl.name);
            Debug.Log(length);

            for (int i = 0; i < items.Count; i++)
            {
                var f = JsonConvert.DeserializeObject(tf["main"][i].ToString(), tt);
                data[files[j].className].Add(((BaseObj)f).id, (BaseObj)f);
            }

        }

    }

    private void Update()
    {
        if ((load == loadMax) && (!isLoaded))
        {
            isLoaded = true;
        }
    }

}
