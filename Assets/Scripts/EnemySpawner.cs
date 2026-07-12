using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Referências")]
    public GameObject enemyPrefab;
    public Transform tower; // Arraste a sua Torre para cá no Inspector

    [Header("Configurações de Spawn")]
    public float spawnRadius = 8f;  // Distância que os inimigos vão nascer
    public float spawnRate = 2f;    // Tempo em segundos entre cada inimigo

    private float nextSpawnTime = 0f;

    void Update()
    {
        // Controla o tempo para instanciar o próximo inimigo
        if (Time.time >= nextSpawnTime)
        {
            SpawnEnemy();
            nextSpawnTime = Time.time + spawnRate;
        }
    }

    void SpawnEnemy()
    {
        // 1. Sorteia um ângulo aleatório em radianos (0 a 360 graus)
        float randomAngle = Random.Range(0f, Mathf.PI * 2f);

        // 2. Calcula a posição exata na borda do círculo usando Seno e Cosseno
        Vector2 spawnDirection = new Vector2(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle));
        Vector2 spawnPosition = (Vector2)tower.position + (spawnDirection * spawnRadius);

        // 3. Calcula a direção em que o inimigo deve olhar (para o centro)
        Vector2 directionToTower = ((Vector2)tower.position - spawnPosition).normalized;

        // Atan2 descobre o ângulo em radianos entre dois pontos, Rad2Deg converte para graus
        float angleToTower = Mathf.Atan2(directionToTower.y, directionToTower.x) * Mathf.Rad2Deg;

        // Cria a rotação final no eixo Z (padrão do 2D)
        // NOTA: Se o "rosto" do seu quadrado estiver virado para CIMA na arte original, mude para: angleToTower - 90f
        Quaternion spawnRotation = Quaternion.Euler(0, 0, angleToTower);

        // 4. Instancia o inimigo na posição e rotação corretas
        Instantiate(enemyPrefab, spawnPosition, spawnRotation);
    }

    // Pro-tip: Isso desenha o raio de spawn de vermelho no Editor para você visualizar!
    private void OnDrawGizmosSelected()
    {
        if (tower != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(tower.position, spawnRadius);
        }
    }
}