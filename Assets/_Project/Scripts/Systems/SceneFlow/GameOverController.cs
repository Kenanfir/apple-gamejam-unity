using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverController : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI finalScoreText;
    [SerializeField] private TextMeshProUGUI instructionText;
    
    [Header("Settings")]
    [SerializeField] private string gameSceneName = "Game";
    [SerializeField] private float restartDelay = 1f;
    
    private bool canRestart = false;
    
    void Start()
    {
        // Show final score if available
        if (finalScoreText)
        {
            var runStats = FindObjectOfType<RunStats>();
            if (runStats)
            {
                finalScoreText.text = $"Final Score: {runStats.Score}";
            }
        }
        
        // Show instruction
        if (instructionText)
        {
            instructionText.text = "Press any key to restart...";
        }
        
        // Enable restart after delay
        Invoke(nameof(EnableRestart), restartDelay);
    }
    
    void Update()
    {
        if (canRestart && Input.anyKeyDown)
        {
            RestartGame();
        }
    }
    
    private void EnableRestart()
    {
        canRestart = true;
    }
    
    public void RestartGame()
    {
        SceneManager.LoadScene(gameSceneName);
    }
}
