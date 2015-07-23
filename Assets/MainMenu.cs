using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {
    public Text inputsText;
    public Text outputsText;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        inputsText.text = "Inputs: " + GameManager.instance.numInputs;
        outputsText.text = "Outputs: " + GameManager.instance.numOutputs;
	}

    public void AddInputs(int num)
    {
        //GameManager.instance.numInputs += num;
    }
    public void AddOutputs(int num)
    {
       // GameManager.instance.numOutputs += num;
    }
}
