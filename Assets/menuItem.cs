using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class menuItem : MonoBehaviour {
    public Image image;

    public string itemname;
    public int itemnum;

	public void Setup () {
        image.sprite = Resources.LoadAll<Sprite>("gates")[itemnum];
	}
	
	void Update () {
	
	}

    public void OnClick()
    {
        GameManager.instance.topComponent.LoadComponent(itemname);
    }
}
