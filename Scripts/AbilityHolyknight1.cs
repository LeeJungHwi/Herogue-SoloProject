using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 스킬 내용 로직
[CreateAssetMenu]
public class AbilityHolyknight1 : AbilityBase
{
    // 스킬1 인스턴스
    GameObject instantAbilityHolyknight1Active;

    // 스킬 실행시 내용
    public override void Activate(GameObject joystick, GameObject player, GameObject poolingManager)
    {
        // PoolingManager 스크립트 할당
        PoolingManager poolManager = poolingManager.GetComponent<PoolingManager>();

        // Player 스크립트 할당
        Player Player = player.GetComponent<Player>();

        // 스킬 이펙트
        instantAbilityHolyknight1Active = poolManager.GetObj("AbilityHolyknight1Active");
        instantAbilityHolyknight1Active.transform.position = player.transform.position + player.transform.forward * 50f;
        instantAbilityHolyknight1Active.transform.rotation = poolManager.AbilityHolyknight1ActivePrefab.transform.rotation;

        // 애니메이션
        Player.anim.SetTrigger("doAbility1");
    }

    // 스킬 종료시 내용
    public override void DeAtivate(GameObject joystick, GameObject player, GameObject poolingManager)
    {
        // 이펙트 비활성화
        instantAbilityHolyknight1Active.SetActive(false);

        // 충돌 이펙트 비활성화
        PoolingManager poolManager = poolingManager.GetComponent<PoolingManager>();
        for (int i = 0; i < poolManager.AbilityHolyknight1HitEffects.Count; i++)
        {
            poolManager.AbilityHolyknight1HitEffects[i].SetActive(false);
        }

        // 충돌 이펙트가 저장된 리스트 클리어
        poolManager.AbilityHolyknight1HitEffects.Clear();
    }
}
