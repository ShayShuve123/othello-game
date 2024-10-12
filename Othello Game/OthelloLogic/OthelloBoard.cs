using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ex05.OthelloLogic
{
    public class OthelloBoard
    {
        private eCoinType[,] m_Board;
        private readonly int r_Size;

        public OthelloBoard(int i_Size = 8)
        {
            this.r_Size = i_Size;
            m_Board = new eCoinType[i_Size, i_Size];
            InitializeBoard();
        }

        public int Size => r_Size;

        public void InitializeBoard()
        {
            for (int i = 0; i < m_Board.GetLength(0); i++)
            {
                for (int j = 0; j < m_Board.GetLength(1); j++)
                {
                    m_Board[i, j] = eCoinType.Empty;
                }
            }

            int mid = m_Board.GetLength(0) / 2;
            m_Board[mid - 1, mid - 1] = eCoinType.TypeRed;
            m_Board[mid - 1, mid] = eCoinType.TypeYellow;
            m_Board[mid, mid - 1] = eCoinType.TypeYellow;
            m_Board[mid, mid] = eCoinType.TypeRed;
        }

        public eCoinType[,] GetBoardState()
        {
            return m_Board;
        }
    }
}
