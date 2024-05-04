using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 스킬 내용 로직
[CreateAssetMenu]
public class AbilityMage0 : AbilityBase
{
    // 스킬0 인스턴스
    private GameObject instantAbilityMage0;

    // 스킬 실행시 내용
    public override void Activate(GameObject joystick, GameObject player, GameObject poolingManager)
    {
        // PoolingManager 스크립트 할당
        PoolingManager poolManager = poolingManager.GetComponent<PoolingManager>();

        // Player 스크립트 할당
        Player Player = player.GetComponent<Player>();

        // 스킬 이펙트
        instantAbilityMage0 = poolManager.GetObj(ObjType.법사스킬1이펙트);
        instantAbilityMage0.transform.position = player.transform.position + player.transform.forward * 30f;
        instantAbilityMage0.transform.rotation = player.transform.rotation;

        // 애니메이션
        Player.anim.SetTrigger("doAbility0");

        // 스킬 사운드
        SoundManager.instance.SFXPlay(ObjType.법사스킬1소리);
    }

    // 스킬 종료시 내용
    public override void DeAtivate(GameObject joystick, GameObject player, GameObject poolingManager)
    {
        // PoolingManager 스크립트 할당
        PoolingManager poolManager = poolingManager.GetComponent<PoolingManager>();

        // 스킬 이펙트 풀에 반환
        poolManager.ReturnObj(instantAbilityMage0, ObjType.법사스킬1이펙트);
    }
}
