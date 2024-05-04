using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 스킬 유한상태머신
public class AbilityFSM : MonoBehaviour
{
    // 시전 할 스킬
    public AbilityBase abilityBase;

    // 스킬 쿨타임
    private float cooldownTime;

    // 스킬 유지시간
    private float activeTime;

    // 스킬 상태 정의
    private enum AbilityState { ready, active, cooldown }

    // 스킬 상태 참조
    private AbilityState state = AbilityState.ready;

    // 스킬 사용 키
    // public KeyCode key;

    // Joystick 오브젝트
    [SerializeField] private GameObject joystick;

    // PoolingManager 오브젝트
    [SerializeField] private GameObject poolingManager;

    // 플레이어 스크립트
    private Player player;

    // 스킬 버튼이 눌렸는지 체크
    private bool abilityPressed;

    // 스킬 쿨타임을 표시할 이미지
    [SerializeField] private Image cooldownImage;

    // 스킬 쿨타임을 표시하기위해
    private float duration = 0f;
    
    // 플레이어 스크립트
    private void Start() { player = GetComponent<Player>(); }

    private void Update()
    {
        switch (state)
        {
            // 스킬 준비
            case AbilityState.ready:
                if(abilityPressed)
                {
                    // 스킬 시전
                    abilityBase.Activate(joystick, gameObject, poolingManager);

                    // 스킬 시전 상태
                    state = AbilityState.active;

                    // 스킬 사용 시간 할당
                    activeTime = abilityBase.activeTime;

                    // 스킬 쿨다운 시간 할당
                    cooldownTime = abilityBase.cooldownTime;

                    // 스킬 쿨타임 표시 초기화
                    duration = 0f;

                    // 스킬 체크 초기화
                    abilityPressed = false;
                }
                break;
            
            // 스킬 유지
            case AbilityState.active:
                if (activeTime > 0)
                {
                    // 유지시간 감소
                    activeTime -= Time.deltaTime;

                    // 스킬 쿨타임 이미지 표시(스킬쿨타임 = 스킬유지시간 + 쿨다운시간)
                    duration += Time.deltaTime;
                    cooldownImage.fillAmount = duration / (abilityBase.activeTime + abilityBase.cooldownTime);
                }
                else
                {
                    // 스킬 시전이 끝나면 종료 로직
                    abilityBase.DeAtivate(joystick, gameObject, poolingManager);

                    // 스킬 시전이 끝나면 쿨다운 상태
                    state = AbilityState.cooldown;
                }
                break;
            
            // 스킬 쿨다운
            case AbilityState.cooldown:
                if (cooldownTime > 0)
                {
                    // 쿨다운시간 감소
                    cooldownTime -= Time.deltaTime;

                    // 스킬 쿨타임 이미지 표시(스킬쿨타임 = 스킬유지시간 + 쿨다운시간)
                    duration += Time.deltaTime;
                    cooldownImage.fillAmount = duration / (abilityBase.activeTime + abilityBase.cooldownTime);
                }
                else
                {
                    // 쿨다운이 끝나면 스킬 사용 가능한 상태
                    state = AbilityState.ready;
                }
                break;
        }
    }

    // 각 스킬버튼 온클릭함수에 추가해서 버튼이 눌렸는지 체크하는 함수
    public void AbilityPressed() { if (state == AbilityState.ready) abilityPressed = true; }
}
