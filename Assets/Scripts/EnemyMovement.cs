using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [Header("Configura��es do Inimigo")]
    public float moveSpeed = 3f;

    private Transform tower;
    private Rigidbody2D rb;

    void Start()
    {
        // Pega o componente de f�sica do pr�prio inimigo
        rb = GetComponent<Rigidbody2D>();

        GameObject towerObject = GameObject.Find("Torre");

        if (towerObject != null)
        {
            tower = towerObject.transform;
        }
    }

    // FixedUpdate roda em sincronia perfeita com a engine de f�sica
    void FixedUpdate()
    {
        if (tower != null)
        {
            // Calcula uma "seta" (vetor) apontando do inimigo para a torre
            Vector2 direcao = (tower.position - transform.position).normalized;

            // Empurra o inimigo nessa dire��o usando a velocidade da f�sica
            rb.linearVelocity = direcao * moveSpeed;
        }
    }
}