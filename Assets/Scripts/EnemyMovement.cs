using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [Header("Configurações do Inimigo")]
    public float moveSpeed = 3f;

    [Header("Comportamento do Atirador")]
    [Tooltip("Se marcado, o inimigo lerá o alcance da torre e vai parar exatamente na borda dele.")]
    public bool pararNoLimiteDaTorre = false;
    public float distanciaDeParadaPadrao = 0f; // Usado para os inimigos normais

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
            scriptDaTorre = towerObject.GetComponent<TowerController>(); // Pega a "mente" da torre
        }
    }

    void FixedUpdate()
    {
        if (tower != null && scriptDaTorre != null)
        {
            float distancia = Vector2.Distance(transform.position, tower.position);

            // ATENÇÃO: Troque 'alcanceDeVisao' abaixo pelo nome exato da variável 
            // que você usa no seu TowerController para definir a área que a torre detecta inimigos!
            // Subtraímos 0.2f para que ele invada um pouquinho a zona de perigo antes de parar
            float limiteAtual = pararNoLimiteDaTorre ? (scriptDaTorre.attackRange - 0.2f) : distanciaDeParadaPadrao;

            // Se o inimigo ainda estiver fora do alcance da torre, continua andando
            if (distancia > limiteAtual)
            {
                Vector2 direcao = (tower.position - transform.position).normalized;
                rb.linearVelocity = direcao * moveSpeed;
            }
            else
            {
                // Entrou no raio da torre: puxa o freio instantaneamente
                rb.linearVelocity = Vector2.zero;
            }
        }
    }
}