using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddRoom : MonoBehaviour
{
    // 방 모델
    private RoomTemplates templates;

    // 오브젝트 타입
    public ObjType type;
    
    // 방이 추가되었는지 체크 : 풀링에서 활성화할때 false로 바꿔서 활성화시켜준다
    [HideInInspector] public bool isAdd;

    // RoomBFS
    private RoomBFS roomBFS;

    private void Start()
    {
        // 생성된 방이 리스트에 추가된다
        templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();

        // 그래프에 오브젝트타입 저장
        roomBFS = GameObject.FindGameObjectWithTag("RoomBFS").GetComponent<RoomBFS>();
    }

    private void Update()
    {
        if(!isAdd)
        {
            // 추가된 상태가 아닐때에만 리스트에 추가한다
            templates.rooms.Add(Tuple.Create(this.gameObject, type));

            // 그래프에 오브젝트타입 저장
            roomBFS.graph[(int)gameObject.transform.position.x / 200, (int)gameObject.transform.position.z / 200] = type;

            // 추가된상태
            isAdd = true;
        }
    }
}
