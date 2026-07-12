using UnityEngine;
using TMPro;

public class TowerHealth : MonoBehaviour
{
    [Header("Atributos de Vida")]
    public float vidaMaxima = 100f;
    private float vidaAtual;

    [Header("Interface")]
    public TextMeshProUGUI textoVida;

    void Start()
    {
        vidaAtual = vidaMaxima;
        AtualizarTextoDeVida();
    }

    // Função que será chamada pelos inimigos
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

    void AtualizarTextoDeVida()
    {
        if (textoVida != null)
        {
            // Mathf.RoundToInt arredonda o valor para não mostrar números quebrados na tela
            textoVida.text = "HP: " + Mathf.RoundToInt(vidaAtual).ToString();
        }
    }

    void DestruirTorre()
    {
        // Chama a função de Game Over que vamos criar no GameManager
        if (GameManager.instance != null)
        {
            GameManager.instance.AtivarGameOver();
        }
    }
}