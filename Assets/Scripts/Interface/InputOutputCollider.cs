using UnityEngine;
using System.Collections;

public class InputOutputCollider : MonoBehaviour {
    public Gate attachedGate;
    public bool isInput; // otherwise output
    public int inputOutputNum;

    public bool visible = true;

    //bool mouseOver;
	
	void Update()
    {
        if (visible)
        {
            if (GameManager.instance.hitcollider == GetComponent<Collider2D>())
            {
                GameManager.instance.current = this;
                GameManager.instance.currentComponent = null;
                transform.localScale = new Vector3(2, 2, 2);
            }
            else
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
            GetComponent<Renderer>().enabled = true;
            GetComponent<Collider2D>().enabled = true;
        }
        else
        {
            GetComponent<Renderer>().enabled = false;
            GetComponent<Collider2D>().enabled = false;
        }
    }
}
