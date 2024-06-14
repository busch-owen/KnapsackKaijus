using UnityEngine;

public class TurnHandler : MonoBehaviour
{
    public void DetermineFirstKaiju(Kaiju k1, Kaiju k2, int playerMoveIndex)
    {
        if (k1.LocalSpeed > k2.LocalSpeed)
        {
            k1.Attack(k2, playerMoveIndex);
            Debug.Log("Player Went First");
        }
        else
        {
            //This is just temporary, the attack on K2 will be a random choice from the AI
            k2.Attack(k1, playerMoveIndex);
            Debug.Log("Enemy Went First");
        }
    }
}
