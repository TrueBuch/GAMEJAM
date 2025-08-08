using System;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class EntityVisual : MonoBehaviour
{


    private float _moveSpeed;
    private Entity _entity;
    public Entity Entity => _entity;

    [SerializeField] private RectTransform _body;
    public RectTransform Body => _body;
    
    [SerializeField] private Image _sprite;
    public Image Sprite => _sprite;

    [SerializeField] private Image _shadow;
    public Image Shadow => _shadow;


    private float _rotationSpeed = 30f;
    private Vector3 _currentRotation;

    [SerializeField] float dampingTime = 0.3f;
    private float zVelocity = 0f;      

    public Vector3 Offset;
    public Vector3 Tilt;

    public void Initialize(Entity entity)
    {
        _entity = entity;
        var tagView = _entity.Get<TagView>();

        _moveSpeed = tagView.FollowSpeed;
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
    float movement = (transform.position.x - _entity.transform.position.x) * Time.deltaTime;
    float targetZ = movement * _rotationSpeed;

    float smoothedZ = Mathf.SmoothDampAngle(
        _currentRotation.z,    
        targetZ,               
        ref zVelocity,         
        dampingTime            
    );

    _currentRotation.z = Mathf.Clamp(smoothedZ, -60f, 60f);

    transform.localRotation = Quaternion.Euler(
        transform.localRotation.eulerAngles.x,
        transform.localRotation.eulerAngles.y,
        _currentRotation.z + Tilt.z
    );
}

    private void Follow()
    {
        var targetPosition = _entity.transform.position + Offset;
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