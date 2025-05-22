using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerBoxer : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 2f;
    public Animator animator;
    public Camera mainCam;

    private bool isBlocking = false;

    [Header("Attack Settings")]
    public float comboMaxDelay = 0.6f;
    public int maxCombo = 3;


    private string currentAnimation = string.Empty;

    private int currentComboIndex = 0;

    private bool isAttacking = false;
    private bool isWaitingAttack = false;
    private Coroutine CorStopCombo;
    private Coroutine CorHurting;


    private bool isDead = false;
    private bool isHurting = false;


    [Header("Player Parameters")]
    [SerializeField] private float hp;
    [SerializeField] private float maxHp = 100f;
    [SerializeField] private float attackDamage = 10f;
    [SerializeField] private DamageDealer _rightPunch;
    [SerializeField] private DamageDealer _leftPunch;


    [Header("Time Intro")]
    [SerializeField] private List<Transform> pathList;
    [SerializeField] private float time_1 = 3f;
    [SerializeField] private float time_2 = 3f;
    public void Init()
    {
        _rightPunch.SetWeaponDamage(attackDamage);
        _leftPunch.SetWeaponDamage(attackDamage);
        hp = maxHp;
        isDead = false;
        isHurting = false;
        currentComboIndex = 0;

        isAttacking = false;
        isWaitingAttack = false;
    }

    private void Awake()
    {
        GameController.attackAction += OnAttackInput;
        GameController.holdAction += HandleBlocking;
    }

    private void OnDestroy()
    {
        GameController.attackAction -= OnAttackInput;
        GameController.holdAction -= HandleBlocking;
    }

    public void TakeDamage(float damage)
    {
        Debug.LogError("Take Damage " + damage);
        if(isHurting)
            return;

        if (isBlocking)
        {
            OnHurt(damage /2f);
        }
        else
        {
            OnHurt(damage);
        }
    }

    public void OnHurt(float dame)
    {

        hp -= dame;

        if (hp <= 0)
        {
            Die();
            return;
        }

        isHurting = true;
        if (!isBlocking)
        {
            ChangeAnimation(Constant.ANIM_HEAD_HURT, 0.1f, 0.1f);
        }


        if(CorHurting != null)
        {
            StopCoroutine(CorHurting);
            CorHurting = null;
        }

        CorHurting = StartCoroutine(IEHurt());

        IEnumerator IEHurt()
        {
            yield return new WaitForSeconds(0.25f);
            isHurting = false;
            CorHurting = null;
            if (!isBlocking)
            {
                ChangeAnimation(Constant.ANIM_IDLE, 0.1f, 0.1f);
            }
        }
    }

    public void Die()
    {
        isDead = true;
        ChangeAnimation(Constant.ANIM_DEATH, 0.1f, 0.1f);
    }



    public void OnAttackInput()
    {
        if (isDead || isHurting) return;
        isAttacking = true;
        if (!isWaitingAttack)
        {
            if (isAttacking)
            {
                isWaitingAttack = true;

                if(CorStopCombo != null)
                {
                    StopCoroutine(CorStopCombo);
                    CorStopCombo = null;
                }

                // combo attack
                ComboAttack();
            }
        }
    }
    private Coroutine CorAttackCallBack;

    public void OnAttackCallBack(float timeDelay)
    {
        if (CorAttackCallBack != null)
        {
            return;
        }


        CorAttackCallBack = StartCoroutine(IEAttackCallBack());

        IEnumerator IEAttackCallBack()
        {
            yield return new WaitForSeconds(timeDelay);
            //Debug.LogError("OnAttackCallBack " + currentComboIndex);

            isWaitingAttack = false;

            if (!isAttacking)
            {

                if (CorStopCombo != null)
                {
                    StopCoroutine(CorStopCombo);
                    CorStopCombo = null;
                }

                CorStopCombo = StartCoroutine(IEStopCombo());
            }
            else
            {
                isWaitingAttack = true;
                // combo attack
                ComboAttack();
            }

            CorAttackCallBack = null;
        }
    }


    private void ComboAttack()
    {
        if(isBlocking)
        {
            isAttacking = false;
            isWaitingAttack = false;
            return;
        }

        Debug.LogError("ComboAttack " + currentComboIndex);
        isAttacking = false;
        currentComboIndex++;

        if (currentComboIndex > 2)
        {
            currentComboIndex = 0;
        }

        PlayComboAnim(currentComboIndex);
    }

    IEnumerator IEStopCombo()
    {
        //Debug.LogError("ComboAttack end " + currentComboIndex);
        yield return new WaitForSeconds(0.1f);
        ChangeAnimation(Constant.ANIM_IDLE, 0.1f, 0.1f);
        currentComboIndex = 0;
        isAttacking = false;
        isWaitingAttack = false;
        _rightPunch.EndDealDamage();
        _leftPunch.EndDealDamage();
    }

    private void PlayComboAnim(int index)
    {
        _rightPunch.StartDealDamage();
        _leftPunch.StartDealDamage();

        switch (index)
        {
            case 0:
                ChangeAnimation(Constant.ANIM_KIDNEY_PUNCH_LEFT, 0.1f, 0.1f);
                break;
            case 1:
                ChangeAnimation(Constant.ANIM_KIDNEY_PUNCH_RIGHT, 0.1f, 0.1f);
                break;
            case 2:
                ChangeAnimation(Constant.ANIM_PUNCH_STOMASH, 0.1f, 0.1f);
                break;
        }  
    }

    private void HandleBlocking(bool isBlock)
    {
        if (isDead || isHurting) return;
        isBlocking = isBlock;
        if (isBlock)
        {
            ChangeAnimation(Constant.ANIM_BLOCK, 0.1f, 0.1f);
        }
        else
        {
            ChangeAnimation(Constant.ANIM_IDLE, 0.1f, 0.1f);
        }
    }



    public void ChangeAnimation(string animation, float crossFade = 0.2f, float time = 0)
    {
        if(time > 0)
        {
            StartCoroutine(Wait());
        }
        else
        {
            Validate();
        }

        IEnumerator Wait()
        {
            yield return new WaitForSeconds(time - crossFade);
            Validate();
        }

        void Validate()
        {
            if (currentAnimation != animation)
            {
                currentAnimation = animation;
                
                animator.CrossFade(animation, crossFade);
            }
        }
    }

    public void Move()
    {

        ChangeAnimation(Constant.ANIM_WALK, 0.1f, 0.1f);
        transform.DORotate(new Vector3(0f, 56f, 0f), time_1, RotateMode.Fast);
        transform.DOMove(pathList[0].position, time_1).OnComplete(() =>
        {
            ChangeAnimation(Constant.ANIM_JUMPTOBOXINGRING, 0.1f, 0.1f);
            transform.DOMove(pathList[1].position, 1.5f).OnComplete(() =>
            {
                transform.DOMove(pathList[2].position, animator.GetCurrentAnimatorStateInfo(0).length - 1.5f).SetDelay(0.5f).OnComplete(() =>
                {
                    ChangeAnimation(Constant.ANIM_WALK, 0.1f, 0.1f);
                    transform.DOMove(pathList[3].position, 2f);
                });
            });
        });
    }
}
