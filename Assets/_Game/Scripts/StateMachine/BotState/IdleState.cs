using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : IState
{

    public void OnEnter(Bot bot)
    {
        bot.ChangeAnimation(Constants.ANIMATION_IDLE);
    }

    public void OnExecute(Bot bot) 
    {
        bot.SetDestination(bot.transform.position);
    }

    public void OnExit(Bot bot)
    {

    }
}
