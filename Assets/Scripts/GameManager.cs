using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    void Start()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }

        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
    }
    private void Update()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
    }
}
