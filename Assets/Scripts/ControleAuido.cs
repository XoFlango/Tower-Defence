using UnityEngine;
using UnityEngine.UI;

public class ControleAudio : MonoBehaviour
{
    public AudioSource musicaDeFundo;
    public Slider sliderVolume;
    public Text textoStatus;
    private bool estaMudo = false;

    void Start()
    {
        if (sliderVolume != null)
        {
            sliderVolume.value = AudioListener.volume;
            sliderVolume.onValueChanged.AddListener(AlterarVolume);
        }
    }

    public void AlternarMudo()
    {
        estaMudo = !estaMudo;
        AudioListener.volume = estaMudo ? 0f : (sliderVolume != null ? sliderVolume.value : 1f);

        if (textoStatus != null)
            textoStatus.text = estaMudo ? "Mudo: ON" : "Mudo: OFF";
    }

    public void AlterarVolume(float valor)
    {
        if (!estaMudo)
        {
            AudioListener.volume = valor;
        }
    }

    public void AumentarVolume()
    {
        float novo = Mathf.Clamp(AudioListener.volume + 0.1f, 0f, 1f);
        AudioListener.volume = novo;
        if (sliderVolume != null) sliderVolume.value = novo;
        estaMudo = false;
    }

    public void DiminuirVolume()
    {
        float novo = Mathf.Clamp(AudioListener.volume - 0.1f, 0f, 1f);
        AudioListener.volume = novo;
        if (sliderVolume != null) sliderVolume.value = novo;
        estaMudo = false;
    }
}