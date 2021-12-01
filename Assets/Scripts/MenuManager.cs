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
    
    private void Awake()
    {
        if (_mm == null || _mm.gameObject == null) {
            _mm = this;
        } else {
            Destroy(this);
        }
    }
    
    [SerializeField] private Button continueButton;

    [SerializeField] private bool pauseStopsTime = true;

    // Start is called before the first frame update
    void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex > PlayerPrefs.GetInt("levelProgress")) {
            PlayerPrefs.SetInt("levelProgress", SceneManager.GetActiveScene().buildIndex);
        }
        if (SceneManager.GetActiveScene().buildIndex > PlayerPrefs.GetInt("maxLevelProgress")) {
            PlayerPrefs.SetInt("maxLevelProgress", SceneManager.GetActiveScene().buildIndex);
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
        SceneManager.LoadScene(1);
        PlayerPrefs.SetInt("levelProgress", 1);
        Time.timeScale = 1;
    }

    public void Continue() {
        if (PlayerPrefs.GetInt("levelProgress") > 0) {
            Time.timeScale = 1;
            SceneManager.LoadScene(PlayerPrefs.GetInt("levelProgress"));
        }
    }

    public void Quit() {
        Application.Quit();
    }

    public void Menu() {
        Time.timeScale = 1;
        SceneManager.LoadScene("Title");
    }

    public void Reset() {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ContinueFromSave() {
        Time.timeScale = 1;
        SceneManager.LoadScene(PlayerPrefs.GetInt("levelProgress"));
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
