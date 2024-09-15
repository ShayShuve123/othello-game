using System;
using System.Collections.Generic;

namespace Ex02_01
{
    public class ComputerPlayer
    {
        private GameLogic m_gameLogic;

        public ComputerPlayer(GameLogic i_GameLogic)
        {
            m_gameLogic = i_GameLogic;
        }

        public (int, int) GetBestMove(eCoinType i_ComputerPlayerSymbol)
        {
            List<(int, int)> validMoves = new List<(int, int)>();
            int maxFlips = 0;
            (int, int) bestMove = (-1, -1);

            for (int row = 0; row < m_gameLogic.GetBoard().GetBoardState().GetLength(0); row++)
            {
                for (int col = 0; col < m_gameLogic.GetBoard().GetBoardState().GetLength(0); col++)
                {
                    if (m_gameLogic.IsMoveValid(row, col, i_ComputerPlayerSymbol))
                    {
                        int flipCount = CountFlips(row, col, i_ComputerPlayerSymbol);
                        if (flipCount > maxFlips)
                        {
                            maxFlips = flipCount;
                            bestMove = (row, col);
                        }
                    }
                }
            }

            return bestMove;
        }

        private int CountFlips(int i_row, int i_col, eCoinType i_ComputerPlayerSymbol)
        {
            int flipCount = 0;
            eCoinType opponentSymbol = i_ComputerPlayerSymbol == eCoinType.TypeX ? eCoinType.TypeO : eCoinType.TypeX;
            int[,] directions = { { 0, 1 }, { 0, -1 }, { 1, 0 }, { -1, 0 }, { 1, 1 }, { 1, -1 }, { -1, 1 }, { -1, -1 } };

            for (int i = 0; i < directions.GetLength(0); i++)
            {
                flipCount += CountFlipsInDirection(i_row, i_col, directions[i, 0], directions[i, 1], i_ComputerPlayerSymbol, opponentSymbol);
            }

            return flipCount;
        }

        private int CountFlipsInDirection(int i_row, int i_col, int i_rowOffset, int i_colOffset,
                                          eCoinType i_computerPlayerSymbol, eCoinType i_opponentSymbol)
        {
            int currentRow = i_row + i_rowOffset;
            int currentCol = i_col + i_colOffset;
            int flips = 0;
            bool canFlip = false;

            while (currentRow >= 0 && currentRow < m_gameLogic.GetBoard().GetBoardState().GetLength(0) &&
                   currentCol >= 0 && currentCol < m_gameLogic.GetBoard().GetBoardState().GetLength(0))
            {
                eCoinType currentSlot = m_gameLogic.GetBoard().GetBoardState()[currentRow, currentCol];

                if (currentSlot == eCoinType.Empty)
                {
                    break;
                }

                if (currentSlot == i_opponentSymbol)
                {
                    flips++;
                }
                else if (currentSlot == i_computerPlayerSymbol && flips > 0)
                {
                    canFlip = true;
                    break;
                }
                else
                {
                    break;
                }

                currentRow += i_rowOffset;
                currentCol += i_colOffset;
            }

            return canFlip ? flips : 0;
        }
    }
}
