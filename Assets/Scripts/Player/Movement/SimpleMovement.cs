using UnityEngine;

[CreateAssetMenu(fileName = "Direct Movement Strategy", menuName = "Game/Movement/Direct")]
public class SimpleMovement : MovementStrategy
{
    public override void Move(Rigidbody2D rb, Vector2 pos)
    {
        Vector2 currentPos = rb.position;
        Vector2 toTarget = pos - currentPos;
        float distance = toTarget.magnitude;

        if (distance <= 1f)
        {
            rb.linearVelocity = Vector2.zero;
            rb.position = pos;

        }
        else
        {
            Vector2 moveDir = toTarget / distance;
            rb.MovePosition(currentPos + moveDir * moveSpeed * Time.fixedDeltaTime);
        }
    }
}



