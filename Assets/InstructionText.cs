using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InstructionText : MonoBehaviour {
    Text textfield;
    Image image;

	// Use this for initialization
	void Start () {
        textfield = GetComponent<Text>();
        image = GetComponentInChildren<Image>();

        if(Level.instance.spritenum>=0)
        image.sprite = Resources.LoadAll<Sprite>("gates")[Level.instance.spritenum];

	}
	
	// Update is called once per frame
	void Update () {
        textfield.text = "AIM: " + Level.instance.description;
	}
}
