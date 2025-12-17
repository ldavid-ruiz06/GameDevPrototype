using UnityEngine;
using UnityEngine.SceneManagement;

public class CursorController : MonoBehaviour
{
    public static CursorController instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
            UpdateCursor();  // Para el caso inicial
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        UpdateCursor();
    }

    private void UpdateCursor()
    {
        // SOLO desbloquea cuando estamos en WinScreen
        if (SceneManager.GetActiveScene().name == "WinScreen")
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        // No hace nada en SampleScene → deja que PlayerMovement lo maneje
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}