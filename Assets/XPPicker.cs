using D2D;
using UnityEngine;

public class XPPicker : MonoBehaviour
{
    [SerializeField] private float getDistance = 1f;
    [SerializeField] private float pickUpForce = 5f;

    private SexyOverlap overlap;

    private void Awake()
    {
        overlap = GetComponent<SexyOverlap>();
    }
    private void FixedUpdate()
    {
        if (overlap.HasTouch)
        {
            foreach (var item in overlap.AllTouched)
            {
                if (item == null)
                {
                    continue;
                }

                var distance = Vector3.Distance(transform.position, item.transform.position);

                if (distance <= getDistance)
                {
                    item.GetComponent<XPPoint>().PickUp();

                    return;
                }

                if (item.attachedRigidbody == null)
                {
                    return;
                }

                item.attachedRigidbody.isKinematic = true;

                float speed = pickUpForce - distance;
                speed = speed * Time.fixedDeltaTime;
                item.transform.position = Vector3.MoveTowards(item.transform.position, transform.position, speed);
            }
        }
    }
}