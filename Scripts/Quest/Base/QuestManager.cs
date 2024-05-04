using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using UnityEngine.UI;
using TMPro;

// 퀘스트 매니저 -> 퀘스트 추가, 제거, UI 관리
public class QuestManager : MonoBehaviour
{
    public static QuestManager instance; // 싱글톤
    public List<QuestBase> QuestList = new List<QuestBase>(); // 퀘스트리스트
    private StringBuilder tempText = new StringBuilder(); // 퀘스트텍스트에 표시할 텍스트
    [SerializeField] private Text questText; // 퀘스트텍스트
    [SerializeField] private PoolingManager poolingManager; // 풀 매니저
    public GameObject newQuestEffect; // 퀘스트창 버튼 UI에 새로운 퀘스트가 추가되었음을 표시할 이펙트

    private void Start()
    {
        // 선택된 캐릭터
        if (DataManager.instance.character.ToString() + "QuestManager" != gameObject.name) gameObject.SetActive(false);
        else instance = this;
    }

    // 퀘스트 추가
    public void AddQuest(QuestBase quest)
    {
        QuestList.Add(quest);

        // 새로운 퀘스트 추가 이펙트 활성화
        newQuestEffect.gameObject.SetActive(true);
    }

    // 퀘스트 제거
    public void DeleteQuest(QuestBase quest)
    {
        QuestList.Remove(quest);

        // 퀘스트 완료 알림
        QuestNotify($" {quest.questName} 완료");
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
            if(QuestList[i] is CountBase countBase) tempText.Append($" {countBase.CurCnt}/{countBase.completeCnt}   보상 : {countBase.rewardCoin} Gold!");
            tempText.AppendLine();
        }

        // 퀘스트텍스트에 표시
        questText.text = tempText.ToString();
    }

    // 퀘스트 진행상황 알림
    public void QuestNotify(string info)
    {
        // 퀘스트 완료 알림
        GameObject instantQuestAddedText = poolingManager.GetObj(ObjType.퀘스트완료텍스트);
        instantQuestAddedText.transform.position = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().transform.position + Vector3.up * 20;
        instantQuestAddedText.transform.rotation = poolingManager.FloationTextPrefs[3].transform.rotation;
        instantQuestAddedText.GetComponent<TMP_Text>().text = info;
    }
}
