using UnityEngine;
using System.Collections;

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
    public int ondaParaTetoDeVariaveis = 15;

    [Header("Chefe (Boss)")]
    public GameObject prefabBoss;
    public int intervaloDeOndasBoss = 10;
    private bool bossSpawnadoNestaOnda = false; // Controla para não nascer mais de 1 boss na onda

    [Header("Enxame (Inimigo Básico)")]
    public GameObject prefabInimigoBasico;
    public float tempoSpawnBasicoInicial = 1.0f;
    public float tempoSpawnBasicoFinal = 0.5f;
    public int loteBasicoInicial = 1;
    public int loteBasicoFinal = 5;
    public float atrasoDentroDoLote = 0.2f;

    [Header("Inimigos Especiais (Chance no Lote)")]
    public int ondaParaLiberarEspeciais = 5;
    [Range(0f, 100f)] public float chanceEspecialInicial = 5f;
    [Range(0f, 100f)] public float chanceEspecialMaxima = 30f;
    public InimigoSorteio[] listaDeEspeciais;

    // Variáveis internas
    private float timerBasico;
    private float currentIntervaloBasico;
    private int currentLoteBasico;
    private float currentChanceEspecial;
    private bool estaSpawnandoLote = false;

    void Start()
    {
        AtualizarDificuldadeDaOnda();
    }

    void Update()
    {
        if (GameManager.instance.emIntervalo)
        {
            bossSpawnadoNestaOnda = false; // Reseta o gatilho para a próxima onda de Boss
            return;
        }

        if (GameManager.instance.inimigosSpawnadosNestaOnda >= GameManager.instance.inimigosNestaOnda) return;

        // --- 1. SPAWN DO BOSS ---
        bool ehOndaDeBoss = (GameManager.instance.ondaAtual % intervaloDeOndasBoss == 0);

        if (ehOndaDeBoss && !bossSpawnadoNestaOnda)
        {
            Spawnar(prefabBoss);
            GameManager.instance.inimigosSpawnadosNestaOnda++;
            bossSpawnadoNestaOnda = true;
        }

        // --- 2. GERAÇÃO DO LOTE (Enxame e Especiais) ---
        if (!estaSpawnandoLote)
        {
            timerBasico += Time.deltaTime;
            if (timerBasico >= currentIntervaloBasico)
            {
                int faltam = GameManager.instance.inimigosNestaOnda - GameManager.instance.inimigosSpawnadosNestaOnda;
                int quantidadeParaSpawnar = Mathf.Min(currentLoteBasico, faltam);

                StartCoroutine(SpawnarLote(quantidadeParaSpawnar));

                AtualizarDificuldadeDaOnda();
                timerBasico = 0f;
            }
        }
    }

    IEnumerator SpawnarLote(int quantidade)
    {
        estaSpawnandoLote = true;
        bool podeSpawnarEspeciais = GameManager.instance.ondaAtual >= ondaParaLiberarEspeciais;

        for (int i = 0; i < quantidade; i++)
        {
            if (GameManager.instance.inimigosSpawnadosNestaOnda >= GameManager.instance.inimigosNestaOnda) break;

            bool virouEspecial = podeSpawnarEspeciais && Random.Range(0f, 100f) <= currentChanceEspecial;

            if (virouEspecial && listaDeEspeciais.Length > 0)
            {
                SpawnarEspecial();
            }
            else
            {
                Spawnar(prefabInimigoBasico);
            }

            GameManager.instance.inimigosSpawnadosNestaOnda++;
            yield return new WaitForSeconds(atrasoDentroDoLote);
        }

        estaSpawnandoLote = false;
    }

    void AtualizarDificuldadeDaOnda()
    {
        float progressoLimitado = 0f;
        float progressoInfinito = 0f;

        if (GameManager.instance != null)
        {
            progressoInfinito = (float)(GameManager.instance.ondaAtual - 1) / (ondaParaTetoDeVariaveis - 1);
            progressoLimitado = Mathf.Clamp01(progressoInfinito);
        }

        currentIntervaloBasico = Mathf.Lerp(tempoSpawnBasicoInicial, tempoSpawnBasicoFinal, progressoLimitado);
        currentChanceEspecial = Mathf.Lerp(chanceEspecialInicial, chanceEspecialMaxima, progressoLimitado);

        currentLoteBasico = Mathf.RoundToInt(Mathf.LerpUnclamped(loteBasicoInicial, loteBasicoFinal, progressoInfinito));
        if (currentLoteBasico < 1) currentLoteBasico = 1;
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