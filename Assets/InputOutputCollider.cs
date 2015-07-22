using UnityEngine;
using System.Collections;

public class InputOutputCollider : MonoBehaviour {
    public Gate attachedGate;
    public bool isInput; // otherwise output
    public int inputOutputNum;

    bool mouseOver;
	
	void Update()
    {
        if (GameManager.instance.hitcollider == GetComponent<Collider2D>())
        {
            GameManager.instance.current = this;
        }
    }
}
