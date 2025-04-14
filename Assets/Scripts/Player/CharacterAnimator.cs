using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator),typeof(Rigidbody), typeof(Character))]
public class CharacterAnimator : MonoBehaviour
{
    [SerializeField] private AnimationClip _attackClip;
        
    private static readonly int IsAttack = Animator.StringToHash("isAttack");
    private static readonly int Speed = Animator.StringToHash("speed");

    private Animator _animator;
    private Rigidbody _rigidbody;
    private Character _character;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody>();
        _character = GetComponent<Character>();
    }

    private void OnEnable()
    {
        _character.Attacked += StartAttackAnimation;
    }

    private void Update()
    {
        _animator.SetFloat(Speed, Math.Abs(_rigidbody.linearVelocity.x) > Math.Abs(_rigidbody.linearVelocity.z) ? 
            Math.Abs(_rigidbody.linearVelocity.x) : Math.Abs(_rigidbody.linearVelocity.z));
    }

    private void StartAttackAnimation()
    {
        StartCoroutine(PlayAttack());
    }

    private IEnumerator PlayAttack()
    {
        _animator.SetBool(IsAttack, true);
        yield return new WaitForSeconds(_attackClip.length);
        _animator.SetBool(IsAttack, false);
    }
}
