⭐️ 2022 MWU 공모전에 제출한 로그라이크 1인 프로젝트입니다.

<br>

![히어로그 썸넬](https://github.com/user-attachments/assets/891b6551-3507-476f-8912-2959f1ba2ab1)

<br>

# 프로젝트 개요
| 개발 환경 | Unity |
|:------:|:------:|
| 개발 기간 | 2022/07/05 ~ ing  |
| 게임 설명 | 로그라이크 RPG |
| 공모전 참여 | 2022 MWU |

<br>

# 시연 영상  
+ [Herogue](<https://youtu.be/cZ1Evc3VNB4>)

<br>

# 주요 기능  
⚡ 절차적 던전 시스템 [[스크립트 폴더](https://github.com/LeeJungHwi/Herogue_./tree/main/Scripts/ProceduralMap)]
- 랜덤맵 생성 : 현재 스테이지에 따라 레벨 디자인
- 몬스터 스폰 : 현재 스테이지에 따라 레벨 디자인
- 미니맵 표시 : 초록색(플레이어), 하얀색(일반), 빨간색(보스, 최장거리) [[스크립트](https://github.com/LeeJungHwi/Herogue_./blob/main/Scripts/ProceduralMap/RoomBFS.cs)]
- ![ProceduralGif](https://github.com/LeeJungHwi/Herogue_./assets/101587101/a126e706-bac7-454e-b062-4bc513dd9e8a)

<br>

⚡ 스킬 시스템 [[스크립트 폴더](https://github.com/LeeJungHwi/Herogue_./tree/main/Scripts/Ability)]
- 스킬 데이터 : Scriptable Object
- ![스킬시스템 스크립터블](https://github.com/LeeJungHwi/Herogue_./assets/101587101/0845e6f3-af8b-46e3-9f1d-b9c36d579f62)

<br>

- 스킬 UML
  - Ability Base : Scriptable Object 상속 ⇒ 모든 스킬 공통 멤버 정의
  - Ability Implement : Ability Base 상속 ⇒ 각 스킬 구체화
  - Ability FSM : Ability Base 참조 ⇒ 각 FSM의 스킬 데이터가 갖는 스킬 상태 관리
  - Pooling Manager : 스킬 시전 또는 종료 시 스킬 오브젝트와 사운드 풀링
- ![스킬 다이어그램 final drawio](https://github.com/LeeJungHwi/Herogue_./assets/101587101/4cd1082d-7f22-421a-a937-f3f710a36156)

<br>

- 스킬 FSM
- ![스킬시스템 UML drawio](https://github.com/LeeJungHwi/Herogue_./assets/101587101/53f58779-1144-4bf6-948b-5c489e2e3099)

<br>

- 스킬 충돌 : Particle Collision
- ![스킬 시전](https://github.com/LeeJungHwi/Herogue_./assets/101587101/0ef1202b-e0d0-4b20-af96-39009da38374)

<br>

⚡ 퀘스트 시스템 [[스크립트 폴더](https://github.com/LeeJungHwi/Herogue_./tree/main/Scripts/Quest)]
- 퀘스트 데이터 : Scriptable Object
- ![퀘스트 스크립터블](https://github.com/LeeJungHwi/Herogue_./assets/101587101/869d7074-b98c-4d57-836a-db3a2954f3fb)

<br>

- 퀘스트 UML
  - Quest Base : Scriptable Object 상속 ⇒ 모든 퀘스트 공통 멤버 정의
  - Count Base : Quest Base 상속 ⇒ 카운트 기반 공통 멤버 정의, 프로퍼티를 사용해서 셋될 때 완료조건 체크
  - Objective Base : Quest Base 상속 ⇒ 목표 기반 공통 멤버 정의
  - ILoop : 반복 퀘스트 인터페이스 멤버 정의
  - ISequential : 순차 퀘스트 인터페이스 멤버 정의
  - Quest Implement : 기반 퀘스트를 상속, 필요 시 반복 퀘스트와 순차 퀘스트를 다중상속해서 각 퀘스트 구현
  - Quest Manager : Quest Base형 리스트를 참조해서 퀘스트 진행 상황 관리
- ![퀘스트 다이어그램 drawio](https://github.com/LeeJungHwi/Herogue_./assets/101587101/ccc545cb-1289-4eaf-9caa-36e8fa1c5b45)

<br>

⚡ 인벤토리 시스템 [[스크립트 폴더](https://github.com/LeeJungHwi/Herogue_./tree/main/Scripts/InventoryShop)]
- 아이템 데이터 : User Class ⇒ 각 아이템이 참조
- ![인벤토리 유저 클래스](https://github.com/LeeJungHwi/Herogue_./assets/101587101/8e08c4bc-1ef0-426b-afd3-17fc79303a3f)

<br>

- 아이템 ↔ 플레이어 상호작용 : 아이템 획득과 장비 장착
- ![아이템 플레이어 상호작용](https://github.com/LeeJungHwi/Herogue_./assets/101587101/e7572b30-1f6d-418c-94e8-ba9894081221)

<br>

- 아이템 ↔ 상점 상호작용 : 아이템 구매와 사용
- ![아이템 상점 상호작용](https://github.com/LeeJungHwi/Herogue_./assets/101587101/d31e10de-43e0-4a1d-8934-e6baa531c69b)

<br>

⚡ 풀 시스템 [[스크립트](https://github.com/LeeJungHwi/Herogue_./blob/main/Scripts/Manager/PoolingManager.cs)]
- 오브젝트 풀링 : 무기, 스킬, 코인, 박스, 몬스터, 맵, 장식, 이펙트, 체력바, 플로팅 텍스트, 사운드, 아이템, 펫, ETC...
- ![풀링 인스펙터](https://github.com/LeeJungHwi/Herogue_./assets/101587101/8c467dce-1084-400f-bc19-0967816ce8d7)

<br>

- 사운드 풀링 : 자주 사용되는 사운드, 특히 스킬 시전 사운드는 무조건 재사용이 필요했음 
- ![스킬 풀링](https://github.com/LeeJungHwi/Herogue_./assets/101587101/6dcc60f9-fb30-4819-ae7d-eafb609a80d4)

<br>

⚡ 세이브 시스템 [[스크립트 폴더](Scripts/Save)]
- Json 형태로 저장 및 로드

<br>

✨시행착오와 개발 후기
- 기술적 어려움
  - 해결1 : 함수의 호출 스택을 따라가거나 로그 찍어보기
  - 해결2 : try~catch를 통한 예외처리하기
  - 해결3 : static과 flag 필드 사용하기

<br>

- 가독성 저하와 유지보수 어려움
  - 원인1 : 프로젝트 초기 설계 고민 시간에 투자 하지 않았음
  - 원인2 : 구현 후 기능이 정상 작동 한다면 코드 검토를 후순위로 미뤘음
  - 해결1 : 각 함수가 하나의 책임을 맡을 수 있도록 분리하기
  - 해결2 : 함수 내부 중복되는 코드조각은 하위 함수로 분리하기
 
<br>

- 구체적인 설계의 중요성
  - 개발 초기 설계보다 구현에 집중 하는 것이 효율적이라고 생각했음
  - 개발 진행 과정에서 간단한 기능의 추가에도 여러 곳에 접근 해야 하는 상황이 많아졌음
  - 앞으로의 개발에서는 구현 전 충분한 설계 시간을 갖도록 노력 중임
 
<br>

- 코드의 지속적인 검토와 리팩토링의 중요성
  - 구현한 기능이 정상적으로 작동 한다면 코드의 검토를 후순위로 미뤘음
  - 프로젝트의 규모가 커지면서 중복코드와 불필요한 코드가 많아져 예외처리에 시간이 오래걸림
  - 코드 리팩토링을 진행하면서 더 간결한 코드를 작성하고 프로젝트 구조를 다시 정리 할 수 있었음
 
<br>

- 기반지식의 중요성([Tech](https://river-pearl-643.notion.site/Hwi-s-Tech-c5062a7c67824137b8fc15cd002c91ec?pvs=4))
  - 컴파일러 : 스킬 FSM을 컴파일러 시간에 배운 전이함수에 따른 상태전이로 이해했음
  - 소프트웨어공학 : 클래스 구조를 설계 할 때 소공 시간에 배운 UML이 도움되었음
  - 객체지향 언어 : 상속 관계 설계 시 객체지향 언어 시간에 배운 상속과 다형성이 도움되었음
  - 알고리즘 : 보스방을 위한 최장거리를 구하거나 미니맵 표시를 위해 bfs 알고리즘을 사용했음
