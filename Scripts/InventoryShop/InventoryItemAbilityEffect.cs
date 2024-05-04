using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 추상함수 상속
// 오버라이딩해서 액티브스킬 효과구현
[CreateAssetMenu(menuName = "InventoryItemEffect/Ability/skillContent")]
public class InventoryItemAbilityEffect : InventoryItemEffect
{
    // 액티브스킬 인덱스
    [SerializeField] private int abilityNum;

    // 추상함수 구현
    public override bool UseEffect(Player player, int inventorySlotNumSave)
    {
        // 액티브스킬 아이템 사용효과 구현
        // 액티브스킬 인덱스에 해당하는 스킬잠금을 해제한다
        // 플래그를 사용해서 해제되지 않았을경우만 사용을 성공하고 아니면 실패한다
        if(!player.isAbility[abilityNum])
        {
            // 스킬 해제
            player.abilityLock[abilityNum].SetActive(false);

            // 스킬 해제 플래그
            player.isAbility[abilityNum] = true;
        }
        else
        {
            // 실패 사운드
            SoundManager.instance.SFXPlay(ObjType.장비장착실패소리);

            // 스킬 해제 실패
            return false;
        }

        // 성공 사운드
        SoundManager.instance.SFXPlay(ObjType.포션사용소리);

        // 스킬 해제 성공
        return true;
    }
}
