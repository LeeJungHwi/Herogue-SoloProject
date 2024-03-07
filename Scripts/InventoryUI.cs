using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 인벤토리 슬롯을 늘리는 스크립트
public class InventoryUI : MonoBehaviour
{
    // 인벤토리 슬롯
    public InventorySlot[] inventorySlots;

    // 인벤토리 슬롯을 관리하는 변수
    public Transform slotHolder;

    // 인벤토리
    Inventory inventory;

    // 인벤토리 슬롯 확장 횟수
    public int inventorySlotExpansionCnt;

    void Start()
    {
        // 인벤토리
        inventory = Inventory.instance;

        // 인벤토리 슬롯
        inventorySlots = slotHolder.GetComponentsInChildren<InventorySlot>();

        // OnSlotCountChange 이벤트대리자가 대리 할 함수 등록
        inventory.onSlotCountChange += InventorySlotChange;

        // OnChangeInventoryItem 이벤트대리자가 대리 할 함수 등록
        inventory.onChangeInventoryItem += RedrawSlotUI;

        // 선택된 캐릭터와 다르면 캔버스 삭제
        if (DataManager.instance.character.ToString() + "Canvas" != gameObject.name)
        {
            gameObject.SetActive(false);
        }
    }

    public void InventorySlotChange(int val)
    {
        // OnSlotCountChange 이벤트대리자가 대리 할 함수
        // 확장된 슬롯만큼 슬롯 활성화
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            // 인벤토리 슬롯 번호 저장
            inventorySlots[i].inventorySlotNum = i;

            // 슬롯 카운트의 개수만 남기고 슬롯 비활성화
            if (i < inventory.InventorySlotCnt)
            {
                inventorySlots[i].GetComponent<Button>().interactable = true;
            }
            else
            {
                inventorySlots[i].GetComponent<Button>().interactable = false;
            }
        }
    }

    // 인벤토리 슬롯 확장 함수
    public void AddInventorySlot()
    {
        if(inventorySlotExpansionCnt < 4) // 4번까지 확장 가능
        {
            // 인벤토리 슬롯 확장
            inventory.InventorySlotCnt = inventory.InventorySlotCnt + 4;

            // 인벤토리 슬롯 확장 횟수 증가
            inventorySlotExpansionCnt++;

            // 인벤토리 슬롯 확장권 제거
            Inventory.instance.RemoveInventoryItem(InventorySlot.inventorySlotNumSave);

            // 인벤토리 슬롯 확장 패널 닫기
            inventorySlots[0].inventoryExpansionSlotItemInfoPanel.SetActive(false);

            // 사운드
            SoundManager.instance.SFXPlay("ButtonSound");
        }
        else
        {
            // 인벤토리 슬롯 확장 불가
            // 사운드 : 장비 장착 실패 소리와 같음
            SoundManager.instance.SFXPlay("FailEquipSound");
        }
    }

    void RedrawSlotUI()
    {
        // OnChangeInventoryItem 이벤트대리자가 대리 할 함수
        // 인벤토리 갱신 함수
        for(int i = 0; i < inventorySlots.Length; i++)
        {
            // 현재 인벤토리 아이템슬롯을 제거한다
            inventorySlots[i].RemoveSlot();
        }
        
        for(int i = 0; i < inventory.inventoryItems.Count; i++)
        {
            // 인벤토리 아이템을 추가한후
            inventorySlots[i].inventoryItem = inventory.inventoryItems[i];

            // 포션 사용을 위해서 플레이어 스크립트를 넘겨줌
            inventory.inventoryItems[i].player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

            // 인벤토리를 다시 그린다
            inventorySlots[i].UpdateSlotUI();
        }
    }
}
