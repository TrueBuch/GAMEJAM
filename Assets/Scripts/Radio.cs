using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Rendering;

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
        _audioSource.clip = Content.AudioZnaesh;
        _audioSource.volume = 0;
        _audioSource.Play();
        StartCoroutine(AAASFFFF());
    }

    private void Update()
    {
        var volume = _volume.CurrentValue;
        var tuning = _tuning.CurrentValue;

        //_audioSource.volume = volume;
    }

    private IEnumerator AAASFFFF()
    {
        while (true)
        {
            _audioSource.volume = 0f;
            yield return new WaitForSeconds(Random.Range(0.03f, 0.05f));
            _audioSource.volume = _volume.CurrentValue;
            yield return new WaitForSeconds(Random.Range(0.03f, 0.05f));
        }
    }
}

public class TagWave : Tag
{
    [SerializeReference] SerializedDictionary<AudioClip, FloatRange> ClipsDON = new();
}
public class Waves
{

    // SW : 3MHz - 30MHz 
    // AM : 540KHz - 1600KHz 
    // FM : 88MHz - 108 MHz

    //ДИАПАЗОНЫ - АУДИОФАЙЛЫ
    //ПОМЕХИ
}

[System.Serializable]
public struct FloatRange
{
    
    [MinMaxSlider(3f, 30f, ShowFields = true)]
    public Vector2Int Range;

    public int Min => Range.x;
    public int Max => Range.y;
}