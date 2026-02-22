# Enemy Spawn System (Single Player, Fully Data-Driven)

---

# 1. System Overview

본 시스템은 하드코딩 없이 적을 소환하기 위한 싱글플레이 전용 데이터 기반 구조이다.

## Goals

- 몬스터 추가 시 코드 수정 없음
- 스폰 시간 변경 시 코드 수정 없음
- ID 기반 몬스터 관리
- 확장 가능한 구조
- 유지보수 용이

---

# 2. Architecture

```
EnemyConfig (ScriptableObject)
        ↓
EnemyRegistry (Auto Load)
        ↓
Dictionary<int, GameObject>
        ↓
SpawnSchedule (ScriptableObject)
        ↓
EnemySpawner
```

---

# 3. Folder Structure

```
Assets/
 ├── Resources/
 │    └── EnemyConfigs/
 │          Goblin.asset
 │          Orc.asset
 │          Skeleton.asset
 │
 ├── Scripts/
 │    ├── EnemyConfig.cs
 │    ├── EnemyRegistry.cs
 │    ├── SpawnSchedule.cs
 │    └── EnemySpawner.cs
```

---

# 4. Implementation

---

## 4.1 EnemyConfig

각 몬스터가 자신의 ID와 Prefab을 보유하는 ScriptableObject.

### EnemyConfig.cs

```csharp
using UnityEngine;

[CreateAssetMenu(menuName = "Game/Enemy Config")]
public class EnemyConfig : ScriptableObject
{
    public int monsterId;
    public GameObject prefab;
}
```

### Usage

1. Create → Game → Enemy Config
2. monsterId 입력
3. Prefab 연결
4. Resources/EnemyConfigs/ 폴더에 저장

---

## 4.2 EnemyRegistry

EnemyConfig를 자동 로드하여 ID 기반 딕셔너리를 생성한다.

### EnemyRegistry.cs

```csharp
using System.Collections.Generic;
using UnityEngine;

public class EnemyRegistry : MonoBehaviour
{
    private Dictionary<int, GameObject> enemyDict = new();

    void Awake()
    {
        LoadAllConfigs();
    }

    void LoadAllConfigs()
    {
        EnemyConfig[] configs =
            Resources.LoadAll<EnemyConfig>("EnemyConfigs");

        foreach (var config in configs)
        {
            if (!enemyDict.ContainsKey(config.monsterId))
            {
                enemyDict.Add(config.monsterId, config.prefab);
            }
            else
            {
                Debug.LogError($"Duplicate Monster ID: {config.monsterId}");
            }
        }
    }

    public GameObject GetEnemy(int id)
    {
        if (enemyDict.TryGetValue(id, out var prefab))
            return prefab;

        Debug.LogError($"Monster ID not found: {id}");
        return null;
    }
}
```

### Scene Setup

- 빈 GameObject 생성
- EnemyRegistry 컴포넌트 추가

---

## 4.3 SpawnSchedule

스폰 시간과 위치를 정의하는 ScriptableObject.

### SpawnSchedule.cs

```csharp
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Game/Spawn Schedule")]
public class SpawnSchedule : ScriptableObject
{
    public List<SpawnEvent> spawnEvents;
}

[System.Serializable]
public class SpawnEvent
{
    public int monsterId;
    public float spawnTime;
    public Vector3 spawnPosition;
}
```

### Usage

1. Create → Game → Spawn Schedule
2. SpawnEvents 리스트에 데이터 추가
3. monsterId / spawnTime / spawnPosition 입력

---

## 4.4 EnemySpawner

SpawnSchedule을 읽어 시간에 맞춰 적을 생성한다.

### EnemySpawner.cs

```csharp
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public EnemyRegistry registry;
    public SpawnSchedule schedule;

    private float timer = 0f;
    private int currentIndex = 0;

    void Update()
    {
        timer += Time.deltaTime;

        if (currentIndex >= schedule.spawnEvents.Count)
            return;

        SpawnEvent spawnEvent = schedule.spawnEvents[currentIndex];

        if (timer >= spawnEvent.spawnTime)
        {
            Spawn(spawnEvent);
            currentIndex++;
        }
    }

    void Spawn(SpawnEvent data)
    {
        GameObject prefab = registry.GetEnemy(data.monsterId);
        if (prefab == null) return;

        Instantiate(prefab, data.spawnPosition, Quaternion.identity);
    }
}
```

### Scene Setup

- 빈 GameObject 생성
- EnemySpawner 컴포넌트 추가
- Registry에 EnemyRegistry 연결
- Schedule에 SpawnSchedule 연결

---

# 5. Runtime Flow

```
Game Start
    ↓
EnemyRegistry loads all EnemyConfigs
    ↓
Dictionary<int, Prefab> 생성
    ↓
EnemySpawner checks SpawnSchedule
    ↓
Time condition met
    ↓
Instantiate enemy
```

---

# 6. Adding New Enemy

1. Prefab 생성
2. EnemyConfig 생성
3. 고유 monsterId 입력
4. SpawnSchedule에 해당 ID 추가

코드 수정 없음.

---

# 7. Extending the System

향후 확장 가능 요소:

- Wave 기반 스폰 구조
- 확률 기반 랜덤 스폰
- 랜덤 위치 생성
- StageManager 통합
- Difficulty 기반 Schedule 교체
- 오브젝트 풀링 적용

---

# 8. Advantages

- Hardcoding 제거
- 데이터 중심 설계
- 코드 수정 없이 콘텐츠 확장 가능
- 유지보수 용이
- 스테이지 분리 가능
- 난이도 분리 가능

---

# 9. Summary

본 시스템은 싱글플레이 환경에서 사용하기 위한 완전한 데이터 기반 적 소환 구조이다.

몬스터 추가, 스폰 변경, 스테이지 분리는 모두 ScriptableObject 데이터 수정만으로 가능하며,  
코드 수정 없이 콘텐츠 확장이 가능하다.