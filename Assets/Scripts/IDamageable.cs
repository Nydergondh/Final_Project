using UnityEngine;

public interface IDamageable 
{
    void OnDamage(int damage);
    void OnDamage(int damage, Vector3 bloodDirection);
}
