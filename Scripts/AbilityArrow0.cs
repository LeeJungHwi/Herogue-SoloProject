using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 스킬 내용 로직
[CreateAssetMenu]
public class AbilityArrow0 : AbilityBase
{
    // 스킬 이펙트
    GameObject instantAbilityArrow0;

    // 스킬 시전
    public override void Activate(GameObject joystick, GameObject player, GameObject poolingManager)
    {
        // 오브젝트 풀
        PoolingManager poolManager = poolingManager.GetComponent<PoolingManager>();

        // 플레이어
        Player Player = player.GetComponent<Player>();

        // 스킬 이펙트 활성화
        instantAbilityArrow0 = poolManager.GetObj("AbilityArrow0");
        instantAbilityArrow0.transform.position = player.transform.position + new Vector3(0, 10f, 0);
        instantAbilityArrow0.transform.rotation = player.transform.rotation;

        // 애니메이션
        Player.anim.SetTrigger("doShoot");

        // 스킬 시전 사운드
        SoundManager.instance.SFXPlay("ArrowSkill02Sound");
    }

    // 스킬 종료
    public override void DeAtivate(GameObject joystick, GameObject player, GameObject poolingManager)
    {
        // 스킬 이펙트 비활성화
        instantAbilityArrow0.SetActive(false);

        // 충돌 이펙트 비활성화
        PoolingManager poolManager = poolingManager.GetComponent<PoolingManager>();
        for (int i = 0; i < poolManager.AbilityArrow0HitEffects.Count; i++)
        {
            poolManager.AbilityArrow0HitEffects[i].SetActive(false);
        }

        // 충돌 이펙트 리스트 초기화
        poolManager.AbilityArrow0HitEffects.Clear();
    }
}
