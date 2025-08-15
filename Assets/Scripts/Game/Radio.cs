using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RadioState
{
    public Wave CurrentWave;
    public Dictionary<string, Wave> Waves = new();

    public RadioState()
    {
        Waves.Add("FM", new Wave("FM"));
        Waves.Add("SW", new Wave("SW"));
        Waves.Add("AM", new Wave("AM"));
        Waves.Add("MENUFM", new Wave("MENUFM"));
        Waves.Add("MENUAM", new Wave("MENUAM"));
        Waves.Add("MENUSW", new Wave("MENUSW"));
    }
}

public class Wave
{
    public float Min = 0;
    public float Max = 0;

    public Entity entity;

    public List<bool> Enabled = new();
    public Wave(string key)
    {
        entity = Main.ECS.Get($"Wave/{key}");
        var tagWave = entity.Get<TagWave>();
        
        Enabled = new(tagWave.Enabled);
        Min = tagWave.Min;
        Max = tagWave.Max;
    }
}

public class Radio : MonoBehaviour, ISingleton
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
    private RadioState _state;
    public RadioState State => _state;

    [SerializeField] private bool _isEnabled = false;
    public bool IsEnabled => _isEnabled;

    public AudioSource Noise => Noise;

    private AudioSource _radio;
    public AudioSource AudioSource => _radio;

    public readonly UnityEvent<Wave, Wave> WaveChanged = new();
    private void Awake()
    {
        _button.onClick.AddListener(OnButtonClick);
        _button.image.sprite = _isEnabled ? _buttonSprites[1] : _buttonSprites[0];
        _radio = GetComponent<AudioSource>();
        _radio.loop = true;
        _noise.loop = true;
        _noise.volume = 0.03f;
        _tuning.ValueChanged.AddListener(ChangeClip);
        _state = new();

        
    }

    private void OnButtonClick()
    {
        Change(!IsEnabled);
    }

    public void Change(bool isEnabled)
    {
        _noise.clip = _state.CurrentWave.entity.Get<TagWave>().Noise;
        if (isEnabled) _noise.Play();
        else _noise.Stop();
        _isEnabled = isEnabled;
        _button.image.sprite = _isEnabled ? _buttonSprites[1] : _buttonSprites[0];
    }

    private void Start()
    {
        StartCoroutine(DynamicAnim());
        _tuning.Init();
        _volume.Init();

        ChangeWave(SceneManager.GetActiveScene().name == "Main" ? _state.Waves["FM"] : _state.Waves["MENUFM"]);
        ChangeClip();
        _radio.volume = 0;
        _radio.Play();

        _noise.clip = _state.CurrentWave.entity.Get<TagWave>().Noise;
        Change(false);
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

    public void ChangeClip()
    {
        var wave = _state.CurrentWave;
        var tagWave = wave.entity.Get<TagWave>();

        int index = tagWave.Ranges
        .Select((v, i) => new { Value = v, Index = i })
        .FirstOrDefault(x => wave.Enabled[x.Index] && x.Value.x <= _tuning.CurrentValue && _tuning.CurrentValue <= x.Value.y)
        ?.Index ?? -1;

        var oldClip = _radio.clip;
        var newClip = index == -1 ? null : tagWave.Clips[index];
        if (oldClip != newClip)
        {
            _radio.clip = newClip;
            if (_radio.clip != null)
            {
                var time = _currentTime % newClip.length;
                _radio.time = time;
            }

            _radio.Play();
            if (SceneManager.GetActiveScene().name == "Main")
            {
                var events = Main.EventSystem.FindAll<IOnClipChanged>();
                foreach (var e in events) StartCoroutine(e.OnChanged(oldClip, newClip));
            }
        }
    }

    public void ChangeWave(Wave wave)
    {
        var old = _state.CurrentWave;
        _state.CurrentWave = wave;
        WaveChanged.Invoke(old, _state.CurrentWave);
        if (SceneManager.GetActiveScene().name == "Main")
        {
            var events = Main.EventSystem.FindAll<IOnWaveChanged>();
            foreach (var e in events) StartCoroutine(e.OnChanged(old, _state.CurrentWave));
        }

        _noise.clip = _state.CurrentWave.entity.Get<TagWave>().Noise;
        if (IsEnabled) _noise.Play();
        else _noise.Stop();
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

    public Wave Wave(string wave)
    {
        return State.Waves[wave];
    }

    public bool IsCurrent(string name)
    {
        if (!IsEnabled) return false;
        var wave = State.CurrentWave.entity.Get<TagWave>();

        var enabled = wave.Clips
            .Select((clip, i) => new { clip, key = wave.Keys[i], enabled = State.CurrentWave.Enabled[i] })
            .Where(x => x.enabled)
            .ToList();

        var index = enabled.FindIndex(x => x.clip == _radio.clip);
        return index != -1 && enabled[index].key == name;
    }

    public void SetEnable(string waveName, string name, bool bol)
    {
        var wave = Wave(waveName);
        wave.Enabled[wave.entity.Get<TagWave>().Keys.IndexOf(name)] = bol;
        if (bol) Debug.Log($"{name} - enabled");
        else Debug.Log($"{name} - disabled");
        ChangeClip();
    }

    public void Initialize() { }
}
public class TagWave : Tag
{
    [SerializeField] public AudioClip Noise;
    public float Min = 3f;
    public float Max = 30f;

    [HorizontalGroup(Width = 100)] public List<bool> Enabled = new();
    [HorizontalGroup] public List<string> Keys = new();
    [HorizontalGroup] public List<AudioClip> Clips = new();
    [MinMaxSlider(-13, nameof(Max), ShowFields = true)]
    [HorizontalGroup] public List<Vector2Int> Ranges = new();
}

public interface IOnWaveChanged { public IEnumerator OnChanged(Wave oldWave, Wave newWave); }
public interface IOnClipChanged { public IEnumerator OnChanged(AudioClip oldClip, AudioClip newClip); }