using UnityEngine;
using TMPro;

public class TowerHealth : MonoBehaviour
{
    [Header("Atributos de Vida")]
    // Agora a vida base é 10!
    public float vidaMaxima = 10f;
    private float vidaAtual;

    [Header("Regeneração")]
    public float regeneracaoPorSegundo = 0f; // Começa em 0. A loja vai aumentar isso.

    [Header("Interface")]
    public TextMeshProUGUI textoVida;

    void Start()
    {
        vidaAtual = vidaMaxima;
        AtualizarTextoDeVida();
    }

    void Update()
    {
        // Se a torre estiver ferida e tiver o upgrade de regeneração
        if (vidaAtual < vidaMaxima && regeneracaoPorSegundo > 0f)
        {
            // Cura a torre gradualmente baseada no tempo do jogo
            vidaAtual += regeneracaoPorSegundo * Time.deltaTime;

            // Trava para não curar além do máximo
            if (vidaAtual > vidaMaxima)
            {
                vidaAtual = vidaMaxima;
            }

            AtualizarTextoDeVida();
        }
    }

    public void ReceberDano(float quantidade)
    {
        vidaAtual -= quantidade;
        AtualizarTextoDeVida();

        if (vidaAtual <= 0)
        {
            vidaAtual = 0;
            DestruirTorre();
        }
    }

    // --- NOVA FUNÇÃO PARA A LOJA ---
    public void AumentarRegeneracao(float quantidade)
    {
        regeneracaoPorSegundo += quantidade;
    }

    public void AumentarVidaMaxima(float quantidade)
    {
        vidaMaxima += quantidade;
        vidaAtual += quantidade;
        AtualizarTextoDeVida();
    }

    void AtualizarTextoDeVida()
    {
        if (textoVida != null)
        {
            int vidaParaMostrar = Mathf.CeilToInt(vidaAtual);
            textoVida.text = "HP: " + vidaParaMostrar.ToString();
        }
    }

    void DestruirTorre()
    {
        if (GameManager.instance != null)
        {
            GameManager.instance.AtivarGameOver();
        }
    }
}