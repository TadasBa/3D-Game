using System.Collections.Generic;
using UnityEngine;

public class PowerUpManager : MonoBehaviour
{
    private List<JumpBoostPowerup> powerUps = new List<JumpBoostPowerup>();

    public void RegisterPowerUp(JumpBoostPowerup powerUp)
    {
        powerUps.Add(powerUp);
    }

    public void RespawnPowerUps()
    {
        foreach (var powerUp in powerUps)
        {
            if (powerUp != null)
            {
                powerUp.Respawn();
            }
        }
    }
}
