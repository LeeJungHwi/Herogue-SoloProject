using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 스킬 내용 로직
[CreateAssetMenu]
public class AbilityArrow2 : AbilityBase
{
    // 스킬2 인스턴스
    GameObject instantAbilityArrow2Active;

    // 스킬 실행시 내용
    public override void Activate(GameObject joystick, GameObject player, GameObject poolingManager)
    {
        // PoolingManager 스크립트 할당
        PoolingManager poolManager = poolingManager.GetComponent<PoolingManager>();

        // Player 스크립트 할당
        Player Player = player.GetComponent<Player>();

        // 스킬 이펙트
        instantAbilityArrow2Active = poolManager.GetObj(ObjType.궁수스킬3이펙트);
        instantAbilityArrow2Active.transform.position = player.transform.position + new Vector3(0, 10f, 0);
        instantAbilityArrow2Active.transform.rotation = player.transform.rotation;

        // 애니메이션
        Player.anim.SetTrigger("doShoot");

        // 스킬 사운드
        SoundManager.instance.SFXPlay(ObjType.궁수스킬13소리);
    }

    // 스킬 종료시 내용
    public override void DeAtivate(GameObject joystick, GameObject player, GameObject poolingManager)
    {
        // 오브젝트 풀
        PoolingManager poolManager = poolingManager.GetComponent<PoolingManager>();

        // 스킬 이펙트 풀에 반환
        poolManager.ReturnObj(instantAbilityArrow2Active, ObjType.궁수스킬3이펙트);

        // 충돌 이펙트 풀에 반환
        for (int i = 0; i < poolManager.AbilityArrow2HitEffects.Count; i++)
        {
            poolManager.ReturnObj(poolManager.AbilityArrow2HitEffects[i].Item1, poolManager.AbilityArrow2HitEffects[i].Item2);
        }

        // 충돌 이펙트가 저장된 리스트 클리어
        poolManager.AbilityArrow2HitEffects.Clear();
    }
}
