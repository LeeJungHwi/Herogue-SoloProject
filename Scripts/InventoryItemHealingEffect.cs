using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 추상함수 상속
// 오버라이딩해서 포션사용효과 구현
[CreateAssetMenu(menuName = "InventoryItemEffect/Consumable/Health")]
public class InventoryItemHealingEffect : InventoryItemEffect
{
    // 힐량
    public int healingPoint = 0;

    // 추상함수 구현
    public override bool UseEffect(Player player, int inventorySlotNumSave)
    {
        // 포션 사용 효과 구현
        // 플레이어 스크립트를 받아와서 현재체력을 증가
        // 현재체력을 넘어가는 회복 불가능
        if(player.curHealth + healingPoint > player.maxHealth)
        {
            player.curHealth = player.maxHealth;
        }
        else
        {
            player.curHealth += healingPoint;
        }

        // 사운드
        SoundManager.instance.SFXPlay("UsePotionSound");

        return true;
    }
}
