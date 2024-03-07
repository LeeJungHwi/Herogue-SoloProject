using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpBar : MonoBehaviour
{
    // HpBar 프리팹을 저장할 변수
    [SerializeField] GameObject HpBarPrefab;

    // 몬스터 자신의 위치를 저장할 변수
    Transform MonsterPos;

    // 캔버스
    private GameObject Canvas;

    // 생성된 체력바
    public GameObject instantHpBar;

    // 오브젝트 풀링
    PoolingManager poolingManager;

    // Enemy 스크립트
    Enemy enemy;

    // 체력바가 생성되었는지 체크
    public bool spawnHpBar;

    // 카메라
    Camera cam = null;

    // 카메라와의 거리
    private float distance;

    void Start()
    {
        // 카메라
        cam = Camera.main;

        // 캔버스
        Invoke("SetCanvas", 0.5f);

        // Enemy 스크립트
        enemy = GetComponent<Enemy>();

        // 오브젝트 풀링
        poolingManager = GameObject.FindGameObjectWithTag("PoolManager").GetComponent<PoolingManager>();

        // 몬스터의 위치
        MonsterPos = transform;
    }

    void Update()
    {
        // 체력바가 생성되지 않았을때에만
        // 체력바를 생성
        // HpBar가 몬스터의 위치를 따라다님
        // 몬스터마다 크기가 달라서 큰몬스터는 HpBar를 위로 더 올려줌
        if(!spawnHpBar)
        {
            instantHpBar = poolingManager.GetObj("MonsterHpBar");
            spawnHpBar = true;
        }
        if(enemy.enemyType == Enemy.Type.Bat)
        {
            instantHpBar.transform.position = cam.WorldToScreenPoint(MonsterPos.position + new Vector3(0, 15f, 0));
        }
        else if(enemy.enemyType == Enemy.Type.Bomb)
        {
            instantHpBar.transform.position = cam.WorldToScreenPoint(MonsterPos.position + new Vector3(0, 20f, 0));
        }
        else if(enemy.enemyType == Enemy.Type.Ciclop)
        {
            instantHpBar.transform.position = cam.WorldToScreenPoint(MonsterPos.position + new Vector3(0, 30f, 0));
        }
        else if(enemy.enemyType == Enemy.Type.Golem)
        {
            instantHpBar.transform.position = cam.WorldToScreenPoint(MonsterPos.position + new Vector3(0, 25f, 0));
        }
        else if(enemy.enemyType == Enemy.Type.Rabbit)
        {
            instantHpBar.transform.position = cam.WorldToScreenPoint(MonsterPos.position + new Vector3(0, 25f, 0));
        }

        // 카메라와의 거리
        distance = Vector3.Distance(cam.GetComponentInParent<Transform>().transform.position, transform.position);

        // 체력에따라 슬라이더 값 조절
        instantHpBar.GetComponent<Slider>().value = Mathf.Lerp(instantHpBar.GetComponent<Slider>().value, enemy.curHealth / enemy.maxHealth, Time.deltaTime * 15);
        instantHpBar.GetComponentInChildren<Text>().text = enemy.curHealth + " / " + enemy.maxHealth;

        // 카메라와의 거리에따른 스케일 조정
        if(distance > 200)
        {
            // 체력바 사라짐
            instantHpBar.transform.localScale = new Vector3(0, 1, 1);
        }
        else
        {
            // 체력바 생김
            instantHpBar.transform.localScale = new Vector3(1, 1, 1);
        }
    }

    void SetCanvas()
    {
        // 캔버스를 할당하는 함수
        // 캔버스 할당
        Canvas = GameObject.FindGameObjectWithTag("Canvas");
    }
}
