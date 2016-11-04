using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private int _count;
    private OrcCharacter _orc;
    private Rigidbody _rb;
    private SceneState _sceneState;
    private int _totalPickups;
    public Text CountText;

    public float Speed;

    private void Start()
    {
        _sceneState = FindObjectOfType<SceneState>();
        _orc = FindObjectOfType<OrcCharacter>();
        _rb = GetComponent<Rigidbody>();
        _totalPickups = GameObject.FindGameObjectsWithTag("PickUp").Length;
        _count = 0;
        SetCountText();
    }

    private void FixedUpdate()
    {
        var moveHorizontal = Input.GetAxis("Horizontal");
        var moveVertical = Input.GetAxis("Vertical");

        var movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        _rb.AddForce(movement*Speed);

        if (transform.position.y < -10)
            _sceneState.State = SceneState.SceneStates.Loose;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PickUp"))
        {
            other.gameObject.SetActive(false);
            _count++;
            SetCountText();
        }
        if (other.gameObject.CompareTag("MagicPickup"))
        {
            var color = other.GetComponent<Renderer>().material.color;
            other.gameObject.SetActive(false);
            var cubes = GameObject.FindGameObjectsWithTag("MagicCube");
            foreach (var cube in cubes)
            {
                cube.GetComponent<Renderer>().material.color = color;
            }
        }
        if (other.gameObject.CompareTag("Hole"))
        {
            _sceneState.State = SceneState.SceneStates.Loose;
        }
        if (other.gameObject.CompareTag("MrBadGuy"))
        {
            _sceneState.State = SceneState.SceneStates.Loose;
        }
    }

    private void SetCountText()
    {
        CountText.text = "Count " + _count + " / " + _totalPickups;
        if (_count >= _totalPickups)
        {
            _sceneState.State = SceneState.SceneStates.Win;
        }
    }
}