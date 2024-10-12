using Ex05.OthelloLogic;
using System;
using System.Collections.Generic;

namespace Ex05.OthelloLogic
{
    public class GameLogic
    {
        private OthelloBoard m_Board;
        private Player m_Player1;
        private Player m_Player2;
        private Player m_CurrentPlayer;
        private ComputerPlayer m_ComputerPlayer;
        private Random m_Random;
        private static int s_TotalGamesPlayed = 0;

        public event EventHandler GameOver;

        protected virtual void OnGameOver()
        {
            GameOver?.Invoke(this, EventArgs.Empty);
        }

        public static int TotalGamesPlayed
        {
            get { return s_TotalGamesPlayed; }
        }

        public GameLogic(OthelloBoard i_Board, bool i_PlayAgainstFriend)
        {
            m_Board = i_Board;
            m_Random = new Random();

            m_Player1 = new Player("Red", ePlayerType.HumanPlayer, eCoinType.TypeRed);
            if (i_PlayAgainstFriend)
            {
                m_Player2 = new Player("Yellow", ePlayerType.HumanPlayer, eCoinType.TypeYellow);
            }
            else
            {
                m_Player2 = new Player("Computer", ePlayerType.ComputerPlayer, eCoinType.TypeYellow);
                m_ComputerPlayer = new ComputerPlayer(this);
            }

            m_CurrentPlayer = m_Player1;
        }

        public static void IncrementTotalGamesPlayed()
        {
            s_TotalGamesPlayed++;
        }

        public Player Player1 => m_Player1;
        public Player Player2 => m_Player2;
        public Player CurrentPlayer => m_CurrentPlayer;

        public void SwitchPlayer()
        {
            m_CurrentPlayer = m_CurrentPlayer == m_Player1 ? m_Player2 : m_Player1;
        }

        public Player GetOpponentPlayer()
        {
            return m_CurrentPlayer == m_Player1 ? m_Player2 : m_Player1;
        }

        public bool HasValidMove(eCoinType i_PlayerSymbol)  
        {
            bool hasValidMove = false;

            for (int row = 0; row < m_Board.GetBoardState().GetLength(0); row++)
            {
                for (int col = 0; col < m_Board.GetBoardState().GetLength(1); col++)
                {
                    if (IsMoveValid(row, col, i_PlayerSymbol))
                    {
                        hasValidMove = true;
                        break;
                    }
                }
                if (hasValidMove)
                {
                    break;
                }
            }

            return hasValidMove;
        }

        public bool IsGameOver()
        {
            return !HasValidMove(m_Player1.Symbol) && !HasValidMove(m_Player2.Symbol);
        }

        public void ResetGame()
        {
            m_Board.InitializeBoard();
            m_CurrentPlayer = m_Player1;
        }

        public int CalculateScore(eCoinType i_PlayerSymbol)
        {
            int score = 0;
            eCoinType[,] boardState = m_Board.GetBoardState();

            for (int row = 0; row < boardState.GetLength(0); row++)
            {
                for (int col = 0; col < boardState.GetLength(1); col++)
                {
                    if (boardState[row, col] == i_PlayerSymbol)
                    {
                        score++;
                    }
                }
            }

            return score;
        }

        public bool MakeComputerMove()
        {
            bool validMove= false;

            if (m_ComputerPlayer != null)
            {
                (int row, int col) = m_ComputerPlayer.GetBestMove(m_Player2.Symbol);

                if (row != -1 && col != -1 && IsMoveValid(row, col, m_Player2.Symbol))
                {
                    FlipDiscsAndPlaceSymbol(row, col, m_Player2.Symbol);
                    validMove = true;
                }
            }

            return validMove; 
        }

        public void CheckForGameOver()
        {
            if (IsGameOver())
            {
                OnGameOver();
            }
        }

        public OthelloBoard GetBoard()
        {
            return this.m_Board;
        }

        public bool IsMoveValid(int i_Row, int i_Col, eCoinType i_PlayerSymbol)    
        {
            if (i_Row < 0 || i_Row >= m_Board.GetBoardState().GetLength(0) ||
                i_Col < 0 || i_Col >= m_Board.GetBoardState().GetLength(1))
            {
                return false;
            }

            if (m_Board.GetBoardState()[i_Row, i_Col] != eCoinType.Empty)
            {
                return false;
            }

            int[,] directions = { { 0, 1 }, { 0, -1 }, { 1, 0 }, { -1, 0 },
                                  { 1, 1 }, { 1, -1 }, { -1, 1 }, { -1, -1 } };

            eCoinType opponentSymbol = i_PlayerSymbol == eCoinType.TypeRed ? eCoinType.TypeYellow : eCoinType.TypeRed;

            for (int i = 0; i < directions.GetLength(0); i++)
            {
                if (checkDirection(i_Row, i_Col, directions[i, 0], directions[i, 1], i_PlayerSymbol, opponentSymbol))
                {
                    return true;
                }
            }

            return false;
        }

        private bool checkDirection(int i_Row, int i_Col, int i_RowOffset, int i_ColOffset,
                                    eCoinType i_PlayerSymbol, eCoinType i_OpponentSymbol)
        {
            bool foundOpponent = false;
            int currentRow = i_Row + i_RowOffset;
            int currentCol = i_Col + i_ColOffset;

            while (withinBounds(currentRow, currentCol))
            {
                eCoinType currentSlot = m_Board.GetBoardState()[currentRow, currentCol];

                if (currentSlot == eCoinType.Empty) break;
                if (currentSlot == i_OpponentSymbol) foundOpponent = true;
                else if (currentSlot == i_PlayerSymbol && foundOpponent) return true;
                else break;

                currentRow += i_RowOffset;
                currentCol += i_ColOffset;
            }

            return false;
        }

        public void FlipDiscsAndPlaceSymbol(int i_Row, int i_Col, eCoinType i_PlayerSymbol)
        {
            int[,] directions = { { 0, 1 }, { 0, -1 }, { 1, 0 }, { -1, 0 },
                                  { 1, 1 }, { 1, -1 }, { -1, 1 }, { -1, -1 } };

            eCoinType opponentSymbol = i_PlayerSymbol == eCoinType.TypeRed ? eCoinType.TypeYellow : eCoinType.TypeRed;

            m_Board.GetBoardState()[i_Row, i_Col] = i_PlayerSymbol;

            for (int i = 0; i < directions.GetLength(0); i++)
            {
                if (checkDirection(i_Row, i_Col, directions[i, 0], directions[i, 1], i_PlayerSymbol, opponentSymbol))
                {
                    flipInDirection(i_Row, i_Col, directions[i, 0], directions[i, 1], i_PlayerSymbol);
                }
            }
        }

        private void flipInDirection(int i_Row, int i_Col, int i_RowOffset, int i_ColOffset, eCoinType i_PlayerSymbol)
        {
            int currentRow = i_Row + i_RowOffset;
            int currentCol = i_Col + i_ColOffset;
            eCoinType opponentSymbol = i_PlayerSymbol == eCoinType.TypeRed ? eCoinType.TypeYellow : eCoinType.TypeRed;
            while (withinBounds(currentRow, currentCol) &&
                   m_Board.GetBoardState()[currentRow, currentCol] == opponentSymbol)
            {
                m_Board.GetBoardState()[currentRow, currentCol] = i_PlayerSymbol;
                currentRow += i_RowOffset;
                currentCol += i_ColOffset;
            }
        }

        private bool withinBounds(int i_Row, int i_Col)
        {
            int rowCount = GetBoard().GetBoardState().GetLength(0); 
            int colCount = GetBoard().GetBoardState().GetLength(1); 
            return i_Row >= 0 && i_Row < rowCount && i_Col >= 0 && i_Col < colCount;
        }

        public string GetGameOverMessage()
        {
            int player1Score = CalculateScore(Player1.Symbol);
            int player2Score = CalculateScore(Player2.Symbol);
            string winnerMessage;
            Player winner = null;

            IncrementTotalGamesPlayed();

            if (player1Score > player2Score)
            {
                winner =Player1;
                winner.GamesWon++;
                winnerMessage = $"{winner.Name} Won!! ({player1Score}/{player2Score}) ({winner.GamesWon}/{GameLogic.TotalGamesPlayed})";
            }
            else if (player2Score > player1Score)
            {
                winner = Player2;
                winner.GamesWon++;
                winnerMessage = $"{winner.Name} Won!! ({player2Score}/{player1Score}) ({winner.GamesWon}/{GameLogic.TotalGamesPlayed})";
            }
            else
            {
                winnerMessage = "It's a Tie!";
            }

            string message = $"{winnerMessage}\nWould you like another round?";

            return message;

        }
    }
}





