using UnityEngine;

public class StaticSphere : MonoBehaviour
{
    public float BounceFactor;

    private void OnCollisionEnter(Collision collision)
    {
        var force = collision.contacts[0].normal;
        collision.rigidbody.AddForce(force*-BounceFactor, ForceMode.Impulse);
    }
}