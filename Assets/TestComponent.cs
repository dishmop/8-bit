using UnityEngine;
using System.Collections;

public class TestComponent : GateComponent
{
    public bool input1;
    public bool input2;

    GateComponent nand1;
    GateComponent nand2;

    GateComponent nand12;
    GateComponent nand22;


    EmptyGateComponent empty;

    EmptyGateComponent empty1;
    EmptyGateComponent empty2;

    InputOutputConnectorComponent io1;
    InputOutputConnectorComponent io2;

    InputInputConnectorComponent in1;
    InputInputConnectorComponent in2;

    OutputOutputConnectorComponent out1;
    OutputOutputConnectorComponent out2;

    InputOutputConnectorComponent io12;
    InputOutputConnectorComponent io22;

    InputInputConnectorComponent in12;
    InputInputConnectorComponent in22;

    OutputOutputConnectorComponent out12;
    OutputOutputConnectorComponent out22;

    void Start()
    {
        nand1 = ((GameObject)Instantiate(Resources.Load("nandgate"), new Vector3(0, 0), new Quaternion())).GetComponent<GateComponent>();
        nand2 = ((GameObject)Instantiate(Resources.Load("nandgate"), new Vector3(0, -100), new Quaternion())).GetComponent<GateComponent>();

        io1 = ((GameObject)Instantiate(Resources.Load("inoutconnector"))).GetComponent<InputOutputConnectorComponent>();
        io2 = ((GameObject)Instantiate(Resources.Load("inoutconnector"))).GetComponent<InputOutputConnectorComponent>();

        in1 = ((GameObject)Instantiate(Resources.Load("ininconnector"))).GetComponent<InputInputConnectorComponent>();
        in2 = ((GameObject)Instantiate(Resources.Load("ininconnector"))).GetComponent<InputInputConnectorComponent>();

        out1 = ((GameObject)Instantiate(Resources.Load("outoutconnector"))).GetComponent<OutputOutputConnectorComponent>();
        out2 = ((GameObject)Instantiate(Resources.Load("outoutconnector"))).GetComponent<OutputOutputConnectorComponent>();

        empty1 = ((GameObject)Instantiate(Resources.Load("empty"))).GetComponent<EmptyGateComponent>();

        empty1.numInputs = 2;
        empty1.numOutputs = 2;

        empty1.setup();


        empty1.gate.AddGate(nand1.gate);
        empty1.gate.AddGate(nand2.gate);







        empty = ((GameObject)Instantiate(Resources.Load("empty"))).GetComponent<EmptyGateComponent>();
        empty.numInputs = 2;
        empty.numOutputs = 2;
        empty.setup();


        empty.gate.AddGate(empty1.gate);

        gate = new Gate();
        gate.AddGate(empty.gate);
        gate.component = this;





        empty1.gate.ConnectInput(0, nand1.gate, 0, (InputInputConnector)in1.connector);
        empty1.gate.ConnectInput(1, nand2.gate, 1, (InputInputConnector)in2.connector);

        empty1.gate.Connect(nand2.gate, 0, nand1.gate, 1, (InputOutputConnector)io1.connector);
        empty1.gate.Connect(nand1.gate, 0, nand2.gate, 0, (InputOutputConnector)io2.connector);

        empty1.gate.ConnectOutput(nand1.gate, 0, 0, (OutputOutputConnector)out1.connector);
        empty1.gate.ConnectOutput(nand2.gate, 0, 1, (OutputOutputConnector)out2.connector);





        in12 = ((GameObject)Instantiate(Resources.Load("ininconnector"))).GetComponent<InputInputConnectorComponent>();
        in22 = ((GameObject)Instantiate(Resources.Load("ininconnector"))).GetComponent<InputInputConnectorComponent>();

        out12 = ((GameObject)Instantiate(Resources.Load("outoutconnector"))).GetComponent<OutputOutputConnectorComponent>();
        out22 = ((GameObject)Instantiate(Resources.Load("outoutconnector"))).GetComponent<OutputOutputConnectorComponent>();



        empty.gate.ConnectInput(0, empty1.gate, 0, (InputInputConnector)in12.connector);
        empty.gate.ConnectInput(1, empty1.gate, 1, (InputInputConnector)in22.connector);

        empty.gate.ConnectOutput(empty1.gate, 0, 0, (OutputOutputConnector)out12.connector);
        empty.gate.ConnectOutput(empty1.gate, 1, 1, (OutputOutputConnector)out22.connector);





        inputs.Add(input1);
        inputs.Add(input2);
    }

    new void Update()
    {
        if(UnityEngine.Input.GetKeyDown(KeyCode.Space))
        {
            gate.gates[0].Load("testsave");
            gate.Save("thirstest");
        }

        if(UnityEngine.Input.GetKeyDown(KeyCode.S))
        {
            gate.Save("testloadsave");
        }

        inputs[0] = input1;
        inputs[1] = input2;

        base.Update();
    }
}