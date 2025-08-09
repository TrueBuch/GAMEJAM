using System.Collections;
using UnityEngine;

public class Radio : MonoBehaviour
{
    [SerializeField] private Volume _volume;
    [SerializeField] private Tuning _tuning;

    private AudioSource _audioSource;
    public AudioSource AudioSource => _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.loop = true;
    }

    private void Start()
    {
        _audioSource.clip = Content.AudioNoise;
        _audioSource.volume = 0;
        _audioSource.Play();
        StartCoroutine(AAASFFFF());
    }

    private void Update()
    {
        var volume = _volume.CurrentValue;
        var tuning = _tuning.CurrentValue;

        _audioSource.volume = volume;
    }

    private IEnumerator AAASFFFF()
    {
        while (true)
        {
            _audioSource.Pause();
            yield return new WaitForSeconds(Random.Range(0.03f, 0.05f));
            _audioSource.UnPause();
            yield return new WaitForSeconds(Random.Range(0.03f, 0.05f));
        }
    }
}