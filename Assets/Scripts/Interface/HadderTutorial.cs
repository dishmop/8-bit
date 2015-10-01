using UnityEngine;
using System.Collections;

public class HadderTutorial
    : MonoBehaviour
{
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
        Update1,
        Update2,
        Arithmetic1,
        Arithmetic2,
        Binary1,
        Decimal1,
        Decimal2,
        Decimal3,
        Binary2,
        Binary2a,
        Binary3,
        Binary4,
        Binary5,
        Representation1,
        Arithmetic3,
        Arithmetic4,
        Arithmetic5,
        Carry1,
        Carry2,
        TryIt,
        Done,
        Again
    }

    step currentstep = step.Update1;

    step laststep = step.Done;


    void Update()
    {
        if (laststep != currentstep)
        {
            Ping();
            ResetColour();
        }

        laststep = currentstep;

        switch (currentstep)
        {
            case step.Update1:
                BlockUI.SetActive(true);
                DisplayUIText("It's going well so far, isn't it?", true);
                DontDisplayUILowText();
                break;
            case step.Update2:
                DisplayUIText("We've made some gates which do seemingly arbitrary things to either on or off.", true);
                break;

            case step.Arithmetic1:
                DisplayUIText("Now let's use those to make some useful parts of a computer.", true);
                break;

            case step.Arithmetic2:
                DisplayUIText("Computers use this on/off system to do calculations.", true);
                break;

            case step.Binary1:
                DisplayUIText("They use a number system called binary.", true);
                break;

            case step.Decimal1:
                DisplayUIText("Normally people use decimal numbers.", true);
                break;

            case step.Decimal2:
                DisplayUIText("That means, whenever we multiply by ten, we add a zero.", true);
                break;

            case step.Decimal3:
                DisplayUIText("So 56 times ten is 560.", true);
                break;

            case step.Binary2:
                DisplayUIText("Binary is the same, but whenever we multiply by two, we add a zero.", true);
                break;

            case step.Binary2a:
                DisplayUIText("Zero is 0. One is 1. Then one times two is 10, so two is 10.", true);
                break;

            case step.Binary3:
                DisplayUIText("So we have 1 (one), 10 (two), 100 (four), 1000 (eight), 10000 (sixteen) just by multiplying by two.", true);
                break;

            case step.Binary4:
                DisplayUIText("Counting in binary goes like this: 1 (one), 10 (two), 11 (three), 100 (four), 101 (five), 110 (six)...", true);
                break;

            case step.Binary5:
                DisplayUIText("This is just a different way of writing numbers with only 1 and 0.", true);
                break;

            case step.Representation1:
                DisplayUIText("In our computer, we can use on to mean 1, and off to mean 0.", true);
                break;

            case step.Arithmetic3:
                DisplayUIText("Now say we want to add A and B, where A and B can be either 1 or 0.", true);
                break;

            case step.Arithmetic4:
                DisplayUIText("If both are 0, the sum is 0.", true);
                break;

            case step.Arithmetic5:
                DisplayUIText("If A is 1 and B is 0, or vice versa, the sum is 1.", true);
                break;

            case step.Carry1:
                DisplayUIText("If A is 1 and B is 1, the sum is 10 (two).", true);
                break;

            case step.Carry2:
                DisplayUIText("But each output is only either on or off, so the sum output is 0, and the carry output is 1.", true);
                break;

            case step.TryIt:
                DisplayUIText("This is the gate we're trying to make here.", true);
                break;

            case step.Done:
                BlockUI.SetActive(false);
                DisplayUIText("Try it. Press OK if you want to see this text again.", true);
                break;

            case step.Again:
                currentstep = step.Update2;
                break;
        }

        GameManager.instance.interactable = !BlockUI.activeSelf;
    }

    public void OKButton()
    {
        currentstep++;
    }

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
