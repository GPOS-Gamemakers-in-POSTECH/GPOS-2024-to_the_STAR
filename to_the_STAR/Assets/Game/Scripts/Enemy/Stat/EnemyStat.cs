using UnityEngine;

[ CreateAssetMenu(fileName = "EnemyStat", menuName = "ScriptableObject/EnemyStat", order = 1) ]
public class EnemyStat : ScriptableObject
{
    public float hp;
    public float attackPower;
    public float detectionRange;
    public float attackRange;
    public float speed;
    public float attackCooldown;
    public float detectionCooldown;
    public float attackDuration;
}