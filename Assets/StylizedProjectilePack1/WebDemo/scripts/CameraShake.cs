using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour {

    //public bool cameraShakeBool = true;
    public Animator CamerShakeAnimator;
	public float tm = 10;
	public float every = 0.1f;
	public float bndX = 1.0f;
	public float bndY = 1.0f;
	
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public IEnumerator Shaking(float shtime = -1)
	{

		if (shtime < 0) shtime = tm;
		
		Vector3 savedPos = transform.position;
		float t = 0;
		while (t < shtime)
		{
			yield return  new WaitForSeconds(every);
			t += every;
			
			Vector3 rnd = new Vector3(Random.Range(-bndX, bndX), Random.Range(-bndY, bndY), 0);
			transform.position += rnd;
			yield return null;
			transform.position -= rnd;
			yield return null;

			t += 2 * Time.deltaTime;

		}

		//transform.position = new Vector3(transform.position.x, savedPos.y, transform.position.z);

		GetComponent<MoveControl>().enabled = true;
	}

    public void ShakeCamera()
    {
	    GetComponent<MoveControl>().enabled = false;
	    
	    StartCoroutine(Shaking());
	    /*
	    Debug.Log("SHAAAAAAAAAAAKaaaaaaaaaaaaaaaaaaaaaaaaED");
	    CamerShakeAnimator.enabled = true;
	    GetComponent<MoveControl>().enabled = false;
	    CamerShakeAnimator.CrossFade("cameraShake", 0.2f);
        //CamerShakeAnimator.SetTrigger("CameraShakeTrigger");
        */
    }


	public void ShakeCamera2(float tt)
	{
		//GetComponent<MoveControl>().enabled = false;
	    
		StartCoroutine(Shaking(tt));
		
		
	}
}
