using UnityEngine;

public class QuitOnEsc : MonoBehaviour
{

    public string OnQuitLevelName;

    //public void Start()
    //{
    //    Debug.Log(PlayerPrefs.GetString("HADDER"));
    //}

    // Update is called once per frame
    void Update()
    {


        // Test for exit
        if (UnityEngine.Input.GetKeyDown(KeyCode.Escape))
        {
            if (OnQuitLevelName != null && OnQuitLevelName != "")
            {
                Application.LoadLevel(OnQuitLevelName);
            }
            else
            {
                Quit();
            }
        }
    }

#if UNITY_WEBPLAYER
	public static string webplayerQuitURL = "http://google.com";
#endif
    public void Quit()
    {
#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_WEBPLAYER
		Application.OpenURL(webplayerQuitURL);
#else
        Application.Quit();
#endif
    }
}