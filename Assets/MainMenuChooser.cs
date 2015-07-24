using UnityEngine;
using System.Collections;

public class MainMenuChooser : MonoBehaviour {
    public GameObject itemPrefab;

	// Use this for initialization
	void Start () {
        for (int i = 0; i < GameManager.gatelevels.Length; i++)
        {
            GameObject item = (GameObject)Instantiate(itemPrefab);
            item.transform.SetParent(transform);

            item.GetComponent<menuItem>().itemlevel = GameManager.gatelevels[i];

            item.GetComponent<menuItem>().Setup();
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
