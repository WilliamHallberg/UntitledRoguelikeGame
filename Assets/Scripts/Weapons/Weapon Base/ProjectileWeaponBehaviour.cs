using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Base script of all projectile behaviours [to be placed on a prefab of a weapon that is a projectile]
/// </summary>
public class ProjectileWeaponBehaviour : MonoBehaviour
{
    public WeaponScriptableObject weaponData;

    protected Vector3 direction;
    public float destroyAfterSeconds;

    // Current Stats
    protected float currentDamage;
    protected float currentSpeed;
    protected float currentCooldownDuration;
    protected int currentPierce;

    private void Awake()
    {
        currentDamage = weaponData.Damage;
        currentSpeed = weaponData.Speed;
        currentCooldownDuration = weaponData.CooldownDuration;
        currentPierce = weaponData.Pierce;
    }

    public float GetCurrentDamage()
    {
        return currentDamage *= FindObjectOfType<PlayerStats>().CurrentMight;
    }

    protected virtual void Start()
    {
        Destroy(gameObject, destroyAfterSeconds);
    }

    public void DirectionChecker(Vector3 dir)
    {
        direction = dir;

        float dirx = direction.x;
        float diry = direction.y;

        Vector3 scale = transform.localScale;
        Vector3 rotation = transform.rotation.eulerAngles;

        // Left
        if (dirx < 0 && diry == 0)
        {
            scale.x *= -1;
            scale.y *= -1;
        }
        // Down
        else if (dirx == 0 && diry < 0)
        {
            scale.y *= -1;
        }
        // Up
        else if (dirx == 0 && diry > 0)
        {
            scale.x *= -1;
        }
        // Left Up
        else if (dirx < 0 && diry > 0)
        {
            scale.x *= -1;
            scale.y *= -1;
            rotation.z = -90f;
        }
        // Left Down
        else if (dirx < 0 && diry < 0)
        {
            scale.x *= -1;
            scale.y *= -1;
            rotation.z = 0f;
        }
        // Right Up
        else if (dirx > 0 && diry > 0)
        {
            rotation.z = 0f;
        }
        // Right Down
        else if (dirx > 0 && diry < 0)
        {
            rotation.z = -90f;
        }

        transform.localScale = scale;
        transform.rotation = Quaternion.Euler(rotation);
    }

    protected virtual void OnTriggerEnter2D(Collider2D col)
    {
        // refference the script from the collided collider and deal damage using TakeDamage()
        if (col.CompareTag("Enemy"))
        {
            EnemyStats enemy = col.GetComponent<EnemyStats>();
            enemy.TakeDamage(GetCurrentDamage()); // Make sure to use CurrentDamage instead of weaponData.damage in case any damage multipliers in the future
            ReducePierce();
        }
        else if (col.CompareTag("Prop"))
        {
            if (col.gameObject.TryGetComponent(out BreakableProps breakable))
            {
                breakable.TakeDamage(GetCurrentDamage());
                ReducePierce();
            }
        }
    }

    void ReducePierce() // Destroy once the pierce reaches 0
    {
        currentPierce--;

        if (currentPierce <= 0)
        {
            Destroy(gameObject);
        }
    }
}
