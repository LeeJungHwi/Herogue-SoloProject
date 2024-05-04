using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTemplates : MonoBehaviour
{
    // 방 모델
    // 아래쪽에 방이있는 방
    public GameObject[] bottomRooms;

    // 위쪽에 방이있는 방
    public GameObject[] topRooms;

    // 왼쪽에 방이있는 방
    public GameObject[] leftRooms;

    // 오른쪽에 방이있는 방
    public GameObject[] rightRooms;

    // 스폰할 몬스터 리스트
    [SerializeField] private GameObject[] MonsterList;

    // 생성할 던전 장식 리스트
    public GameObject[] DungeonDecorationList;

    // 비밀방
    //public GameObject secretRoom;

    // 시작방
    public GameObject entryRoom;

    // 플레이어
    public GameObject player;

    // 다음스테이지 문
    [SerializeField] private GameObject nextStage;

    // 마을에서 던전으로 가는 포털
    [SerializeField] private GameObject goToDungeon;

    // 스테이지에 생성된 방이 추가되는 리스트
    public List<Tuple<GameObject, ObjType>> rooms = new List<Tuple<GameObject, ObjType>>();

    // 스테이지에 생성된 몬스터가 추가되는 리스트
    private List<GameObject> monsters = new List<GameObject>();

    // 스테이지에 생성된 시크릿박스가 추가되는 리스트
    [HideInInspector] public List<GameObject> secretBoxes;

    // 스테이지에 생성된 보스가 추가되는 리스트
    private List<GameObject> boss = new List<GameObject>();

    // 스테이지에 생성된 아이템이 저장되는 리스트
    [HideInInspector] public List<GameObject> items;

    // 스테이지에 생성된 체력바가 저장되는 리스트
    [HideInInspector] public List<GameObject> hpBars;

    // 현재 스테이지
    [HideInInspector] public int currentStage;

    // 대기시간
    public float waitTime;

    // 보스가 스폰되었는지 체크
    [HideInInspector] public bool spawnedBoss;

    // 오브젝트 풀링
    [SerializeField] private PoolingManager poolingManager;

    // 플레이어 스크립트
    public Player playerScript;

    // RoomBFS
    [SerializeField] private RoomBFS roomBFS;

    // 기본스테이지
    public int baseStage = 5;
    
    // 방 계수
    public int stageCoef = 3;

    // 기본몬스터 수
    [SerializeField] private int baseMonster = 10;

    // 몬스터 수 계수
    [SerializeField] private int monsterCoef = 6;
    
    // 플레이어 할당
    private void Awake() { Invoke("SetPlayer", 0.5f); }

    private void Update()
    {
        if(!playerScript.isShelter)
        {
            // 마을이 아닐때만
            if (waitTime <= 0 && spawnedBoss == false)
            {
                // 대기시간이 0초이하면서 보스가 스폰되지 않았을때
                // 보스 생성
                // 풀링에서 먼저 트랜스폼 초기화한후 활성화
                GameObject instantBoss = poolingManager.GetObj(ObjType.보스1);

                // 보스리스트에 저장
                boss.Add(instantBoss);

                // 보스가 생성됨
                spawnedBoss = true;
            }
            else
            {
                // 대기시간을 줄임
                waitTime -= Time.deltaTime;
            }

            // 대기시간이 0이하이고 보스가 생성되었을때
            // 그때부터 생성하는데 몬스터의 수를 제한
            // 기본몬스터 수 + (현재스테이지 / 스테이지계수) * 몬스터 수 계수
            // 0~2 스테이지는 10개까지, 3~5 스테이지는 16개까지, 6~8 스테이지는 22개까지, ...
            if (waitTime <= 0 && monsters.Count < baseMonster + currentStage / stageCoef * monsterCoef && spawnedBoss == true) MonsterSpawn();
            else waitTime -= Time.deltaTime;
        }
    }

    private void MonsterSpawn()
    {
        // 몬스터 생성
        // 풀링에서 시작다음방~보스전방 랜덤돌려서 몬스터위치를 잡아준뒤 활성화
        // 몬스터리스트에서 하나를 랜덤 선택해서 50% 확률로 스폰
        int randomMonster = UnityEngine.Random.Range(0, MonsterList.Length); // 0~16
        int spawnRandom = UnityEngine.Random.Range(0, 2); // 0~1
        if (spawnRandom == 0)
        {
            GameObject instantMonster = poolingManager.GetObj(MonsterList[randomMonster].GetComponent<Enemy>().type);
            monsters.Add(instantMonster);
            instantMonster.GetComponent<HpBar>().spawnHpBar = false;
        }
    }

    public void NextStage()
    {
        // 다음 스테이지 로직
        // 리스트 초기화

        // 풀에 반환
        for (int i = 0; i < hpBars.Count; i++) poolingManager.ReturnObj(hpBars[i], ObjType.몬스터체력바);

        // 리스트 클리어
        hpBars.Clear();

        // 풀에 반환
        for (int i = 0; i < rooms.Count; i++) poolingManager.ReturnObj(rooms[i].Item1, rooms[i].Item2);

        // 리스트 클리어
        rooms.Clear();

        // 풀에 반환
        for (int i = 0; i < monsters.Count; i++) poolingManager.ReturnObj(monsters[i], monsters[i].GetComponent<Enemy>().type);

        // 리스트 클리어
        monsters.Clear();

        // 풀에 반환
        for (int i = 0; i < secretBoxes.Count; i++) poolingManager.ReturnObj(secretBoxes[i], secretBoxes[i].GetComponent<SecretBox>().type);

        // 리스트 클리어
        secretBoxes.Clear();

        // 풀에 반환
        for (int i = 0; i < items.Count; i++) poolingManager.ReturnObj(items[i], items[i].GetComponent<Item>().objType);

        // 리스트 클리어
        items.Clear();

        // 풀에 반환
        for (int i = 0; i < boss.Count; i++) poolingManager.ReturnObj(boss[i], ObjType.보스1);

        // 리스트 클리어
        boss.Clear();

        // 그래프, 거리, 최대거리, BFS체크
        roomBFS.InitForNextBFS();

        // 스폰딜레이
        DelaySpawn();

        // 다음스테이지 문 위로 올리기
        nextStage.transform.position = new Vector3(0, 100f, 0);

        // 새로운 EntryRoom 생성
        Instantiate(entryRoom, entryRoom.transform.position, Quaternion.identity);

        // 플레이어를 새로운 EntryRoom위치로 이동
        player.transform.position = entryRoom.transform.position;

        // 펫의 위치 변경
        playerScript.RepositionPet(playerScript.spawnedPet);

        // currentStage 증가
        currentStage++;
    }

    public void GotoShelter()
    {
        // 던전에서 마을로가는 로직
        // 플레이어가 던전에서 죽으면 실행되어야함
        // 플레이어가 죽는 함수에서 호출
        // 리스트 초기화

        // 풀에 반환
        for (int i = 0; i < hpBars.Count; i++) poolingManager.ReturnObj(hpBars[i], ObjType.몬스터체력바);

        // 리스트 클리어
        hpBars.Clear();

        // 풀에 반환
        for (int i = 0; i < rooms.Count; i++) poolingManager.ReturnObj(rooms[i].Item1, rooms[i].Item2);

        // 리스트 클리어
        rooms.Clear();

        // 풀에 반환
        for (int i = 0; i < monsters.Count; i++) poolingManager.ReturnObj(monsters[i], monsters[i].GetComponent<Enemy>().type);

        // 리스트 클리어
        monsters.Clear();

        // 풀에 반환
        for (int i = 0; i < secretBoxes.Count; i++) poolingManager.ReturnObj(secretBoxes[i], secretBoxes[i].GetComponent<SecretBox>().type);

        // 리스트 클리어
        secretBoxes.Clear();

        // 풀에 반환
        for (int i = 0; i < items.Count; i++) poolingManager.ReturnObj(items[i], items[i].GetComponent<Item>().objType);

        // 리스트 클리어
        items.Clear();

        // 풀에 반환
        for (int i = 0; i < boss.Count; i++) poolingManager.ReturnObj(boss[i], ObjType.보스1);

        // 리스트 클리어
        boss.Clear();

        // 그래프, 거리, 최대거리, BFS체크
        roomBFS.InitForNextBFS();

        // 스테이지 초기화
        currentStage = 0;

        // 플레이어 스탯 초기화
        // 던전에서 마을로 올때 플레이어 스탯이 해제된 장비에 영향을 받는 문제
        // 현재 장착된 장비 플래그에 따라서 던전에서 마을로 돌아올때 플레이어의 스탯 초기화
        // 장착되어있는 장비가 있을때 : 기본 스탯 + 현재 장착되어있는 장비의 스탯
        // 장착되어있는 장비가 없을때 : 기본 스탯
        // 영구적인 패시브가 있는지 체크
        
        // 영구적인 한발노리기 X 영구적인 백발백중 X
        if(!playerScript.isPermanentSkill[0] && !playerScript.isPermanentSkill[1])
        {
            // 원래 로직 구현
            playerScript.criticalPercentage = playerScript.isAmulet ? 25 + playerScript.equipedAmuletItem.criticalPercentage : 25;
            playerScript.criticalDamage = playerScript.isAmulet ? 200f + playerScript.equipedAmuletItem.criticalDamage : 200f;
            playerScript.accuracy = 75f;
        }

        // 영구적인 한발노리기 X 영구적인 백발백중 O
        if (!playerScript.isPermanentSkill[0] && playerScript.isPermanentSkill[1])
        {
            // 백발백중 로직 구현
            playerScript.criticalPercentage = playerScript.isAmulet ? 25 + playerScript.equipedAmuletItem.criticalPercentage : 25;
            playerScript.criticalDamage = playerScript.isAmulet ? 150f + playerScript.equipedAmuletItem.criticalDamage : 150f;
            playerScript.accuracy = 100f;
        }

        // 영구적인 한발노리기 O 영구적인 백발백중 X
        if (playerScript.isPermanentSkill[0] && !playerScript.isPermanentSkill[1])
        {
            // 한발노리기 로직 구현
            playerScript.criticalPercentage = 100;
            playerScript.criticalDamage = playerScript.isAmulet ? 200f + playerScript.equipedAmuletItem.criticalDamage : 200f;
            playerScript.accuracy = 50f;
        }

        // 영구적인 한발노리기 O 영구적인 백발백중 O
        if (playerScript.isPermanentSkill[0] && playerScript.isPermanentSkill[1])
        {
            // 둘다 있는 경우
            playerScript.criticalPercentage = 100;
            playerScript.criticalDamage = playerScript.isAmulet ? 150f + playerScript.equipedAmuletItem.criticalDamage : 150f;
            playerScript.accuracy = 100f;
        }

        // 영구적인 HP부스트 X 영구적인 초월방벽 X
        if(!playerScript.isPermanentSkill[14] && !playerScript.isPermanentSkill[3])
        {
            // 원래 로직 구현
            playerScript.barrier = 0;
            playerScript.maxHealth = playerScript.isArmor ? 2000f + playerScript.equipedArmorItem.health : 2000f;
            playerScript.curHealth = playerScript.isArmor ? 2000f + playerScript.equipedArmorItem.health : 2000f;
        }

        // 영구적인 HP부스트 X 영구적인 초월방벽 O
        if (!playerScript.isPermanentSkill[14] && playerScript.isPermanentSkill[3])
        {
            // 초월방벽 로직 구현
            // 방벽 20개
            playerScript.barrier = 20;

            // 체력 1
            playerScript.maxHealth = playerScript.isArmor ? 1f + playerScript.equipedArmorItem.health : 1f;
            playerScript.curHealth = playerScript.isArmor ? 1f + playerScript.equipedArmorItem.health : 1f;
        }

        // 영구적인 HP부스트 O 영구적인 초월방벽 X
        if (playerScript.isPermanentSkill[14] && !playerScript.isPermanentSkill[3])
        {
            // HP부스트 로직 구현
            playerScript.maxHealth = playerScript.isArmor ? (2000f + playerScript.equipedArmorItem.health) * 2 : 4000f;
            playerScript.curHealth = playerScript.isArmor ? (2000f + playerScript.equipedArmorItem.health) * 2 : 4000f;
        }

        // 영구적인 HP부스트 O 영구적인 초월방벽 O
        if (playerScript.isPermanentSkill[14] && playerScript.isPermanentSkill[3])
        {
            // 둘다 있는 경우
            playerScript.barrier = 20;
            playerScript.maxHealth = playerScript.isArmor ? (1f + playerScript.equipedArmorItem.health) * 2 : 2f;
            playerScript.curHealth = playerScript.isArmor ? (1f + playerScript.equipedArmorItem.health) * 2 : 2f;
        }

        // 영구적인 흡혈귀가 있는지 체크
        playerScript.bloodDrain = playerScript.isPermanentSkill[4] ? 20f : 0f;

        // 영구적인 도박꾼이 있는지 체크
        playerScript.passiveDropPercentage = playerScript.isPermanentSkill[6] ? 15f : 5f;
        playerScript.activeDropPercentage = playerScript.isPermanentSkill[6] ? 13f : 3f;
        playerScript.inventoryItemPercentage = playerScript.isPermanentSkill[6] ? 15f : 5f;

        // 영구적인 시크릿이 있는지 체크
        playerScript.secretPercentage = playerScript.isPermanentSkill[9] ? 20f : 10f;

        // 패시브스킬에 영향을 받지않는 스탯
        playerScript.damage = playerScript.isWeapon ? 250f + playerScript.equipedWeaponItem.attack : 250f;
        playerScript.attackSpeed = playerScript.isGlove ? 0.5f + playerScript.equipedGloveItem.attackSpeed : 0.5f;
        playerScript.joystickScript.moveSpeed = playerScript.isShoes ? 40f + playerScript.equipedShoesItem.moveSpeed : 40f;
        playerScript.isDead = false;

        // 플레이어 스킬 초기화
        for(int i = 0; i < playerScript.ActiveSkill.Count; i++) playerScript.ActiveSkill[i] = 0;
        for(int i = 0; i < playerScript.PassiveSkill.Count; i++) playerScript.PassiveSkill[i] = 0;

        // 마을 활성화
        playerScript.shelter.SetActive(true);

        // 플레이어 위치를 마을로
        player.transform.position = goToDungeon.transform.position + transform.forward * 20f;

        // 펫의 위치 변경
        playerScript.RepositionPet(playerScript.spawnedPet);

        // 플레이어 죽는 코루틴 멈추기
        playerScript.StopCoroutine("DoDie");

        // 플레이어 마을임
        playerScript.isShelter = true;
    }

    private void DelaySpawn()
    {
        // 스폰딜레이
        waitTime = 4f;
        spawnedBoss = false;
    }

    private void SetPlayer()
    {
        // 플레이어를 할당하는 함수
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<Player>();
    }
}
