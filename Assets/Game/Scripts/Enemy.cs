using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    private string currentAnimation = string.Empty;

    [SerializeField] private DamageDealerEnemy _punch_1;
    [SerializeField] private DamageDealerEnemy _punch_2;
    [SerializeField] private Animator animator;

    private bool isHurting = false;
    private bool isHolding = false;
    private bool isBattle = false;
    private bool isDead = false;
    private bool isWaitingAttack = false;
    private bool isAttacking = false;
    private int currentComboIndex = 0;

    public float attackProbability = 0.6f;
    public float blockProbability = 0.3f;
    public float hp;
    public float maxHp = 100f;


    private Coroutine CorStopCombo;
    private Coroutine CorAttackCallBack;
    private Coroutine CorHurting;



    public void Init()
    {
        hp = maxHp;
        currentComboIndex = 0;
        isAttacking = false;
        isWaitingAttack = false;
        isHolding = false;
        isDead = false;
        isHurting = false;
        isBattle = false;
    }

    public void Fight()
    {
        isBattle = true;
    }

    public void TakeDamage(float damage)
    {

        if (isHurting)
            return;

        if (isHolding)
        {
            OnHurt(damage / 2f);
        }
        else
        {
            OnHurt(damage);
        }
    }
    public void Die()
    {
        isDead = true;
        ChangeAnimation(Constant.ANIM_DEATH, 0.1f, 0.1f);
    }
    private void OnHurt(float damage)
    {
        hp -= damage;

        if (hp <= 0)
        {
            Die();
            return;
        }

        Debug.LogError("Take Damage");
        isHurting = true;
        ChangeAnimation(Constant.ANIM_HEAD_HURT, 0.1f, 0.1f);

        if (CorHurting != null)
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
            ChangeAnimation(Constant.ANIM_IDLE, 0.1f, 0.1f);
        }
    }

    private void Update()
    {
        if(isDead) return;

        if (isHurting) return;

        if (isAttacking) return;

        if (!isBattle) return;

        MakeDecision();
    }


    private void MakeDecision()
    {
        float randomValue = Random.value;

        //Debug.LogError("MakeDecision " + randomValue);

        if (randomValue < attackProbability)
        {
            // attack
            Debug.LogError("Attacking " + randomValue);
            OnAttack();
        }
        else if (randomValue < attackProbability + blockProbability) 
        { 
            // block        
            HandleBlocking(true);
        }
    }

    private void HandleBlocking(bool isBlock)
    {
        
        if (isDead || isHurting) return;
        if (isAttacking) return;
        isHolding = isBlock;
        if (isBlock)
        {
            ChangeAnimation(Constant.ANIM_BLOCK, 0.1f, 0.1f);
        }
        else
        {
            ChangeAnimation(Constant.ANIM_IDLE, 0.1f, 0.1f);
        }
    }

    public void OnAttack()
    {
        if (isDead || isHurting) return;
        isHolding = false;
        isAttacking = true;
        if (!isWaitingAttack)
        {
            if (isAttacking)
            {
                isWaitingAttack = true;

                if (CorStopCombo != null)
                {
                    StopCoroutine(CorStopCombo);
                    CorStopCombo = null;
                }

                // combo attack
                ComboAttack();
            }
        }
    }

    private void ComboAttack()
    {
        if (isHolding)
        {
            isAttacking = false;
            isWaitingAttack = false;
            return;
        }

        //Debug.LogError("ComboAttack " + currentComboIndex);
        isAttacking = false;
        currentComboIndex++;

        if (currentComboIndex > 2)
        {
            currentComboIndex = 0;
        }

        PlayComboAnim(currentComboIndex);
    }

    private void PlayComboAnim(int index)
    {
        _punch_1.StartDealDamage();
        _punch_2.StartDealDamage();

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

    public void ChangeAnimation(string animation, float crossFade = 0.2f, float time = 0)
    {
        if (time > 0)
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

    IEnumerator IEStopCombo()
    {
        //Debug.LogError("ComboAttack end " + currentComboIndex);
        yield return new WaitForSeconds(0.1f);
        ChangeAnimation(Constant.ANIM_IDLE, 0.1f, 0.1f);
        currentComboIndex = 0;
        isAttacking = false;
        isWaitingAttack = false;
        _punch_1.EndDealDamage();
        _punch_2.EndDealDamage();
    }

    private Coroutine CorAnimationCallBack;

    public void OnRecovery(float timeDelay)
    {
        if (CorAnimationCallBack != null)
        {
            return;
        }

        CorAnimationCallBack = StartCoroutine(IERecovery());

        IEnumerator IERecovery()
        {
            yield return new WaitForSeconds(timeDelay);
            isHurting = false;
            ChangeAnimation(Constant.ANIM_IDLE, 0.1f, 0.1f);
            CorAnimationCallBack = null;
        }
    }
}
