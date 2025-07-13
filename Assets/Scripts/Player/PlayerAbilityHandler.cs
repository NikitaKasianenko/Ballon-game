using UnityEngine;

public class PlayerAbilityHandler : MonoBehaviour
{

    private Ability currentAbility;
    private IInputProvider inputProvider;
    private float abilityCooldown;


    public void Initialize(Ability ability, IInputProvider ip)
    {
        currentAbility = ability;
        inputProvider = ip;
        abilityCooldown = 0f;
    }

    private void Update()
    {

        if (abilityCooldown > 0)
        {
            abilityCooldown -= Time.deltaTime;
        }

        if (currentAbility != null && inputProvider.abilityRequested)
        {
            UseAbility();
        }
    }

    public bool CanUseAbility()
    {
        return currentAbility != null && abilityCooldown <= 0;
    }

    public void UseAbility()
    {
        if (!CanUseAbility())
        {
            return;
        }

        currentAbility.Activate(this.gameObject);
        abilityCooldown = currentAbility.cooldown;
        Debug.Log($"Used ability: {currentAbility.abilityName}");
    }

}
