using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 스킬 내용 로직
[CreateAssetMenu]
public class AbilityBlacksmith1 : AbilityBase
{
    // 스킬1 인스턴스
    GameObject instantAbilityBlacksmith1;

    // 스킬 실행시 내용
    public override void Activate(GameObject joystick, GameObject player, GameObject poolingManager)
    {
        // PoolingManager 스크립트 할당
        PoolingManager poolManager = poolingManager.GetComponent<PoolingManager>();

        // Player 스크립트 할당
        Player Player = player.GetComponent<Player>();

        // 스킬 이펙트
        instantAbilityBlacksmith1 = poolManager.GetObj("AbilityBlacksmith1");
        instantAbilityBlacksmith1.transform.position = player.transform.position + player.transform.forward * 30f;
        instantAbilityBlacksmith1.transform.rotation = player.transform.rotation;

        // 애니메이션
        Player.anim.SetTrigger("doAbility1");

        // 스킬 사운드
        SoundManager.instance.SFXPlay("BlacksmithSkill1Sound");
    }

    // 스킬 종료시 내용
    public override void DeAtivate(GameObject joystick, GameObject player, GameObject poolingManager)
    {
        // 이펙트 비활성화
        instantAbilityBlacksmith1.SetActive(false);
    }
}
