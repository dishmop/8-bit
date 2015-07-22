using UnityEngine;
using System.Collections;

using Vectrosity;

public class GameManager : MonoBehaviour {
    public static GameManager instance;

    bool connecting;
    VectorLine line;

    public InputOutputCollider current;

    InputOutputCollider first;

    public Collider2D hitcollider;

    void Start ()
    {
        instance = this;

        Vector2[] linepoints = new Vector2[2];
        line = new VectorLine("line", linepoints, null, 2.0f);
        line.color = Color.black;
    }
	
	// Update is called once per frame
	void Update () {
	    if(UnityEngine.Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (current != null)
            {
                connecting = true;
                line.points2[0] = UnityEngine.Input.mousePosition;

                first = current;
            }
        }

        if(UnityEngine.Input.GetKeyUp(KeyCode.Mouse0))
        {
            connecting = false;

            if(current!=null)
            {
                // connect two components same level
                if(first.attachedGate.parentGate == current.attachedGate.parentGate)
                {
                    if(first.isInput && !current.isInput)
                    {
                        if (first.attachedGate.parentGate.childInputs[first.attachedGate.ownInputs[first.inputOutputNum]].connector == -1)
                        {
                            InputOutputConnectorComponent io1 = ((GameObject)Instantiate(Resources.Load("inoutconnector"))).GetComponent<InputOutputConnectorComponent>();
                            first.attachedGate.parentGate.Connect(current.attachedGate, current.inputOutputNum, first.attachedGate, first.inputOutputNum, (InputOutputConnector)io1.connector);
                        }
                    }

                    if( !first.isInput && current.isInput)
                    {
                        if (current.attachedGate.parentGate.childInputs[current.attachedGate.ownInputs[current.inputOutputNum]].connector == -1)
                        {
                            InputOutputConnectorComponent io1 = ((GameObject)Instantiate(Resources.Load("inoutconnector"))).GetComponent<InputOutputConnectorComponent>();
                            first.attachedGate.parentGate.Connect(first.attachedGate, first.inputOutputNum, current.attachedGate, current.inputOutputNum, (InputOutputConnector)io1.connector);
                        }
                    }
                }

                // swap them round to save code
                if(first.attachedGate.parentGate == current.attachedGate)
                {
                    InputOutputCollider temp = current;
                    current = first;
                    first = temp;
                }

                if(first.attachedGate == current.attachedGate.parentGate)
                {
                    // attach input to input
                    if(first.isInput && current.isInput)
                    {
                        if (first.attachedGate.childInputs[current.attachedGate.ownInputs[current.inputOutputNum]].connector == -1)
                        {
                            InputInputConnectorComponent ii1 = ((GameObject)Instantiate(Resources.Load("ininconnector"))).GetComponent<InputInputConnectorComponent>();
                            first.attachedGate.ConnectInput(first.inputOutputNum, current.attachedGate, current.inputOutputNum, (InputInputConnector)ii1.connector);
                        }
                    }

                    // attach output to output
                    if(!first.isInput && !current.isInput)
                    {
                        if (first.attachedGate.parentGate.childOutputs[first.attachedGate.ownOutputs[first.inputOutputNum]].inputConnector == -1)
                        {
                            OutputOutputConnectorComponent oo1 = ((GameObject)Instantiate(Resources.Load("outoutconnector"))).GetComponent<OutputOutputConnectorComponent>();
                            first.attachedGate.ConnectOutput(current.attachedGate, current.inputOutputNum, first.inputOutputNum, (OutputOutputConnector)oo1.connector);
                        }
                    }
                }
            }
        }

        if(connecting)
        {
            line.active = true;

            line.points2[1] = UnityEngine.Input.mousePosition;

            line.Draw();
        }
        else
        {
            line.active = false;
        }

        Ray ray = Camera.main.ScreenPointToRay(UnityEngine.Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);

        if(hit.collider == null)
        {
            current = null;
        }
        hitcollider = hit.collider;
	}
}
