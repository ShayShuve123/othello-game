using System;
using System.Windows.Forms;


namespace Ex05.OthelloUI
{
    public class FormOthelloSettings : Form
    {
        private Button m_ButtonBoardSize;
        private Button m_ButtonPlayAgainstComputer;
        private Button m_ButtonPlayAgainstFriend;
        private int m_BoardSize = 6;

        public FormOthelloSettings()
        {
            InitializeComponent();
            this.Text = "Othello - Game Settings";
            this.Size = new System.Drawing.Size(400, 200);
            this.BackColor = System.Drawing.Color.LightGray;
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void InitializeComponent()
        {
            this.m_ButtonBoardSize = new System.Windows.Forms.Button();
            this.m_ButtonPlayAgainstComputer = new System.Windows.Forms.Button();
            this.m_ButtonPlayAgainstFriend = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // m_ButtonBoardSize
            // 
            this.m_ButtonBoardSize.Location = new System.Drawing.Point(75, 20);
            this.m_ButtonBoardSize.Name = "m_ButtonBoardSize";
            this.m_ButtonBoardSize.Size = new System.Drawing.Size(250, 30);
            this.m_ButtonBoardSize.TabIndex = 0;
            this.m_ButtonBoardSize.Text = "Board Size: 6x6 (click to increase)";
            this.m_ButtonBoardSize.Click += new System.EventHandler(this.buttonBoardSize_Click);
            // 
            // m_ButtonPlayAgainstComputer
            // 
            this.m_ButtonPlayAgainstComputer.ForeColor = System.Drawing.SystemColors.ControlText;
            this.m_ButtonPlayAgainstComputer.Location = new System.Drawing.Point(40, 70);
            this.m_ButtonPlayAgainstComputer.Name = "m_ButtonPlayAgainstComputer";
            this.m_ButtonPlayAgainstComputer.Size = new System.Drawing.Size(160, 30);
            this.m_ButtonPlayAgainstComputer.TabIndex = 1;
            this.m_ButtonPlayAgainstComputer.Text = "Play against the computer";
            this.m_ButtonPlayAgainstComputer.Click += new System.EventHandler(this.buttonPlayAgainstComputer_Click);
            // 
            // m_ButtonPlayAgainstFriend
            // 
            this.m_ButtonPlayAgainstFriend.Location = new System.Drawing.Point(200, 70);
            this.m_ButtonPlayAgainstFriend.Name = "m_ButtonPlayAgainstFriend";
            this.m_ButtonPlayAgainstFriend.Size = new System.Drawing.Size(160, 30);
            this.m_ButtonPlayAgainstFriend.TabIndex = 2;
            this.m_ButtonPlayAgainstFriend.Text = "Play against your friend";
            this.m_ButtonPlayAgainstFriend.Click += new System.EventHandler(this.buttonPlayAgainstFriend_Click);
            // 
            // FormOthelloSettings
            // 
            this.ClientSize = new System.Drawing.Size(456, 261);
            this.Controls.Add(this.m_ButtonBoardSize);
            this.Controls.Add(this.m_ButtonPlayAgainstComputer);
            this.Controls.Add(this.m_ButtonPlayAgainstFriend);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormOthelloSettings";
            this.Load += new System.EventHandler(this.FormOthelloSettings_Load);
            this.ResumeLayout(false);
        }

        public int BoardSize
        {
            get { return m_BoardSize; }
            set { m_BoardSize = value; }
        }

        private void buttonBoardSize_Click(object sender, EventArgs e)
        {
            m_BoardSize += 2;
            if (m_BoardSize > 12)
            {
                m_BoardSize = 6;
            }
            m_ButtonBoardSize.Text = $"Board Size: {m_BoardSize}x{m_BoardSize} (click to increase)";

        }

        private void buttonPlayAgainstComputer_Click(object sender, EventArgs e)
        {
            startGame(false, m_BoardSize);
        }

        private void buttonPlayAgainstFriend_Click(object sender, EventArgs e)
        {
            startGame(true, m_BoardSize);
        }

        private void FormOthello_Load(object sender, EventArgs e)
        {

        }

        private void FormOthelloSettings_Load(object sender, EventArgs e)
        {

        }

        private void startGame(bool i_PlayAgainstFriend, int i_BoardSize)
        {
            FormOthelloBoard gameForm = new FormOthelloBoard(i_BoardSize, i_PlayAgainstFriend);
            this.Hide();
            gameForm.ShowDialog();
            this.Show();
        }
    }
}
