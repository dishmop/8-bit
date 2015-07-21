using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Connector
{
    public Gate parentGate;

    abstract public bool IsOn { get; }
}

public class InputOutputConnector : Connector {
    public int output;
    public int input;

    public override bool IsOn
    {
        get { return parentGate.childOutputs[output].IsOn; }
    }
}

public class InputInputConnector : Connector
{
    public int input;
    public int childInput;

    public override  bool IsOn
    {
        get { 
            return parentGate.parentGate.childInputs[parentGate.ownInputs[childInput]].isOn;
        }
    }
}

public class OutputOutputConnector : Connector
{
    public int childOuput;
    public int output;

    public override bool IsOn
    {
        get { return parentGate.childOutputs[output].IsOn; }
    }
}

public class Input {
    public Gate parentGate;

    public int attachedGate;
    public int connector;

    public int inputNum;

    public bool isOn
    {
        get {
            if (connector >= 0)
            {
                return parentGate.connectors[connector].IsOn;
            } else if( parentGate.component!=null && parentGate.component.inputs.Count>0 )
            {
                return parentGate.component.inputs[inputNum];
            } else{
                return false;
            }
        }
    }
}

public class Output {
    public Gate parentGate;

    public int attachedGate;
    public int connector;

    public int outputNum;

    public int inputConnector = -1;

    bool isOn;

    public void Update()
    {
        isOn = IsOnNew;
    }

    public bool IsOn
    {
        get { return isOn; }
    }

    public bool IsOnNew;
}

public class Gate {

    public GateComponent component;

    public int depth = 0;


    public List<Gate> gates = new List<Gate>();
    public List<Connector> connectors = new List<Connector>();

    public List<Input> childInputs = new List<Input>();
    public List<Output> childOutputs = new List<Output>();

    public List<int> ownInputs = new List<int>();
    public List<int> ownOutputs = new List<int>();

    public Gate parentGate;

    public void AddGate(Gate gate) {
        gates.Add(gate);
        int gateNum = gates.IndexOf(gate);

        gate.depth = depth + 1;

        for(int i = 0; i<gate.ownInputs.Count; i++)
        {
            Input newInput = new Input();
            newInput.attachedGate = gateNum;
            newInput.parentGate = this;

            newInput.connector = -1;

            childInputs.Add(newInput);

            gate.ownInputs[i] = childInputs.IndexOf(newInput);

            newInput.inputNum = i;
        }

        for (int i = 0; i < gate.ownOutputs.Count; i++)
        {
            Output newOutput = new Output();
            newOutput.attachedGate = gateNum;
            newOutput.parentGate = this;

            newOutput.connector = -1;

            childOutputs.Add(newOutput);

            gate.ownOutputs[i] = childOutputs.IndexOf(newOutput);

            newOutput.outputNum = i;
        }

        gate.parentGate = this;
    }

    public void Connect(Gate outputFrom, int outputNum, Gate inputTo, int inputNum, InputOutputConnector connector = null)
    {
        if(outputFrom.parentGate != this || inputTo.parentGate != this) {
            throw new System.ArgumentException("gate parents do not match!");
        }

        if(connector == null)
        {
            connector = new InputOutputConnector();
        }
        if (childOutputs[outputFrom.ownOutputs[outputNum]].connector!= -1)
        {
            throw new System.ArgumentException("each output can only be connected to one input!");
        }

        connectors.Add(connector);
        int connectorNum = connectors.IndexOf(connector);


        childOutputs[outputFrom.ownOutputs[outputNum]].connector = connectorNum;
        childInputs[inputTo.ownInputs[inputNum]].connector = connectorNum;

        connector.parentGate = this;
        connector.output = outputFrom.ownOutputs[outputNum];
        connector.input = inputTo.ownInputs[inputNum];
    }

    public void ConnectOutput(Gate outputFrom, int outputNum, int ownOutputNum, OutputOutputConnector connector = null)
    {
        if(outputFrom.parentGate != this) {
            throw new System.ArgumentException("must be child gate!");
        }
        if(connector == null)
        {
            connector = new OutputOutputConnector();
        }

        if(parentGate.childOutputs[ownOutputs[ownOutputNum]].inputConnector != -1)
        {
            throw new System.ArgumentException("each output can only be connected to one input!");
        }

        connectors.Add(connector);
        int connectorNum = connectors.IndexOf(connector);

        childOutputs[outputFrom.ownOutputs[outputNum]].connector = connectorNum;
        parentGate.childOutputs[ownOutputs[ownOutputNum]].inputConnector = connectorNum;

        connector.parentGate = this;
        connector.output = outputFrom.ownOutputs[outputNum];
        connector.childOuput = ownOutputs[ownOutputNum];

    }

    public void ConnectInput(int ownInputNum, Gate inputTo, int inputNum, InputInputConnector connector = null)
    {
        if (inputTo.parentGate != this)
        {
            throw new System.ArgumentException("must be child gate!");
        }

        if (connector == null)
        {
            connector = new InputInputConnector();
        }

        connectors.Add(connector);
        int connectorNum = connectors.IndexOf(connector);


        childInputs[inputTo.ownInputs[inputNum]].connector = connectorNum;

        connector.parentGate = this;
        connector.input = inputTo.ownInputs[inputNum];
        connector.childInput = ownInputNum;
    }

    public void Update()
    {
        if(parentGate!=null)
        {
            depth = parentGate.depth + 1;
        }
        else
        {
            depth = 0;
        }

        foreach (Gate gate in gates)
        {
            gate.Update();
        }

        UpdateOutputs();
    }


    // set isOnNew for each own output
    protected virtual void UpdateOutputs()
    {
        foreach(int i in ownOutputs)
        {
            if (parentGate.childOutputs[i].inputConnector >= 0)
                parentGate.childOutputs[i].IsOnNew = connectors[parentGate.childOutputs[i].inputConnector].IsOn;
            else
                Debug.Log("problem!");
        }
    }

    public void LateUpdate()
    {
        foreach (Output output in childOutputs)
        {
            output.Update();
        }

        foreach (Gate gate in gates)
        {
            gate.LateUpdate();
        }
    }
}

public abstract class GateComponent : MonoBehaviour
{
    public List<bool> inputs = new List<bool>();

    public List<Vector3> inputoffsets = new List<Vector3>();
    public List<Vector3> outputoffsets = new List<Vector3>();

    public Gate gate;

    public void Update()
    {
        gate.Update();
    }

    void LateUpdate()
    {
        gate.LateUpdate();
    }
}