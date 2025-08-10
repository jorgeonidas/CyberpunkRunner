using UnityEngine;

public class SpeedBoost : PickUp
{
    [SerializeField] float _adjustChangeMoveSpeedAmount = 3f;
    protected override void OnPickUp()
    {
        //LevelGenerator.OnChangeSpeedAmount?.Invoke(_adjustChangeMoveSpeedAmount);
    }
}
