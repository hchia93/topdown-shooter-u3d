using UnityEngine;
using System.Collections.Generic;

public class BulletPool : MonoBehaviour
{
    public static BulletPool Instance { get; private set; }
    
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private int poolSize = 20;
    
    private Queue<GameObject> bulletPool;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        
        InitializePool();
    }
    
    private void InitializePool()
    {
        bulletPool = new Queue<GameObject>();
        
        for (int i = 0; i < poolSize; i++)
        {
            CreateNewBullet();
        }
    }
    
    private void CreateNewBullet()
    {
        GameObject bullet = Instantiate(bulletPrefab, transform);
        bullet.SetActive(false);
        bulletPool.Enqueue(bullet);
    }
    
    public GameObject GetBullet()
    {
        if (bulletPool.Count == 0)
        {
            CreateNewBullet();
        }
        
        GameObject bullet = bulletPool.Dequeue();
        bullet.SetActive(true);
        return bullet;
    }
    
    public void ReturnBullet(GameObject bullet)
    {
        bullet.SetActive(false);
        bullet.transform.SetParent(transform);
        bulletPool.Enqueue(bullet);
    }
} 