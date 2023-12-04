using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2048_MS
{
    internal class Program
    {
        private static int[,] board = new int[4, 4];
        private static int score = 0;

        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Black;
            Console.BackgroundColor = ConsoleColor.Green;

            DebutJeu(); // Initialise le jeu en plaçant deux tuiles aléatoires sur le plateau
            AffichageBoard(); // Affiche le plateau de jeu initial (appelé board dans la suite des commentaires)

            // Boucle principale du jeu qui s'exécute indéfiniment jusqu'à ce que l'utilisateur quitte
            while (true)
            {
                // Récupère la touche pressée par l'utilisateur sans afficher la touche à l'écran
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                ConsoleKey key = keyInfo.Key;

                // Quitte la boucle si l'utilisateur appuie sur la touche 'Q'
                if (key == ConsoleKey.Q)
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
                    AffichageBoard();
                }
                // Vérifie si le jeu est terminé (plus de mouvements possibles)
                if (Partiefini())
                {
                    Console.WriteLine("Game Over! Your score: " + score);
                    break;
                }
                // Vérifie si le joueur a gagné (atteint la tuile 2048)
                if (PartieGagne())
                {
                    Console.WriteLine("You win! Your score: " + score);
                    break;
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

            // Ajoute deux chiffres aléatoires au board.
            RandomTuile();
            RandomTuile();
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

            int startRow, startCol, endRow, endCol;

            if (dh == 1) // mouvement vers la droite
            {
                startRow = 0;
                startCol = 0;
                endRow = 3;
                endCol = 2;
            }
            else if (dh == -1) //mouvement vers la gauche
            {
                startRow = 0;
                startCol = 1;
                endRow = 3;
                endCol = 3;
            }
            else if (dv == 1) // mouvement vers le bas
            {
                startRow = 0;
                startCol = 0;
                endRow = 2;
                endCol = 3;
            }
            else if (dv == -1) // mouvement vers le haut
            {
                startRow = 1;
                startCol = 0;
                endRow = 3;
                endCol = 2;
            }
            else
            {
                // mouvement non valide
                return false;
            }

            // Parcours des lignes de la grille en fonction de la direction spécifiée
            for (int i = startRow; i <= endRow; i++)
            {
                // Parcours des colonnes de la grille en fonction de la direction spécifiée
                for (int j = startCol; j <= endCol; j++)
                {
                    // Vérifie si la cellule actuelle n'est pas vide
                    if (board[i, j] != 0)
                    {
                        // Initialise les nouvelles positions de la tuile aux positions actuelles
                        int newRow = i;
                        int newCol = j;

                        // Continue à déplacer la tuile dans la direction spécifiée jusqu'à atteindre une limite ou une autre tuile
                        while (newRow + dv >= 0 && newRow + dv <= 3 && newCol + dh >= 0 && newCol + dh <= 3 &&
                               (board[newRow + dv, newCol + dh] == 0 || board[newRow + dv, newCol + dh] == board[i, j]))
                        {
                            // Met à jour les nouvelles positions de la tuile
                            newRow += dv;
                            newCol += dh;
                        }

                        // Vérifie si la tuile a effectivement été déplacée vers une nouvelle position
                        if (newRow != i || newCol != j)
                        {
                            if (board[newRow, newCol] == board[i, j])
                            {
                                // combine les tuiles et incrémente le score
                                board[newRow, newCol] *= 2;
                                score += board[newRow, newCol];
                                board[i, j] = 0;
                            }
                            else
                            {
                                // déplace les tuiles qui ne peuvent pas être combinées
                                board[newRow, newCol] = board[i, j];
                                board[i, j] = 0;
                            }
                            moved = true;
                        }
                    }
                }
            }

            // Retourne true si des tuiles ont été déplacées, sinon retourne false
            return moved;
        }
<<<<<<< Updated upstream
            private static void AffichageBoard()
=======

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
                        return true; // Le joueur a gagné. Partie finie.
                    }
                }
            }

            return false; // Aucune tuile de valeur 2048 n'est présente. 
        }
        private static void AffichageBoard()
>>>>>>> Stashed changes
        {
            // Effacer la console pour afficher la nouvelle grille
            Console.Clear();

<<<<<<< Updated upstream
            
=======
            // Afficher le score actuel du joueur
            Console.WriteLine("Score: " + score);
>>>>>>> Stashed changes

            // Parcourir la grille et afficher chaque cellule
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    // Afficher la valeur de la cellule suivie d'une tabulation
                    Console.Write(board[i, j] + "\t");
                }
                // Aller à la ligne après chaque ligne de la grille
                Console.WriteLine();
            }

            // Afficher les instructions pour les déplacements et la sortie du jeu
            Console.WriteLine("Utilisez les flèches directionnelles pour déplacer les tuiles. Appuyez sur q pour quitter.");
        }
    }
}
