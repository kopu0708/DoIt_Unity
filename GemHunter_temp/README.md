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
  
  -----
