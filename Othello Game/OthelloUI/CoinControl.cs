using System;
using System.Drawing;
using System.Windows.Forms;
using Ex05.OthelloLogic;

namespace Ex05.OthelloUI
{
    public class CoinControl : PictureBox
    {
        private readonly int r_Row;
        private readonly int r_Column;
        private eCoinType m_CoinType;

        public event EventHandler CoinClicked;

        public CoinControl(int i_Row, int i_Column, eCoinType i_CoinType)
        {
            r_Row = i_Row;
            r_Column = i_Column;
            m_CoinType = i_CoinType;

            this.Size = new Size(50, 50);
            this.SizeMode = PictureBoxSizeMode.StretchImage;
            this.BorderStyle = BorderStyle.FixedSingle;

            updateCoinImage();

            this.Click += CoinControl_Click;
        }

        public int Row => r_Row;
        public int Column => r_Column;

        private void CoinControl_Click(object sender, EventArgs e)
        {
            OnCoinClicked();
        }

        protected virtual void OnCoinClicked()
        {
            CoinClicked?.Invoke(this, EventArgs.Empty);
        }

        public void UpdateCoin(eCoinType i_NewCoinType)
        {
            m_CoinType = i_NewCoinType;
            updateCoinImage();
        }

        private void updateCoinImage()
        {
            switch (m_CoinType)
            {
                case eCoinType.TypeRed:
                    this.Image = Properties.Resources.CoinRed; 
                    break;
                case eCoinType.TypeYellow:
                    this.Image = Properties.Resources.CoinYellow;
                    break;
                default:
                    this.Image = Properties.Resources.CoinEmpty; 
                    break;
            }

        }
    }
}
