using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(PlayerInputHandler))]
[RequireComponent(typeof(PlayerHealth))]
[RequireComponent(typeof(PlayerAbilityHandler))]
public class PlayerController : MonoBehaviour
{
    // Components
    private PlayerMovement movement;
    private PlayerHealth health;
    private PlayerAbilityHandler abilityHandler;
    private IInputProvider inputProvider;
    private BalloonData balloonData;

    void Awake()
    {
        movement = GetComponent<PlayerMovement>();
        health = GetComponent<PlayerHealth>();
        abilityHandler = GetComponent<PlayerAbilityHandler>();
        inputProvider = GetComponent<IInputProvider>();
    }

    private void Start()
    {
        balloonData = DataManager.Instance.CurrentBalloon();
        Initialize(balloonData);
        GameEvents.OnWavesEnd += HandleState;

    }
    private void OnDestroy()
    {
        GameEvents.OnWavesEnd -= HandleState;
    }

    private void HandleState()
    {

        movement.SetSpeed(0);
        if (health.currentHealth <= 0)
        {
            GameEvents.OnPlayerLose?.Invoke(health.currentHealth, health.maxHealth);
            Debug.Log("Player has lost the game!");
            return;
        }

        GameEvents.OnPlayerWin?.Invoke(health.currentHealth, health.maxHealth);
    }

    public void Initialize(BalloonData data)
    {

        Image im = GetComponentInChildren<Image>();
        if (im != null)
        {
            im.sprite = data.balloonSprite;
        }

        movement.Initialize(inputProvider, data.movementStrategy);
        health.Initialize(data.health);
        abilityHandler.Initialize(data.ability, inputProvider);
        Debug.Log($"init player with balloon: {data.balloonSprite.name}");

    }

}