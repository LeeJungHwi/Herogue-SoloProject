using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// 인벤토리 슬롯과 인벤토리 아이템과의 상호작용을 적은 스크립트
public class InventorySlot : MonoBehaviour, IPointerUpHandler
{
    // 인벤토리 아이템
    public InventoryItem inventoryItem;

    // 인벤토리 아이템 싱글톤 : 소모품을 사용하기위해서 어떤슬롯이든 공유
    public static InventoryItem inventoryItemSave;

    // 인벤토리 아이템 이미지
    public Image inventoryItemImage;

    // 인벤토리 슬롯 숫자
    public int inventorySlotNum;

    // 인벤토리 슬롯 숫자 싱글톤 : 포션을 사용하기위해서 어떤슬롯이든 공유
    public static int inventorySlotNumSave;

    // 인벤토리
    public Inventory inventory;

    // 인벤토리 장비아이템 정보패널
    public GameObject inventoryEquipmentItemInfoPanel;

    // 인벤토리 장비아이템 이미지
    public Image inventoryEquipmentItemInfoImage;

    // 인벤토리 장비아이템 이름
    public Text inventoryEquipmentItemInfoNameText;

    // 인벤토리 장비아이템 스탯
    public Text inventoryEquipmentItemInfoStatusText;

    // 인벤토리 소모품아이템 정보패널
    public GameObject inventoryConsumablesItemInfoPanel;

    // 인벤토리 소모품아이템 이미지
    public Image inventoryConsumablesItemInfoImage;

    // 인벤토리 소모품아이템 이름
    public Text inventoryConsumablesItemInfoNameText;

    // 인벤토리 소모품아이템 스탯
    public Text inventoryConsumablesItemInfoStatusText;

    // 인벤토리 장비아이템 해제패널
    public GameObject inventoryEquipmentItemInfoUnEquipPanel;

    // 인벤토리 장비아이템 해제패널 이미지
    public Image inventoryEquipmentItemInfoUnEquipImage;

    // 인벤토리 장비아이템 해제패널 이름
    public Text inventoryEquipmentItemInfoUnEquipNameText;

    // 인벤토리 장비아이템 해제패널 스탯
    public Text inventoryEquipmentItemInfoUnEquipStatusText;

    // 해제 할 인벤토리 아이템 : 해제 할 장비를 저장하기위한 스태틱변수
    public static InventoryItem unequipInventoryItemSave;

    // 플레이어
    public Player player;

    // 장비아이템 가격 텍스트
    public Text inventoryEquipmentItemPriceText;

    // 소모품 가격 텍스트
    public Text inventoryConsumablesItemPriceText;

    // 인벤토리 아이템 판매 패널
    public GameObject inventoryItemSellPanel;

    // 인벤토리 아이템 판매 이미지
    public Image inventoryItemSellImage;

    // 인벤토리 아이템 판매 이름
    public Text inventoryItemSellNameText;

    // 상점 아이템 구매 스탯
    public Text inventoryItemSellStatusText;

    // 인벤토리 아이템 판매 가격텍스트
    public Text inventoryItemSellPriceText;

    // 인벤토리 슬롯 확장 패널
    public GameObject inventoryExpansionSlotItemInfoPanel;

    // 랜덤 스킬 패널
    public GameObject inventoryRandomSkillItemInfoPanel;

    // 액티브스킬 패널
    public GameObject inventoryAbilityItemInfoPanel;

    // 액티브스킬 이미지
    public Image inventoryAbilityItemInfoImage;

    // 액티브스킬 이름 텍스트
    public Text inventoryAbilityItemInfoNameText;

    // 액티브스킬 스탯 텍스트
    public Text inventoryAbilityItemInfoStatusText;

    // 액티브스킬 가격 텍스트
    public Text inventoryAbilityItemPriceText;

    // 랜덤펫 패널
    public GameObject inventoryRandomPetItemInfoPanel;

    // 장비 슬롯 이미지 스프라이트
    // 0 : 무기 1 : 갑옷 2 : 장갑 3 : 신발 4 : 아뮬렛 5 : 펫
    public Sprite[] equipmentSlotImageSpirte;

    public void UpdateSlotUI()
    {
        // 슬롯에 아이템 그리기
        inventoryItemImage.sprite = inventoryItem.itemImage;
        inventoryItemImage.gameObject.SetActive(true);
    }

    public void RemoveSlot()
    {
        // 슬롯에 있는 아이템 제거
        inventoryItem = null;
        inventoryItemImage.gameObject.SetActive(false);
    }

    public void RemoveInventoryItem(string itemType)
    {
        // 인벤토리 아이템을 버리는 함수
        // 장비 : 장비 패널 닫기
        if(itemType == "Equipment")
        {
            inventoryEquipmentItemInfoPanel.SetActive(false);
        }

        // 소모품 : 소모품 패널 닫기
        if(itemType == "Consumables")
        {
            inventoryConsumablesItemInfoPanel.SetActive(false);
        }

        // 인벤토리 슬롯확장권 : 인벤토리 슬롯 확장 패널 닫기
        if(itemType == "ExpansionSlot")
        {
            inventoryExpansionSlotItemInfoPanel.SetActive(false);
        }

        // 랜덤 스킬 : 랜덤 스킬 패널 닫기
        if (itemType == "RandomSkill")
        {
            inventoryRandomSkillItemInfoPanel.SetActive(false);
        }

        // 액티브스킬 : 액티브스킬 패널 닫기
        if (itemType == "Ability")
        {
            inventoryAbilityItemInfoPanel.SetActive(false);
        }

        // 랜덤펫 : 랜덤펫 패널 닫기
        if (itemType == "RandomPet")
        {
            inventoryRandomPetItemInfoPanel.SetActive(false);
        }

        // 인벤토리 아이템 제거
        inventory.RemoveInventoryItem(inventorySlotNumSave);

        // 사운드
        SoundManager.instance.SFXPlay("ButtonSound");
    }

    public void SellInventoryItem()
    {
        // 인벤토리 아이템을 판매하는 함수
        // 인벤토리 아이템 가격 패널 닫기
        inventoryItemSellPanel.SetActive(false);

        // 인벤토리 아이템 가격에 해당하는 코인 증가
        player.coin += inventory.inventoryItems[inventorySlotNumSave].price;

        // 인벤토리 아이템 제거
        inventory.RemoveInventoryItem(inventorySlotNumSave);

        // 사운드
        SoundManager.instance.SFXPlay("ButtonSound");
    }

    public void DeActiveInventoryPanel(string panelType)
    {
        // 패널 비활성화
        if(panelType == "Sell")
        {
            // 아이템 판매창 닫기
            inventoryItemSellPanel.SetActive(false);
        }
        else if(panelType == "RandomSkill")
        {
            // 랜덤스킬패널 닫기
            inventoryRandomSkillItemInfoPanel.SetActive(false);
        }
        else if(panelType == "Consumables")
        {
            // 소모품 정보창 닫기
            inventoryConsumablesItemInfoPanel.SetActive(false);
        }
        else if (panelType == "Equipment")
        {
            // 장비 정보창 닫기
            inventoryEquipmentItemInfoPanel.SetActive(false);
        }
        else if (panelType == "UnEquip")
        {
            // 장비 아이템 해제패널 닫기
            inventoryEquipmentItemInfoUnEquipPanel.SetActive(false);
        }
        else if (panelType == "ExpansionSlot")
        {
            // 인벤토리 슬롯 확장 패널 닫기
            inventoryExpansionSlotItemInfoPanel.SetActive(false);
        }
        else if (panelType == "Ability")
        {
            // 액티브스킬 패널 닫기
            inventoryAbilityItemInfoPanel.SetActive(false);
        }
        else if (panelType == "RandomPet")
        {
            // 랜덤펫 패널 닫기
            inventoryRandomPetItemInfoPanel.SetActive(false);
        }

        // 사운드
        SoundManager.instance.SFXPlay("ButtonSound");
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // 인벤토리 슬롯 클릭
        // 상점 슬롯과 인벤토리 슬롯에서 아이템이 없는데 슬롯이 눌러지는 문제
        // 현재 인벤토리에 있는 아이템의 개수를 넘어서는 슬롯을 누르면 리턴
        if(inventory.inventoryItems.Count <= inventorySlotNum)
        {
            return;
        }

        if(!player.isShop)
        {
            // 플레이어 정보에서 인벤토리 슬롯 클릭
            // 장비
            if (inventory.inventoryItems[inventorySlotNum].itemType == ItemType.Equipment)
            {
                // 장비아이템 정보패널 활성화
                inventoryEquipmentItemInfoPanel.SetActive(true);

                // 장비아이템 이미지를 보여준다
                inventoryEquipmentItemInfoImage.sprite = inventory.inventoryItems[inventorySlotNum].itemImage;

                // 장비아이템 이름을 보여준다
                inventoryEquipmentItemInfoNameText.text = inventory.inventoryItems[inventorySlotNum].itemName;

                // 장비아이템 종류에 따라 스탯을 보여준다
                if (inventory.inventoryItems[inventorySlotNum].equipmentItemType == EquipmentItemType.Weapon)
                {
                    // 무기면 공격력을 보여준다
                    inventoryEquipmentItemInfoStatusText.text = "공격력 + " + inventory.inventoryItems[inventorySlotNum].attack.ToString();
                }
                else if (inventory.inventoryItems[inventorySlotNum].equipmentItemType == EquipmentItemType.Armor)
                {
                    // 갑옷이면 체력을 보여준다
                    inventoryEquipmentItemInfoStatusText.text = "체력 + " + inventory.inventoryItems[inventorySlotNum].health.ToString();
                }
                else if (inventory.inventoryItems[inventorySlotNum].equipmentItemType == EquipmentItemType.Glove)
                {
                    // 장갑이면 공격속도를 보여준다
                    inventoryEquipmentItemInfoStatusText.text = "공격속도 + " + inventory.inventoryItems[inventorySlotNum].attackSpeed.ToString();
                }
                else if (inventory.inventoryItems[inventorySlotNum].equipmentItemType == EquipmentItemType.Shoes)
                {
                    // 신발이면 이동속도를 보여준다
                    inventoryEquipmentItemInfoStatusText.text = "이동속도 + " + inventory.inventoryItems[inventorySlotNum].moveSpeed.ToString();
                }
                else if (inventory.inventoryItems[inventorySlotNum].equipmentItemType == EquipmentItemType.Amulet)
                {
                    // 아뮬렛이면 크확, 크뎀을 보여준다
                    inventoryEquipmentItemInfoStatusText.text = "크리티컬확률 + " + inventory.inventoryItems[inventorySlotNum].criticalPercentage.ToString() + " %" + System.Environment.NewLine + "크리티컬데미지 + " + inventory.inventoryItems[inventorySlotNum].criticalDamage.ToString() + " %";
                }
                else if (inventory.inventoryItems[inventorySlotNum].equipmentItemType == EquipmentItemType.Pet)
                {
                    // 펫이면
                    // 펫 시스템은 나중에 구현하여 펫의 스탯을 보여준다
                    inventoryEquipmentItemInfoStatusText.text = "귀여움 + " + inventory.inventoryItems[inventorySlotNum].cute.ToString();
                }

                // 장비아이템의 가격을 보여준다
                inventoryEquipmentItemPriceText.text = inventory.inventoryItems[inventorySlotNum].price.ToString();
            }

            // 소모품
            if (inventory.inventoryItems[inventorySlotNum].itemType == ItemType.Consumables)
            {
                // 소모품아이템 정보패널 활성화
                inventoryConsumablesItemInfoPanel.SetActive(true);

                // 소모품아이템 이미지를 보여준다
                inventoryConsumablesItemInfoImage.sprite = inventory.inventoryItems[inventorySlotNum].itemImage;

                // 소모품아이템 이름을 보여준다
                inventoryConsumablesItemInfoNameText.text = inventory.inventoryItems[inventorySlotNum].itemName;

                // 소모품아이템의 힐량을 보여준다
                inventoryConsumablesItemInfoStatusText.text = "HP회복 + " + inventory.inventoryItems[inventorySlotNum].healingPoint.ToString();

                // 소모품아이템의 가격을 보여준다
                inventoryConsumablesItemPriceText.text = inventory.inventoryItems[inventorySlotNum].price.ToString();
            }

            // 인벤토리 슬롯 확장권
            if (inventory.inventoryItems[inventorySlotNum].itemType == ItemType.ExpansionSlot)
            {
                // 인벤토리 슬롯 확장권 아이템 정보패널 활성화
                inventoryExpansionSlotItemInfoPanel.SetActive(true);
            }

            // 랜덤 스킬
            if (inventory.inventoryItems[inventorySlotNum].itemType == ItemType.RandomSkill)
            {
                // 랜덤 스킬 아이템 정보패널 활성화
                inventoryRandomSkillItemInfoPanel.SetActive(true);
            }

            // 액티브스킬
            if (inventory.inventoryItems[inventorySlotNum].itemType == ItemType.Ability)
            {
                // 액티브스킬 정보패널 활성화
                inventoryAbilityItemInfoPanel.SetActive(true);

                // 액티브스킬 이미지를 보여준다
                inventoryAbilityItemInfoImage.sprite = inventory.inventoryItems[inventorySlotNum].itemImage;

                // 액티브스킬 이름을 보여준다
                inventoryAbilityItemInfoNameText.text = inventory.inventoryItems[inventorySlotNum].itemName;

                // 액티브스킬 내용을 보여준다
                inventoryAbilityItemInfoStatusText.text = inventory.inventoryItems[inventorySlotNum].skillContent;

                // 액티브스킬 가격을 보여준다
                inventoryAbilityItemPriceText.text = inventory.inventoryItems[inventorySlotNum].price.ToString();
            }

            if (inventory.inventoryItems[inventorySlotNum].itemType == ItemType.RandomPet)
            {
                // 랜덤펫뽑기 정보패널 활성화
                inventoryRandomPetItemInfoPanel.SetActive(true);
            }

            // 아이템을 스태틱변수에 저장
            inventoryItemSave = inventoryItem;

            // 아이템의 번호를 스태틱변수에 저장
            inventorySlotNumSave = inventorySlotNum;
        }
        else
        {
            // 상점에서 인벤토리 슬롯 클릭
            // 인벤토리 아이템 판매 패널 활성화
            inventoryItemSellPanel.SetActive(true);

            // 판매아이템 이미지를 보여준다
            inventoryItemSellImage.sprite = inventory.inventoryItems[inventorySlotNum].itemImage;

            // 판매아이템 이름을 보여준다
            inventoryItemSellNameText.text = inventory.inventoryItems[inventorySlotNum].itemName;

            // 판매아이템 가격을 보여준다
            inventoryItemSellPriceText.text = inventory.inventoryItems[inventorySlotNum].price.ToString();

            // 판매아이템 스탯을 보여준다
            // 장비
            if (inventory.inventoryItems[inventorySlotNum].itemType == ItemType.Equipment)
            {
                if (inventory.inventoryItems[inventorySlotNum].equipmentItemType == EquipmentItemType.Weapon)
                {
                    // 무기면 공격력을 보여준다
                    inventoryItemSellStatusText.text = "공격력 + " + inventory.inventoryItems[inventorySlotNum].attack.ToString();
                }
                else if (inventory.inventoryItems[inventorySlotNum].equipmentItemType == EquipmentItemType.Armor)
                {
                    // 갑옷이면 체력을 보여준다
                    inventoryItemSellStatusText.text = "체력 + " + inventory.inventoryItems[inventorySlotNum].health.ToString();
                }
                else if (inventory.inventoryItems[inventorySlotNum].equipmentItemType == EquipmentItemType.Glove)
                {
                    // 장갑이면 공격속도를 보여준다
                    inventoryItemSellStatusText.text = "공격속도 + " + inventory.inventoryItems[inventorySlotNum].attackSpeed.ToString();
                }
                else if (inventory.inventoryItems[inventorySlotNum].equipmentItemType == EquipmentItemType.Shoes)
                {
                    // 신발이면 이동속도를 보여준다
                    inventoryItemSellStatusText.text = "이동속도 + " + inventory.inventoryItems[inventorySlotNum].moveSpeed.ToString();
                }
                else if (inventory.inventoryItems[inventorySlotNum].equipmentItemType == EquipmentItemType.Amulet)
                {
                    // 아뮬렛이면 크확, 크뎀을 보여준다
                    inventoryItemSellStatusText.text = "크리티컬확률 + " + inventory.inventoryItems[inventorySlotNum].criticalPercentage.ToString() + " %" + System.Environment.NewLine + "크리티컬데미지 + " + inventory.inventoryItems[inventorySlotNum].criticalDamage.ToString() + " %";
                }
                else if (inventory.inventoryItems[inventorySlotNum].equipmentItemType == EquipmentItemType.Pet)
                {
                    // 펫이면
                    // 펫 시스템은 나중에 구현하여 펫의 스탯을 보여준다
                    inventoryItemSellStatusText.text = "귀여움 + " + inventory.inventoryItems[inventorySlotNum].cute.ToString();
                }
            }

            // 소모품
            if (inventory.inventoryItems[inventorySlotNum].itemType == ItemType.Consumables)
            {
                // 소모품아이템의 힐량을 보여준다
                inventoryItemSellStatusText.text = "HP회복 + " + inventory.inventoryItems[inventorySlotNum].healingPoint.ToString();
            }

            // 인벤토리 슬롯 확장권
            if (inventory.inventoryItems[inventorySlotNum].itemType == ItemType.ExpansionSlot)
            {
                // 인벤토리 슬롯 확장권의 효과를 보여준다
                inventoryItemSellStatusText.text = "인벤토리 슬롯 4칸을 확장합니다";
            }

            // 랜덤 스킬 아이템
            if (inventory.inventoryItems[inventorySlotNum].itemType == ItemType.RandomSkill)
            {
                // 랜덤 스킬 아이템 효과를 보여준다
                inventoryItemSellStatusText.text = "영구적으로 적용되는 스킬을 얻습니다";
            }

            // 액티브스킬
            if (inventory.inventoryItems[inventorySlotNum].itemType == ItemType.Ability)
            {
                // 랜덤 스킬 아이템 효과를 보여준다
                inventoryItemSellStatusText.text = inventory.inventoryItems[inventorySlotNum].skillContent;
            }

            if (inventory.inventoryItems[inventorySlotNum].itemType == ItemType.RandomPet)
            {
                // 랜덤펫뽑기 효과를 보여준다
                inventoryItemSellStatusText.text = "펫이 들어있다";
            }

            // 판매아이템을 스태틱변수에 저장
            inventoryItemSave = inventoryItem;

            // 판매아이템의 번호를 스태틱변수에 저장
            inventorySlotNumSave = inventorySlotNum;
        }

        // 사운드
        SoundManager.instance.SFXPlay("ButtonSound");
    }

    public void UseInventoryItem(string itemType)
    {
        // 인벤토리 아이템 사용
        bool isUse = inventoryItemSave.Use(inventorySlotNumSave);
        if (isUse)
        {
            // 인벤토리 아이템 사용을 성공했으면 인벤토리 아이템 제거
            Inventory.instance.RemoveInventoryItem(inventorySlotNumSave);

            // 패널 비활성화
            if(itemType == "Equipment")
            {
                inventoryEquipmentItemInfoPanel.SetActive(false);
            }
            else if(itemType == "Consumables")
            {
                inventoryConsumablesItemInfoPanel.SetActive(false);
            }
            else if(itemType == "RandomSkill")
            {
                inventoryRandomSkillItemInfoPanel.SetActive(false);
            }
            else if (itemType == "Ability")
            {
                inventoryAbilityItemInfoPanel.SetActive(false);
            }
            else if (itemType == "RandomPet")
            {
                inventoryRandomPetItemInfoPanel.SetActive(false);
            }
        }
    }

    public void OnClickEquipmentSlot(string slotType)
    {
        // 장비 슬롯 클릭

        if(slotType == player.equipedWeaponItem.equipmentItemType.ToString())
        {
            // 비어있는 장비슬롯을 누르면 이전에 착용했던 장비해제패널이 나오는문제
            if(player.isWeapon)
            {
                // 무기
                // 장비아이템 해제패널 활성화
                inventoryEquipmentItemInfoUnEquipPanel.SetActive(true);

                // 장비아이템 이미지를 보여준다
                inventoryEquipmentItemInfoUnEquipImage.sprite = player.equipedWeaponItem.itemImage;

                // 장비아이템 이름을 보여준다
                inventoryEquipmentItemInfoUnEquipNameText.text = player.equipedWeaponItem.itemName;

                // 장비아이템 스탯을 보여준다
                inventoryEquipmentItemInfoUnEquipStatusText.text = "공격력 + " + player.equipedWeaponItem.attack.ToString();

                // 해제 할 인벤토리 아이템 저장
                unequipInventoryItemSave = player.equipedWeaponItem;
            }
        }
        else if (slotType == player.equipedArmorItem.equipmentItemType.ToString())
        {
            // 비어있는 장비슬롯을 누르면 이전에 착용했던 장비해제패널이 나오는문제
            if(player.isArmor)
            {
                // 갑옷
                // 장비아이템 해제패널 활성화
                inventoryEquipmentItemInfoUnEquipPanel.SetActive(true);

                // 장비아이템 이미지를 보여준다
                inventoryEquipmentItemInfoUnEquipImage.sprite = player.equipedArmorItem.itemImage;

                // 장비아이템 이름을 보여준다
                inventoryEquipmentItemInfoUnEquipNameText.text = player.equipedArmorItem.itemName;

                // 장비아이템 스탯을 보여준다
                inventoryEquipmentItemInfoUnEquipStatusText.text = "체력 + " + player.equipedArmorItem.health.ToString();

                // 해제 할 인벤토리 아이템 저장
                unequipInventoryItemSave = player.equipedArmorItem;
            }
        }
        else if (slotType == player.equipedGloveItem.equipmentItemType.ToString())
        {
            // 비어있는 장비슬롯을 누르면 이전에 착용했던 장비해제패널이 나오는문제
            if (player.isGlove)
            {
                // 장갑
                // 장비아이템 해제패널 활성화
                inventoryEquipmentItemInfoUnEquipPanel.SetActive(true);

                // 장비아이템 이미지를 보여준다
                inventoryEquipmentItemInfoUnEquipImage.sprite = player.equipedGloveItem.itemImage;

                // 장비아이템 이름을 보여준다
                inventoryEquipmentItemInfoUnEquipNameText.text = player.equipedGloveItem.itemName;

                // 장비아이템 스탯을 보여준다
                inventoryEquipmentItemInfoUnEquipStatusText.text = "공격속도 + " + player.equipedGloveItem.attackSpeed.ToString();

                // 해제 할 인벤토리 아이템 저장
                unequipInventoryItemSave = player.equipedGloveItem;
            }
        }
        else if (slotType == player.equipedShoesItem.equipmentItemType.ToString())
        {
            // 비어있는 장비슬롯을 누르면 이전에 착용했던 장비해제패널이 나오는문제
            if (player.isShoes)
            {
                // 신발
                // 장비아이템 해제패널 활성화
                inventoryEquipmentItemInfoUnEquipPanel.SetActive(true);

                // 장비아이템 이미지를 보여준다
                inventoryEquipmentItemInfoUnEquipImage.sprite = player.equipedShoesItem.itemImage;

                // 장비아이템 이름을 보여준다
                inventoryEquipmentItemInfoUnEquipNameText.text = player.equipedShoesItem.itemName;

                // 장비아이템 스탯을 보여준다
                inventoryEquipmentItemInfoUnEquipStatusText.text = "이동속도 + " + player.equipedShoesItem.moveSpeed.ToString();

                // 해제 할 인벤토리 아이템 저장
                unequipInventoryItemSave = player.equipedShoesItem;
            }
        }
        else if (slotType == player.equipedAmuletItem.equipmentItemType.ToString())
        {
            // 비어있는 장비슬롯을 누르면 이전에 착용했던 장비해제패널이 나오는문제
            if (player.isAmulet)
            {
                // 아뮬렛
                // 장비아이템 해제패널 활성화
                inventoryEquipmentItemInfoUnEquipPanel.SetActive(true);

                // 장비아이템 이미지를 보여준다
                inventoryEquipmentItemInfoUnEquipImage.sprite = player.equipedAmuletItem.itemImage;

                // 장비아이템 이름을 보여준다
                inventoryEquipmentItemInfoUnEquipNameText.text = player.equipedAmuletItem.itemName;

                // 장비아이템 스탯을 보여준다
                inventoryEquipmentItemInfoUnEquipStatusText.text = "크리티컬확률 + " + player.equipedAmuletItem.criticalPercentage.ToString() + " %" + System.Environment.NewLine + "크리티컬데미지 + " + player.equipedAmuletItem.criticalDamage + " %";

                // 해제 할 인벤토리 아이템 저장
                unequipInventoryItemSave = player.equipedAmuletItem;
            }
        }
        else if (slotType == player.equipedPetItem.equipmentItemType.ToString())
        {
            // 비어있는 장비슬롯을 누르면 이전에 착용했던 장비해제패널이 나오는문제
            if (player.isPet)
            {
                // 펫
                // 장비아이템 해제패널 활성화
                inventoryEquipmentItemInfoUnEquipPanel.SetActive(true);

                // 장비아이템 이미지를 보여준다
                inventoryEquipmentItemInfoUnEquipImage.sprite = player.equipedPetItem.itemImage;

                // 장비아이템 이름을 보여준다
                inventoryEquipmentItemInfoUnEquipNameText.text = player.equipedPetItem.itemName;

                // 장비아이템 스탯을 보여준다
                inventoryEquipmentItemInfoUnEquipStatusText.text = "귀여움 + " + player.equipedPetItem.cute.ToString();

                // 해제 할 인벤토리 아이템 저장
                unequipInventoryItemSave = player.equipedPetItem;
            }
        }

        // 사운드
        SoundManager.instance.SFXPlay("ButtonSound");
    }

    public void UnEquipment()
    {
        // 장비 해제
        // 인벤토리가 가득찼는데 장비해제가 되는 문제 : 인벤토리가 가득차면 리턴
        if(inventory.inventoryItems.Count == inventory.InventorySlotCnt)
        {
            return;
        }

        // 해제한 장비 인벤토리로 이동
        inventory.AddInventoryItem(unequipInventoryItemSave);

        // 장비 해제패널 비활성화
        inventoryEquipmentItemInfoUnEquipPanel.SetActive(false);

        // 무기
        if(unequipInventoryItemSave.equipmentItemType == EquipmentItemType.Weapon)
        {
            // 장착된 무기 없음
            player.isWeapon = false;

            // 장비아이템 이미지 제거
            player.weaponSlotImage.sprite = equipmentSlotImageSpirte[0];

            // 알파값을 다시 100으로 변경
            Color color = player.weaponSlotImage.color;
            color.a = 0.4f;
            player.weaponSlotImage.color = color;

            // 플레이어 스탯 원래대로
            player.damage = player.damage - unequipInventoryItemSave.attack;

            // 현재 장착되어있는 무기 없음
            player.isWeapon = false;
        }
        else if (unequipInventoryItemSave.equipmentItemType == EquipmentItemType.Armor)
        {
            // 장비아이템 이미지 제거
            player.armorSlotImage.sprite = equipmentSlotImageSpirte[1];

            // 알파값을 다시 100으로 변경
            Color color = player.armorSlotImage.color;
            color.a = 0.4f;
            player.armorSlotImage.color = color;

            // 플레이어 스탯 원래대로
            player.curHealth = player.curHealth - unequipInventoryItemSave.health;
            player.maxHealth = player.maxHealth - unequipInventoryItemSave.health;

            // 현재 장착되어있는 갑옷 없음
            player.isArmor = false;
        }
        else if (unequipInventoryItemSave.equipmentItemType == EquipmentItemType.Glove)
        {
            // 장비아이템 이미지 제거
            player.gloveSlotImage.sprite = equipmentSlotImageSpirte[2];

            // 알파값을 다시 100으로 변경
            Color color = player.gloveSlotImage.color;
            color.a = 0.4f;
            player.gloveSlotImage.color = color;

            // 플레이어 스탯 원래대로
            player.attackSpeed = player.attackSpeed - unequipInventoryItemSave.attackSpeed;

            // 현재 장착되어있는 장갑 없음
            player.isGlove = false;
        }
        else if (unequipInventoryItemSave.equipmentItemType == EquipmentItemType.Shoes)
        {
            // 장비아이템 이미지 제거
            player.shoesSlotImage.sprite = equipmentSlotImageSpirte[3];

            // 알파값을 다시 100으로 변경
            Color color = player.shoesSlotImage.color;
            color.a = 0.4f;
            player.shoesSlotImage.color = color;

            // 플레이어 스탯 원래대로
            player.joystickScript.moveSpeed = player.joystickScript.moveSpeed - unequipInventoryItemSave.moveSpeed;

            // 현재 장착되어있는 신발 없음
            player.isShoes = false;
        }
        else if (unequipInventoryItemSave.equipmentItemType == EquipmentItemType.Amulet)
        {
            // 장비아이템 이미지 제거
            player.amuletSlotImage.sprite = equipmentSlotImageSpirte[4];

            // 알파값을 다시 100으로 변경
            Color color = player.amuletSlotImage.color;
            color.a = 0.4f;
            player.amuletSlotImage.color = color;

            // 플레이어 스탯 원래대로
            player.criticalPercentage = player.criticalPercentage - unequipInventoryItemSave.criticalPercentage;
            player.criticalDamage = player.criticalDamage - unequipInventoryItemSave.criticalDamage;

            // 현재 장착되어있는 아뮬렛 없음
            player.isAmulet = false;
        }
        else if (unequipInventoryItemSave.equipmentItemType == EquipmentItemType.Pet)
        {
            // 장비아이템 이미지 제거
            player.petSlotImage.sprite = equipmentSlotImageSpirte[5];

            // 알파값을 다시 100으로 변경
            Color color = player.petSlotImage.color;
            color.a = 0.4f;
            player.petSlotImage.color = color;

            // 현재 장착되어있는 펫 비활성화
            player.spawnedPet.SetActive(false);

            // 현재 장착되어있는 펫 없음
            player.isPet = false;
        }

        // 사운드
        SoundManager.instance.SFXPlay("UnEquipSound");
    }
}

