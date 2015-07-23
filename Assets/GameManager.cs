using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

using Vectrosity;

public class GameManager : MonoBehaviour {
    public static GameManager instance;

    VectorLine line;

    public InputOutputCollider current;

    public GateComponent currentComponent;

    InputOutputCollider first;

    public Collider2D hitcollider;

    bool moving;
    GateComponent movingcomp;

    public static string[] gatenames = { "NAND","NOT","AND","OR","NOR","XOR", "XNOR","AND3","NAND3","OR3" };

    public TopComponent topComponent;

    public GameObject testingPanel;

    public int numInputs
    {
        get
        {
            return Level.instance.numInputs;
        }
    }
    public int numOutputs
    {
        get
        {
            return Level.instance.numOutputs;
        }
    }

    public bool testing;

    Vector3 positionRelative;

    Level currentlevel;

    void Start ()
    {
        instance = this;

        Vector2[] linepoints = new Vector2[2];
        line = new VectorLine("line", linepoints, null, 2.0f);
        line.color = Color.black;

        currentlevel = new AndLevel();
    }
	
	void Update () {
        Level.instance.Update();

	    if(UnityEngine.Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (current != null)
            {
                line.points2[0] = UnityEngine.Input.mousePosition;

                first = current;
            }
        }

        if(UnityEngine.Input.GetKeyUp(KeyCode.Mouse0))
        {
            if(current!=null && first!=null)
            {
                // connect two components same level
                if(first.attachedGate.parentGate == current.attachedGate.parentGate)
                {
                    if(first.isInput && !current.isInput)
                    {
                        if (first.attachedGate.parentGate.childInputs[first.attachedGate.ownInputs[first.inputOutputNum]].connector != -1)
                        {
                           first.attachedGate.parentGate.connectors[first.attachedGate.parentGate.childInputs[first.attachedGate.ownInputs[first.inputOutputNum]].connector].Remove();
                        }

                        InputOutputConnectorComponent io1 = ((GameObject)Instantiate(Resources.Load("inoutconnector"))).GetComponent<InputOutputConnectorComponent>();
                        first.attachedGate.parentGate.Connect(current.attachedGate, current.inputOutputNum, first.attachedGate, first.inputOutputNum, (InputOutputConnector)io1.connector);
     
                    }

                    if( !first.isInput && current.isInput)
                    {
                        if (current.attachedGate.parentGate.childInputs[current.attachedGate.ownInputs[current.inputOutputNum]].connector != -1)
                        {
                            current.attachedGate.parentGate.connectors[current.attachedGate.parentGate.childInputs[current.attachedGate.ownInputs[current.inputOutputNum]].connector].Remove();
                        }

                        InputOutputConnectorComponent io1 = ((GameObject)Instantiate(Resources.Load("inoutconnector"))).GetComponent<InputOutputConnectorComponent>();
                        first.attachedGate.parentGate.Connect(first.attachedGate, first.inputOutputNum, current.attachedGate, current.inputOutputNum, (InputOutputConnector)io1.connector);

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
                        if (first.attachedGate.childInputs[current.attachedGate.ownInputs[current.inputOutputNum]].connector != -1)
                        {
                            first.attachedGate.connectors[first.attachedGate.childInputs[current.attachedGate.ownInputs[current.inputOutputNum]].connector].Remove();
                        }

                        InputInputConnectorComponent ii1 = ((GameObject)Instantiate(Resources.Load("ininconnector"))).GetComponent<InputInputConnectorComponent>();
                        first.attachedGate.ConnectInput(first.inputOutputNum, current.attachedGate, current.inputOutputNum, (InputInputConnector)ii1.connector);
                    }

                    // attach output to output
                    if(!first.isInput && !current.isInput)
                    {
                        if (first.attachedGate.parentGate.childOutputs[first.attachedGate.ownOutputs[first.inputOutputNum]].inputConnector != -1)
                        {
                            first.attachedGate.connectors[first.attachedGate.parentGate.childOutputs[first.attachedGate.ownOutputs[first.inputOutputNum]].inputConnector].Remove();
                        }


                        OutputOutputConnectorComponent oo1 = ((GameObject)Instantiate(Resources.Load("outoutconnector"))).GetComponent<OutputOutputConnectorComponent>();
                        first.attachedGate.ConnectOutput(current.attachedGate, current.inputOutputNum, first.inputOutputNum, (OutputOutputConnector)oo1.connector);
                    }
                }
            }

            first = null;
        }

        if(UnityEngine.Input.GetKeyDown(KeyCode.Mouse0))
        {
            if(currentComponent!=null)
            {
                movingcomp = currentComponent;
                positionRelative = currentComponent.transform.position - Camera.main.ScreenToWorldPoint(UnityEngine.Input.mousePosition);
            }
        }
        if(UnityEngine.Input.GetKeyUp(KeyCode.Mouse0))
        {
            if(movingcomp!=null)
            {
                if(EventSystem.current.IsPointerOverGameObject())
                {
                    movingcomp.gate.Remove();
                }
            }

            movingcomp = null;
        }

        if(movingcomp!=null)
        {
            movingcomp.transform.position = positionRelative + Camera.main.ScreenToWorldPoint(UnityEngine.Input.mousePosition);
        }

        if(first!=null)
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
            currentComponent = null;
        }
        hitcollider = hit.collider;
	}

    public void LoadLevel(int num)
    {
        testing = true;
        Application.LoadLevel(num);
    }

    public void Test()
    {
        Level.instance.BeginTest();
    }
}
