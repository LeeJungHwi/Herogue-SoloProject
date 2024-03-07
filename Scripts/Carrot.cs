using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Carrot : MonoBehaviour
{
    // 당근 및 키클로페스 눈데미지
    public int damage;

    // 당근 및 키클로페스 눈이 유지되는 시간
    public float waitTime;

    void Update()
    {
        // 당근 및 키클로페스 눈 회전
        transform.Rotate(Vector3.right * 90 * Time.deltaTime);

        if (waitTime <= 0)
        {
            // 당근 및 키클로페스 눈 반납
            gameObject.SetActive(false);
        }
        else
        {
            waitTime -= Time.deltaTime;
        }
    }
}
