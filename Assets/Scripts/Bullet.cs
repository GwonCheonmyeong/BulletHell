using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Bullet : MonoBehaviour
{
    public IObjectPool<Bullet> returnPool;
    private float speed = 10f;

    private void OnEnable()
    {
        StartCoroutine(DestroyBullet());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerMove>().Hp--;
            returnPool.Release(this);
        }
    }

    IEnumerator DestroyBullet()
    {
        yield return new WaitForSeconds(4f);
        returnPool.Release(this);
    }

    private void Update()
    {
        float moveDistance = speed * Time.deltaTime;
        transform.Translate(Vector3.forward * moveDistance);
    }
}