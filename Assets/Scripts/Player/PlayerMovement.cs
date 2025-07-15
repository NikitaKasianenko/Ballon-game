using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private IInputProvider inputProvider;
    private MovementStrategy movementStrategy;
    private float speed;

    public void Initialize(IInputProvider input, MovementStrategy strg)
    {
        inputProvider = input;
        movementStrategy = strg;
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        speed = movementStrategy.moveSpeed;
    }


    private void FixedUpdate()
    {
        if (inputProvider == null || movementStrategy == null)
        {
            Debug.LogWarning("PlayerMovement not initialized");
            return;
        }

        if (inputProvider.holding)
        {
            movementStrategy.Move(rb, inputProvider.targetPosition);
        }
    }

    public float GetCurrentSpeed() => speed;
    public void SetSpeed(float newSpeed) => speed = newSpeed;
}
