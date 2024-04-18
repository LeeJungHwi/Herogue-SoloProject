using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
using TMPro;

public class Player : MonoBehaviour
{
    // 체크
    public bool isBorder = false, isShoot = false, isDamage, isDead, isSecretRoom, isShelter = true, iDown, fDown, isShop;

    // 스탯
    public float curHealth, maxHealth, attackSpeed = 0.5f, damage, criticalDamage, accuracy, bloodDrain, barrier;
    public int criticalPercentage;

    // 불사신 무적시간 계산용
    private float invincibilityTime = 3f;
    private float invincibilityTimeCalc = 3f;

    // 코인
    public int coin, maxCoin;

    // 랜덤코인을 뽑기위해서 코인값 저장
    public int[] coinValue;

    // 드랍률
    public float secretPercentage, activeDropPercentage, passiveDropPercentage, inventoryItemPercentage;

    // 애니메이터
    public Animator anim;

    // 화살 생성위치
    public Transform[] ArrowPos;

    // 오브젝트 풀링
    public PoolingManager poolingManager;

    // 물리
    private Rigidbody rigid;

    // 플레이어와 가까이 있는 오브젝트
    public GameObject nearObject;

    // 조이스틱 스크립트
    public Joystick joystickScript;

    // 다음스테이지 함수를 호출하기위해
    public RoomTemplates templates;

    // 체력바
    public Slider playerHpBar;

    // 획득한 스킬의 설명을 저장 할 변수
    private StringBuilder sb = new StringBuilder();

    // 텍스트
    public Text skillListText, statusInfoText, coinText, barrierText;

    // 장착된 장비의 이미지를 보여주기 위한 변수들
    public Image weaponSlotImage, armorSlotImage, gloveSlotImage, shoesSlotImage, amuletSlotImage, petSlotImage;

    // 현재 장비 슬롯에 장착되어있는 장비를 저장하는 변수들
    // 현재 장착되어있는 무기
    public InventoryItem equipedWeaponItem, equipedArmorItem, equipedGloveItem, equipedShoesItem, equipedAmuletItem, equipedPetItem;

    // 현재 장착된 무기가 있는지 체크하는 변수
    // 장착된 장비가 있는상태에서도 장비가 착용되는문제
    // 장착 : 플래그가 false 일때만 가능, 장착후에 플래그 true
    // 해제 : 플래그 false
    public bool isWeapon, isArmor, isGlove, isShoes, isAmulet, isPet;

    // 상점 패널
    public GameObject shopPanel;

    // 인벤토리 패널
    public GameObject inventoryPanel;

    // 상점 아이템 데이터베이스
    public ShopDatabase shopDatabase;

    // 상점 슬롯
    public ShopSlot[] shopSlots;

    // 상점 슬롯을 관리하는 변수
    public Transform shopSlotHolder;

    // 영구 스킬의 설명을 저장 할 변수
    public StringBuilder permanentSb = new StringBuilder();

    // 영구 스킬리스트 텍스트
    public Text permanentSkillListText;

    // 영구 패시브 스킬인지 체크
    public bool[] isPermanentSkill;

    // 영구 패시브 획득 횟수
    public int permanentSkillCnt;

    // 액티브스킬 잠금 이미지
    public GameObject[] abilityLock;
    
    // 액티브스킬을 해제했는지 체크
    public bool[] isAbility;

    // 현재 소환된 펫 저장
    public GameObject spawnedPet;

    // 마을
    public GameObject shelter;

    // 경사각
    public float slopeAngle;

    // 플레이어 앞쪽 아래쪽에 오브젝트가 있는지 체크
    public bool isForwardObject, isDownObject;

    // 경사각 레이어마스크
    public LayerMask layerMask;

    // 스킬 아이템 : 캐릭터에 따라 상점 스킬 아이템을 바꾸기 위해
    public InventoryItem[] abilitySwordSkillItem, abilityMageSkillItem, abilityBlacksmithSkillItem, abilityHolyknightSkillItem;

    // 스킬 카운트 : 캐릭터에 따라 상점 스킬 아이템을 바꾸기 위해
    public int abilitySkillCnt;

    // 무기 아이템 : 캐릭터에 따라 상점 무기를 바꾸기 위해, 검은 성기사도 같이 사용
    public InventoryItem[] swordItem, staffItem, hammerItem;

    // 무기 아이템 카운트 : 캐릭터에 따라 상점 무기를 바꾸기 위해
    public int weaponCnt;

    // 공격 카운트 : 콤보 공격을 위해
    public int attackCnt;

    // 공격 카운트 대기시간 : 콤보 공격을 위해
    public float waitTime;

    // 선택된 캐릭터
    public string characterType;

    // 액티브스킬
    // 0 : 멀티샷
    // 1 : 사선화살
    // 0, 1 모두없으면 화살 하나
    // 0 있으면 화살 두개
    // 1 있으면 사선화살
    // 0, 1 모두있으면 화살 두개씩 사선화살
    public List<int> ActiveSkill = new List<int>();

    // 패시브스킬
    // 플레이어 기본 데이터 -> 공격력 100 체력 1000 크리티컬확률 25% 크리티컬데미지 200% 명중률 75% 다음스테이지로 갈때 현재체력10%회복 O
    // 0 : 한발노리기 -> 크리티컬 확률 100% 명중률 50% O
    // 1 : 백발백중 -> 명중률 100% 크리티컬 데미지 50% 감소 O
    // 2 : 방벽 -> 다음스테이지 갈때마다 1개씩 얻는다 적의 공격을 1회 막는다 O
    // 3 : 초월방벽 -> 방벽을 즉시 20개얻는대신 체력이 1이된다 다음스테이지 갈때마다 2개씩 얻는다 적의 공격을 1회 막는다 O
    // 4 : 흡혈귀 -> 입힌데미지의 20% 회복 O
    // 5 : 거울 -> 50 확률로 공격을 반사 플레이어도 데미지를 입는다 보스와 폭탄은 반사를 하지않는다 근접공격만 반사한다 O 
    // 6 : 도박꾼 -> 몬스터 아이템 드랍률 10% 증가 O
    // 7 : 저거너트 -> 받는피해가 80% 감소 O
    // 8 : 즉사 -> 보스를 제외하고 체력이 20% 이하인적을 공격하면 즉사시킨다 O
    // 9 : 시크릿 -> 시크릿박스에서 아이템 드랍률 10% 증가 O
    // 10 : 혈액갑옷 -> 플레이어 최대체력의 10% 추가 데미지를 입힌다 O
    // 11 : 버티기 -> 죽기전 50% 확률로 버티며 체력이 1이된다 O
    // 12 : 구사일생 -> 버티기 성공시 HP 모두 회복 O
    // 13 : 폭발적치유 -> 다음스테이지로 갈때 현재체력50% 회복 O
    // 14 : HP 부스트 -> 최대체력이 100% 증가 O
    // 15 : 광전사 -> 공격 및 피격 모두 데미지가 2배가 된다 O
    // 16 : 근심 -> 방벽의 획득량이 두배가된다 O
    // 17 : 고동 -> 다음스테이지로 갈때 현제체력100% 회복 O
    // 18 : 꿈의 끝 -> 보스를 제외하고 체력이 50% 이하인적을 공격하면 즉사시킨다 O
    // 19 : 분신 -> 몬스터를 제거하면 방벽을 1개 얻는다 근심을 고려해서 몬스터가 제거되는 모든곳에서 방벽을 증가시켜줘야한다 O
    // 20 : 향상된대쉬 -> 대쉬할때 무적상태가 된다 O
    // 21 : 불사신 -> 3초간격으로 무적상태가 된다 O
    public enum PassiveSkillType { 한발노리기, 백발백중, 방벽, 초월방벽, 흡혈귀, 거울, 도박꾼, 저거너트, 즉사, 시크릿, 혈액갑옷, 버티기, 구사일생, 폭발적치유, HP부스트, 광전사, 근심, 고동, 꿈의끝, 분신, 향상된대쉬, 불사신 } // 패시브 스킬 타입
    public List<int> PassiveSkill = new List<int>(); // 패시브 스킬 관리

    void Awake() 
    {
        // 애니메이터
        anim = GetComponent<Animator>();

        // 물리
        rigid = GetComponent<Rigidbody>();

        // 상점 슬롯
        shopSlots = shopSlotHolder.GetComponentsInChildren<ShopSlot>();

        // 선택된 캐릭터
        characterType = DataManager.instance.character.ToString();

        // 선택된 캐릭터와 다르면 삭제
        if(characterType != gameObject.name)
        {
            gameObject.SetActive(false);
        }
    }

    void Update()
    {
        // 컴퓨터 확인용 키입력
        GetInput();

        // 컴퓨터 플레이어 공격
        // 모바일 빌드할때 주석처리
        // Shoot();

        // 플레이어 상호작용
        ComputerInteraction();

        // 체력에따라 슬라이더 값 조절
        playerHpBar.value = Mathf.Lerp(playerHpBar.value, curHealth / maxHealth, Time.deltaTime * 15);
        playerHpBar.GetComponentInChildren<Text>().text = curHealth + " / " + maxHealth;

        // 플레이어 스탯 텍스트
        statusInfoText.text =
        $"체력 {curHealth} / {maxHealth}{System.Environment.NewLine}공격력 {damage}{System.Environment.NewLine}크리티컬확률 {criticalPercentage} %{System.Environment.NewLine}크리티컬데미지 {criticalDamage} %{System.Environment.NewLine}명중률 {accuracy} %{System.Environment.NewLine}흡혈 {bloodDrain} %{System.Environment.NewLine}공격속도 {attackSpeed}{System.Environment.NewLine}이동속도 {joystickScript.moveSpeed}";

        // 플레이어 코인 텍스트
        coinText.text = $" {coin}";

        // 방벽 텍스트
        barrierText.text = barrier.ToString();

        // 경사각 계산
        CalcSlopeAngle();

        // 콤보 공격
        if (waitTime <= 0 && !isShoot)
        {
            // 대기시간이 0 이하이면서 공격상태가 아닐때
            // 공격 카운트 초기화
            attackCnt = 0;
        }
        else
        {
            waitTime -= Time.deltaTime;
        }
    }

    public float DamageCalc()
    {
        // 플레이어 데미지 계산
        // 패시브스킬이 적용된 데미지
        float applyDamage = 0;

        // 크리티컬 및 광전사 유무에 따른 최종 데미지 계산
        // 영구적인 광전사가 있는지 체크
        if(!isPermanentSkill[15])
        {
            // 영구적인 광전사가 없으면
            int random = Random.Range(0, 100); // 0~99
            if (random < criticalPercentage) // 0~24
            {
                // 크리티컬이 터졌을때 광전사 유무
                if (PassiveSkill[15] > 0)
                {
                    // 크리티컬이 발동하고 광전사가 있을때 -> 100을 기준으로 400
                    return (damage + applyDamage) * (criticalDamage / 100) * 2;
                }
                else
                {
                    // 크리티컬이 발동하고 광전사가 없을때 -> 100을 기준으로 200
                    return (damage + applyDamage) * (criticalDamage / 100);
                }
            }
            else // 25~99
            {
                // 크리티컬이 안터졌을때 광전사 유무
                if (PassiveSkill[15] > 0)
                {
                    // 크리티컬이 안터지고 광전사가 있을때 -> 100을 기준으로 200
                    return (damage + applyDamage) * 2;
                }
                else
                {
                    // 크리티컬이 안터지고 광전사가 없을때 -> 100을 기준으로 100
                    return damage + applyDamage;
                }
            }
        }
        else
        {
            // 영구적인 광전사가 있으면
            int random = Random.Range(0, 100); // 0~99
            if (random < criticalPercentage) // 0~24
            {
                // 크리티컬이 발동하고 광전사가 있을때 -> 100을 기준으로 400
                return (damage + applyDamage) * (criticalDamage / 100) * 2;
            }
            else
            {
                // 크리티컬이 안터지고 광전사가 있을때 -> 100을 기준으로 200
                return (damage + applyDamage) * 2;
            }
        }
    }

    // 컴퓨터 확인
    // 공격
    void GetInput()
    {
        // 컴퓨터 확인용 키입력
        fDown = Input.GetButton("Fire1");
        iDown = Input.GetButtonDown("Interaction");
    }

    // 경사각 계산 함수
    void CalcSlopeAngle()
    {
        isForwardObject = Physics.Raycast(transform.position + transform.up * 2, transform.forward, out RaycastHit hitForward, 5, layerMask); // 앞쪽 오브젝트
        isDownObject = Physics.Raycast(transform.position, transform.up * -1, out RaycastHit hitDown, 5, layerMask); // 아래쪽 오브젝트
        slopeAngle = (isForwardObject && isDownObject) ? Vector3.Angle(hitForward.normal, hitDown.normal) : 0f; // 경사각 계산
    }

    IEnumerator Immortality()
    {
        // 불사신
        while(true)
        {
            yield return null;

            // 처음상태는 무적아닌상태
            transform.gameObject.layer = 7;

            // 점점 시간을 줄여주고
            invincibilityTimeCalc -= Time.deltaTime;

            if (invincibilityTimeCalc <= 0)
            {
                // 3초뒤에
                invincibilityTimeCalc = invincibilityTime;

                // 무적상태
                transform.gameObject.layer = 15;

                // 1초간 지속
                yield return new WaitForSeconds(1f);
            }
        }
    }

    void FixedUpdate()
    {
        // 플레이어 물리충돌시 회전하는문제
        FreezeRotation();

        // 플레이어가 경계에 닿았는지 체크
        StopBorder();
    }

    void FreezeRotation()
    {
        // 플레이어 물리충돌시 회전하는문제
        rigid.angularVelocity = Vector3.zero;
    }

    void StopBorder()
    {
        // 플레이어가 경계와 닿았는지 체크
        // 플레이어가 벽을 뚫는문제 : 레이캐스트 길이 늘렸음 + 플레이어 이동방향으로 레이를 쏨
        // Debug.DrawRay(transform.position + transform.up * 5, transform.forward * 5, Color.green);
        isBorder = Physics.Raycast(transform.position + transform.up * 5, joystickScript.moveVec, 5, LayerMask.GetMask("Border"));
    }
    
    public void MobileShoot()
    {
        // 모바일 플레이어 공격
        // 마을이면 공격 불가능
        if (isShelter)
        {
            return;
        }

        // 궁수 공격
        if (!joystickScript.isDash && !isShoot && characterType.Equals("Archer"))
        {
            ArcherAttack();
        }

        // 소드, 성기사, 블랙스미스 공격
        if (!joystickScript.isDash && !isShoot)
        {
            if (characterType.Equals("Sword") || characterType.Equals("Blacksmith") || characterType.Equals("Holyknight"))
            {
                ComboAttack();
            }
        }

        // 법사 공격
        if (!joystickScript.isDash && !isShoot && characterType.Equals("Mage"))
        {
            MageAttack();
        }
    }

    void Shoot()
    {
        // 컴퓨터 플레이어 공격
        // 마을이면 공격 불가능
        if(isShelter)
        {
            return;
        }

        // 궁수 공격
        if(fDown && !joystickScript.isDash && !isShoot && characterType.Equals("Archer"))
        {
            ArcherAttack();
        }

        // 소드, 성기사, 블랙스미스 공격
        if (fDown && !joystickScript.isDash && !isShoot)
        {
            if (characterType.Equals("Sword") || characterType.Equals("Blacksmith") || characterType.Equals("Holyknight"))
            {
                ComboAttack();
            }
        }

        // 법사 공격
        if (fDown && !joystickScript.isDash && !isShoot && characterType.Equals("Mage"))
        {
            MageAttack();
        }
    }

    void ShootOut()
    {
        // 플레이어 공속제한
        isShoot = false;
    }

    void OnTriggerEnter(Collider other)
    {
        // 방벽에 따른 플레이어 피격
        if(barrier > 0)
        {
            // 방벽 있음
            // 플레이어 블락
            if (other.tag == "EnemyMelee")
            {
                // 근접 몬스터
                if (!isDamage)
                {
                    // 애니메이션
                    StartCoroutine(DoDamaged("doBlock"));

                    // 방벽감소
                    barrier--;

                    // 플레이어 피격 사운드 : 방벽 O
                    SoundManager.instance.SFXPlay(ObjType.방벽소리);
                }
            }
            else if (other.tag == "EnemyRange")
            {
                // 원거리 몬스터
                if (!isDamage)
                {
                    // 원거리 투사체는 리지드바디를 가지고있음
                    if (other.GetComponent<Rigidbody>() != null)
                    {
                        // 당근 반납
                        poolingManager.ReturnObj(other.gameObject, other.gameObject.GetComponent<Carrot>().type);
                    }

                    // 애니메이션
                    StartCoroutine(DoDamaged("doBlock"));

                    // 방벽 감소
                    barrier--;

                    // 플레이어 피격 사운드 : 방벽 O
                    SoundManager.instance.SFXPlay(ObjType.방벽소리);
                }
            }
        }
        else
        {
            // 방벽 없음
            // 플레이어 피격
            if (other.tag == "EnemyMelee")
            {
                // 근접 몬스터
                if (!isDamage)
                {
                    if (curHealth > 0)
                    {
                        Enemy enemy = other.GetComponentInParent<Enemy>();

                        // 저거너트
                        Juggernaut(enemy);

                        // 거울
                        // 영구적인 거울이 있는지 체크
                        if(!isPermanentSkill[5])
                        {
                            // 영구적인 거울이 없으면
                            if (PassiveSkill[5] > 0)
                            {
                                // 거울
                                Mirror(enemy);
                            }
                        }
                        else
                        {
                            // 영구적인 거울이 있을때
                            // 거울
                            Mirror(enemy);
                        }

                        // 피격 리액션
                        if (curHealth > 0)
                        {
                            // 살아있을때
                            StartCoroutine(DoDamaged("doDamaged"));
                        }
                        else
                        {
                            // 죽어있을때
                            // 영구적인 버티기가 있는지 체크
                            if(!isPermanentSkill[11])
                            {
                                // 영구적인 버터기가 없을때
                                if (PassiveSkill[11] > 0)
                                {
                                    // 버티기
                                    HoldOn();
                                }
                                else
                                {
                                    // 버티기가 없으면 죽음
                                    StartCoroutine("DoDie");
                                }
                            }
                            else
                            {
                                // 영구적인 버티기가 있을때
                                // 버티기
                                HoldOn();
                            }
                        }
                    }
                    // 플레이어 피격 사운드 : 방벽 X
                    SoundManager.instance.SFXPlay(ObjType.플레이어피격소리);
                }
            }
            else if (other.tag == "EnemyRange")
            {
                // 원거리 몬스터
                if (!isDamage)
                {
                    // 원거리 무기 스크립트
                    Carrot carrot = other.GetComponent<Carrot>();

                    // 저거너트
                    Juggernaut(carrot);

                    // 원거리 투사체는 리지드바디를 가지고있음
                    if (other.GetComponent<Rigidbody>() != null)
                    {
                        // 당근 반납
                        poolingManager.ReturnObj(other.gameObject, other.gameObject.GetComponent<Carrot>().type);
                    }

                    // 피격 리액션
                    if (curHealth > 0)
                    {
                        // 살아있을때
                        StartCoroutine(DoDamaged("doDamaged"));
                    }
                    else
                    {
                        // 죽어있을때
                        // 영구적인 버티기가 있는지 체크
                        if(!isPermanentSkill[11])
                        {
                            // 영구적인 버티기가 없을때
                            if (PassiveSkill[11] > 0)
                            {
                                // 버티기
                                HoldOn();
                            }
                            else
                            {
                                // 버티기 없으면 죽음
                                StartCoroutine("DoDie");
                            }
                        }
                        else
                        {
                            // 영구적인 버티기가 있을때
                            // 버티기
                            HoldOn();
                        }
                    }
                    // 플레이어 피격 사운드 : 방벽 X
                    SoundManager.instance.SFXPlay(ObjType.플레이어피격소리);
                }
            }
        }

        if (other.tag == "SecretDoorLeft" || other.tag == "SecretDoorRight")
        {
            // 시크릿 문 열기
            // 열린상태면 종료
            if(other.GetComponent<SecretDoor>().isOpen == true)
            {
                return;
            }

            // 문 열기
            if(other.tag == "SecretDoorLeft")
            {
                other.gameObject.transform.rotation *= Quaternion.Euler(0, 90f, 0);
            }
            else if(other.tag == "SecretDoorRight")
            {
                other.gameObject.transform.rotation *= Quaternion.Euler(0, -90f, 0);
            }

            // 문이 열린 상태
            other.gameObject.transform.GetComponent<SecretDoor>().isOpen = true;

            // 사운드
            SoundManager.instance.SFXPlay(ObjType.시크릿문소리);
        }

        if(other.tag == "FallTrigger")
        {
            // 떨어지면 죽음
            StartCoroutine("DoDie");
        }

        // 가까운 오브젝트 있음
        if(other.tag == "GoToDungeon" || other.tag == "Shop" || other.tag == "BossRoom")
        {
            nearObject = other.gameObject;
        }

        // 목표베이스 퀘스트 처리
        if(other.tag == "Shop" || other.tag == "GoToDungeon" || other.tag == "BossRoom")
        {
            foreach (QuestBase quest in QuestManager.instance.QuestList)
            {
                if (quest is ObjectiveBase)
                {
                    ObjectiveBase objectiveBase = quest as ObjectiveBase;
                    objectiveBase.Check();
                    if(other.tag == "BossRoom")
                    {
                        other.gameObject.SetActive(false);
                        Physics.IgnoreLayerCollision(7, 9, true);
                    }
                    return;
                }
            }
        }
    }

    void NextStageHP(float percentage)
    {
        // 패시브 유무에 따른 다음스테이지 HP회복
        if ((curHealth + maxHealth * percentage / 100f) > maxHealth)
        {
            // 최대체력을 넘어서는 회복을하면 현재체력을 최대체력으로
            curHealth = maxHealth;
        }
        else
        {
            // 아니라면 정상적으로 회복
            curHealth += maxHealth * percentage / 100f;
        }
    }

    // 플레이어 피격
    // 애니메이션 이름을 입력받아서 피격 및 블락애니메이션을 실행
    IEnumerator DoDamaged(string animName)
    {
        // 피격상태
        isDamage = true;

        yield return new WaitForSeconds(0.3f);

        // 애니메이션
        anim.SetTrigger(animName);

        yield return new WaitForSeconds(0.7f);

        // 피격상태 종료
        isDamage = false;
    }

    // 플레이어 죽음
    IEnumerator DoDie()
    {
        yield return new WaitForSeconds(0.3f);

        // 불사신을 먹었으면 불사신 초기화
        // 영구적인 불사신이 있는지 체크
        if(!isPermanentSkill[21])
        {
            // 영구적인 불사신이 없을때만 해제
            if (PassiveSkill[21] > 0)
            {
                StopCoroutine("Immortality");
            }
        }

        // 스킬리스트 텍스트 초기화
        skillListText.text = "";

        // 획득한 스킬의 설명이 저장된 변수 초기화
        sb.Clear();

        // 플레이어 죽음
        isDead = true;

        // 애니메이션
        anim.SetTrigger("doDie");

        // 2초뒤에
        yield return new WaitForSeconds(2f);

        // 페이드 인/아웃
        FadeInOut.Instance.Fade();

        // 1초뒤에
        yield return new WaitForSeconds(1f);

        // 마을로가면 죽는애니메이션 종료
        anim.SetTrigger("goShelter");

        // 마을로
        templates.GotoShelter();

        // 마을 배경음악
        SoundManager.instance.BgmSoundPlay(SoundManager.instance.bgmList[1]);
    }

    public void MobileInteraction()
    {
        // 상호작용
        // 모바일
        if (nearObject != null && !joystickScript.isJump && !joystickScript.isDash && !isDead)
        {
            Interaction();
        }
    }

    public void ComputerInteraction()
    {
        // 상호작용
        // 컴퓨터용
        if(iDown && nearObject != null && !joystickScript.isJump && !joystickScript.isDash && !isDead && !isShop)
        {
            Interaction();
        }
    }

    void GetActiveSkill(int activeSkillIndex, GameObject nearObject)
    {
        // 액티브 스킬 얻었을때 공통조건을 정의해둔 함수
        if (ActiveSkill[activeSkillIndex] > 0)
        {
            // 이미 먹은 스킬이야? -> 그럼 리턴
            return;
        }

        // 새로운 스킬이야? -> 그럼 먹고 반납
        ActiveSkill[activeSkillIndex]++;
        poolingManager.ReturnObj(nearObject, nearObject.GetComponent<Item>().objType);
    }

    void OnTriggerStay(Collider other)
    {
        // 가까운 오브젝트 있음
        if(other.tag == "PassiveSkill" || other.tag == "ActiveSkill" || other.tag == "Coin" || other.tag == "SecretBox" || other.tag == "NextStage" || other.tag == "GoToDungeon" || other.tag == "Shop")
        {
            nearObject = other.gameObject;
        }
    }

    void OnTriggerExit(Collider other)
    {
        // 가까운 오브젝트 없음
        if (other.tag == "PassiveSkill" || other.tag == "ActiveSkill" || other.tag == "Coin" || other.tag == "SecretBox" || other.tag == "NextStage" || other.tag == "GotoDungeon" || other.tag == "Shop")
        {
            nearObject = null;
        }
    }

    void DelaySpawn()
    {
        // 룸템플릿에 있는 똑같은함수
        // 던전으로 갔을때 waitTime이 초기화되지않아서
        // 몬스터와 보스가 바로 생성되는 문제
        templates.waitTime = 4f;
        templates.spawnedBoss = false;
    }

    public void RepositionPet(GameObject pet)
    {
        // 펫 장착 시 RepositionPet 함수가 호출되지 않는 문제
        // 로그 찍어 본 결과 if(isPet) 내부로 들어가지 못 함
        // 펫 장착 시 펫 생성보다 플래그 업데이트가 늦어져서 발생한 문제
        //Debug.Log("in1");
        // 펫의 위치를 재설정하는 함수
        if(isPet)
        {
            //Debug.Log("in2");
            // 네비메쉬 잠깐 꺼주고
            pet.GetComponent<Pet>().nav.enabled = false;

            // 트랜스폼 초기화한후
            pet.transform.position = transform.position;
            pet.transform.rotation = poolingManager.PetPrefs[3].transform.rotation;

            // 네비메쉬를 다시 켜준다
            pet.GetComponent<Pet>().nav.enabled = true;

            // 소환된 펫에 저장
            spawnedPet = pet;
        }
    }

    public void AbilityCollisionLogic(float skillBasicDamage, Enemy enemy, Transform skillTransform)
    {
        // 스킬 충돌 공통 로직
        // HP 감소 : 스킬기본데미지 + 플레이어공격력
        enemy.curHealth -= skillBasicDamage + damage;

        // 데미지 텍스트
        GameObject instantDamageText = poolingManager.GetObj(ObjType.데미지텍스트);
        instantDamageText.GetComponent<TextMeshPro>().text = (skillBasicDamage + damage).ToString();
        instantDamageText.transform.position = skillTransform.position + Vector3.up * 25;
        instantDamageText.transform.rotation = poolingManager.FloationTextPrefs[0].transform.rotation;

        // 카메라 흔들림
        CameraShake.Instance.OnCameraShake(0.1f, 0.5f);

        // 넉백
        Vector3 reactVec = enemy.transform.position - skillTransform.position;
        reactVec = reactVec.normalized;
        reactVec += Vector3.up;

        // 피격 리액션
        StartCoroutine(enemy.OnDamage(reactVec));
    }

    public void ArcherAttack()
    {
        // 궁수 공격 함수
        if (ActiveSkill[0] == 0 && ActiveSkill[1] == 0)
        {
            // 0, 1 모두 없을때
            // 화살 생성
            GameObject instantArrow = poolingManager.GetObj(ObjType.화살);

            // 화살 트랜스폼 초기화
            instantArrow.transform.position = ArrowPos[0].position;
            instantArrow.transform.rotation = ArrowPos[0].rotation;

            // 화살 발사
            Rigidbody arrowRigid = instantArrow.GetComponent<Rigidbody>();
            arrowRigid.velocity = ArrowPos[0].forward * 60;
        }
        else if (ActiveSkill[0] > 0 && ActiveSkill[1] == 0)
        {
            // 0 있을때
            // 화살 생성
            GameObject instantArrow1 = poolingManager.GetObj(ObjType.화살);
            GameObject instantArrow2 = poolingManager.GetObj(ObjType.화살);

            // 화살 트랜스폼 초기화
            instantArrow1.transform.position = ArrowPos[0].position;
            instantArrow1.transform.rotation = ArrowPos[0].rotation;
            instantArrow2.transform.position = ArrowPos[1].position;
            instantArrow2.transform.rotation = ArrowPos[1].rotation;

            // 화살 발사
            Rigidbody arrowRigid1 = instantArrow1.GetComponent<Rigidbody>();
            Rigidbody arrowRigid2 = instantArrow2.GetComponent<Rigidbody>();
            arrowRigid1.velocity = ArrowPos[0].forward * 60;
            arrowRigid2.velocity = ArrowPos[1].forward * 60;
        }
        else if (ActiveSkill[0] == 0 && ActiveSkill[1] > 0)
        {
            // 1 있을때
            // 화살 생성
            GameObject instantArrow1 = poolingManager.GetObj(ObjType.화살);
            GameObject instantArrow2 = poolingManager.GetObj(ObjType.화살);
            GameObject instantArrow3 = poolingManager.GetObj(ObjType.화살);

            // 화살 트랜스폼 초기화
            instantArrow1.transform.position = ArrowPos[0].position;
            instantArrow1.transform.rotation = Quaternion.Euler(transform.eulerAngles + new Vector3(0, -90f, 0));
            instantArrow2.transform.position = ArrowPos[1].position;
            instantArrow2.transform.rotation = Quaternion.Euler(transform.eulerAngles + new Vector3(0, 270f, 0));
            instantArrow3.transform.position = ArrowPos[2].position;
            instantArrow3.transform.rotation = ArrowPos[2].rotation;

            // 화살 발사
            Rigidbody arrowRigid1 = instantArrow1.GetComponent<Rigidbody>();
            Rigidbody arrowRigid2 = instantArrow2.GetComponent<Rigidbody>();
            Rigidbody arrowRigid3 = instantArrow3.GetComponent<Rigidbody>();
            arrowRigid1.velocity = ArrowPos[0].right * 60;
            arrowRigid2.velocity = ArrowPos[1].right * 60 * -1;
            arrowRigid3.velocity = ArrowPos[2].forward * 60;
        }
        else if (ActiveSkill[0] > 0 && ActiveSkill[1] > 0)
        {
            // 1, 2 모두 있을때
            // 화살 생성
            GameObject instantArrow1 = poolingManager.GetObj(ObjType.화살);
            GameObject instantArrow2 = poolingManager.GetObj(ObjType.화살);
            GameObject instantArrow3 = poolingManager.GetObj(ObjType.화살);
            GameObject instantArrow4 = poolingManager.GetObj(ObjType.화살);
            GameObject instantArrow5 = poolingManager.GetObj(ObjType.화살);
            GameObject instantArrow6 = poolingManager.GetObj(ObjType.화살);

            // 화살 트랜스폼 초기화
            instantArrow1.transform.position = ArrowPos[0].position;
            instantArrow1.transform.rotation = ArrowPos[0].rotation;
            instantArrow2.transform.position = ArrowPos[1].position;
            instantArrow2.transform.rotation = ArrowPos[1].rotation;
            instantArrow3.transform.position = ArrowPos[2].position;
            instantArrow3.transform.rotation = Quaternion.Euler(transform.eulerAngles + new Vector3(0, -270f, 0));
            instantArrow4.transform.position = ArrowPos[3].position;
            instantArrow4.transform.rotation = Quaternion.Euler(transform.eulerAngles + new Vector3(0, 90f, 0));
            instantArrow5.transform.position = ArrowPos[4].position;
            instantArrow5.transform.rotation = Quaternion.Euler(transform.eulerAngles + new Vector3(0, -90f, 0));
            instantArrow6.transform.position = ArrowPos[5].position;
            instantArrow6.transform.rotation = Quaternion.Euler(transform.eulerAngles + new Vector3(0, 270f, 0));

            // 화살 발사
            Rigidbody arrowRigid1 = instantArrow1.GetComponent<Rigidbody>();
            Rigidbody arrowRigid2 = instantArrow2.GetComponent<Rigidbody>();
            Rigidbody arrowRigid3 = instantArrow3.GetComponent<Rigidbody>();
            Rigidbody arrowRigid4 = instantArrow4.GetComponent<Rigidbody>();
            Rigidbody arrowRigid5 = instantArrow5.GetComponent<Rigidbody>();
            Rigidbody arrowRigid6 = instantArrow6.GetComponent<Rigidbody>();
            arrowRigid1.velocity = ArrowPos[0].forward * 60; // 0.07
            arrowRigid2.velocity = ArrowPos[1].forward * 60; // -0.07
            arrowRigid3.velocity = ArrowPos[2].right * 60; // 0
            arrowRigid4.velocity = ArrowPos[3].right * 60; // 0.07
            arrowRigid5.velocity = ArrowPos[4].right * 60 * -1; // -0.07
            arrowRigid6.velocity = ArrowPos[5].right * 60 * -1; // 0
        }

        // 슈팅 상태
        isShoot = true;

        // 슈팅 애니메이션
        anim.SetTrigger("doShoot");

        // 슈팅 종료
        Invoke("ShootOut", 1f - attackSpeed * 0.2f);

        // 슈팅 효과음 재생
        SoundManager.instance.SFXPlay(ObjType.슈팅소리);
    }

    public void MageAttack()
    {
        // 법사 공격 함수
        // 미사일 생성
        GameObject instantMageMissile = poolingManager.GetObj(ObjType.법사미사일);

        // 미사일 트랜스폼 초기화
        instantMageMissile.transform.position = ArrowPos[0].position;
        instantMageMissile.transform.rotation = ArrowPos[0].rotation;

        // 미사일 발사
        Rigidbody mageMissileRigid = instantMageMissile.GetComponent<Rigidbody>();
        mageMissileRigid.velocity = ArrowPos[0].forward * 60;

        // 슈팅 상태
        isShoot = true;

        // 애니메이션
        anim.SetTrigger("doShoot");

        // 슈팅 종료
        Invoke("ShootOut", 1f - attackSpeed * 0.2f);

        // 슈팅 효과음 재생
        SoundManager.instance.SFXPlay(ObjType.법사미사일소리);
    }

    // 콤보 공격 함수
    public void ComboAttack()
    {
        waitTime = 2f; // 콤보 대기시간

        CreateAndInitializeComboEffect(attackCnt); // 콤보 공격 이펙트 활성화
        
        switch (attackCnt) // 콤보 공격 애니메이션
        {
            case 0:
                anim.SetTrigger("doAbility1");
                break;
            case 1:
                anim.SetTrigger("doAbility0");
                break;
            case 2:
                anim.SetTrigger("doShoot");
                break;
        }

        attackCnt = (attackCnt + 1) % 3; // 콤보 카운트

        isShoot = true; // 공격 플래그

        Invoke("ShootOut", 1f - attackSpeed * 0.2f); // 공격 종료

        SoundManager.instance.SFXPlay(ObjType.칼소리); // 공격 사운드
    }

    // 콤보 공격 이펙트 활성화 함수
    void CreateAndInitializeComboEffect(int index)
    {
        if(characterType.Equals("Sword"))
        {
            GameObject instantComboAttack = poolingManager.GetObj((ObjType)((int)ObjType.전사콤보공격1이펙트 + index)); // 콤보 이펙트 활성화

            // 콤보 이펙트 트랜스폼 설정
            instantComboAttack.transform.position = transform.position + transform.forward * 10f + transform.up * 10f;
            instantComboAttack.transform.rotation = index == 1 ? transform.rotation * poolingManager.EffectPrefs[20].transform.rotation : transform.rotation;
        }
        else if(characterType.Equals("Blacksmith"))
        {
            GameObject instantComboAttack = poolingManager.GetObj((ObjType)((int)ObjType.블랙스미스콤보공격1이펙트 + index)); // 콤보 이펙트 활성화

            // 콤보 이펙트 트랜스폼 설정
            instantComboAttack.transform.position = transform.position + transform.forward * 10f + transform.up * 10f;
            instantComboAttack.transform.rotation = index == 1 ? transform.rotation * poolingManager.EffectPrefs[20].transform.rotation : transform.rotation;
        }
        else if(characterType.Equals("Holyknight"))
        {
            GameObject instantComboAttack = poolingManager.GetObj((ObjType)((int)ObjType.성기사콤보공격1이펙트 + index)); // 콤보 이펙트 활성화

            // 콤보 이펙트 트랜스폼 설정
            instantComboAttack.transform.position = transform.position + transform.forward * 10f + transform.up * 10f;
            instantComboAttack.transform.rotation = index == 1 ? transform.rotation * poolingManager.EffectPrefs[20].transform.rotation : transform.rotation;
        }  
    }


    public void Interaction()
    {
        if (nearObject.tag == "PassiveSkill") // 패시브 스킬이면
        {
            Item item = nearObject.GetComponent<Item>(); // 패시브 스킬 아이템
            PassiveSkillType passiveSkillType = (PassiveSkillType)item.value; // 패시브 스킬 타입

            // 영구적인 패시브 스킬이 있거나 이미 얻은 패시브 스킬이면 리턴
            if (isPermanentSkill[(int)passiveSkillType] || PassiveSkill[(int)passiveSkillType] > 0)
            {
                return;
            }

            // 스킬리스트 텍스트 표시
            sb.AppendLine(item.skillContent);
            skillListText.text = sb.ToString();

            // 패시브 스킬 획득
            PassiveSkill[(int)passiveSkillType]++;

            // 아이템 풀에 반환
            poolingManager.ReturnObj(nearObject, nearObject.GetComponent<Item>().objType);

            // 아이템 획득 사운드
            SoundManager.instance.SFXPlay(ObjType.아이템소리);

            // 추가적 처리가 필요한 패시브 스킬
            switch (passiveSkillType)
            {
                case PassiveSkillType.한발노리기:
                    criticalPercentage = 100; // 크리티컬 확률 증가
                    accuracy = 50; // 명중률 감소
                    break;
                case PassiveSkillType.백발백중:
                    accuracy = 100; // 명중률 증가
                    criticalDamage = 150; // 크리티컬 데미지 감소
                    break;
                case PassiveSkillType.초월방벽:
                    barrier += 20; // 방벽 증가
                    curHealth = 1; // 현재 체력 1
                    maxHealth = 1; // 최대 체력 1
                    break;
                case PassiveSkillType.흡혈귀:
                    bloodDrain = 20; // 흡혈량이 20%가 된다
                    break;
                case PassiveSkillType.도박꾼:
                    activeDropPercentage += 10; // 드랍률 증가
                    passiveDropPercentage += 10; // 드랍률 증가
                    inventoryItemPercentage += 10; // 드랍률 증가
                    break;
                case PassiveSkillType.시크릿:
                    secretPercentage += 10; // 시크릿상자 드랍률 증가
                    break;
                case PassiveSkillType.HP부스트:
                    maxHealth *= 2; // 최대 HP가 100% 증가
                    break;
                case PassiveSkillType.불사신:
                    StartCoroutine("Immortality"); // 1초간 3초간격으로 무적상태
                    break;
            }
        }
        else if (nearObject.tag == "ActiveSkill")
        {
            // 액티브 스킬이면
            // 근처의 액티브스킬을 아이템 참조변수로 받고
            Item item = nearObject.GetComponent<Item>();

            // 받은 액티브스킬의 인덱스를 저장
            int ActiveSkillIndex = item.value;

            // 현재 조건 : 풀링에서 hasItem 플래그를 false로 해서 활성화 시켜주고 던전에서 죽을때 패시브값이 모두 0이된다
            // hasItem 플래그가 false 이면서 해당패시브값이 0일때만 로직을 실행한다
            if (ActiveSkillIndex == 0)
            {
                // 멀티샷
                if (!item.hasItem && ActiveSkill[0] == 0)
                {
                    // 스킬설명을 리스트에 저장
                    sb.AppendLine("멀티샷 : 화살이 2개가 된다");

                    // 스킬을 얻었을때 개별로직에서의 공통로직을 정의해둔 함수
                    // 스킬리스트 텍스트에 표시
                    skillListText.text = sb.ToString();

                    // 사운드 재생
                    // 아이템 획득 음원 재생
                    SoundManager.instance.SFXPlay(ObjType.아이템소리);

                    // 가지고있는 상태
                    item.hasItem = true;
                }
            }
            else if (ActiveSkillIndex == 1)
            {
                // 사선샷
                if (!item.hasItem && ActiveSkill[1] == 0)
                {
                    // 스킬설명을 리스트에 저장
                    sb.AppendLine("사선샷 : 화살이 3개가 된다");

                    // 스킬을 얻었을때 개별로직에서의 공통로직을 정의해둔 함수
                    // 스킬리스트 텍스트에 표시
                    skillListText.text = sb.ToString();

                    // 사운드 재생
                    // 아이템 획득 음원 재생
                    SoundManager.instance.SFXPlay(ObjType.아이템소리);

                    // 가지고있는 상태
                    item.hasItem = true;
                }
            }

            // 액티브 스킬 공통로직 : 먹은적이 없는 액티브스킬 활성화 후 반납
            GetActiveSkill(ActiveSkillIndex, nearObject);
        }
        else if (nearObject.tag == "Coin")
        {
            // 코인이면
            // 코인의 스크립트를 가져온 후
            Item item = nearObject.GetComponent<Item>();

            if (!item.hasItem)
            {
                // 먹은 코인이 아니면
                // 해당코인의 값만큼 현재코인에 더하기
                coin += item.value;

                // 사운드 재생
                // 아이템 획득 음원 재생
                SoundManager.instance.SFXPlay(ObjType.아이템소리);

                // 먹은 코인
                item.hasItem = true;
            }

            // 현재코인이 최대코인을 넘기면 최대코인으로
            if (coin > maxCoin)
            {
                coin = maxCoin;
            }

            // 해당 코인 반납
            poolingManager.ReturnObj(nearObject.gameObject, nearObject.GetComponent<Item>().objType);
        }
        else if (nearObject.tag == "SecretBox")
        {
            // 시크릿 박스
            // 깨진상태가 아닐때에만
            if (nearObject.GetComponent<SecretBox>().isCrash == false)
            {
                // 깨진 상태로 변경
                nearObject.GetComponent<SecretBox>().isCrash = true;

                // 애니메이션 활성화
                nearObject.GetComponent<Animator>().SetTrigger("doCrash");

                // 스킬 드랍
                int random = Random.Range(0, 100); // 0~99
                if (random < secretPercentage) // 0~9
                {
                    // 랜덤 인덱스를 뽑는다
                    int passiveSkillRandom = Random.Range(0, 22); // 0~21

                    // 랜덤인덱스에 해당하는 패시브 스킬을 받아온다
                    GameObject instantPassiveSkill = poolingManager.GetObj((ObjType)((int)ObjType.한발노리기 + passiveSkillRandom));

                    // 받아온 패시브 스킬의 트랜스폼을 초기화한다
                    instantPassiveSkill.transform.position = nearObject.GetComponent<SecretBox>().transform.position;
                    instantPassiveSkill.transform.rotation = Quaternion.identity;
                }
                random = Random.Range(0, 100); // 0~99
                if (random < secretPercentage) // 0~9
                {
                    // 랜덤 인덱스를 뽑는다
                    int activeSkillRandom = Random.Range(0, 2); // 0~1

                    // 랜덤인덱스에 해당하는 액티브 스킬을 받아온다
                    GameObject instantActiveSkill = poolingManager.GetObj((ObjType)((int)ObjType.멀티샷 + activeSkillRandom));

                    // 받아온 액티브 스킬의 트랜스폼을 초기화한다
                    instantActiveSkill.transform.position = nearObject.GetComponent<SecretBox>().transform.position + new Vector3(0, 0, 5f);
                    instantActiveSkill.transform.rotation = Quaternion.identity;
                }
                random = Random.Range(0, 100); // 0~99
                if (random < secretPercentage) // 0~9
                {
                    // 랜덤 인덱스를 뽑는다
                    int inventoryItemRandom = Random.Range(0, 15); // 0~14

                    // 랜덤인덱스에 해당하는 인벤토리 아이템을 받아온다
                    GameObject instantInventoryItem = poolingManager.GetObj((ObjType)((int)ObjType.화려한장신구 + inventoryItemRandom));

                    // 받아온 인벤토리 아이템의 트랜스폼을 초기화한다
                    instantInventoryItem.transform.position = nearObject.GetComponent<SecretBox>().transform.position + new Vector3(0, 0, -5f);
                    instantInventoryItem.transform.rotation = Quaternion.identity;
                }

                // 상자 사운드
                SoundManager.instance.SFXPlay(ObjType.박스소리);
            }
        }
        else if (nearObject.tag == "NextStage")
        {
            // 다음스테이지
            // 페이드 인/아웃
            FadeInOut.Instance.Fade2();

            // 다음스테이지 함수 호출
            templates.NextStage();

            // 방벽 계산
            // 영구적인 방벽이 있는지 체크
            if (!isPermanentSkill[2])
            {
                // 영구적인 방벽이 없을때
                if (PassiveSkill[2] > 0)
                {
                    // 근심 유무에 따른 방벽
                    // 영구적인 근심이 있는지 체크
                    if (!isPermanentSkill[16])
                    {
                        // 영구적인 근심이 없을때
                        if (PassiveSkill[16] > 0)
                        {
                            // 근심이 있을때
                            barrier += 2;
                        }
                        else
                        {
                            // 근심이 없을때
                            barrier++;
                        }
                    }
                    else
                    {
                        // 영구적인 근심이 있을때
                        // 근심이 있을때
                        barrier += 2;
                    }
                }
            }
            else
            {
                // 영구적인 방벽이 있을때
                // 근심 유무에 따른 방벽
                // 영구적인 근심이 있는지 체크
                if (!isPermanentSkill[16])
                {
                    // 영구적인 근심이 없을때
                    if (PassiveSkill[16] > 0)
                    {
                        // 근심이 있을때
                        barrier += 2;
                    }
                    else
                    {
                        // 근심이 없을때
                        barrier++;
                    }
                }
                else
                {
                    // 영구적인 근심이 있을때
                    // 근심이 있을때
                    barrier += 2;
                }
            }

            // 초월방벽 계산
            // 영구적인 초월방벽이 있는지 체크
            if (!isPermanentSkill[3])
            {
                // 영구적인 초월방벽이 없을때
                if (PassiveSkill[3] > 0)
                {
                    // 근심 유무에 따른 초월방벽
                    // 영구적인 근심이 있는지 체크
                    if (!isPermanentSkill[16])
                    {
                        // 영구적인 근심이 없을때
                        if (PassiveSkill[16] > 0)
                        {
                            // 근심이 있을때
                            barrier += 4;
                        }
                        else
                        {
                            // 근심이 없을때
                            barrier += 2;
                        }
                    }
                    else
                    {
                        // 영구적인 근심이 있을때
                        // 근심이 있을때
                        barrier += 4;
                    }
                }
            }
            else
            {
                // 영구적인 초월방벽이 있을때
                // 근심 유무에 따른 초월방벽
                // 영구적인 근심이 있는지 체크
                if (!isPermanentSkill[16])
                {
                    // 영구적인 근심이 없을때
                    if (PassiveSkill[16] > 0)
                    {
                        // 근심이 있을때
                        barrier += 4;
                    }
                    else
                    {
                        // 근심이 없을때
                        barrier += 2;
                    }
                }
                else
                {
                    // 영구적인 근심이 있을때
                    // 근심이 있을때
                    barrier += 4;
                }
            }

            // 패시브 유무에 따른 HP 회복 계산
            // 영구적인 폭발적치유와 고동이 이있는지 체크
            if (!isPermanentSkill[13] && !isPermanentSkill[17])
            {
                // 영구적인 폭발적치유 X 영구적인 고동 X
                // 원래 로직 그대로 적용
                if (PassiveSkill[13] == 0 && PassiveSkill[17] == 0)
                {
                    // 폭발적치유와 고동 둘다없을때 -> 10% 회복
                    NextStageHP(10f);
                }
                else if (PassiveSkill[13] > 0 && PassiveSkill[17] == 0)
                {
                    // 폭발적치유만 있을때 -> 50% 회복
                    NextStageHP(50f);
                }
                else if (PassiveSkill[13] == 0 && PassiveSkill[17] > 0)
                {
                    // 고동만 있을때 -> 100% 회복
                    NextStageHP(100f);
                }
                else if (PassiveSkill[13] > 0 && PassiveSkill[17] > 0)
                {
                    // 폭발적치유와 고동 둘다있을때 -> 100% 회복
                    NextStageHP(100f);
                }
            }
            if (isPermanentSkill[13] && !isPermanentSkill[17])
            {
                // 영구적인 폭발적치유 O 영구적인 고동 X
                // 고동 유무에 따라서 로직
                if (PassiveSkill[17] == 0)
                {
                    // 고동 X
                    // 폭발적치유만 있을때 -> 50% 회복
                    NextStageHP(50f);
                }
                else
                {
                    // 고동 O
                    // 폭발적치유와 고동 둘다있을때 -> 100% 회복
                    NextStageHP(100f);
                }
            }
            if (!isPermanentSkill[13] && isPermanentSkill[17])
            {
                // 영구적인 폭발적치유 X 영구적인 고동 O
                // 폭발적치유 유무에 따라서 로직
                if (PassiveSkill[13] == 0)
                {
                    // 폭발적치유 X
                    // 고동만 있을때 -> 100% 회복
                    NextStageHP(100f);
                }
                else
                {
                    // 폭발적치유 O
                    // 폭발적치유와 고동 둘다있을때 -> 100% 회복
                    NextStageHP(100f);
                }
            }
            if (isPermanentSkill[13] && isPermanentSkill[17])
            {
                // 영구적인 폭발적치유 O 영구적인 고동 O
                // 폭발적치유 O 고동 O 인로직만 수행
                // 폭발적치유와 고동 둘다있을때 -> 100% 회복
                NextStageHP(100f);
            }

            // 던전 배경음악
            SoundManager.instance.BgmSoundPlay(SoundManager.instance.bgmList[3]);
        }
        else if (nearObject.tag == "GoToDungeon")
        {
            // 마을에서 던전으로가는 로직
            // 페이드 인/아웃
            FadeInOut.Instance.Fade2();

            // 스폰딜레이 : 던전으로 이동시 몬스터가 바로 생성되는문제
            DelaySpawn();

            // 새로운 EntryRoom 생성
            Instantiate(templates.entryRoom, templates.entryRoom.transform.position, Quaternion.identity);

            // 플레이어를 새로운 EntryRoom위치로 이동
            transform.position = templates.entryRoom.transform.position;

            // 펫의 위치 변경
            RepositionPet(spawnedPet);

            // 플레이어 마을아님
            isShelter = false;

            // 마을 비활성화
            shelter.SetActive(false);

            // 던전 배경음악
            SoundManager.instance.BgmSoundPlay(SoundManager.instance.bgmList[3]);
        }
        else if (nearObject.tag == "Shop")
        {
            // 상점
            // 상점 패널 활성화
            shopPanel.SetActive(true);

            // 상점 배경음악
            SoundManager.instance.BgmSoundPlay(SoundManager.instance.bgmList[2]);

            // 인벤토리 패널 위치 변경 : 상점과 인벤토리를 보여주기 위해 첫번째 자식으로
            inventoryPanel.transform.SetParent(shopPanel.transform);
            inventoryPanel.transform.SetAsFirstSibling();

            // 상점 데이터 베이스를 가져온후
            shopDatabase = nearObject.GetComponent<ShopDatabase>();

            // 상점 아이템 초기화
            for (int i = 0; i < shopDatabase.shopItemList.Count; i++)
            {
                //  UI를 그리기전에
                if (shopDatabase.shopItemList[i].itemType == ItemType.Ability)
                {
                    // 기본 : 궁수스킬
                    if (characterType.Equals("Sword"))
                    {
                        // 소드스킬이면
                        // 차례로 0, 1, 2번 스킬 아이템이 추가된다
                        shopDatabase.shopItemList[i] = abilitySwordSkillItem[abilitySkillCnt];

                        // Ability 스킬 카운트 증가
                        abilitySkillCnt++;
                    }
                    else if (characterType.Equals("Mage"))
                    {
                        // 마법사 스킬이면
                        // 차례로 0, 1, 2번 스킬 아이템이 추가된다
                        shopDatabase.shopItemList[i] = abilityMageSkillItem[abilitySkillCnt];

                        // Ability 스킬 카운트 증가
                        abilitySkillCnt++;
                    }
                    else if (characterType.Equals("Blacksmith"))
                    {
                        // 블랙스미스 스킬이면
                        // 차례로 0, 1, 2번 스킬 아이템이 추가된다
                        shopDatabase.shopItemList[i] = abilityBlacksmithSkillItem[abilitySkillCnt];

                        // Ability 스킬 카운트 증가
                        abilitySkillCnt++;
                    }
                    else if (characterType.Equals("Holyknight"))
                    {
                        // 성기사 스킬이면
                        // 차례로 0, 1, 2번 스킬 아이템이 추가된다
                        shopDatabase.shopItemList[i] = abilityHolyknightSkillItem[abilitySkillCnt];

                        // Ability 스킬 카운트 증가
                        abilitySkillCnt++;
                    }
                }

                //  UI를 그리기전에
                if (shopDatabase.shopItemList[i].equipmentItemType == EquipmentItemType.Weapon)
                {
                    // 기본 : 궁수무기
                    if (characterType.Equals("Sword") || characterType.Equals("Holyknight"))
                    {
                        // 전사나 성기사 무기이면
                        // 소드무기로 바꿔준다
                        shopDatabase.shopItemList[i] = swordItem[weaponCnt];

                        // 무기 카운트 증가 : 널 에러
                        weaponCnt++;
                    }
                    else if (characterType.Equals("Mage"))
                    {
                        // 마법사 무기이면
                        // 지팡이로 바꿔준다
                        shopDatabase.shopItemList[i] = staffItem[weaponCnt];

                        // 무기 카운트 증가 : 널 에러
                        weaponCnt++;
                    }
                    else if (characterType.Equals("Blacksmith"))
                    {
                        // 블랙스미스 무기이면
                        // 해머로 바꿔준다
                        shopDatabase.shopItemList[i] = hammerItem[weaponCnt];

                        // 무기 카운트 증가 : 널 에러
                        weaponCnt++;
                    }
                }

                // 상점에 아이템 추가
                shopSlots[i].inventoryItem = shopDatabase.shopItemList[i];

                // 상점 슬롯에 UI를 그려준다
                shopSlots[i].UpdateSlotUI();
            }

            // 현재 상점임
            isShop = true;

            // 사운드
            SoundManager.instance.SFXPlay(ObjType.버튼소리);
        }
    }   

    // 버티기
    public void HoldOn()
    {
        // 버티기
        int random = Random.Range(0, 2); // 0, 1
        if (random == 0)
        {
            // 죽기전 50% 확률로 버티며 체력이 1
            curHealth = 1;

            // 죽은게 아니므로 데미지를 입는 애니메이션을 실행
            StartCoroutine(DoDamaged("doDamaged"));

            // 구사일생
            // 영구적인 구사일생이 있는지 체크
            if (!isPermanentSkill[12])
            {
                // 영구적인 구사일생이 없을때
                if (PassiveSkill[12] > 0)
                {
                    // 버티기 성공시 HP 모두 회복
                    curHealth = maxHealth;
                }
            }
            else
            {
                // 영구적인 구사일생이 있을때
                // 버티기 성공시 HP 모두 회복
                curHealth = maxHealth;
            }
        }
        else
        {
            // 버티기 실패시 죽음
            StartCoroutine("DoDie");
        }
    }

    // 거울
    public void Mirror(Enemy enemyScript)
    {
        // 보스 폭탄 원거리를 제외한 공격을 반사
        int random = Random.Range(0, 2); // 0~1
        if (random == 0)
        {
            if (enemyScript.enemyType != Type.Ciclop || enemyScript.enemyType != Type.Bomb)
            {
                // 몬스터 죽음
                if (enemyScript.curHealth - enemyScript.damage <= 0)
                {
                    // 카운트베이스 퀘스트 처리
                    enemyScript.EnemyKillQuestCheck();

                    // 아이템 드랍
                    if (!enemyScript.isDrop)
                    {
                        enemyScript.ItemDrop(false);
                    }

                    // 방벽 얻기
                    enemyScript.GetBarrier();

                    // 몬스터 체력바 반납
                    poolingManager.ReturnObj(enemyScript.GetComponent<HpBar>().instantHpBar, ObjType.몬스터체력바);

                    // 몬스터 반납
                    poolingManager.ReturnObj(enemyScript.transform.gameObject, enemyScript.transform.gameObject.GetComponent<Enemy>().type);
                }
                else
                {
                    // 아니라면 정상적으로 데미지 반사
                    enemyScript.curHealth -= enemyScript.damage;
                }
            }
        }
    }

    // 저거너트 ( 근거리 )
    public void Juggernaut(Enemy enemyScript)
    {
        // 영구적인 저거너트가 있는지 체크
        if (!isPermanentSkill[7])
        {
            // 영구적인 저거너트가 없으면
            if (PassiveSkill[7] > 0)
            {
                // 저거너트
                // 받는피해 80% 감소
                curHealth -= enemyScript.damage * 20 / 100;
            }
            else
            {
                // 저거너트 X
                // 받는피해 그대로 적용
                curHealth -= enemyScript.damage;
            }
        }
        else
        {
            // 영구적인 저거너트가 있으면
            // 저거너트
            // 받는피해 80% 감소
            curHealth -= enemyScript.damage * 20 / 100;
        }
    }

    // 저거너트 ( 원거리 )
    public void Juggernaut(Carrot carrotScript)
    {
        // 영구적인 저거너트가 있는지 체크
        if (!isPermanentSkill[7])
        {
            // 영구적인 저거너트가 없을때
            if (PassiveSkill[7] > 0)
            {
                // 저거너트
                // 받는피해 80% 감소
                // 스테이지가 높아질수록 당근데미지 증가
                curHealth -= (carrotScript.damage + 100 * (templates.currentStage)) * 20 / 100;
            }
            else
            {
                // 저거너트 X
                // 받는피해 그대로 적용
                // 스테이지가 높아질수록 당근데미지 증가
                curHealth -= carrotScript.damage + 100 * (templates.currentStage);
            }
        }
        else
        {
            // 영구적인 저거너트가 있을때
            // 저거너트
            // 받는피해 80% 감소
            // 스테이지가 높아질수록 당근데미지 증가
            curHealth -= (carrotScript.damage + 100 * (templates.currentStage)) * 20 / 100;
        }
    }
}
