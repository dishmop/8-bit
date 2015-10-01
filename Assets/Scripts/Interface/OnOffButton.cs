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
    Text valuetext;

    Image image;
	// Use this for initialization
	void Start () {
        image = GetComponent<Image>();
        foreach(var text in GetComponentsInChildren<Text>()){
            if (text.gameObject.name == "valuetext")
            {
                valuetext = text;
            }
            else
            {
                displaytext = text;
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
        if(outputNum>=0)
        {
            if (GameManager.instance.topComponent.gate.childOutputs.ContainsKey(Level.instance.outputMap[outputNum]))
            {
                isOn = GameManager.instance.topComponent.gate.childOutputs[Level.instance.outputMap[outputNum]].IsOn;
                enabled = true;

                displaytext.text = Level.instance.outputName[Level.instance.outputMap[outputNum]];
            }
            else
            {
                displaytext.text = "";
            }
        }

        if(inputNum>=0)
        {
            if(GameManager.instance.topComponent.inputs.Count>Level.instance.inputMap[inputNum])
            {
                enabled = true;

                isOn = GameManager.instance.topComponent.inputs[Level.instance.inputMap[inputNum]];

                displaytext.text = Level.instance.inputName[Level.instance.inputMap[inputNum]];
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

        if (inputNum >= 0 && UnityEngine.Input.GetKeyDown(key) && GameManager.instance.topComponent.inputs.Count > Level.instance.inputMap[inputNum])
        {
            isOn = true;
            GameManager.instance.topComponent.inputs[Level.instance.inputMap[inputNum]] = isOn;
        }

        if (inputNum >= 0 && UnityEngine.Input.GetKeyUp(key) && GameManager.instance.topComponent.inputs.Count > Level.instance.inputMap[inputNum])
        {
            isOn = false;
            GameManager.instance.topComponent.inputs[Level.instance.inputMap[inputNum]] = isOn;
        }

        if (isOn)
        {
            valuetext.text = "1";
        }
        else
        {
            valuetext.text = "0";
        }
	}

    public void OnClick()
    {
        if (inputNum >= 0 && enabled)
        {
            isOn = !isOn;
            GameManager.instance.topComponent.inputs[Level.instance.inputMap[inputNum]] = isOn;
        }
    }
}
