using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EmptyGateComponent : GateComponent {
    public int numInputs;
    public int numOutputs;

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
            inputoffsets.Add(new Vector3(-100, -100 * i));
            gate.ownInputs.Add(i);

            GameObject point = (GameObject)Instantiate(Resources.Load("inout"), transform.position + inputoffsets[i], new Quaternion());
            point.transform.parent = transform;

            inputpoints.Add(point);
        }

        for (int i = 0; i < numOutputs; i++)
        {
            outputoffsets.Add(new Vector3(100, -100 * i));
            gate.ownOutputs.Add(i);

            GameObject point = (GameObject)Instantiate(Resources.Load("inout"), transform.position + outputoffsets[i], new Quaternion());
            point.transform.parent = transform;

            outputpoints.Add(point);
        }
    }

    void Update()
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
