using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using System.Xml;

public abstract class Connector
{
    public Gate parentGate;
    public int connectorNum = -1;
    public ConnectorComponent component;

    abstract public bool IsOn { get; }

    abstract public void Save(XmlWriter writer, int indexx);

    abstract public void Remove();
}

public class InputOutputConnector : Connector {
    public int output;
    public int input;

    public override bool IsOn
    {
        get { return parentGate.childOutputs[output].IsOn; }
    }

    public override void Save(XmlWriter writer, int index)
    {
        writer.WriteStartElement("inoutconnector");
        writer.WriteAttributeString("index", index.ToString());

        writer.WriteAttributeString("input", input.ToString());
        writer.WriteAttributeString("output", output.ToString());
        writer.WriteEndElement();
    }

    //removes the connector
    public override void Remove()
    {
        Object.Destroy(component.gameObject);

        if (parentGate.childInputs.ContainsKey(input))
            parentGate.childInputs[input].connector = -1;

        if(parentGate.childOutputs.ContainsKey(output) && parentGate.childOutputs[output].connectors.Contains(connectorNum))
            parentGate.childOutputs[output].connectors.Remove(connectorNum);

        parentGate.connectors.Remove(connectorNum);
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

    public override void Save(XmlWriter writer, int index)
    {
        writer.WriteStartElement("ininconnector");
        writer.WriteAttributeString("index", index.ToString());

        writer.WriteAttributeString("input", input.ToString());
        writer.WriteAttributeString("childInput", childInput.ToString());
        writer.WriteEndElement();
    }

    //removes the connector
    public override void Remove()
    {
        Object.Destroy(component.gameObject);

        if (parentGate.childInputs.ContainsKey(input))
        parentGate.childInputs[input].connector = -1;

 //       if (parentGate.parentGate.childInputs.ContainsKey(childInput))
//        parentGate.parentGate.childInputs[childInput].connector = -1;

        parentGate.connectors.Remove(connectorNum);
    }
}

public class OutputOutputConnector : Connector
{
    public int childOuput = -1;
    public int output = -1;

    public override bool IsOn
    {
        get { return parentGate.childOutputs[output].IsOn; }
    }

    public override void Save(XmlWriter writer, int index)
    {
        writer.WriteStartElement("outoutconnector");
        writer.WriteAttributeString("index", index.ToString());

        writer.WriteAttributeString("childOutput", childOuput.ToString());
        writer.WriteAttributeString("output", output.ToString());
        writer.WriteEndElement();
    }

    //removes the connector
    public override void Remove()
    {
        Object.Destroy(component.gameObject);

        if(parentGate.parentGate.childOutputs.ContainsKey(childOuput))
            parentGate.parentGate.childOutputs[childOuput].inputConnector = -1;

        if (parentGate.childOutputs.ContainsKey(output))
        {
            if (parentGate.childOutputs[output].connectors.Contains(connectorNum)) { 
                parentGate.childOutputs[output].connectors.Remove(connectorNum);
                }
        }

        parentGate.connectors.Remove(connectorNum);
    }

    public void RemoveInput()
    {
        Object.Destroy(component.gameObject);

        if (parentGate.childOutputs.ContainsKey(output))
        {
            if (parentGate.childOutputs[output].connectors.Contains(connectorNum))
            {
                parentGate.childOutputs[output].connectors.Remove(connectorNum);
            }
        }

        parentGate.connectors.Remove(connectorNum);
    }

}

public class Input
{
    public Gate parentGate;

    public int attachedGate;
    public int connector;

    public int inputNum;

    public bool isOn
    {
        get
        {
            if (connector >= 0)
            {
                return parentGate.connectors[connector].IsOn;
            }
            else if (parentGate.component != null && parentGate.component.inputs.Count > 0)
            {
                return parentGate.component.inputs[inputNum];
            }
            else
            {
                return false;
            }
        }
    }

    public void Save(XmlWriter writer, int index)
    {
        writer.WriteStartElement("input");
        writer.WriteAttributeString("index", index.ToString());

        writer.WriteAttributeString("attachedGate", attachedGate.ToString());
        writer.WriteAttributeString("connector", connector.ToString());
        writer.WriteAttributeString("inputNum", inputNum.ToString());

        writer.WriteEndElement();
    }

    public void Load(XmlNode node, Dictionary<int,int> oldToNewGates)
    {
        attachedGate = oldToNewGates[System.Int32.Parse(node.Attributes["attachedGate"].Value)];
        connector = System.Int32.Parse(node.Attributes["connector"].Value);
        inputNum = System.Int32.Parse(node.Attributes["inputNum"].Value);
    }

    public void Remove()
    {
        // remove connector
        if(connector!=-1)
            parentGate.connectors[connector].Remove();

        parentGate.childInputs.Remove(parentGate.gates[attachedGate].ownInputs[inputNum]);
    }
}

public class Output {
    public Gate parentGate;

    public int attachedGate;
    public List<int> connectors;

    public int outputNum;

    public int inputConnector = -1;

    //bool isOn;

    //public void Update()
    //{
    //    if (GameManager.instance.testing)
    //    {
    //        isOn = IsOnNew;
    //    }
    //    else
    //    {
    //        isOn = false;
    //    }
    //}

    public bool IsOn;

    //public bool IsOnNew;

    public void Save(XmlWriter writer, int index)
    {
        writer.WriteStartElement("output");
        writer.WriteAttributeString("index", index.ToString());

        writer.WriteAttributeString("attachedGate", attachedGate.ToString());
        //writer.WriteAttributeString("connector", connector.ToString());
        writer.WriteAttributeString("outputNum", outputNum.ToString());
        writer.WriteAttributeString("inputConnector", inputConnector.ToString());

        writer.WriteEndElement();
    }

    public void Load(XmlNode node, Dictionary<int, int> oldToNewGates)
    {
        attachedGate = oldToNewGates[System.Int32.Parse(node.Attributes["attachedGate"].Value)];
        connectors = new List<int>();//System.Int32.Parse(node.Attributes["connector"].Value);
        outputNum = System.Int32.Parse(node.Attributes["outputNum"].Value);
        inputConnector = System.Int32.Parse(node.Attributes["inputConnector"].Value);
    }

    public void Remove()
    {
        // remove connector
        foreach (int connector in connectors.ToArray())
        {
            parentGate.connectors[connector].Remove();
        }

        if(inputConnector!=-1)
        {
            if (parentGate.gates[attachedGate].connectors[inputConnector].GetType() == typeof(OutputOutputConnector))
            {
                ((OutputOutputConnector)parentGate.gates[attachedGate].connectors[inputConnector]).RemoveInput();
            }
            else
            {
                parentGate.gates[attachedGate].connectors[inputConnector].Remove();
            }
        }

        parentGate.childOutputs.Remove(parentGate.gates[attachedGate].ownOutputs[outputNum]);
    }
}

public class Gate {

    public static int findLCM(int a, int b) //method for finding LCM with parameters a and b
    {
        int num1, num2;                         //taking input from user by using num1 and num2 variables
        if (a > b)
        {
            num1 = a; num2 = b;
        }
        else
        {
            num1 = b; num2 = a;
        }

        for (int i = 1; i <= num2; i++)
        {
            if ((num1 * i) % num2 == 0)
            {
                return i * num1;
            }
        }
        return num2;
    }

    public GateComponent component;

    public int depth = 0;


    public Dictionary<int, Gate> gates = new Dictionary<int, Gate>();
    public Dictionary<int, Connector> connectors = new Dictionary<int, Connector>();

    public Dictionary<int, Input> childInputs = new Dictionary<int, Input>();
    public Dictionary<int, Output> childOutputs = new Dictionary<int, Output>();

    public Dictionary<int, int> ownInputs = new Dictionary<int, int>();
    public Dictionary<int, int> ownOutputs = new Dictionary<int, int>();

    public Gate parentGate;

    public int gateNum;

    public int AddGate(Gate gate) {

        if (component != null && gate.component != null)
        {
            gate.component.transform.SetParent(this.component.transform);
        }

        int gateNum = 0;
        while (gates.ContainsKey(gateNum))
        {
            gateNum++;
        }
        gates.Add(gateNum, gate);
        gate.gateNum = gateNum;

        gate.depth = depth + 1;

        List<int> keylist = new List<int>(gate.ownInputs.Keys);
        foreach (int key in keylist)
        {
            Input newInput = new Input();
            newInput.attachedGate = gateNum;
            newInput.parentGate = this;

            newInput.connector = -1;

            gate.ownInputs[key] = 0;
            while (childInputs.ContainsKey(gate.ownInputs[key]))
            {
                gate.ownInputs[key]++;
            }

            childInputs.Add(gate.ownInputs[key], newInput);

            newInput.inputNum = key;
        }

        keylist = new List<int>(gate.ownOutputs.Keys);
        foreach (int key in keylist)
        {
            Output newOutput = new Output();
            newOutput.attachedGate = gateNum;
            newOutput.parentGate = this;

            newOutput.connectors = new List<int>();

            gate.ownOutputs[key] = 0;
            while (childOutputs.ContainsKey(gate.ownOutputs[key]))
            {
                gate.ownOutputs[key]++;
            }

            childOutputs.Add(gate.ownOutputs[key], newOutput);

            newOutput.outputNum = key;
        }

        gate.parentGate = this;

        return gateNum;
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
        if (childInputs[inputTo.ownInputs[inputNum]].connector != -1)
        {
            throw new System.ArgumentException("each input can only be connected to one output!");
        }

        int connectorNum = 0;
        while (connectors.ContainsKey(connectorNum))
        {
            connectorNum++;
        }
        connectors.Add(connectorNum, connector);
        connector.connectorNum = connectorNum;

        childOutputs[outputFrom.ownOutputs[outputNum]].connectors.Add(connectorNum);
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
            throw new System.ArgumentException("each output can only be connected to one output!");
        }

        int connectorNum = 0;
        while (connectors.ContainsKey(connectorNum))
        {
            connectorNum++;
        }
        connectors.Add(connectorNum, connector);
        connector.connectorNum = connectorNum;

        childOutputs[outputFrom.ownOutputs[outputNum]].connectors.Add(connectorNum);
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

        int connectorNum = 0;
        while (connectors.ContainsKey(connectorNum))
        {
            connectorNum++;
        }
        connectors.Add(connectorNum, connector);
        connector.connectorNum = connectorNum;


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

        foreach (KeyValuePair<int,Gate> gate in gates)
        {
            gate.Value.Update();
        }

        UpdateOutputs();
    }


    // set isOnNew for each own output
    protected virtual void UpdateOutputs()
    {
        foreach(KeyValuePair<int,int> output in ownOutputs)
        {
            if (parentGate.childOutputs[output.Value].inputConnector >= 0)
            {
                parentGate.childOutputs[output.Value].IsOn = connectors[parentGate.childOutputs[output.Value].inputConnector].IsOn;
                //parentGate.childOutputs[output.Value].Update();
            }

            //parentGate.childOutputs[output.Value].Update();
        }
    }


    public void LateUpdate()
    {
//        foreach (KeyValuePair<int, Output> output in childOutputs)
//        {
////            output.Value.Update();
//        }

        foreach (KeyValuePair<int, Gate> gate in gates)
        {
            gate.Value.LateUpdate();
        }
    }









    public void Save(out string xml)
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        XmlWriterSettings settings = new XmlWriterSettings();
        settings.Indent = false;
        settings.IndentChars = ("\t");
        settings.OmitXmlDeclaration = true;

        //XmlWriter writer = XmlWriter.Create(Application.persistentDataPath + "/" + name + ".xml", settings);
        XmlWriter writer = XmlWriter.Create(sb);
        writer.WriteStartDocument();

        Save(writer,0,0);

        writer.WriteEndDocument();
        writer.Close();

        xml = sb.ToString();

        //Debug.Log("Saved to " + Application.persistentDataPath + "/" + name + ".xml");
    }

    public void Save(XmlWriter writer, int index, int depth)
    {
        writer.WriteStartElement("gate");
        writer.WriteAttributeString("index", index.ToString());
        writer.WriteAttributeString("type", this.GetType().ToString());

        if(depth==1)
        {
            // STORE SPRITENUM HERE
            writer.WriteAttributeString("spritenum", Level.instance.spritenum.ToString());
        }

        int x = 0;
        int y = 0;

        if(component!=null)
        {
            x = (int)component.transform.position.x;
            y = (int)component.transform.position.y;

            if(parentGate!= null && parentGate.component!=null)
            {
                x -= (int)parentGate.component.transform.position.x;
                y -= (int)parentGate.component.transform.position.y;
            }

        }

        writer.WriteAttributeString("x", x.ToString());
        writer.WriteAttributeString("y", y.ToString());


        foreach(KeyValuePair<int,int> input in ownInputs)
        {
            writer.WriteStartElement("ownInput");
            writer.WriteAttributeString("index", input.Key.ToString());
            writer.WriteAttributeString("num", input.Value.ToString());
            writer.WriteEndElement();
        }

        foreach (KeyValuePair<int, int> output in ownOutputs)
        {
            writer.WriteStartElement("ownOutput");
            writer.WriteAttributeString("index", output.Key.ToString());
            writer.WriteAttributeString("num", output.Value.ToString());
            writer.WriteEndElement();
        }

        foreach(KeyValuePair<int, Input> input in childInputs)
        {
            input.Value.Save(writer, input.Key);
        }

        foreach(KeyValuePair<int, Output> output in childOutputs)
        {
            output.Value.Save(writer, output.Key);
        }

        foreach (KeyValuePair<int, Connector> connector in connectors)
        {
            connector.Value.Save(writer, connector.Key);
        }

        foreach(KeyValuePair<int, Gate> gate in gates)
        {
            gate.Value.Save(writer, gate.Key, depth+1);
        }

        writer.WriteEndElement();
    }

    public GateComponent Load(string xml)
    {
        XmlDocument document = new XmlDocument();
        //document.Load(Application.persistentDataPath + "/" + name + ".xml");

        document.LoadXml(xml);

        return Load(document.ChildNodes[document.ChildNodes.Count-1]);
    }

    public GateComponent Load(XmlNode node)
    {
        GateComponent newcomponent = null;
        Dictionary<int, int> oldToNewGates = new Dictionary<int, int>();
        Dictionary<int, int> oldToNewInputs = new Dictionary<int, int>();
        Dictionary<int, int> oldToNewOutputs = new Dictionary<int, int>();

        // first add gates
        foreach (XmlNode child in node.ChildNodes)
        {
            if(child.Name == "gate")
            {
                if(child.Attributes["type"].Value == "Gate")
                {
                    newcomponent = ((GameObject)Object.Instantiate(Resources.Load("empty"))).GetComponent<EmptyGateComponent>();
                
                    if(child.Attributes["spritenum"]!=null)
                    {
                        ((EmptyGateComponent)newcomponent).spritenum = System.Int32.Parse(child.Attributes["spritenum"].Value);
                    }
                } else if(child.Attributes["type"].Value == "NandGate")
                {
                    newcomponent = ((GameObject)Object.Instantiate(Resources.Load("nandgate"))).GetComponent<NAND>();
                }
                else
                {
                    throw new System.FormatException();
                }

                if(newcomponent!=null)
                {
                    int x = System.Int32.Parse(child.Attributes["x"].Value);
                    int y = System.Int32.Parse(child.Attributes["y"].Value);

                    if(this.component!=null)
                    {
                        newcomponent.transform.position = this.component.transform.position + new Vector3(x, y, 0);

                        newcomponent.transform.SetParent(this.component.transform);
                    }
                    else
                    {
                        newcomponent.transform.position = new Vector3(x, y, 0);
                    }
                }

                Gate gate = newcomponent.gate;
                
                int gateNum = 0;
                while (gates.ContainsKey(gateNum))
                {
                    gateNum++;
                }
                gates.Add(gateNum, gate);
                gate.gateNum = gateNum;

                gate.depth = depth + 1;
                gate.parentGate = this;

                oldToNewGates.Add(System.Int32.Parse(child.Attributes["index"].Value), gateNum);

                gate.Load(child);
            }
        }

        // now add inputs and outputs
        foreach(XmlNode child in node.ChildNodes)
        {
            if (child.Name == "input")
            {
                Input input = new Input();
                input.parentGate = this;
                input.Load(child, oldToNewGates);

                int num = 0;
                while (childInputs.ContainsKey(num)) num++;
                childInputs.Add(num, input);
                oldToNewInputs.Add(System.Int32.Parse(child.Attributes["index"].Value),num);
               
            }

            if (child.Name == "output")
            {
                Output output = new Output();
                output.parentGate = this;
                output.Load(child, oldToNewGates);

                int num = 0;
                while (childOutputs.ContainsKey(num)) num++;
                childOutputs.Add(num, output);
                oldToNewOutputs.Add(System.Int32.Parse(child.Attributes["index"].Value), num);
            }

            if(child.Name == "ownInput")
            {
                if(component!=null)
                {
                    component.numInputs++;
                }
            }
            if(child.Name == "ownOutput")
            {
                if(component!=null)
                {
                    component.numOutputs++;
                }
            }
        }

        // now add connectors, and load ownInput/ownOutput
        foreach (XmlNode child in node.ChildNodes)
        {
            if (child.Name == "ininconnector")
            {
                InputInputConnectorComponent component =  ((GameObject)Object.Instantiate(Resources.Load("ininconnector"))).GetComponent<InputInputConnectorComponent>();
                if(this.component!=null)
                    component.transform.SetParent(this.component.transform);
                
                InputInputConnector connector = (InputInputConnector)component.connector;
                connector.parentGate = this;

                connectors.Add(System.Int32.Parse(child.Attributes["index"].Value), connector);
                connector.connectorNum = System.Int32.Parse(child.Attributes["index"].Value);

                connector.input = System.Int32.Parse(child.Attributes["input"].Value);
                connector.childInput = System.Int32.Parse(child.Attributes["childInput"].Value);
            }

            if(child.Name == "inoutconnector")
            {
                InputOutputConnectorComponent component = ((GameObject)Object.Instantiate(Resources.Load("inoutconnector"))).GetComponent<InputOutputConnectorComponent>();
                if (this.component != null)
                    component.transform.SetParent(this.component.transform);

                
                InputOutputConnector connector = (InputOutputConnector)component.connector;
                connector.parentGate = this;

                connectors.Add(System.Int32.Parse(child.Attributes["index"].Value), connector);
                connector.connectorNum = System.Int32.Parse(child.Attributes["index"].Value);


                connector.input = System.Int32.Parse(child.Attributes["input"].Value);
                connector.output = System.Int32.Parse(child.Attributes["output"].Value);
            }

            if(child.Name == "outoutconnector")
            {
                OutputOutputConnectorComponent component = ((GameObject)Object.Instantiate(Resources.Load("outoutconnector"))).GetComponent<OutputOutputConnectorComponent>();
                if (this.component != null)
                    component.transform.SetParent(this.component.transform);

                OutputOutputConnector connector = (OutputOutputConnector)component.connector;
                connector.parentGate = this;

                connectors.Add(System.Int32.Parse(child.Attributes["index"].Value), connector);
                connector.connectorNum = System.Int32.Parse(child.Attributes["index"].Value);


                connector.childOuput = System.Int32.Parse(child.Attributes["childOutput"].Value);
                connector.output = System.Int32.Parse(child.Attributes["output"].Value);
            }

            //update references
            if(child.Name == "gate")
            {
                Gate childGate = gates[oldToNewGates[System.Int32.Parse(child.Attributes["index"].Value)]];


                if(child.Attributes["type"].Value == "Gate")
                {
                    ((EmptyGateComponent)childGate.component).setup();
                }


                foreach(XmlNode childchild in child.ChildNodes)
                {
                    if(childchild.Name == "ownInput")
                    {
                        childGate.ownInputs[System.Int32.Parse(childchild.Attributes["index"].Value)] = oldToNewInputs[System.Int32.Parse(childchild.Attributes["num"].Value)];
                    }
                    if (childchild.Name == "ownOutput")
                    {
                        childGate.ownOutputs[System.Int32.Parse(childchild.Attributes["index"].Value)] = oldToNewOutputs[System.Int32.Parse(childchild.Attributes["num"].Value)];
                    }
                }
            }
        }

        return newcomponent;
    }

    public void AddInput(int num)
    {
        int index = 0;
        while (ownInputs.ContainsKey(index)) index++;
        ownInputs.Add(index, num);
    }

    public void AddOutput(int num)
    {
        int index = 0;
        while (ownOutputs.ContainsKey(index)) index++;
        ownOutputs.Add(index, num);
    }

    public void Remove()
    {
        //recursively remove children
        List<int> keylist = new List<int>(gates.Keys);
        foreach (int key in keylist)
        {
            gates[key].Remove();
        }

        // remove inputs and outputs (which in turn remove connectors)
        foreach (KeyValuePair<int,int> input in ownInputs)
        {
            parentGate.childInputs[input.Value].Remove();
        }
        foreach (KeyValuePair<int, int> output in ownOutputs)
        {
            parentGate.childOutputs[output.Value].Remove();
        }

        Object.Destroy(component.gameObject);

        parentGate.gates.Remove(gateNum);
    }
}


















public abstract class GateComponent : MonoBehaviour
{
    public List<bool> inputs = new List<bool>();

    public List<Vector3> inputoffsets = new List<Vector3>();
    public List<Vector3> outputoffsets = new List<Vector3>();

    public Gate gate;

    public int numInputs;
    public int numOutputs;

    public bool visible = true;
    bool oldVisible;
    public bool showChildren = false;

    public void Update()
    {
        gate.Update();

        if(visible)
        {
            if (GetComponent<Renderer>() != null)
            {
                GetComponent<Renderer>().enabled = true;
            }

            if(GetComponent<Collider2D>()!=null)
            {
                GetComponent<Collider2D>().enabled = true;

                if (GameManager.instance.hitcollider == GetComponent<Collider2D>())
                {
                    GameManager.instance.currentComponent = this;
                    GameManager.instance.current = null;
                }
            }
        }
        else
        {
            if (GetComponent<Renderer>() != null)
            {
                GetComponent<Renderer>().enabled = false;
            }

            if (GetComponent<Collider2D>() != null)
            {
                GetComponent<Collider2D>().enabled = false;
            }
        }

        if (showChildren)
        {
            foreach (GateComponent component in GetComponentsInChildren<GateComponent>())
            {
                if (component == this) continue;

                component.visible = true;
            }

            foreach (ConnectorComponent component in GetComponentsInChildren<ConnectorComponent>())
            {
                component.visible = true;
            }

            foreach (InputOutputCollider component in GetComponentsInChildren<InputOutputCollider>())
            {
                component.visible = true;
            }
        }

        if (visible && !oldVisible)
        {
            foreach (GateComponent component in GetComponentsInChildren<GateComponent>())
            {
                if (component == this) continue;

                component.visible = false;
            }

            foreach (ConnectorComponent component in GetComponentsInChildren<ConnectorComponent>())
            {
                component.visible = false;
            }

            foreach (InputOutputCollider component in GetComponentsInChildren<InputOutputCollider>())
            {
                component.visible = false;
            }
        }

        oldVisible = visible;
    }

    void LateUpdate()
    {
        gate.LateUpdate();
    }
}