using UnityEngine;

/// <summary>
/// MonoBehaviour class used o control the movement for the canon
/// </summary>
public class CanonMovement : MonoBehaviour
{
    private const float speed = 260f;
    public GameObject cannonBubble;
    private float move;
    private float z;

    private void Start()
    {
        GetComponent<HingeJoint>().connectedBody = cannonBubble.GetComponent<Rigidbody>();
    }

    private void Update()
    {
        move = Input.GetAxis("Horizontal");
        z = move * speed;
        z = Mathf.Clamp(z , -80, 80);

        transform.localEulerAngles =  new Vector3(0f, 0f, z);
    }
}
