using UnityEngine;
using UnityEngine.SceneManagement;

public class GameModeManager : MonoBehaviour
{
    // public vars
    [SerializeField]
    public GameObject player;
    [SerializeField]
    PlayerBodyManager playerBody;
    public AlienManager alienManager;
    

    
    void Start()
    {
        AlienManager.instance.onChange.AddListener(CheckWinCondition);
    }

    void CheckWinCondition()
    {
        if(AlienManager.instance.aliens.Count <= 0)
        {
            print("You win!");
            SceneManager.LoadScene("Scene/WinScree");
        }
    }

}
