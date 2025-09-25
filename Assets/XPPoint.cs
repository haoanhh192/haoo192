
using UnityEngine;

using static D2D.Utilities.CommonGameplayFacade;

public class XPPoint : MonoBehaviour
{
    [SerializeField] private float xp;

    public void Init(Vector3 originPoint)
    {
        GetComponent<Rigidbody>().AddExplosionForce(_gameData.pickUpFlyForce, originPoint, 3);

        Invoke("ChangeLayerToXP", _gameData.timeBeforeXPActivate);
    }
    public float PickUp()
    {
        Destroy(gameObject);

        return xp;
    }

    private void ChangeLayerToXP()
    {
        gameObject.layer = LayerMask.NameToLayer(_gameData.XPLayer);
    }
}