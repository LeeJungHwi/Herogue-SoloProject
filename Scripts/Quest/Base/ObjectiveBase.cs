using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 목표 베이스 -> 이동 퀘스트가 상속
public class ObjectiveBase : QuestBase
{
    public Player player; // 플레이어
    public string tag; // 가까운 오브젝트 태그

    public override void Check()
    {
        // 가까운 오브젝트의 태그가 퀘스트에서 설정한 태그와 같으면 완료
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        if(player.nearObject.tag == tag) Complete();
    }

    // 퀘스트 완료
    public override void Complete()
    {
        // 완료된 퀘스트 삭제
        QuestManager.instance.DeleteQuest(this);
    }
}
