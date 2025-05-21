using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDealerEnemy : MonoBehaviour
{
    bool canDealDamage;

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
            int layerMask = 1 << 10;
            if (Physics.Raycast(transform.position, transform.forward, out hit, weaponLength, layerMask))
            {
                GameObject enemy = hit.collider.gameObject;

                enemy.GetComponent<PlayerBoxer>().TakeDamage(5);

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
