using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 시간에 따른 낮과 밤
public class ControlSky : MonoBehaviour
{
    // 낮
    public Material dayMat;

    // 밤
    public Material nightMat;

    // 초저녁
    public Material earlyEveningMat;

    // 새벽
    public Material dawnMat;

    // 낮 빛
    public GameObject dayLight;

    // 밤 빛
    public GameObject nightLight;

    // 초저녁 빛
    public GameObject earlyEveningLight;

    // 새벽 빛
    public GameObject dawnLight;

    // 낮 안개
    public Color dayFog;

    // 밤 안개
    public Color nightFog;

    // 초저녁 안개
    public Color earlyEveningFog;

    // 새벽 안개
    public Color dawnFog;

    // 하루의 시간
    public float dayTime;

    // 흘러간 시간
    private float passedTime;

    // 낮과 밤이 전환되었는지 체크
    private bool isSwap;

    void Update()
    {
        // 스카이박스 회전
        RenderSettings.skybox.SetFloat("_Rotation", Time.time * 1.5f);

        // 시간이 흘러간다
        passedTime += Time.deltaTime;

        // 하루의 반복
        if(passedTime >= dayTime)
        {
            passedTime = 0;
        }

        // 낮 -> 초저녁 -> 밤 -> 새벽 -> 낮
        if(!isSwap)
        {
            if (Mathf.FloorToInt(dayTime * 0.3f) == Mathf.FloorToInt(passedTime))
            {
                // 낮 -> 초저녁
                ChangeSky(earlyEveningMat, earlyEveningFog, earlyEveningLight, dayLight);
            }
            else if (Mathf.FloorToInt(dayTime * 0.5f) == Mathf.FloorToInt(passedTime))
            {
                // 초저녁 -> 밤
                ChangeSky(nightMat, nightFog, nightLight, earlyEveningLight);
            }
            else if (Mathf.FloorToInt(dayTime * 0.7f) == Mathf.FloorToInt(passedTime))
            {
                // 밤 -> 새벽
                ChangeSky(dawnMat, dawnFog, dawnLight, nightLight);
            }
            else if (Mathf.FloorToInt(dayTime * 0.9f) == Mathf.FloorToInt(passedTime))
            {
                // 새벽 -> 낮
                ChangeSky(dayMat, dayFog, dayLight, dawnLight);
            }
            isSwap = false;
        }
    }

    void ChangeSky(Material curMatType, Color curFogType, GameObject curLightType, GameObject preLightType)
    {
        // 시간에 따른 하늘 변화
        isSwap = true;
        RenderSettings.skybox = curMatType;
        RenderSettings.fogColor = curFogType;
        curLightType.SetActive(true);
        preLightType.SetActive(false);
    }
}
