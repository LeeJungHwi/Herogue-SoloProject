using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Quest/Count/KillNormalLoop")]
public class KillNormalLoopQuest : CountBase, ILoop
{
    // 퀘스트 완료
    public override void Complete()
    {
        // 퀘스트 보상
        Reward();

        // 퀘스트 다시 추가
        LoopQuest();

        // 완료한 퀘스트 삭제
        QuestManager.instance.DeleteQuest(this);

        // 사운드
        SoundManager.instance.SFXPlay(ObjType.포션사용소리);
    }

    // 반복퀘스트
    public void LoopQuest()
    {
        CurCnt = 0;
        QuestManager.instance.AddQuest(this);
    }
}
