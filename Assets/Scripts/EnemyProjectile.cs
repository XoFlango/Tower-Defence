using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public float velocidade = 5f;
    public float dano = 2f;
    public float tempoDeVida = 4f; // Destrói o tiro se errar ou passar direto

    void Start()
    {
        Destroy(gameObject, tempoDeVida);
    }

    void Update()
    {
        // Move o tiro para frente (a rotação já apontará para a torre na hora do spawn)
        transform.Translate(Vector3.right * velocidade * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D colisao)
    {
        // Se bater na torre, causa dano e destrói a bala
        if (colisao.CompareTag("Torre"))
        {
            TowerHealth saudeTorre = colisao.GetComponent<TowerHealth>();
            if (saudeTorre != null)
            {
                saudeTorre.ReceberDano(dano);
            }
            Destroy(gameObject);
        }
    }
}