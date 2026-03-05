using UnityEngine;
using System.Collections.Generic;

// HP_Slot의 자식(하트 아이콘)을 리스트로 관리.
// SetHp(currentHp) 호출 시 남은 HP 개수만큼만 활성화, 나머지는 비활성화.
public class PlayerHpUI : MonoBehaviour
{
    [SerializeField] Transform _hpSlot;
    [SerializeField] GameObject _hpIconPrefab;
    [Tooltip("자식이 없을 때만 사용. 비워두면 _hpSlot의 기존 자식 사용")]
    [SerializeField] int _maxHp = 6;

    private List<GameObject> _hpIcons = new List<GameObject>();

    private void Awake()
    {
        if (_hpSlot == null) _hpSlot = transform;

        // 기존 자식이 있으면 그걸 사용
        for (int i = 0; i < _hpSlot.childCount; i++)
            _hpIcons.Add(_hpSlot.GetChild(i).gameObject);

        // 자식이 없고 프리팹이 있으면 생성
        if (_hpIcons.Count == 0 && _hpIconPrefab != null)
        {
            for (int i = 0; i < _maxHp; i++)
            {
                var go = Instantiate(_hpIconPrefab, _hpSlot);
                _hpIcons.Add(go);
            }
        }
    }

    // 현재 HP에 맞게 아이콘 표시. currentHp 개수만큼만 활성화.
    public void SetHp(int currentHp)
    {
        for (int i = 0; i < _hpIcons.Count; i++)
            _hpIcons[i].SetActive(i < currentHp);
    }
}
