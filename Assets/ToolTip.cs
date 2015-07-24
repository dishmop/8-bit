using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ToolTip : MonoBehaviour {
    public static ToolTip instance;

    Text textfield;
    Image background;

    public string currentText;

    public bool visible ;

    public AudioClip click1;
    public AudioClip click2;

    AudioSource audiosource;

	void Start () {
        instance = this;
        textfield = GetComponentInChildren<Text>();
        background = GetComponent<Image>();
        audiosource = GetComponent<AudioSource>();
	}
	
	void Update () {
        transform.position = UnityEngine.Input.mousePosition+new Vector3(-60,-5,0);

	    if(visible)
        {
            background.enabled = true;
            textfield.enabled = true;
            textfield.text = currentText;
        }
        else
        {
            background.enabled = false;
            textfield.enabled = false;
        }
	}

    public void Click1()
    {
        audiosource.PlayOneShot(click1);
    }

    public void Click2()
    {
        audiosource.PlayOneShot(click2);
    }
}
