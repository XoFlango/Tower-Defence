using UnityEngine;

public class TowerController : MonoBehaviour
{
    [Header("Configurações de Disparo")]
    public GameObject projectilePrefab;
    public float fireRadius = 1f;
    public float fireRate = 0.5f;

    [Header("Atributos da Torre")]
    public int danoAtual = 1;
    public float raioDeExplosaoAtual = 0f; // NOVO: Controla a área de explosão dos tiros

    [Header("Configurações de Alcance")]
    public float attackRange = 5f;
    public LayerMask enemyLayer;

    [Header("Configurações da Estação")]
    public float velocidadeDeRotacao = 30f; // Velocidade do giro (graus por segundo)

    private Transform currentTarget;
    private float nextFireTime;

    void Update()
    {
        // NOVO: A estação gira o tempo todo, independente de ter alvos
        GirarEstacao();

        FindTarget();

        if (currentTarget != null)
        {
            // Removemos a função de mira que ficava aqui!

            if (Time.time >= nextFireTime)
            {
                Shoot();

                float limiteDeSeguranca = Mathf.Max(fireRate, 0.1f);
                nextFireTime = Time.time + limiteDeSeguranca;
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

    void GirarEstacao()
    {
        // Rotate gira o objeto em X, Y e Z. Estamos girando apenas no eixo Z.
        transform.Rotate(0, 0, velocidadeDeRotacao * Time.deltaTime);
    }

    void Shoot()
    {
        // 1. Calcula a direção exata da estação até o inimigo
        Vector2 direcaoAoAlvo = currentTarget.position - transform.position;

        // 2. Calcula o ângulo para a bala nascer apontada para o inimigo
        float angle = Mathf.Atan2(direcaoAoAlvo.y, direcaoAoAlvo.x) * Mathf.Rad2Deg;
        Quaternion rotacaoDaBala = Quaternion.Euler(0, 0, angle);

        // 3. Define onde a bala vai nascer. 
        // Em vez de usar a rotação da estação (transform.right), usamos a direção do inimigo!
        Vector3 direcaoNormalizada = direcaoAoAlvo.normalized;
        Vector3 spawnPosition = transform.position + (direcaoNormalizada * fireRadius);

        // Cria a bala usando a posição e a rotação que acabamos de calcular
        GameObject projetilCriado = Instantiate(projectilePrefab, spawnPosition, rotacaoDaBala);

        ProjectilePhysics scriptBala = projetilCriado.GetComponent<ProjectilePhysics>();

        if (scriptBala != null)
        {
            scriptBala.Configurar(danoAtual, raioDeExplosaoAtual);
        }
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