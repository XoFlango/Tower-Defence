using UnityEngine;

[System.Serializable]
public class InimigoSorteio
{
    public GameObject prefab;
    public float pesoDeSpawn = 100f;
}

public class EnemySpawner : MonoBehaviour
{
    [Header("Configurações Gerais")]
    public float spawnRadius = 8f;
    [Tooltip("Em qual onda o jogo atinge o limite do enxame?")]
    public int ondaParaDificuldadeMaxima = 15;

    [Header("Enxame (Inimigo Básico)")]
    public GameObject prefabInimigoBasico;
    public float tempoSpawnBasicoInicial = 1.0f;
    public float tempoSpawnBasicoFinal = 0.1f;
    public int loteBasicoInicial = 1;
    public int loteBasicoFinal = 4;

    [Header("Inimigos Especiais (Sorteio)")]
    [Tooltip("A partir de qual onda os inimigos especiais começam a aparecer?")]
    public int ondaParaLiberarEspeciais = 5; // NOVO: A trava das ondas!
    public InimigoSorteio[] listaDeEspeciais;
    public float tempoSpawnEspecialInicial = 4.0f;
    public float tempoSpawnEspecialFinal = 1.5f;

    // Variáveis internas
    private float timerBasico;
    private float timerEspecial;

    private float currentIntervaloBasico;
    private int currentLoteBasico;
    private float currentIntervaloEspecial;

    void Start()
    {
        AtualizarDificuldadeDaOnda();
    }

    void Update()
    {
        if (GameManager.instance.emIntervalo) return;
        if (GameManager.instance.inimigosSpawnadosNestaOnda >= GameManager.instance.inimigosNestaOnda) return;

        // 1. GERAÇÃO DO ENXAME (Sempre o Inimigo Básico)
        timerBasico += Time.deltaTime;
        if (timerBasico >= currentIntervaloBasico)
        {
            int faltam = GameManager.instance.inimigosNestaOnda - GameManager.instance.inimigosSpawnadosNestaOnda;
            int quantidadeParaSpawnar = Mathf.Min(currentLoteBasico, faltam);

            for (int i = 0; i < quantidadeParaSpawnar; i++)
            {
                Spawnar(prefabInimigoBasico);
                GameManager.instance.inimigosSpawnadosNestaOnda++;
            }

            AtualizarDificuldadeDaOnda();
            timerBasico = 0f;
        }

        // 2. GERAÇÃO DE ESPECIAIS (Sorteio de Tanque, Atirador, etc)
        // NOVA LÓGICA: Verifica se a onda atual já atingiu a onda de liberação
        bool podeSpawnarEspeciais = GameManager.instance.ondaAtual >= ondaParaLiberarEspeciais;

        if (podeSpawnarEspeciais && listaDeEspeciais.Length > 0 && GameManager.instance.inimigosSpawnadosNestaOnda < GameManager.instance.inimigosNestaOnda)
        {
            timerEspecial += Time.deltaTime;
            if (timerEspecial >= currentIntervaloEspecial)
            {
                SpawnarEspecial();
                GameManager.instance.inimigosSpawnadosNestaOnda++;
                timerEspecial = 0f;
            }
        }
    }

    void AtualizarDificuldadeDaOnda()
    {
        float progresso = 0f;

        if (GameManager.instance != null)
        {
            progresso = (float)(GameManager.instance.ondaAtual - 1) / (ondaParaDificuldadeMaxima - 1);
            progresso = Mathf.Clamp01(progresso);
        }

        currentIntervaloBasico = Mathf.Lerp(tempoSpawnBasicoInicial, tempoSpawnBasicoFinal, progresso);
        currentLoteBasico = Mathf.RoundToInt(Mathf.Lerp(loteBasicoInicial, loteBasicoFinal, progresso));
        currentIntervaloEspecial = Mathf.Lerp(tempoSpawnEspecialInicial, tempoSpawnEspecialFinal, progresso);
    }

    void Spawnar(GameObject prefab)
    {
        float randomAngle = Random.Range(0f, 360f);
        float angleRad = randomAngle * Mathf.Deg2Rad;
        Vector2 spawnDirection = new Vector2(Mathf.Cos(angleRad), Mathf.Sin(angleRad));

        Vector2 spawnPos = (Vector2)transform.position + (spawnDirection * spawnRadius);
        float angleToTower = Mathf.Atan2(-spawnDirection.y, -spawnDirection.x) * Mathf.Rad2Deg;
        Quaternion spawnRotation = Quaternion.Euler(0, 0, angleToTower);

        Instantiate(prefab, spawnPos, spawnRotation);
    }

    void SpawnarEspecial()
    {
        float pesoTotal = 0f;
        foreach (InimigoSorteio inimigo in listaDeEspeciais) pesoTotal += inimigo.pesoDeSpawn;

        float valorSorteado = Random.Range(0f, pesoTotal);
        GameObject prefabEscolhido = listaDeEspeciais[0].prefab;

        foreach (InimigoSorteio inimigo in listaDeEspeciais)
        {
            valorSorteado -= inimigo.pesoDeSpawn;
            if (valorSorteado <= 0f)
            {
                prefabEscolhido = inimigo.prefab;
                break;
            }
        }

        Spawnar(prefabEscolhido);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, spawnRadius);
    }
}