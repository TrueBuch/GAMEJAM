using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class Game : MonoBehaviour, ISingleton
{
    public void Initialize()
    { 
        Main.SceneTransition.TransitionCompleted.AddListener(OnSceneStarted);
    }

    private void OnSceneStarted(bool arg0)
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
        subs.TypeByKey(true, "start");
        yield return new WaitUntil(() => !subs.IsPlaying);
        subs.TypeByKey(true, "start_1");
        yield return new WaitUntil(() => !subs.IsPlaying);
        yield return new WaitForSecondsRealtime(10f);
    }
}

public class Enable102Wave : Event, IOnPageChanged, IOnGameStarted
{
    private bool _invoked = false;
    public IEnumerator OnStarted()
    {
        _invoked = false;
        yield break;
    }
    public IEnumerator OnChanged(int index)
    {
        if (_invoked) yield break;
        if (index != 1) yield break;
        _invoked = true;
        var wave = Main.Get<Radio>().State.Waves["FM"];
        var waveIndex = wave.entity.Get<TagWave>().Keys.IndexOf("R3");
        wave.Enabled[waveIndex] = true;
        Debug.Log("102 enabled");
    }

}

public class Enable990Wave : Event, IOnClipChanged, IOnGameStarted
{
    private bool _invoked = false;
    public IEnumerator OnStarted()
    {
        _invoked = false;
        yield break;
    }
    public IEnumerator OnChanged(AudioClip oldClip, AudioClip newClip)
    {
        if (_invoked) yield break;
        if (newClip == null) yield break;
        
        
        var wave = Main.Get<Radio>().State.CurrentWave;
        var index = wave.entity.Get<TagWave>().Clips.IndexOf(newClip);

        if (wave.entity.Get<TagWave>().Keys[index] != "R3") yield break;
        _invoked = true;
        var subs = Main.Get<Subtitles>();
        yield return new WaitForSecondsRealtime(3f);

        subs.TypeByKey(true, "not_find");
        yield return new WaitUntil(() => !subs.IsPlaying);
        subs.TypeByKey(true, "not_find_1");
        yield return new WaitUntil(() => !subs.IsPlaying);

        var amWave = Main.ECS.Get("Wave/AM").Get<TagWave>();
        amWave.Enabled[amWave.Keys.IndexOf("cosmos")] = true;
        Debug.Log("990 enabled");
    }
}

public class Play990Subtitles : Event, IOnGameStarted
{
    private bool _invoked = false;
    public IEnumerator OnStarted()
    {
        _invoked = false;
        yield break;
    }
    public IEnumerator OnChanged(AudioClip oldClip, AudioClip newClip)
    {
        if (_invoked) yield break;
        if (newClip == null) yield break;
        var subs = Main.Get<Subtitles>();
        var wave = Main.Get<Radio>().State.CurrentWave.entity.Get<TagWave>();
        var index = wave.Clips.IndexOf(newClip);

        if (wave.Keys[index] != "cosmos") yield break;

        yield return new WaitForSecondsRealtime(5f);
        subs.TypeByKey(true, "cosmos");
        
    }
}

public class FirstDon : Event, IOnDonChanged
{
    public IEnumerator OnChanged(int value)
    {
        if (value != 1) yield break;
    }
}

public class SecondDon : Event, IOnDonChanged
{
    public IEnumerator OnChanged(int value)
    {
        if (value != 2) yield break;
    }
}

public class ThirdDon : Event, IOnDonChanged
{
    public IEnumerator OnChanged(int value)
    {
        if (value != 3) yield break;
    }
}

public class FourthDon : Event, IOnDonChanged
{
    public IEnumerator OnChanged(int value)
    {
        if (value != 4) yield break;
    }
}

public interface IOnGameStarted { public IEnumerator OnStarted(); }