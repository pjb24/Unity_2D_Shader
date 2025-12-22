using DG.Tweening;
using System.Collections;
using UnityEngine;

public class HitEffect : MonoBehaviour
{
    [SerializeField] private float _duration = 0.25f;

    private int _hitEffectAmount = Shader.PropertyToID("_HitEffectAmount");

    private SpriteRenderer[] _spriteRenders;
    private Material[] _materials;

    private float _lerpAmount;

    private void Awake()
    {
        _spriteRenders = GetComponentsInChildren<SpriteRenderer>();

        _materials = new Material[_spriteRenders.Length];
        for (int i = 0; i < _materials.Length; i++)
        {
            _materials[i] = _spriteRenders[i].material;
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //StartCoroutine(ApplyHitEffect());
            _lerpAmount = 0f;
            DOTween.To(GetLerpValue, SetLerpValue, 1f, _duration).SetEase(Ease.OutExpo).OnUpdate(OnLerpUpdate).OnComplete(OnLerpComplete);
        }
    }

    private void OnLerpUpdate()
    {
        for (int i = 0; i < _materials.Length; i++)
        {
            _materials[i].SetFloat(_hitEffectAmount, GetLerpValue());
        }
    }

    private void OnLerpComplete()
    {
        DOTween.To(GetLerpValue, SetLerpValue, 0f, _duration).OnUpdate(OnLerpUpdate);
    }

    private float GetLerpValue()
    {
        return _lerpAmount;
    }

    private void SetLerpValue(float newValue)
    {
        _lerpAmount = newValue;
    }

    private IEnumerator ApplyHitEffect()
    {
        float elapsedTime = 0f;

        while (elapsedTime < _duration)
        {
            elapsedTime += Time.deltaTime;

            float lerpedAmount = Mathf.Lerp(0f, 1f, (elapsedTime / _duration));
            for (int i = 0; i < _materials.Length; i++)
            {
                _materials[i].SetFloat(_hitEffectAmount, lerpedAmount);
            }

            yield return null;
        }

        elapsedTime = 0f;
        while (elapsedTime < _duration)
        {
            elapsedTime += Time.deltaTime;

            float lerpedAmount = Mathf.Lerp(1f, 0f, (elapsedTime / _duration));
            for (int i = 0; i < _materials.Length; i++)
            {
                _materials[i].SetFloat(_hitEffectAmount, lerpedAmount);
            }

            yield return null;
        }
    }
}
