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
        yield return new WaitForSecondsRealtime(1f);
        var subs = Main.Get<Subtitles>();
        subs.TypeByKey("start");
        yield return new WaitUntil(() => !subs.IsPlaying);
        subs.TypeByKey("start_1");
        yield return new WaitUntil(() => !subs.IsPlaying);
    }
}

public class EnableFirstWave : IOnPageChanged
{
    public IEnumerator OnChanged()
    {
        var wave = Main.ECS.Get("Wave/FM").Get<TagWave>();
        var index = wave.Keys.IndexOf("R1");
        wave.Enabled[index] = true;
        yield return null;
    }
}

public interface IOnGameStarted { public IEnumerator OnStarted(); }

public interface IOnPageChanged { public IEnumerator OnChanged(); }