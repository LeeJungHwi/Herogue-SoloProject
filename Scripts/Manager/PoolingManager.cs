using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// 생성할 프리팹 타입 -> 키로 사용
public enum ObjType
{
    // 무기
    화살, 당근, 당근그룹, 키클롭스눈, 법사미사일,

    // 패시브 스킬
    한발노리기, 백발백중, 방벽, 초월방벽, 흡혈귀, 거울, 도박꾼, 저거너트, 즉사, 시크릿, 혈액갑옷,
    버티기, 구사일생, 폭발적치유, HP부스트, 광전사, 근심, 고동, 꿈의끝, 분신, 향상된대쉬, 불사신,

    // 액티브 스킬
    멀티샷, 사선화살,

    // 코인
    백원, 오백원, 천원,

    // 시크릿 박스
    시크릿박스,

    // 몬스터
    보스1, 보스2, 보스3,
    기본몬스터1, 기본몬스터2, 기본몬스터3, 기본몬스터4, 기본몬스터5, 기본몬스터6, 기본몬스터7, 기본몬스터8, 기본몬스터9,
    기본몬스터10, 기본몬스터11, 기본몬스터12, 기본몬스터13, 기본몬스터14, 기본몬스터15, 기본몬스터16, 기본몬스터17,

    // 랜덤맵
    B, BL, LB, BLT, LTB, TBL, L, LR, RL, LTR, TRL, RLT, R, RB, BR, RBL, BLR, LRB, SecretRoom, T, TB, BT, TL, LT, TR, RT, TRB, RBT, BTR,

    // 던전 장식
    던전장식1, 던전장식2, 던전장식3, 던전장식4, 던전장식5, 던전장식6, 던전장식7, 던전장식8, 던전장식9, 던전장식10, 던전장식11, 던전장식12, 던전장식13,
    던전장식14, 던전장식15, 던전장식16, 던전장식17, 던전장식18, 던전장식19, 던전장식20, 던전장식21, 던전장식22, 던전장식23, 던전장식24, 던전장식25,
    던전장식26, 던전장식27, 던전장식28, 던전장식29,

    // 이펙트
    플레이어공격이펙트, 플레이어크리티컬공격이펙트,
    궁수스킬1이펙트, 궁수스킬1충돌이펙트, 궁수스킬2이펙트, 궁수스킬2충돌이펙트, 궁수스킬3이펙트, 궁수스킬3충돌이펙트,
    전사스킬1이펙트, 전사스킬2이펙트, 전사스킬3이펙트,
    법사스킬1이펙트, 법사스킬2이펙트, 법사스킬2충돌1이펙트, 법사스킬2충돌2이펙트, 법사스킬3이펙트,
    블랙스미스스킬1이펙트, 블랙스미스스킬2이펙트, 블랙스미스스킬3이펙트,
    전사콤보공격1이펙트, 전사콤보공격2이펙트, 전사콤보공격3이펙트, 블랙스미스콤보공격1이펙트, 블랙스미스콤보공격2이펙트, 블랙스미스콤보공격3이펙트,
    성기사콤보공격1이펙트, 성기사콤보공격2이펙트, 성기사콤보공격3이펙트,
    성기사스킬1이펙트, 성기사스킬2이펙트, 성기사스킬2충돌이펙트, 성기사스킬3이펙트,

    // 체력바
    몬스터체력바,

    // 텍스트 표시
    데미지텍스트, 회복텍스트, 회피텍스트,

    // 사운드
    슈팅소리, 무빙소리, 대쉬소리, 아이템소리, 기본공격소리, 크리티컬공격소리, 방벽소리, 박스소리,
    시크릿문소리, 궁수스킬13소리, 궁수스킬1충돌소리, 궁수스킬2소리, 궁수스킬3충돌소리,
    플레이어피격소리, 버튼소리, 포션사용소리, 장비장착소리, 장비장착해제소리, 장비장착실패소리,
    칼소리, 전사스킬1소리, 전사스킬2소리, 전사스킬3소리, 전사스킬3충돌소리,
    법사스킬1소리, 법사스킬2소리, 법사스킬3소리, 법사미사일소리,
    블랙스미스스킬1소리, 블랙스미스스킬2소리, 블랙스미스스킬2충돌소리, 블랙스미스스킬3소리,
    해머소리, 성기사스킬1소리, 성기사스킬2소리, 성기사스킬3소리,

    // 인벤토리 아이템
    화려한장신구, 녹슨장신구, 단단한갑옷, 부서진갑옷, 큰체력물약,
    강력한활, 낡은활, 매끈한장갑, 찢어진장갑, 중간체력물약,
    깔끔한신발, 더러운신발, 작은체력물약, 강력한소드, 낡은소드,
    강력한지팡이, 낡은지팡이, 강력한해머, 낡은해머,

    // 펫
    고양이, 오리, 펭귄, 양
}

public class PoolingManager : MonoBehaviour
{
    // 기타 필드
    // 방 모델
    public RoomTemplates templates;

    // RoomBFS
    public RoomBFS roomBFS;

    // 방내에서 몬스터 랜덤배치를 위한 벡터
    public Vector3[] MonsterVec;

    // 캔버스
    public GameObject Canvas;

    // 텍스트 표시
    public GameObject FloatingText;

    // 풀셋 -> 부모를 캔버스로 둬야하는 UI를 제외한 프리팹은 부모를 풀셋으로
    public GameObject poolSet; 

    // AbilityArrow1Hit를 저장 할 리스트
    public List<Tuple<GameObject, ObjType>> AbilityArrow1HitEffects = new List<Tuple<GameObject, ObjType>>();

    // AbilityArrow2Hit를 저장 할 리스트
    public List<Tuple<GameObject, ObjType>> AbilityArrow2HitEffects = new List<Tuple<GameObject, ObjType>>();

    // AbilityMage1Hit과 AbilityMage1Hit2를 저장 할 리스트
    public List<Tuple<GameObject, ObjType>> AbilityMage1HitEffects = new List<Tuple<GameObject, ObjType>>();

    // AbilityHolyknight1Hit을 저장 할 리스트
    public List<Tuple<GameObject, ObjType>> AbilityHolyknight1HitEffects = new List<Tuple<GameObject, ObjType>>();

    // AbilityArrow0Hit을 저장 할 리스트
    public List<Tuple<GameObject, ObjType>> AbilityArrow0HitEffects = new List<Tuple<GameObject, ObjType>>();

    // 생성할 프리팹 -> 인스펙터에서 할당

    // 모든 프리팹 리스트 저장
    public List<List<GameObject> > Prefs = new List<List<GameObject> >();

    // 무기
    public List<GameObject> WeaponPrefs = new List<GameObject>();

    // 패시브 스킬
    public List<GameObject> PassiveSkillPrefs = new List<GameObject>();

    // 액티브 스킬
    public List<GameObject> ActiveSkillPrefs = new List<GameObject>();
    
    // 코인
    public List<GameObject> CoinPrefs = new List<GameObject>();

    // 시크릿 박스
    public List<GameObject> SecretBoxPrefs = new List<GameObject>();

    // 몬스터
    public List<GameObject> MonsterPrefs = new List<GameObject>();

    // 랜덤맵 모델
    public List<GameObject> RandomMapPrefs = new List<GameObject>();

    // 던전 장식
    public List<GameObject> DungeonDecoPrefs = new List<GameObject>();

    // 이펙트
    public List<GameObject> EffectPrefs = new List<GameObject>();

    // 체력바
    public List<GameObject> MonsterHpBarPrefs = new List<GameObject>();

    // 텍스트 표시
    public List<GameObject> FloationTextPrefs = new List<GameObject>();

    // 사운드
    public List<GameObject> SoundPrefs = new List<GameObject>();
    
    // 인벤토리 아이템
    public List<GameObject> InventoryItemPrefs = new List<GameObject>();

    // 펫
    public List<GameObject> PetPrefs = new List<GameObject>();

    void Awake()
    {
        // 캔버스 할당
        Canvas = GameObject.Find(DataManager.instance.character.ToString() + "Canvas");

        // 모든 프리팹 리스트 저장
        Pref();

        // (타입, 프리팹) 맵핑
        Map();

        // (타입, 큐) 맵핑
        Gen();
    }

    // 모든 프리팹 리스트 저장
    private void Pref()
    {
        Prefs.Add(WeaponPrefs);
        Prefs.Add(PassiveSkillPrefs);
        Prefs.Add(ActiveSkillPrefs);
        Prefs.Add(CoinPrefs);
        Prefs.Add(SecretBoxPrefs);
        Prefs.Add(MonsterPrefs);
        Prefs.Add(RandomMapPrefs);
        Prefs.Add(DungeonDecoPrefs);
        Prefs.Add(EffectPrefs);
        Prefs.Add(MonsterHpBarPrefs);
        Prefs.Add(FloationTextPrefs);
        Prefs.Add(SoundPrefs);
        Prefs.Add(InventoryItemPrefs);
        Prefs.Add(PetPrefs);
    }

    // (타입, 프리팹) 맵핑
    private Dictionary<ObjType, GameObject> genPref = new Dictionary<ObjType, GameObject>();

    // (타입, 큐) 맵핑
    private Dictionary<ObjType, Queue<GameObject>> poolPref = new Dictionary<ObjType, Queue<GameObject>>();

    // (타입, 프리팹) 맵핑
    private void Map()
    {
        // 현재 맵핑할 오브젝트타입에 따라 프리팹 맵핑
        ObjType curObjType = 0;

        for(int i = 0; i < Prefs.Count; i++)
        {
            for(int j = 0; j < Prefs[i].Count; j++)
            {
                genPref.Add(curObjType, Prefs[i][j]);
                curObjType++;
            }
        }
    }

    // (타입, 큐) 맵핑
    private void Gen(int count = 150)
    {
        // 정의된 타입을 하나씩 가져와서
        foreach (ObjType type in Enum.GetValues(typeof(ObjType)))
        {
            Queue<GameObject> queue = new Queue<GameObject>();
            GameObject prefab = genPref[type];

            // count 개 생성하고 비활성화 후 큐에 저장
            for (int i = 0; i < count; i++)
            {
                // 프리팹 생성해서
                GameObject obj = Instantiate(prefab);

                // 비활성화전에 따로 처리해줘야 하는 프리팹 처리
                if(type == ObjType.데미지텍스트 || type == ObjType.회복텍스트 || type == ObjType.회피텍스트 || type == ObjType.몬스터체력바)
                {
                    GenSubTask(type, obj);
                }
                else // 부모를 캔버스로 둬야하는 UI를 제외한 프리팹은 부모를 풀셋으로
                {
                    obj.transform.SetParent(poolSet.transform);
                }

                // 오브젝트 비활성화
                obj.SetActive(false);
                queue.Enqueue(obj);
            }

            // (타입, 큐) 맵핑
            poolPref.Add(type, queue);
        }
    }

    // 풀에서 꺼냄
    public GameObject GetObj(ObjType type)
    {
        // 키가 존재하고 큐에 오브젝트가 있으면 꺼냄
        if (poolPref.ContainsKey(type) && poolPref[type].Count > 0)
        {
            // 오브젝트 꺼내서 
            GameObject obj = poolPref[type].Dequeue();

            // 활성화전에 따로 처리해줘야 하는 프리팹 처리
            if(type == ObjType.궁수스킬2충돌이펙트 || type == ObjType.궁수스킬3충돌이펙트 || type == ObjType.법사스킬2충돌1이펙트 || type == ObjType.법사스킬2충돌2이펙트
            || type == ObjType.성기사스킬2충돌이펙트 || type == ObjType.궁수스킬1충돌이펙트 || type == ObjType.화살 ||type == ObjType.당근
            || type == ObjType.키클롭스눈 || type == ObjType.당근그룹 || type == ObjType.법사미사일 || type == ObjType.전사콤보공격1이펙트 
            || type == ObjType.전사콤보공격2이펙트 || type == ObjType.전사콤보공격3이펙트 || type == ObjType.블랙스미스콤보공격1이펙트 || type == ObjType.블랙스미스콤보공격2이펙트 
            || type == ObjType.블랙스미스콤보공격3이펙트 || type == ObjType.성기사콤보공격1이펙트 || type == ObjType.성기사콤보공격2이펙트 || type == ObjType.성기사콤보공격3이펙트
            || type == ObjType.데미지텍스트 || type == ObjType.회복텍스트 || type == ObjType.회피텍스트 || type == ObjType.몬스터체력바 
            || type == ObjType.한발노리기 || type == ObjType.백발백중 || type == ObjType.방벽 || type == ObjType.초월방벽 
            || type == ObjType.흡혈귀 || type == ObjType.거울 || type == ObjType.도박꾼 || type == ObjType.저거너트 
            || type == ObjType.즉사 || type == ObjType.시크릿 || type == ObjType.혈액갑옷 || type == ObjType.버티기 
            || type == ObjType.구사일생 || type == ObjType.폭발적치유 || type == ObjType.HP부스트 || type == ObjType.광전사 
            || type == ObjType.근심 || type == ObjType.고동 || type == ObjType.꿈의끝 || type == ObjType.분신 
            || type == ObjType.향상된대쉬 || type == ObjType.불사신 || type == ObjType.멀티샷 || type == ObjType.사선화살 
            || type == ObjType.백원 || type == ObjType.오백원 || type == ObjType.천원 || type == ObjType.보스1 
            || type == ObjType.기본몬스터1 || type == ObjType.기본몬스터2 || type == ObjType.기본몬스터3 || type == ObjType.기본몬스터4 
            || type == ObjType.기본몬스터5 || type == ObjType.기본몬스터6 || type == ObjType.기본몬스터7 || type == ObjType.기본몬스터8
            || type == ObjType.기본몬스터9 || type == ObjType.기본몬스터10 || type == ObjType.기본몬스터11 || type == ObjType.기본몬스터12
            || type == ObjType.기본몬스터13 || type == ObjType.기본몬스터14 || type == ObjType.기본몬스터15 || type == ObjType.기본몬스터16 || type == ObjType.기본몬스터17
            || type == ObjType.B || type == ObjType.BL || type == ObjType.LB || type == ObjType.BLT || type == ObjType.LTB 
            || type == ObjType.TBL || type == ObjType.L || type == ObjType.LR || type == ObjType.RL || type == ObjType.LTR 
            || type == ObjType.TRL || type == ObjType.RLT || type == ObjType.R || type == ObjType.RB || type == ObjType.BR 
            || type == ObjType.RBL || type == ObjType.BLR || type == ObjType.LRB || type == ObjType.T || type == ObjType.TL 
            || type == ObjType.LT || type == ObjType.TB || type == ObjType.BT || type == ObjType.TR || type == ObjType.RT 
            || type == ObjType.TRB || type == ObjType.RBT || type == ObjType.BTR || type == ObjType.SecretRoom || type == ObjType.시크릿박스)
            {
                GetObjSubTask(type, obj);
            }

            // 오브젝트 활성화
            obj.SetActive(true);
            return obj;
        }

        // 사용 가능한 오브젝트가 없는 경우 -> 널 에러가 발생하지않게 꺼내쓸때 null 검사를 하던가 Gen에 충분한 값을 넘겨줌
        return null;
    }

    // 풀에 반환함
    public void ReturnObj(GameObject obj, ObjType type)
    {
        // 비활성화
        obj.SetActive(false);

        // 해당 타입의 풀로 반환
        poolPref[type].Enqueue(obj);
    }

    // 비활성화전에 따로 처리해줘야 하는 프리팹 처리
    void GenSubTask(ObjType type, GameObject obj)
    {
        switch(type)
        {
            // 플로팅텍스트는 부모를 빈오브젝트로 설정해준뒤 비활성화
            case ObjType.데미지텍스트:
            case ObjType.회복텍스트:
            case ObjType.회피텍스트:
                obj.transform.SetParent(FloatingText.transform);
                return;
            
            // 몬스터체력바는 부모를 캔버스로 설정해준뒤 비활성화
            case ObjType.몬스터체력바:
                obj.transform.SetParent(Canvas.transform.GetChild(0));
                return;
        }
    }

    // 활성화전에 따로 처리해줘야 하는 프리팹 처리
    private void GetObjSubTask(ObjType type, GameObject obj)
    {
        switch(type)
        {
            // 스킬 충돌시 생성되는 이펙트는 리스트에 저장
            case ObjType.궁수스킬2충돌이펙트:
                AbilityArrow1HitEffects.Add(Tuple.Create(obj, type)); // 리스트에 저장
                return;
            case ObjType.궁수스킬3충돌이펙트:
                AbilityArrow2HitEffects.Add(Tuple.Create(obj, type)); // 리스트에 저장
                return;
            case ObjType.법사스킬2충돌1이펙트:
            case ObjType.법사스킬2충돌2이펙트:
                AbilityMage1HitEffects.Add(Tuple.Create(obj, type)); // 리스트에 저장
                return;
            case ObjType.성기사스킬2충돌이펙트:
                AbilityHolyknight1HitEffects.Add(Tuple.Create(obj, type)); // 리스트에 저장
                return;     
            case ObjType.궁수스킬1충돌이펙트:
                AbilityArrow0HitEffects.Add(Tuple.Create(obj, type)); // 리스트에 저장
                return;
            
            // 발사체는 발사체 유지시간 원래대로
            case ObjType.화살:
            case ObjType.법사미사일:
            case ObjType.전사콤보공격1이펙트:
            case ObjType.전사콤보공격2이펙트:
            case ObjType.전사콤보공격3이펙트:
            case ObjType.블랙스미스콤보공격1이펙트:
            case ObjType.블랙스미스콤보공격2이펙트:
            case ObjType.블랙스미스콤보공격3이펙트:
            case ObjType.성기사콤보공격1이펙트:
            case ObjType.성기사콤보공격2이펙트:
            case ObjType.성기사콤보공격3이펙트:
                obj.GetComponent<PlayerWeapon>().waitTime = 1f;
                return;
            case ObjType.당근:
            case ObjType.키클롭스눈:
            case ObjType.당근그룹:
                obj.GetComponent<Carrot>().waitTime = 2f;
                return;

            // 텍스트 표시는 사라지는시간, 알파값 원래대로
            case ObjType.데미지텍스트:
            case ObjType.회복텍스트:
            case ObjType.회피텍스트:
                obj.GetComponent<FloatingText>().waitTime = 2f;
                obj.GetComponent<FloatingText>().alpha.a = 1f;
                return;
            
            // 체력바는 리스트에 저장
            case ObjType.몬스터체력바:
                templates.hpBars.Add(obj);
                return;

            // 아이템은 리스트에 저장, 플래그 원래대로
            case ObjType.한발노리기:
            case ObjType.백발백중:
            case ObjType.방벽:
            case ObjType.초월방벽:
            case ObjType.흡혈귀:
            case ObjType.거울:
            case ObjType.도박꾼:
            case ObjType.저거너트:
            case ObjType.즉사:
            case ObjType.시크릿:
            case ObjType.혈액갑옷:
            case ObjType.버티기:
            case ObjType.구사일생:
            case ObjType.폭발적치유:
            case ObjType.HP부스트:
            case ObjType.광전사:
            case ObjType.근심:
            case ObjType.고동:
            case ObjType.꿈의끝:
            case ObjType.분신:
            case ObjType.향상된대쉬:
            case ObjType.불사신:
            case ObjType.멀티샷:
            case ObjType.사선화살:
            case ObjType.백원:
            case ObjType.오백원:
            case ObjType.천원:
                templates.items.Add(obj);
                obj.GetComponent<Item>().hasItem = false;
                return;

            // 보스 처리
            case ObjType.보스1:
                Enemy enemyBoss = obj.GetComponent<Enemy>();
                //obj.transform.position = templates.rooms[templates.rooms.Count - 1].Item1.transform.position; // 마지막방으로 이동
                obj.transform.position = roomBFS.maxDisPos; // 시작방에서 가장 먼 방으로 이동
                obj.transform.rotation = Quaternion.identity; // 회전값 정규화
                enemyBoss.isDrop = false; // 플래그
                enemyBoss.isDead = false;
                enemyBoss.isChase = true;
                enemyBoss.isAttack = false;
                enemyBoss.maxHealth = 5000 + 1000 * (templates.currentStage); // 체력
                enemyBoss.curHealth = 5000 + 1000 * (templates.currentStage);
                obj.layer = 9; // 레이어
                return;

            // 몬스터 처리
            case ObjType.기본몬스터1:
            case ObjType.기본몬스터2:
            case ObjType.기본몬스터3:
            case ObjType.기본몬스터4:
            case ObjType.기본몬스터5:
            case ObjType.기본몬스터6:
            case ObjType.기본몬스터7:
            case ObjType.기본몬스터8:
            case ObjType.기본몬스터9:
            case ObjType.기본몬스터10:
            case ObjType.기본몬스터11:
            case ObjType.기본몬스터12:
            case ObjType.기본몬스터13:
            case ObjType.기본몬스터14:
            case ObjType.기본몬스터15:
            case ObjType.기본몬스터16:
            case ObjType.기본몬스터17:
                int random = UnityEngine.Random.Range(1, templates.rooms.Count - 2); // 랜덤 방
                int randomMonsterVec = UnityEngine.Random.Range(0, MonsterVec.Length); // 방에서 랜덤벡터의 인덱스를 하나 뽑고
                Vector3 randomVec = MonsterVec[randomMonsterVec]; // 인덱스에 해당하는 벡터를 넘겨주고
                obj.transform.position = templates.rooms[random].Item1.transform.position + randomVec; // 포지션에 벡터를 더하기
                obj.transform.rotation = Quaternion.identity; // 회전값 정규화
                Enemy enemyNormal = obj.GetComponent<Enemy>();

                // 스테이지에 따라 스탯 조정
                if (enemyNormal.enemyType == Type.Bat) // 기본
                {
                    enemyNormal.maxHealth = 1000 + 300 * (templates.currentStage);
                    enemyNormal.curHealth = 1000 + 300 * (templates.currentStage);
                    enemyNormal.damage = 100 + 50 * (templates.currentStage);
                }
                else if (enemyNormal.enemyType == Type.Bomb) // 폭탄
                {
                    enemyNormal.maxHealth = 100 + 100 * (templates.currentStage);
                    enemyNormal.curHealth = 100 + 100 * (templates.currentStage);
                    enemyNormal.damage = 300 + 200 * (templates.currentStage);

                    // 플래그
                    enemyNormal.meleeArea.enabled = false;
                    obj.layer = 9;
                    enemyNormal.isDead = false;
                }
                else if (enemyNormal.enemyType == Type.Golem) // 돌진
                {
                    enemyNormal.maxHealth = 2000 + 500 * (templates.currentStage);
                    enemyNormal.curHealth = 2000 + 500 * (templates.currentStage);
                    enemyNormal.damage = 200 + 100 * (templates.currentStage);
                }
                else if (enemyNormal.enemyType == Type.Rabbit) // 토끼
                {
                    // 원거리 데미지는 플레이어 원거리피격에서 조절
                    enemyNormal.maxHealth = 1500 + 500 * (templates.currentStage);
                    enemyNormal.curHealth = 1500 + 500 * (templates.currentStage);
                }

                enemyNormal.isDrop = false; // 플래그
                enemyNormal.isAttack = false;
                enemyNormal.isChase = true;

                return;

            // 방모델 처리
            case ObjType.B:
            case ObjType.BL:
            case ObjType.LB:
            case ObjType.BLT:
            case ObjType.LTB:
            case ObjType.TBL:
            case ObjType.LR:
            case ObjType.RL:
            case ObjType.LTR:
            case ObjType.TRL:
            case ObjType.RLT:
            case ObjType.R:
            case ObjType.RB:
            case ObjType.BR:
            case ObjType.RBL:
            case ObjType.BLR:
            case ObjType.LRB:
            case ObjType.T:
            case ObjType.TL:
            case ObjType.LT:
            case ObjType.TB:
            case ObjType.BT:
            case ObjType.TR:
            case ObjType.RT:
            case ObjType.TRB:
            case ObjType.RBT:
            case ObjType.BTR:
            case ObjType.SecretRoom:
                // spawned는 맵의 하위에있는 spawnPoint가 가지고있음
                Transform[] transforms = obj.GetComponentsInChildren<Transform>();
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
                obj.GetComponent<AddRoom>().isAdd = false;

                return;

            // 시크릿박스 처리
            case ObjType.시크릿박스:
                obj.GetComponent<SecretBox>().isAdd = false;
                return;
        }
    }
}
