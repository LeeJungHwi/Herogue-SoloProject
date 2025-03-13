using UnityEngine;

// 기타 데이터
[System.Serializable]
public class OptionData
{
    public float gameSpeed; // 게임 속도
    public float bgmVolume, sfxVolume; // 게임 사운드
    public float passedTime; // 게임 시간
}

public class OptionSave : SaveBase<OptionData>
{
    public OptionSave() : base("option.json") {}

    // 세이브
    public override void Save()
    {
        // 게임 속도
        saveData.gameSpeed = DataManager.instance.gameSpeed;

        // 게임 사운드
        saveData.bgmVolume = SoundManager.instance.bgmVolume;
        saveData.sfxVolume = SoundManager.instance.sfxVolume;

        // 게임 시간
        ControlSky controlSky = GameObject.Find(DataManager.instance.character.ToString()).GetComponent<ControlSky>();
        saveData.passedTime = controlSky.passedTime;

        base.Save();
    }

    // 로드
    public override void Load()
    {
        base.Load();

        // 게임 속도
        DataManager.instance.gameSpeed = loadData.gameSpeed;

        // 게임 사운드
        SoundManager.instance.bgmVolume = loadData.bgmVolume;
        SoundManager.instance.sfxVolume = loadData.sfxVolume;

        // 게임 시간
        ControlSky controlSky = GameObject.Find(DataManager.instance.character.ToString()).GetComponent<ControlSky>();
        controlSky.passedTime = loadData.passedTime;
    }
}
