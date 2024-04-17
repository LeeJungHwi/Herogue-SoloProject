using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 순차적인 퀘스트 -> 다음 퀘스트를 가지고있음
public interface ISequential
{
    public QuestBase NextQuest { get; set; }
}
