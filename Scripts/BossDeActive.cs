using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDeActive : MonoBehaviour
{
    void Update()
    {
        // 보스 반납 : 보스가 죽는 애니메이션을 유지하기위해서 1초뒤에 반납한다
        if (this.GetComponent<Enemy>().isDead)
        {
            Invoke("DeActive", 1f);
        }
    }

    void DeActive()
    {
        // 보스 반납
        gameObject.SetActive(false);
    }
}
