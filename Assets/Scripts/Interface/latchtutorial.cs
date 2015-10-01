using UnityEngine;
using System.Collections;

public class latchtutorial
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
        Feedback1,
        Feedback2,
        Feedback3,
        Feedback4,
        Opposites,
        Warning,
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
                DisplayUIText("So far, we've only made gates, where the outputs depend solely on the inputs.", true);
                DontDisplayUILowText();
                break;
            case step.Update2:
                DisplayUIText("We want to be able to store information - this is called a computer's memory.", true);
                break;

            case step.Feedback1:
                DisplayUIText("If we connect outputs of our gates back to the inputs, we get an interesting behaviour.", true);
                break;

            case step.Feedback2:
                DisplayUIText("Instead of the outputs chaning whenever we change the inputs, sometimes they just stay the same.", true);
                break;

            case step.Feedback3:
                DisplayUIText("Take two NOR gates, and connect one input of each to the level inputs.", true);
                break;

            case step.Feedback4:
                DisplayUIText("Then connect the output of each both to the level output, and the remaining input on the other gate.", true);
                break;

            case step.Opposites:
                DisplayUIText("All of these so called 'latches' and 'flip-flops' have two outputs, where one is the opposite of the other.", true);
                break;

            case step.Warning:
                DisplayUIText("Be aware that these components can flicker sometimes before you use them, because no state is set, but that's fine.", true);
                break;

            case step.Done:
                BlockUI.SetActive(false);
                DisplayUIText("Try it. Play around before you test. Press OK if you want to see this text again.", true);
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
