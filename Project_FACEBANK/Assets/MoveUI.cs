using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveUI : MonoBehaviour {

    public void TransformUI() {
        this.GetComponent<RectTransform>().transform.position = Camera.main.ScreenToViewportPoint(Input.mousePosition);
    }
}
