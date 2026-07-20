using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BossHealthBar : MonoBehaviour
{


    public static BossHealthBar instance;

    public Slider sliderVida;
    public GameObject painelInteiro;
    public TextMeshProUGUI textoNomeBoss;

    // NOVO: Variável para o texto dos números
    public TextMeshProUGUI textoVidaNumeros;

    // NOVO: Guarda o valor máximo em segredo para usar na atualização do texto
    private float vidaMaximaInterna;

    /*void Awake()
    {
        instance = this;
        painelInteiro.SetActive(false);
    }

    public void MostrarBoss(float vidaMaxima, int nivel)
    {
        painelInteiro.SetActive(true);
        sliderVida.maxValue = vidaMaxima;
        sliderVida.value = vidaMaxima;

        // Salva a vida máxima para usarmos depois
        vidaMaximaInterna = vidaMaxima;

        if (textoNomeBoss != null)
        {
            textoNomeBoss.text = "CHEFÃO - Nv " + nivel;
        }

        // Já atualiza os números assim que o Boss nasce
        AtualizarTextoNumerico(vidaMaxima);
    }*/
    void Awake()
    {
        instance = this;
       
        painelInteiro.SetActive(false);
    }

    public void MostrarBoss(float vidaMaxima, int nivel)
    {
        
        painelInteiro.SetActive(true);
        sliderVida.maxValue = vidaMaxima;
        sliderVida.value = vidaMaxima;

        vidaMaximaInterna = vidaMaxima;

        if (textoNomeBoss != null)
        {
            textoNomeBoss.text = "CHEFÃO - Nv " + nivel;
        }

        AtualizarTextoNumerico(vidaMaxima);
    }

    public void AtualizarVida(float vidaAtual)
    {
        sliderVida.value = vidaAtual;
        AtualizarTextoNumerico(vidaAtual);
    }

    // --- NOVA FUNÇÃO ---
    // Criamos uma função separada só para organizar a matemática do texto
    private void AtualizarTextoNumerico(float vidaAtual)
    {
        if (textoVidaNumeros != null)
        {
            // O Mathf.Max(0, ...) impede que a vida mostre números negativos 
            // (ex: -15 / 500) se a torre der um tiro mais forte que a vida restante.
            int hpAtual = Mathf.Max(0, Mathf.CeilToInt(vidaAtual));
            int hpMaximo = Mathf.CeilToInt(vidaMaximaInterna);

            // Monta o formato: Atual / Máximo
            textoVidaNumeros.text = hpAtual + " / " + hpMaximo;
        }
    }

    public void Esconder()
    {
        painelInteiro.SetActive(false);
    }




}