using System.Collections;
using UnityEngine;

public class Game : MonoBehaviour, ISingleton
{
    public AudioClip windowTuc;
    public AudioClip windowScary;
    public AudioSource Source;
    public bool FMEvent = false;
    public int ListenCount = 0;
    public bool MonsterSays = false;
    public AudioClip FinalNoise;

    public void Initialize()
    {
        Main.SceneTransition.TransitionCompleted.AddListener(OnSceneStarted);
    }

    private void OnSceneStarted(bool arg0)
    {
        Debug.Log("Game Started");
        var events = Main.EventSystem.FindAll<IOnGameStarted>();
        foreach (var e in events) StartCoroutine(e.OnStarted());

        //StartEnding(false);
    }

    public void CheckListenCount()
    {
        if (ListenCount < 3) return;
        Debug.Log("FMEvent completed");
        var events = Main.EventSystem.FindAll<IOnFMEventCompleted>();
        foreach (var e in events) StartCoroutine(e.OnCompleted());
    }

    public void StartEnding(bool isFirst)
    {
        Main.Get<Radio>().Change(false);
        Main.Get<Clock>().gameObject.SetActive(false);
        Main.Get<Ending>().StarEndind(isFirst);
    }
}

public class StartingSentence : Event, IOnGameStarted
{
    public IEnumerator OnStarted()
    {
        yield return new WaitForSecondsRealtime(1f);
        var subs = Main.Get<Subtitles>();
        subs.TypeByKey(Voice.PLAYER, true, "start");
        yield return new WaitUntil(() => !subs.IsPlaying);
        subs.TypeByKey(Voice.PLAYER, true, "start_1");
        yield return new WaitUntil(() => !subs.IsPlaying);
        yield return new WaitForSecondsRealtime(2f);
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
    }

}

public interface IOnFMEventCompleted
{
    public IEnumerator OnCompleted();
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

        subs.TypeByKey(Voice.PLAYER, true, "not_find");
        yield return new WaitUntil(() => !subs.IsPlaying);
        subs.TypeByKey(Voice.PLAYER, true, "not_find_1");
        yield return new WaitUntil(() => !subs.IsPlaying);
        yield return new WaitForSecondsRealtime(2f);
        Main.Get<Radio>().SetEnable("FM", "start", false);
        Main.Get<Radio>().SetEnable("FM", "start_morse", true);
        Main.Get<Gazeta>().GazetaFull.Change(1);
    }
}

public class OnListenStartMorse : Event, IOnClipChanged
{
    private bool IsPlaying = false;
    public IEnumerator OnChanged(AudioClip oldClip, AudioClip newClip)
    {
        if (newClip == null) yield break;
        if (!Main.Get<Radio>().IsCurrent("start_morse")) yield break;
        // --. .- --.. . - .- газета
        if (!IsPlaying)
        {
            IsPlaying = true;
            var subs = Main.Get<Subtitles>();
            subs.Type(Voice.NONE, true, 0.25f, "[ --.");

            yield return new WaitForSecondsRealtime(2f);
            if (!Main.Get<Radio>().IsCurrent("start_morse"))
            {
                IsPlaying = false;
                yield break;
            }
            subs.Type(Voice.NONE, true, 0.25f, ".-");
            
            yield return new WaitForSecondsRealtime(2f);
            if (!Main.Get<Radio>().IsCurrent("start_morse"))
            {
                IsPlaying = false;
                yield break;
            }
            subs.Type(Voice.NONE, true, 0.25f, "--..");
            
            yield return new WaitForSecondsRealtime(2f);
            subs.Type(Voice.NONE, true, 0.25f, ".");

            yield return new WaitForSecondsRealtime(2f);
            if (!Main.Get<Radio>().IsCurrent("start_morse"))
            {
                IsPlaying = false;
                yield break;
            }
            subs.Type(Voice.NONE, true, 0.25f, "-");
            yield return new WaitForSecondsRealtime(2f);
            if (!Main.Get<Radio>().IsCurrent("start_morse"))
            {
                IsPlaying = false;
                yield break;
            }
            subs.Type(Voice.NONE, true, 0.25f, ".- ]");
            yield return new WaitUntil(() => !subs.IsPlaying);
            if (!Main.Get<Radio>().IsCurrent("start_morse"))
            {
                IsPlaying = false;
                yield break;
            }
            IsPlaying = false;
            Main.Get<Game>().StartCoroutine(OnChanged(oldClip, newClip));
            IsPlaying = false;
        }

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
    }
}

public class OnGlitchedGazetaOpened : Event, IOnGazetaOpened, IOnGameStarted
{
    private bool _invoked = false;
    public IEnumerator OnStarted()
    {
        _invoked = false;
        yield break;
    }
    public IEnumerator OnOpened(int index)
    {
        if (_invoked) yield break;
        if (index != 2) yield break;
        _invoked = true;
        var subs = Main.Get<Subtitles>();
        subs.TypeByKey(Voice.PLAYER, true, "looked_at_gazeta");
        yield return new WaitUntil(() => !subs.IsPlaying);
        Main.Get<Painting>().ChangeState(1);
        Main.Get<Radio>().SetEnable("FM", "start", false);
        Main.Get<Radio>().SetEnable("FM", "R3", true);
        Main.Get<Game>().FMEvent = true;
        Debug.Log("FMEvent started");
    }
}

public class Listen990Wave : Event, IOnClipChanged
{
    //.---- ----. -.- --- ... -- --- ...
    private bool IsPlaying = false;
    private bool _invoked = false;
    public IEnumerator OnStarted()
    {
        _invoked = false;
        yield break;
    }
    public IEnumerator OnChanged(AudioClip oldClip, AudioClip newClip)
    {
        
        if (newClip == null) yield break;
        if (!Main.Get<Radio>().IsCurrent("cosmos")) yield break;

        var radio = Main.Get<Radio>();
        var subs = Main.Get<Subtitles>();
        radio.SetEnable("SW", "bipbip", true);
        if (!IsPlaying)
        {
            IsPlaying = true;

            subs.Type(Voice.NONE, true, 0.25f, "[ .----");
            yield return new WaitForSecondsRealtime(2f);
            if (!Main.Get<Radio>().IsCurrent("cosmos"))
            {
                IsPlaying = false;
                yield break;
            }
            subs.Type(Voice.NONE, true, 0.25f, "----.");

            yield return new WaitForSecondsRealtime(2f);
            if (!Main.Get<Radio>().IsCurrent("cosmos"))
            {
                IsPlaying = false;
                yield break;
            }
            subs.Type(Voice.NONE, true, 0.25f, "-.-");

            yield return new WaitForSecondsRealtime(2f);
            if (!Main.Get<Radio>().IsCurrent("cosmos"))
            {
                IsPlaying = false;
                yield break;
            }
            subs.Type(Voice.NONE, true, 0.25f, "---");

            yield return new WaitForSecondsRealtime(2f);
            if (!Main.Get<Radio>().IsCurrent("cosmos"))
            {
                IsPlaying = false;
                yield break;
            }
            subs.Type(Voice.NONE, true, 0.25f, "...");

            yield return new WaitForSecondsRealtime(2f);
            if (!Main.Get<Radio>().IsCurrent("cosmos"))
            {
                IsPlaying = false;
                yield break;
            }
            subs.Type(Voice.NONE, true, 0.25f, "--");

            yield return new WaitForSecondsRealtime(2f);
            if (!Main.Get<Radio>().IsCurrent("cosmos"))
            {
                IsPlaying = false;
                yield break;
            }
            subs.Type(Voice.NONE, true, 0.25f, "---");

            yield return new WaitForSecondsRealtime(2f);
            if (!Main.Get<Radio>().IsCurrent("cosmos"))
            {
                IsPlaying = false;
                yield break;
            }
            subs.Type(Voice.NONE, true, 0.25f, "... ]");
            yield return new WaitUntil(() => !subs.IsPlaying);
            if (!Main.Get<Radio>().IsCurrent("cosmos"))
            {
                IsPlaying = false;
                yield break;
            }
            IsPlaying = false;
            Main.Get<Game>().StartCoroutine(OnChanged(oldClip, newClip));
        }


        if (_invoked) yield break;
        _invoked = true;
        Main.Get<Clock>().ChangeDon(1);
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
        subs.TypeByKey(Voice.PLAYER, true, "listening_sw");
        yield return new WaitUntil(() => !subs.IsPlaying);
        subs.TypeByKey(Voice.PLAYER, true, "listening_sw_1");
        yield return new WaitUntil(() => !subs.IsPlaying);
        subs.TypeByKey(Voice.PLAYER, true, "listening_sw_2");
        yield return new WaitUntil(() => !subs.IsPlaying);
        yield return new WaitForSecondsRealtime(2f);
        radio.SetEnable("SW", "bipbip", false);
        radio.SetEnable("SW", "bipbip_noise", true);
        yield return new WaitForSecondsRealtime(1.5f);

        subs.TypeByKey(Voice.PLAYER, true, "sound_interrupted");
        yield return new WaitUntil(() => !subs.IsPlaying);
        subs.TypeByKey(Voice.PLAYER, true, "sound_interrupted_1");
        yield return new WaitUntil(() => !subs.IsPlaying);
        radio.SetEnable("SW", "bipbip_noise", false);
        radio.SetEnable("SW", "bipbip_morse", true);
    }
}

public class Sw18MorzeListen : Event, IOnClipChanged
{
    private bool IsPlaying = false;
    //-... .-.. --- -.- -. --- -
    public IEnumerator OnChanged(AudioClip oldClip, AudioClip newClip)
    {
        if (newClip == null) yield break;

        if (!Main.Get<Radio>().IsCurrent("bipbip_morse")) yield break;

        var subs = Main.Get<Subtitles>();
        if (!IsPlaying)
        {
            if (Main.Get<Notebook>().scary) yield break;
            IsPlaying = true;
            subs.Type(Voice.NONE, true, 0.25f, "[ -...");

            yield return new WaitForSecondsRealtime(2f);
            if (!Main.Get<Radio>().IsCurrent("bipbip_morse") || (Main.Get<Notebook>().scary))
            {
                IsPlaying = false;
                yield break;
            }
            subs.Type(Voice.NONE, true, 0.25f, ".-..");

            yield return new WaitForSecondsRealtime(2f);
            if (!Main.Get<Radio>().IsCurrent("bipbip_morse") || (Main.Get<Notebook>().scary))
            {
                IsPlaying = false;
                yield break;
            }
            subs.Type(Voice.NONE, true, 0.25f, "---");

            yield return new WaitForSecondsRealtime(2f);
            if (!Main.Get<Radio>().IsCurrent("bipbip_morse") || (Main.Get<Notebook>().scary))
            {
                IsPlaying = false;
                yield break;
            }
            subs.Type(Voice.NONE, true, 0.25f, "-.-");

            yield return new WaitForSecondsRealtime(2f);
            if (!Main.Get<Radio>().IsCurrent("bipbip_morse") || (Main.Get<Notebook>().scary))
            {
                IsPlaying = false;
                yield break;
            }
            subs.Type(Voice.NONE, true, 0.25f, "-.");

            yield return new WaitForSecondsRealtime(2f);
            if (!Main.Get<Radio>().IsCurrent("bipbip_morse") || (Main.Get<Notebook>().scary))
            {
                IsPlaying = false;
                yield break;
            }
            subs.Type(Voice.NONE, true, 0.25f, "---");

            yield return new WaitForSecondsRealtime(2f);
            if (!Main.Get<Radio>().IsCurrent("bipbip_morse") || (Main.Get<Notebook>().scary))
            {
                IsPlaying = false;
                yield break;
            }

            subs.Type(Voice.NONE, true, 0.25f, "- ]");
            yield return new WaitUntil(() => !subs.IsPlaying);
            if (!Main.Get<Radio>().IsCurrent("bipbip_morse") || (Main.Get<Notebook>().scary))
            {
                IsPlaying = false;
                yield break;
            }
            IsPlaying = false;
            Main.Get<Game>().StartCoroutine(OnChanged(oldClip, newClip));
        }

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
        Main.Get<Notebook>().scary = true;
        var subs = Main.Get<Subtitles>();
        Main.Get<Notebook>().AddText("ОБЕРНИСЬ");
        subs.TypeByKey(Voice.PLAYER, true, "looked_at_notebook");
        yield return new WaitUntil(() => !subs.IsPlaying);
        subs.TypeByKey(Voice.PLAYER, true, "looked_at_notebook_1");
        yield return new WaitUntil(() => !subs.IsPlaying);
        Main.Get<Clock>().ChangeDon(2);
    }
}

public class ListenR3Wave : Event, IOnClipChanged, IOnGameStarted
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
        if (!radio.IsCurrent("R3")) yield break;
        if (!Main.Get<Game>().FMEvent) yield break;
        _invoked = true;

        yield return new WaitForSecondsRealtime(3f);

        radio.SetEnable("FM", "R3", false);
        radio.SetEnable("FM", "R3_noise", true);
        Debug.Log("R3 Listened");
        Main.Get<Game>().ListenCount++;
        Main.Get<Game>().CheckListenCount();
    }
}

public class ListenR2Wave : Event, IOnClipChanged, IOnGameStarted
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
        if (!radio.IsCurrent("R2")) yield break;
        if (!Main.Get<Game>().FMEvent) yield break;
        _invoked = true;

        yield return new WaitForSecondsRealtime(3f);

        radio.SetEnable("FM", "R2", false);
        radio.SetEnable("FM", "R2_noise", true);
        Debug.Log("R2 Listened");
        Main.Get<Game>().ListenCount++;
        Main.Get<Game>().CheckListenCount();
    }
}

public class ListenR1Wave : Event, IOnClipChanged, IOnGameStarted
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
        var radio = Main.Get<Radio>();
        if (newClip == null) yield break;
        if (!radio.IsCurrent("R1")) yield break;
        if (!Main.Get<Game>().FMEvent) yield break;
        _invoked = true;

        yield return new WaitForSecondsRealtime(3f);

        radio.SetEnable("FM", "R1", false);
        radio.SetEnable("FM", "R1_noise", true);
        Debug.Log("R1 Listened");
        Main.Get<Game>().ListenCount++;
        Main.Get<Game>().CheckListenCount();
    }
}

public class Listen666Wave : Event, IOnClipChanged, IOnGameStarted
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
        var radio = Main.Get<Radio>();
        if (newClip == null) yield break;
        if (!radio.IsCurrent("devil")) yield break;
        Debug.Log("666 open");
        if (!Main.Get<Window>().CodeViewed) yield break;
        Debug.Log("666 played");
        _invoked = true;
        yield return new WaitForSecondsRealtime(6f);
        Main.Get<Book>().ChangeState(false);
        var subs = Main.Get<Subtitles>();
        yield return new WaitForSecondsRealtime(2f);
        subs.TypeByKey(Voice.PLAYER, true, "looked_at_nnm");
    }
}

public class FirstDon : Event, IOnDonChanged
{
    private bool _invoked = false;
    public IEnumerator OnStarted()
    {
        _invoked = false;
        yield break;
    }

    public IEnumerator OnChanged(int value)
    {
        if (_invoked) yield break;
        if (value != 1) yield break;
        _invoked = true;
    }
}

public class SecondDon : Event, IOnDonChanged
{
    private bool _invoked = false;
    public IEnumerator OnStarted()
    {
        _invoked = false;
        yield break;
    }

    public IEnumerator OnChanged(int value)
    {
        if (_invoked) yield break;
        if (value != 2) yield break;
        _invoked = true;
        Main.Get<Gazeta>().GazetaFull.Change(2);
    }
}

public class ThirdDon : Event, IOnDonChanged
{
    private bool _invoked = false;
    public IEnumerator OnStarted()
    {
        _invoked = false;
        yield break;
    }

    public IEnumerator OnChanged(int value)
    {
        if (value != 3) yield break;
        _invoked = true;
        Main.Get<Painting>().ChangeState(2);
        Main.Get<Eye>().On();
    }
}

public class FourthDon : Event, IOnDonChanged, IOnGameStarted
{
    private bool _invoked = false;
    public IEnumerator OnStarted()
    {
        _invoked = false;
        yield break;
    }

    public IEnumerator OnChanged(int value)
    {
        if (value != 4) yield break;
        if (_invoked) yield break;
        _invoked = true;
        yield return new WaitForSecondsRealtime(8f);
        var subs = Main.Get<Subtitles>();
        subs.TypeByKey(Voice.MONSTER, true, "last_din");
        yield return new WaitUntil(() => !subs.IsPlaying);
        subs.TypeByKey(Voice.MONSTER, true, "last_din_1");
        Main.Get<Radio>().State.Waves["SW"].Min = -13;
        Main.Get<Radio>().SetEnable("SW", "minus_wave", true);
        Main.Get<Game>().MonsterSays = true;
        yield return new WaitUntil(() => !subs.IsPlaying);

        Main.Get<CanvasChanger>().ChangeCanvas(1);
        yield return new WaitForSecondsRealtime(2f);
        subs.TypeByKey(Voice.MONSTER, true, "enter_code");
        yield return new WaitUntil(() => !subs.IsPlaying);
        subs.TypeByKey(Voice.MONSTER, true, "enter_code_1");
        yield return new WaitUntil(() => !subs.IsPlaying);
        subs.TypeByKey(Voice.MONSTER, true, "enter_code_2");
        yield return new WaitUntil(() => !subs.IsPlaying);
    }
}

public class OnRotatedMinusWave : Event, IOnCanvasChanged, IOnGameStarted
{
    private bool _invoked = false;
    public IEnumerator OnStarted()
    {
        _invoked = false;
        yield break;
    }

    public IEnumerator OnChanged(int index)
    {
        if (!Main.Get<Game>().MonsterSays) yield break;
        if (_invoked) yield break;

        _invoked = true;
        var subs = Main.Get<Subtitles>();
        subs.TypeByKey(Voice.MONSTER, true, "rotated");
    }
}

public class OnChairClickedMonster : Event, IOnChairClicked, IOnGameStarted
{
    private bool _invoked = false;
    public IEnumerator OnStarted()
    {
        _invoked = false;
        yield break;
    }
    public IEnumerator OnClicked(bool IsNormal)
    {
        if (_invoked) yield break;
        if (IsNormal) yield break;
        if (!Main.Get<Game>().MonsterSays) yield break;
        _invoked = true;

        var subs = Main.Get<Subtitles>();

        subs.TypeByKey(Voice.MONSTER, true, "ending_1_clicked_on_chair");
        yield return new WaitUntil(() => !subs.IsPlaying);
        subs.TypeByKey(Voice.MONSTER, true, "ending_1_clicked_on_chair_1");
        yield return new WaitUntil(() => !subs.IsPlaying);
        subs.TypeByKey(Voice.MONSTER, true, "ending_1_clicked_on_chair_2");
        yield return new WaitUntil(() => !subs.IsPlaying);
        yield return new WaitForSecondsRealtime(2f);
        Main.Get<Game>().StartEnding(true);
    }
}

public class ListenMinus13Wave : Event, IOnClipChanged, IOnGameStarted
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

        if (!Main.Get<Radio>().IsCurrent("minus_wave")) yield break;
        _invoked = true;

        yield return new WaitForSecondsRealtime(4f);

        subs.TypeByKey(Voice.MONSTER, true, "ending_2_found");
        yield return new WaitUntil(() => !subs.IsPlaying);
        subs.TypeByKey(Voice.MONSTER, true, "ending_2_found");
        yield return new WaitUntil(() => !subs.IsPlaying);
        yield return new WaitForSecondsRealtime(1f);
        Main.Get<Game>().Source.panStereo = Main.Get<CanvasChanger>().GetPanStereo(0);
        Main.Get<Game>().Source.loop = true;
        Main.Get<Game>().Source.clip = Main.Get<Game>().windowScary;
        Main.Get<Game>().Source.Play();
        Main.Get<Window>().StartScary();
    }
}

public class ScaryWindowViewed : Event, IOnCanvasChanged, IOnGameStarted
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
        if (index != 0) yield break;
        if (!Main.Get<Window>().IsScary) yield break;

        var radio = Main.Get<Radio>();
        var subs = Main.Get<Subtitles>();

        _invoked = true;
        subs.TypeByKey(Voice.MONSTER, true, "ending_2_flash");
        yield return new WaitUntil(() => !subs.IsPlaying);

        yield return new WaitForSecondsRealtime(4f);
        Main.Get<Game>().StartEnding(false);
    }
}

public class Print666 : Event, IOnGameStarted, IOnFMEventCompleted
{
    private bool _invoked = false;
    public IEnumerator OnStarted()
    {
        _invoked = false;
        yield break;
    }

    public IEnumerator OnCompleted()
    {
        if (_invoked) yield break;
        _invoked = true;

        var subs = Main.Get<Subtitles>();
        Main.Get<Window>().ViewCode();
        Main.Get<Radio>().SetEnable("AM", "devil", true);
        Main.Get<Game>().Source.volume = 0.5f;
        Main.Get<Game>().Source.panStereo = Main.Get<CanvasChanger>().GetPanStereo(0);
        Main.Get<Game>().Source.PlayOneShot(Main.Get<Game>().windowTuc);
        yield return new WaitForSecondsRealtime(1f);
        subs.TypeByKey(Voice.PLAYER, true, "knock_on_the_window");
        yield return new WaitUntil(() => !subs.IsPlaying);
        subs.TypeByKey(Voice.PLAYER, true, "knock_on_the_window_1");
        Main.Get<Chair>().ChangeState(false);
    }
}

public class OnCodeSee : Event, IOnCanvasChanged, IOnGameStarted
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
        if (index != 0) yield break;
        if (!Main.Get<Window>().CodeViewed) yield break;
        _invoked = true;
        Main.Get<Clock>().ChangeDon(3);
        var subs = Main.Get<Subtitles>();
        subs.TypeByKey(Voice.PLAYER, true, "looked_at_window");
        Main.Get<Painting>().ChangeState(2);
        Main.Get<Eye>().On();
    }
}

public class DevilBookOpened : Event, IOnGameStarted, IOnPageChanged
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
        if (Main.Get<Book>().IsNormal) yield break;
        _invoked = true;
        Main.Get<Radio>().SetEnable("SW", "devil_clip", true);
    }
}

public class SW13Listened : Event, IOnGameStarted, IOnClipChanged
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
        var radio = Main.Get<Radio>();
        if (newClip == null) yield break;
        if (!radio.IsCurrent("devil_clip")) yield break;
        _invoked = true;
        var subs = Main.Get<Subtitles>();
        yield return new WaitForSecondsRealtime(5f);
        subs.TypeByKey(Voice.PLAYER, true, "found_code_in_nnm");
        yield return new WaitForSecondsRealtime(15f);

        Main.Get<Clock>().ChangeDon(4);
    }
}

public class NormalChairClicked : Event, IOnChairClicked
{
    public IEnumerator OnClicked(bool IsNormal)
    {
        if (!IsNormal) yield break;

        var subs = Main.Get<Subtitles>();
        subs.Type(Voice.PLAYER, true, "Классный стул");
    }
}

public class NeNormalChairClicked : Event, IOnChairClicked
{
    public IEnumerator OnClicked(bool IsNormal)
    {
        if (IsNormal) yield break;
        if (Main.Get<Game>().MonsterSays) yield break; 

        var subs = Main.Get<Subtitles>();
        subs.Type(Voice.PLAYER, true, "Но как оно там оказалось?");
    }
}

public interface IOnGameStarted { public IEnumerator OnStarted(); }