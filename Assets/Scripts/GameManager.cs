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

    [SerializeField] bool isMainMenu;

    bool isTransitioning;
    bool isPaused;
    bool isDead;
    bool didWin;
    public bool IsTransitioning { get { return isTransitioning; } }
    public bool IsPaused { get { return isPaused; } }
    public bool IsDead { get {  return isDead; } }
    public bool DidWin { get { return didWin; } }

    int waveCount;
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
        levelIndex = SceneManager.GetActiveScene().buildIndex;
        isMainMenu = levelIndex == 0;
        //if this isn't the main menu scene
        if(!isMainMenu && PlayerMovement.MouseFollowEnabled)
        {
            _timeScaleDefault = Time.timeScale;

            player = GameObject.FindGameObjectWithTag("Player");
            player.TryGetComponent<ProjectileSpawner>(out shoot);
            player.TryGetComponent<PlayerMovement>(out movement);
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = false;
        }
        else
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
        }
        aud.Play();
    }
    private void OnEnable()
    {
        if(!isMainMenu)
        {
            menu.action.performed += PauseMenu;
        }
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
        if(player != null)
        {
            shoot.enabled = true;
            movement.enabled = true;
        }
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
        if(!PlayerMovement.MouseFollowEnabled)
        {
            PlayerMovement.MouseFollowEnabled = true;
        }
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
    public void LoadFirstScene()
    {
        if(!isMainMenu)
        {
            ResumeState();
        }
        SceneManager.LoadScene(levelIndex + 1);
    }

    public void BossDied()
    {
        WinMenu();
    }

    public void CursorConfined()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void CursorLocked()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
}
