using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 스킬 기본 구조
public class AbilityBase : ScriptableObject
{
    // 스킬 쿨다운시간
    public float cooldownTime;

    // 스킬 유지시간
    public float activeTime;

    // 스킬 시전
    public virtual void Activate(GameObject joystick, GameObject player, GameObject poolingManager) { }

    // 스킬 종료
    public virtual void DeAtivate(GameObject joystick, GameObject player, GameObject poolingManager) { }
}
