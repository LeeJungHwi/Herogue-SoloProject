using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "InventoryItemEffect/Equipment/Status")]
public class InventoryItemEquipEffect : InventoryItemEffect
{
    // 장비 장착 효과 구현
    public override bool UseEffect(Player player, int inventorySlotNumSave)
    {
        InventoryItem inventoryItem = player.GetComponent<Inventory>().inventoryItems[inventorySlotNumSave]; // 넘겨받은 인벤토리 아이템

        // 장비 장착 할 수 있는지 체크
        if(!CanEquip(player, inventoryItem))
        {
            SoundManager.instance.SFXPlay(ObjType.장비장착실패소리); // 사운드
            return false; // 장착 실패
        }
        
        EquipLogic(player, inventoryItem); // 장비 장착
        SoundManager.instance.SFXPlay(ObjType.장비장착소리); // 사운드
        return true; // 장착 성공
    }

    // 장비 장착 할 수 있는지 체크
    private bool CanEquip(Player player, InventoryItem inventoryItem)
    {
        EquipmentItemType equipmentItemType = inventoryItem.equipmentItemType; // 장비 타입

        switch (equipmentItemType)
        {
            case EquipmentItemType.Weapon: // 무기 면서
                return IsValidWeaponType(player, inventoryItem) && !player.isWeapon; // 무기 타입이 맞고 장착된 무기가 없다면 true 아니면 false
            case EquipmentItemType.Armor: // 갑옷 이면서
                return !player.isArmor; // 장착된 갑옷이 없다면 true 아니면 false
            case EquipmentItemType.Glove: // 장갑 이면서
                return !player.isGlove; // 장착된 장갑이 없다면 true 아니면 false
            case EquipmentItemType.Shoes: // 신발 이면서
                return !player.isShoes; // 장착된 신발이 없다면 true 아니면 false
            case EquipmentItemType.Amulet: // 아뮬렛 이면서
                return !player.isAmulet; // 장착된 아뮬렛이 없다면 true 아니면 false
            case EquipmentItemType.Pet: // 펫 이면서
                return !player.isPet; // 장착된 펫이 없다면 true 아니면 false
            default: // NotEquipment
                return false; // false
        }
    }

    // 캐릭터 타입에 따른 유효한 무기인지 체크
    private bool IsValidWeaponType(Player player, InventoryItem inventoryItem)
    {
        WeaponType weaponType = inventoryItem.weaponType; // 무기 타입

        switch (weaponType)
        {
            case WeaponType.Arrow: // 활 이면서
                return player.characterType.Equals("Archer"); // 궁수면 true 아니면 false
            case WeaponType.Sword: // 검 이면서
                return player.characterType.Equals("Sword") || player.characterType.Equals("Holyknight"); // 전사 or 성기사면 true 아니면 false
            case WeaponType.Staff: // 지팡이 면서
                return player.characterType.Equals("Mage"); // 법사면 true 아니면 false
            case WeaponType.Hammer: // 망치 면서
                return player.characterType.Equals("Blacksmith"); // 블랙스미스면 true 아니면 false
            default: // NotWeapon
                return false; // false
        }
    }

    // 장비 장착
    private void EquipLogic(Player player, InventoryItem inventoryItem)
    {
        // 장비 슬롯 이미지 업데이트
        UpdateEquipSlotImage(player, inventoryItem);

        // 플레이어 스탯 업데이트
        UpdatePlayerStat(player, inventoryItem);

        // 현재 장착되어있는 장비 업데이트
        UpdateEquipedItem(player, inventoryItem);

        // 장비 장착 플래그 업데이트
        UpdateFlag(player, inventoryItem);

        // 펫 이면 펫 생성
        if(inventoryItem.equipmentItemType == EquipmentItemType.Pet) PetSpawn(player, inventoryItem);
    }

    // 장비 슬롯 이미지 업데이트
    private void UpdateEquipSlotImage(Player player, InventoryItem inventoryItem)
    {
        EquipmentItemType equipmentItemType = inventoryItem.equipmentItemType; // 장비 타입

        Image slotImage = null; // 장비 슬롯 이미지

        switch (equipmentItemType) // 장비 타입에 따라 슬롯 이미지 설정
        {
            case EquipmentItemType.Weapon: // 무기
                slotImage = player.weaponSlotImage; // 무기 슬롯 이미지
                break;
            case EquipmentItemType.Armor: // 갑옷
                slotImage = player.armorSlotImage; // 갑옷 슬롯 이미지
                break;
            case EquipmentItemType.Glove: // 장갑
                slotImage = player.gloveSlotImage; // 장갑 슬롯 이미지
                break;
            case EquipmentItemType.Shoes: // 신발
                slotImage = player.shoesSlotImage; // 신발 슬롯 이미지
                break;
            case EquipmentItemType.Amulet: // 아뮬렛
                slotImage = player.amuletSlotImage; // 아뮬렛 슬롯 이미지
                break;
            case EquipmentItemType.Pet: // 펫
                slotImage = player.petSlotImage; // 펫 슬롯 이미지
                break;
        }

        // 슬롯 이미지 업데이트
        slotImage.sprite = inventoryItem.itemImage;
        Color color = slotImage.color;
        color.a = 1f;
        slotImage.color = color;
    }

    // 플레이어 스탯 업데이트
    private void UpdatePlayerStat(Player player, InventoryItem inventoryItem)
    {
        player.damage += inventoryItem.attack; // 공격력
        player.curHealth += inventoryItem.health; // 현재 체력
        player.maxHealth += inventoryItem.health; // 최대 체력
        player.criticalPercentage += inventoryItem.criticalPercentage; // 크리티컬 확률
        player.criticalDamage += inventoryItem.criticalDamage; // 크리티컬 데미지
        player.joystickScript.moveSpeed += inventoryItem.moveSpeed; // 이동 속도
        player.attackSpeed += inventoryItem.attackSpeed; // 공격 속도
    }

    // 현재 장착되어있는 장비 업데이트
    private void UpdateEquipedItem(Player player, InventoryItem inventoryItem)
    {
        EquipmentItemType equipmentItemType = inventoryItem.equipmentItemType; // 장비 타입

        switch (equipmentItemType) // 장비 타입에 따라 현재 장착되어있는 장비 업데이트
        {
            case EquipmentItemType.Weapon: // 무기
                player.equipedWeaponItem = inventoryItem;
                return;
            case EquipmentItemType.Armor: // 갑옷
                player.equipedArmorItem = inventoryItem;
                return;
            case EquipmentItemType.Glove: // 장갑
                player.equipedGloveItem = inventoryItem;
                return;
            case EquipmentItemType.Shoes: // 신발
                player.equipedShoesItem = inventoryItem;
                return;
            case EquipmentItemType.Amulet: // 아뮬렛
                player.equipedAmuletItem = inventoryItem;
                return;
            case EquipmentItemType.Pet: // 펫
                player.equipedPetItem = inventoryItem;
                return;
        }
    }

    // 장비 장착 플래그 업데이트
    private void UpdateFlag(Player player, InventoryItem inventoryItem)
    {
        EquipmentItemType equipmentItemType = inventoryItem.equipmentItemType; // 장비 타입

        // 장비 타입에 따라 장착 플래그 업데이트
        switch (equipmentItemType)
        {
            case EquipmentItemType.Weapon: // 무기
                player.isWeapon = true;
                return;
            case EquipmentItemType.Armor: // 갑옷
                player.isArmor = true;
                return;
            case EquipmentItemType.Glove: // 장갑
                player.isGlove = true;
                return;
            case EquipmentItemType.Shoes: // 신발
                player.isShoes = true;
                return;
            case EquipmentItemType.Amulet: // 아뮬렛
                player.isAmulet = true;
                return;
            case EquipmentItemType.Pet: // 펫
                player.isPet = true;
                return;
        }
    }

    // 펫 생성
    private void PetSpawn(Player player, InventoryItem inventoryItem)
    {
        GameObject instantPet = player.poolingManager.GetObj(inventoryItem.type); // 펫 활성화
        player.RepositionPet(instantPet); // 펫 위치 설정
    }
}