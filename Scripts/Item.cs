using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    // 아이템 타입
    public enum Type { ActiveSkill, PassiveSkill, Coin };

    // 아이템 타입을 참조할 변수
    public Type type; 

    // 아이템의 값
    public int value;

    // 아이템을 이미 먹었는지 체크
    public bool hasItem;

    // 패시브 스킬 설명
    public string skillContent;

    // 오브젝트 타입
    public ObjType objType;

    void Update()
    {
        // 아이템 회전
        transform.Rotate(Vector3.right * 30 * Time.deltaTime);
    }
}
