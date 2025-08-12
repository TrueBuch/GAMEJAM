using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Radio : MonoBehaviour
{
    [SerializeField] private WaveChanger _waveChanger;
    [SerializeField] private Volume _volume;
    [SerializeField] private Tuning _tuning;
    [SerializeField] private Button _button;
    [SerializeField] private Image _dynamic;

    [SerializeField] private List<Sprite> _dynamicSprites;

    [SerializeField] private List<Sprite> _buttonSprites;

    [SerializeField] private AudioSource _noise;

    private int _dynamicIndex = 0;
    private float _currentTime = 0f;
    private Entity _wave;
    public Entity Wave => _wave;

    private bool _isEnabled = false;
    public bool IsEnabled => _isEnabled;

    public AudioSource Noise => Noise;

    private AudioSource _radio;
    public AudioSource AudioSource => _radio;

    public readonly UnityEvent<Entity, Entity> WaveChanged = new();
    private void Awake()
    {
        _button.onClick.AddListener(OnButtonClick);
        _radio = GetComponent<AudioSource>();
        _radio.loop = true;
        _noise.loop = true;
        _noise.volume = 0f;
        _tuning.ValueChanged.AddListener(ChangeClip);
    }

    private void OnButtonClick()
    {
        _isEnabled = !_isEnabled;
        _button.image.sprite = _isEnabled ? _buttonSprites[1] : _buttonSprites[0];
    }

    private void Start()
    {
        StartCoroutine(DynamicAnim());
        _tuning.Init();
        _volume.Init();
        _waveChanger.Init();

        ChangeWave(Main.ECS.Get("Wave/FM"));
        ChangeClip();
        _radio.volume = 0;
        _radio.Play();

        //_noise.clip = Content.AudioNoise;
        _noise.Play();

        //StartCoroutine(Shake());
    }

    private void Update()
    {
        _currentTime = Time.time;
        if (_isEnabled) _radio.volume = _volume.CurrentValue;
        else _radio.volume = 0;
    }

    private IEnumerator Shake()
    {
        while (true)
        {
            _radio.volume = 0f;
            yield return new WaitForSeconds(Random.Range(0.03f, 0.05f));
            _radio.volume = _volume.CurrentValue;
            yield return new WaitForSeconds(Random.Range(0.03f, 0.05f));
        }
    }

    private void ChangeClip()
    {
        var tagWave = Wave.Get<TagWave>();
        int index = tagWave.Ranges
        .Select((v, i) => new { Value = v, Index = i })
        .FirstOrDefault(x => tagWave.Enabled[x.Index] && x.Value.x <= _tuning.CurrentValue && _tuning.CurrentValue <= x.Value.y)
        ?.Index ?? -1;

        var oldClip = _radio.clip;
        var newClip = index == -1 ? null : tagWave.Clips[index];
        if (oldClip != newClip)
        {
            if (newClip == null) return;
            _radio.clip = newClip;
            var time = _currentTime % newClip.length;
            _radio.time = time;
            _radio.Play();
        }
    }

    public void ChangeWave(Entity wave)
    {
        var old = _wave;
        _wave = wave;
        WaveChanged.Invoke(old, _wave);
        ChangeClip();
    }

    public IEnumerator DynamicAnim()
    {
        while (true)
        {
            if (!_isEnabled)
            {
                yield return null;
                continue;
            }

            _dynamicIndex = (_dynamicIndex + 1) % _dynamicSprites.Count;
            _dynamic.sprite = _dynamicSprites[_dynamicIndex];
            yield return new WaitForSecondsRealtime(0.25f); 
        }
    }
}
public class TagWave : Tag
{
    public float Min = 3f;
    public float Max = 30f;

    [HorizontalGroup(Width = 100)] public List<bool> Enabled = new();
    [HorizontalGroup] public List<string> Keys = new();
    [HorizontalGroup] public List<AudioClip> Clips = new();
    [MinMaxSlider(nameof(Min), nameof(Max), ShowFields = true)]
    [HorizontalGroup] public List<Vector2Int> Ranges = new();
}