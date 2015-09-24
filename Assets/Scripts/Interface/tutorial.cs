using UnityEngine;
using System.Collections;

public class tutorial : MonoBehaviour {

	// Use this for initialization
	void Start () {
        if (Level.instance.name != "NOT")
        {
            GetComponent<UnityEngine.UI.Image>().enabled = false;
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
