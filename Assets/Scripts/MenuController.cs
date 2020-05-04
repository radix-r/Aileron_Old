using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{

	// Panels Button Options
	private int selectedNum;
	public Button[] buttons; // use array for ez extendability
    public Button pause;

    // Operating System Input Control
    private string osOptionVersion;
    private string osSubmitVersion;

    // Jitter Control
    private bool  jitterControl; // buttonTest; use descriptive variable names 

    // label indexes of buttons
    private const int START = 0;
    private const int SETTINGS = 1;
    private const int QUIT = 2;
    private const int NUM_OPTIONS = 4;

    private const float  THRESHOLD = .7f;
    private const float  RELEASE_THRESHOLD = .4f;


    // Start is called before the first frame update
    void Start()
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

        // 0 as start index
        selectedNum = 0;

        // Highlight first selected button
        buttons[selectedNum].GetComponent<Image>().color = Color.yellow;

        // Set all other buttons colors to black
        for (int i = 1; i < buttons.Length;i++)
        {
            buttons[i].GetComponent<Image>().color = Color.black;
        }      
    }

	void Update()
	{
        // checking every frame seems wastefull 
        float vertical = Input.GetAxisRaw("Vertical");

        if (vertical * vertical >= THRESHOLD * THRESHOLD && !jitterControl)
		{
            jitterControl = true;
            
		    //Input telling it to go up
			selectedNum = selectedNum - 1 * Mathf.RoundToInt(vertical/Mathf.Abs(vertical)); // use mod operator
            
            //If out of bounds wrap around
            if (selectedNum >= NUM_OPTIONS)
			{
				selectedNum = 0;
                print("setting: "+selectedNum);

			}
            else if (selectedNum < 0)
            {
                selectedNum = NUM_OPTIONS - 1;
                print("setting: "+selectedNum);
            }
            
            print(selectedNum);
            // Set all button colors to black
            for(int i = 0; i < buttons.Length; i++)
            {
                buttons[i].GetComponent<Image>().color = Color.black;
            } 

            // Highlight selected button
            buttons[selectedNum].GetComponent<Image>().color = Color.yellow;
		}

        // Set jittercontrol to false to begin reciving input again.
        if (vertical * vertical <= RELEASE_THRESHOLD * RELEASE_THRESHOLD && jitterControl)
        {
            jitterControl = false;
        }

        // use osSubmitVersion to select buttons using controller.
        if (Input.GetButtonDown(osSubmitVersion))
        {
            buttons[selectedNum].GetComponent<Button>().onClick.Invoke();
        }

        // Use osOptionVersion to bring up options menu.
        if (Input.GetButtonDown(osOptionVersion))
        {
            print("pause");
            pause.GetComponent<Button>().onClick.Invoke();
        }
    }
    


}

