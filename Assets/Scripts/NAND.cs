using UnityEngine;
using System.Collections;

public class NandGate : Gate
{
    public NandGate()
    {
        // two inputs
        AddInput(0);
        AddInput(1);

        // one output
        AddOutput(0);
    }

    protected override void UpdateOutputs()
    {
        parentGate.childOutputs[ownOutputs[0]].IsOn = !(parentGate.childInputs[ownInputs[0]].isOn && parentGate.childInputs[ownInputs[1]].isOn);
    }
}

public class NAND : GateComponent
{
    void Awake()
    {
        gate = new NandGate();
        gate.component = this;

        inputoffsets.Add(new Vector3(-20, 10));
        inputoffsets.Add(new Vector3(-20, -11));

        outputoffsets.Add(new Vector3(30,-1));

        GetComponent<SpriteRenderer>().sprite = Resources.LoadAll<Sprite>("gates")[1];
    }
}