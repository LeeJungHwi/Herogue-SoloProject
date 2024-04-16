using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 퀘스트 타입
public enum QuestType
{
    카운트, 목표
}

// 구현한 퀘스트 타입
public enum ImplementQuestType
{
    상점가기, 구매하기, 던전포털가기, 일반몬스터잡기, 보스방가기, 보스잡기, 일반몬스터잡기반복
}

// 퀘스트 베이스 -> 스크립터블 오브젝트, 각 퀘스트 타입에 상속
public abstract class QuestBase : ScriptableObject
{
    public QuestType questType; // 퀘스트 타입
    public ImplementQuestType implementQuestType; // 구현한 퀘스트 타입
    public string questName; // 퀘스트 이름
    
    public abstract void Check(); // 퀘스트 체크
    public virtual void Complete() {} // 퀘스트 완료
}
