using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Quest/Count/KillNormalLoop")]
public class KillNormalLoopQuest : CountBase, ILoop
{
    public List<Type> enemyType = new List<Type>(); // 몬스터 타입

    // 퀘스트 완료
    public override void Complete()
    {
        // 퀘스트 다시 추가
        LoopQuest();

        // 완료한 퀘스트 삭제
        QuestManager.instance.DeleteQuest(this);
    }

    // 반복퀘스트
    public void LoopQuest()
    {
        CurCnt = 0;
        QuestManager.instance.AddQuest(this);
    }
}
