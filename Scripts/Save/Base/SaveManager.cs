using UnityEngine;

// 모든 세이브 로드 관리
public class SaveManager : MonoBehaviour
{
    // 싱글톤
    public static SaveManager instance;
    private void Awake()
    {
        if(instance != null) return;
        instance = this;
        DontDestroyOnLoad(gameObject);

        characterSave = new CharacterSave();
        questSave = new QuestSave();
        optionSave = new OptionSave();
    }
    [HideInInspector] public bool isLoad = false; // 로드인 경우 => 게임 씬에서 게임 매니저가 전체 로드

    // 데이터 유형
    [HideInInspector] public CharacterSave characterSave;
    private QuestSave questSave;
    private OptionSave optionSave;

    // 전체 세이브
    public void SaveAll()
    {
        characterSave.Save();
        questSave.Save();
        optionSave.Save();
    }

    // 전체 로드
    public void LoadAll()
    {
        characterSave.Load();
        questSave.Load();
        optionSave.Load();
    }
}
