using UnityEngine;

// 플레이어 상태를 넣어 줄 건데
// 괜히 만든 것 같기도 하고
public class BaseAblity
{
    public enum State
    {
        Idle,
        Run,
        Jump,
        Fall,
        Dash,
        Attack,
        Glide,
        Hit,
        Dead,
        Ignore
    }

}
