using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPrincipal : MonoBehaviour
{
    void Start()
    {

    }

    void Update()
    {

    }
    public void Menu()
    {
        SceneManager.LoadScene("Menu");

    }
    public void Player()
    {
        SceneManager.LoadScene("Player");
    }
    public void Creditos()
    {
        SceneManager.LoadScene("Creditos");
    }
}
