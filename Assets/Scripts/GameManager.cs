using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Economia")]
    public int moedas = 0;
    public TextMeshProUGUI textoMoedas;

    [Header("Upgrades Especiais")]
    public float multiplicadorDeMoedas = 1.0f;

    [Header("Sistema de Ondas")]
    public int ondaAtual = 1;
    public int inimigosMortosNaOnda = 0;
    public int metaDeInimigos = 10; // Quantos inimigos precisam morrer para a onda 2
    public TextMeshProUGUI textoOnda; // Referência para o texto na tela

    [Header("Telas")]
    public GameObject painelGameOver;

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        Time.timeScale = 1f;
        if (painelGameOver != null) painelGameOver.SetActive(false);

        AtualizarInterface();
    }

    // --- ECONOMIA ---
    public void AdicionarMoedas(int valorBase)
    {
        // Aplica o multiplicador (ex: ganhar 1 * 1.5 = 1.5) e arredonda.
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

    // --- SISTEMA DE ONDAS ---
    public void RegistrarMorteInimigo()
    {
        inimigosMortosNaOnda++;

        // Verifica se atingiu a meta para passar de onda
        if (inimigosMortosNaOnda >= metaDeInimigos)
        {
            AvancarOnda();
        }
    }

    void AvancarOnda()
    {
        ondaAtual++;
        inimigosMortosNaOnda = 0;
        metaDeInimigos += 5; // A próxima onda exigirá 5 mortes a mais (15, 20, 25...)

        AtualizarInterface();
    }

    // --- INTERFACE ---
    void AtualizarInterface()
    {
        if (textoMoedas != null) textoMoedas.text = "Moedas: " + moedas;

        if (textoOnda != null) textoOnda.text = "Onda: " + ondaAtual;
    }

    public void AtivarGameOver()
    {
        // Congela o tempo do jogo (inimigos e tiros param na hora)
        Time.timeScale = 0f;

        // Mostra a tela de Game Over
        if (painelGameOver != null)
        {
            painelGameOver.SetActive(true);
        }
    }

    public void ReiniciarPartida()
    {
        // Restaura o tempo ao normal
        Time.timeScale = 1f;

        // Recarrega a cena atual do zero
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
