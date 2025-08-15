using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{
    public static Main Instance { get; private set; }

    private GameConfig _gameConfig;
    public static GameConfig GameConfig => Instance._gameConfig;

    private SceneTransition _sceneTransition;
    public static SceneTransition SceneTransition => Instance._sceneTransition;

    private Cursor _cursor;
    public static Cursor Cursor => Instance._cursor;

    private ECS ecs;
    public static ECS ECS => Instance.ecs;
    
    private GameEventSystem eventSystem;
    public static GameEventSystem EventSystem => Instance.eventSystem;

    public readonly Dictionary<Type, ISingleton> All = new();

    public static T Get<T>() where T : ISingleton
    {
        var All = Instance.All;
        var key = typeof(T);

        if (!All.ContainsKey(key)) throw new InvalidOperationException($"[Main] {key.Name} not registered");

        return (T)All[key];
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Initiallize()
    {
        if (!PlayerPrefs.HasKey("volume")) PlayerPrefs.SetFloat("volume", 0.5f);
        if (!PlayerPrefs.HasKey("fullscreen")) PlayerPrefs.SetInt("fullscreen", Screen.fullScreen ? 1 : 0);
        Instance = null;

        DOTween.SetTweensCapacity(1000, 200);

        if (Instance != null) Destroy(Instance);

        var gameObject = new GameObject("Main");
        Instance = gameObject.AddComponent<Main>();
        Instance._gameConfig = Resources.Load<GameConfig>("GameConfig");
        Instance._sceneTransition = Instantiate(Content.SceneTransition).GetComponent<SceneTransition>();

        Instance._cursor = Instantiate(Content.Cursor).GetComponentInChildren<Cursor>();
        SceneManager.sceneLoaded += OnSceneLoaded;

        DontDestroyOnLoad(Instance);
    }

    public static void ChangeScreen()
    {
        var bol = PlayerPrefs.GetInt("fullscreen") == 1;
        Screen.fullScreen = bol;
        if (bol) Screen.SetResolution(Display.main.systemWidth, Display.main.systemHeight, FullScreenMode.FullScreenWindow);
    }

    private static void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        Instance.All.Clear();

        Instance.ecs.Reset();
        Instance.RegisterAll();
        Instance.InitializeAll();
    }

    private void Awake()
    {
        UnityEngine.Cursor.visible = false;
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        
        Instance.ecs = new();
        Instance.ecs.Init();
        
        eventSystem = new();
        eventSystem.Initialize();
    }

    private void RegisterAll()
    {
        var allTypes = Util.Reflection.FindAllSubclasses<ISingleton>();
        var allMono = Util.Reflection.FindAllMonoBehaviours<ISingleton>();

        foreach (var type in allTypes) if (Activator.CreateInstance(type) is ISingleton singleton) Register(singleton);
        foreach (var singleton in allMono) Register(singleton);
    }

    private void InitializeAll()
    {
        foreach (var key in All.Keys) All[key].Initialize();
    }

    public void Register<T>(T singleton) where T : ISingleton
    {
        var key = singleton.GetType();
        if (All.ContainsKey(key))
        {
            return;
        }
        All.Add(key, singleton);
    }
    
    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        //Destroy(_sceneTransition);
        Instance = null;
    }
    public bool IsRegistered<T>() where T : ISingleton => All.ContainsKey(typeof(T));
}

public interface ISingleton
{
    public void Initialize();
}

