using UnityEngine;
using UnityEngine.SceneManagement;

// 메인화면 UI 관리
public class MainManager : MonoBehaviour
{
    // 메인패널
    [SerializeField] private GameObject mainPanel;

    // 셀렉트패널
    [SerializeField] private GameObject selectPanel;

    // 옵션패널
    [SerializeField] private GameObject optionPanel;

    // 메인타이틀 이미지
    [SerializeField] private GameObject mainTitleImage;

    // 캐릭터선택창
    public void GameStart()
    {
        // 메인패널 비활성화
        mainPanel.SetActive(false);

        // 셀렉트패널 활성화
        selectPanel.SetActive(true);

        // 사운드
        SoundManager.instance.MainSFXPlay("ButtonSound", SoundManager.instance.mainSfxList[0]);
    }

    // 로딩화면
    public void Select()
    {
        // 로딩화면
        SceneManager.LoadScene(1);

        // 마을 배경음악
        SoundManager.instance.BgmSoundPlay(SoundManager.instance.bgmList[1]);

        // 사운드
        SoundManager.instance.MainSFXPlay("ButtonSound", SoundManager.instance.mainSfxList[0]);
    }

    // 불러오기
    public void GameLoad()
    {
        // 캐릭터 타입 로드 => 저장된 파일이 없으면 X
        if(!SaveManager.instance.characterSave.CharacterTypeLoad()) return;

        // 로딩화면
        SceneManager.LoadScene(1);

        // 마을 배경음악
        SoundManager.instance.BgmSoundPlay(SoundManager.instance.bgmList[1]);

        // 사운드
        SoundManager.instance.MainSFXPlay("ButtonSound", SoundManager.instance.mainSfxList[0]);
    }

    // 옵션
    public void Option()
    {
        // 메인패널 비활성화
        mainPanel.SetActive(false);

        // 옵션패널 활성화
        optionPanel.SetActive(true);

        // 사운드
        SoundManager.instance.MainSFXPlay("ButtonSound", SoundManager.instance.mainSfxList[0]);
    }

    // 메인
    public void GoToMain(string panelType)
    {
        // 셀렉트패널 및 옵션패널에서 메인패널로가는 함수
        if(panelType == "Select") selectPanel.SetActive(false);
        else if(panelType == "Option") optionPanel.SetActive(false);

        // 메인패널 활성화
        mainPanel.SetActive(true);

        // 사운드
        SoundManager.instance.MainSFXPlay("ButtonSound", SoundManager.instance.mainSfxList[0]);
    }

    // 중료
    public void Exit()
    {
        // 사운드
        SoundManager.instance.MainSFXPlay("ButtonSound", SoundManager.instance.mainSfxList[0]);
        
        // 게임종료
        Application.Quit();
    }

    // 프레스버튼 클릭
    public void pressBtn()
    {
        // 메인타이틀 이미지 비활성화
        mainTitleImage.SetActive(false);

        // 사운드
        SoundManager.instance.MainSFXPlay("ButtonSound", SoundManager.instance.mainSfxList[0]);
    }
}
