using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Carrot : MonoBehaviour
{
    // 당근 및 키클로페스 눈데미지
    public int damage;

    // 당근 및 키클로페스 눈이 유지되는 시간
    public float waitTime;

    // 오브젝트 타입
    public ObjType type;

    private PoolingManager poolManager;

    private void Awake() { poolManager = GameObject.FindGameObjectWithTag("PoolManager").GetComponent<PoolingManager>(); }

    private void Update()
    {
        // 당근 및 키클로페스 눈 회전
        transform.Rotate(Vector3.right * 90 * Time.deltaTime);
        
        // 당근 및 키클로페스 눈 반납
        if (waitTime <= 0) poolManager.ReturnObj(gameObject, type);
        else waitTime -= Time.deltaTime;
    }
}
