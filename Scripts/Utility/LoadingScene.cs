using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingScene : MonoBehaviour
{
    // 로딩바
    public Slider loadingBarSlider;

    // 진행상황 텍스트
    public Text loadingText;

    // 로딩 게임 이미지
    public Image loadingGameImage;

    // 로딩 게임 스프라이트
    // 0 : 법사 1 : 소드 2 : 블랙스미스 3 : 성기사
    public Sprite[] loadingGameImageSprite;

    // 게임 팁 텍스트
    public Text tipText;

    // 게임 팁 텍스트 문자열
    public string[] tipString;

    // 랜덤 팁
    public int tipRandom;

    // 팁이 바뀌었는지 체크 : 여러번 바뀌는 문제
    public bool[] isTip;

    // 선택된 캐릭터
    public string characterType;

    void Start()
    {
        // 선택된 캐릭터
        characterType = DataManager.instance.character.ToString();

        // 선택된 캐릭터에 따른 로딩 게임 이미지 변경
        // 기본 : 궁수
        if(characterType.Equals("Mage"))
        {
            // 법사
            loadingGameImage.sprite = loadingGameImageSprite[0];
        }
        else if(characterType.Equals("Sword"))
        {
            // 소드
            loadingGameImage.sprite = loadingGameImageSprite[1];
        }
        else if (characterType.Equals("Blacksmith"))
        {
            // 블랙스미스
            loadingGameImage.sprite = loadingGameImageSprite[2];
        }
        else if (characterType.Equals("Holyknight"))
        {
            // 성기사
            loadingGameImage.sprite = loadingGameImageSprite[3];
        }

        // 로딩씬으로 넘어오면 로딩진행상황 코루틴을 실행한다
        StartCoroutine(LoadGameSceneProcess());
    }

    void Update()
    {
        // 진행상황
        int loadingProcess = Mathf.CeilToInt(loadingBarSlider.value * 100);

        // 진행상황 %로 표시
        loadingText.text = loadingProcess.ToString() + "%";

        // 진행상황에 따라 게임 팁 변경
        if (loadingProcess == 60 && !isTip[0])
        {
            isTip[0] = true;
            tipRandom = Random.Range(0, tipString.Length); // 0~10
            tipText.text = tipString[tipRandom];
        }
        else if (loadingProcess == 70 && !isTip[1])
        {
            isTip[1] = true;
            tipRandom = Random.Range(0, tipString.Length); // 0~10
            tipText.text = tipString[tipRandom];
        }
        else if (loadingProcess == 80 && !isTip[2])
        {
            isTip[2] = true;
            tipRandom = Random.Range(0, tipString.Length); // 0~10
            tipText.text = tipString[tipRandom];
        }
        else if (loadingProcess == 90 && !isTip[3])
        {
            isTip[3] = true;
            tipRandom = Random.Range(0, tipString.Length); // 0~10
            tipText.text = tipString[tipRandom];
        }
    }

    IEnumerator LoadGameSceneProcess()
    {
        // 로딩 진행상황
        // 비동기 방식 : 씬 전환 진행상황중에 다른작업을 수행 할 수 있음
        AsyncOperation op = SceneManager.LoadSceneAsync(2);

        // true이면 로딩작업이 완료되면 바로 다음씬으로 넘어간다
        op.allowSceneActivation = false;

        // 시간 측정
        float timer = 0f;

        // 씬전환이 끝나지 않았으면
        while(!op.isDone)
        {
            // 유니티 엔진에 제어권을 넘겨줌
            yield return null;

            // 50퍼 까지 로딩 진행상황을 로딩바에 채움
            if(op.progress < 0.5f)
            {
                loadingBarSlider.value = op.progress;
            }
            else // 50 부터는
            {
                // 그런데 로딩이 너무빨라서
                // 바로 여기가 실행됨
                // 페이크 시간에 따라서
                timer += Time.unscaledDeltaTime;

                // 로딩바를 채움
                loadingBarSlider.value = Mathf.Lerp(0.5f, 1f, timer * 0.1f);

                // 로딩바가 전부 채워졌으면
                if (loadingBarSlider.value >= 1f)
                {
                    // 게임화면을 불러온다
                    op.allowSceneActivation = true;
                    yield break;
                }
            }
        }
    }
}
