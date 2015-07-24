using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class menuItem : MonoBehaviour {
    public Image image;
    public Image background;

    public Level itemlevel;

	public void Setup () {
        image.sprite = Resources.LoadAll<Sprite>("gates")[itemlevel.spritenum];
	}
	
	void Update () {
	    if(itemlevel.isAvailable())
        {
            background.color = Color.white;
        }
        else
        {
            background.color = Color.gray;
        }

        if (itemlevel.Done())
        {
            background.color = Color.green;
        }
	}

    public void OnClick()
    {
        if (Application.loadedLevel == 1)
        {
            // in game, create the object
            GameManager.instance.topComponent.LoadComponent(itemlevel.name);
        }
        if(Application.loadedLevel == 0)
        {
            // in main menu, play the level
            if(!itemlevel.Done() && itemlevel.isAvailable())
            {
                Level.instance = itemlevel;
                GameManager.instance.LoadLevel(1);
            }
        }
    }

    public void MouseOver()
    {
        ToolTip.instance.visible = true;
        ToolTip.instance.currentText = itemlevel.description;

        if(Application.loadedLevel == 0)
        {        
            if (itemlevel.Done())
            {
                ToolTip.instance.currentText += "\nLevel completed";
            } else if (itemlevel.isAvailable())
            {
                ToolTip.instance.currentText += "\nLevel available";
            }
            else
            {
                ToolTip.instance.currentText += "\nRequires: ";

                foreach(Level prerequisite in itemlevel.prerequisites)
                {
                    ToolTip.instance.currentText += prerequisite.name + ", ";
                }

                ToolTip.instance.currentText = ToolTip.instance.currentText.Substring(0,ToolTip.instance.currentText.LastIndexOf(","));
            }
        }
    }

    public void MouseExit()
    {
        ToolTip.instance.visible = false;
    }
}
