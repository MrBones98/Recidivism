using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Slider _healthBar;
    [SerializeField] private Slider _staminaBar;
    [SerializeField] private Slider _bossHealthBar;
    [SerializeField] private int _maxHealth;
    [SerializeField] private int _maxBossHealth;
    [SerializeField] private int _maxStamina;
    [SerializeField] private GameObject _deathScreen;
    [SerializeField] private GameObject _winScreen;
    [SerializeField] private GameObject _exitPrompt;
    [SerializeField] private GameObject _exit;

    private WaitForSeconds _regenTick = new WaitForSeconds(0.01f);
    private Coroutine _regeneration;
    private int _currentHealth;
    private int _currentBossHealth;
    private int _currentStamina;

    public static UIManager Instance;
    private void Awake()
    {
        BossTrigger.onBossFightTriggered += OnBossFightTriggered;
        BossBase.onBossFightWon += OnBossFightWon;
        PlayerBase.onPlayerDeath += OnPlayerDeath;
        Instance = this;
    }

    private void OnPlayerDeath()
    {
        _healthBar.gameObject.SetActive(false);
        _staminaBar.gameObject.SetActive(false);
        StartCoroutine(PlayerDeathTimer());
        _deathScreen.gameObject.SetActive(true);
    }
    private IEnumerator PlayerDeathTimer()
    {
        yield return new WaitForSeconds(3.0f);
    }
    private void OnBossFightWon()
    {
        _bossHealthBar.gameObject.SetActive(false);
        StartCoroutine(WinScreenCounter());
    }

    private IEnumerator WinScreenCounter()
    {
        yield return new WaitForSeconds(3.0f);
        _winScreen.SetActive(true);
        yield return new WaitForSeconds(2.0f);
        _winScreen.SetActive(false);
        _exitPrompt.SetActive(true);
        yield return new WaitForSeconds(2.0f);
        _exit.SetActive(true);
    }

    private void OnBossFightTriggered()
    {
        _bossHealthBar.gameObject.SetActive(true);
        _currentBossHealth = _maxBossHealth;
        _bossHealthBar.maxValue = _maxBossHealth;
        _bossHealthBar.value = _maxBossHealth;
    }

    private void Start()
    {
        _currentStamina = _maxStamina;
        _currentHealth = _maxHealth;
        _staminaBar.maxValue = _maxStamina;
        _staminaBar.value = _maxStamina;
        _healthBar.maxValue = _maxHealth;
        _healthBar.value = _maxHealth;
    }
    public void SetBossHealth(float health)
    {
        _bossHealthBar.value = health;
    }
    public void SetPlayerHealth(float health)
    {
        _healthBar.value = health;
    }
    public bool UseStamina(int usageAmount, float cooldown)
    {
        if (_currentStamina - usageAmount >= 0)
        {
            _currentStamina -= usageAmount;
            _staminaBar.value = _currentStamina;

            if(_regeneration != null)
                StopCoroutine(_regeneration);

            _regeneration = StartCoroutine(RegenStamina(cooldown));
            return true;
        }
        else
            return false;
    }
    private IEnumerator RegenStamina(float staminaRegenCooldown)
    {
        yield return new WaitForSeconds(staminaRegenCooldown);

        while(_currentStamina < _maxStamina)
        {
            _currentStamina += _maxStamina / 100;
            _staminaBar.value = _currentStamina;
            yield return _regenTick;
        }
        _regeneration = null;
    }
    private void OnDestroy()
    {
        BossTrigger.onBossFightTriggered -= OnBossFightTriggered;
        BossBase.onBossFightWon -= OnBossFightWon;
        PlayerBase.onPlayerDeath -= OnPlayerDeath;
    }
}
