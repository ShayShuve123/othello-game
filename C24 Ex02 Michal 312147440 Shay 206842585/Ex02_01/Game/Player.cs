using System;

namespace Ex02_01
{
    public class Player
    {
        private readonly string r_Name;
        private int m_Score = 0;
        private readonly ePlayerType r_PlayerType;
        private readonly eCoinType m_Symbol;

        public Player(string i_Name, ePlayerType i_PlayerType, eCoinType i_Symbol)
        {
            r_Name = i_Name;
            r_PlayerType = i_PlayerType;
            m_Symbol = i_Symbol;
        }

        public string Name
        {
            get { return r_Name; }
        }

        public ePlayerType PlayerType
        {
            get { return r_PlayerType; }
        }

        public int Score
        {
            get { return m_Score; }
            set { m_Score = value; }
        }
        public eCoinType Symbol
        {
            get { return m_Symbol; }
        }
    }
}
