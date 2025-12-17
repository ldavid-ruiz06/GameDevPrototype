using UnityEngine;
using UnityEngine.SceneManagement;

public class GameModeManager : MonoBehaviour
{
    void Start()
    {
        AlienManager.instance.onChange.AddListener(CheckWinCondition);
    }
    
    void CheckWinCondition()
    {
        if (AlienManager.instance.aliens.Count <= 0)
        {
            SceneManager.LoadScene("WinScreen");
        }
    }
}
