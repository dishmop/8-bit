using UnityEngine;
using System.Collections;

public class MoveCamera : MonoBehaviour {	
	// Update is called once per frame
	void Update () {
	    if(UnityEngine.Input.GetKey(KeyCode.Mouse1)) {
            Vector3 pos = transform.position;
            pos.x -= 10*UnityEngine.Input.GetAxis("Mouse X");
            pos.y -= 10*UnityEngine.Input.GetAxis("Mouse Y");

            pos.x = Mathf.RoundToInt(pos.x);
            pos.y = Mathf.RoundToInt(pos.y);

            transform.position = pos;
        }
	}
}
