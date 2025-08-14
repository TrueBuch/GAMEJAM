using System.Collections;
using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    private AudioClip[] _clips;

    private void Start()
    {
        StartCoroutine(PreloadClips());
        StartCoroutine(Hello());
    }

    private IEnumerator PreloadClips()
    {
        _clips = Resources.LoadAll<AudioClip>("Audio");

        foreach (var clip in _clips)
        {
            AudioSource.PlayClipAtPoint(clip, Vector3.zero, 0f);
            yield return null;
        }
    }

    private IEnumerator Hello()
    {
        var subs = Main.Get<Subtitles>();
        subs.Type(Voice.NONE, false, "Игра сделана за 7 дней\n для геймджема от MyIndie\n\n by TrueBuch & Bruhman");
        yield return new WaitUntil(() => !subs.IsPlaying);
        yield return new WaitForSecondsRealtime(1f);
        Main.SceneTransition.SwitchToScene("Menu");
    }
        

}