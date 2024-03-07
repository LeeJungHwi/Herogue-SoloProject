using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 스킬 내용 로직
[CreateAssetMenu]
public class AbilityBlacksmith0 : AbilityBase
{
    // 스킬0 인스턴스
    GameObject instantAbilityBlacksmith0;

    // 스킬 실행시 내용
    public override void Activate(GameObject joystick, GameObject player, GameObject poolingManager)
    {
        // PoolingManager 스크립트 할당
        PoolingManager poolManager = poolingManager.GetComponent<PoolingManager>();

        // Player 스크립트 할당
        Player Player = player.GetComponent<Player>();

        // 스킬 이펙트
        instantAbilityBlacksmith0 = poolManager.GetObj("AbilityBlacksmith0");
        instantAbilityBlacksmith0.transform.position = player.transform.position + player.transform.up * 10f;
        instantAbilityBlacksmith0.transform.rotation = player.transform.rotation;

        // 애니메이션
        Player.anim.SetTrigger("doAbility0");

        // 스킬 사운드
        SoundManager.instance.SFXPlay("BlacksmithSkill0Sound");
    }

    // 스킬 종료시 내용
    public override void DeAtivate(GameObject joystick, GameObject player, GameObject poolingManager)
    {
        // 이펙트 비활성화
        instantAbilityBlacksmith0.SetActive(false);
    }
}
