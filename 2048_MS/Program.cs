using System;

namespace _2048_MS
{
    internal class Program
    {
        private static int[,] board = new int[4, 4];
        private static bool[,] fusionTuile = new bool[4, 4];
        private static int score = 0;

        static void Main(string[] args)
        {

            DebutJeu(); // Initialise le jeu en plaçant deux tuiles aléatoires sur le plateau


            // Boucle principale du jeu qui s'exécute indéfiniment jusqu'à ce que l'utilisateur quitte
            while (true)
            {
                AffichageBoard(); // Affiche le plateau de jeu initial (appelé board dans la suite des commentaires)
                                  // Afficher le score actuel du joueur
               
                // Vérifie si le jeu est terminé (plus de mouvements possibles)
                if (Partiefini())
                {
                    Console.WriteLine("Game Over! Votre score: " + score);
                    Console.WriteLine("Appuyer sur une touche pour quitter.");
                    Console.ReadKey(); // Attend une touche avant de quitter
                    break;
                }
                else
                {
                    Console.WriteLine("Score: " + score);

                    // Afficher les instructions pour les déplacements et la sortie du jeu
                    Console.WriteLine("Utilisez les flèches directionnelles pour déplacer les tuiles. Appuyez sur c pour quitter.");
                }
                // Vérifie si le joueur a gagné (atteint la tuile 2048)
                if (PartieGagne())
                {
                   
                    Console.ReadKey(); // Attend une touche avant de quitter
                    break;
                }

                // Récupère la touche pressée par l'utilisateur sans afficher la touche à l'écran
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                ConsoleKey key = keyInfo.Key;

                // Quitte la boucle si l'utilisateur appuie sur la touche 'C'
                if (key == ConsoleKey.C)
                    break;

                // Variable pour indiquer si une tuile a été déplacée lors du dernier mouvement
                bool moved = false;

                // Switch pour traiter la touche pressée par l'utilisateur
                switch (key)
                {
                    case ConsoleKey.UpArrow: // Déplacer vers le haut
                        moved = DeplaceTuile(0, -1);
                        break;
                    case ConsoleKey.DownArrow: // Déplacer vers le bas
                        moved = DeplaceTuile(0, 1);
                        break;
                    case ConsoleKey.LeftArrow: // Déplacer vers la gauche
                        moved = DeplaceTuile(-1, 0);
                        break;
                    case ConsoleKey.RightArrow: // Déplacer vers la droite
                        moved = DeplaceTuile(1, 0);
                        break;
                }
                // Si une tuile a été déplacée, ajoute une nouvelle tuile aléatoire et affiche le plateau mis à jour
                if (moved)
                {
                    RandomTuile();
                }
               
            }
        }
        private static void DebutJeu()
        {
            // Crée le board de départ. 
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    board[i, j] = 0;
                }
            }

            // Boucle pour appeler deux fois la fonction RandomTuile()
            for (int i = 0; i < 2; i++)
            {
                RandomTuile();
            }
        }
        private static void RandomTuile()
        {
            // Tableau pour stocker les indices des cellules vides dans la grille
            int[] caseVide = new int[16]; // La grille est de taille 4x4, donc il y a au maximum 16 cellules vides
            int count = 0;

            for (int i = 0; i < 4; i++)  //Parcours de la grille : On utilise deux boucles for pour parcourir toutes les cellules de la grille (4x4).
            {
                for (int j = 0; j < 4; j++)
                {
                    if (board[i, j] == 0) // Si la cellule est vide (contient la valeur 0)
                    {
                        // Ajoute l'indice de la cellule (i * 4 + j) au tableau des cellules vides
                        caseVide[count++] = i * 4 + j;
                    }
                }
            }

            // Vérifier s'il y a des cellules vides disponibles
            if (count > 0)
            {
                // Générer un index aléatoire parmi les cellules vides
                int randomIndex = new Random().Next(count);

                // Récupérer l'indice de la cellule sélectionnée aléatoirement
                int cell = caseVide[randomIndex];

                // Calculer les coordonnées de la cellule dans la grille
                int row = cell / 4;
                int col = cell % 4;

                // Ajouter une nouvelle tuile (2 ou 4) à la cellule sélectionnée
                board[row, col] = (new Random().Next(2) + 1) * 2; // Soit 2, soit 4
            }
        }
        private static bool DeplaceTuile(int dh, int dv)
        {
            bool moved = false;

            // Réinitialiser le tableau de fusion à chaque mouvement
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    fusionTuile[i, j] = false;
                }
            }

            // Déterminer la direction du mouvement
            int[] order = new int[4] { 0, 1, 2, 3 };
            if (dh == 1 || dv == 1)
            {
                order = new int[4] { 3, 2, 1, 0 }; // Inverser l'ordre pour les mouvements vers la droite ou le bas
            }

            // Parcourir la grille
            for (int x = 0; x < 4; x++)
            {
                for (int y = 0; y < 4; y++)
                {
                    int i = order[x];
                    int j = order[y];

                    if (board[i, j] == 0) continue; // Ignorer les cases vides

                    // Trouver la prochaine position
                    int nextI = i + dv;
                    int nextJ = j + dh;

                    while (nextI >= 0 && nextI < 4 && nextJ >= 0 && nextJ < 4)
                    {
                        // Vérifier si la case est occupée
                        if (board[nextI, nextJ] != 0)
                        {
                            // Vérifier si une fusion est possible
                            if (board[nextI, nextJ] == board[i, j] && !fusionTuile[nextI, nextJ] && !fusionTuile[i, j])
                            {
                                board[nextI, nextJ] *= 2;
                                score += board[nextI, nextJ];
                                board[i, j] = 0;
                                fusionTuile[nextI, nextJ] = true;
                                moved = true;
                                break;
                            }
                            else
                            {
                                // Déplacer la tuile sans fusionner
                                if (nextI - dv != i || nextJ - dh != j)
                                {
                                    board[nextI - dv, nextJ - dh] = board[i, j];
                                    if (nextI - dv != i || nextJ - dh != j) board[i, j] = 0;
                                    moved = true;
                                }
                                break;
                            }
                        }

                        nextI += dv;
                        nextJ += dh;
                    }

                    // Gérer le cas où la tuile se déplace vers une case vide en fin de grille
                    if (nextI < 0 || nextI >= 4 || nextJ < 0 || nextJ >= 4)
                    {
                        nextI -= dv;
                        nextJ -= dh;
                        if (nextI != i || nextJ != j)
                        {
                            board[nextI, nextJ] = board[i, j];
                            board[i, j] = 0;
                            moved = true;
                        }
                    }
                }
            }

            return moved;
        }
        private static bool Partiefini()
        {
            // Vérifie s'il reste des tuiles vides.
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (board[i, j] == 0)
                    {
                        return false; //La partie n'est pas finie
                    }
                }
            }

            //Vérifie s'il reste des tuiles adjacentes de valeurs identiques. 
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if ((i - 1 >= 0 && board[i, j] == board[i - 1, j]) ||
                        (i + 1 <= 3 && board[i, j] == board[i + 1, j]) ||
                        (j - 1 >= 0 && board[i, j] == board[i, j - 1]) ||
                        (j + 1 <= 3 && board[i, j] == board[i, j + 1]))
                    {
                        return false; // La partie n'est pas terminée
                    }
                }
            }

            return true; // Plus de mouvements/fusion possible. La partie est terminée. 
        }
        private static bool PartieGagne()
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (board[i, j] == 2048)
                    {
                        // L'utilisateur peut continuer à jouer ou quitter la partie
                        Console.WriteLine("Félicitations! Vous avez atteint la tuile 2048. Appuyez sur Y pour continuer ou une autre touche pour quitter.");

                        // Attend la réponse de l'utilisateur
                        ConsoleKeyInfo response = Console.ReadKey(true);
                        char answer = char.ToUpper(response.KeyChar);

                        if (answer == 'Y')
                        {
                            // L'utilisateur veut continuer à jouer, donc ne termine pas la partie ici
                            return false;
                        }
                        else
                        {
                            // L'utilisateur ne veut pas continuer à jouer, donc termine la partie
                            return true;
                        }
                    }
                }
            }

            return false; // Aucune tuile de valeur 2048 n'est présente.
        }
        private static void AffichageBoard()

        {
            // Effacer la console pour afficher la nouvelle grille
            Console.Clear();

            // Parcourir la grille et afficher chaque cellule
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    // Changer la couleur en fonction de la valeur de la cellule
                    switch (board[i, j])
                    {
                        case 0:
                            Console.ForegroundColor = ConsoleColor.Black; // Cellule vide
                            break;
                        case 2:
                            Console.ForegroundColor = ConsoleColor.Blue;
                            break;
                        case 4:
                            Console.ForegroundColor = ConsoleColor.Red;
                            break;
                        case 8:
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            break;
                        case 16:
                            Console.ForegroundColor = ConsoleColor.Green;
                            break;
                        case 32:
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            break;
                        case 64:
                            Console.ForegroundColor = ConsoleColor.DarkMagenta;
                            break;
                        case 128:
                            Console.ForegroundColor = ConsoleColor.DarkRed;
                            break;
                        case 256:
                            Console.ForegroundColor = ConsoleColor.Gray;
                            break;
                        case 512:
                            Console.ForegroundColor = ConsoleColor.DarkBlue;
                            break;
                        case 1024:
                            Console.ForegroundColor = ConsoleColor.Magenta;
                            break;
                        case 2048:
                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                            break;
                        default:
                            Console.ForegroundColor = ConsoleColor.White; // Couleur par défaut pour les autres valeurs
                            break;
                    }

                    // Afficher la valeur de la cellule suivie d'une tabulation
                    Console.Write(board[i, j] + "\t");
                }
                // Aller à la ligne après chaque ligne de la grille
                Console.WriteLine();
            }
            // Rétablir la couleur par défaut de la console
            Console.ForegroundColor = ConsoleColor.White;


        }
    }
}
