using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseState
{
    protected MonsterController monster;

    public BaseState(MonsterController monster)
    {
        this.monster = monster;
    }

    public abstract void Enter();
    public abstract void Update();
    public abstract void Exit();
}