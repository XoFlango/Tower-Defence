using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [Header("Configurações do Inimigo")]
    public float moveSpeed = 3f;

    [Header("Comportamento do Atirador")]
    public bool pararNoLimiteDaTorre = false;
    public float distanciaDeParadaPadrao = 0f;

    [Header("Visual")]
    [Tooltip("Coloque -90 ou 90 se o sprite estiver andando de lado.")]
    public float compensacaoDeRotacao = -90f; // -90 resolve 99% dos casos de sprites virados para cima

    private Transform tower;
    private TowerController scriptDaTorre;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        GameObject towerObject = GameObject.Find("Torre");

        if (towerObject != null)
        {
            tower = towerObject.transform;
            scriptDaTorre = towerObject.GetComponent<TowerController>();
        }
    }

    void FixedUpdate()
    {
        if (tower != null && scriptDaTorre != null)
        {
            // Calcula a direção e a distância
            Vector2 direcao = (tower.position - transform.position).normalized;
            float distancia = Vector2.Distance(transform.position, tower.position);

            // Faz o inimigo olhar para a torre constantemente
            float angulo = Mathf.Atan2(direcao.y, direcao.x) * Mathf.Rad2Deg;
            rb.rotation = angulo + compensacaoDeRotacao; // Aplica a correção da arte aqui!

            float limiteAtual = pararNoLimiteDaTorre ? (scriptDaTorre.attackRange - 0.2f) : distanciaDeParadaPadrao;

            if (distancia > limiteAtual)
            {
                rb.linearVelocity = direcao * moveSpeed;
            }
            else
            {
                rb.linearVelocity = Vector2.zero;
            }
        }
    }
}