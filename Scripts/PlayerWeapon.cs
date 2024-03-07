using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    // 화살이 유지되는 시간
    public float waitTime;

    void Update()
    {
        if(waitTime <= 0)
        {
            // 화살 반납
            gameObject.SetActive(false);
        }
        else
        {
            waitTime -= Time.deltaTime;
        }
    }
}
