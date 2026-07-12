using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [Header("Configurações do Inimigo")]
    public float moveSpeed = 3f;

    private Transform tower;

    void Start()
    {
        // Procura a torre pelo nome exato que está na hierarquia da Unity.
        // Se o seu objeto da torre tiver outro nome, mude "Torre" aqui abaixo!
        GameObject towerObject = GameObject.Find("Torre");

        if (towerObject != null)
        {
            tower = towerObject.transform;
        }
    }

    void Update()
    {
        if (tower != null)
        {
            // MoveTowards puxa o inimigo exatamente para a posição do alvo, 
            // ignorando para qual lado o "transform.right" está apontando.
            transform.position = Vector2.MoveTowards(transform.position, tower.position, moveSpeed * Time.deltaTime);
        }
    }
}