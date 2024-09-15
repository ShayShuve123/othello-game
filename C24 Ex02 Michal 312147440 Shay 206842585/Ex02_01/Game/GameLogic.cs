using System;
using System.Collections.Generic;

namespace Ex02_01
{
    public class GameLogic
    {
        private OthelloBoard m_board;
        private Random m_random;

        public GameLogic(OthelloBoard i_board)
        {
            this.m_board = i_board;
            m_random = new Random();
        }

        public bool HasValidMove(eCoinType i_playerSymbol)
        {
            bool hasValidMove = false;
            for (int row = 0; row < m_board.GetBoardState().GetLength(0); row++)
            {
                for (int col = 0; col < m_board.GetBoardState().GetLength(1); col++)
                {
                    if (IsMoveValid(row, col, i_playerSymbol))
                    {
                        hasValidMove = true;
                        break;
                    }
                }
                if (hasValidMove) break;
            }
            return hasValidMove;
        }

        public OthelloBoard GetBoard()
        {
            return this.m_board;
        }

        public bool IsMoveValid(int i_row, int i_col, eCoinType i_playerSymbol)
        {
            if (i_row < 0 || i_row >= m_board.GetBoardState().GetLength(0) ||
                i_col < 0 || i_col >= m_board.GetBoardState().GetLength(1))
            {
                return false;
            }

            if (m_board.GetBoardState()[i_row, i_col] != eCoinType.Empty)
            {
                return false;
            }

            int[,] directions = { { 0, 1 }, { 0, -1 }, { 1, 0 }, { -1, 0 }, { 1, 1 }, { 1, -1 }, { -1, 1 }, { -1, -1 } };
            eCoinType opponentSymbol = i_playerSymbol == eCoinType.TypeX ? eCoinType.TypeO : eCoinType.TypeX;

            for (int i = 0; i < directions.GetLength(0); i++)
            {
                if (CheckDirection(i_row, i_col, directions[i, 0], directions[i, 1], i_playerSymbol, opponentSymbol))
                {
                    return true;
                }
            }

            return false;
        }

        private bool CheckDirection(int i_row, int i_col, int i_rowOffset, int i_colOffset, eCoinType i_playerSymbol, eCoinType i_opponentSymbol)
        {
            bool foundOpponent = false;
            int currentRow = i_row + i_rowOffset;
            int currentCol = i_col + i_colOffset;

            while (currentRow >= 0 && currentRow < m_board.GetBoardState().GetLength(0) &&
                   currentCol >= 0 && currentCol < m_board.GetBoardState().GetLength(1))
            {
                eCoinType currentSlot = m_board.GetBoardState()[currentRow, currentCol];

                if (currentSlot == eCoinType.Empty)
                {
                    break;
                }

                if (currentSlot == i_opponentSymbol)
                {
                    foundOpponent = true;
                }
                else if (currentSlot == i_playerSymbol && foundOpponent)
                {
                    return true;
                }
                else
                {
                    break;
                }

                currentRow += i_rowOffset;
                currentCol += i_colOffset;
            }

            return false;
        }

        public void FlipDiscsAndPlaceSymbol(int i_row, int i_col, eCoinType i_playerSymbol)
        {
            int[,] directions = { { 0, 1 }, { 0, -1 }, { 1, 0 }, { -1, 0 }, { 1, 1 }, { 1, -1 }, { -1, 1 }, { -1, -1 } };
            eCoinType opponentSymbol = i_playerSymbol == eCoinType.TypeX ? eCoinType.TypeO : eCoinType.TypeX;

            m_board.GetBoardState()[i_row, i_col] = i_playerSymbol;

            for (int i = 0; i < directions.GetLength(0); i++)
            {
                if (CheckDirection(i_row, i_col, directions[i, 0], directions[i, 1], i_playerSymbol, opponentSymbol))
                {
                    FlipInDirection(i_row, i_col, directions[i, 0], directions[i, 1], i_playerSymbol);
                }
            }
        }

        private void FlipInDirection(int i_row, int i_col, int i_rowOffset, int i_colOffset, eCoinType i_playerSymbol)
        {
            int currentRow = i_row + i_rowOffset;
            int currentCol = i_col + i_colOffset;
            eCoinType opponentSymbol = i_playerSymbol == eCoinType.TypeX ? eCoinType.TypeO : eCoinType.TypeX;

            while (m_board.GetBoardState()[currentRow, currentCol] == opponentSymbol)
            {
                m_board.GetBoardState()[currentRow, currentCol] = i_playerSymbol;
                currentRow += i_rowOffset;
                currentCol += i_colOffset;
            }
        }

        public (int, int) GetComputerMove(eCoinType i_playerSymbol)
        {
            List<(int, int)> validMoves = new List<(int, int)>();
            for (int row = 0; row < m_board.GetBoardState().GetLength(0); row++)
            {
                for (int col = 0; col < m_board.GetBoardState().GetLength(1); col++)
                {
                    if (IsMoveValid(row, col, i_playerSymbol))
                    {
                        validMoves.Add((row, col));
                    }
                }
            }

            (int, int) move = (-1, -1);
            if (validMoves.Count > 0)
            {
                int index = m_random.Next(validMoves.Count);
                move = validMoves[index];
            }

            return move;
        }
    }
}
