using UnityEngine;

[CreateAssetMenu(fileName = "Direct Movement Strategy", menuName = "Game/Movement/Direct")]
public class SimpleMovement : MovementStrategy
{
    public override void Move(Rigidbody2D rb, Vector2 pos)
    {
        Vector2 currentPos = rb.position;
        Vector2 toTarget = pos - currentPos;
        float distance = toTarget.magnitude;

        if (distance > 0.1)
        {
            Vector2 next = Vector2.MoveTowards(
                 currentPos,
                 pos,
                 moveSpeed * Time.fixedDeltaTime
             );
            rb.MovePosition(next);

        }
        else
        {
            rb.MovePosition(pos);
        }
    }
}



