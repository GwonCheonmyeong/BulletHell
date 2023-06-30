using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum State
{
    Base = 0, // 1자로 던지는거
    Spin,
    LookPlayer,
}

public class Tower : MonoBehaviour
{
    public GameObject target;
    public State state;
    public float spinAngle;

    public void Awake()
    {
        target = GameObject.FindWithTag("Player");
    }
    public void Update()
    {
        switch((int)state)
        {
            case 0:                break;
            case 1:
                Spin();
                break;
            default:
                LookPlayer();
                break;
                
        }
    }
    public void Spin()
    {
        if(true)//gameObject.transform.localRotation.y <= -90)
        {
            transform.Rotate(new Vector3(0, spinAngle, 0) * Time.deltaTime, Space.World);
        }
    }
    public void LookPlayer()
    {
        if(true)
        {
            transform.LookAt(target.transform);
        }
    }
}
