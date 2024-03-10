using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 플레이어에 할당 되어있음 : 넘겨받은 인벤토리 필드아이템을 인벤토리 슬롯에 추가 및 삭제 스크립트
public class Inventory : MonoBehaviour
{
    // 싱글톤
    public static Inventory instance;
    private void Awake()
    {
        if(instance!= null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    // 이벤트대리자
    // 인벤토리 슬롯의 개수가 변경되면 이벤트에 등록된 InventorySlotChange함수를 호출
    public delegate void OnSlotCountChange(int val);
    public OnSlotCountChange onSlotCountChange;

    // 이벤트대리자
    // 인벤토리에 아이템이 추가되거나 삭제되면 이벤트에 등록된 RedrawSlotUI함수를 호출
    public delegate void OnChangeInventoryItem();
    public OnChangeInventoryItem onChangeInventoryItem;

    // 획득한 인벤토리 아이템을 저장 할 리스트
    public List<InventoryItem> inventoryItems = new List<InventoryItem>();

    // 인벤토리 슬롯의 개수
    private int inventorySlotCnt;
    public int InventorySlotCnt
    {
        get => inventorySlotCnt;
        set
        {
            inventorySlotCnt = value;
            onSlotCountChange.Invoke(inventorySlotCnt);
        }
    }

    // 오브젝트 풀
    private PoolingManager poolingManager;

    void Start()
    {
        // 인벤토리 슬롯 초기화
        InventorySlotCnt = 4;

        // 오브젝트 풀
        poolingManager = GameObject.FindGameObjectWithTag("PoolManager").GetComponent<PoolingManager>();
    }

    // 인벤토리에 아이템을 추가하는 함수
    public bool AddInventoryItem(InventoryItem inventoryItem)
    {
        // 슬롯이 남아있다면
        if(inventoryItems.Count < InventorySlotCnt)
        {
            // 넘겨받은 아이템을 인벤토리에 추가
            inventoryItems.Add(inventoryItem);

            // 인벤토리 다시 그리기
            onChangeInventoryItem.Invoke();

            // 아이템 추가 성공
            return true;
        }

        // 아이템 추가 실패
        return false;
    }

    // 인벤토리에 아이템을 제거하는 함수
    public void RemoveInventoryItem(int index)
    {
        // 넘겨받은 아이템 번호에 해당하는 아이템을 제거
        inventoryItems.RemoveAt(index);

        // 인벤토리 다시 그리기
        onChangeInventoryItem.Invoke();
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("InventoryItem"))
        {
            // 필드 아이템
            InventoryFieldItems inventoryFieldItems = other.GetComponent<InventoryFieldItems>();

            // 필드 아이템 정보를 넘겨 인벤토리에 추가
            if(AddInventoryItem(inventoryFieldItems.inventoryItem))
            {
                // 필드 아이템 비활성화
                poolingManager.ReturnObj(inventoryFieldItems.gameObject, inventoryFieldItems.inventoryItem.type);
            }

            // 사운드
            SoundManager.instance.SFXPlay(ObjType.아이템소리);
        }
    }
}
