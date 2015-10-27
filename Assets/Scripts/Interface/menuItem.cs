using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine.Analytics;

public class menuItem : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {
    public Image image;
    public Image background;
    public GameObject tutorialText;

    public Level itemlevel;

	public void Setup () {
        image.sprite = Resources.LoadAll<Sprite>("gates")[itemlevel.spritenum];

        if (Application.loadedLevel != 0 || itemlevel.name != "NOT")
        {
            tutorialText.SetActive(false);
        }
	}
	
	void Update () {
	    if(itemlevel.isAvailable())
        {
            background.color = Color.white;
        }
        else
        {
            background.color = Color.gray;
            GetComponent<Button>().enabled = false;
        }

        if (itemlevel.Done())
        {
            background.color = new Color(0.1f,0.1f,0.1f,1.0f);
        }
	}

    public void OnPointerDown(PointerEventData ped)
    {
        if (Application.loadedLevel != 0)
        {
            // in game, create the object
            GateComponent comp = GameManager.instance.topComponent.LoadComponent(PlayerPrefs.GetString(itemlevel.name));

            GameManager.instance.movingcomp = comp;
            GameManager.instance.positionRelative = new Vector3(0,0,10);
        }
    }


    public void OnPointerUp(PointerEventData ped)
    {

    }

    public void OnClick()
    {
        if(Application.loadedLevel == 0)
        {
            // in main menu, play the level
            if(!itemlevel.Done() && itemlevel.isAvailable())
            {
                ToolTip.instance.Click2();
                Level.instance = itemlevel;
				Level.instance.startTime = Time.time;
				Level.instance.numTestAttempts = 0;
				
				Analytics.CustomEvent("levelStart", new Dictionary<string, object>
				                      {
					{ "levelName", itemlevel.name },
				});		
//				Debug.Log("Analytics: levelStart - levelName: " + itemlevel.name);
				
                

                if (itemlevel.name == "NOT")
                {
                    GameManager.instance.LoadLevel(2);
                }
                else if (itemlevel.name == "HADDER")
                {
                    GameManager.instance.LoadLevel(3);
                }
                else if (itemlevel.name == "SR")
                {
                    GameManager.instance.LoadLevel(4);
                }
                else
                {
                    GameManager.instance.LoadLevel(1);
                }
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
