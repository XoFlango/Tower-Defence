using UnityEngine;

public class ProjectilePhysics : MonoBehaviour
{
    [Header("Configurações de Movimento")]
    [Tooltip("Força inicial que lança o projétil. Ajuste no Inspector.")]
    public float launchForce = 15f;

    [Header("Configurações de Dano")]
    [HideInInspector] public int dano = 1;
    [HideInInspector] public float raioDeExplosao = 0f; // NOVO: Controla se a bala explode
    
    [Header("Efeitos Visuais")]
    public GameObject prefabExplosao;

    public float lifeTime = 3f;

    private Rigidbody2D rb;

    void Start()
    {
        // 1. Pega a referência do Rigidbody2D
        rb = GetComponent<Rigidbody2D>();

        if (rb == null)
        {
            Debug.LogError("ERRO: O Prefab do Projétil precisa de um componente Rigidbody2D!");
            return;
        }

        // 2. Aplica a força INSTANTANEAMENTE na direção 'frente' (transform.right)
        rb.AddForce(transform.right * launchForce, ForceMode2D.Impulse);

        // 3. Destrói o objeto após o tempo de vida
        Destroy(gameObject, lifeTime);
    }

    // --- NOVA FUNÇÃO DE CONFIGURAÇÃO ---
    // A Torre vai chamar essa função logo após criar a bala para passar os upgrades da loja
    public void Configurar(int danoDaTorre, float raioDaExplosao)
    {
        dano = danoDaTorre;
        raioDeExplosao = raioDaExplosao;
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        if (hitInfo.CompareTag("Inimigo"))
        {
            // 1. INSTANCIA O EFEITO SEMPRE QUE BATER (Tiro normal ou explosivo)
            if (prefabExplosao != null)
            {
                // Força o Z a ser 0 para garantir que a partícula apareça na frente da câmera
                Vector3 posicaoCorrecaoZ = new Vector3(transform.position.x, transform.position.y, 0f);
                Instantiate(prefabExplosao, posicaoCorrecaoZ, Quaternion.identity);
            }

            // 2. APLICA A LÓGICA DE DANO
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

            // 3. DESTRÓI A BALA
            Destroy(gameObject);
        }
    }

    void Explodir()
    {
        Collider2D[] atingidos = Physics2D.OverlapCircleAll(transform.position, raioDeExplosao);

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

    // Desenha o círculo de explosão na aba 'Scene' da Unity para facilitar o ajuste do tamanho
    private void OnDrawGizmosSelected()
    {
        if (raioDeExplosao > 0f)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, raioDeExplosao);
        }
    }
}