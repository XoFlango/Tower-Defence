using UnityEngine;

public class TowerController : MonoBehaviour
{
    [Header("Configurações de Disparo")]
    public GameObject projectilePrefab;
    public float fireRadius = 1f; // Substituímos o Transform pelo Raio
    public float fireRate = 0.5f;

    [Header("Configurações de Alcance")]
    public float attackRange = 5f;
    public LayerMask enemyLayer;

    private Transform currentTarget;
    private float nextFireTime;

    void Update()
    {
        FindTarget();

        if (currentTarget != null)
        {
            AimAtTarget();

            if (Time.time >= nextFireTime)
            {
                Shoot();
                nextFireTime = Time.time + fireRate;
            }
        }
    }

    void FindTarget()
    {
        Collider2D[] enemiesInRange = Physics2D.OverlapCircleAll(transform.position, attackRange, enemyLayer);

        float shortestDistance = Mathf.Infinity;
        Transform nearestEnemy = null;

        foreach (Collider2D enemy in enemiesInRange)
        {
            float distanceToEnemy = Vector2.Distance(transform.position, enemy.transform.position);

            if (distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy.transform;
            }
        }

        currentTarget = nearestEnemy;
    }

    void AimAtTarget()
    {
        Vector2 lookDir = currentTarget.position - transform.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    void Shoot()
    {
        // Calcula a posição do tiro: Posição do centro + (Direção que a torre olha * Raio do tiro)
        // transform.right representa a "frente" da torre em 2D após ela rotacionar
        Vector3 spawnPosition = transform.position + (transform.right * fireRadius);

        Instantiate(projectilePrefab, spawnPosition, transform.rotation);
    }

    void OnDrawGizmosSelected()
    {
        // Desenha o radar de alcance em Verde
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        // Desenha de onde o tiro vai sair em Amarelo
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, fireRadius);
    }
}