using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("UI References (TMP)")]
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private Button restartButton;

    private float timeSurvived;
    private int health = 2;
    private int score = 0;
    private bool isGameOver;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        Time.timeScale = 1f;

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        if (restartButton != null)
            restartButton.onClick.AddListener(RestartGame);

        UpdateHealthUI();
        UpdateScoreUI();
        UpdateTimerUI();
    }

    void Update()
    {
        if (isGameOver) return;

        timeSurvived += Time.deltaTime;
        UpdateTimerUI();
        UpdateScoreUI();
    }

    public void AddScore(int amount)
    {
        if (isGameOver) return;

        score += amount;
        UpdateScoreUI();
    }

    public void TakeDamage()
    {
        if (isGameOver) return;

        health--;
        UpdateHealthUI();

        if (health <= 0)
            EndGame();
    }

    private void UpdateHealthUI()
    {
        if (healthText != null)
            healthText.text = $"HP: {health}";
    }

    private void UpdateTimerUI()
    {
        if (timerText != null)
            timerText.text = $"Time: {timeSurvived:F1}s";
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
            scoreText.text = $"Score: {Mathf.FloorToInt(timeSurvived) + score}";
    }

    private void EndGame()
    {
        isGameOver = true;
        Time.timeScale = 0f;

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);

            if (restartButton != null)
                EventSystem.current.SetSelectedGameObject(restartButton.gameObject);
        }

        Debug.Log($"GAME OVER — Final Score: {Mathf.FloorToInt(timeSurvived) + score}");
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
