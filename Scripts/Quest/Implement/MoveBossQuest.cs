using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Quest/Objective/MoveBoss")]
public class MoveBossQuest : ObjectiveBase, ISequential
{
    // 다음 퀘스트
    [SerializeField]
    private QuestBase nextQuest;
    public QuestBase NextQuest 
    { 
        get { return nextQuest; }
        set { nextQuest = value; }
    }

    public override void Check()
    {
        // 보스 감지하면 바로 완료
        Complete();
    }

    // 퀘스트 완료
    public override void Complete()
    {
        // 다음 퀘스트 추가
        QuestManager.instance.AddQuest(nextQuest);
        
        // 완료된 퀘스트 삭제
        QuestManager.instance.DeleteQuest(this);

        // 사운드
        SoundManager.instance.SFXPlay(ObjType.퀘스트완료소리);
    }
}
