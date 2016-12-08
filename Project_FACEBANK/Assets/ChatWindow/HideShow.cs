using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class HideShow : MonoBehaviour {


    public Vector3 showPos;
    public Vector3 hiddenPos;
    public float slerpValue;
    public float slerpSpeed;

    public GameObject enableThis;

    public bool show;
    public bool getShowPos, getHidePos;

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

        
        
        if (!show && slerpValue < 1)
        {
            slerpValue = slerpValue + slerpSpeed * Time.deltaTime;
            transform.position = Vector3.Slerp(showPos, hiddenPos, slerpValue);

        }
        else if (show && slerpValue > 0){
            slerpValue = slerpValue - slerpSpeed * Time.deltaTime;
            transform.position = Vector3.Lerp(showPos, hiddenPos, slerpValue);

        }
        


        //if (show && this.transform.position != startPos)
        //{
        //    MoveToStartPos();
        //}


        if (getShowPos)
        {
            showPos = this.transform.position;
            getShowPos = false;
        }

        if (getHidePos) {
            hiddenPos = this.transform.position;
            getHidePos = false;
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
            if (this.transform.position.x < showPos.x)
            {
                this.transform.Translate(slerpSpeed * Time.deltaTime, 0, 0);
            }
            else if (this.transform.position.x > showPos.x)
            {
                this.transform.Translate(-slerpSpeed * Time.deltaTime, 0, 0);
            }
        }

        //Vertical
        if (applyYTransform)
        {
            if (this.transform.position.y < showPos.y)
            {
                this.transform.Translate(0, slerpSpeed * Time.deltaTime, 0);
            }
            else if (this.transform.position.y > showPos.y)
            {
                this.transform.Translate(0, -slerpSpeed * Time.deltaTime, 0);
            }
        }

        //Depth
        if (applyZTransform)
        {
            if (this.transform.position.z < showPos.z)
            {
                this.transform.Translate(0, 0, slerpSpeed * Time.deltaTime);
            }
            else if (this.transform.position.z > showPos.z)
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
            if (this.transform.position.x < hiddenPos.x)
            {
                this.transform.Translate(slerpSpeed * Time.deltaTime, 0, 0);
            }
            else if (this.transform.position.x > hiddenPos.x)
            {
                this.transform.Translate(-slerpSpeed * Time.deltaTime, 0, 0);
            }
        }

        //Vertical
        if (applyYTransform)
        {
            if (this.transform.position.y < hiddenPos.y)
            {
                this.transform.Translate(0, slerpSpeed * Time.deltaTime, 0);
            }
            else if (this.transform.position.y > hiddenPos.y)
            {
                this.transform.Translate(0, -slerpSpeed * Time.deltaTime, 0);
            }
        }

        //Depth
        if (applyZTransform)
        {
            if (this.transform.position.z < hiddenPos.z)
            {
                this.transform.Translate(0, 0, slerpSpeed * Time.deltaTime);
            }
            else if (this.transform.position.z > hiddenPos.z)
            {
                this.transform.Translate(0, 0, -slerpSpeed * Time.deltaTime);
            }
        }
    }
}
