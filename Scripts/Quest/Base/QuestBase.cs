using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 퀘스트 베이스 -> 스크립터블 오브젝트, 각 퀘스트 베이스에 상속
public abstract class QuestBase : ScriptableObject
{
    public string questName; // 퀘스트 이름
    public abstract void Check(); // 퀘스트 체크
    public virtual void Complete() {} // 퀘스트 완료
}
