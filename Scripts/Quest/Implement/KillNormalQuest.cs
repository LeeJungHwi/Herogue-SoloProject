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

    // 퀘스트 완료
    public override void Complete()
    {
        // 퀘스트 보상
        Reward();

        // 다음 퀘스트 추가
        QuestManager.instance.AddQuest(nextQuest);
        
        // 완료된 퀘스트 삭제
        QuestManager.instance.DeleteQuest(this);

        // 보스방 체크
        Physics.IgnoreLayerCollision(7, 9, false);

        // 사운드
        SoundManager.instance.SFXPlay(ObjType.포션사용소리);
    }
}
