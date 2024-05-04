using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectCharacter : MonoBehaviour
{
    [SerializeField] private Character character; // 각 캐릭터 버튼이 가지고 있을 캐릭터 종류

    public void CharacterSelect()
    {
        DataManager.instance.character = character; // 선택된 캐릭터를 데이터매니저에게 넘겨줌
        SoundManager.instance.MainSFXPlay("ButtonSound", SoundManager.instance.mainSfxList[0]); // 사운드
    }
}


