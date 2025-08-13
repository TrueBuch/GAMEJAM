using System.Collections;
using UnityEditor.Localization.Plugins.XLIFF.V12;
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
        var radio = Main.Get<Radio>();
        radio.SetEnable("FM", "start", true);
        Debug.Log("102 enabled");
    }

}

public class Listen102Wave : Event, IOnClipChanged, IOnGameStarted
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
        if (!Main.Get<Radio>().IsCurrent("start")) yield break;

        _invoked = true;
        var subs = Main.Get<Subtitles>();
        yield return new WaitForSecondsRealtime(3f);

        subs.TypeByKey(true, "not_find");
        yield return new WaitUntil(() => !subs.IsPlaying);
        subs.TypeByKey(true, "not_find_1");
        yield return new WaitUntil(() => !subs.IsPlaying);
        Main.Get<Gazeta>().GazetaFull.Change(1);
    }
}

public class Enable990Wave : Event, IOnGazetaOpened, IOnGameStarted
{
    private bool _invoked = false;
    public IEnumerator OnStarted()
    {
        _invoked = false;
        yield break;
    }

    public IEnumerator OnOpened(int index)
    {
        if (index != 1) yield break;
        if (_invoked) yield break;

        _invoked = true;
        var radio = Main.Get<Radio>();
        radio.SetEnable("AM", "cosmos", true);
        
        Debug.Log("990 enabled");
    }
}

public class Listen990Wave : Event, IOnGameStarted, IOnClipChanged
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
        if (!Main.Get<Radio>().IsCurrent("cosmos")) yield break;

        _invoked = true;

        var radio = Main.Get<Radio>();
        var subs = Main.Get<Subtitles>();
        radio.SetEnable("SW", "bipbip", true);

        yield return new WaitForSecondsRealtime(5f);
        subs.TypeByKey(true, "cosmos");
        yield return new WaitUntil(() => !subs.IsPlaying);
        Main.Get<Clock>().ChangeDon();
        Debug.Log("18 enabled");
    }
}

public class Sw18Listen : Event, IOnClipChanged, IOnGameStarted
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
        var radio = Main.Get<Radio>();
        var subs = Main.Get<Subtitles>();

        if (!Main.Get<Radio>().IsCurrent("bipbip")) yield break;
        _invoked = true;
        yield return new WaitForSecondsRealtime(5f);

        var state = Main.Get<Radio>().State;
        subs.TypeByKey(true, "listening_sw");
        yield return new WaitUntil(() => !subs.IsPlaying);
        subs.TypeByKey(true, "listening_sw_1");
        yield return new WaitUntil(() => !subs.IsPlaying);
        subs.TypeByKey(true, "listening_sw_2");
        yield return new WaitUntil(() => !subs.IsPlaying);
        yield return new WaitForSecondsRealtime(2f);
        radio.SetEnable("SW", "bipbip", false);
        radio.SetEnable("SW", "bipbip_code", true);
        yield return new WaitForSecondsRealtime(2.5f);

        Main.Get<Radio>().ChangeClip();
        Main.Get<Notebook>().notebook = true;
    }
}

public class OBERNIS : Event, IOnNotebookUp, IOnGameStarted
{
    private bool _invoked = false;
    public IEnumerator OnStarted()
    {
        _invoked = false;
        yield break;
    }

    public IEnumerator OnUp()
    {
        if (_invoked) yield break;
        if (!Main.Get<Notebook>().notebook) yield break;
        _invoked = true;

        var subs = Main.Get<Subtitles>();
        Main.Get<Notebook>().AddText("ОБЕРНИСЬ");
        subs.TypeByKey(true, "looked_at_notebook");
        yield return new WaitUntil(() => !subs.IsPlaying);
        subs.TypeByKey(true, "looked_at_notebook_1");
        yield return new WaitUntil(() => !subs.IsPlaying);
        Main.Get<Clock>().ChangeDon();
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
        Main.Get<Gazeta>().GazetaFull.Change(2);
        Main.Get<Painting>().ChangeState(1);
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