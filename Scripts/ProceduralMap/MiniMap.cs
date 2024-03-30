using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;

// 던전 그래프 정보에 따라 미니맵 표시
public class MiniMap : MonoBehaviour
{
    public RoomBFS roomBFS; // 그래프
    public List<GameObject> roomTilePref = new List<GameObject>(); // 미니맵에 표시 할 타일
    public List<int> drawIndex = new List<int>(); // 인덱스 백업
    public Player player; // 플레이어
    public Vector2Int curRoom; // 현재 플레이어가 위치한 방의 좌표, BFS 돌릴때 시작방 (10,10)으로 초기화함
    public Vector2Int preRoom; // 이전에 플레이어가 위치했던 방의 좌표, BFS 돌릴때 시작방 (10,10)으로 초기화함

    // 플레이어가 있는 방의 타일을 초록색으로
    void Update()
    {
        // 던전일때만
        if(!player.isShelter)
        {
            // 플레이어의 현재 위치를 가져와서
            Vector2Int playerPos = new Vector2Int((int)player.transform.position.x, (int)player.transform.position.z);

            // 좌우는 z축 상하는 x축으로 결정됨
            // 현재방을 기준으로 상하좌우로 이동하면 현재 플레이어가 위치한 방 갱신해줘야함
            // 갱신해주면서 플레이어가 이전에 위치한 방은 다시 흰색으로 돌려놔야함(단, 보스방은 빨간색으로 돌려야함)
            // 흰색으로 돌리고나서 이전에 플레이어가 위치했던 방을 현재 방으로 갱신해줘야함

            // 1.플레이어가 왼쪽방에 간다고 생각해보자(z)
            // 현재 플레이어 위치 < (현재 플레이어가 위치한 방의 좌표 - 100) 이면 왼쪽방

            // 2.풀레이어가 오른쪽방에 간다고 생각해보자(z)
            // 현재 플레이어 위치 > (현재 플레이어가 위치한 방의 좌표 + 100) 이면 오른쪽방

            // 3.플레이어가 위쪽방에 간다고 생각해보자(x)
            // 현재 플레이어 위치 < (현재 플레이어가 위치한 방의 좌표 - 100) 이면 위쪽방

            // 4.플레이어가 아래쪽방에 간다고 생각해보자(x)
            // 현재 플레이어 위치 > (현재 플레이어가 위치한 방의 좌표 + 100) 이면 아래쪽방

            // 왼쪽
            if (playerPos.y < (curRoom.y * 200 - 100))
            {
                curRoom = new Vector2Int(curRoom.x, curRoom.y - 1); // 현재 플레이어가 위치한 방 갱신
                roomTilePref[preRoom.x * 21 + preRoom.y].GetComponent<Image>().color = ((int)roomBFS.maxDisPos.x / 200 == preRoom.x && (int)roomBFS.maxDisPos.z / 200 == preRoom.y) ? Color.red : Color.white; // 플레이어가 이전에 위치했던방 보스방은 빨간색 일반방은 흰색으로 돌림
                preRoom = curRoom; // 플레이어가 이전에 위치했던 방 갱신
            }
            
            // 오른쪽
            if (playerPos.y > (curRoom.y * 200 + 100))
            {
                curRoom = new Vector2Int(curRoom.x, curRoom.y + 1);
                roomTilePref[preRoom.x * 21 + preRoom.y].GetComponent<Image>().color = ((int)roomBFS.maxDisPos.x / 200 == preRoom.x && (int)roomBFS.maxDisPos.z / 200 == preRoom.y) ? Color.red : Color.white;
                preRoom = curRoom;
            }
            
            // 위쪽
            if (playerPos.x < (curRoom.x * 200 - 100))
            {
                curRoom = new Vector2Int(curRoom.x - 1, curRoom.y);
                roomTilePref[preRoom.x * 21 + preRoom.y].GetComponent<Image>().color = ((int)roomBFS.maxDisPos.x / 200 == preRoom.x && (int)roomBFS.maxDisPos.z / 200 == preRoom.y) ? Color.red : Color.white;
                preRoom = curRoom; 
            }
            
            // 아래쪽
            if (playerPos.x > (curRoom.x * 200 + 100))
            {
                curRoom = new Vector2Int(curRoom.x + 1, curRoom.y);
                roomTilePref[preRoom.x * 21 + preRoom.y].GetComponent<Image>().color = ((int)roomBFS.maxDisPos.x / 200 == preRoom.x && (int)roomBFS.maxDisPos.z / 200 == preRoom.y) ? Color.red : Color.white;
                preRoom = curRoom; 
            }

            // 플레이어가 현재 위치한 방의 타일을 초록색으로
            roomTilePref[curRoom.x * 21 + curRoom.y].GetComponent<Image>().color = Color.green;
        }
    }

    // 미니맵 표시하는 함수
    public void DrawMiniMap()
    {
        // 그래프 돌면서
        for(int i = 0; i < 21; i++)
        {
            for(int j = 0; j < 21; j++)
            {
                // 방을 만나면
                if(roomBFS.graph[i, j] != ObjType.화살)
                {
                    // 해당하는 위치의 타일 활성화
                    roomTilePref[i * 21 + j].SetActive(true);

                    // 이전 스테이지에서 활성화했던 인덱스 백업
                    // 다음 스테이지 또는 마을로갈때 초기화
                    drawIndex.Add(i * 21 + j);
                }
            }
        }

        // maxDisPos 보스 방은 빨간색으로
        roomTilePref[(int)roomBFS.maxDisPos.x / 200 * 21 + (int)roomBFS.maxDisPos.z / 200].GetComponent<Image>().color = Color.red;
    }
}
