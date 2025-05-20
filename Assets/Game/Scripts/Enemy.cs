using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public enum AIState
    {
        Idle,
        Attacking,
        Holding,
        Recovering
    }

    public AIState currentState = AIState.Idle;

    [Header("CombatSettings")]
    public float attackRange = 2f;
    public float holdDuration = 1.5f;
    public float recoveryTime = 0.8f;
    public float decisionInterval = 0.5f;

    [Header("Attack Probabilities")]
    [Range(0, 1)] public float holdChance = 0.3f;
    [Range(0, 1)] public float attack1Chance = 0.5f;
    [Range(0, 1)] public float attack2Chance = 0.3f;
    [Range(0, 1)] public float attack3Chance = 0.2f;

    private Transform player;
    [SerializeField] private Animator animator;
    private float stateTimer;
    private float decisionTimer;
    private string currentAnimation = string.Empty;

    [SerializeField] private DamageDealer _punch_1;
    [SerializeField] private DamageDealer _punch_2;

    private bool isHurting = false;
    void Start()
    {
        player = GameController.Instance.playerBoxer.transform;
        decisionTimer = decisionInterval;
    }

    void Update()
    {
        decisionTimer -= Time.deltaTime;

        if (decisionTimer <= 0 && currentState == AIState.Idle)
        {
            MakeDecision();
            decisionTimer = decisionInterval;
        }

        UpdateState();
    }

    void MakeDecision()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= attackRange)
        {
            if (Random.value < holdChance)
            {
                EnterHoldState();
            }
            else
            {
                ChooseAttack();
            }
        }
    }

    void ChooseAttack()
    {
        float attackChoice = Random.value;

        if (attackChoice < attack1Chance)
        {
            ExecuteAttack(1);
        }
        else if (attackChoice < attack1Chance + attack2Chance)
        {
            ExecuteAttack(2);
        }
        else
        {
            ExecuteAttack(3);
        }
    }
    void ExecuteAttack(int attackType)
    {
        currentState = AIState.Attacking;
        _punch_1.StartDealDamage();
        _punch_2.StartDealDamage();
        switch (attackType)
        {
            case 1:
                ChangeAnimation(Constant.ANIM_KIDNEY_PUNCH_LEFT, 0.1f, 0.1f);
                break;
            case 2:
                ChangeAnimation(Constant.ANIM_KIDNEY_PUNCH_RIGHT, 0.1f, 0.1f);
                break;
            case 3:
                ChangeAnimation(Constant.ANIM_PUNCH_STOMASH, 0.1f, 0.1f);
                break;
        }

        // Thời gian recovery khác nhau cho mỗi đòn
        stateTimer = attackType switch
        {
            1 => 0.5f,
            2 => 0.7f,
            3 => 1f,
            _ => 0.5f
        };
    }

    void EnterHoldState()
    {
        currentState = AIState.Holding;
        //animator.SetBool("IsHolding", true);
        ChangeAnimation(Constant.ANIM_BLOCK, 0.1f, 0.1f);
        stateTimer = holdDuration;
    }

    void ExitHoldState()
    {
        ChangeAnimation(Constant.ANIM_IDLE, 0.1f, 0.1f);
        currentState = AIState.Recovering;
        stateTimer = recoveryTime;
    }

    void UpdateState()
    {

        if (currentState == AIState.Holding || currentState == AIState.Attacking || currentState == AIState.Recovering)
        {
            stateTimer -= Time.deltaTime;

            if (stateTimer <= 0)
            {
                switch (currentState)
                {
                    case AIState.Holding:
                        ExitHoldState();
                        break;
                    case AIState.Recovering:
                        currentState = AIState.Idle;
                        break;
                    case AIState.Attacking:
                        currentState = AIState.Recovering;
                        stateTimer = recoveryTime;
                        break;
                }
            }
        }
    }

    public void OnAttackComplete()
    {
        if (currentState == AIState.Attacking)
        {
            currentState = AIState.Recovering;
            stateTimer = recoveryTime;
            _punch_1.EndDealDamage();
            _punch_2.EndDealDamage();
        }
    }

    public void TakeDamage()
    {
        

        if (isHurting)
            return;

        Debug.LogError("Take Damage");
        isHurting = true;
        ChangeAnimation(Constant.ANIM_HEAD_HURT, 0.1f, 0.1f);
        //if (currentState == AIState.Holding)
        //{
        //    // Giảm damage khi đang hold
        //    // Có thể thêm logic counter ở đây
        //    animator.SetTrigger("BlockImpact");
        //}
        //else
        //{

        //}
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
