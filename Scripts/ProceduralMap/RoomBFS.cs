using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UI;

// 목적 : 문의 다음방이 없는 이슈 픽스 + 시작방에서 가장 거리가 먼 방에 보스 생성
public class RoomBFS : MonoBehaviour
{
    public int n = 21, m = 21; // N, M 21, 21
    public ObjType[,] graph; // 그래프
    public int[,] dis; // 거리
    Queue<Vector2Int> checkPos = new Queue<Vector2Int>(); // 체크 할 위치
    List<Vector2Int> checkDir = new List<Vector2Int>(); // 상하좌우 -> 상 : X -200 하 X +200 좌 Z -200 우 Z +200 -> 200을 1단위로 봄 상 : X - 1 하 : X + 1 좌 Z - 1 우 Z + 1
    public PoolingManager poolingManager; // 방생성
    public RoomTemplates templates; // 방모델
    int maxDis = 0; // 최대거리
    public Vector3 maxDisPos = Vector3.zero; // 최대거리 위치
    public bool isBFS = false; // BFS 돌렸는지 체크
    public MiniMap miniMap; // 던전 미니맵

    void Awake()
    {
        // 그래프
        graph = new ObjType[n, m];

        // 거리
        dis = new int[n, m];

        // 체크 할 위치
        checkPos = new Queue<Vector2Int>();

        // 상하좌우
        checkDir.Add(new Vector2Int(-1, 0));
        checkDir.Add(new Vector2Int(1, 0));
        checkDir.Add(new Vector2Int(0, -1));
        checkDir.Add(new Vector2Int(0, 1));

        // 미니맵
        miniMap = GameObject.Find(DataManager.instance.character.ToString() + "Canvas").GetComponentInChildren<MiniMap>();
    }

    void Update()
    {
        // BFS 이미 돌렸으면 리턴
        if(isBFS)
        {
            return;
        }

        // 방이 모두 생성된 상태
        if(templates.rooms.Count > templates.baseStage + templates.currentStage / templates.stageCoef * templates.stageCoef)
        {
            // 방이 모두 생성되고 BFS 돌림
            // 문의 다음방이 없는 버그 픽스 + 시작방에서 가장 거리가 먼 방에 보스 생성
            // 오브젝트 풀에서 보스 꺼낼때 maxDisPos로 보스 이동
            isBFS = true;
            BFS();

            //디버깅용
            //Debug.Log(maxDisPos);          
        }
    }

    // 시작방 상하좌우로 연결되어있는 방들부터 BFS 돌면서
    // 문의 다음방이 없으면 3면이 막힌 방을 다음방에 생성
    // 시작방부터 가장 거리가 먼 방에 보스 생성
    public void BFS()
    {
        // 디버깅용
        // for(int i = 0; i < 21; i++)
        // {
        //     for(int j = 0; j < 21; j++)
        //     {
        //         if(graph[i, j] != ObjType.화살)
        //         {
        //             Debug.Log("(" + i + ", " + j + ")" + "Dis : " + dis[i, j]);        
        //         }
        //     }
        // }

        checkPos.Enqueue(new Vector2Int(9, 10));
        checkPos.Enqueue(new Vector2Int(11, 10));
        checkPos.Enqueue(new Vector2Int(10, 9));
        checkPos.Enqueue(new Vector2Int(10, 11));
        graph[10, 10] = ObjType.SecretRoom;
        dis[10, 10] = 1;
        dis[9, 10] = 1;
        dis[11, 10] = 1;
        dis[10, 9] = 1;
        dis[10, 11] = 1;
        maxDis = 1;
        maxDisPos = new Vector3(1800, 0, 2000);
        miniMap.curRoom = new Vector2Int(10, 10);
        miniMap.preRoom = new Vector2Int(10, 10);

        while(checkPos.Count != 0) // 큐가 빌때까지
        {
            // 기준위치 꺼냄
            Vector2Int standardPos = checkPos.Peek();
            checkPos.Dequeue();

            // 기준위치 방의 타입에 따라 문의 다음방이 없으면
            // 3면이 막힌방을 다음방에 생성하고 거리저장 최대거리갱신 최대거리위치갱신 미니맵에표시한인덱스저장

            string checkDoor = graph[standardPos.x, standardPos.y].ToString(); // 체크 할 문 : 방타입을 일단 문자열로 가져옴 B BL TBL ....

            // 시크릿방은 4방향 모두 문이므로 TBLR로 바꿔줌
            if(checkDoor == "SecretRoom")
            {
                checkDoor = "TBLR";
            }

            for(int i = 0; i < checkDoor.Length; i++) 
            {
                char curDir = checkDoor[i]; // 현재 체크 할 문 방향 T B L R 중 하나

                switch(curDir) // 현재 체크 할 문 방향에 따라 방이있는지 체크하고 없으면 
                {
                    case 'T': // 위쪽에 방이 있는지 체크하고 없으면 B만 열려있는 방 생성 거리저장 최대거리갱신 최대거리위치갱신 미니맵에표시한인덱스저장
                        if(graph[standardPos.x - 1, standardPos.y] == ObjType.화살)
                        {
                            // 방 생성 거리저장
                            RoomSpawn(ObjType.B, new Vector3((standardPos.x - 1) * 200, 0, standardPos.y * 200));
                            dis[standardPos.x - 1, standardPos.y] = dis[standardPos.x, standardPos.y] + 1;

                            // 최대거리갱신 최대거리위치갱신
                            if(maxDis < dis[standardPos.x - 1, standardPos.y])
                            {
                                maxDis = dis[standardPos.x - 1, standardPos.y];
                                maxDisPos = new Vector3((standardPos.x - 1) * 200, 0, standardPos.y * 200);
                            }

                            // 생성한 막힌 방 미니맵에 표시하고 인덱스 백업
                            miniMap.roomTilePref[(standardPos.x - 1) * 21 + standardPos.y].SetActive(true);
                            miniMap.drawIndex.Add((standardPos.x - 1) * 21 + standardPos.y);
                        }
                        break; 
                    case 'B': // 아래쪽에 방이 있는지 체크하고 없으면 T만 열려있는 방 생성 거리저장 최대거리갱신 최대거리위치갱신 미니맵에표시한인덱스저장
                        if(graph[standardPos.x + 1, standardPos.y] == ObjType.화살)
                        {
                            RoomSpawn(ObjType.T, new Vector3((standardPos.x + 1) * 200, 0, standardPos.y * 200));
                            dis[standardPos.x + 1, standardPos.y] = dis[standardPos.x, standardPos.y] + 1;

                            // 최대거리갱신 최대거리위치갱신
                            if(maxDis < dis[standardPos.x + 1, standardPos.y])
                            {
                                maxDis = dis[standardPos.x + 1, standardPos.y];
                                maxDisPos = new Vector3((standardPos.x + 1) * 200, 0, standardPos.y * 200);
                            }            

                            // 생성한 막힌 방 미니맵에 표시하고 인덱스 백업
                            miniMap.roomTilePref[(standardPos.x + 1) * 21 + standardPos.y].SetActive(true);   
                            miniMap.drawIndex.Add((standardPos.x + 1) * 21 + standardPos.y);            
                        }
                        break;
                    case 'L': // 왼쪽에 방이 있는지 체크하고 없으면 R만 열려있는 방 생성 거리저장 최대거리갱신 최대거리위치갱신 미니맵에표시한인덱스저장
                        if(graph[standardPos.x, standardPos.y - 1] == ObjType.화살)
                        {
                            RoomSpawn(ObjType.R, new Vector3(standardPos.x * 200, 0, (standardPos.y - 1) * 200));
                            dis[standardPos.x, standardPos.y - 1] = dis[standardPos.x, standardPos.y] + 1;

                            // 최대거리갱신 최대거리위치갱신
                            if(maxDis < dis[standardPos.x, standardPos.y - 1])
                            {
                                maxDis = dis[standardPos.x, standardPos.y - 1];
                                maxDisPos = new Vector3(standardPos.x * 200, 0, (standardPos.y - 1) * 200);
                            }                           

                            // 생성한 막힌 방 미니맵에 표시하고 인덱스 백업
                            miniMap.roomTilePref[standardPos.x * 21 + standardPos.y - 1].SetActive(true);
                            miniMap.drawIndex.Add(standardPos.x * 21 + standardPos.y - 1); 
                        }
                        break;
                    case 'R': // 오른쪽에 방이 있는지 체크하고 없으면 L만 열려있는 방 생성 거리저장 최대거리갱신 최대거리위치갱신 미니맵에표시한인덱스저장
                        if(graph[standardPos.x, standardPos.y + 1] == ObjType.화살)
                        {
                            RoomSpawn(ObjType.L, new Vector3(standardPos.x * 200, 0, (standardPos.y + 1) * 200));
                            dis[standardPos.x, standardPos.y + 1] = dis[standardPos.x, standardPos.y] + 1;

                            // 최대거리갱신 최대거리위치갱신
                            if(maxDis < dis[standardPos.x, standardPos.y + 1])
                            {
                                maxDis = dis[standardPos.x, standardPos.y + 1];
                                maxDisPos = new Vector3(standardPos.x * 200, 0, (standardPos.y + 1) * 200);
                            }      

                            // 생성한 막힌 방 미니맵에 표시하고 인덱스 백업
                            miniMap.roomTilePref[standardPos.x * 21 + standardPos.y + 1].SetActive(true);
                            miniMap.drawIndex.Add(standardPos.x * 21 + standardPos.y + 1); 
                        }
                        break;
                }
            }

            // 상하좌우
            for(int i = 0; i < 4; i++)
            {
                // 여기서 문이 있는 방향만 가야 제대로된 거리 측정
                if(checkDoor.IndexOf("T") == -1) // T가 없으면 상으로 갈수없음
                {
                    if(i == 0)
                    {
                        continue;
                    }
                }

                if(checkDoor.IndexOf("B") == -1) // B가 없으면 하로 갈수없음
                {
                    if(i == 1)
                    {
                        continue;
                    }
                }

                if(checkDoor.IndexOf("L") == -1) // L이 없으면 좌로 갈수없음
                {
                    if(i == 2)
                    {
                        continue;
                    }
                }

                if(checkDoor.IndexOf("R") == -1) // R이 없으면 우로 갈수없음
                {
                    if(i == 3)
                    {
                        continue;
                    }
                }

                // 체크 할 위치
                int checkI = standardPos.x + checkDir[i].x;
                int checkJ = standardPos.y + checkDir[i].y;

                // 경계체크
                if(checkI < 0 || checkJ < 0 || checkI >= n || checkJ >= m)
                {
                    continue;
                }

                // 방문체크
                if(dis[checkI, checkJ] > 0)
                {
                    continue;
                }

                // 방체크
                if(graph[checkI, checkJ] == ObjType.화살)
                {
                    continue;
                }

                // 큐에저장 거리저장 최대거리갱신 최대거리위치갱신
                checkPos.Enqueue(new Vector2Int(checkI, checkJ));
                dis[checkI, checkJ] = dis[standardPos.x, standardPos.y] + 1;

                // 최대거리갱신 최대거리위치갱신
                if(maxDis < dis[checkI, checkJ])
                {
                    maxDis = dis[checkI, checkJ];
                    maxDisPos = new Vector3(checkI * 200, 0, checkJ * 200);
                }  
            }
        }

        // 그래프 돌면서 미니맵에 방 표시
        miniMap.DrawMiniMap();
    }

    // 방생성
    void RoomSpawn(ObjType type, Vector3 pos)
    {
        GameObject instantRoom = poolingManager.GetObj(type);
        instantRoom.transform.position = pos;
        instantRoom.transform.rotation = poolingManager.RandomMapPrefs[(int)type - 53].transform.rotation;
    }

    // 다음 BFS를 위해서 다음스테이지 또는 마을로 갈때 초기화
    public void InitForNextBFS()
    {
        // 그래프, 거리 초기화
        for(int i = 0; i < 21; i++)
        {
            for(int j = 0; j < 21; j++)
            {
                graph[i, j] = ObjType.화살;
                dis[i, j] = 0;
            }
        }

        // 이전 스테이지에서 활성화했던 미니맵 타일 초기화 및 보스방 타일 다시 흰색으로
        for(int i = 0; i < miniMap.drawIndex.Count; i++)
        {
            miniMap.roomTilePref[miniMap.drawIndex[i]].SetActive(false);
        }
        miniMap.drawIndex.Clear();
        miniMap.roomTilePref[(int)maxDisPos.x / 200 * 21 + (int)maxDisPos.z / 200].GetComponent<UnityEngine.UI.Image>().color = Color.white;

        // 최대거리, BFS체크 초기화
        maxDisPos = Vector3.zero;
        isBFS = false;
    }
}
