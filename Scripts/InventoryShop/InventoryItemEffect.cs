using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 각 아이템 사용 효과를 오버라이딩
public abstract class InventoryItemEffect : ScriptableObject
{
    // Player -> 포션 사용 효과를 위해 넘겨줌, 슬롯번호 -> 장비 장착 효과를 위해 넘겨줌
    public abstract bool UseEffect(Player player, int inventorySlotNumSave);
}
