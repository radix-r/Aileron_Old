using UnityEngine;
using UnityEngine.UI;

public class PauseController : MonoBehaviour{
    public Button pauseButton;

    // Panels
    public GameObject InGamePanel;
    public GameObject PausePanel;

    // Operating System Input Control
    private string osOptionVersion;
    private string osSubmitVersion;

    private void Start()
    {
        // Check which input to run based on OS version and set inputVersion.
        // Windows player and editor
        if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor)
        {
            osOptionVersion = "WindowsOptions";
            osSubmitVersion = "WindowsSubmit";
        }
        // MacOS player and editor
        if (Application.platform == RuntimePlatform.OSXPlayer || Application.platform == RuntimePlatform.OSXEditor)
        {
            osOptionVersion = "MacOptions";
            osSubmitVersion = "MacSubmit";
        }
        // Linux player and editor
        if (Application.platform == RuntimePlatform.LinuxPlayer || Application.platform == RuntimePlatform.LinuxEditor)
        {
            osOptionVersion = "LinuxOptions";
            osSubmitVersion = "LinuxSubmit";
        }
    }

    void Update()
    { 
        // use osOptionVersion to bring up or close in-game menu
        if (Input.GetButtonDown(osOptionVersion))
        {
            print("pause");
            PauseControl();
        }
    }

    public void PauseControl()
    {
        if (Time.timeScale == 1.0f)
        {
            PausePanel.SetActive(false);
            InGamePanel.SetActive(true);
            Time.timeScale = 0;
        }
        else if (Time.timeScale == 0)
        {
            PausePanel.SetActive(true);
            InGamePanel.SetActive(false);
            Time.timeScale = 1;

        }
    }
}