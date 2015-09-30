using UnityEngine;
using System.Collections;

public class TutorialManager : MonoBehaviour {
    public GameObject UITextPanel;
    public GameObject UITextOkButton;
    public GameObject UITextActual;

    public GameObject UILowTextPanel;
    public GameObject UILowTextOkButton;
    public GameObject UILowTextActual;

    public GameObject TestingPanel;

    public GameObject BlockUI;

    public UnityEngine.UI.Button testButton;

    enum step
    {
        Welcome,
        LogicGates1,
        LogicGates2,
        LogicGates3,
        LogicGates4,
        Level1,
        Level2,
        Inputs1,
        Inputs2,
        Inputs3,
        Components1,
        Components2,
        Components3,
        Components4,
        Components5,
        Components6,
        Delete1,
        Delete2,
        Connect1,
        Connect2,
        Connect3,
        Connect4,
        Explain1,
        Explain2,
        Explain3,
        Explain4,
        Test1,
        Test2,
        Fail
    }

    step currentstep = step.Welcome;

    step laststep = step.LogicGates1;

    int okframes = 0;

    bool doneon = false;
    bool doneoff = false;

    bool panelon = false;

	void Update () {
        if (laststep != currentstep)
        {
            Ping();
            ResetColour();
        }

        laststep = currentstep;

        switch (currentstep)
        {
            case step.Welcome:
                DisplayUIText("Welcome to 8-Bit!", true);
                DontDisplayUILowText();
                testButton.interactable = false;
                break;
            case step.LogicGates1:
                DisplayUIText("This is a game about logic gates.", true);
                break;
            case step.LogicGates2:
                DisplayUIText("Logic gates are what computers are made from.", true);
                break;
            case step.LogicGates3:
                DisplayUIText("A logic gate takes inputs and gives outputs.", true);
                break;
            case step.LogicGates4:
                DisplayUIText("These inputs and outputs can be either on or off.", true);
                break;
            case step.Level1:
                DisplayUIText("On this level, we are hold to make a NOT gate (above).", true);
                break;
            case step.Level2:
                DisplayUIText("A NOT gate's output is the opposite of its input.", true);
                break;
            case step.Inputs1:
                DisplayUIText("In this game, we use green for on and grey for off.", true);
                break;
            case step.Inputs2:
                DisplayUILowText("On this level, we have only one input and one output.", true);
                DontDisplayUIText();
                break;
            case step.Inputs3:
                DisplayUILowText("Try clicking on the input below to turn it on.", false);
                BlockUI.SetActive(false);

                if (GameManager.instance.topComponent.inputs[0] == true)
                {
                    currentstep++;
                }
                break;
            case step.Components1:
                DisplayUIText("Nothing happened...", true);
                DontDisplayUILowText();
                BlockUI.SetActive(true);
                break;
            case step.Components2:
                DisplayUIText("We need to connect some components.", true);
                break;
            case step.Components3:
                DisplayUIText("We can make all the components we need from one simple logic gate.", true);
                break;
            case step.Components4:
                DisplayUIText("The NAND gate takes two inputs, and has one output.", true);
                break;
            case step.Components5:
                DisplayUIText("The output is on as long as both inputs aren't on together.", true);
                break;
            case step.Components6:
                DisplayUIText("Drag a NAND gate from the panel on the left onto the screen.", false);
                BlockUI.SetActive(false);

                if (okframes>3)
                {
                    currentstep++;
                }

                if (GameManager.instance.topComponent.gate.gates[0].gates.Count == 1 && !UnityEngine.Input.GetMouseButton(0))
                {
                    okframes++;
                }
                else
                {
                    okframes = 0;
                }
                break;
            case step.Delete1:
                DisplayUIText("You can delete it by dragging it off the screen", false);
                BlockUI.SetActive(false);



                if (GameManager.instance.topComponent.gate.gates[0].gates.Count == 0 && !UnityEngine.Input.GetMouseButton(0))
                {
                    okframes++;
                }
                else
                {
                    okframes = 0;
                }

                if (okframes > 3)
                {
                    currentstep++;
                }
                break;
            case step.Delete2:
                DisplayUIText("Actually, we needed that, make another one.", false);
                BlockUI.SetActive(false);


                if (GameManager.instance.topComponent.gate.gates[0].gates.Count == 1 && !UnityEngine.Input.GetMouseButton(0))
                {
                    okframes++;
                }
                else
                {
                    okframes = 0;
                }


                if (okframes > 3)
                {
                    currentstep++;
                }
                break;
            case step.Connect1:
                DisplayUIText("Cool. Now we need to connect it up.", true);
                BlockUI.SetActive(true);
                break;
            case step.Connect2:
                DisplayUIText("Click and drag from the dot to the right of the gate to the dot on the right of the screen.", false);
                BlockUI.SetActive(false);
                if (GameManager.instance.topComponent.gate.gates[0].connectors.Count == 1)
                {
                    currentstep++;
                }
                break;
            case step.Connect3:
                DisplayUIText("You just connected the output of the gate to the output of the level.", true);
                BlockUI.SetActive(true);
                break;
            case step.Connect4:
                DisplayUIText("Now connect both gate inputs to the level input.", false);
                BlockUI.SetActive(false);
                if (GameManager.instance.topComponent.gate.gates[0].connectors.Count == 3)
                {
                    currentstep++;
                }
                break;
            case step.Explain1:
                DisplayUIText("You've made a NOT gate!", true);
                BlockUI.SetActive(true);
                break;
            case step.Explain2:
                DisplayUIText("We know that when both inputs of a NAND gate are on, the output is off...", true);
                GameManager.instance.topComponent.inputs[0] = true;
                BlockUI.SetActive(true);
                break;
            case step.Explain3:
                DisplayUIText("And when neither input is on, the output is on.", true);
                GameManager.instance.topComponent.inputs[0] = false;
                BlockUI.SetActive(true);
                break;
            case step.Explain4:
                DisplayUIText("So the output of the level is the opposite of the input", true);
                BlockUI.SetActive(true);
                break;
            case step.Test1:
                DisplayUILowText("Try it! (toggle the input below)", false);
                DontDisplayUIText();
                BlockUI.SetActive(false);

                if (GameManager.instance.topComponent.inputs[0] == true)
                {
                    doneon = true;
                }
                if (GameManager.instance.topComponent.inputs[0] == false)
                {
                    doneoff = true;
                }

                if (doneon && doneoff)
                {
                    currentstep++;
                }

                break;

            case step.Test2:
                DisplayUILowText("Now click the 'Test' button to complete the level", false);
                testButton.interactable = true;

                if (TestingPanel.activeSelf)
                {
                    panelon = true;
                }

                if (!TestingPanel.activeSelf && panelon)
                {
                    currentstep++;
                }

                break;

            case step.Fail:
                DisplayUIText("Hmm, something's not right. Keep trying.", false);
                DontDisplayUILowText();
                BlockUI.SetActive(false);
                break;
        }

        GameManager.instance.interactable = !BlockUI.activeSelf;
	}

    public void OKButton() {
        currentstep++;
    }

    //void StartStep()
    //{
    //    switch (currentstep)
    //    {
    //        case step.Welcome:
    //            ResetColour();
    //            break;
    //        case step.LogicGates1:
    //            DontDisplayUIText();
    //            break;
    //        case step.LogicGates2:
    //            break;
    //    }
    //}

    void DisplayUIText(string text, bool okbutton)
    {
        UITextPanel.SetActive(true);
        UITextOkButton.SetActive(okbutton);
        UITextActual.GetComponent<UnityEngine.UI.Text>().text = text;

        UITextPanel.GetComponent<UnityEngine.UI.Image>().color = Color.Lerp(UITextPanel.GetComponent<UnityEngine.UI.Image>().color, Color.green, Time.deltaTime);
    }

    void DontDisplayUIText()
    {
        UITextPanel.SetActive(false);
    }

    void DisplayUILowText(string text, bool okbutton)
    {
        UILowTextPanel.SetActive(true);
        UILowTextOkButton.SetActive(okbutton);
        UILowTextActual.GetComponent<UnityEngine.UI.Text>().text = text;
          
        UILowTextPanel.GetComponent<UnityEngine.UI.Image>().color = Color.Lerp(UILowTextPanel.GetComponent<UnityEngine.UI.Image>().color, Color.green, Time.deltaTime);
    }

    void DontDisplayUILowText()
    {
        UILowTextPanel.SetActive(false);
    }

    void Ping()
    {
        GetComponent<AudioSource>().Play();
    }

    void ResetColour()
    {
        UITextPanel.GetComponent<UnityEngine.UI.Image>().color = Color.white;
        UILowTextPanel.GetComponent<UnityEngine.UI.Image>().color = Color.white;
    }
}
