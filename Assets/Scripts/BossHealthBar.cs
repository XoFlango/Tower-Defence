using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BossHealthBar : MonoBehaviour
{
    // Isso cria uma "linha direta" para qualquer script acessar essa barra
    public static BossHealthBar instance;

    public Slider sliderVida;
    public GameObject painelInteiro; // Para podermos ligar/desligar a barra
    public TextMeshProUGUI textoNomeBoss;

    void Awake()
    {
        instance = this;
        // Começa o jogo com a barra de vida invisível
        painelInteiro.SetActive(false);
    }

    public void MostrarBoss(float vidaMaxima, int nivel)
    {
        painelInteiro.SetActive(true);
        sliderVida.maxValue = vidaMaxima;
        sliderVida.value = vidaMaxima;

        if (textoNomeBoss != null)
        {
            textoNomeBoss.text = "CHEFÃO - Nv " + nivel;
        }
    }

    public void AtualizarVida(float vidaAtual)
    {
        sliderVida.value = vidaAtual;
    }

    public void Esconder()
    {
        painelInteiro.SetActive(false);
    }
}