using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] InputActionReference menu;
    
    [Header("Player")]
    [SerializeField] GameObject player;

    [Header("UI Parts")]
    [SerializeField] GameObject activeMenu;
    [SerializeField] GameObject settingsMenu;
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject loseMenu;
    [SerializeField] GameObject winMenu;

    [Header("Game Audio")]
    [SerializeField] AudioSource aud;
    [SerializeField] AudioClip music;

    bool isTransitioning;
    bool isPaused;
    bool isDead;
    bool didWin;
    public bool IsTransitioning { get { return isTransitioning; } }
    public bool IsPaused { get { return isPaused; } }
    public bool IsDead { get {  return isDead; } }
    public bool DidWin { get { return didWin; } }

    int levelIndex;
    float _timeScaleDefault;

    ProjectileSpawner shoot;
    PlayerMovement movement;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }

    }
    void Start()
    {
        _timeScaleDefault = Time.timeScale;

        player = GameObject.FindGameObjectWithTag("Player");
        player.TryGetComponent<ProjectileSpawner>(out shoot);
        player.TryGetComponent<PlayerMovement>(out movement);
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;

        levelIndex = SceneManager.GetActiveScene().buildIndex;
    }

    private void OnEnable()
    {
        menu.action.performed += PauseMenu;
    }
    public void PauseMenu(InputAction.CallbackContext obj)
    {
        if (!isDead && !didWin)
        {
            isPaused = !isPaused;
            if (isPaused)
            {
                activeMenu = pauseMenu;
                activeMenu.SetActive(true);
                PauseState();
            }
            else
            {
                ResumeState();
            }
        }
        else
        {
            return;
        }
    }
    public void LoseMenu()
    {
        isDead = true;
        if (activeMenu != null)
        {
            activeMenu.SetActive(false);
        }
        activeMenu = loseMenu;
        activeMenu.SetActive(true);
        player.SetActive(false);
        PauseState();
    }
    public void WinMenu()
    {
        didWin = true;
        if (activeMenu != null)
        {
            activeMenu.SetActive(false);
        }
        activeMenu = winMenu;
        activeMenu.SetActive(true);
        PauseState();
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ResumeState()
    {
        if(activeMenu != null)
        {
            activeMenu.SetActive(false);
            activeMenu = null;
        }
        shoot.enabled = true;
        movement.enabled = true;
        isPaused = false;
        Time.timeScale = _timeScaleDefault;
        Cursor.visible = false;
    }

    private void PauseState()
    {
        if(player != null)
        {
            shoot.enabled = false;
            movement.enabled = false;
        }
        isPaused = true;
        Cursor.visible = true;
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void ReloadLevel()
    {
        ResumeState();
        SceneManager.LoadScene(levelIndex);
    }

    public void LoadStartOver()
    {
        if(activeMenu != null)
        {
            activeMenu.SetActive(false);
            activeMenu = null;
        }
        ResumeState();
        SceneManager.LoadScene(0);
    }
}
