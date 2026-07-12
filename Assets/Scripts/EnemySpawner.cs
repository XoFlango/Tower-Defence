using UnityEngine;

// System.Serializable faz com que essa classe customizada apareça no Inspector da Unity
[System.Serializable]
public class InimigoSorteio
{
    public GameObject prefab;
    [Tooltip("Quanto maior o número, maior a chance deste inimigo nascer.")]
    public float pesoDeSpawn = 100f;
}

public class EnemySpawner : MonoBehaviour
{
    [Header("Configurações de Spawn")]
    // Substituímos o array de GameObjects pelo array da nossa nova classe
    public InimigoSorteio[] listaDeInimigos;
    public float spawnRadius = 8f;

    [Header("Dificuldade Inicial (Lento)")]
    public float minSpawnInicial = 1.0f;
    public float maxSpawnInicial = 3.0f;

    [Header("Dificuldade Máxima (Caos)")]
    public float minSpawnFinal = 0.2f;
    public float maxSpawnFinal = 0.8f;

    [Header("Escalonamento")]
    public float tempoParaDificuldadeMaxima = 120f;

    private float timer;
    private float tempoDeJogo;
    private float currentSpawnInterval;

    void Start()
    {
        SortearNovoIntervalo();
    }

    void Update()
    {
        tempoDeJogo += Time.deltaTime;
        timer += Time.deltaTime;

        if (timer >= currentSpawnInterval)
        {
            SpawnEnemy();
            timer = 0f;
            SortearNovoIntervalo();
        }
    }

    void SortearNovoIntervalo()
    {
        float progressoDaDificuldade = Mathf.Clamp01(tempoDeJogo / tempoParaDificuldadeMaxima);
        float minAtual = Mathf.Lerp(minSpawnInicial, minSpawnFinal, progressoDaDificuldade);
        float maxAtual = Mathf.Lerp(maxSpawnInicial, maxSpawnFinal, progressoDaDificuldade);
        currentSpawnInterval = Random.Range(minAtual, maxAtual);
    }

    void SpawnEnemy()
    {
        if (listaDeInimigos.Length == 0) return;

        // --- LÓGICA DE POSIÇÃO E ROTAÇÃO ---
        float randomAngle = Random.Range(0f, 360f);
        float angleRad = randomAngle * Mathf.Deg2Rad;
        Vector2 spawnDirection = new Vector2(Mathf.Cos(angleRad), Mathf.Sin(angleRad));

        Vector2 spawnPos = (Vector2)transform.position + (spawnDirection * spawnRadius);
        float angleToTower = Mathf.Atan2(-spawnDirection.y, -spawnDirection.x) * Mathf.Rad2Deg;
        Quaternion spawnRotation = Quaternion.Euler(0, 0, angleToTower);

        // --- NOVA LÓGICA DE SORTEIO POR PESO ---
        // 1. Calcula a soma de todos os pesos
        float pesoTotal = 0f;
        foreach (InimigoSorteio inimigo in listaDeInimigos)
        {
            pesoTotal += inimigo.pesoDeSpawn;
        }

        // 2. Sorteia um valor entre 0 e o peso total
        float valorSorteado = Random.Range(0f, pesoTotal);
        GameObject prefabEscolhido = null;

        // 3. Subtrai os pesos até encontrar o inimigo sorteado
        foreach (InimigoSorteio inimigo in listaDeInimigos)
        {
            valorSorteado -= inimigo.pesoDeSpawn;

            if (valorSorteado <= 0f)
            {
                prefabEscolhido = inimigo.prefab;
                break;
            }
        }

        // Trava de segurança caso o float falhe por precisão
        if (prefabEscolhido == null)
        {
            prefabEscolhido = listaDeInimigos[0].prefab;
        }

        // 4. Instancia o inimigo escolhido
        Instantiate(prefabEscolhido, spawnPos, spawnRotation);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, spawnRadius);
    }
}