using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class OrcCharacter : MonoBehaviour
{
    private readonly Transform[] _pathLocations = new Transform[12];
    private readonly Stopwatch _stopwatch = new Stopwatch();
    private int _clockwise = 1;
    private int _currentMarker;

    private float _endingAngle;
    private OrcState _orcState;

    private readonly string _path = "MrBadGuy/WalkingPath/path";
    private SceneState _sceneState;
    private float _startingAngle;

    private float _startLineTime;
    private bool _walkingFoward = true;


    private readonly List<string> debug = new List<string>();
    public GameObject Magic;
    public float RotationSpeed = 150f;

    public float Speed = 5.0f;

    private void Start()
    {
        _sceneState = FindObjectOfType<SceneState>();

        for (var i = 0; i < 12; i++)
        {
            _pathLocations[i] = GameObject.Find(_path + (i + 1)).transform;
            Console.WriteLine(_pathLocations[i]);
        }
        transform.position = _pathLocations[0].position;
        transform.rotation = _pathLocations[_currentMarker].rotation;
        Magic = GameObject.FindWithTag("MagicPickup");
        ChangeState(OrcState.Walking);
        _stopwatch.Start();
    }

    private void Update()
    {
        switch (_orcState)
        {
            case OrcState.Walking:
                TranslateOrc();
                DropMagicPickUp();
                break;
            case OrcState.Rotating:
                RotateOrc();
                break;
        }

        DropMagicPickUp();
    }

    private void DropMagicPickUp()
    {
        if (_stopwatch.ElapsedMilliseconds >= 3000)
        {
            Instantiate(Magic, new Vector3(transform.position.x, 1, transform.position.z), transform.rotation);
            _stopwatch.Reset();
            _stopwatch.Start();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _sceneState.State = SceneState.SceneStates.Loose;
        }
    }

    private void ChangeState(OrcState state)
    {
        switch (state)
        {
            case OrcState.Walking:
                GetComponent<Animation>().Play("orcwalk");
                _startLineTime = Time.time;
                break;
            case OrcState.Rotating:
                InitializeRotation();
                break;
        }

        _orcState = state;
    }

    private int GetNextMarker()
    {
        if (_walkingFoward)
            return _currentMarker == _pathLocations.Length - 1 ? _currentMarker - 1 : _currentMarker + 1;

        return _currentMarker == 0 ? _currentMarker + 1 : _currentMarker - 1;
    }

    private void InitializeRotation()
    {
        var cY = _pathLocations[_currentMarker].eulerAngles.y;
        var nY = _pathLocations[GetNextMarker()].eulerAngles.y;
        var orcY = transform.rotation.eulerAngles.y;

        _startingAngle = orcY;

        if (_walkingFoward)
            _endingAngle = cY;
        else
            _endingAngle = nY + 180f >= 360 ? nY - 180f : nY + 180f;

        var diff = _endingAngle - _startingAngle;
        if (diff < 0)
            diff += 360f;

        _clockwise = diff <= 180 ? 1 : -1;
    }

    private void RotateOrc()
    {
        var orcY = transform.rotation.eulerAngles.y;
        var diff = orcY - _endingAngle;

        transform.Rotate(new Vector3(0, RotationSpeed*_clockwise*Time.deltaTime, 0));

        debug.Add(diff.ToString());
        if (-10 < diff && diff < 10)
        {
            ChangeState(OrcState.Walking);
        }
    }

    private void TranslateOrc()
    {
        var nextMarker = GetNextMarker();

        var orcPosition = transform.position;
        var startLinePosition = _pathLocations[_currentMarker].position;
        var endLinePosition = _pathLocations[nextMarker].position;

        var lineLength = Vector3.Distance(startLinePosition, endLinePosition);
        var distCovered = (Time.time - _startLineTime)*Speed;
        var fracJourney = distCovered/lineLength;

        transform.position = Vector3.Lerp(_pathLocations[_currentMarker].position, _pathLocations[nextMarker].position,
            fracJourney);


        if (endLinePosition.x == orcPosition.x && endLinePosition.z == orcPosition.z)
        {
            _currentMarker = nextMarker;
            if (_currentMarker == 0 || _currentMarker == _pathLocations.Length - 1)
                _walkingFoward = !_walkingFoward;

            ChangeState(OrcState.Rotating);
        }
    }

    private enum OrcState
    {
        Walking,
        Rotating
    }
}