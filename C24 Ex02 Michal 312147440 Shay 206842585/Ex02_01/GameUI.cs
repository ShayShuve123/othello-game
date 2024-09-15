using Ex02_01;
using System;
using System.Text.RegularExpressions;

namespace Ex02_01
{
    public class GameUI
    {
        private OthelloBoard m_board;
        private GameLogic m_logic;
        private Player m_player1;
        private Player m_player2;

        public void StartGame()
        {
            createPlayers();
            int size = getBoardSize();
            m_board = new OthelloBoard(size);
            m_logic = new GameLogic(m_board);
            playGame();
        }

        private void createPlayers()
        {
            string name = getValidPlayerName();
            m_player1 = new Player(name, ePlayerType.HumanPlayer, eCoinType.TypeX);
            bool isVsComputer = askIfVsComputer();

            if (isVsComputer)
            {
                m_player2 = new Player("Computer", ePlayerType.ComputerPlayer, eCoinType.TypeO);
                Console.WriteLine("---Playing against the computer.---");
            }
            else
            {
                name = getValidPlayerName();
                m_player2 = new Player(name, ePlayerType.HumanPlayer, eCoinType.TypeO);
                Console.WriteLine("---Playing against another player.---");
            }
        }

        private string getValidPlayerName()
        {
            string name;
            bool isValidName = false;
            Regex regex = new Regex(@"^[a-zA-Z]+$");

            do
            {
                Console.WriteLine("Please enter a valid player name (letters only):");
                name = Console.ReadLine();

                if (regex.IsMatch(name))
                {
                    isValidName = true;
                }
                else
                {
                    Console.WriteLine("Invalid name. Please enter a name with letters only (a-z, A-Z).");
                }

            } while (!isValidName);

            return name;
        }

        private bool askIfVsComputer()
        {
            Console.WriteLine("Who would you like to play against?");
            Console.WriteLine("1. Play against the computer");
            Console.WriteLine("2. Play against another player");

            string choice;
            do
            {
                choice = Console.ReadLine();
                if (choice == "1")
                {
                    return true;
                }
                else if (choice == "2")
                {
                    return false;
                }
                else
                {
                    Console.WriteLine("Invalid input, please enter 1 or 2.");
                }
            } while (choice != "1" && choice != "2");

            return false;
        }

        private int getBoardSize()
        {
            Console.WriteLine("Please choose the board size:");
            Console.WriteLine("1. 6x6");
            Console.WriteLine("2. 8x8");

            string choice;
            do
            {
                choice = Console.ReadLine();

                if (choice == "1")
                {
                    return 6;
                }
                else if (choice == "2")
                {
                    return 8;
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter 1 for 6x6 or 2 for 8x8.");
                }
            } while (choice != "1" && choice != "2");

            return 8;
        }

        public void playGame()
        {
            ComputerPlayer computerPlayer = null;

            if (m_player2.PlayerType == ePlayerType.ComputerPlayer)
            {
                computerPlayer = new ComputerPlayer(m_logic);
            }

            Player currentPlayer = m_player1;
            bool isGameOver = false;

            while (!isGameOver)
            {
                m_board.PrintBoard();

                if (m_logic.HasValidMove((eCoinType)currentPlayer.Symbol))
                {
                    if (currentPlayer.PlayerType == ePlayerType.ComputerPlayer && computerPlayer != null)
                    {
                        Console.WriteLine("Computer is thinking...");
                        (int row, int col) = computerPlayer.GetBestMove(currentPlayer.Symbol);
                        m_logic.FlipDiscsAndPlaceSymbol(row, col, currentPlayer.Symbol);
                        Console.WriteLine("Computer made its move.");
                        m_board.PrintBoard();
                    }
                    else
                    {
                        Console.WriteLine($"{currentPlayer.Name}, it's your turn ({currentPlayer.Symbol}).");
                        (int row, int col) = getValidMove(currentPlayer);
                        m_logic.FlipDiscsAndPlaceSymbol(row, col, currentPlayer.Symbol);
                        Console.WriteLine($"{currentPlayer.Name}, your move has been made.");
                        Console.WriteLine("Clearing the screen...");
                        Console.Clear();
                        m_board.PrintBoard();
                        Console.WriteLine($"{currentPlayer.Name}, your move display");

                        if (m_player2.PlayerType == ePlayerType.ComputerPlayer)
                        {
                            currentPlayer = m_player2;
                            Console.WriteLine("Computer is thinking...");
                            (int rowAI, int colAI) = computerPlayer.GetBestMove(m_player2.Symbol);
                            m_logic.FlipDiscsAndPlaceSymbol(rowAI, colAI, m_player2.Symbol);
                            Console.WriteLine("Computer made its move.");
                            m_board.PrintBoard();
                        }
                    }
                }
                else
                {
                    Console.WriteLine($"{currentPlayer.Name} has no valid moves.");
                }

                currentPlayer = currentPlayer == m_player1 ? m_player2 : m_player1;

                if (!m_logic.HasValidMove(m_player1.Symbol) && !m_logic.HasValidMove(m_player2.Symbol))
                {
                    isGameOver = true;
                    calculateAndDisplayScores();
                }
            }
        }

        private (int, int) getValidMove(Player i_player)
        {
            int row = -1, col = -1;
            bool isValidMove = false;

            do
            {
                Console.WriteLine($"{i_player.Symbol}, please enter your move (row and column or press 'Q' to quit):");

                Console.Write("Row: ");
                string rowInput = Console.ReadLine();
                if (rowInput.ToUpper() == "Q")
                {
                    Console.WriteLine("Game aborted.");
                    Environment.Exit(0);
                }

                Console.Write("Column: ");
                string colInput = Console.ReadLine();

                if (int.TryParse(rowInput, out row) &&
                    row > 0 && row <= m_board.GetBoardState().GetLength(0) &&
                    colInput.Length == 1 && Char.ToUpper(colInput[0]) >= 'A' && Char.ToUpper(colInput[0]) < 'A' + m_board.GetBoardState().GetLength(1))
                {
                    col = Char.ToUpper(colInput[0]) - 'A';

                    if (m_logic.IsMoveValid(row - 1, col, (eCoinType)i_player.Symbol))
                    {
                        isValidMove = true;
                    }
                    else
                    {
                        Console.WriteLine("Invalid move, try again.");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input, try again.");
                }

            } while (!isValidMove);

            return (row - 1, col);
        }

        private void calculateAndDisplayScores()
        {
            for (int row = 0; row < m_board.GetBoardState().GetLength(0); row++)
            {
                for (int col = 0; col < m_board.GetBoardState().GetLength(1); col++)
                {
                    if (m_board.GetBoardState()[row, col] == eCoinType.TypeX) m_player1.Score++;
                    else if (m_board.GetBoardState()[row, col] == eCoinType.TypeO) m_player2.Score++;
                }
            }

            Console.WriteLine($"Final Score: X = {m_player1.Score}, O = {m_player2.Score}");
            Console.WriteLine(m_player1.Score > m_player2.Score ? "Player X wins!" : m_player2.Score > m_player1.Score ? "Player O wins!" : "It's a draw!");
        }
    }
}
