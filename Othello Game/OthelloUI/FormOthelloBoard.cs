using System;
using System.Drawing;
using System.Windows.Forms;
using Ex05.OthelloLogic;
using Ex05.OthelloUI;

namespace Ex05.OthelloUI
{
    public partial class FormOthelloBoard : Form
    {
        private readonly int r_BoardSize;
        private readonly bool r_PlayAgainstFriend;
        private CoinControl[,] m_CoinControls;
        private GameLogic m_GameLogic;

        public FormOthelloBoard(int i_BoardSize, bool i_PlayAgainstFriend)
        {
            r_BoardSize = i_BoardSize;
            r_PlayAgainstFriend = i_PlayAgainstFriend;
            m_GameLogic = new GameLogic(new OthelloBoard(r_BoardSize), r_PlayAgainstFriend);
            m_GameLogic.GameOver += GameLogic_GameOver;
            this.ShowIcon = false;
            
            InitializeComponent();
            InitializeBoard();
        }

        private void GameLogic_GameOver(object sender, EventArgs e)
        {
            updateBoard(); 
            showGameOverMessage();
        }

        private void InitializeBoard()
        {
            m_CoinControls = new CoinControl[r_BoardSize, r_BoardSize];
            int coinSize = 50;
            int spacing = 3;
            int totalSize = coinSize + spacing;

            this.ClientSize = new Size(r_BoardSize * totalSize + spacing, r_BoardSize * totalSize + spacing);
            this.StartPosition = FormStartPosition.CenterScreen;

            for (int row = 0; row < r_BoardSize; row++)
            {
                for (int col = 0; col < r_BoardSize; col++)
                {
                    eCoinType coinType = m_GameLogic.GetBoard().GetBoardState()[row, col];

                    CoinControl coinControl = new CoinControl(row, col, coinType);
                    coinControl.Size = new Size(coinSize, coinSize);

                    int x = spacing + col * totalSize;
                    int y = spacing + row * totalSize;
                    coinControl.Location = new Point(x, y);

                    coinControl.CoinClicked += CoinControl_CoinClicked;

                    this.Controls.Add(coinControl);
                    m_CoinControls[row, col] = coinControl;
                }
            }

            updateBoard();
        }

        private void CoinControl_CoinClicked(object sender, EventArgs e)
        {
            CoinControl clickedCoin = sender as CoinControl;
            int row = clickedCoin.Row;
            int col = clickedCoin.Column;

            eCoinType currentPlayerSymbol = m_GameLogic.CurrentPlayer.Symbol;

            if (m_GameLogic.IsMoveValid(row, col, currentPlayerSymbol))
            {
                m_GameLogic.FlipDiscsAndPlaceSymbol(row, col, currentPlayerSymbol);

                updateBoard();

                if (!r_PlayAgainstFriend && m_GameLogic.CurrentPlayer.PlayerType == ePlayerType.ComputerPlayer)
                {
                    bool computerMoved = m_GameLogic.MakeComputerMove();

                    if (computerMoved)
                    {
                        updateBoard();
                    }
                    else
                    {
                        MessageBox.Show("Computer has no valid moves, Turn goes to Red.");
                    }
                }

                showTurnGoesToMessage();
            }
            else
            {
                MessageBox.Show("Invalid move!");
            }
        }

        private void updateTitle()
        {
            if (m_GameLogic.CurrentPlayer.Name == "Computer")
            {
                this.Text = $"Othello - Yellow's Turn ({m_GameLogic.CurrentPlayer.Name})";
            }
            else
            {
                this.Text = $"Othello - {m_GameLogic.CurrentPlayer.Name}'s Turn";
            }
        }

        private void updateBoard()
        {
            int countValidMoves = 0;

            eCoinType[,] boardState = m_GameLogic.GetBoard().GetBoardState();

            for (int row = 0; row < r_BoardSize; row++)
            {
                for (int col = 0; col < r_BoardSize; col++)
                {
                    m_CoinControls[row, col].UpdateCoin(boardState[row, col]);

                    if (m_GameLogic.IsMoveValid(row, col, m_GameLogic.CurrentPlayer.Symbol))
                    {
                        countValidMoves++;

                        m_CoinControls[row, col].Enabled = true;
                        m_CoinControls[row, col].BackColor = Color.LightGreen;
                        m_CoinControls[row, col].Image = null;
                    }
                    else
                    {
                        m_CoinControls[row, col].Enabled = false;
                        m_CoinControls[row, col].BackColor = Color.Transparent;
                    }
                }
            }

            updateTitle();
        }

        private void showTurnGoesToMessage()
        {
            m_GameLogic.SwitchPlayer();
            updateTitle();
            updateBoard();

            if (!m_GameLogic.HasValidMove(m_GameLogic.CurrentPlayer.Symbol))
            {
                string playerName = m_GameLogic.CurrentPlayer.Name;
                string opponentName = m_GameLogic.GetOpponentPlayer().Name;

                if (m_GameLogic.GetOpponentPlayer().Name == "Computer")
                {
                    opponentName = "Yellow";
                }

                MessageBox.Show(playerName + " has no valid moves, Turn goes to " + opponentName);

                m_GameLogic.SwitchPlayer();
                updateTitle();
                updateBoard();

                if (!m_GameLogic.HasValidMove(m_GameLogic.CurrentPlayer.Symbol))
                {
                    m_GameLogic.CheckForGameOver();
                }
            }
        }

        private void showGameOverMessage()
        {
            string gameOverMessage = m_GameLogic.GetGameOverMessage();

            DialogResult result = MessageBox.Show(gameOverMessage, "Game Over", MessageBoxButtons.YesNo);

            if (result == DialogResult.Yes)
            {
                m_GameLogic.ResetGame();
                updateBoard();
            }
            else
            {
                this.Close();
            }
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.ClientSize = new System.Drawing.Size(282, 306);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormOthelloBoard";
            this.ResumeLayout(false);
        }
    }
}


