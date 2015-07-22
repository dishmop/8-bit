﻿using UnityEngine;
using System.Collections;

public class TopComponent : GateComponent
{
    EmptyGateComponent empty;

    void Start()
    {
        empty = ((GameObject)Instantiate(Resources.Load("empty"))).GetComponent<EmptyGateComponent>();
        empty.numInputs = numInputs;
        empty.numOutputs = numOutputs;
        empty.extrawidth = 200;
        empty.setup();

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

    public void LoadComponent(string name)
    {
        gate.gates[0].Load(name);
    }

    public void Save(string name)
    {
        gate.Save(name);
    }
}