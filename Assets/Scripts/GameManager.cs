using UnityEngine;
using TMPro; // Biblioteca necessária para mexer com textos na interface

public class GameManager : MonoBehaviour
{
    // O Singleton: Permite que outros scripts acessem este via "GameManager.instance"
    public static GameManager instance;

    [Header("Economia")]
    public int moedas = 0;
    public TextMeshProUGUI textoMoedas; // Referência ao texto na tela

    void Awake()
    {
        // Garante que só exista um GameManager na cena
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        AtualizarInterface(); // Garante que o texto comece mostrando "0"
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
}