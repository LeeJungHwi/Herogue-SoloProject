using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecretBox : MonoBehaviour
{
    // 방 모델
    private RoomTemplates templates;

    // 파괴되었는지 체크
    [HideInInspector] public bool isCrash;

    // 추가되었는지 체크
    // 풀링에서 활성화할때 false로 바꿔서 활성화
    [HideInInspector] public bool isAdd;

    // 오브젝트 타입
    public ObjType type;

    // 오브젝트 풀
    private PoolingManager poolManager;

    private void Start()
    {
        // 방 모델
        templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();

        // 오브젝트 풀
        poolManager = GameObject.FindGameObjectWithTag("PoolManager").GetComponent<PoolingManager>();
    }

    private void Update()
    {
        // 애니메이션을 보여주기위해 2초뒤에 시크릿박스 반납
        if(isCrash) Invoke("DeActive", 2f);

        // 추가된 상태가 아닐때에만 리스트에 시크릿박스 추가
        if (!isAdd)
        {
            templates.secretBoxes.Add(this.gameObject);
            isAdd = true;
        }
    }

    // 풀에 반환
    private void DeActive() { poolManager.ReturnObj(gameObject, ObjType.시크릿박스); }
}
