using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundDeActive : MonoBehaviour
{
    // 오디오소스
    private AudioSource audioSource;

    // 오브젝트 타입
    [SerializeField] private ObjType type;

    // 오브젝트 풀
    private PoolingManager poolManager;

    private void Awake()
    {
        // 오디오소스
        audioSource = GetComponent<AudioSource>();

        // 오브젝트 풀
        poolManager = GameObject.FindGameObjectWithTag("PoolManager").GetComponent<PoolingManager>();
    }

    // 사운드 비활성화
    private void Update() { if(!audioSource.isPlaying) poolManager.ReturnObj(gameObject, type); }
}
