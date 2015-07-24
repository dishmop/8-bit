using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TestingPanel : MonoBehaviour {
    public bool fading = false;
    int fadedframes;

    public bool success = false;
	
	void Update () {
	    if(fading)
        {
            if (success)
            {
                fadedframes++;
                GetComponent<Image>().color = new Color(0.0f, 1.0f, 0.0f, 1.0f / Mathf.Sqrt(Mathf.Sqrt(fadedframes)));

                if (fadedframes == 20)
                {
                    Application.LoadLevel(0);
                }
            }
            else
            {
                fadedframes++;
                GetComponent<Image>().color = new Color(1.0f, 0.0f, 0.0f, 1.0f / Mathf.Sqrt(Mathf.Sqrt(fadedframes)));

                if (fadedframes == 20)
                {
                    gameObject.SetActive(false);
                }
            }
        }
        else
        {
            fadedframes = 0;
            GetComponent<Image>().color = new Color(1.0f,1.0f,1.0f,0.5f);
        }
	}
}
