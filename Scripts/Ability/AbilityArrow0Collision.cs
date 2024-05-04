using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// 스킬 충돌 처리
public class AbilityArrow0Collision : MonoBehaviour
{
    // 스킬 데미지
    private float damage = 500f;

    // 플레이어
    private Player player;

    // 오브젝트 풀
    private PoolingManager poolingManager;

    // 충돌 이펙트
    private GameObject instantHit;

    private void Start()
    {
        // 플레이어
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        // 오브젝트 풀
        poolingManager = GameObject.FindGameObjectWithTag("PoolManager").GetComponent<PoolingManager>();
    }

    private void OnParticleCollision(GameObject other)
    {
        // 파티클 충돌
        if(other.TryGetComponent(out Enemy enemy))
        {
            // 스킬 충돌 공통 로직
            player.AbilityCollisionLogic(damage, enemy, transform);

            // 충돌 이펙트 활성화
            instantHit = poolingManager.GetObj(ObjType.궁수스킬1충돌이펙트);
            instantHit.transform.position = enemy.transform.position + enemy.transform.up * 10f;
            instantHit.transform.rotation = poolingManager.EffectPrefs[3].transform.rotation;

            // 스킬 충돌 사운드
            SoundManager.instance.SFXPlay(ObjType.궁수스킬1충돌소리);
        }
    }
}
