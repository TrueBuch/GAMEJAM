using System.Collections;
using UnityEngine;

public class Game : MonoBehaviour, ISingleton
{

    public void Initialize() { }

    public void Start()
    {
        var events = Main.EventSystem.FindAll<IOnGameStarted>();
        foreach (var e in events) StartCoroutine(e.OnStarted());
    }
}

public class StartingSentence : Event, IOnGameStarted
{
    public IEnumerator OnStarted()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        var subs = Main.Get<Subtitles>();
        subs.TypeByKey("StartingSentence");
        while (subs.IsPlaying) yield return null;

        Debug.Log("Done");
    }
}

public interface IOnGameStarted
{
    public IEnumerator OnStarted();
}