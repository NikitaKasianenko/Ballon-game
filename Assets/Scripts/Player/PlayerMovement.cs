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
            Vector2 current = rb.position;
            Vector2 target = inputProvider.targetPosition;
            float dist = Vector2.Distance(current, target);

            if (dist > 0.1)
            {
                Vector2 next = Vector2.MoveTowards(
                    current,
                    target,
                    speed * Time.fixedDeltaTime
                );
                rb.MovePosition(next);
            }
            else
            {
                rb.MovePosition(target);
            }
        }
    }

    public float GetCurrentSpeed() => speed;
    public void SetSpeed(float newSpeed) => speed = newSpeed;
}
