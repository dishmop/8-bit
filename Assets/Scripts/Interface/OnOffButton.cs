using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class OnOffButton : MonoBehaviour {
    public bool isOn;

    new bool enabled = false;

    public int inputNum = -1;
    public int outputNum = -1;

    public KeyCode key;

    Text displaytext;

    Image image;
	// Use this for initialization
	void Start () {
        image = GetComponent<Image>();
        displaytext = GetComponentInChildren<Text>();
	}
	
	// Update is called once per frame
	void Update () {
        if(outputNum>=0)
        {
            if (GameManager.instance.topComponent.gate.childOutputs.ContainsKey(outputNum))
            {
                isOn = GameManager.instance.topComponent.gate.childOutputs[outputNum].IsOn;
                enabled = true;

                displaytext.text = Level.instance.outputName[outputNum];
            }
            else
            {
                displaytext.text = "";
            }
        }

        if(inputNum>=0)
        {
            if(GameManager.instance.topComponent.inputs.Count>inputNum)
            {
                enabled = true;

                isOn = GameManager.instance.topComponent.inputs[inputNum];

                displaytext.text = Level.instance.inputName[inputNum];
            }
            else
            {
                displaytext.text = "";
            }
        }

        if (enabled)
        {

            if (isOn)
            {
                image.color = Color.green;
            }
            else
            {
                image.color = Color.gray;
            }
        } else
        {
            image.color = Color.black;
        }

        if (inputNum >= 0 && UnityEngine.Input.GetKeyDown(key))
        {
            isOn = true;
            GameManager.instance.topComponent.inputs[inputNum] = isOn;
        }

        if (inputNum >= 0 && UnityEngine.Input.GetKeyUp(key))
        {
            isOn = false;
            GameManager.instance.topComponent.inputs[inputNum] = isOn;
        }
	}

    public void OnClick()
    {
        if(inputNum>=0 && enabled)
        {
            isOn = !isOn;
            GameManager.instance.topComponent.inputs[inputNum] = isOn;
        }
    }
}
