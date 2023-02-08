using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretShoot : MonoBehaviour
{
    private List<GameObject> bulletPool = default;
    private TurretBase ownTurret = default;
    private GameObject SpawnPos = default;
    private GameObject target = default;
    public int poolCount = default;

    private int damage = default;
    private float speed = default;
    private float spawnRate = default;
    // Start is called before the first frame update
    void Awake()
    {
        SpawnPos = gameObject.FindChildObj("BulletSpawnPoint");
        ownTurret = transform.parent.gameObject.GetComponentMust<TurretBase>();
    }
    void Start()
    { 
        bulletPool = new List<GameObject>();
        for(int i = 0 ; i < poolCount; i++)
        {
            GameObject newBullet = Instantiate(ownTurret.bullet, gameObject.transform);
            newBullet.transform.parent = gameObject.transform;
            newBullet.SetActive(false);
            bulletPool.Add(newBullet);
        }
        ownTurret.onChangeTarget += GetTarget;

        damage = ownTurret.Stat.basicDamage;
        speed = ownTurret.Stat.speed;
        spawnRate = ownTurret.Stat.frequency;

        StartCoroutine(ShootLoop());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator ShootLoop()
    {
        while(true)
        {
            yield return new WaitForSeconds(spawnRate);
            Shoot();
        }
    }

    void Shoot()
    {
        if(target == null || target == default)
            return;

        RotateToTarget();
        GameObject newBullet = GetBulletfromPool();
        Transform spawnTransform = SpawnPos.transform;
        newBullet.transform.position = spawnTransform.position;
        newBullet.transform.rotation = spawnTransform.rotation;
        Bullet bc = newBullet.GetComponentMust<Bullet>();
        bc.basicDamage = damage;
        // GFunc.LogWarning($"SpawnTransform.forward {spawnTransform.forward}, speed : {speed}");
        bc.SetVelo(spawnTransform.up * speed);
    }
    GameObject GetBulletfromPool()
    {
        GameObject launchBullet = default;
        foreach(GameObject bullet in bulletPool)
        {
            if(!bullet.activeSelf)
            {
                launchBullet = bullet;
                launchBullet.SetActive(true);
                break;
            }
        }
        return launchBullet;
    }
    void RotateToTarget()
    {
        if(target == null || target == default)
            return;
        Vector2 direction = new Vector2(target.transform.position.x - transform.position.x, target.transform.position.y - transform.position.y);

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion angleAxis = Quaternion.AngleAxis(angle - 90f, Vector3.forward);
        // Quaternion rotation = Quaternion.(transform.rotation, angleAxis, rotateSpeed * Time.deltaTime);
        transform.rotation = angleAxis;
    }
    void GetTarget()
    {
        GFunc.LogWarning($"Target is {ownTurret.Target}");
        target = ownTurret.Target;
    }
}
