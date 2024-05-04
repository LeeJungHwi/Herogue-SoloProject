using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 메인화면, 게임화면에서 사운드를 관리해주는 매니저
public class SoundManager : MonoBehaviour
{
    // 싱글톤
    public static SoundManager instance;
    private void Awake()
    {
        if(instance == null)
        {
            // 사운드매니저 할당
            instance = this;

            // 씬전환시 파괴되지 않게
            DontDestroyOnLoad(instance);

            // 메인에서 배경음 및 효과음 볼륨을 초기화
            bgmVolume = 0.1f;
            sfxVolume = 1f;

            // 메인 배경음악
            BgmSoundPlay(bgmList[0]);

            // 화면 꺼짐 방지
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
        }
        else
        {
            // 씬전환시 이미 있으면 파괴
            Destroy(gameObject);
        }
    }

    // 배경음악을 재생하는 근원지
    [SerializeField] private AudioSource bgmSound;

    // 배경음악 리스트
    public AudioClip[] bgmList;

    // 던전배경음악 리스트
    public AudioClip[] dungeonBgmList;

    // 메인화면 효과음 리스트
    // 메인화면 버튼클릭 사운드는 풀링을 하지않았음
    // 게임화면에서의 모든 사운드는 풀링을 이용
    public AudioClip[] mainSfxList;

    // 랜덤
    private int random;

    // 배경음 볼륨을 저장 할 변수
    [HideInInspector] public float bgmVolume;

    // 효과음 볼륨을 저장 할 변수
    [HideInInspector] public float sfxVolume;

    // 메인화면 효과음
    public void MainSFXPlay(string sfxName, AudioClip clip)
    {
        // 효과음이름 오브젝트 생성
        GameObject go = new GameObject(sfxName);

        // 생성된 오브젝트가 효과음을 재생하는 근원지
        AudioSource audioSource = go.AddComponent<AudioSource>();

        // 음원 지정
        audioSource.clip = clip;

        // 음원 볼륨 조정
        audioSource.volume = sfxVolume;

        // 지정된 음원 재생
        audioSource.Play();

        // 음원이 끝나면 삭제
        Destroy(go, clip.length);
    }

    // 게임화면 효과음
    public void SFXPlay(ObjType type)
    {
        // 사운드 풀링
        PoolingManager poolingManager = GameObject.FindGameObjectWithTag("PoolManager").GetComponent<PoolingManager>();
        GameObject instantSfx = poolingManager.GetObj(type);

        // 오디오소스
        AudioSource audioSource = instantSfx.GetComponent<AudioSource>();

        // 음원 볼륨 조정
        audioSource.volume = sfxVolume;
    }

    public void BgmSoundPlay(AudioClip clip)
    {
        // 배경음이 발생되는 근원지에 음원 지정
        bgmSound.clip = clip;

        // 반복 재생
        bgmSound.loop = true;

        // 음원 볼륨 조정
        bgmSound.volume = bgmVolume;

        // 음원 재생
        bgmSound.Play();
    }

    public void SetBgmVolume(float volume)
    {
        // 배경음 조절 함수
        // 슬라이더 값에따라 볼륨 적용
        bgmSound.volume = volume;

        // 슬라이더 값을 변수에 저장해서 배경음악을 실행할때마다 볼륨을 지정
        bgmVolume = volume;
    }

    public void SetSfxVolume(float volume)
    {
        // 효과음 조절 함수
        // 슬라이더 값을 변수에 저장해서 효과음을 실행할때마다 볼륨을 지정
        sfxVolume = volume;
    }
}
