using UnityEngine;

public class ProjectilePhysics : MonoBehaviour
{
    [Header("Configurações de Movimento")]
    public float launchForce = 15f;

    [Header("Configurações de Dano")]
    [HideInInspector] public int dano = 1;
    [HideInInspector] public float raioDeExplosao = 0f;

    [Header("Otimização")]
    public LayerMask enemyLayer;

    public float lifeTime = 3f;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (rb == null)
        {
            Debug.LogError("ERRO: O Prefab do Projétil precisa de um componente Rigidbody2D!");
            return;
        }

        rb.AddForce(transform.right * launchForce, ForceMode2D.Impulse);
        Destroy(gameObject, lifeTime);
    }

    public void Configurar(int danoDaTorre, float raioDaExplosao)
    {
        dano = danoDaTorre;
        raioDeExplosao = Mathf.Max(0f, raioDaExplosao);
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        if (hitInfo.CompareTag("Inimigo"))
        {
            // Aplica a lógica de dano (em área ou alvo único)
            if (raioDeExplosao > 0f)
            {
                Explodir();
            }
            else
            {
                Enemy inimigoAtingido = hitInfo.GetComponent<Enemy>();
                if (inimigoAtingido != null)
                {
                    inimigoAtingido.ReceberDano(dano);
                }
            }

            // Destrói a bala após o impacto
            Destroy(gameObject);
        }
    }

    void Explodir()
    {
        Collider2D[] atingidos = Physics2D.OverlapCircleAll(transform.position, raioDeExplosao, enemyLayer);

        foreach (Collider2D hit in atingidos)
        {
            if (hit.CompareTag("Inimigo"))
            {
                Enemy inimigoAtingido = hit.GetComponent<Enemy>();

                if (inimigoAtingido != null)
                {
                    inimigoAtingido.ReceberDano(dano);
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (raioDeExplosao > 0f)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, raioDeExplosao);
        }
    }
}