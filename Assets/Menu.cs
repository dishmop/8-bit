using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour {
    public GameObject itemPrefab;

	// Use this for initialization
	void Start () {
	    for(int i=0; i<GameManager.gatenames.Length; i++)
        {
            if (System.IO.File.Exists(Application.persistentDataPath + "/" + GameManager.gatenames[i] + ".xml"))
            {
                GameObject item = (GameObject)Instantiate(itemPrefab);
                item.transform.SetParent(transform);

                item.GetComponent<menuItem>().itemnum = i;
                item.GetComponent<menuItem>().itemname = GameManager.gatenames[i];

                item.GetComponent<menuItem>().Setup();
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
