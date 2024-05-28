using UnityEngine;

public class JumpBoostPowerup : MonoBehaviour
{
    public float boostAmount = 5f;
    public float duration = 3f;

    public Vector3 initialPosition;
    public PowerUpManager powerUpManager;

    void Start()
    {
        initialPosition = transform.position;
        powerUpManager = FindObjectOfType<PowerUpManager>();
        if (powerUpManager != null)
        {
            powerUpManager.RegisterPowerUp(this);
        }
        else
        {
            Debug.LogError("PowerUpManager not found in the scene.");
        }
    }

    public void Respawn()
    {
        transform.position = initialPosition;
        gameObject.SetActive(true);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                player.Powerup(boostAmount, duration);
                gameObject.SetActive(false); // Deactivate the power-up after pickup
            }
        }
    }
}
