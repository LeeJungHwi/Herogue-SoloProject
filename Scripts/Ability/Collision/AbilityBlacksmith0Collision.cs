using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// 스킬 충돌 처리
public class AbilityBlacksmith0Collision : MonoBehaviour
{
    // 스킬 기본 데미지
    private float damage = 300f;

    // 플레이어
    private Player player;

    // Player 스크립트
    private void Start() { player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>(); }

    // 파티클 충돌
    private void OnParticleCollision(GameObject other) { if (other.TryGetComponent(out Enemy enemy)) player.AbilityCollisionLogic(damage, enemy, transform); }
}
