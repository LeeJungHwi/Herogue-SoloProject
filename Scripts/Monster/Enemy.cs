using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;
using System;
using Sirenix.OdinInspector;

// 몬스터 타입종류
public enum Type { Bat, Golem, Rabbit, Ciclop, Bomb };

public class Enemy : MonoBehaviour
{
    // 몬스터 타입
    [Title ("몬스터 타입")]
    public Type enemyType; // 몬스터 타입을 저장할 변수
    [PropertySpace (0, 20)] public ObjType type; // 오브젝트 타입

    // 몬스터 스탯
    [Title ("몬스터 스탯")]
    [ReadOnly] public float maxHealth;
    [ReadOnly] public float curHealth, damage;
    [SerializeField] [ReadOnly] private float moveSpeed;

    private Transform target; // 타겟
    private float distance; // 타겟 거리
    [SerializeField] [Title ("몬스터 공격")] [PropertySpace (20, 0)] [InfoBox ("타겟 인식 거리")] private float recognizeArea; // 타겟 인식 거리
    [SerializeField] [InfoBox ("발사체 생성 위치")] private Transform[] carrotPos; // 당근 및 키클롭스눈이 생성될 위치를 저장할 변수
    [InfoBox ("근접 공격 범위")] public BoxCollider meleeArea; // 근접공격범위


    private PoolingManager poolingManager; // 오브젝트 풀링
    private GameObject nextStage; // 다음 스테이지로가는 문 : 보스가 죽고나서 보스 뒤에 생기게한다
    private Rigidbody rigid; // 물리
    private Animator anim; // 애니메이터
    [HideInInspector] public NavMeshAgent nav; // 네비매쉬
    private BoxCollider boxCollider; // 콜라이더
    [HideInInspector] public GameObject player; // 플레이어
    private Player playerScript; // 플레이어 스크립트
    private HpBar hpBarScript; // 체력바
    private float waitTime; // 얼리기 해제 대기시간
    [HideInInspector] public bool isAttack, isChase, isDead, isFrost, isDrop; // 체크
    private Vector3 spawnPosition; // 스폰지점

    private void Awake()
    { 
        rigid = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        nav = GetComponent<NavMeshAgent>();
        boxCollider = GetComponent<BoxCollider>();
        spawnPosition = transform.position;
        poolingManager = GameObject.FindGameObjectWithTag("PoolManager").GetComponent<PoolingManager>();
        Invoke("SetPlayer", 0.5f);
        nextStage = GameObject.Find("NextStageSet");
        moveSpeed = nav.speed;
        hpBarScript = GetComponent<HpBar>();
        Invoke("ChaseStart", 2); // 생성후 2초뒤에 추적시작
    }

    // 추적 시작
    private void ChaseStart()
    {
        isChase = true; // 추적 상태
        anim.SetBool("isWalk", true); // 애니메이션
    }

    private void Update()
    {
        // 플레이어와의 거리
        distance = Vector3.Distance(target.transform.position, transform.position);

        // 내비게이션이 활성화 되어있고 인식할수있는 거리에 올때
        if (nav.enabled && distance < recognizeArea)
        {
            nav.SetDestination(target.position); // 타겟 설정
            nav.isStopped = !isChase; // 추적중이면 isStopped가 false가되어 쫓아가고 추적중이 아니면 isStopped가 true가되어 멈춘다
        }

        // 키클롭스가 죽은상태이면
        if (enemyType == Type.Ciclop && isDead)
        {
            StopAllCoroutines(); // 모든 행동 중지
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
            else waitTime -= Time.deltaTime;
        }
    }

    private IEnumerator EnemyStop()
    {
        // 플레이어와 거리가 멀때까지 서기
        while (true)
        {
            yield return null;
            if (distance >= recognizeArea) anim.SetBool("isWalk", false);
        }
    }

    private void FixedUpdate()
    {
        Targeting(); // 공격 타겟팅
        FreezeVelocity(); // 물리충돌시 회전이 유지되는 문제
    }

    // 물리충돌시 회전이 유지되는 문제
    private void FreezeVelocity()
    {
        // 추적 상태면
        if (isChase)
        {
            rigid.velocity = Vector3.zero; // 속도 0
            rigid.angularVelocity = Vector3.zero; // 회전속도 0
        }
    }

    private void Targeting()
    {
        StartCoroutine(EnemyStop()); // 플레이어와 거리가 멀때까지 서기
        anim.SetBool("isWalk", true); // 플레이어와 거리가 가까워지면 다시 걷기
        float targetRadius = 0; // 공격사정거리
        float targetRange = 0; // 플레이어를 인식할수있는범위 (변수로선언된 recognizeArea랑 같다)

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

        RaycastHit[] rayHits = Physics.SphereCastAll(transform.position, targetRadius, transform.forward, targetRange, LayerMask.GetMask("Player")); // 몬스터 앞쪽으로 레이캐스트를 쏴서 플레이어의 정보를 가져온다
        
        // 플레이어의 정보가 들어와있고 공격중이 아닐때
        if (rayHits.Length > 0 && !isAttack) StartCoroutine(Attack()); // 몬스터 공격
    }

    private IEnumerator Attack()
    {
        isChase = false; // 추적상태 X
        isAttack = true; // 공격상태
        anim.SetBool("isAttack", true); // 애니메이션 활성화
 
        switch(enemyType)
        {
            case Type.Bat:
                yield return new WaitForSeconds(0.2f); // 애니메이션이 공격타이밍보다 느림
                meleeArea.enabled = true; // 공격범위 활성화
                yield return new WaitForSeconds(1f);
                meleeArea.enabled = false; // 공격범위 비활성화
                yield return new WaitForSeconds(1f);
                break;
            case Type.Bomb:
                gameObject.layer = 14; // 피격 불가
                isDead = true; // 죽은 상태
                isChase = false; // 죽으면 추적하지 못하게
                yield return new WaitForSeconds(0.2f); // 애니메이션이 공격타이밍보다 느림
                meleeArea.enabled = true; // 공격범위 활성화
                yield return new WaitForSeconds(1f);
                poolingManager.ReturnObj(hpBarScript.instantHpBar, ObjType.몬스터체력바); // 체력바 반납
                poolingManager.ReturnObj(transform.gameObject, type); // 폭탄 반납
                ItemDrop(false); // 아이템 드랍
                break;
            case Type.Golem:
                yield return new WaitForSeconds(0.1f); // 애니메이션이 공격타이밍보다 느림
                rigid.AddForce(transform.forward * 20, ForceMode.Impulse); // 돌격
                meleeArea.enabled = true; // 공격범위 활성화
                yield return new WaitForSeconds(0.5f); // 빠르게 0.5초뒤에 멈춰 세우기
                rigid.velocity = Vector3.zero;
                meleeArea.enabled = false; // 공격범위 비활성화
                yield return new WaitForSeconds(2f);
                break;
            case Type.Rabbit:
                yield return new WaitForSeconds(0.5f); // 애니메이션이 공격타이밍보다 느림
                GameObject instantCarrot = poolingManager.GetObj(ObjType.당근); // 당근 생성

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
                yield return new WaitForSeconds(0.1f); // 보스 행동패턴 결정

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

        isChase = true; // 추적상태
        isAttack = false; // 공격상태 X
        anim.SetBool("isAttack", false); // 애니메이션 비활성화
    }

    // 키클롭스눈 생성 후 발사
    private void FireCiclopEye(Vector3 position, Quaternion rotation, Vector3 velocity)
    {
        GameObject instantCiclopEye = poolingManager.GetObj(ObjType.키클롭스눈);
        instantCiclopEye.transform.position = position;
        instantCiclopEye.transform.rotation = rotation;
        Rigidbody ciclopEyeRigid = instantCiclopEye.GetComponent<Rigidbody>();
        ciclopEyeRigid.velocity = velocity;
    }

    // 키클롭스눈 하나씩 발사
    private IEnumerator CiclopEye()
    {
        // 애니메이션이 공격타이밍보다 느림
        yield return new WaitForSeconds(0.5f);

        // 키클롭스눈 방향 맵핑
        // 0~4 : forward * 40
        // 5~9 : right * 40
        // 10~14 : forward * 40 * (-1)
        // 15~19 : right * 40 * (-1)
        Dictionary<int, Vector3> eyeDir = new Dictionary<int, Vector3>();
        eyeDir[0] = transform.forward * 40;
        eyeDir[1] = transform.right * 40;
        eyeDir[2] = -transform.forward * 40;
        eyeDir[3] = -transform.right * 40;

        // 키클롭스눈 생성 위치 돌면서
        for (int i = 0; i < carrotPos.Length; i++)
        {
            Vector3 velocity = eyeDir[i / 5]; // 방향 설정
            FireCiclopEye(carrotPos[i].position, carrotPos[i].rotation, velocity); // 키클롭스눈 생성
            yield return new WaitForSeconds(0.1f); // 0.1초마다 한개씩
        }
    }

    // 키클롭스눈 한번에 발사
    private IEnumerator CiclopEyeAll()
    {
        // 애니메이션이 공격타이밍보다 느림
        yield return new WaitForSeconds(0.5f);

        // 키클롭스눈 방향 맵핑
        // 0~4 : forward * 40
        // 5~9 : right * 40
        // 10~14 : forward * 40 * (-1)
        // 15~19 : right * 40 * (-1)
        Dictionary<int, Vector3> eyeDir = new Dictionary<int, Vector3>();
        eyeDir[0] = transform.forward * 40;
        eyeDir[1] = transform.right * 40;
        eyeDir[2] = -transform.forward * 40;
        eyeDir[3] = -transform.right * 40;

        // 키클롭스눈 생성 위치 돌면서
        for (int i = 0; i < carrotPos.Length; i++)
        {
            Vector3 velocity = eyeDir[i / 5]; // 방향 설정
            FireCiclopEye(carrotPos[i].position, carrotPos[i].rotation, velocity); // 키클롭스눈 생성
        }

        // 2초 쉼
        yield return new WaitForSeconds(2f);
    }

    private void OnTriggerEnter(Collider other)
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
                        if (enemyType != Type.Ciclop && curHealth <= maxHealth * 20 / 100) InstantDeathAndEndOfDream(); // 즉사
                    }
                }
                else
                {
                    // 영구적인 즉사가 있을때
                    // 보스를 제외하고 체력이 20 % 이하인 적을 즉사
                    if (enemyType != Type.Ciclop && curHealth <= maxHealth * 20 / 100) InstantDeathAndEndOfDream(); // 즉사
                }

                // 꿈의 끝
                // 영구적인 꿈의 끝이 있는지 체크
                if(!playerScript.isPermanentSkill[18])
                {
                    // 영구적인 꿈의 끝이 없을때
                    if (playerScript.PassiveSkill[18] > 0)
                    {
                        // 보스를 제외하고 체력이 50 % 이하인 적을 즉사
                        if (enemyType != Type.Ciclop && curHealth <= maxHealth * 50 / 100) InstantDeathAndEndOfDream(); // 꿈의 끝
                    }
                }
                else
                {
                    // 영구적인 꿈의 끝이 있을때
                    // 보스를 제외하고 체력이 50 % 이하인 적을 즉사
                    if (enemyType != Type.Ciclop && curHealth <= maxHealth * 50 / 100) InstantDeathAndEndOfDream(); // 꿈의 끝
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
                        if (playerDamage >= playerScript.damage * 2) PlayerCriticalAtkEffect(other); // 크리티컬 데미지 이펙트
                        else PlayerAtkEffect(other); // 기본 데미지 이펙트
                        curHealth -= playerDamage; // HP 감소
                    }
                    if (playerScript.PassiveSkill[15] > 0 && playerScript.PassiveSkill[1] == 0)
                    {
                        // 광전사 O 백발백중 X
                        if (playerDamage >= playerScript.damage * 4) PlayerCriticalAtkEffect(other); // 크리티컬 데미지 이펙트
                        else PlayerAtkEffect(other); // 기본 데미지 이펙트
                        curHealth -= playerDamage; // HP 감소
                    }
                    if (playerScript.PassiveSkill[15] == 0 && playerScript.PassiveSkill[1] > 0)
                    {
                        // 광전사 X 백발백중 O
                        if (playerDamage >= playerScript.damage * 1.5) PlayerCriticalAtkEffect(other); // 크리티컬 데미지 이펙트
                        else PlayerAtkEffect(other); // 기본 데미지 이펙트
                        curHealth -= playerDamage; // HP 감소
                    }
                    if (playerScript.PassiveSkill[15] > 0 && playerScript.PassiveSkill[1] > 0)
                    {
                        // 광전사 O 백발백중 O
                        if (playerDamage >= playerScript.damage * 3) PlayerCriticalAtkEffect(other); // 크리티컬 데미지 이펙트
                        else PlayerAtkEffect(other); // 기본 데미지 이펙트
                        curHealth -= playerDamage; // HP 감소
                    }
                }
                if (playerScript.isPermanentSkill[15] && !playerScript.isPermanentSkill[1])
                {
                    // 영구적인 광전사 O 영구적인 백발백중 X
                    // 백발백중 유무에 따라서 로직
                    if (playerScript.PassiveSkill[1] == 0)
                    {
                        // 백발백중 X
                        if (playerDamage >= playerScript.damage * 4) PlayerCriticalAtkEffect(other); // 크리티컬 데미지 이펙트
                        else PlayerAtkEffect(other); // 기본 데미지 이펙트
                        curHealth -= playerDamage; // HP 감소
                    }
                    if (playerScript.PassiveSkill[1] > 0)
                    {
                        // 백발백중 O
                        if (playerDamage >= playerScript.damage * 3) PlayerCriticalAtkEffect(other); // 크리티컬 데미지 이펙트
                        else PlayerAtkEffect(other); // 기본 데미지 이펙트
                        curHealth -= playerDamage; // HP 감소
                    }
                }
                if (!playerScript.isPermanentSkill[15] && playerScript.isPermanentSkill[1])
                {
                    // 영구적인 광전사 X 영구적인 백발백중 O
                    // 광전사 유무에 따라서 로직
                    if (playerScript.PassiveSkill[15] == 0)
                    {
                        // 광전사 X
                        if (playerDamage >= playerScript.damage * 1.5) PlayerCriticalAtkEffect(other); // 크리티컬 데미지 이펙트
                        else PlayerAtkEffect(other); // 기본 데미지 이펙트
                        curHealth -= playerDamage; // HP 감소
                    }
                    if (playerScript.PassiveSkill[15] > 0)
                    {
                        // 광전사 O
                        if (playerDamage >= playerScript.damage * 3) PlayerCriticalAtkEffect(other); // 크리티컬 데미지 이펙트
                        else PlayerAtkEffect(other); // 기본 데미지 이펙트
                        curHealth -= playerDamage; // HP 감소
                    }
                }
                if (playerScript.isPermanentSkill[15] && playerScript.isPermanentSkill[1])
                {
                    // 영구적인 광전사 O 영구적인 백발백중 O
                    // 광전사 O 백발백중 O 인경우만 수행
                    // 광전사 O 백발백중 O
                    if (playerDamage >= playerScript.damage * 3) PlayerCriticalAtkEffect(other); // 크리티컬 데미지 이펙트
                    else PlayerAtkEffect(other); // 기본 데미지 이펙트
                    curHealth -= playerDamage; // HP 감소
                }
                
                // 흡혈귀
                // 영구적인 흡혈귀가 있는지 체크
                if(!playerScript.isPermanentSkill[4])
                {
                    // 영구적인 흡혈귀가 없을떄
                    if (playerScript.PassiveSkill[4] > 0) Vampire(playerDamage); // 흡혈귀
                }
                else Vampire(playerDamage); // 영구적인 흡혈귀가 있을때

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
                if (other.tag == "Arrow") poolingManager.ReturnObj(other.transform.gameObject, other.transform.gameObject.GetComponent<PlayerWeapon>().type);

                // 피격 리액션
                if(gameObject.activeSelf) StartCoroutine(OnDamage(reactVec));
            }
        }
        else // 명중률 50 기준 50~99
        {
            // 몬스터 회피
            if (other.tag == "Arrow" || other.tag == "Sword")
            {
                // 소드콤보공격은 이펙트가 유지되어야하므로 화살만 반납
                if(other.tag == "Arrow") poolingManager.ReturnObj(other.transform.gameObject, other.transform.gameObject.GetComponent<PlayerWeapon>().type);

                // 회피 텍스트
                GameObject instantMissText = poolingManager.GetObj(ObjType.회피텍스트);
                instantMissText.transform.position = transform.position + Vector3.up * 20;
                instantMissText.transform.rotation = poolingManager.FloationTextPrefs[2].transform.rotation;
            }
        }
    }

    private void PlayerAtkEffect(Collider Arrow)
    {
        // 플레이어 공격 이펙트
        GameObject instantPlayerAtkEffect = poolingManager.GetObj(ObjType.플레이어공격이펙트);
        instantPlayerAtkEffect.transform.position = Arrow.transform.position;
        instantPlayerAtkEffect.transform.rotation = Quaternion.identity;

        // 플레이어 공격 사운드
        SoundManager.instance.SFXPlay(ObjType.기본공격소리);
    }

    private void PlayerCriticalAtkEffect(Collider Arrow)
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
        else if(!isDead)
        {
            isDead = true; // 죽어있는 상태
            EnemyKillQuestCheck(); // 카운트베이스 퀘스트 처리
            
            if(enemyType != Type.Ciclop && enemyType != Type.Bomb)
            {
                // 보스와 폭탄을 제외한 몬스터
                ItemDrop(false); // 아이템 드랍
                poolingManager.ReturnObj(hpBarScript.instantHpBar, ObjType.몬스터체력바); // 체력바 반납
                poolingManager.ReturnObj(transform.gameObject, type); // 보스 및 폭탄을 제외한 몬스터 반납
                GetBarrier(); // 방벽 얻기
            }
            else if(enemyType == Type.Bomb)
            {
                // 폭탄
                ItemDrop(false); // 아이템 드랍
                gameObject.layer = 14; // 피격 불가
                isDead = true; // 죽은 상태
                isChase = false; // 죽으면 추적하지 못하게
                anim.SetTrigger("doDamaged"); // 애니메이션
                yield return new WaitForSeconds(0.2f); // 애니메이션이 공격타이밍보다 느림
                meleeArea.enabled = true; // 공격범위 활성화
                yield return new WaitForSeconds(1f);
                poolingManager.ReturnObj(hpBarScript.instantHpBar, ObjType.몬스터체력바); // 체력바 반납
                poolingManager.ReturnObj(transform.gameObject, type); // 폭탄 반납
                GetBarrier(); // 방벽 얻기
            }
            else if(enemyType == Type.Ciclop)
            {
                // 보스
                ItemDrop(true); // 아이템 드랍
                gameObject.layer = 14; // 피격 불가
                isDead = true; // 죽은 상태
                isChase = false; // 죽으면 추적하지 못하게
                anim.SetTrigger("doDie"); // 애니메이션

                // 다음스테이지 문 가져오기
                nextStage.SetActive(true);
                nextStage.transform.position = transform.position + transform.forward * -20f;
                nextStage.transform.rotation = Quaternion.identity;

                poolingManager.ReturnObj(hpBarScript.instantHpBar, ObjType.몬스터체력바); // 체력바 반납
                GetBarrier(); // 방벽 얻기
            }
        }
    }

    private void SetPlayer()
    {
        player = GameObject.FindGameObjectWithTag("Player"); // 플레이어
        target = player.transform; // 타겟 : 플레이어
        playerScript = player.GetComponent<Player>(); // 플레이어 스크립트
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
                    if (playerScript.PassiveSkill[16] > 0) playerScript.barrier += 2; // 근심이 있을때
                    else playerScript.barrier++; // 근심이 없을때
                }
                else playerScript.barrier += 2; // 영구적인 근심이 있을때
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
                if (playerScript.PassiveSkill[16] > 0) playerScript.barrier += 2; // 근심이 있을때
                else playerScript.barrier++; // 근심이 없을때
            }
            else playerScript.barrier += 2; // 영구적인 근심이 있을때
        }
    }

    // 혈액갑옷
    private void BloodArmor()
    {
        // 영구적인 혈액갑옷이 있는지 체크
        if (!playerScript.isPermanentSkill[10])
        {
            // 영구적인 혈액갑옷이 없을때 
            if (playerScript.PassiveSkill[10] > 0) curHealth -= playerScript.maxHealth * 10 / 100; // 플레이어 최대체력의 10% 추가 데미지를 입힌다
        }
        else curHealth -= playerScript.maxHealth * 10 / 100; // 영구적인 혈액갑옷이 있을때 플레이어 최대체력의 10% 추가 데미지를 입힌다
    }

    // 즉사와 꿈의끝
    private void InstantDeathAndEndOfDream()
    {
        EnemyKillQuestCheck(); // 카운트베이스 퀘스트 처리
        ItemDrop(false); // 아이템드랍
        poolingManager.ReturnObj(hpBarScript.instantHpBar, ObjType.몬스터체력바); // 체력바 반납
        poolingManager.ReturnObj(transform.gameObject, type); // 보스를 제외한 몬스터 반납
        GetBarrier(); // 방벽 얻기
    }

    // 흡혈귀
    private void Vampire(float playerDamage)
    {
        if (playerScript.curHealth + playerDamage * 20 / 100 > maxHealth) playerScript.curHealth = playerScript.maxHealth; // 최대체력을 넘어서는 흡혈하면 현재체력을 최대체력으로
        else playerScript.curHealth += playerDamage * 20 / 100; // 아니라면 정상적으로 흡혈

        // 흡혈 텍스트
        GameObject instantHealingText = poolingManager.GetObj(ObjType.회복텍스트);
        instantHealingText.GetComponent<TextMeshPro>().text = "+" + (playerDamage * 20 / 100).ToString();
        instantHealingText.transform.position = transform.position + Vector3.up * 20;
        instantHealingText.transform.rotation = poolingManager.FloationTextPrefs[1].transform.rotation;
    }

    // 몬스터 킬 퀘스트 체크
    public void EnemyKillQuestCheck()
    {
        // 일반몬스터
        if(enemyType == Type.Bat || enemyType == Type.Golem || enemyType == Type.Rabbit || enemyType == Type.Bomb)
        {
            foreach (QuestBase quest in QuestManager.instance.QuestList)
            {
                if (quest is KillNormalQuest)
                {
                    CountBase countBase = quest as CountBase;
                    countBase.CurCnt++;
                    QuestManager.instance.QuestNotify($"{countBase.questName} {countBase.CurCnt}/{countBase.completeCnt}"); // 퀘스트 진행상황 알림
                    break;
                }
            }

            foreach (QuestBase quest in QuestManager.instance.QuestList)
            {
                if (quest is KillNormalLoopQuest)
                {
                    CountBase countBase = quest as CountBase;
                    countBase.CurCnt++;
                    QuestManager.instance.QuestNotify($"{countBase.questName} {countBase.CurCnt}/{countBase.completeCnt}"); // 퀘스트 진행상황 알림
                    break;
                }
            }

            return;
        }

        // 보스
        foreach (QuestBase quest in QuestManager.instance.QuestList)
        {
            if (quest is KillBossQuest)
            {
                CountBase countBase = quest as CountBase;
                countBase.CurCnt++;
                QuestManager.instance.QuestNotify($"{countBase.questName} {countBase.CurCnt}/{countBase.completeCnt}"); // 퀘스트 진행상황 알림
                return;
            }
        }
    }
}