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
    }
}
