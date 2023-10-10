using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BetUpHandler : MonoBehaviour
{
    public void betUpHandler()
    {
        if (GameMgr.inst.currentState == GameMgr.GameState.PreDeal)
        {
            if (GameMgr.inst.betAmt < 5)
            {
                GameMgr.inst.betAmt += 1;
            }
            else if (GameMgr.inst.betAmt == 5)
            {
                GameMgr.inst.betAmt = 1;
            }
        }
    }
}
