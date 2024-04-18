using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Quest/Count/ShopBuy")]
public class ShopBuyQuest : CountBase, ISequential
{
    // 다음 퀘스트
    [SerializeField]
    private QuestBase nextQuest;
    public QuestBase NextQuest 
    { 
        get { return nextQuest; }
        set { nextQuest = value; }
    }
    public List<ItemType> itemType = new List<ItemType>(); // 아이템 타입
    

    // 퀘스트 완료
    public override void Complete()
    {
        // 퀘스트 보상
        Reward();

        // 다음 퀘스트 추가
        QuestManager.instance.AddQuest(nextQuest);
        
        // 완료된 퀘스트 삭제
        QuestManager.instance.DeleteQuest(this);

        // 사운드
        SoundManager.instance.SFXPlay(ObjType.퀘스트완료소리);

        // 퀘스트 경계 비활성화
        GameObject.FindGameObjectWithTag("QuestBorder").gameObject.SetActive(false);

        // 퀘스트 진행방향 화살표 활성화
        GameObject.FindGameObjectWithTag("QuestNaviArrow").gameObject.GetComponent<SpriteRenderer>().enabled = true;
    }
}
