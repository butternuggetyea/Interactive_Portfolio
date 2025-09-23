using UnityEngine;

public class BasketBall : InteractableObject
{
    public override void Interact() 
    {
        HeldItemPosition._instance.PickUpItem(gameObject);
    }


}
