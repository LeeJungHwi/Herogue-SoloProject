using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    // 카메라 회전속도
    private float rot_speed = 100.0f;

    // 플레이어
    private GameObject Player;

    // 카메라
    public GameObject MainCamera;

    // 특정 레이어 감지
    public LayerMask layerMask;

    // 리그로부터 카메라까지의 거리
    private float camera_dist = 0f;

    // 가로거리
    private float camera_width = -60f;

    // 세로거리
    private float camera_height = 15f;

    // 레이케스트 후 리그쪽으로 올 거리
    private float camera_fix = 3f;

    // 방향
    Vector3 dir;

    // 카메라 흔들림 효과가 진행중인지 체크
    public bool isShake { set; get; }

    void Start()
    {
        // 카메라리그에서 카메라까지의 길이
        camera_dist = Vector3.Distance(transform.position, MainCamera.transform.position);

        // 카메라리그에서 카메라위치까지의 방향벡터
        dir = new Vector3(0, camera_height, camera_width).normalized;

        // 플레이어
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        // 카메라가 흔들리는 중일땐 실행하지 않는다
        if(isShake)
        {
            return;
        }

        // 리그의 위치를 플레이어의 위치로 초기화
        transform.position = Player.transform.position;

        // 레이캐스트할 벡터값
        Vector3 ray_target = transform.up * camera_height + transform.forward * camera_width;

        if(!Player.GetComponent<Player>().isShelter)
        {
            // 마을이 아닐때에만
            RaycastHit hitinfo;
            Physics.Raycast(transform.position, ray_target, out hitinfo, camera_dist, layerMask);

            if (hitinfo.point != Vector3.zero)
            {
                //레이케스트 성공시
                //point로 옮긴다.
                MainCamera.transform.position = hitinfo.point + transform.up * 10;

                //카메라 보정
                MainCamera.transform.Translate(dir * -1 * camera_fix);
            }
            else
            {
                // 레이케스트 실패시
                // 원래 카메라 위치로
                //로컬좌표를 0으로 맞춘다. (카메라리그로 옮긴다.)
                MainCamera.transform.localPosition = Vector3.zero;

                //카메라위치까지의 방향벡터 * 카메라 최대거리 로 옮긴다.
                MainCamera.transform.Translate(dir * camera_dist);

                //카메라 보정
                MainCamera.transform.Translate(dir * -1 * camera_fix);

            }
        }
        else
        {
            // 마을일 경우에는
            // 원래 카메라 위치로
            //로컬좌표를 0으로 맞춘다. (카메라리그로 옮긴다.)
            MainCamera.transform.localPosition = Vector3.zero;

            //카메라위치까지의 방향벡터 * 카메라 최대거리 로 옮긴다.
            MainCamera.transform.Translate(dir * camera_dist);
            
            //카메라 보정
            MainCamera.transform.Translate(dir * -1 * camera_fix);
        }
    }

    void SetPlayer()
    {
        // 플레이어를 할당하는 함수
        Player = GameObject.FindGameObjectWithTag("Player");
    }
}
