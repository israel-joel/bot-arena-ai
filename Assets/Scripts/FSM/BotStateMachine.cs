// Composant de gestion des états du bot.
// Permet de changer d'état et d'exécuter l'état courant à chaque frame.
using UnityEngine;

public class BotStateMachine : MonoBehaviour
{
    private IBotState currentState;

    // Méthode qui gère le changement d'état du bot
    public void ChangeState(IBotState newState)
    {
        if (currentState != null) currentState.Exit(); // Si un état est en cour on sort d'abord de l'état courant avant passer au nouvelle états
        currentState = newState;
        currentState.Enter();
    }

    void Update()
    {
        if (currentState != null)
            currentState.Execute(); // Exécute l'état courant
    }
}
