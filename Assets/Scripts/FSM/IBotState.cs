// Interface de base pour tous les états du bot.
// Chaque état doit implémenter ces trois méthodes.
public interface IBotState
{
    void Enter();   // Appelé lors de l'entrée dans l'état
    void Execute(); // Appelé à chaque frame
    void Exit();    // Appelé lors de la sortie de l'état
}
