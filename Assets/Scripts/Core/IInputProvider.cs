using UnityEngine;

public interface IInputProvider
{
    Vector2 targetPosition { get; }
    bool abilityRequested { get; }
    bool holding { get; }
}
