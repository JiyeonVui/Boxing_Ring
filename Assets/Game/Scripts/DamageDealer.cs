using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    bool canDealDamage;
    [SerializeField] private bool isEnemy = false;

    List<GameObject> hasDealtDamage;

    [SerializeField] float weaponLength;
    float weaponDamage;
    private void Start()
    {
        canDealDamage = false;
        hasDealtDamage = new List<GameObject>();
    }

    public void SetWeaponDamage(float damage)
    {
        weaponDamage = damage;
    }

    private void Update()
    {
        if (canDealDamage)
        {
            RaycastHit hit;
            int layerMask = 1 << 9;
            if (Physics.Raycast(transform.position, transform.forward, out hit, weaponLength, layerMask))
            {

                Debug.Log("Hit enemy: " + hit.collider.gameObject.name);
                GameObject enemy = hit.collider.gameObject;

                if (!isEnemy)
                {
                    enemy.GetComponent<Enemy>().TakeDamage();
                }
                else
                {
                    enemy.GetComponent<PlayerBoxer>().TakeDamage(5);
                }
                
                
            }
        }
    }

    public void StartDealDamage()
    {
        canDealDamage = true;
        hasDealtDamage.Clear();
    }

    public void EndDealDamage()
    {
        canDealDamage = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.forward * weaponLength);
    }
}