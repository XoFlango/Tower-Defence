using UnityEngine;

public class ProjectilePhysics : MonoBehaviour
{
    [Header("Configurações de Movimento")]
    [Tooltip("Força inicial que lança o projétil. Ajuste no Inspector.")]
    public float launchForce = 15f; // Valor padrão robusto

    [Header("Configurações de Dano")]
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
        // Usamos Impulse para dar um 'kick' inicial.
        rb.AddForce(transform.right * launchForce, ForceMode2D.Impulse);

        // 3. Destrói o objeto após o tempo de vida
        Destroy(gameObject, lifeTime);
    }

    // A lógica de dano permanece a mesma
    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        if (hitInfo.CompareTag("Inimigo"))
        {
            // Tenta pegar o script "Enemy" do quadrado que o tiro acertou
            Enemy inimigoAtingido = hitInfo.GetComponent<Enemy>();

            if (inimigoAtingido != null)
            {
                inimigoAtingido.ReceberDano(1); // Causa 1 de dano
            }

            // O tiro se destrói ao bater, independente se o inimigo morreu ou não
            Destroy(gameObject);
        }
    }
}