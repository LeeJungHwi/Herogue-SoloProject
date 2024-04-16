using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 카운트 베이스 -> 킬, 구매 퀘스트가 상속
public class CountBase : QuestBase
{
    public int completeCnt; // 완료 개수
    private int curCnt; // 현재 개수
    public int CurCnt 
    {
        get { return curCnt; }
        set 
        {
	        // 카운팅되면 퀘스트 체크
            curCnt = value;
            Check();
        }
    }

    // 퀘스트 체크
    public override void Check()
    {
	    // 완료 개수 이상이되면 완료
        if (curCnt >= completeCnt) Complete();
    }

    // 퀘스트 완료
    public override void Complete()
    {
        // 완료된 퀘스트 삭제
        QuestManager.instance.DeleteQuest(this);
    }
}
