using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonDecoSpawner : MonoBehaviour
{
    // 방 모델
    private RoomTemplates templates;

    // 풀링
    private PoolingManager poolManager;

    private void Start()
    {
        // 생성된 방이 리스트에 추가된다
        templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();

        // 던전 장식 생성
        poolManager = GameObject.FindGameObjectWithTag("PoolManager").GetComponent<PoolingManager>();
        int dungeonDecorationRandom = Random.Range(0, templates.DungeonDecorationList.Length); // 0~28
        GameObject instantDungeonDecoration = poolManager.GetObj((ObjType)((int)ObjType.던전장식1 + dungeonDecorationRandom));
        instantDungeonDecoration.transform.position = transform.position;
        instantDungeonDecoration.transform.rotation = Quaternion.identity;
        instantDungeonDecoration.transform.SetParent(transform);
    }
}
