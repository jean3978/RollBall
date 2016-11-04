using UnityEngine;

public class MagicPickup : MonoBehaviour
{
    public Renderer rend;

    private void Start()
    {
        rend = GetComponent<Renderer>();

        rend.material.color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
    }

    // Update is called once per frame
    private void Update()
    {
    }
}