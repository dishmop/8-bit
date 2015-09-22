using UnityEngine;
using System.Collections;

public class InputOutputConnectorComponent : ConnectorComponent
{
    void Awake()
    {
        connector = new InputOutputConnector();
    }

    void Update()
    {
        Input input = connector.parentGate.childInputs[((InputOutputConnector)connector).input];
        Output output = connector.parentGate.childOutputs[((InputOutputConnector)connector).output];

        from = connector.parentGate.gates[output.attachedGate].component.transform.position;
        to = connector.parentGate.gates[input.attachedGate].component.transform.position;

        from = from + connector.parentGate.gates[output.attachedGate].component.outputoffsets[output.outputNum];
        to = to + connector.parentGate.gates[input.attachedGate].component.inputoffsets[input.inputNum];
    }
}
