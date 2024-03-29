using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AbilityArrow2Collision : MonoBehaviour
{
    // 스킬 기본 데미지
    private float damage = 0f;

    // 플레이어
    private Player player;

    // 오브젝트 풀링
    private PoolingManager poolingManager;

    // 충돌시 생성할 이펙트
    public GameObject instantHit;

    void Start()
    {
        // Player 스크립트
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        // PoolingManager 스크립트
        poolingManager = GameObject.FindGameObjectWithTag("PoolManager").GetComponent<PoolingManager>();
    }

    void OnParticleCollision(GameObject other)
    {
        // 파티클 충돌 
        if (other.TryGetComponent(out Enemy enemy))
        {
            // 스킬 충돌 공통 로직
            player.AbilityCollisionLogic(damage, enemy, transform);

            // 적 얼리기
            enemy.nav.speed = 0f;
            enemy.isFrost = true;

            // 충돌 이펙트 활성화
            instantHit = poolingManager.GetObj(ObjType.궁수스킬3충돌이펙트);
            instantHit.transform.position = enemy.transform.position + enemy.transform.up * 5f;
            instantHit.transform.rotation = poolingManager.EffectPrefs[7].transform.rotation;

            // 스킬 사운드
            SoundManager.instance.SFXPlay(ObjType.궁수스킬3충돌소리);
        }
    }
}
