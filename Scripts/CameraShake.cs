using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    // 싱글톤
    private static CameraShake instance;
    public static CameraShake Instance => instance;

    // 흔들림 시간
    private float shakeTime;

    // 흔들림 강도
    private float shakeIntensity;

    // CameraMove 스크립트
    private CameraMove cameraMove;

    // 생성자
    public CameraShake()
    {
        instance = this;
    }

    private void Awake()
    {
        // CameraMove 스크립트
        cameraMove = GetComponent<CameraMove>();
    }

    public void OnCameraShake(float shakeTime = 1.0f, float shakeIntensity = 0.1f)
    {
        // 카메라가 흔들리는 중일땐 실행하지 않는다
        if(cameraMove.isShake)
        {
            return;
        }

        // 카메라 흔들림 시작
        // 위치 or 회전 선택
        // 흔들림 시간
        this.shakeTime = shakeTime;

        // 흔들림 강도
        this.shakeIntensity = shakeIntensity;

        // 카메라 흔들림 시작
        StopCoroutine("ShakeByRotation");
        StartCoroutine("ShakeByRotation");
    }

    private IEnumerator ShakeByPosition()
    {
        // 위치로 흔들기
        // 흔들리기 직전 시작 위치
        Vector3 startPosition = transform.position;

        while(shakeTime > 0.0f)
        {
            // 초기위치에서 카메라위치 변동
            transform.position = startPosition + Random.insideUnitSphere * shakeIntensity;
            
            // 시간 감소
            shakeTime -= Time.deltaTime;

            yield return null;
        }

        // 다시 제자리로
        transform.position = startPosition;
    }

    private IEnumerator ShakeByRotation()
    {
        // 회전으로 흔들기
        // 카메라 흔들림 진행중
        cameraMove.isShake = true;

        // 흔들리기 직전 회전값
        Vector3 startRotation = transform.eulerAngles;

        // 회전 강도
        float power = 10f;

        while(shakeTime > 0.0f)
        {
            // 회전하길 원하는 좌표를 축으로 흔들림
            float x = 0;
            float y = 0;
            float z = Random.Range(-1f, 1f);
            transform.rotation = Quaternion.Euler(startRotation + new Vector3(x, y, z) * shakeIntensity * power);

            // 시간 감소
            shakeTime -= Time.deltaTime;

            yield return null;
        }

        // 회전값 초기화
        transform.rotation = Quaternion.Euler(startRotation);

        // 카메라 흔들림 종료
        cameraMove.isShake = false;
    }
}
