using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 퀘스트 데이터
[System.Serializable]
public class QuestData
{
    public List<QuestBase> QuestList = new List<QuestBase>(); // 퀘스트리스트
}

public class QuestSave : SaveBase<QuestData>
{
    public QuestSave() : base("quest.json") {}

    // 세이브
    public override void Save()
    {
        // 퀘스트 리스트
        saveData.QuestList = QuestManager.instance.QuestList;

        base.Save();
    }

    // 로드
    public override void Load()
    {
        base.Load();

        // 퀘스트 리스트
        SaveManager.instance.StartCoroutine(QuestListLoad());
    }

    // 퀘스트리스트 로드 => 퀘스트 매니저가 할당될 때 까지 대기
    private IEnumerator QuestListLoad()
    {
        yield return new WaitUntil(() => QuestManager.instance != null);

        QuestManager.instance.QuestList = loadData.QuestList;

        if(QuestManager.instance.QuestList[0] is QuestBase quest)
        {
            if(quest.questName != "상점으로 가기" && quest.questName != "상점에서 장비, 액티브, 패시브 1개씩 구매하기")
            {
                QuestManager.instance.questBorder.SetActive(false); // 퀘스트 경계 비활성화
            }
        }
    }
}
