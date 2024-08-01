using UnityEngine;

public class DrawBackGround : MonoBehaviour
{
    SpriteRenderer _spriteRenderer;
    readonly float _width = 0.25f;
    readonly float _height = 0.33333f;

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        GetComponent<Transform>().localScale = new Vector3(_spriteRenderer.sprite.rect.width * _width, _spriteRenderer.sprite.rect.height * _height, 0);
    }
}
