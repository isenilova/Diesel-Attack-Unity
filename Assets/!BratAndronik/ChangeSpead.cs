using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSpead : MonoBehaviour
{
    public float newSp = 0f;

    private MoveControl moveScr;
    public AnimationCurve myCoef;

    public float addSpeadOf = 2f;


    private float startSpeed;
    
    // Start is called before the first frame update
    void Start()
    {
        moveScr = gameObject.GetComponent<MoveControl>();

        startSpeed = moveScr.addSpeed;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator mySp()
    {
        while ( moveScr.addSpeed >= newSp)
        {
            float t = Mathf.Abs(moveScr.addSpeed - newSp)/Mathf.Abs(startSpeed - newSp);
            Debug.Log(t + " " + moveScr.addSpeed + " " + newSp);
            moveScr.addSpeed -= addSpeadOf * Time.deltaTime * (myCoef.Evaluate(t));
            
            
            yield return null;
        }
        moveScr.addSpeed = newSp;

        yield return null;
    }


    public void ChSp()
    {
        StartCoroutine(mySp());
    }
}
