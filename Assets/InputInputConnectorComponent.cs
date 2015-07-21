using UnityEngine;
using System.Collections;

public class InputInputConnectorComponent : ConnectorComponent
{
    void Awake()
    {
        connector = new InputInputConnector();
    }

    void Update()
    {
        Input input = connector.parentGate.childInputs[((InputInputConnector)connector).input];
        Input childinput = connector.parentGate.parentGate.childInputs[((InputInputConnector)connector).childInput];


        from = connector.parentGate.component.transform.position;

        from = from + connector.parentGate.component.inputoffsets[childinput.inputNum];

        to = connector.parentGate.gates[input.attachedGate].component.transform.position;
        to = to + connector.parentGate.gates[input.attachedGate].component.inputoffsets[input.inputNum];
    }
}
