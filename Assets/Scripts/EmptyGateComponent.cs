using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EmptyGateComponent : GateComponent {

    List<GameObject> inputpoints = new List<GameObject>();
    List<GameObject> outputpoints = new List<GameObject>();

    public int spritenum = -1;

    public int extrawidth = 0;

    public GameObject child;


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

        if (spritenum >= 0)
            GetComponent<SpriteRenderer>().sprite = Resources.LoadAll<Sprite>("gates")[spritenum];
    }

    new void Update()
    {
        for (int i = 0; i < numInputs; i++)
        {
            inputoffsets[i] = new Vector3(-extrawidth*0.9f-50, -60*i + 30*(numInputs-1));
            inputpoints[i].transform.position = transform.position + inputoffsets[i];
        }

        for (int i = 0; i < numOutputs; i++)
        {
            outputoffsets[i] = new Vector3(extrawidth*1.1f + 50, -60 * i + 30 * (numOutputs-1));
            outputpoints[i].transform.position = transform.position + outputoffsets[i];
        }

        base.Update();

        if(GameManager.instance.currentComponent == this)
        {
            child.SetActive(true);
        }
        else
        {
            child.SetActive(false);
        }
    }

    void OnDestroy()
    {
        for (int i = 0; i < numInputs; i++)
        {
            Object.Destroy(inputpoints[i]);
        }

        for (int i = 0; i < numOutputs; i++)
        {
            Object.Destroy(outputpoints[i]);
        }

    }
}
