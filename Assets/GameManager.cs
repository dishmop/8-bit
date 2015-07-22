using UnityEngine;
using System.Collections;

using Vectrosity;

public class GameManager : MonoBehaviour {
    bool connecting;

    VectorLine line;

    void Start ()
    {
        Vector2[] linepoints = new Vector2[2];
        line = new VectorLine("line", linepoints, null, 2.0f);
        line.color = Color.black;
    }
	
	// Update is called once per frame
	void Update () {
	    if(UnityEngine.Input.GetKeyDown(KeyCode.Mouse0))
        {
            connecting = true;
            line.points2[0] = UnityEngine.Input.mousePosition;
        }

        if(UnityEngine.Input.GetKeyUp(KeyCode.Mouse0))
        {
            connecting = false;
        }

        if(connecting)
        {
            line.active = true;

            line.points2[1] = UnityEngine.Input.mousePosition;

            line.Draw();
        }
        else
        {
            line.active = false;
        }
	}
}
