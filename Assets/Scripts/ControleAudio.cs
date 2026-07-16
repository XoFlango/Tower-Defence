using UnityEngine;
using UnityEngine.UI;

public class ControleAudio : MonoBehaviour
{
    public Slider sliderVolume;
    public Text textoStatusMudo;
    public GameObject painelConfig;
    private bool estaMudo = false;

    void Start()
    {
        // Inicializa o slider com o volume atual
        if (sliderVolume != null)
        {
            sliderVolume.value = AudioListener.volume;
            sliderVolume.onValueChanged.AddListener(AlterarVolume);
        }

        // Painel começa fechado
        if (painelConfig != null)
            painelConfig.SetActive(false);
    }

    // Chamado pelo botão Configurações
    public void AbrirConfiguracoes()
    {
        painelConfig.SetActive(true);
    }

    // Chamado pelo botão de fechar dentro do painel
    public void FecharConfiguracoes()
    {
        painelConfig.SetActive(false);
    }

    // Chamado pelo botão Mutar/Desmutar
    public void AlternarMudo()
    {
        estaMudo = !estaMudo;
        AudioListener.volume = estaMudo ? 0f : sliderVolume.value;

        if (textoStatusMudo != null)
            textoStatusMudo.text = estaMudo ? "Mudo: ON" : "Mudo: OFF";
    }

    // Chamado pelo slider
    public void AlterarVolume(float valor)
    {
        if (!estaMudo)
        {
            AudioListener.volume = valor;
        }
    }
}