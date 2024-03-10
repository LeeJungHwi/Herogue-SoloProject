using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 상점 아이템 데이터베이스
public class ShopDatabase : MonoBehaviour
{
    // 상점 아이템을 저장 할 리스트
    public List<InventoryItem> shopItemList = new List<InventoryItem>();
    
    // 상점 아이템을 통해서 얻을 수 있는 아이템 : 랜덤스킬주문서
    // 패시브 아이템을 저장 할 리스트
    public List<InventoryItem> passiveItemList = new List<InventoryItem>();

    // 상점 아이템을 통해서 얻을 수 있는 아이템 : 랜덤펫뽑기
    // 펫 아이템을 저장 할 리스트
    public List<InventoryItem> petItemList = new List<InventoryItem>();
}
