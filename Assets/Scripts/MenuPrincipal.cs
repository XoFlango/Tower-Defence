using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPrincipal : MonoBehaviour
{
    // Carrega a cena do jogo
    public void IniciarJogo()
    {
        SceneManager.LoadScene("TheGame");
    }

    // Sai do jogo
    public void SairDoJogo()
    {
        Application.Quit();
        Debug.Log("Jogo fechado!"); // Só aparece no editor
    }
}