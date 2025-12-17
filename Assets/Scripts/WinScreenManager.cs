using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinScreenManager : MonoBehaviour
{
    [SerializeField] Button playAgainButton;

    void Start()
    {
        playAgainButton.onClick.AddListener(RestartGame);
    }
    
    void RestartGame()
    {
        

        SceneManager.LoadScene("ForestLevel");  // Carga tu escena principal 

        
    }

}