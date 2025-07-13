using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "New SpeedBoost Ability", menuName = "Game/Abilities/Speed Boost")]
public class SpeedBoostAbility : Ability
{
    public float speedMultiplier = 2f;
    public float duration = 3f;

    public override void Activate(GameObject owner)
    {
        PlayerMovement movement = owner.GetComponent<PlayerMovement>();
        if (movement != null)
        {
            movement.StartCoroutine(SpeedBoostCoroutine(movement));
        }

    }

    private IEnumerator SpeedBoostCoroutine(PlayerMovement movement)
    {
        float originalSpeed = movement.GetCurrentSpeed();
        movement.SetSpeed(originalSpeed * speedMultiplier);

        yield return new WaitForSeconds(duration);

        if (movement != null)
        {
            movement.SetSpeed(originalSpeed);
        }
    }
}
