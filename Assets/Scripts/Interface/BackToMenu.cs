using UnityEngine;
using System.Collections;

public class BackToMenu : MonoBehaviour {
    int frames = 0;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        frames++;
	    if(frames == 100)
        {
            Application.LoadLevel(0);
        }
	}
}
