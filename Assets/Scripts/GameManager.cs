using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections; // NOVO: Obrigatório para usar Coroutines (temporizadores)

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Economia")]
    public int moedas = 0;
    public float multiplicadorDeMoedas = 1.0f;
    public TextMeshProUGUI textoMoedas;

    [Header("Sistema de Ondas")]
    public int ondaAtual = 1;
    public int inimigosNestaOnda = 15; // Começa em 15
    public float multiplicadorDeInimigos = 1.05f; // Cresce 5% a cada onda

    // Variáveis de controle invisíveis para o jogador
     public int inimigosSpawnadosNestaOnda = 0;
     public int inimigosMortosNaOnda = 0;
    [HideInInspector] public bool emIntervalo = false;

    public float tempoDeIntervalo = 5f; // Segundos de descanso entre as ondas

    [Header("Interface das Ondas")]
    public TextMeshProUGUI textoOnda;
    public TextMeshProUGUI textoAvisoOnda; // NOVO: Para mostrar "Próxima onda em 3..."

    [Header("Telas")]
    public GameObject painelGameOver;

    [Header("Meta Progressão")]
    public int metaMoedas = 0; // Quantidade atual de €
    public TextMeshProUGUI textoMetaMoedas; // O texto na tela

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        Time.timeScale = 1f;
        if (painelGameOver != null) painelGameOver.SetActive(false);
        if (textoAvisoOnda != null) textoAvisoOnda.gameObject.SetActive(false);

        AtualizarInterface();
    }

    // --- ECONOMIA ---
    public void AdicionarMoedas(int valorBase)
    {
        int valorFinal = Mathf.RoundToInt(valorBase * multiplicadorDeMoedas);
        moedas += valorFinal;
        AtualizarInterface();
    }

    public bool GastarMoedas(int valor)
    {
        if (moedas >= valor)
        {
            moedas -= valor;
            AtualizarInterface();
            return true;
        }
        return false;
    }

    public void AdicionarMetaMoedas(int quantidade)
    {
        metaMoedas += quantidade;
        AtualizarUITextoMetaMoedas();
    }

    private void AtualizarUITextoMetaMoedas()
    {
        if (textoMetaMoedas != null)
        {
            textoMetaMoedas.text = "€ " + metaMoedas;
        }
    }

    // --- NOVO SISTEMA DE ONDAS ---
    public void RegistrarMorteInimigo()
    {
        inimigosMortosNaOnda++;

        // Se o jogador matou TODOS os inimigos da onda atual...
        if (inimigosMortosNaOnda >= inimigosNestaOnda && !emIntervalo)
        {
            StartCoroutine(RotinaDeIntervalo()); // Inicia o descanso
        }
    }

    // A Coroutine permite pausar a execução do código (esperar os segundos passarem) sem travar o jogo inteiro
    IEnumerator RotinaDeIntervalo()
    {
        emIntervalo = true; // Avisa o Spawner para parar de trabalhar

        if (textoAvisoOnda != null) textoAvisoOnda.gameObject.SetActive(true);

        // Faz uma contagem regressiva no texto da tela
        for (float i = tempoDeIntervalo; i > 0; i--)
        {
            if (textoAvisoOnda != null) textoAvisoOnda.text = "Próxima onda em: " + i;

            yield return new WaitForSeconds(1f); // Espera exatamente 1 segundo real
        }

        if (textoAvisoOnda != null) textoAvisoOnda.gameObject.SetActive(false);

        // Prepara a matemática da próxima onda
        ondaAtual++;
        inimigosMortosNaOnda = 0;
        inimigosSpawnadosNestaOnda = 0; // Zera para o Spawner começar de novo
        AdicionarMetaMoedas(ondaAtual);
        // Ex: Onda 1 era 15. Onda 2 será 15 * 1.25 = 19 inimigos.
        inimigosNestaOnda = Mathf.RoundToInt(inimigosNestaOnda * multiplicadorDeInimigos);

        emIntervalo = false; // Libera o Spawner para trabalhar novamente
        AtualizarInterface();
    }
    public string FormatarMoedas(int valor)
    {
        if (valor >= 1000000)
        {
            return (valor / 1000000f).ToString("0.##") + "M"; // Já deixamos pronto para os milhões!
        }
        else if (valor >= 1000)
        {
            return (valor / 1000f).ToString("0.##") + "k";
        }

        // Se for menor que mil, retorna o número normal
        return valor.ToString();
    }


    // --- INTERFACE & GAME OVER ---
    void AtualizarInterface()
    {
        if (textoMoedas != null) textoMoedas.text = "$: " + FormatarMoedas(moedas);

        if (textoOnda != null) textoOnda.text = "Onda: " + ondaAtual;
    }

    public void AtivarGameOver() { Time.timeScale = 0f; if (painelGameOver != null) painelGameOver.SetActive(true); }
    public void ReiniciarPartida() { Time.timeScale = 1f; SceneManager.LoadScene(SceneManager.GetActiveScene().name); }
}