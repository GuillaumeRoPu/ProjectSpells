using UnityEngine;

public class GroundValues : MonoBehaviour
{
    [SerializeField] private float _slippingMeter;
    [SerializeField] private float _speedMult;

    public (float, float) GiveValues()
    {
        return (_slippingMeter, _speedMult);
    }
}
