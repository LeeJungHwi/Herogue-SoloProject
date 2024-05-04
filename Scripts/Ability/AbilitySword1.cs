using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// 스킬 내용 로직
[CreateAssetMenu]
public class AbilitySword1 : AbilityBase
{
    // 스킬1 인스턴스
    private GameObject instantAbilitySword1;

    // 스킬 실행시 내용
    public override void Activate(GameObject joystick, GameObject player, GameObject poolingManager)
    {
        // PoolingManager 스크립트 할당
        PoolingManager poolManager = poolingManager.GetComponent<PoolingManager>();

        // Player 스크립트 할당
        Player Player = player.GetComponent<Player>();

        // 스킬 이펙트
        instantAbilitySword1 = poolManager.GetObj(ObjType.전사스킬2이펙트);
        instantAbilitySword1.transform.SetParent(Player.transform);
        instantAbilitySword1.transform.localPosition = Vector3.zero;
        instantAbilitySword1.transform.position += new Vector3(0, 10f, 0);
        instantAbilitySword1.transform.localRotation = Quaternion.identity;

        // 플레이어 흡혈
        Player.curHealth = Player.curHealth + Player.damage > Player.maxHealth ? Player.maxHealth : Player.curHealth + Player.damage;

        // 흡혈 텍스트
        GameObject instantHealingText = poolManager.GetObj(ObjType.회복텍스트);
        instantHealingText.GetComponent<TextMeshPro>().text = "+" + Player.damage.ToString();
        instantHealingText.transform.position = Player.transform.position + Vector3.up * 20;
        instantHealingText.transform.rotation = poolManager.FloationTextPrefs[1].transform.rotation;

        // 방벽 얻기
        Player.barrier += 2;

        // 데미지감소 패시브 켜기
        Player.isPermanentSkill[(int)Player.PassiveSkillType.저거너트] = true;

        // 애니메이션
        Player.anim.SetTrigger("doAbility1");

        // 스킬 사운드
        SoundManager.instance.SFXPlay(ObjType.전사스킬2소리);
    }

    // 스킬 종료시 내용
    public override void DeAtivate(GameObject joystick, GameObject player, GameObject poolingManager)
    {
        // PoolingManager 스크립트 할당
        PoolingManager poolManager = poolingManager.GetComponent<PoolingManager>();

        // Player 스크립트 할당
        Player Player = player.GetComponent<Player>();

        // 방벽 다시 원래대로
        Player.barrier -= 2;
        if(Player.barrier < 0) Player.barrier = 0;

        // 데미지감소 패시브 끄기
        Player.isPermanentSkill[(int)Player.PassiveSkillType.저거너트] = false;

        // 스킬 이펙트 풀에 반환
        poolManager.ReturnObj(instantAbilitySword1, ObjType.전사스킬2이펙트);
    }
}
