using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BuildState : IState
{

    public void OnEnter(Bot bot)
    {
        bot.ChangeAnimation(Constants.ANIMATION_RUN);
        bot.SetDestination(bot.GetRandomResetPointPos());
    }

    public void OnExecute(Bot bot)
    {
        if (bot.IsReachTarget())
        {
            bot.SetDestination(bot.finishBox.position);
        }
    }

    public void OnExit(Bot bot)
    {

    }
}
