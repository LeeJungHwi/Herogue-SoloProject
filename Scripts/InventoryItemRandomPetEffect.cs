using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 추상함수 상속
// 오버라이딩해서 랜덤펫 효과구현
[CreateAssetMenu(menuName = "InventoryItemEffect/RandomPet/skillContent")]
public class InventoryItemRandomPetEffect : InventoryItemEffect
{
    // 추상함수 구현
    public override bool UseEffect(Player player, int inventorySlotNumSave)
    {
        // 랜덤펫 아이템 사용효과 구현

        // 인벤토리가 가득찼는데 랜덤펫이 사용되는 문제 : 인벤토리가 가득차면 리턴
        if (player.GetComponent<Inventory>().inventoryItems.Count == player.GetComponent<Inventory>().InventorySlotCnt)
        {
            // 사용 실패
            // 사운드
            SoundManager.instance.SFXPlay("FailEquipSound");

            return false;
        }

        // 랜덤펫을 뽑아서
        int petRandom = Random.Range(0, player.shopDatabase.petItemList.Count);

        // 랜덤펫을 인벤토리에 추가
        Inventory.instance.AddInventoryItem(player.shopDatabase.petItemList[petRandom]);

        // 사운드
        SoundManager.instance.SFXPlay("UsePotionSound");

        return true;
    }
}
