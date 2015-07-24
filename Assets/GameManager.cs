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

    public static Level[] gatelevels = { new NandLevel(), new NotLevel(), new AndLevel(), new OrLevel(), new NorLevel(), new XorLevel(), new XnorLevel(), new And3Level(), new Or3Level(), new Nand3Level(), new SRLevel(), new SRGatedLevel() };

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

    void Start ()
    {
        instance = this;

        Vector3[] linepoints = new Vector3[2];
        line = new VectorLine("line", linepoints, null, 2.0f);
        line.color = Color.green;

        if(!System.IO.File.Exists(Application.persistentDataPath + "/NAND.xml"))
        {
            // lolololol
            string[] lines = {
"<gate index=\"0\" type=\"Gate\" x=\"0\" y=\"0\">",
	"<input index=\"0\" attachedGate=\"0\" connector=\"-1\" inputNum=\"0\" />",
	"<input index=\"1\" attachedGate=\"0\" connector=\"-1\" inputNum=\"1\" />",
	"<output index=\"0\" attachedGate=\"0\" connector=\"-1\" outputNum=\"0\" inputConnector=\"2\" />",
	"<gate index=\"0\" type=\"Gate\" x=\"0\" y=\"0\" spritenum=\"0\">",
		"<ownInput index=\"0\" num=\"0\" />",
		"<ownInput index=\"1\" num=\"1\" />",
		"<ownOutput index=\"0\" num=\"0\" />",
		"<input index=\"0\" attachedGate=\"0\" connector=\"0\" inputNum=\"0\" />",
		"<input index=\"1\" attachedGate=\"0\" connector=\"1\" inputNum=\"1\" />",
		"<output index=\"0\" attachedGate=\"0\" connector=\"2\" outputNum=\"0\" inputConnector=\"-1\" />",
		"<ininconnector index=\"0\" input=\"0\" childInput=\"0\" />",
		"<ininconnector index=\"1\" input=\"1\" childInput=\"1\" />",
		"<outoutconnector index=\"2\" childOutput=\"0\" output=\"0\" />",
		"<gate index=\"0\" type=\"NandGate\" x=\"0\" y=\"30\">",
			"<ownInput index=\"0\" num=\"0\" />",
			"<ownInput index=\"1\" num=\"1\" />",
			"<ownOutput index=\"0\" num=\"0\" />",
		"</gate>",
	"</gate>",
"</gate>"};
            System.IO.File.WriteAllLines(Application.persistentDataPath + "/NAND.xml", lines);
        }
    }
	
	void Update () {
        if(Level.instance!=null)
        Level.instance.Update();

	    if(UnityEngine.Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (current != null)
            {
                line.points3[0] = current.transform.position;

                first = current;

                ToolTip.instance.Click1();
            }
        }

        if(UnityEngine.Input.GetKeyUp(KeyCode.Mouse0))
        {
            if(current!=null && first!=null)
            {
                ToolTip.instance.Click2();

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

            line.points3[1] = Camera.main.ScreenToWorldPoint(UnityEngine.Input.mousePosition);

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

        VectorLine.canvas.sortingOrder = -1;
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
