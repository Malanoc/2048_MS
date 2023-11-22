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
       // Déplace les tuiles dans la grille en fonction de la direction spécifiée.
        private static bool DeplaceTuile(int dh, int dv)
        {
            // dh = La direction horizontale du déplacement(-1 pour gauche, 1 pour droite).
            // dv = La direction verticale du déplacement (-1 pour haut, 1 pour bas).
            // moved : Indique si des tuiles ont été déplacées pendant l'exécution de la fonction. Si des tuiles ont été déplacées, la valeur est true, sinon elle est false.
            bool moved = false;

            //  Determine jusqu'où incrémenter le board en fonction de la direction.
            int startRow, startCol, endRow, endCol;
            // Configuration des indices de début et de fin en fonction de la direction
            if (dh == 1) // mouvement vers la droite
            {
                startRow = 0;
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

            // Parcours de la grille et déplacement des tuiles
            for (int i = startRow; i <= endRow; i++)
            {
                for (int j = startCol; j <= endCol; j++)
                {
                    if (board[i, j] != 0)
                    {
                        /* newRow, newCol (int) : Représentent les nouvelles positions calculées lors du déplacement des tuiles. 
                        Ces variables sont mises à jour dans la boucle while pour déterminer la destination finale de chaque tuile.*/
                        int newRow = i;
                        int newCol = j;
                        // Déplacement de la tuile dans la direction spécifiée
                        while (newRow + dv >= 0 && newRow + dv <= 3 && newCol + dh >= 0 && newCol + dh <= 3 &&
                               (board[newRow + dv, newCol + dh] == 0 || board[newRow + dv, newCol + dh] == board[i, j]))
                        {
                            newRow += dv;
                            newCol += dh;
                        }

                        // Si la tuile peut être déplacée, elle l'est. 
                        // Vérifie si la tuile a été déplacée à une nouvelle position dans la grille
                        if (newRow != i || newCol != j)
                        {
                            // Vérifie si la tuile à la nouvelle position peut fusionner avec la tuile d'origine
                            if (board[newRow, newCol] == board[i, j])
                            {
                                // Fusion des tuiles : la valeur de la tuile à la nouvelle position est doublée
                                board[newRow, newCol] *= 2;

                                // Met à jour le score en ajoutant la nouvelle valeur de la tuile fusionnée
                                score += board[newRow, newCol];

                                // La tuile d'origine est vidée après la fusion
                                board[i, j] = 0;
                            }
                            else
                            {
                                // Mouvement des tuiles : la tuile d'origine est déplacée vers la nouvelle position
                                board[newRow, newCol] = board[i, j];

                                // La tuile d'origine est vidée de sa position initiale
                                board[i, j] = 0;
                            }

                            // Indique que des tuiles ont été déplacées pendant cette itération
                            moved = true;
                        }
                    }
                }
            }

            return moved;
        }
        private static void AffichageBoard()
        {
            // Effacer la console pour afficher la nouvelle grille
            Console.Clear();

            // Afficher le score actuel du joueur
            Console.WriteLine("Score: " + score);

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
