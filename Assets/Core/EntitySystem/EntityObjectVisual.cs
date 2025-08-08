using System;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class EntityObjectVisual : MonoBehaviour
{
    private int _moveSpeed;
    private EntityObject _entityObject;
    public EntityObject EntityObject => _entityObject;

    [SerializeField] private RectTransform _body;
    public RectTransform Body => _body;
    
    [SerializeField] private Image _sprite;
    public Image Sprite => _sprite;
    [SerializeField] private Image _shadow;
    public Image Shadow => _shadow;


    private float _rotationSpeed = 30f;
    private Vector3 _currentRotation;

    public Vector3 Offset;
    public Vector3 Tilt;

    public void Initialize(EntityObject entityObject)
    {
        _entityObject = entityObject;
        var tagView = _entityObject.Entity.Get<TagView>();

        _moveSpeed = tagView.VisualMoveSpeed;
        _sprite.sprite = tagView.Sprite;
        _shadow.sprite = tagView.Shadow;
    }

    protected void Update()
    {
        Rotate();
        Follow();
    }

    private void Rotate()
    {
        var movement = (transform.position.x - _entityObject.transform.position.x) * Time.deltaTime;
        var movementRotation = movement * _rotationSpeed;

        _currentRotation.z = Mathf.Lerp(_currentRotation.z, movementRotation, _rotationSpeed * Time.deltaTime);
        _currentRotation.z = Mathf.Clamp(_currentRotation.z, -60, 60);

        transform.localRotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, _currentRotation.z + Tilt.z);
    }

    private void Follow()
    {
        var targetPosition = _entityObject.transform.position + Offset;
        var distance = Vector2.Distance(transform.position, targetPosition);

        if (distance < 0.1f)
        {
            transform.position = targetPosition;
            return;
        }

        Move(targetPosition);
    }

    private void Move(Vector2 targetPosition)
    {
        transform.position = Vector2.Lerp(
            transform.position,
            targetPosition,
            _moveSpeed * Time.deltaTime
        );
    }
}