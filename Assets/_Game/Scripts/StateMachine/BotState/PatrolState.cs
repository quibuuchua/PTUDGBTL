using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : IState
{

    public void OnEnter(Bot bot)
    {
        bot.ChangeAnimation(Constants.ANIMATION_RUN);
        bot.SetDestination(bot.RandomBrickPos());
    }

    public void OnExecute(Bot bot)
    {
        if (bot.IsReachTarget())
        {
            bot.SetDestination(bot.RandomBrickPos());
        }

        if (bot.BuildBridge())
        {   
            bot.ChangeState(new BuildState());
        }
    }

    public void OnExit(Bot bot)
    {

    }
}
