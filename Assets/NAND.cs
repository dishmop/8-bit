using UnityEngine;
using System.Collections;

public class NandGate : Gate
{
    public NandGate()
    {
        // two inputs
        ownInputs.Add(0);
        ownInputs.Add(1);

        // one output
        ownOutputs.Add(0);
    }

    protected override void UpdateOutputs()
    {
        parentGate.childOutputs[ownOutputs[0]].IsOnNew = !(parentGate.childInputs[ownInputs[0]].isOn && parentGate.childInputs[ownInputs[1]].isOn);
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
    }
}