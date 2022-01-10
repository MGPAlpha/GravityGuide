using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public static MenuManager _mm {
        get;
        private set;
    }

    public LoadScreen loadScreen;

    private string waitLoadName = null;
    private int waitLoadIndex = 0;
    
    private void Awake()
    {
        if (_mm == null || _mm.gameObject == null) {
            _mm = this;
        } else {
            Destroy(this);
        }
    }

    [SerializeField] private bool countLevelAsSave = true;
    
    [SerializeField] private Button continueButton;

    [SerializeField] private bool pauseStopsTime = true;

    public void ReadyToLoad() {
        if (waitLoadName != null) {
            SceneManager.LoadSceneAsync(waitLoadName);
        } else {
            SceneManager.LoadSceneAsync(waitLoadIndex);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (countLevelAsSave) {
            if (SceneManager.GetActiveScene().buildIndex > PlayerPrefs.GetInt("levelProgress")) {
                PlayerPrefs.SetInt("levelProgress", SceneManager.GetActiveScene().buildIndex);
            }
            if (SceneManager.GetActiveScene().buildIndex > PlayerPrefs.GetInt("maxLevelProgress")) {
                PlayerPrefs.SetInt("maxLevelProgress", SceneManager.GetActiveScene().buildIndex);
            }
        }
        if (continueButton && PlayerPrefs.GetInt("levelProgress") < 1) {
            continueButton.interactable = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (canPause) {
            if (Input.GetKeyDown(KeyCode.Escape)) {
                Pause();
            }
        }
    }

    public void Play() {

        //Put this in whenever you want to load a scene
        SpeedrunTimer.ResetSpeedrunTimer();
        PlayerPrefs.SetInt("levelProgress", 1);
        Time.timeScale = 1;
        if (!loadScreen) SceneManager.LoadScene(1);
        else {
            waitLoadIndex = 1;
            loadScreen.FillAndTriggerMenu();
        }
    }

    public void Continue() {
        if (PlayerPrefs.GetInt("levelProgress") > 0) {
            Time.timeScale = 1;
            int loadIndex = PlayerPrefs.GetInt("levelProgress");
            if (!loadScreen) SceneManager.LoadScene(loadIndex);
            else {
                waitLoadIndex = loadIndex;
                loadScreen.FillAndTriggerMenu();
            }
        }
    }

    public void Quit() {
        Application.Quit();
    }

    public void Menu() {
        Time.timeScale = 1;
        if (SpeedrunTimer._st) {
            SpeedrunTimer._st.ActivateTimer(false);
        }
        if (!loadScreen) SceneManager.LoadScene("Title");
        else {
            waitLoadName = "Title";
            loadScreen.FillAndTriggerMenu();
            if (pauseMenu) pauseMenu.SetActive(false);
        }
    }

    public void Reset() {
        Time.timeScale = 1;
        if (SpeedrunTimer._st) {
            SpeedrunTimer._st.ActivateTimer(false);
        }
        int loadIndex = SceneManager.GetActiveScene().buildIndex;
        if (!loadScreen) SceneManager.LoadScene(loadIndex);
        else {
            waitLoadIndex = loadIndex;
            loadScreen.FillAndTriggerMenu();
            if (pauseMenu) pauseMenu.SetActive(false);
        }
    }

    public void ContinueFromSave() {
        Time.timeScale = 1;
        SceneManager.LoadScene(PlayerPrefs.GetInt("levelProgress"));
    }

    public void Credits() {
        Time.timeScale = 1;
        if (!loadScreen) SceneManager.LoadScene("Credits");
        else {
            waitLoadName = "Credits";
            loadScreen.FillAndTriggerMenu();
        }
    }

    public bool paused {
        get;
        private set;
    } = false;
    private float lastTimeScale = 0;

    [SerializeField] private bool canPause = false;
    [SerializeField] GameObject pauseMenu;

    public void Pause() {
        if (!paused) {
            lastTimeScale = Time.timeScale;
            if (pauseStopsTime) Time.timeScale = 0;
            paused = true;
            if (pauseMenu) pauseMenu.SetActive(true);
        } else {
            if (pauseStopsTime) Time.timeScale = 1;
            paused = false;
            if (pauseMenu) pauseMenu.SetActive(false);
        }
    }

    public void Resume() {
        if (paused) Pause();
    }
}
