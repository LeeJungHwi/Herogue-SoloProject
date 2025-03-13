using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

// 스탯
[System.Serializable]
public class CharacterStat
{
    public float curHealth, maxHealth;
    public float attackSpeed, damage, criticalDamage;
    public float accuracy, bloodDrain, barrier;
    public int criticalPercentage;
    public float secretPercentage, activeDropPercentage, passiveDropPercentage, inventoryItemPercentage;
}

// 장비
[System.Serializable]
public class CharacterEquip
{
    public bool isWeapon, isArmor, isGlove, isShoes, isAmulet, isPet; // 현재 장착된 상태 체크
    public InventoryItem equipedWeaponItem, equipedArmorItem, equipedGloveItem, equipedShoesItem, equipedAmuletItem, equipedPetItem; // 현재 장착된 장비 아이템
}

// 인벤토리
[System.Serializable]
public class CharacterInven
{
    public List<InventoryItem> inventoryItems = new List<InventoryItem>(); // 인벤토리
    public int inventorySlotExpansionCnt; // 인벤토리 확장 횟수
}

// 스킬
[System.Serializable]
public class CharacterSkill
{
    public string permanentSb; // 영구적인 스킬 설명
    public bool[] isPermanentSkill = new bool[22]; // 영구적인 스킬 체크
    public int permanentSkillCnt; // 영구적인 스킬 획득 횟수
    public bool[] isAbility = new bool[3]; // 액티브 스킬 체크
}

// 캐릭터 데이터
[System.Serializable]
public class CharacterData
{
    public Character character; // 캐릭터 타입
    public float x, y, z; // 캐릭터 위치
    public CharacterStat characterStat = new CharacterStat(); // 캐릭터 스탯
    public CharacterEquip characterEquip = new CharacterEquip(); // 캐릭터 장비
    public int coin; // 재화
    public CharacterInven characterInven = new CharacterInven(); // 인벤토리
    public CharacterSkill characterSkill = new CharacterSkill(); // 스킬
}

public class CharacterSave : SaveBase<CharacterData>
{ 
    public CharacterSave() : base("character.json") {}
    
    // 세이브
    public override void Save()
    {
        Player player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        // 캐릭터 타입
        saveData.character = DataManager.instance.character;

        // 캐릭터 위치
        saveData.x = player.transform.position.x;
        saveData.y = player.transform.position.y;
        saveData.z = player.transform.position.z;

        // 캐릭터 스탯
        saveData.characterStat.curHealth = player.curHealth;
        saveData.characterStat.maxHealth = player.maxHealth;
        saveData.characterStat.attackSpeed = player.attackSpeed;
        saveData.characterStat.damage = player.damage;
        saveData.characterStat.criticalDamage = player.criticalDamage;
        saveData.characterStat.accuracy = player.accuracy;
        saveData.characterStat.bloodDrain = player.bloodDrain;
        saveData.characterStat.barrier = player.barrier;
        saveData.characterStat.criticalPercentage = player.criticalPercentage;
        saveData.characterStat.secretPercentage = player.secretPercentage;
        saveData.characterStat.activeDropPercentage = player.activeDropPercentage;
        saveData.characterStat.passiveDropPercentage = player.passiveDropPercentage;
        saveData.characterStat.inventoryItemPercentage = player.inventoryItemPercentage;

        // 캐릭터 장비
        saveData.characterEquip.isWeapon = player.isWeapon;
        saveData.characterEquip.isArmor = player.isArmor;
        saveData.characterEquip.isGlove = player.isGlove;
        saveData.characterEquip.isShoes = player.isShoes;
        saveData.characterEquip.isAmulet = player.isAmulet;
        saveData.characterEquip.isPet = player.isPet;
        saveData.characterEquip.equipedWeaponItem = player.equipedWeaponItem;
        saveData.characterEquip.equipedArmorItem = player.equipedArmorItem;
        saveData.characterEquip.equipedGloveItem = player.equipedGloveItem;
        saveData.characterEquip.equipedShoesItem = player.equipedShoesItem;
        saveData.characterEquip.equipedAmuletItem = player.equipedAmuletItem;
        saveData.characterEquip.equipedPetItem = player.equipedPetItem;

        // 재화
        saveData.coin = player.coin;

        // 인벤토리
        saveData.characterInven.inventoryItems = player.GetComponent<Inventory>().inventoryItems;
        saveData.characterInven.inventorySlotExpansionCnt = GameObject.FindGameObjectWithTag("Canvas").GetComponent<InventoryUI>().inventorySlotExpansionCnt;

        // 스킬
        saveData.characterSkill.permanentSb = player.permanentSb.ToString();
        for(int i = 0; i < 22; i++) saveData.characterSkill.isPermanentSkill[i] = player.isPermanentSkill[i];
        saveData.characterSkill.permanentSkillCnt = player.permanentSkillCnt;
        for(int i = 0; i < 3; i++) saveData.characterSkill.isAbility[i] = player.isAbility[i];

        base.Save();
    }

    // 로드
    public override void Load()
    {
        Player player = GameObject.Find(DataManager.instance.character.ToString()).GetComponent<Player>();

        // 캐릭터 위치
        player.transform.position = new Vector3(loadData.x, loadData.y, loadData.z);

        // 캐릭터 스탯
        player.curHealth = loadData.characterStat.curHealth;
        player.maxHealth = loadData.characterStat.maxHealth;
        player.attackSpeed = loadData.characterStat.attackSpeed;
        player.damage = loadData.characterStat.damage;
        player.criticalDamage = loadData.characterStat.criticalDamage;
        player.accuracy = loadData.characterStat.accuracy;
        player.bloodDrain = loadData.characterStat.bloodDrain;
        player.barrier = loadData.characterStat.barrier;
        player.criticalPercentage = loadData.characterStat.criticalPercentage;
        player.secretPercentage = loadData.characterStat.secretPercentage;
        player.activeDropPercentage = loadData.characterStat.activeDropPercentage;
        player.passiveDropPercentage = loadData.characterStat.passiveDropPercentage;
        player.inventoryItemPercentage = loadData.characterStat.inventoryItemPercentage;

        // 캐릭터 장비
        player.isWeapon = loadData.characterEquip.isWeapon;
        player.isArmor = loadData.characterEquip.isArmor;
        player.isGlove = loadData.characterEquip.isGlove;
        player.isShoes = loadData.characterEquip.isShoes;
        player.isAmulet = loadData.characterEquip.isAmulet;
        player.isPet = loadData.characterEquip.isPet;
        if(player.isWeapon)
        {
            player.equipedWeaponItem = loadData.characterEquip.equipedWeaponItem;
            if(player.equipedWeaponItem.Effects[0] is InventoryItemEquipEffect effect) effect.UpdateEquipSlotImage(player, player.equipedWeaponItem);
        }
        if(player.isArmor)
        {
            player.equipedArmorItem = loadData.characterEquip.equipedArmorItem;
            if(player.equipedArmorItem.Effects[0] is InventoryItemEquipEffect effect) effect.UpdateEquipSlotImage(player, player.equipedArmorItem);
        }
        if(player.isGlove)
        {
            player.equipedGloveItem = loadData.characterEquip.equipedGloveItem;
            if(player.equipedGloveItem.Effects[0] is InventoryItemEquipEffect effect) effect.UpdateEquipSlotImage(player, player.equipedGloveItem);
        }
        if(player.isShoes)
        {
            player.equipedShoesItem = loadData.characterEquip.equipedShoesItem;
            if(player.equipedShoesItem.Effects[0] is InventoryItemEquipEffect effect) effect.UpdateEquipSlotImage(player, player.equipedShoesItem);
        }
        if(player.isAmulet)
        {
            player.equipedAmuletItem = loadData.characterEquip.equipedAmuletItem;
            if(player.equipedAmuletItem.Effects[0] is InventoryItemEquipEffect effect) effect.UpdateEquipSlotImage(player, player.equipedAmuletItem);
        }
        if(player.isPet)
        {
            player.equipedPetItem = loadData.characterEquip.equipedPetItem;
            if(player.equipedPetItem.Effects[0] is InventoryItemEquipEffect effect)
            {
                effect.UpdateEquipSlotImage(player, player.equipedPetItem);
                if(player.spawnedPet == null) effect.PetSpawn(player, player.equipedPetItem);
            }
        }

        // 재화
        player.coin = loadData.coin;

        // 인벤토리
        SaveManager.instance.StartCoroutine(InvenLoad(player));

        // 스킬
        player.permanentSb = new StringBuilder(loadData.characterSkill.permanentSb);
        player.permanentSkillListText.text = player.permanentSb.ToString();
        for(int i = 0; i < 22; i++) player.isPermanentSkill[i] = loadData.characterSkill.isPermanentSkill[i];
        player.permanentSkillCnt = loadData.characterSkill.permanentSkillCnt;
        for(int i = 0; i < 3; i++)
        {
            player.isAbility[i] = loadData.characterSkill.isAbility[i];
            player.abilityLock[i].SetActive(!player.isAbility[i]);
        }
    }

    // 캐릭터 타입 로드 => 로드 버튼을 누르면 캐릭터 타입을 로드하고 데이터 매니저의 선택된 캐릭터로 넘김 => 게임 씬에서 게임 매니저가 전체 로드
    public bool CharacterTypeLoad()
    {
        // 저장된 파일이 없으면 X
        if(!File.Exists(path)) return false;

        base.Load();

        DataManager.instance.character = loadData.character;
        SaveManager.instance.isLoad = true;
        
        return true;
    }

    // 인벤토리 로드 => 슬롯이 할당될 때 까지 대기
    private IEnumerator InvenLoad(Player player)
    {
        // 인벤토리 아이템
        player.GetComponent<Inventory>().inventoryItems = loadData.characterInven.inventoryItems;

        // 인벤토리 슬롯 확장
        InventoryUI inventoryUI = GameObject.Find(DataManager.instance.character.ToString() + "Canvas").GetComponent<InventoryUI>();
        yield return new WaitUntil(() => inventoryUI.inventorySlots != null);
        inventoryUI.RedrawSlotUI();
        inventoryUI.inventorySlotExpansionCnt = SaveManager.instance.characterSave.loadData.characterInven.inventorySlotExpansionCnt;
        player.GetComponent<Inventory>().InventorySlotCnt = (inventoryUI.inventorySlotExpansionCnt + 1) * 4;
    }
}
