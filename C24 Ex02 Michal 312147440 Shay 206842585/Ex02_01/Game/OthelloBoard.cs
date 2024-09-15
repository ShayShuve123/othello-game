using System;

namespace Ex02_01
{
    public class OthelloBoard
    {
        private eCoinType[,] m_board;
        private readonly int r_Size;
        public OthelloBoard(int i_Size = 8)
        {
            this.r_Size = i_Size;
            m_board = new eCoinType[i_Size, i_Size];
            InitializeBoard();
        }

        private void InitializeBoard()
        {
            for (int i = 0; i < m_board.GetLength(0); i++)
            {
                for (int j = 0; j < m_board.GetLength(1); j++)
                {
                    m_board[i, j] = eCoinType.Empty;
                }
            }

            int mid = m_board.GetLength(0) / 2;
            m_board[mid - 1, mid - 1] = eCoinType.TypeO;
            m_board[mid - 1, mid] = eCoinType.TypeX;
            m_board[mid, mid - 1] = eCoinType.TypeX;
            m_board[mid, mid] = eCoinType.TypeO;
        }

        public void PrintBoard()
        {
            Console.Clear();
            Console.Write("   ");
            for (char c = 'A'; c < 'A' + r_Size; c++)
            {
                Console.Write(" " + c + "  ");
            }

            Console.WriteLine();
            string separator = new string('=', r_Size * 4);
            Console.WriteLine("  " + separator);

            for (int i = 0; i < m_board.GetLength(0); i++)
            {
                Console.Write((i + 1) + " |");
                for (int j = 0; j < m_board.GetLength(1); j++)
                {
                    char printChar = (char)(m_board[i, j]);
                    Console.Write(" " + printChar + " |");
                }
                Console.WriteLine();
                Console.WriteLine("  " + separator);
            }
        }

        public eCoinType[,] GetBoardState()
        {
            return m_board;
        }
    }
}
