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

    // 플레이어와의 거리
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
            instantHpBar = poolingManager.GetObj(ObjType.몬스터체력바);
            spawnHpBar = true;
        }
        if(enemy.enemyType == Type.Bat)
        {
            instantHpBar.transform.position = cam.WorldToScreenPoint(MonsterPos.position + new Vector3(0, 15f, 0));
        }
        else if(enemy.enemyType == Type.Bomb)
        {
            instantHpBar.transform.position = cam.WorldToScreenPoint(MonsterPos.position + new Vector3(0, 20f, 0));
        }
        else if(enemy.enemyType == Type.Ciclop)
        {
            instantHpBar.transform.position = cam.WorldToScreenPoint(MonsterPos.position + new Vector3(0, 30f, 0));
        }
        else if(enemy.enemyType == Type.Golem)
        {
            instantHpBar.transform.position = cam.WorldToScreenPoint(MonsterPos.position + new Vector3(0, 25f, 0));
        }
        else if(enemy.enemyType == Type.Rabbit)
        {
            instantHpBar.transform.position = cam.WorldToScreenPoint(MonsterPos.position + new Vector3(0, 25f, 0));
        }

        // 플레이어와의 거리
        distance = Vector3.Distance(enemy.player.transform.position, transform.position);

        // 체력에따라 슬라이더 값 조절
        instantHpBar.GetComponent<Slider>().value = Mathf.Lerp(instantHpBar.GetComponent<Slider>().value, enemy.curHealth / enemy.maxHealth, Time.deltaTime * 15);
        instantHpBar.GetComponentInChildren<Text>().text = enemy.curHealth + " / " + enemy.maxHealth;

        // 플레이어와의 거리 및 레이 결과에 따라 체력바 보이기
        instantHpBar.transform.localScale = distance > 150 || !IsMonsterVisible() ? new Vector3(0, 1, 1) : new Vector3(1, 1, 1);
    }

    // 플레이어 시야에 몬스터가 보이는지 체크
    bool IsMonsterVisible()
    {
        Vector3 layDir = MonsterPos.position - enemy.player.transform.position; // 레이 방향
        float radius = 1.0f; // 레이 반지름
        RaycastHit[] hits; // 레이 충돌정보

        // 충돌정보 가져와서
        hits = Physics.SphereCastAll(enemy.player.transform.position + transform.up * 2, radius, layDir, Mathf.Infinity);

        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.gameObject.CompareTag("Enemy")) // 몬스터면 체력바 보이기
            {
                // 레이 디버깅
                Debug.DrawLine(enemy.player.transform.position, hit.point, Color.green);

                // 보스 방 퀘스트 처리
                if(enemy.type == ObjType.보스1)
                {
                    foreach (QuestBase quest in QuestManager.instance.QuestList)
                    {
                        if (quest is ObjectiveBase)
                        {
                            ObjectiveBase objectiveBase = quest as ObjectiveBase;
                            objectiveBase.Check();

                            break;
                        }
                    }
                }

                return true;
            }
            else if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Border")) // 벽이면 체력바 안 보이기
            {
                // 레이 디버깅
                Debug.DrawLine(enemy.player.transform.position, hit.point, Color.red);
                return false;
            }
        }

        // 레이 디버깅
        Debug.DrawRay(enemy.player.transform.position, layDir * 1000f, Color.blue);

        // 여기까지 오면 레이가 아무것도 부딪히지 않았으므로 몬스터 체력바 안 보이기
        return false;
    }

    void SetCanvas()
    {
        // 캔버스를 할당하는 함수
        // 캔버스 할당
        Canvas = GameObject.FindGameObjectWithTag("Canvas");
    }
}
