using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {
	public void Quit()
    {
        Application.Quit();
    }

    public void MouseOverFreePlay()
    {
        ToolTip.instance.visible = true;
        ToolTip.instance.currentText = "Connect up the components you've made so far";

    }

    public void FreePlay()
    {
        Level.instance = new FreePlay();
        GameManager.instance.LoadLevel(1);
    }

    public void MouseExitFreePlay()
    {
        ToolTip.instance.visible = false;
    }
}
