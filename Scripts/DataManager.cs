using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 선택가능한 캐릭터 종류
public enum Character
{
    Sword, Archer, Mage, Blacksmith, Holyknight
}

// 메인화면에서 이루어진 데이터가 게임화면까지 유지되는 데이터 관리
public class DataManager : MonoBehaviour
{
    // 싱글톤
    public static DataManager instance;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else if(instance != null)
        {
            return;
        }

        DontDestroyOnLoad(gameObject); // 씬전환시 파괴되지 않게

        gameSpeed = 1f; // 메인에서 게임 속도 초기화
    }

    public Character character; // 선택된 캐릭터를 저장 할 변수

    public float gameSpeed; // 게임 속도를 저장 할 변수

    // 게임 속도 조절 함수
    public void SetGameSpeed(float speed)
    {
        gameSpeed = speed;
    }
}

