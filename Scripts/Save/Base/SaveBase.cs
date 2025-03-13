using System.IO;
using UnityEngine;

// 세이브 베이스 => 모든 데이터 유형 공통 멤버 정의
public class SaveBase<T> where T : new()
{
    protected string path; // 저장 경로
    protected T saveData = new T(); // 저장 할 데이터
    protected T loadData = new T(); // 로드된 데이터

    // 생성될 때 JSON 파일 저장 경로 설정
    public SaveBase(string fileName) { path = Application.persistentDataPath + "/" + fileName; }

    // 세이브 => saveData를 JSON 파일로 저장 => 하위클래스에서 게임 데이터를 saveData에 할당하고 base.Save 호출
    public virtual void Save()
    {
        string json = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(path, json); // 파일이 없으면 새로 생성되고 파일이 있으면 덮어씀
    }

    // 로드 => 저장된 JSON 데이터를 loadData에 로드 => 하위클래스에서 base.Load를 호출해 가져온 loadData를 게임 데이터에 할당
    public virtual void Load()
    {
        string json = File.ReadAllText(path);
        loadData = JsonUtility.FromJson<T>(json);
    }
}
