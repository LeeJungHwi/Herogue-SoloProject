using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 순차적인 퀘스트 -> 다음 퀘스트와 다음 퀘스트를 추가하는 함수를 가지고있음
public interface ISequential
{
    public QuestBase NextQuest { get; set; }
    public void AddNextQuest();
}
