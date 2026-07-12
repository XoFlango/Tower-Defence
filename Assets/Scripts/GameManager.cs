using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement; // Adicione esta linha no topo!

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Economia")]
    public int moedas = 0;
    public TextMeshProUGUI textoMoedas;

    [Header("Telas")]
    public GameObject painelGameOver; // Referência para a tela de fim de jogo

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        // Garante que o jogo comece rodando normalmente e esconde o Game Over
        Time.timeScale = 1f;
        if (painelGameOver != null) painelGameOver.SetActive(false);

        AtualizarInterface();
    }

    public void AdicionarMoedas(int valor)
    {
        moedas += valor;
        AtualizarInterface();
    }

    void AtualizarInterface()
    {
        if (textoMoedas != null)
        {
            textoMoedas.text = "Moedas: " + moedas;
        }
    }

    public bool GastarMoedas(int valor)
    {
        if (moedas >= valor)
        {
            moedas -= valor;
            AtualizarInterface();
            return true;
        }
        return false; // Não tem moedas suficientes
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
