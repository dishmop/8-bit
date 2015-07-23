using System;
using System.Collections.Generic;

abstract public class Level
{
    public static Level instance;

    bool testing = false;
    protected int frames = 0;

    public int numInputs;
    public int numOutputs;

    protected string name;

    public void Update()
    {
        if(testing)
        {
            if (GameManager.instance.testingPanel != null)
                GameManager.instance.testingPanel.SetActive(true);
            if(Test())
            {
                testing = false;
                Succeeded();
            }

            frames++;

            if(frames>=100)
            {
                Failed();
                testing = false;
            }
        }
        else
        {
            if (GameManager.instance.testingPanel!=null)
            GameManager.instance.testingPanel.SetActive(false);

            frames = 0;
        }
    }

    protected abstract bool Test();

    public void BeginTest()
    {
        testing = true;
    }

    void Succeeded()
    {
        UnityEngine.Debug.Log("success!");

    }

    void Failed()
    {
        UnityEngine.Debug.Log("failed");
    }
}

public class AndLevel : Level
{
    public AndLevel()
    {
        instance = this;
        numInputs = 2;
        numOutputs = 1;

        currentstep = step.onon;

        name = "AND";
    }

    enum step
    {
        onon,onoff,offon,offoff
    }

    step currentstep;

    int onframes;

    protected override bool Test()
    {
        switch(currentstep)
        {
            case step.onon:
                GameManager.instance.topComponent.inputs[0] = true;
                GameManager.instance.topComponent.inputs[1] = true;
                if(frames>5 && GameManager.instance.topComponent.gate.childOutputs[0].IsOn == true){
                    onframes ++;
                }else{
                    onframes = 0;
                }
                if(onframes == 10)
                {
                    currentstep = step.offon;
                    frames = 0;
                }
                break;
            case step.offon:
                GameManager.instance.topComponent.inputs[0] = false;
                GameManager.instance.topComponent.inputs[1] = true;
                if(frames>5 && GameManager.instance.topComponent.gate.childOutputs[0].IsOn == false){
                    onframes ++;
                }else{
                    onframes = 0;
                }
                if(onframes == 10)
                {
                    currentstep = step.onoff;
                    frames = 0;
                }
                break;
            case step.onoff:
                GameManager.instance.topComponent.inputs[0] = true;
                GameManager.instance.topComponent.inputs[1] = false;
                if(frames>5 && GameManager.instance.topComponent.gate.childOutputs[0].IsOn == false){
                    onframes ++;
                }else{
                    onframes = 0;
                }
                if(onframes == 10)
                {
                    currentstep = step.offoff;
                    frames = 0;
                }
                break;
            case step.offoff:
                GameManager.instance.topComponent.inputs[0] = false;
                GameManager.instance.topComponent.inputs[1] = false;
                if(frames>5 && GameManager.instance.topComponent.gate.childOutputs[0].IsOn == false){
                    onframes ++;
                }else{
                    onframes = 0;
                }
                if(onframes == 10)
                {
                    return true;
                }
                break;
        }

        return false;
    }
}