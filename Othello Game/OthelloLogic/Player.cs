using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ex05.OthelloLogic
{
    public class Player
    {
        private readonly string r_Name;
        private int m_Score = 0;
        private readonly ePlayerType r_PlayerType;
        private readonly eCoinType m_Symbol;
        private int m_GamesWon = 0;

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

        public int GamesWon
        {
            get { return m_GamesWon; }
            set { m_GamesWon = value; }
        }

        public eCoinType Symbol
        {
            get { return m_Symbol; }
        }
    }
}
