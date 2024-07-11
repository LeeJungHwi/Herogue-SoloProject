using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 스킬 내용 로직
[CreateAssetMenu]
public class AbilityMage1 : AbilityBase
{
    // 스킬1 인스턴스
    private GameObject instantAbilityMage1Active;

    // 스킬 실행시 내용
    public override void Activate(GameObject joystick, GameObject player, GameObject poolingManager)
    {
        // PoolingManager 스크립트 할당
        PoolingManager poolManager = poolingManager.GetComponent<PoolingManager>();

        // Player 스크립트 할당
        Player Player = player.GetComponent<Player>();

        // 스킬 이펙트
        instantAbilityMage1Active = poolManager.GetObj(ObjType.법사스킬2이펙트);
        instantAbilityMage1Active.transform.position = player.transform.position + player.transform.forward * 45f;
        instantAbilityMage1Active.transform.rotation = poolManager.EffectPrefs[12].transform.rotation;

        // 애니메이션
        Player.anim.SetTrigger("doAbility1");

        // 스킬 사운드
        SoundManager.instance.SFXPlay(ObjType.법사스킬2소리);
    }

    // 스킬 종료시 내용
    public override void DeAtivate(GameObject joystick, GameObject player, GameObject poolingManager)
    {
        PoolingManager poolManager = poolingManager.GetComponent<PoolingManager>();

        // 스킬 이펙트 풀에 반환
        poolManager.ReturnObj(instantAbilityMage1Active, ObjType.법사스킬2이펙트);

        // 충돌 이펙트 풀에 반환
        for (int i = 0; i < poolManager.AbilityMage1HitEffects.Count; i++) poolManager.ReturnObj(poolManager.AbilityMage1HitEffects[i].Item1, poolManager.AbilityMage1HitEffects[i].Item2);

        // 충돌 이펙트가 저장된 리스트 클리어
        poolManager.AbilityMage1HitEffects.Clear();
    }
}
