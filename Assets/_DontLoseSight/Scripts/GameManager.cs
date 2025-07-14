using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    private bool isGameOver = false;

    private void Awake()
    {
        // Singleton
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void Victory()
    {
        if (isGameOver) return;
        
        isGameOver = true;
        Debug.Log("🎉 Victoire !");
        // TODO : UI / transition / score
    }

    public void GameOver()
    {
        if (isGameOver) return;
        
        isGameOver = true;
        Debug.Log("💀 Perdu !");
        // TODO : UI / reset
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}