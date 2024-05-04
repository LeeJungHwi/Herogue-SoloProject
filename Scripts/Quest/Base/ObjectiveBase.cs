using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 목표 베이스 -> 특정 행동 퀘스트가 상속
public class ObjectiveBase : QuestBase
{
    private Player player; // 플레이어
    [SerializeField] private string tag; // 가까운 오브젝트 태그

    // 가까운 오브젝트의 태그가 퀘스트에서 설정한 태그와 같으면 완료
    public override void Check() { if(GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().nearObject.tag == tag) Complete(); }
}
