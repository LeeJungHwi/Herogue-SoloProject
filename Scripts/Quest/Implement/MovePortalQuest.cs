using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Quest/Objective/MovePortal")]
public class MovePortalQuest : ObjectiveBase, ISequential
{
    // 다음 퀘스트
    [SerializeField]
    private QuestBase nextQuest;
    public QuestBase NextQuest 
    { 
        get { return nextQuest; }
        set { nextQuest = value; }
    }

    // 퀘스트 완료
    public override void Complete()
    {
        // 다음 퀘스트 추가
        AddNextQuest();
        
        // 완료된 퀘스트 삭제
        QuestManager.instance.DeleteQuest(this);
    }

    // 다음 퀘스트 추가
    public void AddNextQuest()
    {
        QuestManager.instance.AddQuest(nextQuest);
    }
}
