using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SaveButton : MonoBehaviour {
    public InputField text;

    public void OnClick()
    {
        if (text.text != "" && GameManager.instance.topComponent!=null)
            GameManager.instance.topComponent.Save(text.text);
    }
}
