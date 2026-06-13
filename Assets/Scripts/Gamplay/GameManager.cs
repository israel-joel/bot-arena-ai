using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // Référence aux Bots
    public BotController bot1;
    public BotController bot2;
    // Variable pour la gestion du game over 
    [Header("Game Over")]
    public GameObject gameOver;
    public Text gameOverText;

    // Start is called before the first frame update
    void Start()
    {
        gameOver.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // Si le bot1 meurs on affice le menu de Game Over et le text de victoire du bot2
        if(bot1.Health <= 0)
        {
            gameOver.SetActive(true);
            gameOverText.text = "Yellow Bot WIW the game";
        }

        // Si le bot2 meurs on affice le menu de Game Over et le text de victoire du bot1
        if (bot2.Health <= 0)
        {
            gameOver.SetActive(true);
            gameOverText.text = "Blue Bot WIW the game";
        }
    }
}
