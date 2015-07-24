using UnityEngine;
using System.Collections;

public class HintButton : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void ShowHint()
    {
        ToolTip.instance.visible = true;
        ToolTip.instance.currentText = Level.instance.hint;
    }

    public void HideHint()
    {
        ToolTip.instance.visible = false;
    }

}
