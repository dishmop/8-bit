using System;
using System.Collections.Generic;
using UnityEngine;

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
            {
                GameManager.instance.testingPanel.SetActive(true);
                GameManager.instance.testingPanel.GetComponent<TestingPanel>().fading = false;
            }
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
            //if (GameManager.instance.testingPanel!=null)
            //GameManager.instance.testingPanel.SetActive(false);

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
        Application.LoadLevel(2);
        GameManager.instance.topComponent.Save(name);
    }

    void Failed()
    {
        UnityEngine.Debug.Log("failed");
        GameManager.instance.testingPanel.GetComponent<TestingPanel>().fading = true;
    }

    protected int currentStep = 0;
    int onframes = 0;

    protected void testConfiguration(bool[] inputs, bool[] desiredOutputs)
    {
        for(int i=0; i<numInputs; i++)
        {
            GameManager.instance.topComponent.inputs[i] = inputs[i];
        }

        // allow 10 frames for things to settle
        if(frames>10)
        {
            bool correct = true;
            for(int i=0; i<numOutputs; i++)
            {
                if(GameManager.instance.topComponent.gate.childOutputs[i].IsOn != desiredOutputs[i])
                {
                    correct = false;
                }
            }

            if(correct)
            {
                onframes++;

                if(onframes == 10)
                {
                    currentStep++;
                    onframes = 0;
                    frames = 0;
                }
            }
            else
            {
                onframes = 0;
            }
        }
    }
}

public class AndLevel : Level
{
    public AndLevel()
    {
        instance = this;
        numInputs = 2;
        numOutputs = 1;

        name = "AND";
    }


    protected override bool Test()
    {
        switch(currentStep)
        {
            case 0:
                testConfiguration(new bool[]{true,true},new bool[]{true});
                break;
            case 1:
                testConfiguration(new bool[]{true,false},new bool[]{false});
                break;
            case 2:
                testConfiguration(new bool[]{false,true},new bool[]{false});
                break;
            case 3:
                testConfiguration(new bool[] { false, false }, new bool[] { false });
                break;
            case 4:
                return true;
        }

        return false;
    }
}