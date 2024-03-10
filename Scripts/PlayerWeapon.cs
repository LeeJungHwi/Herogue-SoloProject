using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    // 화살이 유지되는 시간
    public float waitTime;

    // 오브젝트 타입
    public ObjType type;

    // 오브젝트 풀
    private PoolingManager poolManager;

    void Awake()
    {
        // 오브젝트 풀
        poolManager = GameObject.FindGameObjectWithTag("PoolManager").GetComponent<PoolingManager>();
    }

    void Update()
    {
        if(waitTime <= 0)
        {
            // 화살 반납
            poolManager.ReturnObj(gameObject, type);
        }
        else
        {
            waitTime -= Time.deltaTime;
        }
    }
}
