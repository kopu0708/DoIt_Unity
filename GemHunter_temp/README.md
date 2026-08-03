# GemHunter 
## 참고
- 원본 튜토리얼: Do it! 유니티로 배우는 게임 개발

------
# 2026-07-29 학습 기록

## 오늘 배운 내용
- ScriptableObject를 활용한 데이터 관리 (StageData) (아 이런 식으로 데이터를 저장해두고 읽게 하는구나)
- Vector3가 값 타입(구조체)이라 transform.position을 직접 수정 못하는 이유
- Mathf.Clamp로 카메라 이동 범위 제한하기
- 사소한 것 이긴 한데 스크립트를 짤 때 하위 폴더를 만들어 분류를 하더라 이렇게 하니깐 찾기 쉬운듯
- [RequireComponent(typeof(Rigidbody2D))] 이런 어트리뷰트를 붙여주면 해당 스크립트가 부착되어 있는 오브젝트는 자동으로 컴포넌트를 부착해준다.
- 예전에는 Update에서 입력키를 감지하고 이동을 처리하는 구식 Input Manager 방식(Input.GetKey 등)을 썼던 것 같은데, 
  이번에 새로운 Input System(InputAction, PlayerInput) 사용법을 처음 봤다.
  
## 코드 작성하며 헷갈렸던 부분
- `transform.position.x = 5f;` 가 왜 컴파일 에러가 나는지 몰랐는데,
  Vector3가 구조체(값 타입)라서 프로퍼티가 반환하는 게 "복사본"이기 때문이었다.
  → 그래서 변수에 복사 → 수정 → 재대입 패턴을 써야 함
  
- StageData 스크립트를 짜면서 'public Vector2 CameraLimitMin => cameraLimitMin' 이 부분이 무슨 역할인지 몰랐음
  다른 스크립트에서 데이터를 읽을 수 있게 해주는 읽기 전용 프로퍼티를 식 본문 멤버로 쓴 거였다. 난 왜 람다식이 나오는가 했었는데

## 좀 더 알아봐야 할 것
- 새 Input System: PlayerInput 컴포넌트의 Behavior 옵션 차이, Input Actions 에셋 구조

  -----

# 2026-07-30 ~ 31 학습 기록
## 오늘 배운 내용
- 상속 구조 설계: EntityStats(데이터) → EntityBase(공통 로직) → PlayerBase / EnemyBase(각자 특화)
  → 데이터랑 로직을 분리하고, 공통되는 부분은 부모 클래스에 몰아두는 이유를 이해함
- protected 접근 제한자: 자기 자신 + 자식 클래스에서만 접근 가능
  → 캡슐화 지키면서 상속 구조에서 필드 공유할 때 씀
- virtual / override / base 키워드로 다형성(polymorphism) 이해
  → 부모 타입 변수에 자식 객체를 담아도 실제 객체의 오버라이드된 메서드가 실행되는 것
  → EnemyBase.Setup()에서 MaxHp 먼저 계산하고 base.Setup()으로 currenHp 초기화하는 순서가 중요했음
- 읽기 전용 프로퍼티(`=>`)와 `{ get; set; }` 자동 구현 프로퍼티의 차이
  → `=>`는 매번 계산만 하고 값을 저장 안 함 (IsDead 같은 경우)
  → `{ get; set; }`는 값을 저장하는 그릇일 뿐, 자동으로 안 바뀌고 누군가 직접 대입해줘야 함
- IEnumerable / IEnumerator 차이
  → IEnumerable: foreach로 순회 가능한 컬렉션 자체
  → IEnumerator: 코루틴에서 쓰는 것 (이름 비슷해서 헷갈리기 쉬움)
- UnityAction(델리게이트)과 콜백 패턴
  → 메서드 자체를 변수처럼 담아서 나중에 실행(Invoke)할 수 있게 하는 것
  → action?.Invoke()로 null 체크하면서 안전하게 실행 (콜백 없을 때 대비)
- ScaleEffect 스크립트로 코루틴 + 델리게이트 + Lerp 보간을 한 번에 복습
  (StartCoroutine, yield return null, Vector3.Lerp)
- EnemySpawner: 타일맵(Tilemap) 기반으로 스폰 가능한 좌표 계산해서 몬스터 랜덤 배치
  → CompressBounds(), GetTilesBlock, CellToWorld 등 타일맵 API 처음 봄

## 코드 작성하며 헷갈렸던 부분
- IsMoved를 왜 굳이 Update() 안의 지역 변수에서 프로퍼티로 뺐는지 처음엔 이해 못함
  → 지역 변수는 Update() 함수 안에서만 존재해서 다른 스크립트(스킬 시스템 등)에서 접근 불가능
  → 여러 스크립트에서 공유해야 하는 값이면 프로퍼티로 빼야 한다는 걸 배움
- UIHP 스크립트에서 필드 타입(EnemyBase)이랑 Setup() 매개변수 타입(EntityBase)이 안 맞아서
  타입 에러 나는 걸 놓칠 뻔함 → 상속 관계에서 타입 통일 중요성 체감
- action?.Invoke()에서 `?.`가 정확히 뭘 하는 건지 처음엔 몰랐음
  → if (action != null) { action.Invoke(); } 를 한 줄로 줄인 문법이라는 걸 이해

## 좀 더 알아봐야 할 것
- EnemySpawner에서 같은 타일에 몬스터가 겹쳐서 생성될 수 있는 문제 (랜덤 인덱스 중복 가능성)
  → 나중에 겹침 버그 생기면 여기부터 확인해볼 것
- IEnumerable을 실제로 매개변수 타입으로 써보는 연습 (아직 코드에서 직접 써본 적은 없음, LINQ 쪽도 마찬가지)
- 교재 다 끝나면 책 안 보고 스스로 기능 하나 추가해보기 (반복 숙달만으론 부족하다는 걸 깨달음)

# 2026-08-03 학습 기록 
## 학습 목표 
- 확장 가능한 모듈식 스탯 시스템을 설계하고 이벤트 기반 구조(delegate, event)를 구현할 수 있다.
- ScriptableObject를 활용하여 스킬 데이터를 정의하고 게임 로직에 적용하기

## 오늘 배운 내용
- 스탯에는 기본으로 설정한 값과 스킬, 아이템 등을 추가로 얻는 보너스 값이 있는데, 해당 스탯을 사용할 때는 더한 값을 사용하지만, 경우에 따라서는 구분해서 사용해야 할 때도 있다. 또한 값이 바뀌거나 최솟값이나 최댓값에 근접했을 때 유연하게 대응할 수 있도록 delegate,event을 이용해 메서드를 등록해야한다. 스텟 정보를 구조체로 저장해두고 쓰면 아이템이나 스텟 보너스를 적용할 때 스탯 변수에 보너스를 적용해야하는 식으로 코드를 짜야하고 새로운 버프나 스탯이 추가할 때마다 코드를 추가해야한다. 하지만 스탯 클래스를 이용해 작성하면 코드를 추가하지 않아도 된다. 
