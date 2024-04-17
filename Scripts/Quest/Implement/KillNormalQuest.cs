using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Quest/Count/KillNormal")]
public class KillNormalQuest : CountBase, ISequential
{
    // 다음 퀘스트
    [SerializeField]
    private QuestBase nextQuest;
    public QuestBase NextQuest 
    { 
        get { return nextQuest; }
        set { nextQuest = value; }
    }
    public List<Type> enemyType = new List<Type>(); // 몬스터 타입

    // 퀘스트 완료
    public override void Complete()
    {
        // 다음 퀘스트 추가
        AddNextQuest();
        
        // 완료된 퀘스트 삭제
        QuestManager.instance.DeleteQuest(this);

        // 보스방 체크
        Physics.IgnoreLayerCollision(7, 9, false);
    }

    // 다음 퀘스트 추가
    public void AddNextQuest()
    {
        QuestManager.instance.AddQuest(nextQuest);
    }
}
