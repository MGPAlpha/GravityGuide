using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(PlayerPrefs.GetInt("levelProgress"));
        if (SceneManager.GetActiveScene().buildIndex > PlayerPrefs.GetInt("levelProgress")) {
            PlayerPrefs.SetInt("levelProgress", SceneManager.GetActiveScene().buildIndex);
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
    Time.timeScale = 1;
    }

    public void Continue() {
        if (PlayerPrefs.GetInt("levelProgress") > 0);
        Time.timeScale = 1;
        SceneManager.LoadScene(PlayerPrefs.GetInt("levelProgress"));
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
            Time.timeScale = 0;
            paused = true;
            if (pauseMenu) pauseMenu.SetActive(true);
        } else {
            Time.timeScale = lastTimeScale;
            paused = false;
            if (pauseMenu) pauseMenu.SetActive(false);
        }
    }

    public void Resume() {
        if (paused) Pause();
    }
}
