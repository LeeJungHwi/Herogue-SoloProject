// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using TMPro;

// 버프로 변경
// // 스킬 충돌 처리
// public class AbilitySword1Collision : MonoBehaviour
// {
//     // 스킬 기본 데미지
//     private float damage = 200f;

//     // 플레이어
//     private Player player;

//     // 오브젝트 풀링
//     private PoolingManager poolingManager;

//     void Start()
//     {
//         // Player 스크립트
//         player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

//         // PoolingManager 스크립트
//         poolingManager = GameObject.FindGameObjectWithTag("PoolManager").GetComponent<PoolingManager>();
//     }

//     void OnParticleCollision(GameObject other)
//     {
//         // 파티클 충돌
//         if (other.TryGetComponent(out Enemy enemy))
//         {
//             // 스킬 충돌 공통 로직
//             player.AbilityCollisionLogic(damage, enemy, transform);

//             // 플레이어 흡혈
//             player.curHealth = player.curHealth + (damage + player.damage) > player.maxHealth ? player.maxHealth : player.curHealth + (damage + player.damage);

//             // 흡혈 텍스트
//             GameObject instantHealingText = poolingManager.GetObj(ObjType.회복텍스트);
//             instantHealingText.GetComponent<TextMeshPro>().text = "+" + (damage + player.damage).ToString();
//             instantHealingText.transform.position = transform.position + Vector3.up * 20;
//             instantHealingText.transform.rotation = poolingManager.FloationTextPrefs[1].transform.rotation;
//         }
//     }
// }
