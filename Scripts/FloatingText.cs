using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FloatingText : MonoBehaviour
{
    // 텍스트 이동속도
    public float moveSpeed;

    // 텍스트 투명속도
    public float alphaSpeed;

    // TMP
    public TextMeshPro text;

    // TMP Color
    public Color alpha;

    // 텍스트 반납 타임
    public float waitTime;

    void Start()
    {
        // TMP
        text = GetComponent<TextMeshPro>();

        // TMP Color
        alpha = text.color;
    }

    void Update()
    {
        // 텍스트가 위로올라가면서 투명
        transform.Translate(new Vector3(0, moveSpeed * Time.deltaTime, 0));
        alpha.a = Mathf.Lerp(alpha.a, 0, Time.deltaTime * alphaSpeed);
        text.color = alpha;

        // 데미지 텍스트 반납
        if(waitTime <= 0)
        {
            gameObject.SetActive(false);
        }
        else
        {
            waitTime -= Time.deltaTime;
        }
    }
}