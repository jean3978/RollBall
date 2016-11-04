using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneState : MonoBehaviour
{
    public enum SceneStates
    {
        Restart,
        Pause,
        Win,
        Loose,
        Play
    }

    private readonly Stopwatch _stopwatch = new Stopwatch();
    public SceneStates State = SceneStates.Play;

    public Text StateText;
    public Text StatsText;


    private void Update()
    {
        if (Input.GetKeyUp("r"))
        {
            SceneManager.LoadScene("minigame");
            Time.timeScale = 1.0f;
        }
        if (Input.GetKeyUp("p"))
        {
            Pause();
        }

        switch (State)
        {
            case SceneStates.Play:
                StateText.text = "";
                break;
            case SceneStates.Loose:
                StateText.text = "You Loose!";
                FreezeAndRestart();
                break;
            case SceneStates.Win:
                StateText.text = "You Win!";
                FreezeAndRestart();
                break;

            case SceneStates.Pause:
                StateText.text = "Pause...";
                break;

            default:
                StateText.text = "";
                break;
        }
        StatsText.text = "Deaths: " + GameStat.Stastistic.Deaths;
    }


    private void FreezeAndRestart()
    {
        if (!_stopwatch.IsRunning)
        {
            _stopwatch.Start();
            Time.timeScale = 0.0f;
            if (State == SceneStates.Loose)
            {
                GameStat.Stastistic.Deaths++;
            }
            if (State == SceneStates.Win)
            {
                GameStat.Stastistic.Deaths = 0;
            }
        }
        else if (_stopwatch.IsRunning && _stopwatch.ElapsedMilliseconds >= 500)
        {
            _stopwatch.Stop();
            _stopwatch.Reset();
            SceneManager.LoadScene("minigame");

            Time.timeScale = 1.0f;
        }
    }

    private void Pause()
    {
        if (State == SceneStates.Pause)
        {
            State = SceneStates.Play;
            Time.timeScale = 1.0f;
        }
        else
        {
            State = SceneStates.Pause;
            Time.timeScale = 0.0f;
        }
    }
}