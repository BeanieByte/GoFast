using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    [SerializeField] private Vector2 _parallaxEffectMultiplier;

    [SerializeField] private Camera _parallaxCamera;
    private Transform _cameraTransform;
    private Vector3 _lastCameraPosition;

    private float _textureUniteSizeX;
    private float _textureUniteSizeY;

    [SerializeField] private bool _infiniteHorizontal;
    [SerializeField] private bool _infiniteVertical;

    private void Start() {
        _cameraTransform = Camera.main.transform;  //_parallaxCamera.transform;
        _lastCameraPosition = _cameraTransform.position;
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.drawMode = SpriteDrawMode.Tiled;
        Sprite sprite = spriteRenderer.sprite;
        Texture2D texture = sprite.texture;
        _textureUniteSizeX = texture.width / sprite.pixelsPerUnit;
        _textureUniteSizeY = texture.height / sprite.pixelsPerUnit;
    }

    private void LateUpdate() {
        Vector3 deltaMovement = _cameraTransform.position - _lastCameraPosition;
        transform.position += new Vector3(deltaMovement.x * _parallaxEffectMultiplier.x, deltaMovement.y * _parallaxEffectMultiplier.y);
        _lastCameraPosition = _cameraTransform.position;

        if (_infiniteHorizontal) {
            if (Mathf.Abs(_cameraTransform.position.x - transform.position.x) >= _textureUniteSizeX) {
                float offsetPositionX = (_cameraTransform.position.x - transform.position.x) % _textureUniteSizeX;
                transform.position = new Vector3(_cameraTransform.position.x + offsetPositionX, transform.position.y);
            }
        }

        if (_infiniteVertical) {
            if (Mathf.Abs(_cameraTransform.position.y - transform.position.y) >= _textureUniteSizeY) {
                float offsetPositionY = (_cameraTransform.position.y - transform.position.y) % _textureUniteSizeY;
                transform.position = new Vector3(transform.position.x, _cameraTransform.position.y + offsetPositionY);
            }
        }
    }
}
