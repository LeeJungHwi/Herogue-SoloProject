using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Pet : MonoBehaviour
{
    // 플레이어
    private GameObject player;

    // 타겟
    private Transform target;

    // 네비매쉬
    public NavMeshAgent nav;

    // 물리
    private Rigidbody rigid;

    // 애니메이터
    private Animator anim;

    // 플레이어와의 거리
    private float distance;

    void Awake()
    {
        // 플레이어 관련 할당
        Invoke("SetPlayer", 0.5f);

        // 물리
        rigid = GetComponent<Rigidbody>();

        // 애니메이터
        anim = GetComponent<Animator>();

        // 네비매쉬
        nav = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        // 플레이어와의 거리
        distance = Vector3.Distance(target.transform.position, transform.position);

        // 펫 이동 설정
        if(distance > 30)
        {
            nav.isStopped = false;
            anim.SetBool("isWalk", true);
            nav.SetDestination(target.position);
        }
        else
        {
            nav.isStopped = true;
            anim.SetBool("isWalk", false);
        }
    }

    void FixedUpdate()
    {
        // 물리충돌시 회전이 유지되는 문제
        FreezeVelocity();
    }

    void FreezeVelocity()
    {
        // 물리충돌시 회전이 유지되는 문제
        // 속도 0
        rigid.velocity = Vector3.zero;

        // 회전속도 0
        rigid.angularVelocity = Vector3.zero;
    }

    void SetPlayer()
    {
        // 플레이어
        player = GameObject.FindGameObjectWithTag("Player");

        // 타겟 : 플레이어
        target = player.transform;
    }
}
