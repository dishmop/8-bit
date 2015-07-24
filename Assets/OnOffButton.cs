using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class OnOffButton : MonoBehaviour {
    public bool isOn;

    new bool enabled = false;

    public int inputNum = -1;
    public int outputNum = -1;

    Image image;
	// Use this for initialization
	void Start () {
        image = GetComponent<Image>();
	}
	
	// Update is called once per frame
	void Update () {
        if(outputNum>=0)
        {
            if (GameManager.instance.topComponent.gate.childOutputs.ContainsKey(outputNum))
            {
                isOn = GameManager.instance.topComponent.gate.childOutputs[outputNum].IsOn;
                enabled = true;
            }
        }

        if(inputNum>=0)
        {
            if(GameManager.instance.topComponent.inputs.Count>inputNum)
            {
                enabled = true;

                isOn = GameManager.instance.topComponent.inputs[inputNum];
            }
        }

        if (enabled)
        {

            if (isOn)
            {
                image.color = Color.white;
            }
            else
            {
                image.color = Color.green;
            }
        } else
        {
            image.color = Color.grey;
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
