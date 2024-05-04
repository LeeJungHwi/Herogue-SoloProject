using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 인벤토리 아이템 특성 Set
public enum ItemType { Equipment, Consumables, ExpansionSlot, RandomSkill, Ability, RandomPet, Etc } // 인벤토리 아이템 종류 : 장비 소모품 슬롯확장권 랜덤스킬 랜덤펫 기타

public enum EquipmentItemType { Weapon, Armor, Glove, Shoes, Amulet, Pet, NotEquipment } // 인벤토리 장비아이템 종류 : 무기 갑옷 장갑 신발 아뮬렛 펫 장비X

public enum WeaponType { Arrow, Sword, Staff, Hammer, NotWeapon } // 무기아이템 종류 : 활 검 지팡이 망치 무기x

[System.Serializable]
public class InventoryItem
{ 
    // 플레이어
    public Player player;
    
    // 아이템 종류
    public ItemType itemType;

    // 장비 종류
    public EquipmentItemType equipmentItemType;

    // 무기 종류
    public WeaponType weaponType;

    // 오브젝트 타입
    public ObjType type;

    // 아이템 이름
    public string itemName; 

    // 아이템 이미지
    public Sprite itemImage;

    // 아이템 효과
    public List<InventoryItemEffect> Effects;   

    // 장비 스탯 : 공격력
    public float attack;

    // 장비 스탯 : 체력
    public float health;

    // 장비 스탯 : 공격속도
    public float attackSpeed;

    // 장비 스탯 : 이동속도
    public float moveSpeed;

    // 장비 스탯 : 크확, 크뎀
    public int criticalPercentage;
    public int criticalDamage;

    // 소모품 스탯 : 힐량
    public float healingPoint;

    // 펫 스탯 : 귀여움
    public float cute;

    // 스킬 스탯 : 스킬 내용
    public string skillContent;

    // 아이템 가격
    public int price;

    // 효과 사용 체크
    // 장비 착용을 위해서 아이템 번호를 넘겨줌
    public bool Use(int inventorySlotNumSave)
    {
        // 효과 사용 체크
        bool isUsed = false;

        // 아이템 효과 사용
        foreach(InventoryItemEffect effects in Effects) isUsed = effects.UseEffect(player, inventorySlotNumSave);

        // ExecuteRole에서 효과 사용 성공 시 true 실패 시 false 반환
        return isUsed;
    }
}
