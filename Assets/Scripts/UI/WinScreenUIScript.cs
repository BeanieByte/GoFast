using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class WinScreenUIScript : MonoBehaviour
{
    private int _collectedCoins;
    private int _finalScoreCoinMultiplier = 10;
    private int _killedEnemies;
    private int _finalScoreEnemiesMultiplier = 50;
    private double _recommendedTime;
    private double _timeTaken;
    private string _timeTakenText;
    private int _finalScoreUnderRecommendedTimeMultiplier = 100;
    private float _finalTimeConsideredForScore;
    [SerializeField] private Transform _victoryScreen;
    [SerializeField] private Transform _finalScoreTextGameObject;
    private TextMeshProUGUI _finalScoreText;
    [SerializeField] private Transform _speedRunTimeTextGameObject;
    private TextMeshProUGUI _speedrunTimeText;
    private int _finalScore;
    [SerializeField] private GameObject _winScreenFirstButton;

    private void Awake()
    {
        _finalScoreText = _finalScoreTextGameObject.GetComponent<TextMeshProUGUI>();
        _speedrunTimeText = _speedRunTimeTextGameObject.GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        if (_victoryScreen.gameObject.activeInHierarchy)
        {
            _victoryScreen.gameObject.SetActive(false);
        }

        _collectedCoins = 0;
        _killedEnemies = 0;

        _finalTimeConsideredForScore = 0;

        _finalScore = 0;

        TimerManager.Instance.OnRecommendedTimeChanged += Instance_OnRecommendedTimeChanged;
        TimerManager.Instance.OnTimerChanged += Instance_OnTimerChanged;

        CoinManager.Instance.OnCoinCollected += Instance_OnCoinCollected;
        EnemyManager.Instance.OnEnemyKilled += Instance_OnEnemyKilled;

        GameManager.Instance.OnGameWon += Instance_OnGameWon;
    }

    private void Instance_OnGameWon(object sender, System.EventArgs e)
    {
        PlayVictoryScreen();
    }

    private void Instance_OnTimerChanged(object sender, TimerManager.OnTimerChangedEventArgs e)
    {
        _timeTaken = e.timeTaken;
        _timeTakenText = e.currentTimeText.Replace(",", ":");
    }

    private void Instance_OnRecommendedTimeChanged(object sender, TimerManager.OnRecommendedTimeChangedEventArgs e)
    {
        _recommendedTime = e.recommendedTime;
    }

    private void Instance_OnEnemyKilled(object sender, EnemyManager.OnEnemyKilledEventArgs e)
    {
        _killedEnemies = e.totalKilledEnemies;
    }

    private void Instance_OnCoinCollected(object sender, CoinManager.OnCoinCollectedEventArgs e)
    {
        _collectedCoins = e.collectedCoins;
    }

    private void PlayVictoryScreen()
    {
        if (!_victoryScreen.gameObject.activeInHierarchy)
        {
            _victoryScreen.gameObject.SetActive(true);
        }

        _finalTimeConsideredForScore = (float)(_recommendedTime - _timeTaken);

        if (_finalTimeConsideredForScore <= 0)
        {
            _finalTimeConsideredForScore = 0;
        }

        _finalScore = Mathf.CeilToInt((_collectedCoins * _finalScoreCoinMultiplier) + (_killedEnemies * _finalScoreEnemiesMultiplier) + (_finalTimeConsideredForScore * _finalScoreUnderRecommendedTimeMultiplier));

        _finalScoreText.text = _finalScore.ToString();

        _speedrunTimeText.text = _timeTakenText;

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(_winScreenFirstButton);
    }

    private void OnDestroy() {
        TimerManager.Instance.OnRecommendedTimeChanged -= Instance_OnRecommendedTimeChanged;
        TimerManager.Instance.OnTimerChanged -= Instance_OnTimerChanged;

        CoinManager.Instance.OnCoinCollected -= Instance_OnCoinCollected;
        EnemyManager.Instance.OnEnemyKilled -= Instance_OnEnemyKilled;

        GameManager.Instance.OnGameWon -= Instance_OnGameWon;
    }
}
