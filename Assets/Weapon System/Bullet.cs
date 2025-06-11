using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float lifetime = 3f;
    
    private void OnEnable()
    {
        // Reset position and rotation when bullet is enabled
        transform.rotation = Quaternion.identity;
        Invoke(nameof(ReturnToPool), lifetime);
    }
    
    private void Update()
    {
        // Move bullet upward
        transform.Translate(Vector2.up * speed * Time.deltaTime);
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Add your collision logic here
        ReturnToPool();
    }
    
    private void ReturnToPool()
    {
        if (GenericBulletPool.Instance != null)
        {
            GenericBulletPool.Instance.ReturnBullet(gameObject);
        }
    }
    
    private void OnDisable()
    {
        CancelInvoke(nameof(ReturnToPool));
    }
} 