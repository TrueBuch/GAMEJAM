using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;

public class SceneTransition : MonoBehaviour
{
    private bool _playOpenAnimation = false;
    private bool _isTransitioning = false;
    public bool IsTransitioning => _isTransitioning;

    private AsyncOperation _asyncOperation;

    [SerializeField] private GameObject _pixelPrefab;
    [SerializeField] private List<Sprite> _sprites;
    [SerializeField] private int _pixelCountX = 16;
    [SerializeField] private int _pixelCountY = 9;

    private GameObject[,] _pixels;
    public readonly UnityEvent<bool> TransitionCompleted = new();
    private void Awake()
    {
        // if (Main.SceneTransition != null && Main.SceneTransition != this)
        // {
        //     Destroy(gameObject);
        //     return;
        // }
        GeneratePixels();
        DontDestroyOnLoad(this);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Start()
    {
        if (!_isTransitioning) TransitionCompleted.Invoke(false);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (_playOpenAnimation) StartCoroutine(PlayAnimation(false));
    }

    private void GeneratePixels()
    {
        int width = Screen.width;
        int height = Screen.height;

        float spriteSizeX = (float)width / _pixelCountX;
        float spriteSizeY = (float)height / _pixelCountY;

        float spriteSize = Mathf.Min(spriteSizeX, spriteSizeY);

        int countX = Mathf.FloorToInt(width / spriteSize) + 1;
        int countY = Mathf.FloorToInt(height / spriteSize) + 1;

        _pixels = new GameObject[countX, countY];

        float startX = -width / 2f + spriteSize / 2f;
        float startY = -height / 2f + spriteSize / 2f;

        for (int i = 0; i < countX; i++)
        {
            for (int j = 0; j < countY; j++)
            {
                GameObject obj = Instantiate(_pixelPrefab, transform);
                obj.SetActive(false);
                _pixels[i, j] = obj;

                obj.name = $"Image_{i}_{j}";

                RectTransform rectTransform = obj.GetComponent<RectTransform>();
                Image image = obj.GetComponent<Image>();

                int randomIndex = Random.Range(0, _sprites.Count);
                image.sprite = _sprites[randomIndex];

                rectTransform.sizeDelta = new(spriteSize, spriteSize);

                float posX = startX + i * spriteSize;
                float posY = startY + j * spriteSize;

                rectTransform.anchoredPosition = new Vector2(posX, posY);
            }
        }
    }

    private IEnumerator PlayAnimation(bool isClosing)
    {
        int cols = _pixels.GetLength(0);
        int rows = _pixels.GetLength(1);

        int max = (cols - 1) + (rows - 1);

        int diagCount = (cols - 1) + (rows - 1) + 1;

        float totalDuration = 0.5f;
        float delay = totalDuration / diagCount;
        for (int d = max; d >= 0; d--)
        {
            List<GameObject> diagonal = new();
            for (int i = 0; i < cols; i++)
            {
                int j = d - i;

                if (j >= 0 && j < rows) diagonal.Add(_pixels[i, j]);
            }

            foreach (GameObject obj in diagonal)
            {
                StartCoroutine(PlayPixelAnimation(obj, isClosing));
            }

            yield return new WaitForSecondsRealtime(delay);
        }
        yield return new WaitForSecondsRealtime(0.15f);
        TransitionComplete(isClosing);
    }

    public IEnumerator PlayPixelAnimation(GameObject obj, bool isClosing)
    {
        if (obj == null || obj.transform == null) yield break;
        var t = obj.transform;
        string tweenId = "PixelTween_" + obj.name;

        DOTween.Kill(tweenId);
        t.localScale = isClosing ? Vector3.zero : Vector3.one;

        if (isClosing)
        {
            obj.SetActive(true);
            t.DOScale(1, 0.1f).SetId(tweenId);
        }
        else
        {
            t.DOScale(0, 0.1f).SetId(tweenId);
            yield return new WaitForSeconds(0.1f);
            obj.SetActive(false);
        }

    }

    public void SwitchToScene(string name)
    {
        if (_asyncOperation != null) return;
        _asyncOperation = SceneManager.LoadSceneAsync(name);
        _asyncOperation.allowSceneActivation = false;

        _isTransitioning = true;
        StartCoroutine(PlayAnimation(true));
    }

    private void TransitionComplete(bool isClosing)
    {
        if (isClosing && _asyncOperation != null)
        {
            _playOpenAnimation = true;
            _asyncOperation.allowSceneActivation = true;
        }
        else
        {
            _asyncOperation = null;
            _playOpenAnimation = false;
        }

        _isTransitioning = false;
        TransitionCompleted?.Invoke(isClosing);
    }

    private void OnDestroy()
    {
        foreach (var obj in _pixels)
        {
            DOTween.Kill("PixelTween_" + obj.name);
            Destroy(obj);
        }
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}