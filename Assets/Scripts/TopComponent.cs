using UnityEngine;
using System.Collections;

public class TopComponent : GateComponent
{
    EmptyGateComponent empty;

    void Start()
    {
        numInputs = GameManager.instance.numInputs;
        numOutputs = GameManager.instance.numOutputs;

        empty = ((GameObject)Instantiate(Resources.Load("empty"))).GetComponent<EmptyGateComponent>();
        empty.numInputs = numInputs;
        empty.numOutputs = numOutputs;
        empty.extrawidth = 300;
        empty.setup();

        empty.visible = false;

        gate = new Gate();
        gate.AddGate(empty.gate);
        gate.component = this;

        for (int i = 0; i < numInputs; i++)
        {
            inputs.Add(false);
        }
    }

    new void Update()
    {

        base.Update();
    }

    public GateComponent LoadComponent(string xml)
    {
        return gate.gates[0].Load(xml);
    }

    public void Save(out string xml)
    {
        gate.Save(out xml);
    }
}