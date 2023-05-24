using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class SpawnManager : MonoBehaviour
{
    public Bullet bulletPrefab;

    public Transform[] spawnPoint;

    private ObjectPool<Bullet> bulletPool;

    private void Start()
    {
        bulletPool = new ObjectPool<Bullet>(
            createFunc: () =>
            {
                return Instantiate(bulletPrefab);
            },
            actionOnGet: (bullet) => // ������Ʈ�� Ǯ���� �������� ������ �Լ�
            {
                bullet.gameObject.SetActive(true);
            },
            actionOnRelease: (bullet) => // ������Ʈ�� Ǯ�� �ǵ������� �Լ�
            {
                bullet.gameObject.SetActive(false);
            },
            actionOnDestroy: (bullet) => // ������Ʈ Ǯ�� ���� á�� ���
            {
                Destroy(bullet.gameObject);
            },
            maxSize: 100
            );
    }

    private void CreateBullet()
    {
        int ran = Random.Range(0, spawnPoint.Length);

        var bullet = bulletPool.Get();
        bullet.transform.position = spawnPoint[ran].transform.position;
        bullet.transform.rotation = spawnPoint[ran].transform.rotation;
    }

    private void ReleaseBullet(Bullet bullet)
    {
        bulletPool.Release(bullet);
    }
}
