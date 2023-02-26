using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class OneDialog : BaseObj
{

    public string idA;
    public string idB;
    public string idBeg;
    public string phrase;
    public List<TripleS> myPhrases;

    public object Clone()
    {
        return this.MemberwiseClone();
    }

}
