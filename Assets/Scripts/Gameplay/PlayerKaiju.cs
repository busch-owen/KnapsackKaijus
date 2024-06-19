using UnityEngine;

public class PlayerKaiju : Kaiju
{
    protected override void Die()
    {
        //Allow player to swap a new Kaiju into battle so long as there is another Kaiju to swap in
        base.Die();
    }
}
