using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TestingPanel : MonoBehaviour {
    public bool fading = false;
    int fadedframes;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    if(fading)
        {
            fadedframes++;
            GetComponent<Image>().color = new Color(1.0f, 0.0f, 0.0f, 1.0f / Mathf.Sqrt(Mathf.Sqrt(fadedframes)));

            if(fadedframes == 20)
            {
                gameObject.SetActive(false);
            }
        }
        else
        {
            fadedframes = 0;
            GetComponent<Image>().color = Color.white;
        }
	}
}
