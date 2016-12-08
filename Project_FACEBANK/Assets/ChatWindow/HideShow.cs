using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class HideShow : MonoBehaviour {


    public Vector3 startPos;
    public Vector3 stopPos;
    public float slerpValue;
    public float slerpSpeed;

    public GameObject enableThis;

    public bool show;
    public bool getFirstPos, getSecondPos;

    public float value;

    public Text showHodeButtonText;

    public bool applyXTransform;
    public bool applyYTransform;
    public bool applyZTransform;



    //private IEnumerator coroutine;

    // Use this for initialization
    void Start () {
        //startPos = this.transform.position;
        //coroutine = Move(0);
		
	}
	
	// Update is called once per frame
	void Update () {
        /*
        if (show)
            StartCoroutine(Move( 2.0F));
            */



        if (show)
        {
            showHodeButtonText.text = "Hide";
           // MoveToStartPos();
        }

        if (!show)
        {
            showHodeButtonText.text = "Show";
            //MoveToStopPos();

        }
        if (enableThis != null)
            enableThis.SetActive(show);

        
        
        if (show && slerpValue < 1)
        {
            slerpValue = slerpValue + slerpSpeed * Time.deltaTime;
            transform.position = Vector3.Slerp(startPos, stopPos, slerpValue);

        }
        else if (!show && slerpValue > 0){
            slerpValue = slerpValue - slerpSpeed * Time.deltaTime;
            transform.position = Vector3.Lerp(startPos, stopPos, slerpValue);

        }
        


        //if (show && this.transform.position != startPos)
        //{
        //    MoveToStartPos();
        //}


        if (getFirstPos)
        {
            startPos = this.transform.position;
            getFirstPos = false;
        }

        if (getSecondPos) {
            stopPos = this.transform.position;
            getSecondPos = false;
        }
    }

    public void Show_Hide() {
        show = !show;
    }

    private IEnumerator Move(float waitTime) {
        //yield return new wa(waitTime);
        value = waitTime;
        yield return value;

    }

    void MoveToStartPos() {

        //Horizontal
        if (applyXTransform)
        {
            if (this.transform.position.x < startPos.x)
            {
                this.transform.Translate(slerpSpeed * Time.deltaTime, 0, 0);
            }
            else if (this.transform.position.x > startPos.x)
            {
                this.transform.Translate(-slerpSpeed * Time.deltaTime, 0, 0);
            }
        }

        //Vertical
        if (applyYTransform)
        {
            if (this.transform.position.y < startPos.y)
            {
                this.transform.Translate(0, slerpSpeed * Time.deltaTime, 0);
            }
            else if (this.transform.position.y > startPos.y)
            {
                this.transform.Translate(0, -slerpSpeed * Time.deltaTime, 0);
            }
        }

        //Depth
        if (applyZTransform)
        {
            if (this.transform.position.z < startPos.z)
            {
                this.transform.Translate(0, 0, slerpSpeed * Time.deltaTime);
            }
            else if (this.transform.position.z > startPos.z)
            {
                this.transform.Translate(0, 0, -slerpSpeed * Time.deltaTime);
            }
        }
    }

    void MoveToStopPos()
    {

        //Horizontal
        if (applyXTransform)
        {
            if (this.transform.position.x < stopPos.x)
            {
                this.transform.Translate(slerpSpeed * Time.deltaTime, 0, 0);
            }
            else if (this.transform.position.x > stopPos.x)
            {
                this.transform.Translate(-slerpSpeed * Time.deltaTime, 0, 0);
            }
        }

        //Vertical
        if (applyYTransform)
        {
            if (this.transform.position.y < stopPos.y)
            {
                this.transform.Translate(0, slerpSpeed * Time.deltaTime, 0);
            }
            else if (this.transform.position.y > stopPos.y)
            {
                this.transform.Translate(0, -slerpSpeed * Time.deltaTime, 0);
            }
        }

        //Depth
        if (applyZTransform)
        {
            if (this.transform.position.z < stopPos.z)
            {
                this.transform.Translate(0, 0, slerpSpeed * Time.deltaTime);
            }
            else if (this.transform.position.z > stopPos.z)
            {
                this.transform.Translate(0, 0, -slerpSpeed * Time.deltaTime);
            }
        }
    }
}
