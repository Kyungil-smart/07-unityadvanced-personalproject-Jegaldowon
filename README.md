# 가시숲의 아이
<img width="1557" height="869" alt="스크린샷 2026-03-06 004933" src="https://github.com/user-attachments/assets/13327e23-c9aa-4551-bd3d-40700bb62884" />




    2D 액션 플랫폼 게임 프로젝트.  
    상태 패턴 기반 적 AI, 전투 시스템을 사용합니다.

---

## 프로젝트 개요

- **엔진**: Unity (2D)
- **입력**: Input System (키보드)
- **구조**: MainScene(메인 메뉴) → GameScene(인게임), MVP 패턴 적용

플레이어는 이동, 점프, 공격으로 적과 전투하고, HP가 0이 되면 사망합니다.  
지상 적은 패트롤 -> 추적 -> 공격 상태를 가지고, 비행형 적도 존재합니다.


## 개발 환경

- Unity 6000.3.91f1 LTS 
- Universal 2D

---

## 주요 기능

| 구분 | 내용 |
|------|------|
| **플레이어** | 좌우 이동, 점프, 3타 콤보 공격, HP(하트 UI), 사망·리스폰 |
| **적 AI** | 상태 패턴(Patrol / Chase / Attack / Dead), ScriptableObject 데이터 |
| **적 종류** | 지상형(MeleeEnemy), 비행형(FlyEnemy) |
| **전투** | 박스 히트 판정, 공격 구간 히트 윈도우, IDamageable 인터페이스 |
| **메인 메뉴** | START(게임 시작), How To(조작법), Exit(종료) |
| **인게임** | ESC 메뉴(일시정지·종료) |

---

## 조작 방법

### 메인 메뉴 (MainScene)

| 버튼 | 동작 |
|------|------|
| **START** | 게임 씬(GameScene)으로 이동 |
| **How To Play** | 조작법 패널 표시 |
| **Back** | How To Play 패널에서 메인 메뉴로 복귀 |
| **Exit** | 게임 종료 (빌드 시), 에디터에서는 재생 모드 종료 |

### 인게임 (GameScene)

| 입력 | 동작 |
|------|------|
| **← / →** | 좌우 이동 |
| **Z** | 점프 |
| **X** | 공격 (3타 콤보) |
| **ESC** | 일시정지 메뉴 열기/닫기 (메뉴에서 종료 가능) |

---

### [Notion 작업 페이지](https://www.notion.so/312463860b138026824cf9d1d3ee69c2?source=copy_link)

> Tile Map, State Pattern, State Machine, SO, Save 






