using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindFirstObjectByType<GameManager>();
                if (_instance == null)
                    Debug.LogError("No GameManager found in the scene!");
            }
            return _instance;
        }
    }

    public Collectible.CollectibleType currentTarget;
    public int score = 0;

    [Header("UI References")]
    public TMP_Text targetText;
    public TMP_Text timerText;
    public GameObject gameOverPanel;

    [Header("Timer Settings")]
    public float totalTime = 60f;
    private float remainingTime;
    private bool isGameOver = false;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        remainingTime = totalTime;
        ChooseNewTarget();
        UpdateUI();

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
    }

    private void Update()
    {
        if (isGameOver) return;

        remainingTime -= Time.deltaTime;
        if (remainingTime <= 0)
        {
            remainingTime = 0;
            GameOver();
        }

        UpdateUI();
    }

    public void ChooseNewTarget()
    {
        var types = (Collectible.CollectibleType[])System.Enum.GetValues(typeof(Collectible.CollectibleType));
        currentTarget = types[Random.Range(0, types.Length)];
        UpdateUI();
    }

    public void UpdateScore(bool correct)
    {
        if (isGameOver) return;

        score += correct ? 10 : -5;
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (targetText != null)
            targetText.text = $"Target: {currentTarget} | Score: {score}";

        if (timerText != null)
            timerText.text = $"Time: {Mathf.CeilToInt(remainingTime)}";
    }

    public void GameOver()
    {
        if (isGameOver) return;
        isGameOver = true;
        Time.timeScale = 0f;

        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);
    }

    // 🔁 Called when Retry button is clicked
    public void RetryButton()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // ❌ Called when Exit button is clicked
    public void ExitButton()
    {
        Time.timeScale = 1f;

        // If running in the editor
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // If running as a built game
        Application.Quit();
#endif
    }
}