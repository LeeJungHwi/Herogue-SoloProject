using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Joystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    // 조이스틱 배경
    Image imageBackground;

    // 조이스틱 바
    Image imageController;

    // 터치 위치
    public Vector2 touchPosition;

    // 플레이어
    public GameObject player;
    
    // 플레이어 스크립트
    Player playerScript;

    // 물리
    Rigidbody rigid;

    // 애니메이터
    Animator anim;

    // 이동 체크
    bool isRun = false;
    
    // 점프 체크
    public bool isJump = false;

    // 이동 체크
    public bool isPlayerMoving = false;

    // 대쉬 체크
    public bool isDash = false;

    // 플레이어 이동속도
    public float moveSpeed;

    // 다음 발소리 대기시간
    public float waitTime;

    // 컴퓨터 확인용 

    // Dash
    bool jDown;

    // x축
    float hAxis;

    // z축
    float vAxis;

    // 이동 방향
    Vector3 moveVec;

    // 대쉬 방향
    Vector3 dashVec;

    void GetInput()
    {
        // 키보드 입력
        jDown = Input.GetButtonDown("Jump");
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");
    }

    void Awake()
    {
        // 조이스틱 배경
        imageBackground = GetComponent<Image>();

        // 조이스틱 바
        imageController = transform.GetChild(0).GetComponent<Image>();

        // 물리
        rigid = player.GetComponent<Rigidbody>();

        // 애니메이터
        anim = player.GetComponent<Animator>();

        // 플레이어 스크립트
        playerScript = player.GetComponent<Player>();

        // 발소리
        StartCoroutine(SFXStep());
    }

    void Update()
    {
        // 컴퓨터 키입력
        GetInput();

        // 컴퓨터 플레이어 이동
        Move();

        // 모바일 플레이어 이동
        MobileMove();

        // 컴퓨터 플레이어 회전
        Turn();

        // 모바일 플레이어 회전
        MobileTurn();

        // 컴퓨터 플레이어 대쉬
        Dash();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // 인터페이스 구현
        // 최초 터치
        // 이동상태
        isPlayerMoving = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        // 인터페이스 구현
        // 터치 상태
        // 조이스틱의 위치가 어디에있든 동일한값을 연산하기위해
        // 터치지점은 조이스틱이미지의 현재위치를 기준으로 얼마나떨어져있는지 계산한다
        touchPosition = Vector2.zero;
        if(RectTransformUtility.ScreenPointToLocalPointInRectangle(imageBackground.rectTransform, eventData.position, eventData.pressEventCamera, out touchPosition))
        {
            // 터치지점이 0~1 사이가 나오도록 정규화한다
            // 터지지점을 이미지의 크기로 나눈다
            touchPosition.x = (touchPosition.x / imageBackground.rectTransform.sizeDelta.x);
            touchPosition.y = (touchPosition.y / imageBackground.rectTransform.sizeDelta.y);

            // 터지지점이 -n~n 사이가 나오도록 정규화한다
            // 피봇에따라 값을 수정해야한다
            touchPosition = new Vector2(touchPosition.x * 2 - 1, touchPosition.y * 2 - 1);

            // 터치지점이 -1~1 사이가 나오도록 정규화한다
            // 배경이미지 밖을 터치하면 -1~1를 벗어나는 큰값이 나오므로 normalized 해준다
            touchPosition = (touchPosition.magnitude > 1) ? touchPosition.normalized : touchPosition;

            // 터치방향으로 컨트롤러의 이미지가 이동한다
            imageController.rectTransform.anchoredPosition = new Vector2(touchPosition.x * imageBackground.rectTransform.sizeDelta.x / 2, touchPosition.y * imageBackground.rectTransform.sizeDelta.y / 2);
            
        }
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        // 인터페이스 구현
        // 터치 종료
        // 컨트롤러를 다시 중앙으로 이동시킨다
        imageController.rectTransform.anchoredPosition = Vector2.zero;

        // 터치가 종료되었을때 플레이어가 움직이지 않도록 터지지점 초기화
        touchPosition = Vector2.zero;

        // 이동상태 X
        isRun = false;
        isPlayerMoving = false;
    }

    // 모바일 이동
    void MobileMove()
    {
        // 업데이트에서 모바일 이동과 PC 이동이 둘 다 호출되므로 뒤에 실행되는 이동로직에서 moveVec이 0이 되어서 애니메이션이 작동 안 됨 -> 컴퓨터 입력이 없을경우만 모바일 이동
        // 벽의 경계와 닿지 않았을때만 이동이 가능
        if(hAxis == 0 && vAxis == 0 && !playerScript.isBorder)
        {
            moveVec = isDash ? dashVec : new Vector3(-touchPosition.y, 0, touchPosition.x).normalized; // 대쉬상태 일 때 대쉬방향으로
            moveVec = (playerScript.slopeAngle > 0) ? (moveVec + transform.up).normalized : moveVec; // 경사각이 있다면 위쪽 방향으로
            player.transform.position += moveVec * moveSpeed * Time.deltaTime; // 이동
            isRun = moveVec != Vector3.zero; // 이동 플래그
            anim.SetBool("isRun", isRun); // 이동 애니메이션
        }
    }
    
    void Move()
    {
        // 컴퓨터 플레이어 이동
        // 업데이트에서 뒤에 실행되는 이동로직에서 moveVec이 0이 되어서
        // 달리는 애니메이션이 적용되지 않는문제
        // 모바일 입력이 없을경우만
        if(touchPosition.y == 0 && touchPosition.x == 0)
        {
            // 대각선 이동속도 정규화
            moveVec = new Vector3(vAxis * -1, 0, hAxis).normalized;

            // 대쉬상태에서 이동방향을 대쉬방향으로
            if (isDash)
                moveVec = dashVec;

            // 벽의 경계와 닿지 않았을때만 이동이 가능
            if (!playerScript.isBorder)
            {
                // 경사각이 있다면
                if (playerScript.slopeAngle > 0)
                {
                    // 위쪽으로도 이동
                    moveVec = (moveVec + transform.up).normalized;
                    player.transform.position += moveVec * moveSpeed * Time.deltaTime;
                }
                else
                {
                    // 경사각이 없다면 그냥 이동
                    player.transform.position += moveVec * moveSpeed * Time.deltaTime;
                }
            }

            // 이동상태
            if (moveVec != Vector3.zero)
            {
                // 이동상태
                isRun = true;
                anim.SetBool("isRun", true);
            }
            else
            {
                // 이동상태 X
                isRun = false;
                anim.SetBool("isRun", false);
            }
        }
    }

    IEnumerator SFXStep()
    {
        // 발소리
        while (true)
        {
            yield return null;
            if (isRun && !isDash && !playerScript.isShoot)
            {
                // 이동상태 대쉬 X 슈팅 X 일때만 발소리 재생
                if(waitTime <= 0)
                {
                    SoundManager.instance.SFXPlay(ObjType.무빙소리);
                    waitTime = 0.4f;
                }
                else
                {
                    waitTime -= Time.deltaTime;
                }
            }
        }
    }

    public void MobileTurn()
    {
        // 모바일 플레이어 회전
        if (touchPosition.x != 0 && touchPosition.y != 0)
        {
            player.transform.rotation = Quaternion.Euler(0f, Mathf.Atan2(touchPosition.y * -1, this.touchPosition.x) * Mathf.Rad2Deg, 0f);
        }
    }

    void Turn()
    {
        // 컴퓨터 플레이어 회전
        if(vAxis !=0 || hAxis !=0)
        {
            player.transform.rotation = Quaternion.Euler(0f, Mathf.Atan2(vAxis * -1, hAxis) * Mathf.Rad2Deg, 0f);
        }
    }

    public void Jump()
    {
        // 플레이어 점프 : 필요없는 것 같아서 사용 안하는중
        if(isJump || isDash)
        {
            return;
        }

        // 플레이어 점프
        rigid.AddForce(Vector3.up * 10, ForceMode.Impulse);

        // 점프상태
        isJump = true;

        // 애니메이션
        anim.SetBool("isJump", true);
    }

    public void MobileDash()
    {
        // 모바일 플레이어 대쉬
        if (!isDash && !isJump)
        {
            // 플레이어 대쉬
            // 이동속도 증가
            moveSpeed *= 1.5f;

            // 향상된대쉬
            if (playerScript.PassiveSkill[20] > 0)
            {
                // 향상된대쉬
                // 대쉬할때 무적 레이어
                playerScript.transform.gameObject.layer = 15;
            }

            // 대쉬상태
            isDash = true;

            // 대쉬방향을 이동방향으로
            dashVec = moveVec;

            // 애니메이션
            anim.SetTrigger("doDash");

            // 플레이어 대쉬 종료
            Invoke("DashOut", 0.5f);

            // 대쉬 효과음 재생
            SoundManager.instance.SFXPlay(ObjType.대쉬소리);
        }
    }

    void Dash()
    {
        // 컴퓨터 플레이어 대쉬
        if (jDown && !isDash && !isJump)
        {
            // 플레이어 대쉬
            // 이동속도 증가
            moveSpeed *= 1.5f;

            // 향상된대쉬
            if(playerScript.PassiveSkill[20] > 0)
            {
                // 향상된대쉬
                // 대쉬할때 무적 레이어
                playerScript.transform.gameObject.layer = 15;
            }

            // 대쉬상태
            isDash = true;

            // 대쉬방향을 이동방향으로
            dashVec = moveVec;

            // 애니메이션
            anim.SetTrigger("doDash");

            // 플레이어 대쉬 종료
            Invoke("DashOut", 0.5f);

            // 대쉬 효과음 재생
            SoundManager.instance.SFXPlay(ObjType.대쉬소리);
        }
    }

    void DashOut()
    {
        // 컴퓨터 플레이어 대쉬 종료
        // 이동속도 원래대로
        moveSpeed = moveSpeed * 2 / 3;

        // 향상된대쉬
        if (playerScript.PassiveSkill[20] > 0)
        {
            // 향상된대쉬
            // 대쉬 종료시 무적 레이어 해제
            playerScript.transform.gameObject.layer = 7;
        }

        // 대쉬상태 X
        isDash = false;
    }
}
