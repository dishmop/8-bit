using UnityEngine;
using System.Collections;

public class Clock : MonoBehaviour {
    bool isOn = false;

    public OnOffButton button;


    int frames = 0;
	
	void Update () {
	    if(isOn)
        {
            frames++;
            if(frames == 100)
            {
                button.OnClick();
                frames = 0;
            }
        }
	}

    public void onClick()
    {
        isOn = !isOn;
    }
}
