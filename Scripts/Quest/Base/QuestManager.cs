using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using UnityEngine.UI;

// 퀘스트 매니저 -> 퀘스트 추가, 제거, UI 관리
public class QuestManager : MonoBehaviour
{
    public static QuestManager instance; // 싱글톤
    private void Awake() { instance = this; }
    public List<QuestBase> QuestList = new List<QuestBase>(); // 퀘스트리스트
    private StringBuilder tempText = new StringBuilder(); // 퀘스트텍스트에 표시할 텍스트
    public Text questText; // 퀘스트텍스트

    // 퀘스트 추가
    public void AddQuest(QuestBase quest)
    {
        Debug.Log("Add : " + quest.questName);
        QuestList.Add(quest);
    }

    // 퀘스트 제거
    public void DeleteQuest(QuestBase quest)
    {
        Debug.Log("Delete : " + quest.questName);
        QuestList.Remove(quest);
    }
    
    // 퀘스트 UI 업데이트
    public void UpdateUI()
    {
        // 퀘스트텍스트에 표시할 텍스트 초기화
        tempText.Clear();

        // 퀘스트리스트에 있는 퀘스트 타입에 따라 처리
        for(int i = 0; i < QuestList.Count; i++)
        {
            tempText.Append(QuestList[i].questName);
            if(QuestList[i] is CountBase countBase)
            {
                tempText.Append($" {countBase.CurCnt}/{countBase.completeCnt} ");
            }
            tempText.AppendLine();
        }

        // 퀘스트텍스트에 표시
        questText.text = tempText.ToString();
    }
}
