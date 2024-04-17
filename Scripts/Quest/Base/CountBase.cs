using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 카운트 베이스 -> 카운트 관련 퀘스트 상속
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
    public int rewardCoin; // 퀘스트 보상 코인

    // 퀘스트 체크
    public override void Check()
    {
	    // 완료 개수 이상이되면 완료
        if (curCnt >= completeCnt) Complete();
    }

    // 퀘스트 보상
    protected virtual void Reward()
    {
        // 코인 보상
        Player player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        player.coin += rewardCoin;
        if(player.coin > player.maxCoin) player.coin = player.maxCoin;
    }
}
