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

    private void Awake() { poolManager = GameObject.FindGameObjectWithTag("PoolManager").GetComponent<PoolingManager>(); }

    private void Update()
    {
        // 화살 반납
        if(waitTime <= 0) poolManager.ReturnObj(gameObject, type);
        else waitTime -= Time.deltaTime;
    }
}
