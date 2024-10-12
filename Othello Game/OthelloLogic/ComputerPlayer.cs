using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ex05.OthelloLogic
{
    public class ComputerPlayer
    {
        private GameLogic m_GameLogic;

        public ComputerPlayer(GameLogic i_GameLogic)
        {
            m_GameLogic = i_GameLogic;
        }

        public (int, int) GetBestMove(eCoinType i_ComputerPlayerSymbol)
        {
            List<(int, int)> validMoves = new List<(int, int)>();
            int maxFlips = 0;
            (int, int) bestMove = (-1, -1);

            for (int row = 0; row < m_GameLogic.GetBoard().GetBoardState().GetLength(0); row++)
            {
                for (int col = 0; col < m_GameLogic.GetBoard().GetBoardState().GetLength(0); col++)
                {
                    if (m_GameLogic.IsMoveValid(row, col, i_ComputerPlayerSymbol))
                    {
                        int flipCount = countFlips(row, col, i_ComputerPlayerSymbol);

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

        private int countFlips(int i_Row, int i_Col, eCoinType i_ComputerPlayerSymbol)
        {
            int flipCount = 0;
            eCoinType opponentSymbol = i_ComputerPlayerSymbol == eCoinType.TypeYellow ? eCoinType.TypeRed : eCoinType.TypeYellow;
            int[,] directions = { { 0, 1 }, { 0, -1 }, { 1, 0 }, { -1, 0 }, { 1, 1 }, { 1, -1 }, { -1, 1 }, { -1, -1 } };

            for (int i = 0; i < directions.GetLength(0); i++)
            {
                flipCount +=
                    countFlipsInDirection(
                        i_Row, i_Col, directions[i, 0], directions[i, 1], i_ComputerPlayerSymbol, opponentSymbol);
            }

            return flipCount;
        }

        private int countFlipsInDirection(int i_Row, int i_Col, int i_RowOffset, int i_ColOffset,
                                          eCoinType i_ComputerPlayerSymbol, eCoinType i_OpponentSymbol)
        {
            int currentRow = i_Row + i_RowOffset;
            int currentCol = i_Col + i_ColOffset;
            int flips = 0;

            while (isWithinBounds(currentRow, currentCol))
            {
                eCoinType currentSlot = m_GameLogic.GetBoard().GetBoardState()[currentRow, currentCol];

                if (currentSlot == eCoinType.Empty)
                {
                    break;
                }

                if (currentSlot == i_OpponentSymbol)
                {
                    flips++;
                }
                else if (currentSlot == i_ComputerPlayerSymbol && flips > 0)
                {
                    break;
                }
                else
                {
                    flips = 0;
                    break;
                }
                currentRow += i_RowOffset;
                currentCol += i_ColOffset;
            }

            return flips;
        }

        private bool isWithinBounds(int i_Row, int i_Col)
        {
            int boardSize = m_GameLogic.GetBoard().GetBoardState().GetLength(0);

            return i_Row >= 0 && i_Row < boardSize && i_Col >= 0 && i_Col < boardSize;
        }
    }
}
