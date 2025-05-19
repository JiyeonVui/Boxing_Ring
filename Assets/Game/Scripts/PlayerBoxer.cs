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
    public Transform[] enemies;

    private Vector3 moveDir;
    private Transform currentTarget;
    private bool isBlocking = false;

    [Header("Attack Settings")]
    public float comboMaxDelay = 0.6f;
    public int maxCombo = 3;
    private int currentCombo = 0;
    private float lastPunchTime = 0f;

    [Header("Stamina Settings")]
    public int maxStamina = 5;
    public float staminaRecoveryDelay = 2f;
    public float staminaRecoveryRate = 1f; // mỗi giây
    private float currentStamina;
    private float lastPunchStaminaTime;

    private bool isMoving = false;
    private string currentAnimation = string.Empty;

    private int currentComboIndex = 0;
    private bool inputBuffered = false;

    private float timeSinceLastPunch = 0f;

    private bool isAttacking = false;
    private bool isWaitingAttack = false;
    private Coroutine CorStopCombo;
    private bool isDead = false;

    [Header("Player Parameters")]
    [SerializeField] private float hp;
    [SerializeField] private float maxHp = 100f;
    [SerializeField] private float attackDamage = 10f;
    [SerializeField] private DamageDealer _rightPunch;
    [SerializeField] private DamageDealer _leftPunch;

    private void Start()
    {
        currentStamina = maxStamina;
        _rightPunch.SetWeaponDamage(attackDamage);
        _leftPunch.SetWeaponDamage(attackDamage);
        hp = maxHp;
        isDead = false;
    }

    private void Awake()
    {
        GameController.movingAction += MoveInDirection;
        GameController.stopAction += StopMoving;
        GameController.attackAction += OnAttackInput;
        GameController.holdAction += HandleBlocking;
    }

    private void OnDestroy()
    {
        GameController.movingAction -= MoveInDirection;
        GameController.stopAction -= StopMoving;
        GameController.attackAction -= OnAttackInput;
        GameController.holdAction -= HandleBlocking;
    }

    private void Update()
    {
        timeSinceLastPunch += Time.deltaTime;
        HandleComboReset();
        RecoverStamina();
    }

    public void TakeDamage(float damage)
    {
        if (isBlocking)
        {
            damage /= 2; // giảm một nửa sát thương khi đang block
        }
        hp -= damage;
        if (hp <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        isDead = true;
    }

    private void HandleComboReset()
    {
        if (Time.time - lastPunchTime > comboMaxDelay)
        {
            currentCombo = 0;
        }
    }
    private void RecoverStamina()
    {
        if (currentStamina < maxStamina && Time.time - lastPunchStaminaTime > staminaRecoveryDelay)
        {
            Debug.LogError("RecoverStamina " + currentStamina);
            Debug.LogError("staminaRecoveryRate " + (Time.time - lastPunchStaminaTime) + " "+ staminaRecoveryDelay);
            currentStamina += staminaRecoveryRate * Time.deltaTime;
            currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
        }
    }

    public void OnAttackInput()
    {
        if (isDead) return;
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
            Debug.LogError("OnAttackCallBack " + currentComboIndex);

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

            _rightPunch.EndDealDamage();
            _leftPunch.EndDealDamage();

            CorAttackCallBack = null;
        }
    }


    private void ComboAttack()
    {
        Debug.LogError("ComboAttack " + currentComboIndex);
        isAttacking = false;
        currentComboIndex++;

        if (currentComboIndex > 2)
        {
            currentComboIndex = 0;
        }

        PlayComboAnim(currentComboIndex);

        _rightPunch.StartDealDamage();
        _leftPunch.StartDealDamage();

        timeSinceLastPunch = 0f;
    }

    IEnumerator IEStopCombo()
    {
        Debug.LogError("ComboAttack end " + currentComboIndex);
        yield return new WaitForSeconds(0.1f);
        ChangeAnimation(Constant.ANIM_IDLE, 0.1f, 0.1f);
        currentComboIndex = 0;
        isAttacking = false;
        isWaitingAttack = false;
    }

    private void PlayComboAnim(int index)
    {
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
        if (isDead) return;
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

    public void MoveInDirection(Vector2 touchDir)
    {
        if(isDead) return;

        Vector3 camForward = mainCam.transform.forward;
        camForward.y = 0;
        camForward.Normalize();

        Vector3 camRight = mainCam.transform.right;
        camRight.y = 0;
        camRight.Normalize();

        moveDir = camRight * touchDir.x + camForward * touchDir.y;
        transform.position += moveDir * moveSpeed * Time.deltaTime;

        if (moveDir != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moveDir), 0.2f);
        }

        if(!isMoving)
        {
            isMoving = true;
            ChangeAnimation(Constant.ANIM_RUNNING, 0f, 0.1f);
        }
            
    }

    public void StopMoving()
    {
        if (isDead) return;
        Debug.LogError("StopMoving");
        moveDir = Vector3.zero;
        ChangeAnimation(Constant.ANIM_IDLE, 0.1f, 0.1f);
        isMoving = false;
        FaceNearestEnemy();
    }

    void FaceNearestEnemy()
    {
        if (enemies.Length == 0) return;

        float minDist = Mathf.Infinity;
        Transform nearest = null;

        foreach (Transform enemy in enemies)
        {
            float dist = Vector3.Distance(transform.position, enemy.position);
            if (dist < minDist)
            {
                minDist = dist;
                nearest = enemy;
            }
        }

        if (nearest != null)
        {
            Vector3 dir = (nearest.position - transform.position).normalized;
            dir.y = 0;
            if (dir != Vector3.zero)
                transform.rotation = Quaternion.LookRotation(dir);
            currentTarget = nearest;
        }
    }

    public void Punch()
    {
        if (currentTarget != null)
        {
            Vector3 dir = (currentTarget.position - transform.position).normalized;
            dir.y = 0;
            transform.rotation = Quaternion.LookRotation(dir);
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
}
