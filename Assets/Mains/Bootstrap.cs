using System.Collections;
using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    private AudioClip[] _clips;

    private void Start()
    {
        StartCoroutine(Hello());
    }

    private IEnumerator PreloadClips()
    {
        _clips = Resources.LoadAll<AudioClip>("Audio");

        foreach (var clip in _clips)
        {
            AudioSource.PlayClipAtPoint(clip, Vector3.zero, 0f);
        }
        yield return null;
    }

    private IEnumerator Hello()
    {
        var subs = Main.Get<Subtitles>();
        subs.Type(Voice.NONE, false, 0.05f, "Игра сделана за 7 дней\nдля геймджема от MyIndie\n\n\nby Bruhman & Truebuch\n[ Zvezdnaya 58 Team ]");
        yield return new WaitUntil(() => !subs.IsPlaying);
        StartCoroutine(PreloadClips());
        yield return new WaitForSecondsRealtime(2f);
        Main.SceneTransition.SwitchToScene("Menu");
    }
        

}