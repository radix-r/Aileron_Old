using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneController : MonoBehaviour
{
    // Next Scene Name
    public int sceneName;
    private int mainMenu = 0;

    private PauseController pauseCall;

    // Panels
    public GameObject MainPanel;
    public GameObject LoadingPanel;
    public GameObject PanelFade;
    public GameObject CreditsPanel;
    public GameObject GameControlsPanel;

    private const int LOADTIME = 3;
    private void Start()
    {
        // Get the pause script from the 
        pauseCall = gameObject.GetComponent<PauseController>();

        if (SceneManager.GetActiveScene().buildIndex == mainMenu)
            MainPanel.SetActive(true);
    }

    // Generic Functions that 
    IEnumerator WaitTransitions(GameObject Panel, int time)
    { 
        yield return new WaitForSeconds(time);
        Panel.SetActive(false);

        SceneManager.LoadScene(sceneName);
    }

    public void Play()
    {
        
        // Tempoaraliy setting the next scene to be 2 so that it loads Ross test city
        sceneName = 1;
        // ******** \\

        LoadingPanel.SetActive(true);
        MainPanel.SetActive(false);
        StartCoroutine(WaitTransitions(LoadingPanel, LOADTIME));
        
    }

    public void MainMenu()
    {
        pauseCall.PauseControl();
        sceneName = mainMenu;
        LoadingPanel.SetActive(true);
        StartCoroutine(WaitTransitions(LoadingPanel, LOADTIME));
    }

    public void LevelsControl()
    {
         
    }

    // This isnt being called anywhere just yet.
    public void LoadNextLevel()
    {
        // Start loading animations
        LoadingPanel.SetActive(true);
        // Set the next level to the + 1 index from the current bulid index.
        sceneName = SceneManager.GetActiveScene().buildIndex + 1;
        StartCoroutine(WaitTransitions(LoadingPanel, LOADTIME));
    }

    

    public void Credits()
    {
        if (CreditsPanel.activeInHierarchy == true)
        {
            MainPanel.SetActive(true);
            CreditsPanel.SetActive(false);
        }
        else
        {
            MainPanel.SetActive(false);
            CreditsPanel.SetActive(true);
        }

    }

    public void GameControls()
    {
        if (GameControlsPanel.activeInHierarchy == true)
        {
            pauseCall.InGamePanel.SetActive(true);
            GameControlsPanel.SetActive(false);
        }
        else
        {
            pauseCall.InGamePanel.SetActive(false);
            GameControlsPanel.SetActive(true);
        }

    }

    public void QuitControl()
    {
        Application.Quit();
    }
   
}
