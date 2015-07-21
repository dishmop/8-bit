using UnityEngine;
using System.Collections;

public class OutputOutputConnectorComponent : ConnectorComponent
{
    void Awake()
    {
        connector = new OutputOutputConnector();
    }


    void Update()
    {
        Output childoutput = connector.parentGate.parentGate.childOutputs[((OutputOutputConnector)connector).childOuput];
        Output output = connector.parentGate.childOutputs[((OutputOutputConnector)connector).output];

        from = connector.parentGate.gates[output.attachedGate].component.transform.position;

        from = from + connector.parentGate.gates[output.attachedGate].component.outputoffsets[output.outputNum];


        to = connector.parentGate.component.transform.position;

        to = to + connector.parentGate.component.outputoffsets[childoutput.outputNum];
    }
}
