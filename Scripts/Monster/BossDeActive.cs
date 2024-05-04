using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDeActive : MonoBehaviour
{
    // 오브젝트 풀
    private PoolingManager poolManager;

    // 오브젝트 풀
    private void Awake() { poolManager = GameObject.FindGameObjectWithTag("PoolManager").GetComponent<PoolingManager>(); }
    
    // 보스 반납 : 보스가 죽는 애니메이션을 유지하기위해서 1초뒤에 반납한다
    private void Update() { if (this.GetComponent<Enemy>().isDead) Invoke("DeActive", 1f); }

    // 보스 풀에 반환
    private void DeActive() { poolManager.ReturnObj(gameObject, ObjType.보스1); }
}
