using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 게임화면 UI 관리
public class GameManager : MonoBehaviour
{
    // Player 스크립트
    [SerializeField] private Player player;

    // RoomTemplates 스크립트
    [SerializeField] private RoomTemplates templates;

    // 스테이지 텍스트
    [SerializeField] private Text stageText;

    // 옵션패널
    [SerializeField] private GameObject optionPanel;

    // 배경음 슬라이더
    [SerializeField] private Slider bgmSlider;

    // 효과음 슬라이더
    [SerializeField] private Slider sfxSlider;

    // 게임속도 슬라이더
    [SerializeField] private Slider gameSpeedSlider;

    // 플레이어 정보패널
    [SerializeField] private GameObject playerInfoPanel;

    // 스킬리스트 패널
    [SerializeField] private GameObject skillListPanel;

    // 상점 관련 닫혀야 할 패널
    [SerializeField] private GameObject[] closeShopPanel;

    // 플레이어 정보 관련 닫혀야 할 패널
    [SerializeField] private GameObject[] closePlayerInfoPanel;

    // 액티브스킬 패널
    [SerializeField] private GameObject abilityPanel;

    // 퀘스트 패널
    [SerializeField] private GameObject questPanel;

    // 처음인지 체크
    [SerializeField] private bool[] isFirst;

    // IndicateHand
    [SerializeField] private GameObject[] indicateHandImage;

    private void Start()
    {
        // 게임화면 시작시 배경음 슬라이더값 세팅
        bgmSlider.value = SoundManager.instance.bgmVolume;

        // 게임화면 시작시 효과음 슬라이더값 세팅
        sfxSlider.value = SoundManager.instance.sfxVolume;

        // 게임화면 시작시 게임속도 슬라이더값 세팅
        gameSpeedSlider.value = DataManager.instance.gameSpeed;

        // 게임화면 시작시 배경음 슬라이더에 이벤트 리스너 등록
        bgmSlider.onValueChanged.AddListener(SoundManager.instance.SetBgmVolume);

        // 게임화면 시작시 효과음 슬라이더에 이벤트 리스너 등록
        sfxSlider.onValueChanged.AddListener(SoundManager.instance.SetSfxVolume);

        // 게임화면 시작시 게임속도 슬라이더에 이벤트 리스너 등록
        gameSpeedSlider.onValueChanged.AddListener(DataManager.instance.SetGameSpeed);

        // 선택된 캐릭터와 다르면 게임매니저 삭제
        if (DataManager.instance.character.ToString() + "GameManager" != gameObject.name) gameObject.SetActive(false);
    }

    private void LateUpdate()
    {
        // 스테이지 텍스트
        if(player.isShelter) stageText.text = "마을";
        else stageText.text = "STAGE " + (templates.currentStage + 1);

        // 게임속도
        Time.timeScale = DataManager.instance.gameSpeed;
    }

    // 패널 활성화 함수
    public void ActivePanel(string panelType)
    {
        if (panelType == "Option") optionPanel.SetActive(true);
        else if (panelType == "PlayerInfo") playerInfoPanel.SetActive(true);
        else if (panelType == "SkillList") skillListPanel.SetActive(true);
        else if (panelType == "Ability") abilityPanel.SetActive(true);
        else if(panelType == "Quest")
        {
            // 퀘스트 패널이면
            // 퀘스트 패널 활성화
            questPanel.SetActive(true);

            // 퀘스트 UI 업데이트
            QuestManager.instance.UpdateUI();

            // 새로운 퀘스트 추가 이펙트 비활성화
            QuestManager.instance.newQuestEffect.gameObject.SetActive(false);
        }

        // 사운드
        SoundManager.instance.SFXPlay(ObjType.버튼소리);
    }

    // 패널 비활성화 함수
    public void DeActivePanel(string panelType)
    {
        if (panelType == "Option")
        {
            // 옵션 패널이면
            // 옵션패널 비활성화
            optionPanel.SetActive(false);

            // 옵션 IndicateHand 비활성화
            // 퀘스트 IndicateHand 활성화
            if(!isFirst[0])
            {
                isFirst[0] = true;
                indicateHandImage[0].SetActive(false);
                indicateHandImage[1].SetActive(true);
            }
        }
        else if (panelType == "PlayerInfo")
        {
            // 플레이어 정보 패널이면
            // 아이템 장착 및 사용 패널을 닫지않고 플레이어 정보 패널을 나간후
            // 상점에서 해당 아이템을 팔고 다시 플레이어 정보 패널로 오면 사용 및 장착 할 수 있는문제
            // 플레이어 정보 패널안에있는 패널중에 닫혀야 할 패널 닫기
            for (int i = 0; i < closePlayerInfoPanel.Length; i++) closePlayerInfoPanel[i].SetActive(false);

            // 플레이어 정보 패널 비활성화
            playerInfoPanel.SetActive(false);
        }
        else if(panelType == "SkillList")
        {
            // 스킬리스트 패널이면
            // 스킬리스트 패널 비활성화
            skillListPanel.SetActive(false);
        }
        else if(panelType == "Shop")
        {
            // 상점 패널이면
            // 상점 판매 패널을 닫지않고 상점을 나간후
            // 인벤토리에서 장비를 장착하고 다시 상점을 열면 판매 할 수 있는 문제
            // 상점 패널 안에있는 패널중에 닫혀야 할 패널 닫기
            for(int i = 0; i < closeShopPanel.Length; i++) closeShopPanel[i].SetActive(false);

            // 상점 패널 비활성화
            player.shopPanel.SetActive(false);

            // 인벤토리 패널 위치 변경 : 인벤토리 패널 원래 위치로 이동후 첫번째 자식으로
            player.inventoryPanel.transform.SetParent(playerInfoPanel.transform);
            player.inventoryPanel.transform.SetAsFirstSibling();

            // Ability 스킬 카운트 초기화 : 널 에러
            player.abilitySkillCnt = 0;

            // 무기 카운트 초기화 : 널 에러
            player.weaponCnt = 0;

            // 현재 상점아님
            player.isShop = false;

            // 상호작용 IndicateHand 비활성화
            if(!isFirst[2] && isFirst[1])
            {
                isFirst[2] = true;
                indicateHandImage[2].SetActive(false);
            }

            // 마을 배경음악
            SoundManager.instance.BgmSoundPlay(SoundManager.instance.bgmList[1]);
        }
        else if (panelType == "Ability")
        {
            // 액티브스킬 패널이면
            // 액티브스킬 패널 비활성화
            abilityPanel.SetActive(false);
        }
        else if(panelType == "Quest")
        {
            // 퀘스트 패널이면
            // 퀘스트 패널 비활성화
            questPanel.SetActive(false);

            // 퀘스트 IndicateHand 비활성화
            // 상호작용 IndicateHand 활성화
            if(!isFirst[1] && isFirst[0])
            {
                isFirst[1] = true;
                indicateHandImage[1].SetActive(false);
                indicateHandImage[2].SetActive(true);
            }
        }

        // 사운드
        SoundManager.instance.SFXPlay(ObjType.버튼소리);
    }

    // 게임 종료
    public void Exit()
    {
        // 사운드
        SoundManager.instance.SFXPlay(ObjType.버튼소리);

        // 게임종료
        Application.Quit();
    }

    // 플레이어를 할당하는 함수
    private void SetPlayer() { player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>(); }
}
