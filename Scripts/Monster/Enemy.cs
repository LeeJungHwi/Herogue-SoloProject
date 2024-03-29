using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;
using System;

public class Enemy : MonoBehaviour
{
    // 몬스터 타입종류
    public enum Type { Bat, Golem, Rabbit, Ciclop, Bomb };

    // 몬스터 타입을 저장할 변수
    public Type enemyType;

    // 오브젝트 타입
    public ObjType type;

    // 몬스터 스탯
    public float maxHealth, curHealth, damage, moveSpeed;

    // 체크
    public bool isAttack, isChase, isDead, isFrost, isDrop;

    // 타겟
    private Transform target;

    // 타겟 거리
    private float distance;

    // 타겟 인식 거리
    public float recognizeArea;

    // 스폰지점
    Vector3 spawnPosition;

    // 당근 및 키클롭스눈이 생성될 위치를 저장할 변수
    public Transform[] carrotPos;

    // 오브젝트 풀링
    private PoolingManager poolingManager;

    // 다음 스테이지로가는 문 : 보스가 죽고나서 보스 뒤에 생기게한다
    public GameObject nextStage;

    // 물리
    private Rigidbody rigid;

    // 근접공격범위
    public BoxCollider meleeArea;

    // 애니메이터
    private Animator anim;

    // 네비매쉬
    public NavMeshAgent nav;

    // 콜라이더
    public BoxCollider boxCollider;

    // 플레이어
    public GameObject player;

    // 플레이어 스크립트
    private Player playerScript;

    // 체력바
    private HpBar hpBarScript;

    // 얼리기 해제 대기시간
    private float waitTime;

    void Awake()
    { 
        // 물리
        rigid = GetComponent<Rigidbody>();

        // 애니메이터
        anim = GetComponent<Animator>();

        // 네비매쉬
        nav = GetComponent<NavMeshAgent>();

        // 콜라이더
        boxCollider = GetComponent<BoxCollider>();

        // 스폰지점
        spawnPosition = transform.position;

        // 오브젝트 풀링
        poolingManager = GameObject.FindGameObjectWithTag("PoolManager").GetComponent<PoolingManager>();
        
        // 플레이어 관련 할당
        Invoke("SetPlayer", 0.5f);
        
        // 다음스테이지 문
        nextStage = GameObject.Find("NextStageSet");

        // 몬스터 이동속도
        moveSpeed = nav.speed;

        // 체력바
        hpBarScript = GetComponent<HpBar>();

        // 생성후 2초뒤에 추적시작
        Invoke("ChaseStart", 2);
    }

    void ChaseStart()
    {
        // 추적 시작
        // 추적 상태
        isChase = true;

        // 애니메이션
        anim.SetBool("isWalk", true);
    }

    void Update()
    {
        // 플레이어와의 거리
        distance = Vector3.Distance(target.transform.position, transform.position);

        // 내비게이션이 활성화 되어있고 인식할수있는 거리에 올때
        if (nav.enabled && distance < recognizeArea)
        {
            // 타겟 설정
            nav.SetDestination(target.position);

            // 추적중이면 isStopped가 false가되어 쫓아가고 추적중이 아니면 isStopped가 true가되어 멈춘다
            nav.isStopped = !isChase;
        }

        // 키클롭스가 죽은상태이면
        if (enemyType == Type.Ciclop && isDead)
        {
            // 모든 행동 중지
            StopAllCoroutines();
            return;
        }

        // 얼려지고 2초후에 다시 풀림
        if(isFrost)
        {
            if(waitTime <= 0)
            {
                nav.speed = moveSpeed;
                isFrost = false;
                waitTime = 3f;
            }
            else
            {
                waitTime -= Time.deltaTime;
            }
        }
    }

    IEnumerator EnemyStop()
    {
        // 플레이어와 거리가 멀때까지 서기
        while (true)
        {
            yield return null;
            if (distance >= recognizeArea)
            {
                anim.SetBool("isWalk", false);
            }
        }
    }

    void FixedUpdate()
    {
        // 공격 타겟팅
        Targeting();

        // 물리충돌시 회전이 유지되는 문제
        FreezeVelocity();
    }

    void FreezeVelocity()
    {
        // 물리충돌시 회전이 유지되는 문제

        // 추적 상태면
        if (isChase)
        {
            // 속도 0
            rigid.velocity = Vector3.zero;

            // 회전속도 0
            rigid.angularVelocity = Vector3.zero;
        }
    }

    void Targeting()
    {
        // 플레이어와 거리가 멀때까지 서기
        StartCoroutine(EnemyStop());

        // 플레이어와 거리가 가까워지면 다시 걷기
        anim.SetBool("isWalk", true);

        // 공격사정거리
        float targetRadius = 0;

        // 플레이어를 인식할수있는범위 (변수로선언된 recognizeArea랑 같다)
        float targetRange = 0;

        // 몬스터 타입에 따른 타겟인식범위
        switch (enemyType)
        {
            // 일반
            case Type.Bat:
                targetRadius = 2f;
                targetRange = 3f;
                break;
            // 돌격
            case Type.Golem:
                targetRadius = 2f;
                targetRange = 12f;
                break;
            // 원거리
            case Type.Rabbit:
                targetRadius = 3f;
                targetRange = 25f;
                break;
            // 보스
            case Type.Ciclop:
                targetRadius = 5f;
                targetRange = 25f;
                break;
            // 폭탄
            case Type.Bomb:
                targetRadius = 2f;
                targetRange = 12f;
                break;
        }
        // 몬스터 앞쪽으로 레이캐스트를 쏴서 플레이어의 정보를 가져온다
        RaycastHit[] rayHits = Physics.SphereCastAll(transform.position, targetRadius, transform.forward, targetRange, LayerMask.GetMask("Player"));
        
        // 플레이어의 정보가 들어와있고 공격중이 아닐때
        if (rayHits.Length > 0 && !isAttack)
        {
            // 몬스터 공격
            StartCoroutine(Attack());
        }
    }

    IEnumerator Attack()
    {
        // 추적상태 X
        isChase = false;

        // 공격상태
        isAttack = true;

        // 애니메이션 활성화
        anim.SetBool("isAttack", true);

        switch(enemyType)
        {
            case Type.Bat:
                // 애니메이션이 공격타이밍보다 느림
                yield return new WaitForSeconds(0.2f);

                // 공격범위 활성화
                meleeArea.enabled = true;

                // 1초뒤에
                yield return new WaitForSeconds(1f);

                // 공격범위 비활성화
                meleeArea.enabled = false;

                // 1초간 쉰다
                yield return new WaitForSeconds(1f);
                break;
            case Type.Bomb:
                // 피격 불가
                gameObject.layer = 14;

                // 죽은 상태
                isDead = true;

                // 죽으면 추적하지 못하게
                isChase = false;

                // 애니메이션이 공격타이밍보다 느림
                yield return new WaitForSeconds(0.2f);

                // 공격범위 활성화
                meleeArea.enabled = true;

                // 1초뒤에
                yield return new WaitForSeconds(1f);

                // 체력바 반납
                poolingManager.ReturnObj(hpBarScript.instantHpBar, ObjType.몬스터체력바);

                // 폭탄 반납
                poolingManager.ReturnObj(transform.gameObject, type);

                // 아이템 드랍
                ItemDrop(false);

                break;
            case Type.Golem:
                // 애니메이션이 공격타이밍보다 느림
                yield return new WaitForSeconds(0.1f);

                // 돌격
                rigid.AddForce(transform.forward * 20, ForceMode.Impulse); 

                // 공격범위 활성화
                meleeArea.enabled = true;

                // 빠르게 0.5초뒤에 멈춰 세우기
                yield return new WaitForSeconds(0.5f);
                rigid.velocity = Vector3.zero;

                // 공격범위 비활성화
                meleeArea.enabled = false;

                // 돌격을 했으니 2초간 쉰다
                yield return new WaitForSeconds(2f);
                break;
            case Type.Rabbit:
                // 애니메이션이 공격타이밍보다 느림
                yield return new WaitForSeconds(0.5f);

                // 당근 생성
                GameObject instantCarrot = poolingManager.GetObj(ObjType.당근);

                // 당근 트랜스폼 초기화
                instantCarrot.transform.position = carrotPos[0].position;
                instantCarrot.transform.rotation = carrotPos[0].rotation;
                
                // 당근 발사
                Rigidbody carrotRigid = instantCarrot.GetComponent<Rigidbody>();
                carrotRigid.velocity = transform.forward * 40;

                yield return new WaitForSeconds(0.2f);

                // 당근 생성
                instantCarrot = poolingManager.GetObj(ObjType.당근);

                // 당근 트랜스폼 초기화
                instantCarrot.transform.position = carrotPos[2].position;
                instantCarrot.transform.rotation = carrotPos[2].rotation;

                // 당근 발사
                carrotRigid = instantCarrot.GetComponent<Rigidbody>();
                carrotRigid.velocity = transform.forward * 40;

                yield return new WaitForSeconds(0.5f);

                // 당근 생성
                instantCarrot = poolingManager.GetObj(ObjType.당근그룹);

                // 당근 트랜스폼 초기화
                instantCarrot.transform.position = carrotPos[1].position;
                instantCarrot.transform.rotation = carrotPos[1].rotation;

                // 당근 발사
                carrotRigid = instantCarrot.GetComponent<Rigidbody>();
                carrotRigid.velocity = transform.forward * 40;

                // 당근 발사후 2초간 쉰다
                yield return new WaitForSeconds(1.7f);
                break;
            case Type.Ciclop:
                // 보스 행동패턴 결정
                yield return new WaitForSeconds(0.1f);

                // 랜덤 공격 패턴
                int ranAction = UnityEngine.Random.Range(0, 2); // 0~1
                if(ranAction == 0)
                {
                    // 키클롭스눈 하나씩 생성
                    StartCoroutine(CiclopEye());
                    yield return new WaitForSeconds(2f);
                    break;
                }
                else
                {
                    // 키클롭스눈 한번에 생성
                    StartCoroutine(CiclopEyeAll());
                    yield return new WaitForSeconds(2f);
                    break;
                }
        }
        // 추적상태
        isChase = true;

        // 공격상태 X
        isAttack = false;

        // 애니메이션 비활성화
        anim.SetBool("isAttack", false);
    }

    IEnumerator CiclopEye()
    {
        // 키클롭스눈 하나씩 생성
        // 애니메이션이 공격타이밍보다 느림
        yield return new WaitForSeconds(0.5f);

        // 키클롭스눈 발사
        GameObject instantCiclopEye = poolingManager.GetObj(ObjType.키클롭스눈);
        instantCiclopEye.transform.position = carrotPos[0].position;
        instantCiclopEye.transform.rotation = carrotPos[0].rotation;
        Rigidbody ciclopEyeRigid = instantCiclopEye.GetComponent<Rigidbody>();
        ciclopEyeRigid.velocity = transform.forward * 40;
        yield return new WaitForSeconds(0.1f);
        instantCiclopEye = poolingManager.GetObj(ObjType.키클롭스눈);
        instantCiclopEye.transform.position = carrotPos[1].position;
        instantCiclopEye.transform.rotation = carrotPos[1].rotation;
        ciclopEyeRigid = instantCiclopEye.GetComponent<Rigidbody>();
        ciclopEyeRigid.velocity = transform.forward * 40;
        yield return new WaitForSeconds(0.1f);
        instantCiclopEye = poolingManager.GetObj(ObjType.키클롭스눈);
        instantCiclopEye.transform.position = carrotPos[2].position;
        instantCiclopEye.transform.rotation = carrotPos[2].rotation;
        ciclopEyeRigid = instantCiclopEye.GetComponent<Rigidbody>();
        ciclopEyeRigid.velocity = transform.forward * 40;
        yield return new WaitForSeconds(0.1f);
        instantCiclopEye = poolingManager.GetObj(ObjType.키클롭스눈);
        instantCiclopEye.transform.position = carrotPos[3].position;
        instantCiclopEye.transform.rotation = carrotPos[3].rotation;
        ciclopEyeRigid = instantCiclopEye.GetComponent<Rigidbody>();
        ciclopEyeRigid.velocity = transform.forward * 40;
        yield return new WaitForSeconds(0.1f);
        instantCiclopEye = poolingManager.GetObj(ObjType.키클롭스눈);
        instantCiclopEye.transform.position = carrotPos[4].position;
        instantCiclopEye.transform.rotation = carrotPos[4].rotation;
        ciclopEyeRigid = instantCiclopEye.GetComponent<Rigidbody>();
        ciclopEyeRigid.velocity = transform.forward * 40;
        yield return new WaitForSeconds(0.1f);
        instantCiclopEye = poolingManager.GetObj(ObjType.키클롭스눈);
        instantCiclopEye.transform.position = carrotPos[5].position;
        instantCiclopEye.transform.rotation = carrotPos[5].rotation;
        ciclopEyeRigid = instantCiclopEye.GetComponent<Rigidbody>();
        ciclopEyeRigid.velocity = transform.right * 40;
        yield return new WaitForSeconds(0.1f);
        instantCiclopEye = poolingManager.GetObj(ObjType.키클롭스눈);
        instantCiclopEye.transform.position = carrotPos[6].position;
        instantCiclopEye.transform.rotation = carrotPos[6].rotation;
        ciclopEyeRigid = instantCiclopEye.GetComponent<Rigidbody>();
        ciclopEyeRigid.velocity = transform.right * 40;
        yield return new WaitForSeconds(0.1f);
        instantCiclopEye = poolingManager.GetObj(ObjType.키클롭스눈);
        instantCiclopEye.transform.position = carrotPos[7].position;
        instantCiclopEye.transform.rotation = carrotPos[7].rotation;
        ciclopEyeRigid = instantCiclopEye.GetComponent<Rigidbody>();
        ciclopEyeRigid.velocity = transform.right * 40;
        yield return new WaitForSeconds(0.1f);
        instantCiclopEye = poolingManager.GetObj(ObjType.키클롭스눈);
        instantCiclopEye.transform.position = carrotPos[8].position;
        instantCiclopEye.transform.rotation = carrotPos[8].rotation;
        ciclopEyeRigid = instantCiclopEye.GetComponent<Rigidbody>();
        ciclopEyeRigid.velocity = transform.right * 40;
        yield return new WaitForSeconds(0.1f);
        instantCiclopEye = poolingManager.GetObj(ObjType.키클롭스눈);
        instantCiclopEye.transform.position = carrotPos[9].position;
        instantCiclopEye.transform.rotation = carrotPos[9].rotation;
        ciclopEyeRigid = instantCiclopEye.GetComponent<Rigidbody>();
        ciclopEyeRigid.velocity = transform.right * 40;
        yield return new WaitForSeconds(0.1f);
        instantCiclopEye = poolingManager.GetObj(ObjType.키클롭스눈);
        instantCiclopEye.transform.position = carrotPos[10].position;
        instantCiclopEye.transform.rotation = carrotPos[10].rotation;
        ciclopEyeRigid = instantCiclopEye.GetComponent<Rigidbody>();
        ciclopEyeRigid.velocity = transform.forward * 40 * -1;
        yield return new WaitForSeconds(0.1f);
        instantCiclopEye = poolingManager.GetObj(ObjType.키클롭스눈);
        instantCiclopEye.transform.position = carrotPos[11].position;
        instantCiclopEye.transform.rotation = carrotPos[11].rotation;
        ciclopEyeRigid = instantCiclopEye.GetComponent<Rigidbody>();
        ciclopEyeRigid.velocity = transform.forward * 40 * -1;
        yield return new WaitForSeconds(0.1f);
        instantCiclopEye = poolingManager.GetObj(ObjType.키클롭스눈);
        instantCiclopEye.transform.position = carrotPos[12].position;
        instantCiclopEye.transform.rotation = carrotPos[12].rotation;
        ciclopEyeRigid = instantCiclopEye.GetComponent<Rigidbody>();
        ciclopEyeRigid.velocity = transform.forward * 40 * -1;
        yield return new WaitForSeconds(0.1f);
        instantCiclopEye = poolingManager.GetObj(ObjType.키클롭스눈);
        instantCiclopEye.transform.position = carrotPos[13].position;
        instantCiclopEye.transform.rotation = carrotPos[13].rotation;
        ciclopEyeRigid = instantCiclopEye.GetComponent<Rigidbody>();
        ciclopEyeRigid.velocity = transform.forward * 40 * -1;
        yield return new WaitForSeconds(0.1f);
        instantCiclopEye = poolingManager.GetObj(ObjType.키클롭스눈);
        instantCiclopEye.transform.position = carrotPos[14].position;
        instantCiclopEye.transform.rotation = carrotPos[14].rotation;
        ciclopEyeRigid = instantCiclopEye.GetComponent<Rigidbody>();
        ciclopEyeRigid.velocity = transform.forward * 40 * -1;
        yield return new WaitForSeconds(0.1f);
        instantCiclopEye = poolingManager.GetObj(ObjType.키클롭스눈);
        instantCiclopEye.transform.position = carrotPos[15].position;
        instantCiclopEye.transform.rotation = carrotPos[15].rotation;
        ciclopEyeRigid = instantCiclopEye.GetComponent<Rigidbody>();
        ciclopEyeRigid.velocity = transform.right * 40 * -1;
        yield return new WaitForSeconds(0.1f);
        instantCiclopEye = poolingManager.GetObj(ObjType.키클롭스눈);
        instantCiclopEye.transform.position = carrotPos[16].position;
        instantCiclopEye.transform.rotation = carrotPos[16].rotation;
        ciclopEyeRigid = instantCiclopEye.GetComponent<Rigidbody>();
        ciclopEyeRigid.velocity = transform.right * 40 * -1;
        yield return new WaitForSeconds(0.1f);
        instantCiclopEye = poolingManager.GetObj(ObjType.키클롭스눈);
        instantCiclopEye.transform.position = carrotPos[17].position;
        instantCiclopEye.transform.rotation = carrotPos[17].rotation;
        ciclopEyeRigid = instantCiclopEye.GetComponent<Rigidbody>();
        ciclopEyeRigid.velocity = transform.right * 40 * -1;
        yield return new WaitForSeconds(0.1f);
        instantCiclopEye = poolingManager.GetObj(ObjType.키클롭스눈);
        instantCiclopEye.transform.position = carrotPos[18].position;
        instantCiclopEye.transform.rotation = carrotPos[18].rotation;
        ciclopEyeRigid = instantCiclopEye.GetComponent<Rigidbody>();
        ciclopEyeRigid.velocity = transform.right * 40 * -1;
        yield return new WaitForSeconds(0.1f);
        instantCiclopEye = poolingManager.GetObj(ObjType.키클롭스눈);
        instantCiclopEye.transform.position = carrotPos[19].position;
        instantCiclopEye.transform.rotation = carrotPos[19].rotation;
        ciclopEyeRigid = instantCiclopEye.GetComponent<Rigidbody>();
        ciclopEyeRigid.velocity = transform.right * 40 * -1;
    }

    IEnumerator CiclopEyeAll()
    {
        // 키클롭스눈 한번에
        // 애니메이션이 공격타이밍보다 느림
        yield return new WaitForSeconds(0.5f);

        // 키클롭스눈 발사
        GameObject instantCiclopEye = poolingManager.GetObj(ObjType.키클롭스눈);
        instantCiclopEye.transform.position = carrotPos[0].position;
        instantCiclopEye.transform.rotation = carrotPos[0].rotation;
        Rigidbody ciclopEyeRigid = instantCiclopEye.GetComponent<Rigidbody>();
        ciclopEyeRigid.velocity = transform.forward * 40;
        instantCiclopEye = poolingManager.GetObj(ObjType.키클롭스눈);
        instantCiclopEye.transform.position = carrotPos[1].position;
        instantCiclopEye.transform.rotation = carrotPos[1].rotation;
        ciclopEyeRigid = instantCiclopEye.GetComponent<Rigidbody>();
        ciclopEyeRigid.velocity = transform.forward * 40;
        instantCiclopEye = poolingManager.GetObj(ObjType.키클롭스눈);
        instantCiclopEye.transform.position = carrotPos[2].position;
        instantCiclopEye.transform.rotation = carrotPos[2].rotation;
        ciclopEyeRigid = instantCiclopEye.GetComponent<Rigidbody>();
        ciclopEyeRigid.velocity = transform.forward * 40;
        instantCiclopEye = poolingManager.GetObj(ObjType.키클롭스눈);
        instantCiclopEye.transform.position = carrotPos[3].position;
        instantCiclopEye.transform.rotation = carrotPos[3].rotation;
        ciclopEyeRigid = instantCiclopEye.GetComponent<Rigidbody>();
        ciclopEyeRigid.velocity = transform.forward * 40;
        instantCiclopEye = poolingManager.GetObj(ObjType.키클롭스눈);
        instantCiclopEye.transform.position = carrotPos[4].position;
        instantCiclopEye.transform.rotation = carrotPos[4].rotation;
        ciclopEyeRigid = instantCiclopEye.GetComponent<Rigidbody>();
        ciclopEyeRigid.velocity = transform.forward * 40;
        instantCiclopEye = poolingManager.GetObj(ObjType.키클롭스눈);
        instantCiclopEye.transform.position = carrotPos[5].position;
        instantCiclopEye.transform.rotation = carrotPos[5].rotation;
        ciclopEyeRigid = instantCiclopEye.GetComponent<Rigidbody>();
        ciclopEyeRigid.velocity = transform.right * 40;
        instantCiclopEye = poolingManager.GetObj(ObjType.키클롭스눈);
        instantCiclopEye.transform.position = carrotPos[6].position;
        instantCiclopEye.transform.rotation = carrotPos[6].rotation;
        ciclopEyeRigid = instantCiclopEye.GetComponent<Rigidbody>();
        ciclopEyeRigid.velocity = transform.right * 40;
        instantCiclopEye = poolingManager.GetObj(ObjType.키클롭스눈);
        instantCiclopEye.transform.position = carrotPos[7].position;
        instantCiclopEye.transform.rotation = carrotPos[7].rotation;
        ciclopEyeRigid = instantCiclopEye.GetComponent<Rigidbody>();
        ciclopEyeRigid.velocity = transform.right * 40;
        instantCiclopEye = poolingManager.GetObj(ObjType.키클롭스눈);
        instantCiclopEye.transform.position = carrotPos[8].position;
        instantCiclopEye.transform.rotation = carrotPos[8].rotation;
        ciclopEyeRigid = instantCiclopEye.GetComponent<Rigidbody>();
        ciclopEyeRigid.velocity = transform.right * 40;
        instantCiclopEye = poolingManager.GetObj(ObjType.키클롭스눈);
        instantCiclopEye.transform.position = carrotPos[9].position;
        instantCiclopEye.transform.rotation = carrotPos[9].rotation;
        ciclopEyeRigid = instantCiclopEye.GetComponent<Rigidbody>();
        ciclopEyeRigid.velocity = transform.right * 40;
        instantCiclopEye = poolingManager.GetObj(ObjType.키클롭스눈);
        instantCiclopEye.transform.position = carrotPos[10].position;
        instantCiclopEye.transform.rotation = carrotPos[10].rotation;
        ciclopEyeRigid = instantCiclopEye.GetComponent<Rigidbody>();
        ciclopEyeRigid.velocity = transform.forward * 40 * -1;
        instantCiclopEye = poolingManager.GetObj(ObjType.키클롭스눈);
        instantCiclopEye.transform.position = carrotPos[11].position;
        instantCiclopEye.transform.rotation = carrotPos[11].rotation;
        ciclopEyeRigid = instantCiclopEye.GetComponent<Rigidbody>();
        ciclopEyeRigid.velocity = transform.forward * 40 * -1;
        instantCiclopEye = poolingManager.GetObj(ObjType.키클롭스눈);
        instantCiclopEye.transform.position = carrotPos[12].position;
        instantCiclopEye.transform.rotation = carrotPos[12].rotation;
        ciclopEyeRigid = instantCiclopEye.GetComponent<Rigidbody>();
        ciclopEyeRigid.velocity = transform.forward * 40 * -1;
        instantCiclopEye = poolingManager.GetObj(ObjType.키클롭스눈);
        instantCiclopEye.transform.position = carrotPos[13].position;
        instantCiclopEye.transform.rotation = carrotPos[13].rotation;
        ciclopEyeRigid = instantCiclopEye.GetComponent<Rigidbody>();
        ciclopEyeRigid.velocity = transform.forward * 40 * -1;
        instantCiclopEye = poolingManager.GetObj(ObjType.키클롭스눈);
        instantCiclopEye.transform.position = carrotPos[14].position;
        instantCiclopEye.transform.rotation = carrotPos[14].rotation;
        ciclopEyeRigid = instantCiclopEye.GetComponent<Rigidbody>();
        ciclopEyeRigid.velocity = transform.forward * 40 * -1;
        instantCiclopEye = poolingManager.GetObj(ObjType.키클롭스눈);
        instantCiclopEye.transform.position = carrotPos[15].position;
        instantCiclopEye.transform.rotation = carrotPos[15].rotation;
        ciclopEyeRigid = instantCiclopEye.GetComponent<Rigidbody>();
        ciclopEyeRigid.velocity = transform.right * 40 * -1;
        instantCiclopEye = poolingManager.GetObj(ObjType.키클롭스눈);
        instantCiclopEye.transform.position = carrotPos[16].position;
        instantCiclopEye.transform.rotation = carrotPos[16].rotation;
        ciclopEyeRigid = instantCiclopEye.GetComponent<Rigidbody>();
        ciclopEyeRigid.velocity = transform.right * 40 * -1;
        instantCiclopEye = poolingManager.GetObj(ObjType.키클롭스눈);
        instantCiclopEye.transform.position = carrotPos[17].position;
        instantCiclopEye.transform.rotation = carrotPos[17].rotation;
        ciclopEyeRigid = instantCiclopEye.GetComponent<Rigidbody>();
        ciclopEyeRigid.velocity = transform.right * 40 * -1;
        instantCiclopEye = poolingManager.GetObj(ObjType.키클롭스눈);
        instantCiclopEye.transform.position = carrotPos[18].position;
        instantCiclopEye.transform.rotation = carrotPos[18].rotation;
        ciclopEyeRigid = instantCiclopEye.GetComponent<Rigidbody>();
        ciclopEyeRigid.velocity = transform.right * 40 * -1;
        instantCiclopEye = poolingManager.GetObj(ObjType.키클롭스눈);
        instantCiclopEye.transform.position = carrotPos[19].position;
        instantCiclopEye.transform.rotation = carrotPos[19].rotation;
        ciclopEyeRigid = instantCiclopEye.GetComponent<Rigidbody>();
        ciclopEyeRigid.velocity = transform.right * 40 * -1; 

        // 2초간 쉰다
        yield return new WaitForSeconds(2f);
    }

    void OnTriggerEnter(Collider other)
    {
        // 명중률에 따른 몬스터 피격
        int random = UnityEngine.Random.Range(0, 100); // 0~99
        if(random < playerScript.accuracy) // 명중률 50 기준 0~49
        {
            // 몬스터 피격
            if (other.tag == "Arrow" || other.tag == "Sword")
            {
                // 혈액갑옷
                BloodArmor();

                // 즉사
                // 영구적인 즉사가 있는지 체크
                if(!playerScript.isPermanentSkill[8])
                {
                    // 영구적인 즉사가 없을때
                    if (playerScript.PassiveSkill[8] > 0)
                    {
                        // 보스를 제외하고 체력이 20 % 이하인 적을 즉사
                        if (enemyType != Type.Ciclop && curHealth <= maxHealth * 20 / 100)
                        {
                            // 즉사
                            InstantDeathAndEndOfDream();
                        }
                    }
                }
                else
                {
                    // 영구적인 즉사가 있을때
                    // 보스를 제외하고 체력이 20 % 이하인 적을 즉사
                    if (enemyType != Type.Ciclop && curHealth <= maxHealth * 20 / 100)
                    {
                        // 즉사
                        InstantDeathAndEndOfDream();
                    }
                }

                // 꿈의 끝
                // 영구적인 꿈의 끝이 있는지 체크
                if(!playerScript.isPermanentSkill[18])
                {
                    // 영구적인 꿈의 끝이 없을때
                    if (playerScript.PassiveSkill[18] > 0)
                    {
                        // 보스를 제외하고 체력이 50 % 이하인 적을 즉사
                        if (enemyType != Type.Ciclop && curHealth <= maxHealth * 50 / 100)
                        {
                            // 꿈의 끝
                            InstantDeathAndEndOfDream();
                        }
                    }
                }
                else
                {
                    // 영구적인 꿈의 끝이 있을때
                    // 보스를 제외하고 체력이 50 % 이하인 적을 즉사
                    if (enemyType != Type.Ciclop && curHealth <= maxHealth * 50 / 100)
                    {
                        // 꿈의 끝
                        InstantDeathAndEndOfDream();
                    }
                }

                // HP 감소
                // 화살의 계산된 데미지를 변수로 받은후
                // 광전사와 백발백중 유무에 따라 크리티컬인지 체크
                // 광전사 15 백발백중 1
                // 광전사 X 백발백중 X : 크리티컬 데미지는 기본데미지의 2배
                // 광전사 O 백발백중 X : 크리티컬 데미지는 기본데미지의 4배
                // 광전사 X 백발백중 O : 크리티컬 데미지는 기본데미지의 1.5배
                // 광전사 O 백발백중 O : 크리티컬 데미지는 기본데미지의 3배
                // 크리티컬 데미지에 해당하는 데미지라면 크리티컬공격 이펙트를
                // 기본데미지에 해당하는 데미지라면 일반공격 이펙트를 풀링
                // 영구적인 광전사와 백발백중 유무에 따라서 로직 수정
                float playerDamage = playerScript.DamageCalc();
                if(!playerScript.isPermanentSkill[15] && !playerScript.isPermanentSkill[1])
                {
                    // 영구적인 광전사 X 영구적인 백발백중 X
                    // 원래 로직 그대로 적용
                    if (playerScript.PassiveSkill[15] == 0 && playerScript.PassiveSkill[1] == 0)
                    {
                        // 광전사 X 백발백중 X
                        if (playerDamage >= playerScript.damage * 2)
                        {
                            // 크리티컬 데미지 이펙트
                            PlayerCriticalAtkEffect(other);
                        }
                        else
                        {
                            // 기본 데미지 이펙트
                            PlayerAtkEffect(other);
                        }
                        // HP 감소
                        curHealth -= playerDamage;
                    }
                    if (playerScript.PassiveSkill[15] > 0 && playerScript.PassiveSkill[1] == 0)
                    {
                        // 광전사 O 백발백중 X
                        if (playerDamage >= playerScript.damage * 4)
                        {
                            // 크리티컬 데미지 이펙트
                            PlayerCriticalAtkEffect(other);
                        }
                        else
                        {
                            // 기본 데미지 이펙트
                            PlayerAtkEffect(other);
                        }
                        // HP 감소
                        curHealth -= playerDamage;
                    }
                    if (playerScript.PassiveSkill[15] == 0 && playerScript.PassiveSkill[1] > 0)
                    {
                        // 광전사 X 백발백중 O
                        if (playerDamage >= playerScript.damage * 1.5)
                        {
                            // 크리티컬 데미지 이펙트
                            PlayerCriticalAtkEffect(other);
                        }
                        else
                        {
                            // 기본 데미지 이펙트
                            PlayerAtkEffect(other);
                        }
                        // HP 감소
                        curHealth -= playerDamage;
                    }
                    if (playerScript.PassiveSkill[15] > 0 && playerScript.PassiveSkill[1] > 0)
                    {
                        // 광전사 O 백발백중 O
                        if (playerDamage >= playerScript.damage * 3)
                        {
                            // 크리티컬 데미지 이펙트
                            PlayerCriticalAtkEffect(other);
                        }
                        else
                        {
                            // 기본 데미지 이펙트
                            PlayerAtkEffect(other);
                        }
                        // HP 감소
                        curHealth -= playerDamage;
                    }
                }
                if (playerScript.isPermanentSkill[15] && !playerScript.isPermanentSkill[1])
                {
                    // 영구적인 광전사 O 영구적인 백발백중 X
                    // 백발백중 유무에 따라서 로직
                    if (playerScript.PassiveSkill[1] == 0)
                    {
                        // 백발백중 X
                        if (playerDamage >= playerScript.damage * 4)
                        {
                            // 크리티컬 데미지 이펙트
                            PlayerCriticalAtkEffect(other);
                        }
                        else
                        {
                            // 기본 데미지 이펙트
                            PlayerAtkEffect(other);
                        }
                        // HP 감소
                        curHealth -= playerDamage;
                    }
                    if (playerScript.PassiveSkill[1] > 0)
                    {
                        // 백발백중 O
                        if (playerDamage >= playerScript.damage * 3)
                        {
                            // 크리티컬 데미지 이펙트
                            PlayerCriticalAtkEffect(other);
                        }
                        else
                        {
                            // 기본 데미지 이펙트
                            PlayerAtkEffect(other);
                        }
                        // HP 감소
                        curHealth -= playerDamage;
                    }
                }
                if (!playerScript.isPermanentSkill[15] && playerScript.isPermanentSkill[1])
                {
                    // 영구적인 광전사 X 영구적인 백발백중 O
                    // 광전사 유무에 따라서 로직
                    if (playerScript.PassiveSkill[15] == 0)
                    {
                        // 광전사 X
                        if (playerDamage >= playerScript.damage * 1.5)
                        {
                            // 크리티컬 데미지 이펙트
                            PlayerCriticalAtkEffect(other);
                        }
                        else
                        {
                            // 기본 데미지 이펙트
                            PlayerAtkEffect(other);
                        }
                        // HP 감소
                        curHealth -= playerDamage;
                    }
                    if (playerScript.PassiveSkill[15] > 0)
                    {
                        // 광전사 O
                        if (playerDamage >= playerScript.damage * 3)
                        {
                            // 크리티컬 데미지 이펙트
                            PlayerCriticalAtkEffect(other);
                        }
                        else
                        {
                            // 기본 데미지 이펙트
                            PlayerAtkEffect(other);
                        }
                        // HP 감소
                        curHealth -= playerDamage;
                    }
                }
                if (playerScript.isPermanentSkill[15] && playerScript.isPermanentSkill[1])
                {
                    // 영구적인 광전사 O 영구적인 백발백중 O
                    // 광전사 O 백발백중 O 인경우만 수행
                    // 광전사 O 백발백중 O
                    if (playerDamage >= playerScript.damage * 3)
                    {
                        // 크리티컬 데미지 이펙트
                        PlayerCriticalAtkEffect(other);
                    }
                    else
                    {
                        // 기본 데미지 이펙트
                        PlayerAtkEffect(other);
                    }
                    // HP 감소
                    curHealth -= playerDamage;
                }
                
                // 흡혈귀
                // 영구적인 흡혈귀가 있는지 체크
                if(!playerScript.isPermanentSkill[4])
                {
                    // 영구적인 흡혈귀가 없을떄
                    if (playerScript.PassiveSkill[4] > 0)
                    {
                        // 흡혈귀
                        Vampire(playerDamage);
                    }
                }
                else
                {
                    // 영구적인 흡혈귀가 있을때
                    // 흡혈귀
                    Vampire(playerDamage);
                    
                }

                // 데미지 텍스트
                GameObject instantDamageText = poolingManager.GetObj(ObjType.데미지텍스트);
                instantDamageText.GetComponent<TextMeshPro>().text = playerDamage.ToString();
                instantDamageText.transform.position = transform.position + Vector3.up * 25;
                instantDamageText.transform.rotation = poolingManager.FloationTextPrefs[0].transform.rotation;

                // 카메라 흔들림
                CameraShake.Instance.OnCameraShake(0.1f, 0.5f);

                // 넉백
                Vector3 reactVec = transform.position - player.transform.position;
                reactVec = reactVec.normalized;
                reactVec += Vector3.up * 10f;

                // 소드콤보공격은 이펙트가 유지되어야하므로 화살만 반납
                if (other.tag == "Arrow")
                {
                    poolingManager.ReturnObj(other.transform.gameObject, other.transform.gameObject.GetComponent<PlayerWeapon>().type);
                }

                // 피격 리액션
                StartCoroutine(OnDamage(reactVec));
            }
        }
        else // 명중률 50 기준 50~99
        {
            // 몬스터 회피
            if (other.tag == "Arrow" || other.tag == "Sword")
            {
                // 소드콤보공격은 이펙트가 유지되어야하므로 화살만 반납
                if(other.tag == "Arrow")
                {
                    poolingManager.ReturnObj(other.transform.gameObject, other.transform.gameObject.GetComponent<PlayerWeapon>().type);
                }

                // 회피 텍스트
                GameObject instantMissText = poolingManager.GetObj(ObjType.회피텍스트);
                instantMissText.transform.position = transform.position + Vector3.up * 20;
                instantMissText.transform.rotation = poolingManager.FloationTextPrefs[2].transform.rotation;
            }
        }
    }

    void PlayerAtkEffect(Collider Arrow)
    {
        // 플레이어 공격 이펙트
        GameObject instantPlayerAtkEffect = poolingManager.GetObj(ObjType.플레이어공격이펙트);
        instantPlayerAtkEffect.transform.position = Arrow.transform.position;
        instantPlayerAtkEffect.transform.rotation = Quaternion.identity;

        // 플레이어 공격 사운드
        SoundManager.instance.SFXPlay(ObjType.기본공격소리);
    }

    void PlayerCriticalAtkEffect(Collider Arrow)
    {
        // 플레이어 크리티컬공격 이펙트
        GameObject instantPlayerCriticalAtkEffect = poolingManager.GetObj(ObjType.플레이어크리티컬공격이펙트);
        instantPlayerCriticalAtkEffect.transform.position = Arrow.transform.position;
        instantPlayerCriticalAtkEffect.transform.rotation = Quaternion.identity;

        // 플레이어 크리티컬 공격 사운드
        SoundManager.instance.SFXPlay(ObjType.크리티컬공격소리);
    }

    public IEnumerator OnDamage(Vector3 reactVec)
    {
        // 몬스터 피격 리액션
        yield return new WaitForSeconds(0.1f);

        if(curHealth > 0)
        {
            // 살아있는 상태
            anim.SetTrigger("doDamaged");
            rigid.AddForce(reactVec * 10, ForceMode.Impulse);
        }
        else
        {
            // 죽어있는 상태
            if(enemyType != Type.Ciclop && enemyType != Type.Bomb)
            {
                // 보스와 폭탄을 제외한 몬스터
                // 아이템 드랍
                ItemDrop(false);

                // 체력바 반납
                poolingManager.ReturnObj(hpBarScript.instantHpBar, ObjType.몬스터체력바);

                // 보스 및 폭탄을 제외한 몬스터 반납
                poolingManager.ReturnObj(transform.gameObject, type);    

                // 방벽 얻기
                GetBarrier();
            }
            else if(enemyType == Type.Bomb)
            {
                // 폭탄
                // 아이템 드랍
                ItemDrop(false);

                // 피격 불가
                gameObject.layer = 14;

                // 죽은 상태
                isDead = true;

                // 죽으면 추적하지 못하게
                isChase = false;

                // 애니메이션
                anim.SetTrigger("doDamaged");

                // 애니메이션이 공격타이밍보다 느림
                yield return new WaitForSeconds(0.2f);

                // 공격범위 활성화
                meleeArea.enabled = true;

                // 1초뒤에
                yield return new WaitForSeconds(1f);

                // 체력바 반납
                poolingManager.ReturnObj(hpBarScript.instantHpBar, ObjType.몬스터체력바);

                // 폭탄 반납
                poolingManager.ReturnObj(transform.gameObject, type); 

                // 방벽 얻기
                GetBarrier();
            }
            else if(enemyType == Type.Ciclop)
            {
                // 보스
                // 아이템 드랍
                ItemDrop(true);

                // 피격 불가
                gameObject.layer = 14;

                // 죽은 상태
                isDead = true;

                // 죽으면 추적하지 못하게
                isChase = false;

                // 애니메이션
                anim.SetTrigger("doDie");

                // 다음스테이지 문 가져오기
                nextStage.SetActive(true);
                nextStage.transform.position = transform.position + transform.forward * -20f;
                nextStage.transform.rotation = Quaternion.identity;

                // 체력바 반납
                poolingManager.ReturnObj(hpBarScript.instantHpBar, ObjType.몬스터체력바);

                // 방벽 얻기
                GetBarrier();
            }
        }
    }

    void SetPlayer()
    {
        // 플레이어
        player = GameObject.FindGameObjectWithTag("Player");

        // 타겟 : 플레이어
        target = player.transform;

        // 플레이어 스크립트
        playerScript = player.GetComponent<Player>();
    }

    public void ItemDrop(bool isBoss)
    {
        // 아이템을 드랍하는 함수
        if (!isDrop)
        {
            // 드랍 실행
            isDrop = true;

            if(!isBoss)
            {
                // 일반몬스터이면
                int random = UnityEngine.Random.Range(0, 100); // 0~99
                if (random < playerScript.passiveDropPercentage) // 0~4
                {
                    // 랜덤 인덱스를 뽑는다
                    int passiveSkillRandom = UnityEngine.Random.Range(0, 22); // 0~21

                    // 랜덤인덱스에 해당하는 패시브 스킬을 받아온다
                    GameObject instantPassiveSkill = poolingManager.GetObj((ObjType)((int)ObjType.한발노리기 + passiveSkillRandom));

                    // 받아온 패시브 스킬의 트랜스폼을 초기화한다
                    instantPassiveSkill.transform.position = transform.position + new Vector3(0, 15f, 0);
                    instantPassiveSkill.transform.rotation = Quaternion.identity;
                }
                random = UnityEngine.Random.Range(0, 100); // 0~99
                if (random < playerScript.activeDropPercentage) // 0~2
                {
                    // 랜덤 인덱스를 뽑는다
                    int activeSkillRandom = UnityEngine.Random.Range(0, 2); // 0~1

                    // 랜덤인덱스에 해당하는 액티브 스킬을 받아온다
                    GameObject instantActiveSkill = poolingManager.GetObj((ObjType)((int)ObjType.멀티샷 + activeSkillRandom));

                    // 받아온 액티브 스킬의 트랜스폼을 초기화한다
                    instantActiveSkill.transform.position = transform.position + new Vector3(0, 15f, 15f);
                    instantActiveSkill.transform.rotation = Quaternion.identity;
                }
                random = UnityEngine.Random.Range(0, 100); // 0~99
                if (random < 5) // 0~4
                {
                    // 랜덤 인덱스를 뽑는다
                    int coinRandom = UnityEngine.Random.Range(0, 3); // 0~2

                    // 랜덤인덱스에 해당하는 코인값의 코인을 받아온다
                    GameObject instantCoin = poolingManager.GetObj((ObjType)((int)ObjType.백원 + coinRandom));

                    // 받아온 코인의 트랜스폼을 초기화한다
                    instantCoin.transform.position = transform.position + new Vector3(0, 15f, -15f);
                    instantCoin.transform.rotation = Quaternion.identity;
                }
                random = UnityEngine.Random.Range(0, 100); // 0~99
                if (random < playerScript.inventoryItemPercentage) // 0~4
                {
                    // 랜덤 인덱스를 뽑는다
                    int inventoryItemRandom = UnityEngine.Random.Range(0, 19); // 0~18

                    // 랜덤인덱스에 해당하는 인벤토리 아이템을 받아온다
                    GameObject instantInventoryItem = poolingManager.GetObj((ObjType)((int)ObjType.화려한장신구 + inventoryItemRandom));

                    // 받아온 인벤토리 아이템의 트랜스폼을 초기화한다
                    instantInventoryItem.transform.position = GetComponent<Enemy>().transform.position + new Vector3(15f, 15f, 0f);
                    instantInventoryItem.transform.rotation = Quaternion.identity;
                }
            }
            else
            {
                // 보스이면
                int random = UnityEngine.Random.Range(0, 100); // 0~99
                if (random < 50) // 0~49
                {
                    // 랜덤 인덱스를 뽑는다
                    int passiveSkillRandom = UnityEngine.Random.Range(0, 22); // 0~21

                    // 랜덤인덱스에 해당하는 패시브 스킬을 받아온다
                    GameObject instantPassiveSkill = poolingManager.GetObj((ObjType)((int)ObjType.한발노리기 + passiveSkillRandom));

                    // 받아온 패시브 스킬의 트랜스폼을 초기화한다
                    instantPassiveSkill.transform.position = transform.position + new Vector3(0, 15f, 0);
                    instantPassiveSkill.transform.rotation = Quaternion.identity;
                }
                random = UnityEngine.Random.Range(0, 100); // 0~99
                if (random < 20) // 0~19
                {
                    // 랜덤 인덱스를 뽑는다
                    int activeSkillRandom = UnityEngine.Random.Range(0, 2); // 0~1

                    // 랜덤인덱스에 해당하는 액티브 스킬을 받아온다
                    GameObject instantActiveSkill = poolingManager.GetObj((ObjType)((int)ObjType.멀티샷 + activeSkillRandom));

                    // 받아온 액티브 스킬의 트랜스폼을 초기화한다
                    instantActiveSkill.transform.position = transform.position + new Vector3(0, 15f, 15f);
                    instantActiveSkill.transform.rotation = Quaternion.identity;
                }
                random = UnityEngine.Random.Range(0, 100); // 0~99
                if (random < 100) // 0~99
                {
                    // 랜덤 인덱스를 뽑는다
                    int coinRandom = UnityEngine.Random.Range(0, 3); // 0~2

                    // 랜덤인덱스에 해당하는 코인값의 코인을 받아온다
                    GameObject instantCoin = poolingManager.GetObj((ObjType)((int)ObjType.백원 + coinRandom));

                    // 받아온 코인의 트랜스폼을 초기화한다
                    instantCoin.transform.position = transform.position + new Vector3(0, 15f, -15f);
                    instantCoin.transform.rotation = Quaternion.identity;
                }
                random = UnityEngine.Random.Range(0, 100); // 0~99
                if (random < 100) // 0~99
                {
                    // 랜덤 인덱스를 뽑는다
                    int inventoryItemRandom = UnityEngine.Random.Range(0, 19); // 0~18

                    // 랜덤인덱스에 해당하는 인벤토리 아이템을 받아온다
                    GameObject instantInventoryItem = poolingManager.GetObj((ObjType)((int)ObjType.화려한장신구 + inventoryItemRandom));

                    // 받아온 인벤토리 아이템의 트랜스폼을 초기화한다
                    instantInventoryItem.transform.position = GetComponent<Enemy>().transform.position + new Vector3(15f, 15f, 0f);
                    instantInventoryItem.transform.rotation = Quaternion.identity;
                }
            }
        }
    }

    public void GetBarrier()
    {
        // 방벽을 얻는 함수
        // 분신
        // 몬스터를 제거하면 방벽을 1개
        // 근심 유무에 따라 몬스터가 제거되는 모든곳에서 방벽을 증가
        // 영구적인 분신이 있는지 체크
        if (!playerScript.isPermanentSkill[19])
        {
            // 영구적인 분신이 없을때
            if (playerScript.PassiveSkill[19] > 0)
            {
                // 근심 유무에 따른 분신
                // 영구적인 근심이 있는지 체크
                if (!playerScript.isPermanentSkill[16])
                {
                    // 영구적인 근심이 없을때
                    if (playerScript.PassiveSkill[16] > 0)
                    {
                        // 근심이 있을때
                        playerScript.barrier += 2;
                    }
                    else
                    {
                        // 근심이 없을때
                        playerScript.barrier++;
                    }
                }
                else
                {
                    // 영구적인 근심이 있을때
                    // 근심이 있을때
                    playerScript.barrier += 2;
                }
            }
        }
        else
        {
            // 영구적인 분신이 있을때
            // 근심 유무에 따른 분신
            // 영구적인 근심이 있는지 체크
            if (!playerScript.isPermanentSkill[16])
            {
                // 영구적인 근심이 없을때
                if (playerScript.PassiveSkill[16] > 0)
                {
                    // 근심이 있을때
                    playerScript.barrier += 2;
                }
                else
                {
                    // 근심이 없을때
                    playerScript.barrier++;
                }
            }
            else
            {
                // 영구적인 근심이 있을때
                // 근심이 있을때
                playerScript.barrier += 2;
            }
        }
    }

    // 혈액갑옷
    public void BloodArmor()
    {
        // 영구적인 혈액갑옷이 있는지 체크
        if (!playerScript.isPermanentSkill[10])
        {
            // 영구적인 혈액갑옷이 없을때
            if (playerScript.PassiveSkill[10] > 0)
            {
                // 플레이어 최대체력의 10% 추가 데미지를 입힌다
                curHealth -= playerScript.maxHealth * 10 / 100;
            }
        }
        else
        {
            // 영구적인 혈액갑옷이 있을때
            // 플레이어 최대체력의 10% 추가 데미지를 입힌다
            curHealth -= playerScript.maxHealth * 10 / 100;
        }
    }

    // 즉사와 꿈의끝
    public void InstantDeathAndEndOfDream()
    {
        // 아이템드랍
        ItemDrop(false);

        // 체력바 반납
        poolingManager.ReturnObj(hpBarScript.instantHpBar, ObjType.몬스터체력바);

        // 보스를 제외한 몬스터 반납
        poolingManager.ReturnObj(transform.gameObject, type);

        // 방벽 얻기
        GetBarrier();
    }

    // 흡혈귀
    public void Vampire(float playerDamage)
    {
        if (playerScript.curHealth + playerDamage * 20 / 100 > maxHealth)
        {
            // 최대체력을 넘어서는 흡혈하면 현재체력을 최대체력으로
            playerScript.curHealth = playerScript.maxHealth;
        }
        else
        {
            // 아니라면 정상적으로 흡혈
            playerScript.curHealth += playerDamage * 20 / 100;
        }

        // 흡혈 텍스트
        GameObject instantHealingText = poolingManager.GetObj(ObjType.회복텍스트);
        instantHealingText.GetComponent<TextMeshPro>().text = "+" + (playerDamage * 20 / 100).ToString();
        instantHealingText.transform.position = transform.position + Vector3.up * 20;
        instantHealingText.transform.rotation = poolingManager.FloationTextPrefs[1].transform.rotation;
    }
}