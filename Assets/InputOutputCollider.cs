using UnityEngine;
using System.Collections;

public class InputOutputCollider : MonoBehaviour {
    public Gate attachedGate;
    public bool isInput; // otherwise output
    public int inputOutputNum;

    public bool visible = true;

    bool mouseOver;
	
	void Update()
    {
        if (visible)
        {
            if (GameManager.instance.hitcollider == GetComponent<Collider2D>())
            {
                GameManager.instance.current = this;
            }
            GetComponent<Renderer>().enabled = true;
        }
        else
        {
            GetComponent<Renderer>().enabled = false;
        }
    }
}
