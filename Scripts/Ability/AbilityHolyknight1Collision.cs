using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// 스킬 충돌 처리
public class AbilityHolyknight1Collision : MonoBehaviour
{
    // 스킬 기본 데미지
    private float damage = 300f;

    // 플레이어
    private Player player;

    // 오브젝트 풀링
    private PoolingManager poolingManager;

    // 파티클 시스템
    private ParticleSystem particle;

    // 충돌 이벤트를 저장 할 리스트
    private List<ParticleCollisionEvent> collisionEvents = new List<ParticleCollisionEvent>();

    // 충돌시 생성할 이펙트
    private GameObject instantHit;

    private void Start()
    {
        // 파티클 시스템
        particle = GetComponent<ParticleSystem>();

        // Player 스크립트
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        // PoolingManager 스크립트
        poolingManager = GameObject.FindGameObjectWithTag("PoolManager").GetComponent<PoolingManager>();
    }

    private void OnParticleCollision(GameObject other)
    {
        // 파티클 충돌
        // 파티클 충돌 이벤트의 수
        int events = particle.GetCollisionEvents(other, collisionEvents);

        for (int i = 0; i < events; i++)
        {
            // 바닥에 닿으면 충돌이펙트 생성
            if(other.layer == 12)
            {
                instantHit = poolingManager.GetObj(ObjType.성기사스킬2충돌이펙트);
                instantHit.transform.position = collisionEvents[i].intersection;
                instantHit.transform.rotation = Quaternion.LookRotation(collisionEvents[i].normal);
            }

            // 스킬 사운드
            SoundManager.instance.SFXPlay(ObjType.성기사스킬2소리);
        }

        // 스킬 충돌 공통 로직
        if (other.TryGetComponent(out Enemy enemy)) player.AbilityCollisionLogic(damage, enemy, transform);
    }
}
