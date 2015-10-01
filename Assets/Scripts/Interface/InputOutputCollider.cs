using UnityEngine;
using System.Collections;

public class InputOutputCollider : MonoBehaviour {
    public Gate attachedGate;
    public bool isInput; // otherwise output
    public int inputOutputNum;

    public bool visible = true;

    public TextMesh inputText;
    public TextMesh outputText;

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

        if (attachedGate == GameManager.instance.topComponent.gate.gates[0])
        {
            if (isInput)
            {
                outputText.gameObject.SetActive( false);
                inputText.text = Level.instance.inputName[inputOutputNum];
            }
            else
            {
                inputText.gameObject.SetActive( false);
                outputText.text = Level.instance.outputName[inputOutputNum];
            }
        } else {
                inputText.gameObject.SetActive( false);
                outputText.gameObject.SetActive( false);
            }
    }
}
