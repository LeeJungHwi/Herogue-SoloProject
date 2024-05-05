using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
using TMPro;
using System.Linq;
using Sirenix.OdinInspector;

public class Player : MonoBehaviour
{
    [HideInInspector] public bool isBorder = false, isShoot = false, isDamage, isDead, isSecretRoom, isShelter = true, iDown, fDown, isShop; // 체크
    private float invincibilityTime = 3f, invincibilityTimeCalc = 3f; // 불사신 무적시간 계산용
    
    [Title ("플레이어 스탯")]
    [DetailedInfoBox ("플레이어 스탯 정보", "현재체력\n풀체력\n공속\n공격력\n크리티컬뎀\n정확도\n크리티컬확률\n시크릿박스확률\n액티브드랍률\n패시브드랍률\n인벤토리아이템드랍률")]
    public float curHealth;
    public float maxHealth, attackSpeed = 0.5f, damage, criticalDamage, accuracy;
    [HideInInspector] public float bloodDrain, barrier;
    public int criticalPercentage;
    public float secretPercentage, activeDropPercentage, passiveDropPercentage, inventoryItemPercentage;
    public int coin, maxCoin;

    [Title ("플레이어 장비")]
    public Image weaponSlotImage; // 장착된 장비의 이미지를 보여주기 위한 변수들
    public Image armorSlotImage, gloveSlotImage, shoesSlotImage, amuletSlotImage;
    [PropertySpace (0, 20)] public Image petSlotImage;
    [HideInInspector] public bool isWeapon, isArmor, isGlove, isShoes, isAmulet, isPet; // 현재 장착된 무기가 있는지 체크하는 변수 => 장착된 장비가 있는상태에서도 장비가 착용되는문제 => 장착 : 플래그가 false 일때만 가능, 장착후에 플래그 true, 해제 : 플래그 false
    [HideInInspector] public InventoryItem equipedWeaponItem, equipedArmorItem, equipedGloveItem, equipedShoesItem, equipedAmuletItem, equipedPetItem; // 현재 장비 슬롯에 장착되어있는 장비를 저장하는 변수들 
    [HideInInspector] public GameObject spawnedPet; // 현재 소환된 펫 저장

    [Title ("인벤토리 및 상점")]
    public GameObject shopPanel;
    public GameObject inventoryPanel;
    [HideInInspector] public ShopDatabase shopDatabase;
    [HideInInspector] public ShopSlot[] shopSlots;
    [SerializeField] private Transform shopSlotHolder; // 상점 슬롯을 관리하는 변수
    [SerializeField] [FoldoutGroup ("상점 이미지 변경")] private InventoryItem[] abilitySwordSkillItem, abilityMageSkillItem, abilityBlacksmithSkillItem, abilityHolyknightSkillItem; // 스킬 아이템 : 캐릭터에 따라 상점 스킬 아이템을 바꾸기 위해
    [HideInInspector] public int abilitySkillCnt; // 스킬 카운트 : 캐릭터에 따라 상점 스킬 아이템을 바꾸기 위해
    [SerializeField] [FoldoutGroup ("상점 이미지 변경")] private InventoryItem[] swordItem, staffItem, hammerItem; // 무기 아이템 : 캐릭터에 따라 상점 무기를 바꾸기 위해, 검은 성기사도 같이 사용
    [HideInInspector] public int weaponCnt; // 무기 아이템 카운트 : 캐릭터에 따라 상점 무기를 바꾸기 위해

    [Title ("플레이어 스킬")]
    [PropertySpace (20, 0)] [InfoBox ("영구 패시브 표시")] public Text permanentSkillListText;
    public StringBuilder permanentSb = new StringBuilder(); // 영구 스킬의 설명을 저장 할 변수
    [InfoBox ("영구 패시브 체크")] public bool[] isPermanentSkill; // 영구 패시브 스킬인지 체크
    [HideInInspector] public int permanentSkillCnt; // 영구 패시브 획득 횟수
    [InfoBox ("액티브 잠금 이미지")] public GameObject[] abilityLock; // 액티브스킬 잠금 이미지
    [InfoBox ("액티브 해제 체크")] public bool[] isAbility; // 액티브스킬을 해제했는지 체크
    [InfoBox ("0 : 멀티샷 1 : 사선샷")] public List<int> ActiveSkill = new List<int>(); // 액티브 관리
    public enum PassiveSkillType { 한발노리기, 백발백중, 방벽, 초월방벽, 흡혈귀, 거울, 도박꾼, 저거너트, 즉사, 시크릿, 혈액갑옷, 버티기, 구사일생, 폭발적치유, HP부스트, 광전사, 근심, 고동, 꿈의끝, 분신, 향상된대쉬, 불사신 } // 패시브 스킬 타입")
    [DetailedInfoBox ("패시브 정보", "0 : 한발노리기 -> 크리티컬 확률 100% 명중률 50%\n1 : 백발백중 -> 명중률 100% 크리티컬 데미지 50% 감소\n2 : 방벽 -> 다음스테이지 갈때마다 1개씩 얻는다 적의 공격을 1회 막는다\n3 : 초월방벽 -> 방벽을 즉시 20개얻는대신 체력이 1이된다 다음스테이지 갈때마다 2개씩 얻는다 적의 공격을 1회 막는다\n4 : 흡혈귀 -> 입힌데미지의 20% 회복\n5 : 거울 -> 50 확률로 공격을 반사 플레이어도 데미지를 입는다 보스와 폭탄은 반사를 하지않는다 근접공격만 반사한다\n6 : 도박꾼 -> 몬스터 아이템 드랍률 10% 증가\n7 : 저거너트 -> 받는피해가 80% 감소\n8 : 즉사 -> 보스를 제외하고 체력이 20% 이하인적을 공격하면 즉사시킨다\n9 : 시크릿 -> 시크릿박스에서 아이템 드랍률 10% 증가\n10 : 혈액갑옷 -> 플레이어 최대체력의 10% 추가 데미지를 입힌다\n11 : 버티기 -> 죽기전 50% 확률로 버티며 체력이 1이된다\n12 : 구사일생 -> 버티기 성공시 HP 모두 회복\n13 : 폭발적치유 -> 다음스테이지로 갈때 현재체력50% 회복\n14 : HP 부스트 -> 최대체력이 100% 증가\n15 : 광전사 -> 공격 및 피격 모두 데미지가 2배가 된다\n16 : 근심 -> 방벽의 획득량이 두배가된다\n17 : 고동 -> 다음스테이지로 갈때 현제체력100% 회복\n18 : 꿈의 끝 -> 보스를 제외하고 체력이 50% 이하인적을 공격하면 즉사시킨다\n19 : 분신 -> 몬스터를 제거하면 방벽을 1개 얻는다 근심을 고려해서 몬스터가 제거되는 모든곳에서 방벽을 증가시켜줘야한다\n20 : 향상된대쉬 -> 대쉬할때 무적상태가 된다\n21 : 불사신 -> 3초간격으로 무적상태가 된다")]
    public List<int> PassiveSkill = new List<int>(); // 패시브 관리

    [Title ("기타 필드")]
    [SerializeField] [PropertySpace (20, 20)] [InfoBox ("발사체 생성 위치")] private Transform[] ArrowPos;
    public PoolingManager poolingManager;
    [HideInInspector] public Animator anim;
    private Rigidbody rigid;
    [HideInInspector] public GameObject nearObject;
    public Joystick joystickScript;
    [SerializeField] private RoomTemplates templates;
    private StringBuilder sb = new StringBuilder(); // 획득한 스킬의 설명을 저장 할 변수
    [SerializeField] private Text skillListText, statusInfoText, coinText, barrierText;
    [SerializeField] private Slider playerHpBar;
    [HideInInspector] public float slopeAngle; // 경사각
    private bool isForwardObject, isDownObject; // 플레이어 앞쪽 아래쪽에 오브젝트가 있는지 체크
    [SerializeField] private LayerMask layerMask; // 경사각 레이어마스크
    public GameObject shelter;
    private int attackCnt; // 공격 카운트 : 콤보 공격을 위해
    private float waitTime; // 공격 카운트 대기시간 : 콤보 공격을 위해
    [HideInInspector] public string characterType; // 선택된 캐릭터

    private void Awake() 
    {
        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();
        shopSlots = shopSlotHolder.GetComponentsInChildren<ShopSlot>();
        characterType = DataManager.instance.character.ToString();
        if(characterType != gameObject.name) gameObject.SetActive(false); // 선택된 캐릭터와 다르면 삭제
    }

    private void Update()
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
        // 대기시간이 0 이하이면서 공격상태가 아닐때
        // 공격 카운트 초기화
        if (waitTime <= 0 && !isShoot) attackCnt = 0;
        else waitTime -= Time.deltaTime;
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
                // 크리티컬이 발동하고 광전사가 있을때 -> 100을 기준으로 400
                // 크리티컬이 발동하고 광전사가 없을때 -> 100을 기준으로 200
                if (PassiveSkill[15] > 0) return (damage + applyDamage) * (criticalDamage / 100) * 2;
                else return (damage + applyDamage) * (criticalDamage / 100);
            }
            else // 25~99
            {
                // 크리티컬이 안터졌을때 광전사 유무
                // 크리티컬이 안터지고 광전사가 있을때 -> 100을 기준으로 200
                // 크리티컬이 안터지고 광전사가 없을때 -> 100을 기준으로 100
                if (PassiveSkill[15] > 0) return (damage + applyDamage) * 2;
                else return damage + applyDamage;
            }
        }
        else
        {
            // 영구적인 광전사가 있으면
            int random = Random.Range(0, 100); // 0~99
            if (random < criticalPercentage) return (damage + applyDamage) * (criticalDamage / 100) * 2; // 0~24 크리티컬이 발동하고 광전사가 있을때 -> 100을 기준으로 400
            else return (damage + applyDamage) * 2; // 크리티컬이 안터지고 광전사가 있을때 -> 100을 기준으로 200
        }
    }

    // 컴퓨터 확인용
    // 공격
    private void GetInput()
    {
        fDown = Input.GetButton("Fire1");
        iDown = Input.GetButtonDown("Interaction");
    }

    // 경사각 계산 함수
    private void CalcSlopeAngle()
    {
        isForwardObject = Physics.Raycast(transform.position + transform.up * 2, transform.forward, out RaycastHit hitForward, 5, layerMask); // 앞쪽 오브젝트
        isDownObject = Physics.Raycast(transform.position, transform.up * -1, out RaycastHit hitDown, 5, layerMask); // 아래쪽 오브젝트
        slopeAngle = (isForwardObject && isDownObject) ? Vector3.Angle(hitForward.normal, hitDown.normal) : 0f; // 경사각 계산
    }

    private IEnumerator Immortality()
    {
        // 불사신
        while(true)
        {
            yield return null;
            transform.gameObject.layer = 7; // 처음상태는 무적아닌상태
            invincibilityTimeCalc -= Time.deltaTime; // 점점 시간을 줄여주고

            if (invincibilityTimeCalc <= 0)
            {
                invincibilityTimeCalc = invincibilityTime; // 3초뒤에
                transform.gameObject.layer = 15; // 무적상태
                yield return new WaitForSeconds(1f); // 1초간 지속
            }
        }
    }

    private void FixedUpdate()
    {
        FreezeRotation(); // 플레이어 물리충돌시 회전하는문제
        StopBorder(); // 플레이어가 경계에 닿았는지 체크
    }

    // 플레이어 물리충돌시 회전하는문제
    private void FreezeRotation() { rigid.angularVelocity = Vector3.zero; }

    // 플레이어가 경계와 닿았는지 체크
    // 플레이어가 벽을 뚫는문제 : 레이캐스트 길이 늘렸음 + 플레이어 이동방향으로 레이를 쏨
    private void StopBorder() { isBorder = Physics.Raycast(transform.position + transform.up * 5, joystickScript.moveVec, 5, LayerMask.GetMask("Border")); }
    
    // 모바일 플레이어 공격
    public void MobileShoot()
    {
        if (isShelter) return; // 마을이면 공격 불가능
        if (!joystickScript.isDash && !isShoot && characterType.Equals("Archer")) ArcherAttack(); // 궁수 공격
        if (!joystickScript.isDash && !isShoot && (characterType.Equals("Sword") || characterType.Equals("Blacksmith") || characterType.Equals("Holyknight"))) ComboAttack(); // 소드, 성기사, 블랙스미스 공격
        if (!joystickScript.isDash && !isShoot && characterType.Equals("Mage")) MageAttack(); // 법사 공격
    }

    // 컴퓨터 플레이어 공격
    private void Shoot()
    {
        if(isShelter) return; // 마을이면 공격 불가능
        if(fDown && !joystickScript.isDash && !isShoot && characterType.Equals("Archer")) ArcherAttack(); // 궁수 공격
        if (fDown && !joystickScript.isDash && !isShoot && (characterType.Equals("Sword") || characterType.Equals("Blacksmith") || characterType.Equals("Holyknight"))) ComboAttack(); // 소드, 성기사, 블랙스미스 공격
        if (fDown && !joystickScript.isDash && !isShoot && characterType.Equals("Mage")) MageAttack(); // 법사 공격
    }

    // 플레이어 공속제한
    private void ShootOut() { isShoot = false; }

    private void OnTriggerEnter(Collider other)
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
                    StartCoroutine(DoDamaged("doBlock")); // 애니메이션
                    barrier--; // 방벽감소
                    SoundManager.instance.SFXPlay(ObjType.방벽소리); // 플레이어 피격 사운드 : 방벽 O
                }
            }
            else if (other.tag == "EnemyRange")
            {
                // 원거리 몬스터
                if (!isDamage)
                {
                    if (other.GetComponent<Rigidbody>() != null) poolingManager.ReturnObj(other.gameObject, other.gameObject.GetComponent<Carrot>().type); // 원거리 투사체는 리지드바디를 가지고있음 => 당근 반납
                    StartCoroutine(DoDamaged("doBlock")); // 애니메이션
                    barrier--; // 방벽 감소
                    SoundManager.instance.SFXPlay(ObjType.방벽소리); // 플레이어 피격 사운드 : 방벽 O
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
                        Juggernaut(enemy); // 저거너트

                        // 거울
                        // 영구적인 거울이 있는지 체크
                        if(!isPermanentSkill[5])
                        {
                            // 영구적인 거울이 없으면
                            if (PassiveSkill[5] > 0) Mirror(enemy);
                        }
                        else Mirror(enemy); // 영구적인 거울이 있을때

                        // 피격 리액션
                        if (curHealth > 0) StartCoroutine(DoDamaged("doDamaged")); // 살아있을때
                        else
                        {
                            // 죽어있을때
                            // 영구적인 버티기가 있는지 체크
                            if(!isPermanentSkill[11])
                            {
                                // 영구적인 버터기가 없을때
                                if (PassiveSkill[11] > 0) HoldOn();
                                else StartCoroutine("DoDie"); // 버티기가 없으면 죽음
                            }
                            else HoldOn(); // 영구적인 버티기가 있을때
                        }
                    }
                    SoundManager.instance.SFXPlay(ObjType.플레이어피격소리); // 플레이어 피격 사운드 : 방벽 X
                }
            }
            else if (other.tag == "EnemyRange")
            {
                // 원거리 몬스터
                if (!isDamage)
                {
                    Carrot carrot = other.GetComponent<Carrot>();
                    Juggernaut(carrot); // 저거너트

                    // 원거리 투사체는 리지드바디를 가지고있음 => 당근 반납
                    if (other.GetComponent<Rigidbody>() != null) poolingManager.ReturnObj(other.gameObject, other.gameObject.GetComponent<Carrot>().type);

                    // 피격 리액션
                    if (curHealth > 0) StartCoroutine(DoDamaged("doDamaged")); // 살아있을때
                    else
                    {
                        // 죽어있을때
                        // 영구적인 버티기가 있는지 체크
                        if(!isPermanentSkill[11])
                        {
                            // 영구적인 버티기가 없을때
                            if (PassiveSkill[11] > 0) HoldOn();
                            else StartCoroutine("DoDie"); // 버티기 없으면 죽음
                        }
                        else HoldOn(); // 영구적인 버티기가 있을때
                    }
                    SoundManager.instance.SFXPlay(ObjType.플레이어피격소리); // 플레이어 피격 사운드 : 방벽 X
                }
            }
        }

        if (other.tag == "SecretDoorLeft" || other.tag == "SecretDoorRight")
        {
            // 시크릿 문 열기
            // 열린상태면 종료
            if(other.GetComponent<SecretDoor>().isOpen == true) return;

            // 문 열기
            if(other.tag == "SecretDoorLeft") other.gameObject.transform.rotation *= Quaternion.Euler(0, 90f, 0);
            else if(other.tag == "SecretDoorRight") other.gameObject.transform.rotation *= Quaternion.Euler(0, -90f, 0);

            // 문이 열린 상태
            other.gameObject.transform.GetComponent<SecretDoor>().isOpen = true;

            // 사운드
            SoundManager.instance.SFXPlay(ObjType.시크릿문소리);
        }

        if(other.tag == "FallTrigger") StartCoroutine("DoDie"); // 떨어지면 죽음

        if(other.tag == "GoToDungeon" || other.tag == "Shop") nearObject = other.gameObject; // 가까운 오브젝트 있음

        // 목표베이스 퀘스트 처리
        if(other.tag == "Shop" || other.tag == "GoToDungeon")
        {
            foreach (QuestBase quest in QuestManager.instance.QuestList)
            {
                if (quest is ObjectiveBase)
                {
                    ObjectiveBase objectiveBase = quest as ObjectiveBase;
                    objectiveBase.Check();
                    return;
                }
            }
        }

        if(other.tag == "QuestBorder") QuestManager.instance.QuestNotify("퀘스트를 완료하세요!");
    }

    // 패시브 유무에 따른 다음스테이지 HP회복
    private void NextStageHP(float percentage)
    {
        if ((curHealth + maxHealth * percentage / 100f) > maxHealth) curHealth = maxHealth;
        else curHealth += maxHealth * percentage / 100f;
    }

    // 플레이어 피격
    // 애니메이션 이름을 입력받아서 피격 및 블락애니메이션을 실행
    private IEnumerator DoDamaged(string animName)
    {
        isDamage = true; // 피격상태
        yield return new WaitForSeconds(0.3f);
        anim.SetTrigger(animName); // 애니메이션
        yield return new WaitForSeconds(0.7f);
        isDamage = false; // 피격상태 종료
    }

    // 플레이어 죽음
    private IEnumerator DoDie()
    {
        yield return new WaitForSeconds(0.3f);

        // 불사신을 먹었으면 불사신 초기화
        // 영구적인 불사신이 있는지 체크
        // 영구적인 불사신이 없을때만 해제
        if(!isPermanentSkill[21] && PassiveSkill[21] > 0) StopCoroutine("Immortality");

        skillListText.text = ""; // 스킬리스트 텍스트 초기화
        sb.Clear(); // 획득한 스킬의 설명이 저장된 변수 초기화
        isDead = true; // 플레이어 죽음
        anim.SetTrigger("doDie"); // 애니메이션
        yield return new WaitForSeconds(2f);
        FadeInOut.Instance.Fade(); // 페이드 인/아웃
        yield return new WaitForSeconds(1f);
        anim.SetTrigger("goShelter"); // 마을로가면 죽는애니메이션 종료
        templates.GotoShelter(); // 마을로
        SoundManager.instance.BgmSoundPlay(SoundManager.instance.bgmList[1]); // 마을 배경음악
    }

    // 모바일 상호작용 
    public void MobileInteraction() { if (nearObject != null && !joystickScript.isJump && !joystickScript.isDash && !isDead) Interaction(); }

    // 컴퓨터 상호작용
    private void ComputerInteraction() { if(iDown && nearObject != null && !joystickScript.isJump && !joystickScript.isDash && !isDead && !isShop) Interaction(); }

    // 액티브 스킬 얻었을때 공통조건을 정의해둔 함수
    private void GetActiveSkill(int activeSkillIndex, GameObject nearObject)
    {
        if (ActiveSkill[activeSkillIndex] > 0) return; // 이미 먹은 스킬이야? -> 그럼 리턴

        // 새로운 스킬이야? -> 그럼 먹고 반납
        ActiveSkill[activeSkillIndex]++;
        poolingManager.ReturnObj(nearObject, nearObject.GetComponent<Item>().objType);
    }

    // 가까운 오브젝트 있음
    private void OnTriggerStay(Collider other) { if(other.tag == "PassiveSkill" || other.tag == "ActiveSkill" || other.tag == "Coin" || other.tag == "SecretBox" || other.tag == "NextStage" || other.tag == "GoToDungeon" || other.tag == "Shop") nearObject = other.gameObject; } 

    // 가까운 오브젝트 없음
    private void OnTriggerExit(Collider other) { if (other.tag == "PassiveSkill" || other.tag == "ActiveSkill" || other.tag == "Coin" || other.tag == "SecretBox" || other.tag == "NextStage" || other.tag == "GotoDungeon" || other.tag == "Shop") nearObject = null; }

    // 룸템플릿에 있는 똑같은함수
    // 던전으로 갔을때 waitTime이 초기화되지않아서
    // 몬스터와 보스가 바로 생성되는 문제
    private void DelaySpawn()
    {
        templates.waitTime = 4f;
        templates.spawnedBoss = false;
    }

    // 펫 장착 시 RepositionPet 함수가 호출되지 않는 문제
    // 로그 찍어 본 결과 if(isPet) 내부로 들어가지 못 함
    // 펫 장착 시 펫 생성보다 플래그 업데이트가 늦어져서 발생한 문제
    // 펫의 위치를 재설정하는 함수
    public void RepositionPet(GameObject pet)
    {
        if(isPet)
        {
            pet.GetComponent<Pet>().nav.enabled = false; // 네비메쉬 잠깐 꺼주고
            pet.transform.position = transform.position; // 트랜스폼 초기화한후
            pet.transform.rotation = poolingManager.PetPrefs[3].transform.rotation;
            pet.GetComponent<Pet>().nav.enabled = true; // 네비메쉬를 다시 켜준다
            spawnedPet = pet;// 소환된 펫에 저장
        }
    }

    // 스킬 충돌 공통 로직
    public void AbilityCollisionLogic(float skillBasicDamage, Enemy enemy, Transform skillTransform)
    {
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

    // 궁수 공격 함수
    public void ArcherAttack()
    {
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

        isShoot = true; // 슈팅 상태
        anim.SetTrigger("doShoot"); // 슈팅 애니메이션
        Invoke("ShootOut", 1f - attackSpeed * 0.2f); // 슈팅 종료
        SoundManager.instance.SFXPlay(ObjType.슈팅소리); // 슈팅 효과음 재생
    }

    // 법사 공격 함수
    public void MageAttack()
    {
        // 미사일 생성
        GameObject instantMageMissile = poolingManager.GetObj(ObjType.법사미사일);

        // 미사일 트랜스폼 초기화
        instantMageMissile.transform.position = ArrowPos[0].position;
        instantMageMissile.transform.rotation = ArrowPos[0].rotation;

        // 미사일 발사
        Rigidbody mageMissileRigid = instantMageMissile.GetComponent<Rigidbody>();
        mageMissileRigid.velocity = ArrowPos[0].forward * 60;

        isShoot = true; // 슈팅 상태
        anim.SetTrigger("doShoot"); // 애니메이션
        Invoke("ShootOut", 1f - attackSpeed * 0.2f); // 슈팅 종료
        SoundManager.instance.SFXPlay(ObjType.법사미사일소리); // 슈팅 효과음 재생
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
    private void CreateAndInitializeComboEffect(int index)
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

    // 상호작용
    public void Interaction()
    {
        if (nearObject.tag == "PassiveSkill") // 패시브 스킬이면
        {
            Item item = nearObject.GetComponent<Item>(); // 패시브 스킬 아이템
            PassiveSkillType passiveSkillType = (PassiveSkillType)item.value; // 패시브 스킬 타입

            // 영구적인 패시브 스킬이 있거나 이미 얻은 패시브 스킬이면 리턴
            if (isPermanentSkill[(int)passiveSkillType] || PassiveSkill[(int)passiveSkillType] > 0) return;

            // 스킬리스트 텍스트 표시
            sb.AppendLine(item.skillContent);
            skillListText.text = sb.ToString();

            PassiveSkill[(int)passiveSkillType]++; // 패시브 스킬 획득
            poolingManager.ReturnObj(nearObject, nearObject.GetComponent<Item>().objType); // 아이템 풀에 반환
            SoundManager.instance.SFXPlay(ObjType.아이템소리); // 아이템 획득 사운드

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
            // 스킬설명을 리스트에 저장
            // 스킬리스트 텍스트에 표시
            // 아이템 획득 음원 재생
            // 가지고있는 상태
            if (ActiveSkillIndex == 0)
            {
                // 멀티샷
                if (!item.hasItem && ActiveSkill[0] == 0)
                {
                    sb.AppendLine("멀티샷 : 화살이 2개가 된다");
                    skillListText.text = sb.ToString();
                    SoundManager.instance.SFXPlay(ObjType.아이템소리);
                    item.hasItem = true;
                }
            }
            else if (ActiveSkillIndex == 1)
            {
                // 사선샷
                if (!item.hasItem && ActiveSkill[1] == 0)
                {
                    sb.AppendLine("사선샷 : 화살이 3개가 된다");
                    skillListText.text = sb.ToString();
                    SoundManager.instance.SFXPlay(ObjType.아이템소리);
                    item.hasItem = true;
                }
            }

            GetActiveSkill(ActiveSkillIndex, nearObject); // 액티브 스킬 공통로직 : 먹은적이 없는 액티브스킬 활성화 후 반납
        }
        else if (nearObject.tag == "Coin")
        {
            // 코인
            Item item = nearObject.GetComponent<Item>();

            if (!item.hasItem)
            {
                coin += item.value;
                SoundManager.instance.SFXPlay(ObjType.아이템소리);
                item.hasItem = true;
            }

            if (coin > maxCoin) coin = maxCoin; // 현재코인이 최대코인을 넘기면 최대코인으로
            poolingManager.ReturnObj(nearObject.gameObject, nearObject.GetComponent<Item>().objType); // 해당 코인 반납
        }
        else if (nearObject.tag == "SecretBox")
        {
            // 시크릿 박스
            // 깨진상태가 아닐때에만
            if (nearObject.GetComponent<SecretBox>().isCrash == false)
            {
                nearObject.GetComponent<SecretBox>().isCrash = true; // 깨진 상태로 변경
                nearObject.GetComponent<Animator>().SetTrigger("doCrash"); // 애니메이션 활성화

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
                        if (PassiveSkill[16] > 0) barrier += 2; // 근심이 있을때
                        else barrier++; // 근심이 없을때
                    }
                    else barrier += 2; // 영구적인 근심이 있을때
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
                    if (PassiveSkill[16] > 0) barrier += 2; // 근심이 있을때
                    else barrier++; // 근심이 없을때
                }
                else barrier += 2; // 영구적인 근심이 있을때
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
                        if (PassiveSkill[16] > 0) barrier += 4; // 근심이 있을때
                        else barrier += 2; // 근심이 없을때
                    }
                    else barrier += 4; // 영구적인 근심이 있을때
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
                    if (PassiveSkill[16] > 0) barrier += 4; // 근심이 있을때
                    else barrier += 2; // 근심이 없을때
                }
                else barrier += 4; // 영구적인 근심이 있을때
            }

            // 패시브 유무에 따른 HP 회복 계산
            // 영구적인 폭발적치유와 고동이 이있는지 체크
            if (!isPermanentSkill[13] && !isPermanentSkill[17])
            {
                // 영구적인 폭발적치유 X 영구적인 고동 X
                // 원래 로직 그대로 적용
                if (PassiveSkill[13] == 0 && PassiveSkill[17] == 0)NextStageHP(10f); // 폭발적치유와 고동 둘다없을때 -> 10% 회복
                else if (PassiveSkill[13] > 0 && PassiveSkill[17] == 0) NextStageHP(50f); // 폭발적치유만 있을때 -> 50% 회복
                else if (PassiveSkill[13] == 0 && PassiveSkill[17] > 0) NextStageHP(100f); // 고동만 있을때 -> 100% 회복
                else if (PassiveSkill[13] > 0 && PassiveSkill[17] > 0) NextStageHP(100f); // 폭발적치유와 고동 둘다있을때 -> 100% 회복
            }
            if (isPermanentSkill[13] && !isPermanentSkill[17])
            {
                // 영구적인 폭발적치유 O 영구적인 고동 X
                // 고동 유무에 따라서 로직
                if (PassiveSkill[17] == 0) NextStageHP(50f); // 고동 X 폭발적치유만 있을때 -> 50% 회복
                else NextStageHP(100f); // 고동 O 폭발적치유와 고동 둘다있을때 -> 100% 회복
            }
            if (!isPermanentSkill[13] && isPermanentSkill[17])
            {
                // 영구적인 폭발적치유 X 영구적인 고동 O
                // 폭발적치유 유무에 따라서 로직
                if (PassiveSkill[13] == 0) NextStageHP(100f); // 폭발적치유 X 고동만 있을때 -> 100% 회복
                else NextStageHP(100f); // 폭발적치유 O 폭발적치유와 고동 둘다있을때 -> 100% 회복
            }
            if (isPermanentSkill[13] && isPermanentSkill[17]) NextStageHP(100f); // 영구적인 폭발적치유 O 영구적인 고동 O, 폭발적치유 O 고동 O 인로직만 수행, 폭발적치유와 고동 둘다있을때 -> 100% 회복
            SoundManager.instance.BgmSoundPlay(SoundManager.instance.dungeonBgmList[Random.Range(0, SoundManager.instance.dungeonBgmList.Length)]); // 던전 배경음악
        }
        else if (nearObject.tag == "GoToDungeon")
        {
            // 마을에서 던전으로가는 로직
            FadeInOut.Instance.Fade2(); // 페이드 인/아웃
            DelaySpawn(); // 스폰딜레이 : 던전으로 이동시 몬스터가 바로 생성되는문제
            Instantiate(templates.entryRoom, templates.entryRoom.transform.position, Quaternion.identity); // 새로운 EntryRoom 생성
            transform.position = templates.entryRoom.transform.position; // 플레이어를 새로운 EntryRoom위치로 이동
            RepositionPet(spawnedPet); // 펫의 위치 변경
            isShelter = false; // 플레이어 마을아님
            shelter.SetActive(false); // 마을 비활성화
            SoundManager.instance.BgmSoundPlay(SoundManager.instance.dungeonBgmList[Random.Range(0, SoundManager.instance.dungeonBgmList.Length)]); // 던전 배경음악
        }
        else if (nearObject.tag == "Shop")
        {
            // 상점
            shopPanel.SetActive(true); // 상점 패널 활성화
            SoundManager.instance.BgmSoundPlay(SoundManager.instance.bgmList[2]); // 상점 배경음악

            // 인벤토리 패널 위치 변경 : 상점과 인벤토리를 보여주기 위해 첫번째 자식으로
            inventoryPanel.transform.SetParent(shopPanel.transform);
            inventoryPanel.transform.SetAsFirstSibling();

            // 상점 데이터 베이스를 가져온후
            shopDatabase = nearObject.GetComponent<ShopDatabase>();

            // 상점 아이템 초기화
            // UI를 그리기전에
            // 각 타입에 맞는 아이템 이미지 추가
            for (int i = 0; i < shopDatabase.shopItemList.Count; i++)
            {
                if (shopDatabase.shopItemList[i].itemType == ItemType.Ability)
                {
                    if (characterType.Equals("Sword")) // 소드스킬이면
                    {
                        shopDatabase.shopItemList[i] = abilitySwordSkillItem[abilitySkillCnt];
                        abilitySkillCnt++;
                    }
                    else if (characterType.Equals("Mage")) // 마법사 스킬이면
                    {
                        shopDatabase.shopItemList[i] = abilityMageSkillItem[abilitySkillCnt];
                        abilitySkillCnt++;
                    }
                    else if (characterType.Equals("Blacksmith")) // 블랙스미스 스킬이면
                    {
                        shopDatabase.shopItemList[i] = abilityBlacksmithSkillItem[abilitySkillCnt];
                        abilitySkillCnt++;
                    }
                    else if (characterType.Equals("Holyknight")) // 성기사 스킬이면
                    {
                        shopDatabase.shopItemList[i] = abilityHolyknightSkillItem[abilitySkillCnt];
                        abilitySkillCnt++;
                    }
                }

                if (shopDatabase.shopItemList[i].equipmentItemType == EquipmentItemType.Weapon)
                {
                    if (characterType.Equals("Sword") || characterType.Equals("Holyknight")) // 전사나 성기사 무기이면
                    {
                        shopDatabase.shopItemList[i] = swordItem[weaponCnt];
                        weaponCnt++;
                    }
                    else if (characterType.Equals("Mage")) // 마법사 무기이면
                    {
                        shopDatabase.shopItemList[i] = staffItem[weaponCnt];
                        weaponCnt++;
                    }
                    else if (characterType.Equals("Blacksmith")) // 블랙스미스 무기이면
                    {
                        shopDatabase.shopItemList[i] = hammerItem[weaponCnt];
                        weaponCnt++;
                    }
                }
                shopSlots[i].inventoryItem = shopDatabase.shopItemList[i]; // 상점에 아이템 추가
                shopSlots[i].UpdateSlotUI(); // 상점 슬롯에 UI를 그려준다
            }

            isShop = true; // 현재 상점임
            SoundManager.instance.SFXPlay(ObjType.버튼소리); // 사운드
        }
    }   

    // 버티기
    private void HoldOn()
    {
        int random = Random.Range(0, 2); // 0, 1
        if (random == 0)
        {
            curHealth = 1; // 죽기전 50% 확률로 버티며 체력이 1
            StartCoroutine(DoDamaged("doDamaged")); // 죽은게 아니므로 데미지를 입는 애니메이션을 실행

            // 구사일생
            // 영구적인 구사일생이 있는지 체크
            if (!isPermanentSkill[12])
            {
                // 영구적인 구사일생이 없을때
                if (PassiveSkill[12] > 0) curHealth = maxHealth; // 버티기 성공시 HP 모두 회복
            }
            else curHealth = maxHealth; // 영구적인 구사일생이 있을때 버티기 성공시 HP 모두 회복
        }
        else StartCoroutine("DoDie"); // 버티기 실패시 죽음
    }

    // 거울
    private void Mirror(Enemy enemyScript)
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
                    enemyScript.EnemyKillQuestCheck(); // 카운트베이스 퀘스트 처리
                    if (!enemyScript.isDrop) enemyScript.ItemDrop(false); // 아이템 드랍
                    enemyScript.GetBarrier(); // 방벽 얻기
                    poolingManager.ReturnObj(enemyScript.GetComponent<HpBar>().instantHpBar, ObjType.몬스터체력바); // 몬스터 체력바 반납
                    poolingManager.ReturnObj(enemyScript.transform.gameObject, enemyScript.transform.gameObject.GetComponent<Enemy>().type); // 몬스터 반납
                }
                else enemyScript.curHealth -= enemyScript.damage; // 아니라면 정상적으로 데미지 반사
            }
        }
    }

    // 저거너트 ( 근거리 )
    private void Juggernaut(Enemy enemyScript)
    {
        // 영구적인 저거너트가 있는지 체크
        if (!isPermanentSkill[7])
        {
            // 영구적인 저거너트가 없으면
            if (PassiveSkill[7] > 0) curHealth -= enemyScript.damage * 20 / 100; // 저거너트 있으면 받는피해 80% 감소
            else curHealth -= enemyScript.damage; // 받는피해 그대로 적용
        }
        else curHealth -= enemyScript.damage * 20 / 100; // 영구적인 저거너트가 있으면 받는피해 80% 감소
    }

    // 저거너트 ( 원거리 )
    private void Juggernaut(Carrot carrotScript)
    {
        // 영구적인 저거너트가 있는지 체크
        if (!isPermanentSkill[7])
        {
            // 영구적인 저거너트가 없을때
            if (PassiveSkill[7] > 0) curHealth -= (carrotScript.damage + 100 * (templates.currentStage)) * 20 / 100; // 저거너트 있으면 받는피해 80% 감소
            else curHealth -= carrotScript.damage + 100 * (templates.currentStage); // 받는피해 그대로 적용
        }
        else curHealth -= (carrotScript.damage + 100 * (templates.currentStage)) * 20 / 100; // 영구적인 저거너트가 있을때 받는피해 80% 감소
    }
}
