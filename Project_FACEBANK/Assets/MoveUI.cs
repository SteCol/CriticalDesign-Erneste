using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveUI : MonoBehaviour {
    public bool grabbed;
    public Vector2 offset;
    public Vector2 mousePos;
    public Vector2 inBetweenCalc;
    public Vector2 UIPos;

    public void Grab() {
        grabbed = true;
        offset = UIPos - mousePos;

        //offset = new Vector2(0, 0);

    }

    public void Release()
    {
        grabbed = false;
    }

    void Update() {
        mousePos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        UIPos = new Vector2(this.GetComponent<RectTransform>().transform.position.x, this.GetComponent<RectTransform>().transform.position.y);



        if (grabbed) {
            this.GetComponent<RectTransform>().transform.position = new Vector2(Input.mousePosition.x + offset.x, Input.mousePosition.y + offset.y);

            

        }
    }
}
