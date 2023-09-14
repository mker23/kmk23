using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : GenericSingleton<GameManager>
{
    public int playerScore = 0;                         //관리할 플레이어 스코어

    public void InscreaseScore(int amount)
    {
        playerScore += amount;
    }
}
