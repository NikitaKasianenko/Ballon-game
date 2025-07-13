using UnityEngine;

public abstract class MovementStrategy : ScriptableObject
{
    public float moveSpeed = 100f;
    public abstract void Move(Rigidbody2D rb, Vector2 pos);

}
