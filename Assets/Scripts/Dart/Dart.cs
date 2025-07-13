using UnityEngine;

public class Dart : MonoBehaviour
{
    private Rigidbody2D rb;

    public float speed = 10f;
    public int damage = 5;

    private Vector3 direction;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }


    public void Initialize(Vector3 dir)
    {
        rb.gravityScale = 0f;
        direction = dir.normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        rb.linearVelocity = direction * speed;
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("Collided with Player");
            IDamageable damageable = other.GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.TakeDamage(damage);
                Destroy(gameObject);
            }

        }
    }

}
