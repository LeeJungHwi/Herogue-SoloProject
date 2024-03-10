using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// 스킬 충돌 처리
public class AbilityArrow0HitCollision : MonoBehaviour
{
    // 스킬 기본 데미지
    private float damage = 50f;

    // 플레이어
    private Player player;

    void Start()
    {
        // Player 스크립트
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    void OnParticleCollision(GameObject other)
    {
        // 파티클 충돌
        // 스킬 충돌 공통 로직
        if (other.TryGetComponent(out Enemy enemy))
        {
            player.AbilityCollisionLogic(damage, enemy, transform);
        }
    }
}
