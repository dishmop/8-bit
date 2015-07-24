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

    public string name;

    public string hint;

    public int spritenum;

    public Level[] prerequisites;

    public string description;

    public bool Done()
    {
        return System.IO.File.Exists(Application.persistentDataPath + "/" + name + ".xml");
    }

    public bool isAvailable()
    {
        foreach(Level prerequisite in prerequisites)
        {
            if (!prerequisite.Done())
                return false;
        }

        return true;
    }

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
        //Application.LoadLevel(2);
        GameManager.instance.topComponent.Save(name);
        GameManager.instance.testingPanel.GetComponent<TestingPanel>().fading = true;
        GameManager.instance.testingPanel.GetComponent<TestingPanel>().success = true;
        ToolTip.instance.Success();
    }

    void Failed()
    {
        UnityEngine.Debug.Log("failed");
        GameManager.instance.testingPanel.GetComponent<TestingPanel>().fading = true;
        ToolTip.instance.Failure();
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

public class NotLevel : Level
{
    public NotLevel()
    {
        numInputs = 1;
        numOutputs = 1;

        name = "NOT";
        spritenum = 1;

        prerequisites = new Level[] { new NandLevel() };

        description = "'NOT' gate: If the input is on, the output is off; if the input is off, the output is on.";

        hint = "What happens if both inputs of a NAND gate are the same?";
    }


    protected override bool Test()
    {
        switch (currentStep)
        {
            case 0:
                testConfiguration(new bool[] { true }, new bool[] { false });
                break;
            case 1:
                testConfiguration(new bool[] { false }, new bool[] { true });
                break;
            case 2:
                return true;
        }

        return false;
    }
}

public class AndLevel : Level
{
    public AndLevel()
    {
        numInputs = 2;
        numOutputs = 1;

        name = "AND";
        spritenum = 2;

        description = "'AND' gate: The output is on only if both inputs are on.";

        prerequisites = new Level[] { new NotLevel() };

        hint = "NAND is NOT AND, and equivalently AND is NOT NAND.";
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

public class OrLevel : Level
{
    public OrLevel()
    {
        numInputs = 2;
        numOutputs = 1;

        name = "OR";
        spritenum = 3;

        description = "'OR' gate: The output is on if either or both inputs are on.";

        prerequisites = new Level[] { new NotLevel() };

        hint = "NAND sort of does what you want, but is the wrong way round...";
    }


    protected override bool Test()
    {
        switch (currentStep)
        {
            case 0:
                testConfiguration(new bool[] { true, true }, new bool[] { true });
                break;
            case 1:
                testConfiguration(new bool[] { true, false }, new bool[] { true });
                break;
            case 2:
                testConfiguration(new bool[] { false, true }, new bool[] { true });
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

public class NorLevel : Level
{
    public NorLevel()
    {
        numInputs = 2;
        numOutputs = 1;

        name = "NOR";
        spritenum = 4;

        description = "'NOR' gate: The output is on only if neither input is on.";

        prerequisites = new Level[] { new OrLevel() };

        hint = "NOR is short for NOT OR";
    }


    protected override bool Test()
    {
        switch (currentStep)
        {
            case 0:
                testConfiguration(new bool[] { true, true }, new bool[] { false });
                break;
            case 1:
                testConfiguration(new bool[] { true, false }, new bool[] { false });
                break;
            case 2:
                testConfiguration(new bool[] { false, true }, new bool[] { false });
                break;
            case 3:
                testConfiguration(new bool[] { false, false }, new bool[] { true });
                break;
            case 4:
                return true;
        }

        return false;
    }
}

public class XorLevel : Level
{
    public XorLevel()
    {
        numInputs = 2;
        numOutputs = 1;

        name = "XOR";
        spritenum = 5;

        description = "'XOR' gate: The output is on if either - but not both - of the inputs is on.";

        prerequisites = new Level[] { new OrLevel(), new AndLevel() };

        hint = "We want the output to be true when first OR second input is on, AND when NOT first AND second input is on.";
    }


    protected override bool Test()
    {
        switch (currentStep)
        {
            case 0:
                testConfiguration(new bool[] { true, true }, new bool[] { false });
                break;
            case 1:
                testConfiguration(new bool[] { true, false }, new bool[] { true });
                break;
            case 2:
                testConfiguration(new bool[] { false, true }, new bool[] { true });
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

public class XnorLevel : Level
{
    public XnorLevel()
    {
        numInputs = 2;
        numOutputs = 1;

        name = "XNOR";
        spritenum = 6;

        description = "'XNOR' gate: The output is on if both or neither input is on.";

        prerequisites = new Level[] { new NorLevel() };

        hint = "XNOR is an abbreviation for NOT XOR";
    }


    protected override bool Test()
    {
        switch (currentStep)
        {
            case 0:
                testConfiguration(new bool[] { true, true }, new bool[] { true });
                break;
            case 1:
                testConfiguration(new bool[] { true, false }, new bool[] { false });
                break;
            case 2:
                testConfiguration(new bool[] { false, true }, new bool[] { false });
                break;
            case 3:
                testConfiguration(new bool[] { false, false }, new bool[] { true });
                break;
            case 4:
                return true;
        }

        return false;
    }
}

public class And3Level : Level
{
    public And3Level()
    {
        numInputs = 3;
        numOutputs = 1;

        description = "Three-way 'AND' gate: The output is on only if all three inputs are on.";
        name = "AND3";
        spritenum = 7;

        prerequisites = new Level[] { new AndLevel() };

        hint = "We want the output to be on if first AND second are on, AND third is on.";
    }


    protected override bool Test()
    {
        switch (currentStep)
        {
            case 0:
                testConfiguration(new bool[] { true, true, true }, new bool[] { true });
                break;
            case 1:
                testConfiguration(new bool[] { true, false, true }, new bool[] { false });
                break;
            case 2:
                testConfiguration(new bool[] { false, true, true }, new bool[] { false });
                break;
            case 3:
                testConfiguration(new bool[] { false, false, true }, new bool[] { false });
                break;
            case 4:
                testConfiguration(new bool[] { true, true, false }, new bool[] { false });
                break;
            case 5:
                testConfiguration(new bool[] { true, false, false }, new bool[] { false });
                break;
            case 6:
                testConfiguration(new bool[] { false, true, false }, new bool[] { false });
                break;
            case 7:
                testConfiguration(new bool[] { false, false, false }, new bool[] { false });
                break;
            case 8:
                return true;
        }

        return false;
    }
}

public class Nand3Level : Level
{
    public Nand3Level()
    {
        numInputs = 3;
        numOutputs = 1;

        description = "Three-way 'NAND' gate: The output on unless all three inputs are on.";
        name = "NAND3";
        spritenum = 8;

        prerequisites = new Level[] { new And3Level() };

        hint = "NAND is short for NOT AND.";
    }


    protected override bool Test()
    {
        switch (currentStep)
        {
            case 0:
                testConfiguration(new bool[] { true, true, true }, new bool[] { false });
                break;
            case 1:
                testConfiguration(new bool[] { true, false, true }, new bool[] { true });
                break;
            case 2:
                testConfiguration(new bool[] { false, true, true }, new bool[] { true });
                break;
            case 3:
                testConfiguration(new bool[] { false, false, true }, new bool[] { true });
                break;
            case 4:
                testConfiguration(new bool[] { true, true, false }, new bool[] { true });
                break;
            case 5:
                testConfiguration(new bool[] { true, false, false }, new bool[] { true });
                break;
            case 6:
                testConfiguration(new bool[] { false, true, false }, new bool[] { true });
                break;
            case 7:
                testConfiguration(new bool[] { false, false, false }, new bool[] { true });
                break;
            case 8:
                return true;
        }

        return false;
    }
}

public class Or3Level : Level
{
    public Or3Level()
    {
        numInputs = 3;
        numOutputs = 1;

        description = "Three-way 'OR' gate: The output on if at least one input is on.";
        name = "OR3";
        spritenum = 9;

        prerequisites = new Level[] { new OrLevel() };

        hint = "We want the output to be on if first OR second is on, OR third is on.";
    }


    protected override bool Test()
    {
        switch (currentStep)
        {
            case 0:
                testConfiguration(new bool[] { true, true, true }, new bool[] { true });
                break;
            case 1:
                testConfiguration(new bool[] { true, false, true }, new bool[] { true });
                break;
            case 2:
                testConfiguration(new bool[] { false, true, true }, new bool[] { true });
                break;
            case 3:
                testConfiguration(new bool[] { false, false, true }, new bool[] { true });
                break;
            case 4:
                testConfiguration(new bool[] { true, true, false }, new bool[] { true });
                break;
            case 5:
                testConfiguration(new bool[] { true, false, false }, new bool[] { true });
                break;
            case 6:
                testConfiguration(new bool[] { false, true, false }, new bool[] { true });
                break;
            case 7:
                testConfiguration(new bool[] { false, false, false }, new bool[] { false });
                break;
            case 8:
                return true;
        }

        return false;
    }
}

public class SRLevel : Level
{
    public SRLevel()
    {
        numInputs = 2;
        numOutputs = 2;

        description = "'SR' latch: if S is on, Q is on and Q' is off, if R is on, Q is off and Q' is on. If neither is on, it holds its state.";
        name = "SR";
        spritenum = 10;

        prerequisites = new Level[] { new NorLevel() };

        hint = "What happens if we take two NOR gates, and connect the output of one to an input of the other, and vice versa";
    }


    protected override bool Test()
    {
        switch (currentStep)
        {
            case 0:
                testConfiguration(new bool[] { true, false }, new bool[] { false, true });
                break;
            case 1:
                testConfiguration(new bool[] { false, false }, new bool[] { false, true });
                break;
            case 2:
                testConfiguration(new bool[] { false, true }, new bool[] { true, false });
                break;
            case 3:
                testConfiguration(new bool[] { false, false }, new bool[] { true, false });
                break;
            case 4:
                return true;
        }

        return false;
    }
}
public class NandLevel : Level
{
    public NandLevel()
    {
        numInputs = 2;
        numOutputs = 1;

        name = "NAND";
        spritenum = 0;

        prerequisites = new Level[] { };

        description = "'NAND' gate: The output is off if both inputs are on, otherwise it is off.";
    }

    protected override bool Test()
    {
        return false;
    }
}

public class FreePlay : Level
{
    public FreePlay()
    {
        numInputs = 4;
        numOutputs = 4;

        name = "";

        spritenum = -1;

        prerequisites = new Level[] { };

        description = "Have fun.";

        hint = "Have you made all the components yet?";
    }

    protected override bool Test()
    {
        return false;
    }
}