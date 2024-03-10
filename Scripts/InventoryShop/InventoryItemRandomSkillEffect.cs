using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 추상함수 상속
// 오버라이딩해서 랜덤 스킬 큐브 효과구현
[CreateAssetMenu(menuName = "InventoryItemEffect/RandomSkill/skillContent")]
public class InventoryItemRandomSkillEffect : InventoryItemEffect
{
    // 추상함수 구현
    public override bool UseEffect(Player player, int inventorySlotNumSave)
    {
        // 랜덤 스킬 아이템 사용효과 구현
        // 영구 스킬 획득 횟수 5회 제한
        if(player.permanentSkillCnt == 5)
        {
            // 사운드
            SoundManager.instance.SFXPlay(ObjType.장비장착실패소리);

            return false;
        }

        // 랜덤으로 스킬을 뽑아서
        int skillRandom = Random.Range(0, player.shopDatabase.passiveItemList.Count); // 0~21

        // 스킬 설명을 저장하고
        player.permanentSb.AppendLine(player.shopDatabase.passiveItemList[skillRandom].skillContent);

        // 스킬 텍스트에 쓴다
        player.permanentSkillListText.text = player.permanentSb.ToString();

        // 스킬 스탯 적용
        if(skillRandom == 0)
        {
            // 한발노리기
            if(!player.isPermanentSkill[0])
            {
                // 크리티컬 확률 증가
                player.criticalPercentage = 100;

                // 명중률 감소
                player.accuracy = 50;
            }
        }
        else if(skillRandom == 1)
        {
            // 백발백중
            if (!player.isPermanentSkill[1])
            {
                // 명중률 증가
                player.accuracy = 100;

                // 크리티컬 데미지 감소
                player.criticalDamage = 150;
            }
        }
        else if (skillRandom == 3)
        {
            // 초월방벽
            if (!player.isPermanentSkill[3])
            {
                // 방벽 증가
                player.barrier += 20;

                // 체력 1
                player.curHealth = 1;
                player.maxHealth = 1;
            }
        }
        else if (skillRandom == 4)
        {
            // 흡혈귀
            if (!player.isPermanentSkill[4])
            {
                // 흡혈량이 20%가 된다
                player.bloodDrain = 20;
            }
        }
        else if (skillRandom == 6)
        {
            // 도박꾼
            if (!player.isPermanentSkill[6])
            {
                // 드랍률 증가
                player.activeDropPercentage += 10;
                player.passiveDropPercentage += 10;
                player.inventoryItemPercentage += 10;
            }
        }
        else if (skillRandom == 9)
        {
            // 시크릿
            if (!player.isPermanentSkill[9])
            {
                // 시크릿상자 드랍률 증가
                player.secretPercentage += 10;
            }
        }
        else if (skillRandom == 14)
        {
            // HP부스트
            if (!player.isPermanentSkill[14])
            {
                // 최대 HP가 100% 증가
                player.maxHealth *= 2;
            }
        }
        else if (skillRandom == 21)
        {
            // 불사신
            if (!player.isPermanentSkill[21])
            {
                // 1초간 3초간격으로 무적상태
                player.StartCoroutine("Immortality");
            }
        }

        // 영구 스킬 적용
        player.isPermanentSkill[skillRandom] = true;

        // 영구 스킬 적용 횟수 증가
        player.permanentSkillCnt++;

        // 사운드
        SoundManager.instance.SFXPlay(ObjType.포션사용소리);

        return true;
    }
}
