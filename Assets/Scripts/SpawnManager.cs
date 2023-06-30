using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class SpawnManager : MonoBehaviour
{
    public Bullet bulletPrefab;

    public float bulletSpeed;
    public Transform[] spawnPoint;

    private ObjectPool<Bullet> bulletPool;

    private void Start()
    {
        bulletPool = new ObjectPool<Bullet>(
            createFunc: () =>
            {
                var createHat = Instantiate(bulletPrefab);
                createHat.returnPool = bulletPool;
                return createHat;
            },
            actionOnGet: (bullet) => // 오브젝트를 풀에서 가져가는 과정의 함수
            {
                bullet.gameObject.SetActive(true);
            },
            actionOnRelease: (bullet) => // 오브젝트를 풀로 되돌려놓는 함수
            {
                bullet.gameObject.SetActive(false);
            },
            actionOnDestroy: (bullet) => // 오브젝트 풀이 가득 찼을 경우
            {
                Destroy(bullet.gameObject);
            },
            maxSize: 1000
            );

        StartCoroutine(BulletCreative());
    }

    IEnumerator BulletCreative()
    {
        while (true)
        {
            yield return new WaitForSeconds(bulletSpeed);
            CreateBullet();
        }
    }

    private void CreateBullet()
    {
        for(int i = 0; i< spawnPoint.Length;i++)
        {
            var bullet = bulletPool.Get();
            bullet.transform.position = spawnPoint[i].transform.position;
            bullet.transform.rotation = spawnPoint[i].transform.rotation;
        }
    }
}