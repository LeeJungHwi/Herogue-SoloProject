using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 페이드 인/아웃
public class FadeInOut : MonoBehaviour
{
    // 싱글톤
    private static FadeInOut instance;
    public static FadeInOut Instance => instance;
    public FadeInOut() { instance = this; }

    // 페이드 인/아웃 이미지 할당
    public void Awake() { Invoke("SetFadeInOutImage", 0.5f); }
    public void SetFadeInOutImage()
    {
        // 페이드 인/아웃 이미지 할당
        fadeInOutImage = GameObject.FindGameObjectWithTag("FadeInOutImage");

        // 페이드 인/아웃 이미지 비활성화
        fadeInOutImage.SetActive(false);
    }

    // 페이드 인/아웃 이미지
    private GameObject fadeInOutImage;

    // 진행시간
    private float time = 0f;

    // 진행시간 계산용
    private float timeCalc = 1f;

    // 페이드 인/아웃 코루틴 실행
    // 플레이어가 죽을때
    public void Fade() { StartCoroutine(GoFadeInOut()); }

    // 페이드 인/아웃 코루틴 실행
    // 다음스테이지, 던전들어가기
    public void Fade2() { StartCoroutine(GoFadeInOut2()); }

    // 페이드 인/아웃 코루틴
    // 플레이어가 죽을때
    private IEnumerator GoFadeInOut()
    {
        // 페이드 인/아웃 이미지 활성화
        fadeInOutImage.gameObject.SetActive(true);
        
        // 진행시간 초기화
        time = 0f;

        // 알파값을 조절해서 페이드 인/아웃 이미지 컬러에 대입
        Color alpha = fadeInOutImage.GetComponent<Image>().color;

        // 페이드 인
        // 알파값이 1 미만일때
        while(alpha.a < 1f)
        {
            // 진행시간 증가
            time += Time.deltaTime / timeCalc;

            // 알파값 증가
            alpha.a = Mathf.Lerp(0, 1, time);

            // 알파값 대입
            fadeInOutImage.GetComponent<Image>().color = alpha;

            yield return null;
        }

        // 진행시간 초기화
        time = 0f;

        // 1초 대기
        yield return new WaitForSeconds(1f);

        // 페이드 아웃
        // 알파값이 0 초과일때
        while(alpha.a > 0f)
        {
            // 진행시간 증가
            time += Time.deltaTime / timeCalc;

            // 알파값 감소
            alpha.a = Mathf.Lerp(1, 0, time);

            // 알파값 대입
            fadeInOutImage.GetComponent<Image>().color = alpha;

            yield return null;
        }

        // 페이드 인/아웃 이미지 비활성화
        fadeInOutImage.gameObject.SetActive(false);
        
        yield return null;
    }

    // 페이드 인/아웃 코루틴
    // 다음스테이지, 던전들어가기
    // 1초 대기시간이 있어서 플레이어가 먼저 이동하고 페이드 인/아웃이 실행되므로
    // 처음에 바로 알파값을 1로한후에 페이드 아웃을 진행
    private IEnumerator GoFadeInOut2()
    {
        // 페이드 인/아웃 이미지 활성화
        fadeInOutImage.gameObject.SetActive(true);

        // 진행시간 초기화
        time = 0f;

        // 알파값을 조절해서 페이드 인/아웃 이미지 컬러에 대입
        Color alpha = fadeInOutImage.GetComponent<Image>().color;

        // 알파값 최대
        alpha.a = 1f;

        // 알파값 대입
        fadeInOutImage.GetComponent<Image>().color = alpha;

        // 페이드 아웃
        // 알파값이 0 초과일때
        while (alpha.a > 0f)
        {
            // 진행시간 증가
            time += Time.deltaTime / timeCalc;

            // 알파값 감소
            alpha.a = Mathf.Lerp(1, 0, time);

            // 알파값 대입
            fadeInOutImage.GetComponent<Image>().color = alpha;

            yield return null;
        }

        // 페이드 인/아웃 이미지 비활성화
        fadeInOutImage.gameObject.SetActive(false);

        yield return null;
    }
}
