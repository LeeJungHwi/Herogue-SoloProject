using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolingManager : MonoBehaviour
{
    // 방 모델
    public RoomTemplates templates;

    // 방내에서 몬스터 랜덤배치를 위한 벡터
    public Vector3[] MonsterVec;

    // 캔버스
    public GameObject Canvas;

    // 텍스트 표시
    public GameObject FloatingText;

    // AbilityArrow1Hit를 저장 할 리스트
    public List<GameObject> AbilityArrow1HitEffects;

    // AbilityArrow2Hit를 저장 할 리스트
    public List<GameObject> AbilityArrow2HitEffects;

    // AbilityMage1Hit과 AbilityMage1Hit2를 저장 할 리스트
    public List<GameObject> AbilityMage1HitEffects;

    // AbilityHolyknight1Hit을 저장 할 리스트
    public List<GameObject> AbilityHolyknight1HitEffects;

    // AbilityArrow0Hit을 저장 할 리스트
    public List<GameObject> AbilityArrow0HitEffects;

    // 프리팹
    // 무기
    public GameObject ArrowPrefab;
    public GameObject CarrotPrefab;
    public GameObject CarrotGroubPrefab;
    public GameObject CiclopEyePrefab;
    public GameObject MageMissilePrefab;

    // 패시브 스킬
    public GameObject PassiveSkill0Prefab;
    public GameObject PassiveSkill1Prefab;
    public GameObject PassiveSkill2Prefab;
    public GameObject PassiveSkill3Prefab;
    public GameObject PassiveSkill4Prefab;
    public GameObject PassiveSkill5Prefab;
    public GameObject PassiveSkill6Prefab;
    public GameObject PassiveSkill7Prefab;
    public GameObject PassiveSkill8Prefab;
    public GameObject PassiveSkill9Prefab;
    public GameObject PassiveSkill10Prefab;
    public GameObject PassiveSkill11Prefab;
    public GameObject PassiveSkill12Prefab;
    public GameObject PassiveSkill13Prefab;
    public GameObject PassiveSkill14Prefab;
    public GameObject PassiveSkill15Prefab;
    public GameObject PassiveSkill16Prefab;
    public GameObject PassiveSkill17Prefab;
    public GameObject PassiveSkill18Prefab;
    public GameObject PassiveSkill19Prefab;
    public GameObject PassiveSkill20Prefab;
    public GameObject PassiveSkill21Prefab;

    // 액티브 스킬
    public GameObject ActiveSkill0Prefab;
    public GameObject ActiveSkill1Prefab;
    
    // 코인
    public GameObject Coin100Prefab;
    public GameObject Coin500Prefab;
    public GameObject Coin1000Prefab;

    // 시크릿 박스
    public GameObject SecretBoxPrefab;

    // 몬스터
    public GameObject Boss0Prefab;
    public GameObject Boss1Prefab;
    public GameObject Boss2Prefab;
    public GameObject Normal0Prefab;
    public GameObject Normal1Prefab;
    public GameObject Normal2Prefab;
    public GameObject Normal3Prefab;
    public GameObject Normal4Prefab;
    public GameObject Normal5Prefab;
    public GameObject Normal6Prefab;
    public GameObject Normal7Prefab;
    public GameObject Normal8Prefab;
    public GameObject Normal9Prefab;
    public GameObject Normal10Prefab;
    public GameObject Normal11Prefab;
    public GameObject Normal12Prefab;
    public GameObject Normal13Prefab;
    public GameObject Normal14Prefab;
    public GameObject Normal15Prefab;
    public GameObject Normal16Prefab;

    // 랜덤맵 모델
    public GameObject BPrefab;
    public GameObject BLPrefab;
    public GameObject LBPrefab;
    public GameObject BLTPrefab;
    public GameObject LTBPrefab;
    public GameObject TBLPrefab;
    public GameObject LPrefab;
    public GameObject LRPrefab;
    public GameObject RLPrefab;
    public GameObject LTRPrefab;
    public GameObject TRLPrefab;
    public GameObject RLTPrefab;
    public GameObject RPrefab;
    public GameObject RBPrefab;
    public GameObject BRPrefab;
    public GameObject RBLPrefab;
    public GameObject BLRPrefab;
    public GameObject LRBPrefab;
    public GameObject SecretRoomPrefab;
    public GameObject TPrefab;
    public GameObject TBPrefab;
    public GameObject BTPrefab;
    public GameObject TLPrefab;
    public GameObject LTPrefab;
    public GameObject TRPrefab;
    public GameObject RTPrefab;
    public GameObject TRBPrefab;
    public GameObject RBTPrefab;
    public GameObject BTRPrefab;

    // 던전 장식
    public GameObject DungeonDecoration1Prefab;
    public GameObject DungeonDecoration2Prefab;
    public GameObject DungeonDecoration3Prefab;
    public GameObject DungeonDecoration4Prefab;
    public GameObject DungeonDecoration5Prefab;
    public GameObject DungeonDecoration6Prefab;
    public GameObject DungeonDecoration7Prefab;
    public GameObject DungeonDecoration8Prefab;
    public GameObject DungeonDecoration9Prefab;
    public GameObject DungeonDecoration10Prefab;
    public GameObject DungeonDecoration11Prefab;
    public GameObject DungeonDecoration12Prefab;
    public GameObject DungeonDecoration13Prefab;
    public GameObject DungeonDecoration14Prefab;
    public GameObject DungeonDecoration15Prefab;
    public GameObject DungeonDecoration16Prefab;
    public GameObject DungeonDecoration17Prefab;
    public GameObject DungeonDecoration18Prefab;
    public GameObject DungeonDecoration19Prefab;
    public GameObject DungeonDecoration20Prefab;
    public GameObject DungeonDecoration21Prefab;
    public GameObject DungeonDecoration22Prefab;
    public GameObject DungeonDecoration23Prefab;
    public GameObject DungeonDecoration24Prefab;
    public GameObject DungeonDecoration25Prefab;
    public GameObject DungeonDecoration26Prefab;
    public GameObject DungeonDecoration27Prefab;
    public GameObject DungeonDecoration28Prefab;
    public GameObject DungeonDecoration29Prefab;

    // 이펙트
    public GameObject PlayerAtkEffectPrefab;
    public GameObject PlayerCriticalAtkEffectPrefab;
    public GameObject AbilityArrow0Prefab;
    public GameObject AbilityArrow0HitPrefab;
    public GameObject AbilityArrow1ActivePrefab;
    public GameObject AbilityArrow1HitPrefab;
    public GameObject AbilityArrow2ActivePrefab;
    public GameObject AbilityArrow2HitPrefab;
    public GameObject AbilitySword0Prefab;
    public GameObject AbilitySword1Prefab;
    public GameObject AbilitySword2Prefab;
    public GameObject AbilityMage0Prefab;
    public GameObject AbilityMage1ActivePrefab;
    public GameObject AbilityMage1HitPrefab;
    public GameObject AbilityMage1Hit2Prefab;
    public GameObject AbilityMage2Prefab;
    public GameObject AbilityBlacksmith0Prefab;
    public GameObject AbilityBlacksmith1Prefab;
    public GameObject AbilityBlacksmith2Prefab;
    public GameObject SwordComboAttack0Prefab;
    public GameObject SwordComboAttack1Prefab;
    public GameObject SwordComboAttack2Prefab;
    public GameObject BlacksmithComboAttack0Prefab;
    public GameObject BlacksmithComboAttack1Prefab;
    public GameObject BlacksmithComboAttack2Prefab;
    public GameObject AbilityHolyknight0Prefab;
    public GameObject AbilityHolyknight1ActivePrefab;
    public GameObject AbilityHolyknight1HitPrefab;
    public GameObject AbilityHolyknight2Prefab;
    public GameObject HolyknightComboAttack0Prefab;
    public GameObject HolyknightComboAttack1Prefab;
    public GameObject HolyknightComboAttack2Prefab;

    // 체력바
    public GameObject MonsterHpBarPrefab;

    // 텍스트 표시
    public GameObject DamageTextPrefab;
    public GameObject HealingTextPrefab;
    public GameObject MissTextPrefab;

    // 사운드
    public GameObject ShootSoundPrefab;
    public GameObject MoveSoundPrefab;
    public GameObject DashSoundPrefab;
    public GameObject ItemSoundPrefab;
    public GameObject BasicAttackSoundPrefab;
    public GameObject CriticalAttackSoundPrefab;
    public GameObject BarrierSoundPrefab;
    public GameObject BoxSoundPrefab;
    public GameObject SecretDoorSoundPrefab;
    public GameObject ArrowSkill02SoundPrefab;
    public GameObject ArrowSkill0HitSoundPrefab;
    public GameObject ArrowSkill1SoundPrefab;
    public GameObject ArrowSkill2HitSoundPrefab;
    public GameObject PlayerDamagedSoundPrefab;
    public GameObject ButtonSoundPrefab;
    public GameObject UsePotionSoundPrefab;
    public GameObject EquipSoundPrefab;
    public GameObject UnEquipSoundPrefab;
    public GameObject FailEquipSoundPrefab;
    public GameObject SwordSoundPrefab;
    public GameObject SwordSkill0SoundPrefab;
    public GameObject SwordSkill1SoundPrefab;
    public GameObject SwordSkill2SoundPrefab;
    public GameObject SwordSkill2HitSoundPrefab;
    public GameObject MageSkill0SoundPrefab;
    public GameObject MageSkill1SoundPrefab;
    public GameObject MageSkill2SoundPrefab;
    public GameObject MageMissileSoundPrefab;
    public GameObject BlacksmithSkill0SoundPrefab;
    public GameObject BlacksmithSkill1SoundPrefab;
    public GameObject BlacksmithSkill1HitSoundPrefab;
    public GameObject BlacksmithSkill2SoundPrefab;
    public GameObject HammerSoundPrefab;
    public GameObject HolyknightSkill0SoundPrefab;
    public GameObject HolyknightSkill1SoundPrefab;
    public GameObject HolyknightSkill2SoundPrefab;
    
    // 인벤토리 아이템
    public GameObject inventoryItem0Prefab;
    public GameObject inventoryItem1Prefab;
    public GameObject inventoryItem2Prefab;
    public GameObject inventoryItem3Prefab;
    public GameObject inventoryItem4Prefab;
    public GameObject inventoryItem5Prefab;
    public GameObject inventoryItem6Prefab;
    public GameObject inventoryItem7Prefab;
    public GameObject inventoryItem8Prefab;
    public GameObject inventoryItem9Prefab;
    public GameObject inventoryItem10Prefab;
    public GameObject inventoryItem11Prefab;
    public GameObject inventoryItem12Prefab;
    public GameObject inventoryItem13Prefab;
    public GameObject inventoryItem14Prefab;
    public GameObject inventoryItem15Prefab;
    public GameObject inventoryItem16Prefab;
    public GameObject inventoryItem17Prefab;
    public GameObject inventoryItem18Prefab;

    // 펫
    public GameObject CatPrefab;
    public GameObject DuckPrefab;
    public GameObject PenguinPrefab;
    public GameObject SheepPrefab;

    // 프리팹을 저장할 배열
    // 무기
    GameObject[] Arrow;
    GameObject[] Carrot;
    GameObject[] CarrotGroup;
    GameObject[] CiclopEye;
    GameObject[] MageMissile;

    // 패시브 스킬
    GameObject[] PassiveSkill0;
    GameObject[] PassiveSkill1;
    GameObject[] PassiveSkill2;
    GameObject[] PassiveSkill3;
    GameObject[] PassiveSkill4;
    GameObject[] PassiveSkill5;
    GameObject[] PassiveSkill6;
    GameObject[] PassiveSkill7;
    GameObject[] PassiveSkill8;
    GameObject[] PassiveSkill9;
    GameObject[] PassiveSkill10;
    GameObject[] PassiveSkill11;
    GameObject[] PassiveSkill12;
    GameObject[] PassiveSkill13;
    GameObject[] PassiveSkill14;
    GameObject[] PassiveSkill15;
    GameObject[] PassiveSkill16;
    GameObject[] PassiveSkill17;
    GameObject[] PassiveSkill18;
    GameObject[] PassiveSkill19;
    GameObject[] PassiveSkill20;
    GameObject[] PassiveSkill21;

    // 액티브 스킬
    GameObject[] ActiveSkill0;
    GameObject[] ActiveSkill1;

    // 코인
    GameObject[] Coin100;
    GameObject[] Coin500;
    GameObject[] Coin1000;

    // 시크릿박스
    GameObject[] SecretBox;

    // 몬스터
    GameObject[] Boss0;
    GameObject[] Boss1;
    GameObject[] Boss2;
    GameObject[] Normal0;
    GameObject[] Normal1;
    GameObject[] Normal2;
    GameObject[] Normal3;
    GameObject[] Normal4;
    GameObject[] Normal5;
    GameObject[] Normal6;
    GameObject[] Normal7;
    GameObject[] Normal8;
    GameObject[] Normal9;
    GameObject[] Normal10;
    GameObject[] Normal11;
    GameObject[] Normal12;
    GameObject[] Normal13;
    GameObject[] Normal14;
    GameObject[] Normal15;
    GameObject[] Normal16;

    // 랜덤맵 모델
    GameObject[] B;
    GameObject[] BL;
    GameObject[] LB;
    GameObject[] BLT;
    GameObject[] LTB;
    GameObject[] TBL;
    GameObject[] L;
    GameObject[] LR;
    GameObject[] RL;
    GameObject[] LTR;
    GameObject[] TRL;
    GameObject[] RLT;
    GameObject[] R;
    GameObject[] RB;
    GameObject[] BR;
    GameObject[] RBL;
    GameObject[] BLR;
    GameObject[] LRB;
    GameObject[] SecretRoom;
    GameObject[] T;
    GameObject[] TB;
    GameObject[] BT;
    GameObject[] TL;
    GameObject[] LT;
    GameObject[] TR;
    GameObject[] RT;
    GameObject[] TRB;
    GameObject[] RBT;
    GameObject[] BTR;

    // 던전 장식
    GameObject[] DungeonDecoration1;
    GameObject[] DungeonDecoration2;
    GameObject[] DungeonDecoration3;
    GameObject[] DungeonDecoration4;
    GameObject[] DungeonDecoration5;
    GameObject[] DungeonDecoration6;
    GameObject[] DungeonDecoration7;
    GameObject[] DungeonDecoration8;
    GameObject[] DungeonDecoration9;
    GameObject[] DungeonDecoration10;
    GameObject[] DungeonDecoration11;
    GameObject[] DungeonDecoration12;
    GameObject[] DungeonDecoration13;
    GameObject[] DungeonDecoration14;
    GameObject[] DungeonDecoration15;
    GameObject[] DungeonDecoration16;
    GameObject[] DungeonDecoration17;
    GameObject[] DungeonDecoration18;
    GameObject[] DungeonDecoration19;
    GameObject[] DungeonDecoration20;
    GameObject[] DungeonDecoration21;
    GameObject[] DungeonDecoration22;
    GameObject[] DungeonDecoration23;
    GameObject[] DungeonDecoration24;
    GameObject[] DungeonDecoration25;
    GameObject[] DungeonDecoration26;
    GameObject[] DungeonDecoration27;
    GameObject[] DungeonDecoration28;
    GameObject[] DungeonDecoration29;

    // 이펙트
    GameObject[] PlayerAtkEffect;
    GameObject[] PlayerCriticalAtkEffect;
    GameObject[] AbilityArrow0;
    GameObject[] AbilityArrow0Hit;
    GameObject[] AbilityArrow1Active;
    GameObject[] AbilityArrow1Hit;
    GameObject[] AbilityArrow2Active;
    GameObject[] AbilityArrow2Hit;
    GameObject[] AbilitySword0;
    GameObject[] AbilitySword1;
    GameObject[] AbilitySword2;
    GameObject[] AbilityMage0;
    GameObject[] AbilityMage1Active;
    GameObject[] AbilityMage1Hit;
    GameObject[] AbilityMage1Hit2;
    GameObject[] AbilityMage2;
    GameObject[] AbilityBlacksmith0;
    GameObject[] AbilityBlacksmith1;
    GameObject[] AbilityBlacksmith2;
    GameObject[] SwordComboAttack0;
    GameObject[] SwordComboAttack1;
    GameObject[] SwordComboAttack2;
    GameObject[] BlacksmithComboAttack0;
    GameObject[] BlacksmithComboAttack1;
    GameObject[] BlacksmithComboAttack2;
    GameObject[] AbilityHolyknight0;
    GameObject[] AbilityHolyknight1Active;
    GameObject[] AbilityHolyknight1Hit;
    GameObject[] AbilityHolyknight2;
    GameObject[] HolyknightComboAttack0;
    GameObject[] HolyknightComboAttack1;
    GameObject[] HolyknightComboAttack2;

    // 체력바
    GameObject[] MonsterHpBar;

    // 텍스트 표시
    GameObject[] DamageText;
    GameObject[] HealingText;
    GameObject[] MissText;

    // 사운드
    GameObject[] ShootSound;
    GameObject[] MoveSound;
    GameObject[] DashSound;
    GameObject[] ItemSound;
    GameObject[] BasicAttackSound;
    GameObject[] CriticalAttackSound;
    GameObject[] BarrierSound;
    GameObject[] BoxSound;
    GameObject[] SecretDoorSound;
    GameObject[] ArrowSkill02Sound;
    GameObject[] ArrowSkill0HitSound;
    GameObject[] ArrowSkill1Sound;
    GameObject[] ArrowSkill2HitSound;
    GameObject[] PlayerDamagedSound;
    GameObject[] ButtonSound;
    GameObject[] UsePotionSound;
    GameObject[] EquipSound;
    GameObject[] UnEquipSound;
    GameObject[] FailEquipSound;
    GameObject[] SwordSound;
    GameObject[] SwordSkill0Sound;
    GameObject[] SwordSkill1Sound;
    GameObject[] SwordSkill2Sound;
    GameObject[] SwordSkill2HitSound;
    GameObject[] MageSkill0Sound;
    GameObject[] MageSkill1Sound;
    GameObject[] MageSkill2Sound;
    GameObject[] MageMissileSound;
    GameObject[] BlacksmithSkill0Sound;
    GameObject[] BlacksmithSkill1Sound;
    GameObject[] BlacksmithSkill1HitSound;
    GameObject[] BlacksmithSkill2Sound;
    GameObject[] HammerSound;
    GameObject[] HolyknightSkill0Sound;
    GameObject[] HolyknightSkill1Sound;
    GameObject[] HolyknightSkill2Sound;


    // 인벤토리 아이템
    GameObject[] inventoryItem0;
    GameObject[] inventoryItem1;
    GameObject[] inventoryItem2;
    GameObject[] inventoryItem3;
    GameObject[] inventoryItem4;
    GameObject[] inventoryItem5;
    GameObject[] inventoryItem6;
    GameObject[] inventoryItem7;
    GameObject[] inventoryItem8;
    GameObject[] inventoryItem9;
    GameObject[] inventoryItem10;
    GameObject[] inventoryItem11;
    GameObject[] inventoryItem12;
    GameObject[] inventoryItem13;
    GameObject[] inventoryItem14;
    GameObject[] inventoryItem15;
    GameObject[] inventoryItem16;
    GameObject[] inventoryItem17;
    GameObject[] inventoryItem18;

    // 펫
    GameObject[] Cat;
    GameObject[] Duck;
    GameObject[] Penguin;
    GameObject[] Sheep;

    // 생성할 프리팹 종류
    GameObject[] targetPool;

    void Awake()
    {
        // 프리팹을 저장할 배열 할당
        // 무기
        Arrow = new GameObject[100];
        Carrot = new GameObject[100];
        CarrotGroup = new GameObject[100];
        CiclopEye = new GameObject[100];
        MageMissile = new GameObject[100];

        // 패시브 스킬
        PassiveSkill0 = new GameObject[5];
        PassiveSkill1 = new GameObject[5];
        PassiveSkill2 = new GameObject[5];
        PassiveSkill3 = new GameObject[5];
        PassiveSkill4 = new GameObject[5];
        PassiveSkill5 = new GameObject[5];
        PassiveSkill6 = new GameObject[5];
        PassiveSkill7 = new GameObject[5];
        PassiveSkill8 = new GameObject[5];
        PassiveSkill9 = new GameObject[5];
        PassiveSkill10 = new GameObject[5];
        PassiveSkill11 = new GameObject[5];
        PassiveSkill12 = new GameObject[5];
        PassiveSkill13 = new GameObject[5];
        PassiveSkill14 = new GameObject[5];
        PassiveSkill15 = new GameObject[5];
        PassiveSkill16 = new GameObject[5];
        PassiveSkill17 = new GameObject[5];
        PassiveSkill18 = new GameObject[5];
        PassiveSkill19 = new GameObject[5];
        PassiveSkill20 = new GameObject[5];
        PassiveSkill21 = new GameObject[5];

        // 액티브 스킬
        ActiveSkill0 = new GameObject[5];
        ActiveSkill1 = new GameObject[5];

        // 코인
        Coin100 = new GameObject[10];
        Coin500 = new GameObject[10];
        Coin1000 = new GameObject[10];

        // 시크릿박스
        SecretBox = new GameObject[100];

        // 몬스터
        Boss0 = new GameObject[20];
        Boss1 = new GameObject[20];
        Boss2 = new GameObject[20];
        Normal0 = new GameObject[100];
        Normal1 = new GameObject[100];
        Normal2 = new GameObject[100];
        Normal3 = new GameObject[100];
        Normal4 = new GameObject[100];
        Normal5 = new GameObject[100];
        Normal6 = new GameObject[100];
        Normal7 = new GameObject[100];
        Normal8 = new GameObject[100];
        Normal9 = new GameObject[100];
        Normal10 = new GameObject[100];
        Normal11 = new GameObject[100];
        Normal12 = new GameObject[100];
        Normal13 = new GameObject[100];
        Normal14 = new GameObject[100];
        Normal15 = new GameObject[100];
        Normal16 = new GameObject[100];

        // 랜덤맵 모델
        B = new GameObject[100];
        BL = new GameObject[100];
        LB = new GameObject[100];
        BLT = new GameObject[100];
        LTB = new GameObject[100];
        TBL = new GameObject[100];
        L = new GameObject[100];
        LR = new GameObject[100];
        RL = new GameObject[100];
        LTR = new GameObject[100];
        TRL = new GameObject[100];
        RLT = new GameObject[100];
        R = new GameObject[100];
        RB = new GameObject[100];
        BR = new GameObject[100];
        RBL = new GameObject[100];
        BLR = new GameObject[100];
        LRB = new GameObject[100];
        SecretRoom = new GameObject[100];
        T = new GameObject[100];
        TB = new GameObject[100];
        BT = new GameObject[100];
        TL = new GameObject[100];
        LT = new GameObject[100];
        TR = new GameObject[100];
        RT = new GameObject[100];
        TRB = new GameObject[100];
        RBT = new GameObject[100];
        BTR = new GameObject[100];

        // 던전 장식
        DungeonDecoration1 = new GameObject[100];
        DungeonDecoration2 = new GameObject[100];
        DungeonDecoration3 = new GameObject[100];
        DungeonDecoration4 = new GameObject[100];
        DungeonDecoration5 = new GameObject[100];
        DungeonDecoration6 = new GameObject[100];
        DungeonDecoration7 = new GameObject[100];
        DungeonDecoration8 = new GameObject[100];
        DungeonDecoration9 = new GameObject[100];
        DungeonDecoration10 = new GameObject[100];
        DungeonDecoration11 = new GameObject[100];
        DungeonDecoration12 = new GameObject[100];
        DungeonDecoration13 = new GameObject[100];
        DungeonDecoration14 = new GameObject[100];
        DungeonDecoration15 = new GameObject[100];
        DungeonDecoration16 = new GameObject[100];
        DungeonDecoration17 = new GameObject[100];
        DungeonDecoration18 = new GameObject[100];
        DungeonDecoration19 = new GameObject[100];
        DungeonDecoration20 = new GameObject[100];
        DungeonDecoration21 = new GameObject[100];
        DungeonDecoration22 = new GameObject[100];
        DungeonDecoration23 = new GameObject[100];
        DungeonDecoration24 = new GameObject[100];
        DungeonDecoration25 = new GameObject[100];
        DungeonDecoration26 = new GameObject[100];
        DungeonDecoration27 = new GameObject[100];
        DungeonDecoration28 = new GameObject[100];
        DungeonDecoration29 = new GameObject[100];

        // 이펙트
        PlayerAtkEffect = new GameObject[5];
        PlayerCriticalAtkEffect = new GameObject[5];
        AbilityArrow0 = new GameObject[5];
        AbilityArrow0Hit = new GameObject[5];
        AbilityArrow1Active = new GameObject[5];
        AbilityArrow1Hit = new GameObject[100];
        AbilityArrow2Active = new GameObject[5];
        AbilityArrow2Hit = new GameObject[5];
        AbilitySword0 = new GameObject[5];
        AbilitySword1 = new GameObject[5];
        AbilitySword2 = new GameObject[5];
        AbilityMage0 = new GameObject[5];
        AbilityMage1Active = new GameObject[5];
        AbilityMage1Hit = new GameObject[50];
        AbilityMage1Hit2 = new GameObject[50];
        AbilityMage2 = new GameObject[5];
        AbilityBlacksmith0 = new GameObject[5];
        AbilityBlacksmith1 = new GameObject[5];
        AbilityBlacksmith2 = new GameObject[5];
        SwordComboAttack0 = new GameObject[5];
        SwordComboAttack1 = new GameObject[5];
        SwordComboAttack2 = new GameObject[5];
        BlacksmithComboAttack0 = new GameObject[5];
        BlacksmithComboAttack1 = new GameObject[5];
        BlacksmithComboAttack2 = new GameObject[5];
        AbilityHolyknight0 = new GameObject[5];
        AbilityHolyknight1Active = new GameObject[5];
        AbilityHolyknight1Hit = new GameObject[50];
        AbilityHolyknight2 = new GameObject[5];
        HolyknightComboAttack0 = new GameObject[5];
        HolyknightComboAttack1 = new GameObject[5];
        HolyknightComboAttack2 = new GameObject[5];

        // 체력바
        MonsterHpBar = new GameObject[200];

        // 텍스트 표시
        DamageText = new GameObject[50];
        HealingText = new GameObject[50];
        MissText = new GameObject[50];

        // 사운드
        ShootSound = new GameObject[5];
        MoveSound = new GameObject[5];
        DashSound = new GameObject[5];
        ItemSound = new GameObject[30];
        BasicAttackSound = new GameObject[5];
        CriticalAttackSound = new GameObject[5];
        BarrierSound = new GameObject[5];
        BoxSound = new GameObject[5];
        SecretDoorSound = new GameObject[5];
        ArrowSkill02Sound = new GameObject[5];
        ArrowSkill0HitSound = new GameObject[5];
        ArrowSkill1Sound = new GameObject[5];
        ArrowSkill2HitSound = new GameObject[5];
        PlayerDamagedSound = new GameObject[5];
        ButtonSound = new GameObject[5];
        UsePotionSound = new GameObject[5];
        EquipSound = new GameObject[5];
        UnEquipSound = new GameObject[5];
        FailEquipSound = new GameObject[5];
        SwordSound = new GameObject[5];
        SwordSkill0Sound = new GameObject[5];
        SwordSkill1Sound = new GameObject[5];
        SwordSkill2Sound = new GameObject[5];
        SwordSkill2HitSound = new GameObject[100];
        MageSkill0Sound = new GameObject[5];
        MageSkill1Sound = new GameObject[5];
        MageSkill2Sound = new GameObject[5];
        MageMissileSound = new GameObject[5];
        BlacksmithSkill0Sound = new GameObject[5];
        BlacksmithSkill1Sound = new GameObject[5];
        BlacksmithSkill1HitSound = new GameObject[100];
        BlacksmithSkill2Sound = new GameObject[5];
        HammerSound = new GameObject[5];
        HolyknightSkill0Sound = new GameObject[5];
        HolyknightSkill1Sound = new GameObject[100];
        HolyknightSkill2Sound = new GameObject[5];

        // 인벤토리 아이템
        inventoryItem0 = new GameObject[20];
        inventoryItem1 = new GameObject[20];
        inventoryItem2 = new GameObject[20];
        inventoryItem3 = new GameObject[20];
        inventoryItem4 = new GameObject[20];
        inventoryItem5 = new GameObject[20];
        inventoryItem6 = new GameObject[20];
        inventoryItem7 = new GameObject[20];
        inventoryItem8 = new GameObject[20];
        inventoryItem9 = new GameObject[20];
        inventoryItem10 = new GameObject[20];
        inventoryItem11 = new GameObject[20];
        inventoryItem12 = new GameObject[20];
        inventoryItem13 = new GameObject[20];
        inventoryItem14 = new GameObject[20];
        inventoryItem15 = new GameObject[20];
        inventoryItem16 = new GameObject[20];
        inventoryItem17 = new GameObject[20];
        inventoryItem18 = new GameObject[20];

        // 펫
        Cat = new GameObject[5];
        Duck = new GameObject[5];
        Penguin = new GameObject[5];
        Sheep = new GameObject[5];

        // 프리팹 생성후 배열에 저장 후 비활성화
        Generate();
    }

    void Generate()
    {
        // 프리팹 생성후 배열에 저장 후 비활성화
        // 무기
        for(int i = 0; i < Arrow.Length; i++)
        {
            Arrow[i] = Instantiate(ArrowPrefab);
            Arrow[i].SetActive(false);
        }
        for (int i = 0; i < Carrot.Length; i++)
        {
            Carrot[i] = Instantiate(CarrotPrefab);
            Carrot[i].SetActive(false);
        }
        for (int i = 0; i < CarrotGroup.Length; i++)
        {
            CarrotGroup[i] = Instantiate(CarrotGroubPrefab);
            CarrotGroup[i].SetActive(false);
        }
        for (int i = 0; i < CiclopEye.Length; i++)
        {
            CiclopEye[i] = Instantiate(CiclopEyePrefab);
            CiclopEye[i].SetActive(false);
        }
        for (int i = 0; i < MageMissile.Length; i++)
        {
            MageMissile[i] = Instantiate(MageMissilePrefab);
            MageMissile[i].SetActive(false);
        }

        // 패시브 스킬
        for (int i = 0; i < PassiveSkill0.Length; i++)
        {
            PassiveSkill0[i] = Instantiate(PassiveSkill0Prefab);
            PassiveSkill0[i].SetActive(false);
        }
        for (int i = 0; i < PassiveSkill1.Length; i++)
        {
            PassiveSkill1[i] = Instantiate(PassiveSkill1Prefab);
            PassiveSkill1[i].SetActive(false);
        }
        for (int i = 0; i < PassiveSkill2.Length; i++)
        {
            PassiveSkill2[i] = Instantiate(PassiveSkill2Prefab);
            PassiveSkill2[i].SetActive(false);
        }
        for (int i = 0; i < PassiveSkill3.Length; i++)
        {
            PassiveSkill3[i] = Instantiate(PassiveSkill3Prefab);
            PassiveSkill3[i].SetActive(false);
        }
        for (int i = 0; i < PassiveSkill4.Length; i++)
        {
            PassiveSkill4[i] = Instantiate(PassiveSkill4Prefab);
            PassiveSkill4[i].SetActive(false);
        }
        for (int i = 0; i < PassiveSkill5.Length; i++)
        {
            PassiveSkill5[i] = Instantiate(PassiveSkill5Prefab);
            PassiveSkill5[i].SetActive(false);
        }
        for (int i = 0; i < PassiveSkill6.Length; i++)
        {
            PassiveSkill6[i] = Instantiate(PassiveSkill6Prefab);
            PassiveSkill6[i].SetActive(false);
        }
        for (int i = 0; i < PassiveSkill7.Length; i++)
        {
            PassiveSkill7[i] = Instantiate(PassiveSkill7Prefab);
            PassiveSkill7[i].SetActive(false);
        }
        for (int i = 0; i < PassiveSkill8.Length; i++)
        {
            PassiveSkill8[i] = Instantiate(PassiveSkill8Prefab);
            PassiveSkill8[i].SetActive(false);
        }
        for (int i = 0; i < PassiveSkill9.Length; i++)
        {
            PassiveSkill9[i] = Instantiate(PassiveSkill9Prefab);
            PassiveSkill9[i].SetActive(false);
        }
        for (int i = 0; i < PassiveSkill10.Length; i++)
        {
            PassiveSkill10[i] = Instantiate(PassiveSkill10Prefab);
            PassiveSkill10[i].SetActive(false);
        }
        for (int i = 0; i < PassiveSkill11.Length; i++)
        {
            PassiveSkill11[i] = Instantiate(PassiveSkill11Prefab);
            PassiveSkill11[i].SetActive(false);
        }
        for (int i = 0; i < PassiveSkill12.Length; i++)
        {
            PassiveSkill12[i] = Instantiate(PassiveSkill12Prefab);
            PassiveSkill12[i].SetActive(false);
        }
        for (int i = 0; i < PassiveSkill3.Length; i++)
        {
            PassiveSkill3[i] = Instantiate(PassiveSkill3Prefab);
            PassiveSkill3[i].SetActive(false);
        }
        for (int i = 0; i < PassiveSkill14.Length; i++)
        {
            PassiveSkill14[i] = Instantiate(PassiveSkill14Prefab);
            PassiveSkill14[i].SetActive(false);
        }
        for (int i = 0; i < PassiveSkill15.Length; i++)
        {
            PassiveSkill15[i] = Instantiate(PassiveSkill15Prefab);
            PassiveSkill15[i].SetActive(false);
        }
        for (int i = 0; i < PassiveSkill16.Length; i++)
        {
            PassiveSkill16[i] = Instantiate(PassiveSkill16Prefab);
            PassiveSkill16[i].SetActive(false);
        }
        for (int i = 0; i < PassiveSkill17.Length; i++)
        {
            PassiveSkill17[i] = Instantiate(PassiveSkill17Prefab);
            PassiveSkill17[i].SetActive(false);
        }
        for (int i = 0; i < PassiveSkill18.Length; i++)
        {
            PassiveSkill18[i] = Instantiate(PassiveSkill18Prefab);
            PassiveSkill18[i].SetActive(false);
        }
        for (int i = 0; i < PassiveSkill19.Length; i++)
        {
            PassiveSkill19[i] = Instantiate(PassiveSkill19Prefab);
            PassiveSkill19[i].SetActive(false);
        }
        for (int i = 0; i < PassiveSkill20.Length; i++)
        {
            PassiveSkill20[i] = Instantiate(PassiveSkill20Prefab);
            PassiveSkill20[i].SetActive(false);
        }
        for (int i = 0; i < PassiveSkill21.Length; i++)
        {
            PassiveSkill21[i] = Instantiate(PassiveSkill21Prefab);
            PassiveSkill21[i].SetActive(false);
        }

        // 액티브 스킬
        for (int i = 0; i < ActiveSkill0.Length; i++)
        {
            ActiveSkill0[i] = Instantiate(ActiveSkill0Prefab);
            ActiveSkill0[i].SetActive(false);
        }
        for (int i = 0; i < ActiveSkill1.Length; i++)
        {
            ActiveSkill1[i] = Instantiate(ActiveSkill1Prefab);
            ActiveSkill1[i].SetActive(false);
        }

        // 코인
        for (int i = 0; i < Coin100.Length; i++)
        {
            Coin100[i] = Instantiate(Coin100Prefab);
            Coin100[i].SetActive(false);
        }
        for (int i = 0; i < Coin500.Length; i++)
        {
            Coin500[i] = Instantiate(Coin500Prefab);
            Coin500[i].SetActive(false);
        }
        for (int i = 0; i < Coin1000.Length; i++)
        {
            Coin1000[i] = Instantiate(Coin1000Prefab);
            Coin1000[i].SetActive(false);
        }

        // 시크릿박스
        for (int i = 0; i < SecretBox.Length; i++)
        {
            SecretBox[i] = Instantiate(SecretBoxPrefab);
            SecretBox[i].SetActive(false);
        }

        // 몬스터
        for (int i = 0; i < Boss0.Length; i++)
        {
            Boss0[i] = Instantiate(Boss0Prefab);
            Boss0[i].SetActive(false);
        }
        for (int i = 0; i < Boss1.Length; i++)
        {
            Boss1[i] = Instantiate(Boss1Prefab);
            Boss1[i].SetActive(false);
        }
        for (int i = 0; i < Boss2.Length; i++)
        {
            Boss2[i] = Instantiate(Boss2Prefab);
            Boss2[i].SetActive(false);
        }
        for (int i = 0; i < Normal0.Length; i++)
        {
            Normal0[i] = Instantiate(Normal0Prefab);
            Normal0[i].SetActive(false);
        }
        for (int i = 0; i < Normal1.Length; i++)
        {
            Normal1[i] = Instantiate(Normal1Prefab);
            Normal1[i].SetActive(false);
        }
        for (int i = 0; i < Normal2.Length; i++)
        {
            Normal2[i] = Instantiate(Normal2Prefab);
            Normal2[i].SetActive(false);
        }
        for (int i = 0; i < Normal3.Length; i++)
        {
            Normal3[i] = Instantiate(Normal3Prefab);
            Normal3[i].SetActive(false);
        }
        for (int i = 0; i < Normal4.Length; i++)
        {
            Normal4[i] = Instantiate(Normal4Prefab);
            Normal4[i].SetActive(false);
        }
        for (int i = 0; i < Normal5.Length; i++)
        {
            Normal5[i] = Instantiate(Normal5Prefab);
            Normal5[i].SetActive(false);
        }
        for (int i = 0; i < Normal6.Length; i++)
        {
            Normal6[i] = Instantiate(Normal6Prefab);
            Normal6[i].SetActive(false);
        }
        for (int i = 0; i < Normal7.Length; i++)
        {
            Normal7[i] = Instantiate(Normal7Prefab);
            Normal7[i].SetActive(false);
        }
        for (int i = 0; i < Normal8.Length; i++)
        {
            Normal8[i] = Instantiate(Normal8Prefab);
            Normal8[i].SetActive(false);
        }
        for (int i = 0; i < Normal9.Length; i++)
        {
            Normal9[i] = Instantiate(Normal9Prefab);
            Normal9[i].SetActive(false);
        }
        for (int i = 0; i < Normal10.Length; i++)
        {
            Normal10[i] = Instantiate(Normal10Prefab);
            Normal10[i].SetActive(false);
        }
        for (int i = 0; i < Normal11.Length; i++)
        {
            Normal11[i] = Instantiate(Normal11Prefab);
            Normal11[i].SetActive(false);
        }
        for (int i = 0; i < Normal12.Length; i++)
        {
            Normal12[i] = Instantiate(Normal12Prefab);
            Normal12[i].SetActive(false);
        }
        for (int i = 0; i < Normal13.Length; i++)
        {
            Normal13[i] = Instantiate(Normal13Prefab);
            Normal13[i].SetActive(false);
        }
        for (int i = 0; i < Normal14.Length; i++)
        {
            Normal14[i] = Instantiate(Normal14Prefab);
            Normal14[i].SetActive(false);
        }
        for (int i = 0; i < Normal15.Length; i++)
        {
            Normal15[i] = Instantiate(Normal15Prefab);
            Normal15[i].SetActive(false);
        }
        for (int i = 0; i < Normal16.Length; i++)
        {
            Normal16[i] = Instantiate(Normal16Prefab);
            Normal16[i].SetActive(false);
        }

        // 랜덤맵 모델
        for (int i = 0; i < B.Length; i++)
        {
            B[i] = Instantiate(BPrefab);
            B[i].SetActive(false);
        }
        for (int i = 0; i < BL.Length; i++)
        {
            BL[i] = Instantiate(BLPrefab);
            BL[i].SetActive(false);
        }
        for (int i = 0; i < LB.Length; i++)
        {
            LB[i] = Instantiate(LBPrefab);
            LB[i].SetActive(false);
        }
        for (int i = 0; i < BLT.Length; i++)
        {
            BLT[i] = Instantiate(BLTPrefab);
            BLT[i].SetActive(false);
        }
        for (int i = 0; i < LTB.Length; i++)
        {
            LTB[i] = Instantiate(LTBPrefab);
            LTB[i].SetActive(false);
        }
        for (int i = 0; i < TBL.Length; i++)
        {
            TBL[i] = Instantiate(TBLPrefab);
            TBL[i].SetActive(false);
        }
        for (int i = 0; i < L.Length; i++)
        {
            L[i] = Instantiate(LPrefab);
            L[i].SetActive(false);
        }
        for (int i = 0; i < LR.Length; i++)
        {
            LR[i] = Instantiate(LRPrefab);
            LR[i].SetActive(false);
        }
        for (int i = 0; i < RL.Length; i++)
        {
            RL[i] = Instantiate(RLPrefab);
            RL[i].SetActive(false);
        }
        for (int i = 0; i < LTR.Length; i++)
        {
            LTR[i] = Instantiate(LTRPrefab);
            LTR[i].SetActive(false);
        }
        for (int i = 0; i < TRL.Length; i++)
        {
            TRL[i] = Instantiate(TRLPrefab);
            TRL[i].SetActive(false);
        }
        for (int i = 0; i < RLT.Length; i++)
        {
            RLT[i] = Instantiate(RLTPrefab);
            RLT[i].SetActive(false);
        }
        for (int i = 0; i < R.Length; i++)
        {
            R[i] = Instantiate(RPrefab);
            R[i].SetActive(false);
        }
        for (int i = 0; i < RB.Length; i++)
        {
            RB[i] = Instantiate(RBPrefab);
            RB[i].SetActive(false);
        }
        for (int i = 0; i < BR.Length; i++)
        {
            BR[i] = Instantiate(BRPrefab);
            BR[i].SetActive(false);
        }
        for (int i = 0; i < RBL.Length; i++)
        {
            RBL[i] = Instantiate(RBLPrefab);
            RBL[i].SetActive(false);
        }
        for (int i = 0; i < BLR.Length; i++)
        {
            BLR[i] = Instantiate(BLRPrefab);
            BLR[i].SetActive(false);
        }
        for (int i = 0; i < LRB.Length; i++)
        {
            LRB[i] = Instantiate(LRBPrefab);
            LRB[i].SetActive(false);
        }
        for (int i = 0; i < SecretRoom.Length; i++)
        {
            SecretRoom[i] = Instantiate(SecretRoomPrefab);
            SecretRoom[i].SetActive(false);
        }
        for (int i = 0; i < T.Length; i++)
        {
            T[i] = Instantiate(TPrefab);
            T[i].SetActive(false);
        }
        for (int i = 0; i < TB.Length; i++)
        {
            TB[i] = Instantiate(TBPrefab);
            TB[i].SetActive(false);
        }
        for (int i = 0; i < BT.Length; i++)
        {
            BT[i] = Instantiate(BTPrefab);
            BT[i].SetActive(false);
        }
        for (int i = 0; i < TL.Length; i++)
        {
            TL[i] = Instantiate(TLPrefab);
            TL[i].SetActive(false);
        }
        for (int i = 0; i < LT.Length; i++)
        {
            LT[i] = Instantiate(LTPrefab);
            LT[i].SetActive(false);
        }
        for (int i = 0; i < TR.Length; i++)
        {
            TR[i] = Instantiate(TRPrefab);
            TR[i].SetActive(false);
        }
        for (int i = 0; i < RT.Length; i++)
        {
            RT[i] = Instantiate(RTPrefab);
            RT[i].SetActive(false);
        }
        for (int i = 0; i < TRB.Length; i++)
        {
            TRB[i] = Instantiate(TRBPrefab);
            TRB[i].SetActive(false);
        }
        for (int i = 0; i < RBT.Length; i++)
        {
            RBT[i] = Instantiate(RBTPrefab);
            RBT[i].SetActive(false);
        }
        for (int i = 0; i < BTR.Length; i++)
        {
            BTR[i] = Instantiate(BTRPrefab);
            BTR[i].SetActive(false);
        }

        // 던전 장식
        for (int i = 0; i < DungeonDecoration1.Length; i++)
        {
            DungeonDecoration1[i] = Instantiate(DungeonDecoration1Prefab);
            DungeonDecoration1[i].SetActive(false);
        }
        for (int i = 0; i < DungeonDecoration2.Length; i++)
        {
            DungeonDecoration2[i] = Instantiate(DungeonDecoration2Prefab);
            DungeonDecoration2[i].SetActive(false);
        }
        for (int i = 0; i < DungeonDecoration3.Length; i++)
        {
            DungeonDecoration3[i] = Instantiate(DungeonDecoration3Prefab);
            DungeonDecoration3[i].SetActive(false);
        }
        for (int i = 0; i < DungeonDecoration4.Length; i++)
        {
            DungeonDecoration4[i] = Instantiate(DungeonDecoration4Prefab);
            DungeonDecoration4[i].SetActive(false);
        }
        for (int i = 0; i < DungeonDecoration5.Length; i++)
        {
            DungeonDecoration5[i] = Instantiate(DungeonDecoration5Prefab);
            DungeonDecoration5[i].SetActive(false);
        }
        for (int i = 0; i < DungeonDecoration6.Length; i++)
        {
            DungeonDecoration6[i] = Instantiate(DungeonDecoration6Prefab);
            DungeonDecoration6[i].SetActive(false);
        }
        for (int i = 0; i < DungeonDecoration7.Length; i++)
        {
            DungeonDecoration7[i] = Instantiate(DungeonDecoration7Prefab);
            DungeonDecoration7[i].SetActive(false);
        }
        for (int i = 0; i < DungeonDecoration8.Length; i++)
        {
            DungeonDecoration8[i] = Instantiate(DungeonDecoration8Prefab);
            DungeonDecoration8[i].SetActive(false);
        }
        for (int i = 0; i < DungeonDecoration9.Length; i++)
        {
            DungeonDecoration9[i] = Instantiate(DungeonDecoration9Prefab);
            DungeonDecoration9[i].SetActive(false);
        }
        for (int i = 0; i < DungeonDecoration10.Length; i++)
        {
            DungeonDecoration10[i] = Instantiate(DungeonDecoration10Prefab);
            DungeonDecoration10[i].SetActive(false);
        }
        for (int i = 0; i < DungeonDecoration11.Length; i++)
        {
            DungeonDecoration11[i] = Instantiate(DungeonDecoration11Prefab);
            DungeonDecoration11[i].SetActive(false);
        }
        for (int i = 0; i < DungeonDecoration12.Length; i++)
        {
            DungeonDecoration12[i] = Instantiate(DungeonDecoration12Prefab);
            DungeonDecoration12[i].SetActive(false);
        }
        for (int i = 0; i < DungeonDecoration13.Length; i++)
        {
            DungeonDecoration13[i] = Instantiate(DungeonDecoration13Prefab);
            DungeonDecoration13[i].SetActive(false);
        }
        for (int i = 0; i < DungeonDecoration14.Length; i++)
        {
            DungeonDecoration14[i] = Instantiate(DungeonDecoration14Prefab);
            DungeonDecoration14[i].SetActive(false);
        }
        for (int i = 0; i < DungeonDecoration15.Length; i++)
        {
            DungeonDecoration15[i] = Instantiate(DungeonDecoration15Prefab);
            DungeonDecoration15[i].SetActive(false);
        }
        for (int i = 0; i < DungeonDecoration16.Length; i++)
        {
            DungeonDecoration16[i] = Instantiate(DungeonDecoration16Prefab);
            DungeonDecoration16[i].SetActive(false);
        }
        for (int i = 0; i < DungeonDecoration17.Length; i++)
        {
            DungeonDecoration17[i] = Instantiate(DungeonDecoration17Prefab);
            DungeonDecoration17[i].SetActive(false);
        }
        for (int i = 0; i < DungeonDecoration18.Length; i++)
        {
            DungeonDecoration18[i] = Instantiate(DungeonDecoration18Prefab);
            DungeonDecoration18[i].SetActive(false);
        }
        for (int i = 0; i < DungeonDecoration19.Length; i++)
        {
            DungeonDecoration19[i] = Instantiate(DungeonDecoration19Prefab);
            DungeonDecoration19[i].SetActive(false);
        }
        for (int i = 0; i < DungeonDecoration20.Length; i++)
        {
            DungeonDecoration20[i] = Instantiate(DungeonDecoration20Prefab);
            DungeonDecoration20[i].SetActive(false);
        }
        for (int i = 0; i < DungeonDecoration21.Length; i++)
        {
            DungeonDecoration21[i] = Instantiate(DungeonDecoration21Prefab);
            DungeonDecoration21[i].SetActive(false);
        }
        for (int i = 0; i < DungeonDecoration22.Length; i++)
        {
            DungeonDecoration22[i] = Instantiate(DungeonDecoration22Prefab);
            DungeonDecoration22[i].SetActive(false);
        }
        for (int i = 0; i < DungeonDecoration23.Length; i++)
        {
            DungeonDecoration23[i] = Instantiate(DungeonDecoration23Prefab);
            DungeonDecoration23[i].SetActive(false);
        }
        for (int i = 0; i < DungeonDecoration24.Length; i++)
        {
            DungeonDecoration24[i] = Instantiate(DungeonDecoration24Prefab);
            DungeonDecoration24[i].SetActive(false);
        }
        for (int i = 0; i < DungeonDecoration25.Length; i++)
        {
            DungeonDecoration25[i] = Instantiate(DungeonDecoration25Prefab);
            DungeonDecoration25[i].SetActive(false);
        }
        for (int i = 0; i < DungeonDecoration26.Length; i++)
        {
            DungeonDecoration26[i] = Instantiate(DungeonDecoration26Prefab);
            DungeonDecoration26[i].SetActive(false);
        }
        for (int i = 0; i < DungeonDecoration27.Length; i++)
        {
            DungeonDecoration27[i] = Instantiate(DungeonDecoration27Prefab);
            DungeonDecoration27[i].SetActive(false);
        }
        for (int i = 0; i < DungeonDecoration28.Length; i++)
        {
            DungeonDecoration28[i] = Instantiate(DungeonDecoration28Prefab);
            DungeonDecoration28[i].SetActive(false);
        }
        for (int i = 0; i < DungeonDecoration29.Length; i++)
        {
            DungeonDecoration29[i] = Instantiate(DungeonDecoration29Prefab);
            DungeonDecoration29[i].SetActive(false);
        }


        // 이펙트
        for (int i = 0; i < PlayerAtkEffect.Length; i++)
        {
            PlayerAtkEffect[i] = Instantiate(PlayerAtkEffectPrefab);
            PlayerAtkEffect[i].SetActive(false);
        }
        for (int i = 0; i < PlayerCriticalAtkEffect.Length; i++)
        {
            PlayerCriticalAtkEffect[i] = Instantiate(PlayerCriticalAtkEffectPrefab);
            PlayerCriticalAtkEffect[i].SetActive(false);
        }
        for (int i = 0; i < AbilityArrow0.Length; i++)
        {
            AbilityArrow0[i] = Instantiate(AbilityArrow0Prefab);
            AbilityArrow0[i].SetActive(false);
        }
        for (int i = 0; i < AbilityArrow0Hit.Length; i++)
        {
            AbilityArrow0Hit[i] = Instantiate(AbilityArrow0HitPrefab);
            AbilityArrow0Hit[i].SetActive(false);
        }
        for (int i = 0; i < AbilityArrow1Active.Length; i++)
        {
            AbilityArrow1Active[i] = Instantiate(AbilityArrow1ActivePrefab);
            AbilityArrow1Active[i].SetActive(false);
        }
        for (int i = 0; i < AbilityArrow1Hit.Length; i++)
        {
            AbilityArrow1Hit[i] = Instantiate(AbilityArrow1HitPrefab);
            AbilityArrow1Hit[i].SetActive(false);
        }
        for (int i = 0; i < AbilityArrow2Active.Length; i++)
        {
            AbilityArrow2Active[i] = Instantiate(AbilityArrow2ActivePrefab);
            AbilityArrow2Active[i].SetActive(false);
        }
        for (int i = 0; i < AbilityArrow2Hit.Length; i++)
        {
            AbilityArrow2Hit[i] = Instantiate(AbilityArrow2HitPrefab);
            AbilityArrow2Hit[i].SetActive(false);
        }
        for (int i = 0; i < AbilitySword0.Length; i++)
        {
            AbilitySword0[i] = Instantiate(AbilitySword0Prefab);
            AbilitySword0[i].SetActive(false);
        }
        for (int i = 0; i < AbilitySword1.Length; i++)
        {
            AbilitySword1[i] = Instantiate(AbilitySword1Prefab);
            AbilitySword1[i].SetActive(false);
        }
        for (int i = 0; i < AbilitySword2.Length; i++)
        {
            AbilitySword2[i] = Instantiate(AbilitySword2Prefab);
            AbilitySword2[i].SetActive(false);
        }
        for (int i = 0; i < AbilityMage0.Length; i++)
        {
            AbilityMage0[i] = Instantiate(AbilityMage0Prefab);
            AbilityMage0[i].SetActive(false);
        }
        for (int i = 0; i < AbilityMage1Active.Length; i++)
        {
            AbilityMage1Active[i] = Instantiate(AbilityMage1ActivePrefab);
            AbilityMage1Active[i].SetActive(false);
        }
        for (int i = 0; i < AbilityMage1Hit.Length; i++)
        {
            AbilityMage1Hit[i] = Instantiate(AbilityMage1HitPrefab);
            AbilityMage1Hit[i].SetActive(false);
        }
        for (int i = 0; i < AbilityMage1Hit2.Length; i++)
        {
            AbilityMage1Hit2[i] = Instantiate(AbilityMage1Hit2Prefab);
            AbilityMage1Hit2[i].SetActive(false);
        }
        for (int i = 0; i < AbilityMage2.Length; i++)
        {
            AbilityMage2[i] = Instantiate(AbilityMage2Prefab);
            AbilityMage2[i].SetActive(false);
        }
        for (int i = 0; i < AbilityBlacksmith0.Length; i++)
        {
            AbilityBlacksmith0[i] = Instantiate(AbilityBlacksmith0Prefab);
            AbilityBlacksmith0[i].SetActive(false);
        }
        for (int i = 0; i < AbilityBlacksmith1.Length; i++)
        {
            AbilityBlacksmith1[i] = Instantiate(AbilityBlacksmith1Prefab);
            AbilityBlacksmith1[i].SetActive(false);
        }
        for (int i = 0; i < AbilityBlacksmith2.Length; i++)
        {
            AbilityBlacksmith2[i] = Instantiate(AbilityBlacksmith2Prefab);
            AbilityBlacksmith2[i].SetActive(false);
        }
        for (int i = 0; i < SwordComboAttack0.Length; i++)
        {
            SwordComboAttack0[i] = Instantiate(SwordComboAttack0Prefab);
            SwordComboAttack0[i].SetActive(false);
        }
        for (int i = 0; i < SwordComboAttack1.Length; i++)
        {
            SwordComboAttack1[i] = Instantiate(SwordComboAttack1Prefab);
            SwordComboAttack1[i].SetActive(false);
        }
        for (int i = 0; i < SwordComboAttack2.Length; i++)
        {
            SwordComboAttack2[i] = Instantiate(SwordComboAttack2Prefab);
            SwordComboAttack2[i].SetActive(false);
        }
        for (int i = 0; i < BlacksmithComboAttack0.Length; i++)
        {
            BlacksmithComboAttack0[i] = Instantiate(BlacksmithComboAttack0Prefab);
            BlacksmithComboAttack0[i].SetActive(false);
        }
        for (int i = 0; i < BlacksmithComboAttack1.Length; i++)
        {
            BlacksmithComboAttack1[i] = Instantiate(BlacksmithComboAttack1Prefab);
            BlacksmithComboAttack1[i].SetActive(false);
        }
        for (int i = 0; i < BlacksmithComboAttack2.Length; i++)
        {
            BlacksmithComboAttack2[i] = Instantiate(BlacksmithComboAttack2Prefab);
            BlacksmithComboAttack2[i].SetActive(false);
        }
        for (int i = 0; i < AbilityHolyknight0.Length; i++)
        {
            AbilityHolyknight0[i] = Instantiate(AbilityHolyknight0Prefab);
            AbilityHolyknight0[i].SetActive(false);
        }
        for (int i = 0; i < AbilityHolyknight1Active.Length; i++)
        {
            AbilityHolyknight1Active[i] = Instantiate(AbilityHolyknight1ActivePrefab);
            AbilityHolyknight1Active[i].SetActive(false);
        }
        for (int i = 0; i < AbilityHolyknight1Hit.Length; i++)
        {
            AbilityHolyknight1Hit[i] = Instantiate(AbilityHolyknight1HitPrefab);
            AbilityHolyknight1Hit[i].SetActive(false);
        }
        for (int i = 0; i < AbilityHolyknight2.Length; i++)
        {
            AbilityHolyknight2[i] = Instantiate(AbilityHolyknight2Prefab);
            AbilityHolyknight2[i].SetActive(false);
        }
        for (int i = 0; i < HolyknightComboAttack0.Length; i++)
        {
            HolyknightComboAttack0[i] = Instantiate(HolyknightComboAttack0Prefab);
            HolyknightComboAttack0[i].SetActive(false);
        }
        for (int i = 0; i < HolyknightComboAttack1.Length; i++)
        {
            HolyknightComboAttack1[i] = Instantiate(HolyknightComboAttack1Prefab);
            HolyknightComboAttack1[i].SetActive(false);
        }
        for (int i = 0; i < HolyknightComboAttack2.Length; i++)
        {
            HolyknightComboAttack2[i] = Instantiate(HolyknightComboAttack2Prefab);
            HolyknightComboAttack2[i].SetActive(false);
        }

        // 체력바
        // 체력바는 부모를 캔버스로 설정해준뒤 비활성화 해둔다
        Invoke("SetCanvas", 0.5f);

        // 텍스트 표시
        // 부모를 FloatingText 오브젝트로 설정
        for (int i = 0; i < DamageText.Length; i++)
        {
            DamageText[i] = Instantiate(DamageTextPrefab);
            DamageText[i].transform.SetParent(FloatingText.transform);
            DamageText[i].SetActive(false);
        }
        for (int i = 0; i < HealingText.Length; i++)
        {
            HealingText[i] = Instantiate(HealingTextPrefab);
            HealingText[i].transform.SetParent(FloatingText.transform);
            HealingText[i].SetActive(false);
        }
        for (int i = 0; i < MissText.Length; i++)
        {
            MissText[i] = Instantiate(MissTextPrefab);
            MissText[i].transform.SetParent(FloatingText.transform);
            MissText[i].SetActive(false);
        }

        // 사운드
        for (int i = 0; i < ShootSound.Length; i++)
        {
            ShootSound[i] = Instantiate(ShootSoundPrefab);
            ShootSound[i].SetActive(false);
        }
        for (int i = 0; i < MoveSound.Length; i++)
        {
            MoveSound[i] = Instantiate(MoveSoundPrefab);
            MoveSound[i].SetActive(false);
        }
        for (int i = 0; i < DashSound.Length; i++)
        {
            DashSound[i] = Instantiate(DashSoundPrefab);
            DashSound[i].SetActive(false);
        }
        for (int i = 0; i < ItemSound.Length; i++)
        {
            ItemSound[i] = Instantiate(ItemSoundPrefab);
            ItemSound[i].SetActive(false);
        }
        for (int i = 0; i < BasicAttackSound.Length; i++)
        {
            BasicAttackSound[i] = Instantiate(BasicAttackSoundPrefab);
            BasicAttackSound[i].SetActive(false);
        }
        for (int i = 0; i < CriticalAttackSound.Length; i++)
        {
            CriticalAttackSound[i] = Instantiate(CriticalAttackSoundPrefab);
            CriticalAttackSound[i].SetActive(false);
        }
        for (int i = 0; i < BarrierSound.Length; i++)
        {
            BarrierSound[i] = Instantiate(BarrierSoundPrefab);
            BarrierSound[i].SetActive(false);
        }
        for (int i = 0; i < BoxSound.Length; i++)
        {
            BoxSound[i] = Instantiate(BoxSoundPrefab);
            BoxSound[i].SetActive(false);
        }
        for (int i = 0; i < SecretDoorSound.Length; i++)
        {
            SecretDoorSound[i] = Instantiate(SecretDoorSoundPrefab);
            SecretDoorSound[i].SetActive(false);
        }
        for (int i = 0; i < ArrowSkill02Sound.Length; i++)
        {
            ArrowSkill02Sound[i] = Instantiate(ArrowSkill02SoundPrefab);
            ArrowSkill02Sound[i].SetActive(false);
        }
        for (int i = 0; i < ArrowSkill0HitSound.Length; i++)
        {
            ArrowSkill0HitSound[i] = Instantiate(ArrowSkill0HitSoundPrefab);
            ArrowSkill0HitSound[i].SetActive(false);
        }
        for (int i = 0; i < ArrowSkill1Sound.Length; i++)
        {
            ArrowSkill1Sound[i] = Instantiate(ArrowSkill1SoundPrefab);
            ArrowSkill1Sound[i].SetActive(false);
        }
        for (int i = 0; i < ArrowSkill2HitSound.Length; i++)
        {
            ArrowSkill2HitSound[i] = Instantiate(ArrowSkill2HitSoundPrefab);
            ArrowSkill2HitSound[i].SetActive(false);
        }
        for (int i = 0; i < PlayerDamagedSound.Length; i++)
        {
            PlayerDamagedSound[i] = Instantiate(PlayerDamagedSoundPrefab);
            PlayerDamagedSound[i].SetActive(false);
        }
        for (int i = 0; i < ButtonSound.Length; i++)
        {
            ButtonSound[i] = Instantiate(ButtonSoundPrefab);
            ButtonSound[i].SetActive(false);
        }
        for (int i = 0; i < UsePotionSound.Length; i++)
        {
            UsePotionSound[i] = Instantiate(UsePotionSoundPrefab);
            UsePotionSound[i].SetActive(false);
        }
        for (int i = 0; i < EquipSound.Length; i++)
        {
            EquipSound[i] = Instantiate(EquipSoundPrefab);
            EquipSound[i].SetActive(false);
        }
        for (int i = 0; i < UnEquipSound.Length; i++)
        {
            UnEquipSound[i] = Instantiate(UnEquipSoundPrefab);
            UnEquipSound[i].SetActive(false);
        }
        for (int i = 0; i < FailEquipSound.Length; i++)
        {
            FailEquipSound[i] = Instantiate(FailEquipSoundPrefab);
            FailEquipSound[i].SetActive(false);
        }
        for (int i = 0; i < SwordSound.Length; i++)
        {
            SwordSound[i] = Instantiate(SwordSoundPrefab);
            SwordSound[i].SetActive(false);
        }
        for (int i = 0; i < SwordSkill0Sound.Length; i++)
        {
            SwordSkill0Sound[i] = Instantiate(SwordSkill0SoundPrefab);
            SwordSkill0Sound[i].SetActive(false);
        }
        for (int i = 0; i < SwordSkill1Sound.Length; i++)
        {
            SwordSkill1Sound[i] = Instantiate(SwordSkill1SoundPrefab);
            SwordSkill1Sound[i].SetActive(false);
        }
        for (int i = 0; i < SwordSkill2Sound.Length; i++)
        {
            SwordSkill2Sound[i] = Instantiate(SwordSkill2SoundPrefab);
            SwordSkill2Sound[i].SetActive(false);
        }
        for (int i = 0; i < SwordSkill2HitSound.Length; i++)
        {
            SwordSkill2HitSound[i] = Instantiate(SwordSkill2HitSoundPrefab);
            SwordSkill2HitSound[i].SetActive(false);
        }
        for (int i = 0; i < MageSkill0Sound.Length; i++)
        {
            MageSkill0Sound[i] = Instantiate(MageSkill0SoundPrefab);
            MageSkill0Sound[i].SetActive(false);
        }
        for (int i = 0; i < MageSkill1Sound.Length; i++)
        {
            MageSkill1Sound[i] = Instantiate(MageSkill1SoundPrefab);
            MageSkill1Sound[i].SetActive(false);
        }
        for (int i = 0; i < MageSkill2Sound.Length; i++)
        {
            MageSkill2Sound[i] = Instantiate(MageSkill2SoundPrefab);
            MageSkill2Sound[i].SetActive(false);
        }
        for (int i = 0; i < MageMissileSound.Length; i++)
        {
            MageMissileSound[i] = Instantiate(MageMissileSoundPrefab);
            MageMissileSound[i].SetActive(false);
        }
        for (int i = 0; i < BlacksmithSkill0Sound.Length; i++)
        {
            BlacksmithSkill0Sound[i] = Instantiate(BlacksmithSkill0SoundPrefab);
            BlacksmithSkill0Sound[i].SetActive(false);
        }
        for (int i = 0; i < BlacksmithSkill1Sound.Length; i++)
        {
            BlacksmithSkill1Sound[i] = Instantiate(BlacksmithSkill1SoundPrefab);
            BlacksmithSkill1Sound[i].SetActive(false);
        }
        for (int i = 0; i < BlacksmithSkill1HitSound.Length; i++)
        {
            BlacksmithSkill1HitSound[i] = Instantiate(BlacksmithSkill1HitSoundPrefab);
            BlacksmithSkill1HitSound[i].SetActive(false);
        }
        for (int i = 0; i < BlacksmithSkill2Sound.Length; i++)
        {
            BlacksmithSkill2Sound[i] = Instantiate(BlacksmithSkill2SoundPrefab);
            BlacksmithSkill2Sound[i].SetActive(false);
        }
        for (int i = 0; i < HammerSound.Length; i++)
        {
            HammerSound[i] = Instantiate(HammerSoundPrefab);
            HammerSound[i].SetActive(false);
        }
        for (int i = 0; i < HolyknightSkill0Sound.Length; i++)
        {
            HolyknightSkill0Sound[i] = Instantiate(HolyknightSkill0SoundPrefab);
            HolyknightSkill0Sound[i].SetActive(false);
        }
        for (int i = 0; i < HolyknightSkill1Sound.Length; i++)
        {
            HolyknightSkill1Sound[i] = Instantiate(HolyknightSkill1SoundPrefab);
            HolyknightSkill1Sound[i].SetActive(false);
        }
        for (int i = 0; i < HolyknightSkill2Sound.Length; i++)
        {
            HolyknightSkill2Sound[i] = Instantiate(HolyknightSkill2SoundPrefab);
            HolyknightSkill2Sound[i].SetActive(false);
        }

        // 인벤토리 아이템
        for (int i = 0; i < inventoryItem0.Length; i++)
        {
            inventoryItem0[i] = Instantiate(inventoryItem0Prefab);
            inventoryItem0[i].SetActive(false);
        }
        for (int i = 0; i < inventoryItem1.Length; i++)
        {
            inventoryItem1[i] = Instantiate(inventoryItem1Prefab);
            inventoryItem1[i].SetActive(false);
        }
        for (int i = 0; i < inventoryItem2.Length; i++)
        {
            inventoryItem2[i] = Instantiate(inventoryItem2Prefab);
            inventoryItem2[i].SetActive(false);
        }
        for (int i = 0; i < inventoryItem3.Length; i++)
        {
            inventoryItem3[i] = Instantiate(inventoryItem3Prefab);
            inventoryItem3[i].SetActive(false);
        }
        for (int i = 0; i < inventoryItem4.Length; i++)
        {
            inventoryItem4[i] = Instantiate(inventoryItem4Prefab);
            inventoryItem4[i].SetActive(false);
        }
        for (int i = 0; i < inventoryItem5.Length; i++)
        {
            inventoryItem5[i] = Instantiate(inventoryItem5Prefab);
            inventoryItem5[i].SetActive(false);
        }
        for (int i = 0; i < inventoryItem6.Length; i++)
        {
            inventoryItem6[i] = Instantiate(inventoryItem6Prefab);
            inventoryItem6[i].SetActive(false);
        }
        for (int i = 0; i < inventoryItem7.Length; i++)
        {
            inventoryItem7[i] = Instantiate(inventoryItem7Prefab);
            inventoryItem7[i].SetActive(false);
        }
        for (int i = 0; i < inventoryItem8.Length; i++)
        {
            inventoryItem8[i] = Instantiate(inventoryItem8Prefab);
            inventoryItem8[i].SetActive(false);
        }
        for (int i = 0; i < inventoryItem9.Length; i++)
        {
            inventoryItem9[i] = Instantiate(inventoryItem9Prefab);
            inventoryItem9[i].SetActive(false);
        }
        for (int i = 0; i < inventoryItem10.Length; i++)
        {
            inventoryItem10[i] = Instantiate(inventoryItem10Prefab);
            inventoryItem10[i].SetActive(false);
        }
        for (int i = 0; i < inventoryItem11.Length; i++)
        {
            inventoryItem11[i] = Instantiate(inventoryItem11Prefab);
            inventoryItem11[i].SetActive(false);
        }
        for (int i = 0; i < inventoryItem12.Length; i++)
        {
            inventoryItem12[i] = Instantiate(inventoryItem12Prefab);
            inventoryItem12[i].SetActive(false);
        }
        for (int i = 0; i < inventoryItem13.Length; i++)
        {
            inventoryItem13[i] = Instantiate(inventoryItem13Prefab);
            inventoryItem13[i].SetActive(false);
        }
        for (int i = 0; i < inventoryItem14.Length; i++)
        {
            inventoryItem14[i] = Instantiate(inventoryItem14Prefab);
            inventoryItem14[i].SetActive(false);
        }
        for (int i = 0; i < inventoryItem15.Length; i++)
        {
            inventoryItem15[i] = Instantiate(inventoryItem15Prefab);
            inventoryItem15[i].SetActive(false);
        }
        for (int i = 0; i < inventoryItem16.Length; i++)
        {
            inventoryItem16[i] = Instantiate(inventoryItem16Prefab);
            inventoryItem16[i].SetActive(false);
        }
        for (int i = 0; i < inventoryItem17.Length; i++)
        {
            inventoryItem17[i] = Instantiate(inventoryItem17Prefab);
            inventoryItem17[i].SetActive(false);
        }
        for (int i = 0; i < inventoryItem18.Length; i++)
        {
            inventoryItem18[i] = Instantiate(inventoryItem18Prefab);
            inventoryItem18[i].SetActive(false);
        }

        // 펫
        for (int i = 0; i < Cat.Length; i++)
        {
            Cat[i] = Instantiate(CatPrefab);
            Cat[i].SetActive(false);
        }
        for (int i = 0; i < Duck.Length; i++)
        {
            Duck[i] = Instantiate(DuckPrefab);
            Duck[i].SetActive(false);
        }
        for (int i = 0; i < Penguin.Length; i++)
        {
            Penguin[i] = Instantiate(PenguinPrefab);
            Penguin[i].SetActive(false);
        }
        for (int i = 0; i < Sheep.Length; i++)
        {
            Sheep[i] = Instantiate(SheepPrefab);
            Sheep[i].SetActive(false);
        }
    }

    public GameObject GetObj(string type)
    {
        // 생성할 프리팹 종류 할당
        switch(type)
        {
            // 무기
            case "Arrow":
                targetPool = Arrow;
                break;
            case "Carrot":
                targetPool = Carrot;
                break;
            case "CarrotGroup":
                targetPool = CarrotGroup;
                break;
            case "CiclopEye":
                targetPool = CiclopEye;
                break;
            case "MageMissile":
                targetPool = MageMissile;
                break;

            // 패시브 스킬
            case "PassiveSkill0":
                targetPool = PassiveSkill0;
                break;
            case "PassiveSkill1":
                targetPool = PassiveSkill1;
                break;
            case "PassiveSkill2":
                targetPool = PassiveSkill2;
                break;
            case "PassiveSkill3":
                targetPool = PassiveSkill3;
                break;
            case "PassiveSkill4":
                targetPool = PassiveSkill4;
                break;
            case "PassiveSkill5":
                targetPool = PassiveSkill5;
                break;
            case "PassiveSkill6":
                targetPool = PassiveSkill6;
                break;
            case "PassiveSkill7":
                targetPool = PassiveSkill7;
                break;
            case "PassiveSkill8":
                targetPool = PassiveSkill8;
                break;
            case "PassiveSkill9":
                targetPool = PassiveSkill9;
                break;
            case "PassiveSkill10":
                targetPool = PassiveSkill10;
                break;
            case "PassiveSkill11":
                targetPool = PassiveSkill11;
                break;
            case "PassiveSkill12":
                targetPool = PassiveSkill12;
                break;
            case "PassiveSkill13":
                targetPool = PassiveSkill13;
                break;
            case "PassiveSkill14":
                targetPool = PassiveSkill14;
                break;
            case "PassiveSkill15":
                targetPool = PassiveSkill15;
                break;
            case "PassiveSkill16":
                targetPool = PassiveSkill16;
                break;
            case "PassiveSkill17":
                targetPool = PassiveSkill17;
                break;
            case "PassiveSkill18":
                targetPool = PassiveSkill18;
                break;
            case "PassiveSkill19":
                targetPool = PassiveSkill19;
                break;
            case "PassiveSkill20":
                targetPool = PassiveSkill20;
                break;
            case "PassiveSkill21":
                targetPool = PassiveSkill21;
                break;

            // 액티브 스킬
            case "ActiveSkill0":
                targetPool = ActiveSkill0;
                break;
            case "ActiveSkill1":
                targetPool = ActiveSkill1;
                break;

            // 코인
            case "Coin100":
                targetPool = Coin100;
                break;
            case "Coin500":
                targetPool = Coin500;
                break;
            case "Coin1000":
                targetPool = Coin1000;
                break;

            // 시크릿박스
            case "SecretBox":
                targetPool = SecretBox;
                break;

            // 몬스터
            case "Boss0":
                targetPool = Boss0;
                break;
            case "Boss1":
                targetPool = Boss1;
                break;
            case "Boss2":
                targetPool = Boss2;
                break;
            case "Normal0":
                targetPool = Normal0;
                break;
            case "Normal1":
                targetPool = Normal1;
                break;
            case "Normal2":
                targetPool = Normal2;
                break;
            case "Normal3":
                targetPool = Normal3;
                break;
            case "Normal4":
                targetPool = Normal4;
                break;
            case "Normal5":
                targetPool = Normal5;
                break;
            case "Normal6":
                targetPool = Normal6;
                break;
            case "Normal7":
                targetPool = Normal7;
                break;
            case "Normal8":
                targetPool = Normal8;
                break;
            case "Normal9":
                targetPool = Normal9;
                break;
            case "Normal10":
                targetPool = Normal10;
                break;
            case "Normal11":
                targetPool = Normal11;
                break;
            case "Normal12":
                targetPool = Normal12;
                break;
            case "Normal13":
                targetPool = Normal13;
                break;
            case "Normal14":
                targetPool = Normal14;
                break;
            case "Normal15":
                targetPool = Normal15;
                break;
            case "Normal16":
                targetPool = Normal16;
                break;

            // 랜덤맵 모델
            case "B":
                targetPool = B;
                break;
            case "BL":
                targetPool = BL;
                break;
            case "LB":
                targetPool = LB;
                break;
            case "BLT":
                targetPool = BLT;
                break;
            case "LTB":
                targetPool = LTB;
                break;
            case "TBL":
                targetPool = TBL;
                break;
            case "L":
                targetPool = L;
                break;
            case "LR":
                targetPool = LR;
                break;
            case "RL":
                targetPool = RL;
                break;
            case "LTR":
                targetPool = LTR;
                break;
            case "TRL":
                targetPool = TRL;
                break;
            case "RLT":
                targetPool = RLT;
                break;
            case "R":
                targetPool = R;
                break;
            case "RB":
                targetPool = RB;
                break;
            case "BR":
                targetPool = BR;
                break;
            case "RBL":
                targetPool = RBL;
                break;
            case "BLR":
                targetPool = BLR;
                break;
            case "LRB":
                targetPool = LRB;
                break;
            case "SecretRoom":
                targetPool = SecretRoom;
                break;
            case "T":
                targetPool = T;
                break;
            case "TB":
                targetPool = TB;
                break;
            case "BT":
                targetPool = BT;
                break;
            case "TL":
                targetPool = TL;
                break;
            case "LT":
                targetPool = LT;
                break;
            case "TR":
                targetPool = TR;
                break;
            case "RT":
                targetPool = RT;
                break;
            case "TRB":
                targetPool = TRB;
                break;
            case "RBT":
                targetPool = RBT;
                break;
            case "BTR":
                targetPool = BTR;
                break;

            // 던전 장식
            case "DungeonDecoration1":
                targetPool = DungeonDecoration1;
                break;
            case "DungeonDecoration2":
                targetPool = DungeonDecoration2;
                break;
            case "DungeonDecoration3":
                targetPool = DungeonDecoration3;
                break;
            case "DungeonDecoration4":
                targetPool = DungeonDecoration4;
                break;
            case "DungeonDecoration5":
                targetPool = DungeonDecoration5;
                break;
            case "DungeonDecoration6":
                targetPool = DungeonDecoration6;
                break;
            case "DungeonDecoration7":
                targetPool = DungeonDecoration7;
                break;
            case "DungeonDecoration8":
                targetPool = DungeonDecoration8;
                break;
            case "DungeonDecoration9":
                targetPool = DungeonDecoration9;
                break;
            case "DungeonDecoration10":
                targetPool = DungeonDecoration10;
                break;
            case "DungeonDecoration11":
                targetPool = DungeonDecoration11;
                break;
            case "DungeonDecoration12":
                targetPool = DungeonDecoration12;
                break;
            case "DungeonDecoration13":
                targetPool = DungeonDecoration13;
                break;
            case "DungeonDecoration14":
                targetPool = DungeonDecoration14;
                break;
            case "DungeonDecoration15":
                targetPool = DungeonDecoration15;
                break;
            case "DungeonDecoration16":
                targetPool = DungeonDecoration16;
                break;
            case "DungeonDecoration17":
                targetPool = DungeonDecoration17;
                break;
            case "DungeonDecoration18":
                targetPool = DungeonDecoration18;
                break;
            case "DungeonDecoration19":
                targetPool = DungeonDecoration19;
                break;
            case "DungeonDecoration20":
                targetPool = DungeonDecoration20;
                break;
            case "DungeonDecoration21":
                targetPool = DungeonDecoration21;
                break;
            case "DungeonDecoration22":
                targetPool = DungeonDecoration22;
                break;
            case "DungeonDecoration23":
                targetPool = DungeonDecoration23;
                break;
            case "DungeonDecoration24":
                targetPool = DungeonDecoration24;
                break;
            case "DungeonDecoration25":
                targetPool = DungeonDecoration25;
                break;
            case "DungeonDecoration26":
                targetPool = DungeonDecoration26;
                break;
            case "DungeonDecoration27":
                targetPool = DungeonDecoration27;
                break;
            case "DungeonDecoration28":
                targetPool = DungeonDecoration28;
                break;
            case "DungeonDecoration29":
                targetPool = DungeonDecoration29;
                break;

            // 이펙트
            case "PlayerAtkEffect":
                targetPool = PlayerAtkEffect;
                break;
            case "PlayerCriticalAtkEffect":
                targetPool = PlayerCriticalAtkEffect;
                break;
            case "AbilityArrow0":
                targetPool = AbilityArrow0;
                break;
            case "AbilityArrow0Hit":
                targetPool = AbilityArrow0Hit;
                break;
            case "AbilityArrow1Active":
                targetPool = AbilityArrow1Active;
                break;
            case "AbilityArrow1Hit":
                targetPool = AbilityArrow1Hit;
                break;
            case "AbilityArrow2Active":
                targetPool = AbilityArrow2Active;
                break;
            case "AbilityArrow2Hit":
                targetPool = AbilityArrow2Hit;
                break;
            case "AbilitySword0":
                targetPool = AbilitySword0;
                break;
            case "AbilitySword1":
                targetPool = AbilitySword1;
                break;
            case "AbilitySword2":
                targetPool = AbilitySword2;
                break;
            case "AbilityMage0":
                targetPool = AbilityMage0;
                break;
            case "AbilityMage1Active":
                targetPool = AbilityMage1Active;
                break;
            case "AbilityMage1Hit":
                targetPool = AbilityMage1Hit;
                break;
            case "AbilityMage1Hit2":
                targetPool = AbilityMage1Hit2;
                break;
            case "AbilityMage2":
                targetPool = AbilityMage2;
                break;
            case "AbilityBlacksmith0":
                targetPool = AbilityBlacksmith0;
                break;
            case "AbilityBlacksmith1":
                targetPool = AbilityBlacksmith1;
                break;
            case "AbilityBlacksmith2":
                targetPool = AbilityBlacksmith2;
                break;
            case "SwordComboAttack0":
                targetPool = SwordComboAttack0;
                break;
            case "SwordComboAttack1":
                targetPool = SwordComboAttack1;
                break;
            case "SwordComboAttack2":
                targetPool = SwordComboAttack2;
                break;
            case "BlacksmithComboAttack0":
                targetPool = BlacksmithComboAttack0;
                break;
            case "BlacksmithComboAttack1":
                targetPool = BlacksmithComboAttack1;
                break;
            case "BlacksmithComboAttack2":
                targetPool = BlacksmithComboAttack2;
                break;
            case "AbilityHolyknight0":
                targetPool = AbilityHolyknight0;
                break;
            case "AbilityHolyknight1Active":
                targetPool = AbilityHolyknight1Active;
                break;
            case "AbilityHolyknight1Hit":
                targetPool = AbilityHolyknight1Hit;
                break;
            case "AbilityHolyknight2":
                targetPool = AbilityHolyknight2;
                break;
            case "HolyknightComboAttack0":
                targetPool = HolyknightComboAttack0;
                break;
            case "HolyknightComboAttack1":
                targetPool = HolyknightComboAttack1;
                break;
            case "HolyknightComboAttack2":
                targetPool = HolyknightComboAttack2;
                break;

            // 체력바
            case "MonsterHpBar":
                targetPool = MonsterHpBar;
                break;

            // 텍스트 표시
            case "DamageText":
                targetPool = DamageText;
                break;
            case "HealingText":
                targetPool = HealingText;
                break;
            case "MissText":
                targetPool = MissText;
                break;

            // 사운드
            case "ShootSound":
                targetPool = ShootSound;
                break;
            case "MoveSound":
                targetPool = MoveSound;
                break;
            case "DashSound":
                targetPool = DashSound;
                break;
            case "ItemSound":
                targetPool = ItemSound;
                break;
            case "BasicAttackSound":
                targetPool = BasicAttackSound;
                break;
            case "CriticalAttackSound":
                targetPool = CriticalAttackSound;
                break;
            case "BarrierSound":
                targetPool = BarrierSound;
                break;
            case "BoxSound":
                targetPool = BoxSound;
                break;
            case "SecretDoorSound":
                targetPool = SecretDoorSound;
                break;
            case "ArrowSkill02Sound":
                targetPool = ArrowSkill02Sound;
                break;
            case "ArrowSkill0HitSound":
                targetPool = ArrowSkill0HitSound;
                break;
            case "ArrowSkill1Sound":
                targetPool = ArrowSkill1Sound;
                break;
            case "ArrowSkill2HitSound":
                targetPool = ArrowSkill2HitSound;
                break;
            case "PlayerDamagedSound":
                targetPool = PlayerDamagedSound;
                break;
            case "ButtonSound":
                targetPool = ButtonSound;
                break;
            case "UsePotionSound":
                targetPool = UsePotionSound;
                break;
            case "EquipSound":
                targetPool = EquipSound;
                break;
            case "UnEquipSound":
                targetPool = UnEquipSound;
                break;
            case "FailEquipSound":
                targetPool = FailEquipSound;
                break;
            case "SwordSound":
                targetPool = SwordSound;
                break;
            case "SwordSkill0Sound":
                targetPool = SwordSkill0Sound;
                break;
            case "SwordSkill1Sound":
                targetPool = SwordSkill1Sound;
                break;
            case "SwordSkill2Sound":
                targetPool = SwordSkill2Sound;
                break;
            case "SwordSkill2HitSound":
                targetPool = SwordSkill2HitSound;
                break;
            case "MageSkill0Sound":
                targetPool = MageSkill0Sound;
                break;
            case "MageSkill1Sound":
                targetPool = MageSkill1Sound;
                break;
            case "MageSkill2Sound":
                targetPool = MageSkill2Sound;
                break;
            case "MageMissileSound":
                targetPool = MageMissileSound;
                break;
            case "BlacksmithSkill0Sound":
                targetPool = BlacksmithSkill0Sound;
                break;
            case "BlacksmithSkill1Sound":
                targetPool = BlacksmithSkill1Sound;
                break;
            case "BlacksmithSkill1HitSound":
                targetPool = BlacksmithSkill1HitSound;
                break;
            case "BlacksmithSkill2Sound":
                targetPool = BlacksmithSkill2Sound;
                break;
            case "HammerSound":
                targetPool = HammerSound;
                break;
            case "HolyknightSkill0Sound":
                targetPool = HolyknightSkill0Sound;
                break;
            case "HolyknightSkill1Sound":
                targetPool = HolyknightSkill1Sound;
                break;
            case "HolyknightSkill2Sound":
                targetPool = HolyknightSkill2Sound;
                break;

            // 인벤토리 아이템
            case "inventoryItem0":
                targetPool = inventoryItem0;
                break;
            case "inventoryItem1":
                targetPool = inventoryItem1;
                break;
            case "inventoryItem2":
                targetPool = inventoryItem2;
                break;
            case "inventoryItem3":
                targetPool = inventoryItem3;
                break;
            case "inventoryItem4":
                targetPool = inventoryItem4;
                break;
            case "inventoryItem5":
                targetPool = inventoryItem5;
                break;
            case "inventoryItem6":
                targetPool = inventoryItem6;
                break;
            case "inventoryItem7":
                targetPool = inventoryItem7;
                break;
            case "inventoryItem8":
                targetPool = inventoryItem8;
                break;
            case "inventoryItem9":
                targetPool = inventoryItem9;
                break;
            case "inventoryItem10":
                targetPool = inventoryItem10;
                break;
            case "inventoryItem11":
                targetPool = inventoryItem11;
                break;
            case "inventoryItem12":
                targetPool = inventoryItem12;
                break;
            case "inventoryItem13":
                targetPool = inventoryItem13;
                break;
            case "inventoryItem14":
                targetPool = inventoryItem14;
                break;
            case "inventoryItem15":
                targetPool = inventoryItem15;
                break;
            case "inventoryItem16":
                targetPool = inventoryItem16;
                break;
            case "inventoryItem17":
                targetPool = inventoryItem17;
                break;
            case "inventoryItem18":
                targetPool = inventoryItem18;
                break;

            // 펫
            case "Cat":
                targetPool = Cat;
                break;
            case "Duck":
                targetPool = Duck;
                break;
            case "Penguin":
                targetPool = Penguin;
                break;
            case "Sheep":
                targetPool = Sheep;
                break;
        }

        // 프리팹 생성
        for (int i = 0; i < targetPool.Length; i++)
        {
            if (!targetPool[i].activeSelf)
            {
                // 스킬 충돌시 생성되는 이펙트
                if(targetPool == AbilityArrow1Hit)
                {
                    // AbilityArrow1Hit
                    // 리스트에 저장
                    AbilityArrow1HitEffects.Add(targetPool[i]);

                    // 활성화
                    targetPool[i].SetActive(true);
                    return targetPool[i];
                }
                else if(targetPool == AbilityArrow2Hit)
                {
                    // AbilityArrow2Hit
                    // 리스트에 저장
                    AbilityArrow2HitEffects.Add(targetPool[i]);

                    // 활성화
                    targetPool[i].SetActive(true);
                    return targetPool[i];
                }
                else if(targetPool == AbilityMage1Hit || targetPool == AbilityMage1Hit2)
                {
                    // AbilityMage1Hit
                    // 리스트에 저장
                    AbilityMage1HitEffects.Add(targetPool[i]);

                    // 활성화
                    targetPool[i].SetActive(true);
                    return targetPool[i];
                }
                else if (targetPool == AbilityHolyknight1Hit)
                {
                    // AbilityHolyknight1Hit
                    // 리스트에 저장
                    AbilityHolyknight1HitEffects.Add(targetPool[i]);

                    // 활성화
                    targetPool[i].SetActive(true);
                    return targetPool[i];
                }
                else if (targetPool == AbilityArrow0Hit)
                {
                    // AbilityArrow0Hit
                    // 리스트에 저장
                    AbilityArrow0HitEffects.Add(targetPool[i]);

                    // 활성화
                    targetPool[i].SetActive(true);
                    return targetPool[i];
                }

                // 발사체
                if (targetPool == Arrow || targetPool == Carrot || targetPool == CiclopEye || targetPool == CarrotGroup || targetPool == MageMissile
                || targetPool == SwordComboAttack0 || targetPool == SwordComboAttack1 || targetPool == SwordComboAttack2
                || targetPool == BlacksmithComboAttack0 || targetPool == BlacksmithComboAttack1 || targetPool == BlacksmithComboAttack2
                || targetPool == HolyknightComboAttack0 || targetPool == HolyknightComboAttack1 || targetPool == HolyknightComboAttack2)
                {
                    // 발사체 유지시간 원래대로
                    if (targetPool == Arrow || targetPool == MageMissile
                    || targetPool == SwordComboAttack0 || targetPool == SwordComboAttack1 || targetPool == SwordComboAttack2
                    || targetPool == BlacksmithComboAttack0 || targetPool == BlacksmithComboAttack1 || targetPool == BlacksmithComboAttack2
                    || targetPool == HolyknightComboAttack0 || targetPool == HolyknightComboAttack1 || targetPool == HolyknightComboAttack2)
                    {
                        // 플레이어 발사체
                        targetPool[i].GetComponent<PlayerWeapon>().waitTime = 1f;
                    }
                    else
                    {
                        // 당근 및 키클로페스 눈
                        targetPool[i].GetComponent<Carrot>().waitTime = 2f;
                    }

                    // 활성화
                    targetPool[i].SetActive(true);
                    return targetPool[i];
                }

                // 텍스트 표시
                if (targetPool == DamageText || targetPool == HealingText || targetPool == MissText)
                {
                    // 텍스트가 사라지는 시간 원래대로
                    targetPool[i].GetComponent<FloatingText>().waitTime = 2f;

                    // 텍스트 알파값 원래대로
                    targetPool[i].GetComponent<FloatingText>().alpha.a = 1f;

                    // 활성화
                    targetPool[i].SetActive(true);
                    return targetPool[i];
                }

                // 체력바
                if (targetPool == MonsterHpBar)
                {
                    // 체력바리스트에 추가
                    templates.hpBars.Add(targetPool[i]);

                    // 활성화
                    targetPool[i].SetActive(true);
                    return targetPool[i];
                }

                // 아이템
                if (targetPool == PassiveSkill0 || targetPool == PassiveSkill1 || targetPool == PassiveSkill2 ||
                targetPool == PassiveSkill3 || targetPool == PassiveSkill4 || targetPool == PassiveSkill5 ||
                targetPool == PassiveSkill6 || targetPool == PassiveSkill7 || targetPool == PassiveSkill8 ||
                targetPool == PassiveSkill9 || targetPool == PassiveSkill10 || targetPool == PassiveSkill11 ||
                targetPool == PassiveSkill12 || targetPool == PassiveSkill13 || targetPool == PassiveSkill14 ||
                targetPool == PassiveSkill15 || targetPool == PassiveSkill16 || targetPool == PassiveSkill17 ||
                targetPool == PassiveSkill18 || targetPool == PassiveSkill19 || targetPool == PassiveSkill20 ||
                targetPool == PassiveSkill21 || targetPool == ActiveSkill0 || targetPool == ActiveSkill1 ||
                targetPool == Coin100 || targetPool == Coin500 || targetPool == Coin1000)
                {
                    // 아이템리스트에 추가
                    templates.items.Add(targetPool[i]);

                    // 플래그 원래대로
                    targetPool[i].GetComponent<Item>().hasItem = false;

                    // 활성화
                    targetPool[i].SetActive(true);
                    return targetPool[i];
                }

                // 보스
                if (targetPool == Boss0)
                {
                    // Enemy 스크립트를 가져온뒤
                    Enemy enemy = targetPool[i].GetComponent<Enemy>();

                    // 마지막방으로 이동
                    targetPool[i].transform.position = templates.rooms[templates.rooms.Count - 1].transform.position;

                    // 회전값 정규화
                    targetPool[i].transform.rotation = Quaternion.identity;

                    // 플래그
                    enemy.isDrop = false;
                    enemy.isDead = false;
                    enemy.isChase = true;
                    enemy.isAttack = false;
                    enemy.maxHealth = 5000 + 1000 * (templates.currentStage);
                    enemy.curHealth = 5000 + 1000 * (templates.currentStage);
                    targetPool[i].layer = 9;
                    targetPool[i].SetActive(true);
                    return targetPool[i];
                }

                // 몬스터
                if (targetPool == Normal0 || targetPool == Normal1 || targetPool == Normal2 || targetPool == Normal3 || targetPool == Normal4 || targetPool == Normal5 || targetPool == Normal6 || targetPool == Normal7 || targetPool == Normal8 || targetPool == Normal9 || targetPool == Normal10 || targetPool == Normal11 || targetPool == Normal12 || targetPool == Normal13 || targetPool == Normal14 || targetPool == Normal15 || targetPool == Normal16)
                {
                    // 랜덤 방
                    int random = Random.Range(1, templates.rooms.Count - 2);

                    // 방에서 랜덤벡터의 인덱스를 하나 뽑고
                    int randomMonsterVec = Random.Range(0, MonsterVec.Length);

                    // 인덱스에 해당하는 벡터를 넘겨주고
                    Vector3 randomVec = MonsterVec[randomMonsterVec];

                    // 포지션에 벡터를 더하기
                    targetPool[i].transform.position = templates.rooms[random].transform.position + randomVec;

                    // 회전값 정규화
                    targetPool[i].transform.rotation = Quaternion.identity;

                    // Enemy 스크립트
                    Enemy enemy = targetPool[i].GetComponent<Enemy>();
                    if (enemy.enemyType == Enemy.Type.Bat)
                    {
                        // 기본
                        // 스테이지가 높아질수록 체력 및 데미지가 증가
                        enemy.maxHealth = 1000 + 300 * (templates.currentStage);
                        enemy.curHealth = 1000 + 300 * (templates.currentStage);
                        enemy.damage = 100 + 50 * (templates.currentStage);
                    }
                    else if (enemy.enemyType == Enemy.Type.Bomb)
                    {
                        // 폭탄
                        // 스테이지가 높아질수록 체력 및 데미지가 증가
                        enemy.maxHealth = 100 + 100 * (templates.currentStage);
                        enemy.curHealth = 100 + 100 * (templates.currentStage);
                        enemy.damage = 300 + 200 * (templates.currentStage);

                        // 플래그
                        enemy.meleeArea.enabled = false;
                        targetPool[i].layer = 9;
                        enemy.isDead = false;
                    }
                    else if (enemy.enemyType == Enemy.Type.Golem)
                    {
                        // 돌진
                        // 스테이지가 높아질수록 체력 및 데미지가 증가
                        enemy.maxHealth = 2000 + 500 * (templates.currentStage);
                        enemy.curHealth = 2000 + 500 * (templates.currentStage);
                        enemy.damage = 200 + 100 * (templates.currentStage);
                    }
                    else if (enemy.enemyType == Enemy.Type.Rabbit)
                    {
                        // 토끼
                        // 스테이지가 높아질수록 체력 및 데미지가 증가
                        // 원거리 데미지는 플레이어 원거리피격에서 조절
                        enemy.maxHealth = 1500 + 500 * (templates.currentStage);
                        enemy.curHealth = 1500 + 500 * (templates.currentStage);
                    }

                    // 플래그
                    enemy.isDrop = false;
                    enemy.isAttack = false;
                    enemy.isChase = true;

                    // 활성화
                    targetPool[i].SetActive(true);
                    return targetPool[i];
                }

                // 방 모델
                if (targetPool == B || targetPool == BL || targetPool == LB ||
                targetPool == BLT || targetPool == LTB || targetPool == TBL ||
                targetPool == L || targetPool == LR || targetPool == RL ||
                targetPool == LTR || targetPool == TRL || targetPool == RLT ||
                targetPool == R || targetPool == RB || targetPool == BR ||
                targetPool == RBL || targetPool == BLR || targetPool == LRB ||
                targetPool == T || targetPool == TL || targetPool == LT ||
                targetPool == TB || targetPool == BT || targetPool == TR || targetPool == RT ||
                targetPool == TRB || targetPool == RBT || targetPool == BTR || targetPool == SecretRoom)
                {
                    // spawned는 맵의 하위에있는 spawnPoint가 가지고있음
                    Transform[] transforms = targetPool[i].GetComponentsInChildren<Transform>();
                    for (int j = 0; j < transforms.Length; j++)
                    {
                        if (transforms[j].name == "Spawn Point")
                        {
                            // 플래그
                            transforms[j].transform.GetComponent<RoomSpawner>().spawned = false;
                            transforms[j].transform.GetComponent<RoomSpawner>().waitTime = 0.1f;
                        }
                    }

                    // 플래그
                    targetPool[i].GetComponent<AddRoom>().isAdd = false;

                    // 활성화
                    targetPool[i].SetActive(true);
                    return targetPool[i];
                }

                // 시크릿박스
                if (targetPool == SecretBox)
                {
                    // 플래그
                    targetPool[i].GetComponent<SecretBox>().isAdd = false;

                    // 활성화
                    targetPool[i].SetActive(true);
                    return targetPool[i];
                }

                // 나머지
                if (!targetPool[i].activeSelf)
                {
                    // 활성화
                    targetPool[i].SetActive(true);
                    return targetPool[i];
                }
            }
        }
        return null;
    }

    void SetCanvas()
    {
        // 캔버스를 할당하는 함수
        // 캔버스 할당
        Canvas = GameObject.FindGameObjectWithTag("Canvas");

        // 체력바는 부모를 캔버스로 설정해준뒤 비활성화 해둔다
        for (int i = 0; i < MonsterHpBar.Length; i++)
        {
            MonsterHpBar[i] = Instantiate(MonsterHpBarPrefab);
            MonsterHpBar[i].transform.SetParent(Canvas.transform);
            MonsterHpBar[i].SetActive(false);
        }
    }
}
