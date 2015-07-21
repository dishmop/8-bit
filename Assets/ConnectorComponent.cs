using UnityEngine;
using System.Collections;
using Vectrosity;

public abstract class ConnectorComponent : MonoBehaviour {
    public  Connector connector;

    protected Vector3 from;
    protected Vector3 to;

    VectorLine line;

    void Start()
    {
        Vector3[] linepoints = new Vector3[30];
        line = new VectorLine("line", linepoints, null, 2.0f);
        line.color = Color.black;
    }

    void LateUpdate()
    {
        float offset = 0;

        if(from.x > to.x)
        {
            offset = 50;
        }
        else
        {
            offset = (to.x - from.x) / 2;
        }

        Vector3 control1 = from + new Vector3(offset, 0);
        Vector3 control2 = to + new Vector3(-offset, 0);

        line.MakeCurve(from, control1, to, control2);

        if(connector.IsOn)
        {
            line.color = Color.green;
        }
        else
        {
            line.color = Color.black;
        }

        line.Draw();
    }
}
