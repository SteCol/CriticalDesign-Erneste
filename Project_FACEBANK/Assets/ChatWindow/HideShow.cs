using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HideShow : MonoBehaviour {


    public Vector3 startPos;
    public Vector3 stopPos;
    public float slerpValue;
    public float slerpSpeed;

    public bool show;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (show)
            slerpValue = slerpValue + slerpSpeed * Time.deltaTime;
            transform.position = Vector3.Slerp(startPos, stopPos, slerpValue);
	}
}
