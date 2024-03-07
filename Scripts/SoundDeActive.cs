using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundDeActive : MonoBehaviour
{
    // 오디오소스
    AudioSource audioSource;

    void Awake()
    {
        // 오디오소스
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        // 사운드 비활성화
        if(!audioSource.isPlaying)
        {
            gameObject.SetActive(false);
        }
    }
}
