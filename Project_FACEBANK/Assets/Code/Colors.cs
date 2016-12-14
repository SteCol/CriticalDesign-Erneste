using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Colors : MonoBehaviour {
    public List<ColorScript> colors;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        for (int i = 0; i < colors.Count; i++) {
            for (int j = 0; j < colors[i].thingsToHaveThisColor.Count; j++) {
                colors[i].thingsToHaveThisColor[i].color = colors[i].color;
            }
        }
	}
}
