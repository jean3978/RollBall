using UnityEngine;

public class GameStat : MonoBehaviour
{
    public static GameStat Stastistic;

    public int Deaths;

    private void Awake()
    {
        if (Stastistic == null)
        {
            DontDestroyOnLoad(gameObject);
            Stastistic = this;
        }
        else if (Stastistic != this)
        {
            Destroy(gameObject);
        }
    }
}