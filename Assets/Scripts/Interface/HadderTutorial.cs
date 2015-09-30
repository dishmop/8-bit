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
        Update,
        Done
    }

    step currentstep = step.Update;

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
            case step.Update:
                BlockUI.SetActive(true);
                DisplayUIText("It's going well so far, isn't it?", true);
                DontDisplayUILowText();
                break;


            case step.Done:
                BlockUI.SetActive(false);
                DontDisplayUILowText();
                DontDisplayUIText();
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
