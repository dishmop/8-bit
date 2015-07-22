﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EmptyGateComponent : GateComponent {

    List<GameObject> inputpoints = new List<GameObject>();
    List<GameObject> outputpoints = new List<GameObject>();


    void Awake()
    {
        gate = new Gate();
        gate.component = this;
    }

    public void setup()
    {
        for (int i = 0; i < numInputs; i++)
        {
            gate.AddInput(i);
        }

        for (int i = 0; i < numOutputs; i++)
        {
            gate.AddOutput(i);
        }

        setupvisual();
    }

    public void setupvisual()
    {
        for (int i = 0; i < numInputs; i++)
        {
            inputoffsets.Add(new Vector3(-100, -100 * i));

            GameObject point = (GameObject)Instantiate(Resources.Load("inout"), transform.position + inputoffsets[i], new Quaternion());

            if (gate.parentGate != null && gate.parentGate.component != null)
            {
                point.transform.parent = gate.parentGate.component.transform;
            }

            point.GetComponent<InputOutputCollider>().attachedGate = gate;
            point.GetComponent<InputOutputCollider>().inputOutputNum = i;
            point.GetComponent<InputOutputCollider>().isInput = true;

            inputpoints.Add(point);
        }

        for (int i = 0; i < numOutputs; i++)
        {
            outputoffsets.Add(new Vector3(100, -100 * i));

            GameObject point = (GameObject)Instantiate(Resources.Load("inout"), transform.position + outputoffsets[i], new Quaternion());
            if (gate.parentGate != null && gate.parentGate.component != null)
            {
                point.transform.parent = gate.parentGate.component.transform;
            }

            point.GetComponent<InputOutputCollider>().attachedGate = gate;
            point.GetComponent<InputOutputCollider>().inputOutputNum = i;
            point.GetComponent<InputOutputCollider>().isInput = false;

            outputpoints.Add(point);
        }
    }

    new void Update()
    {
        for (int i = 0; i < numInputs; i++)
        {
            inputoffsets[i] = new Vector3(-50*(4-gate.depth), -100*i);
            inputpoints[i].transform.position = transform.position + inputoffsets[i];
        }

        for (int i = 0; i < numOutputs; i++)
        {
            outputoffsets[i] = new Vector3(50 * (4 - gate.depth), -100 * i);
            outputpoints[i].transform.position = transform.position + outputoffsets[i];
        }

        base.Update();
    }
}
