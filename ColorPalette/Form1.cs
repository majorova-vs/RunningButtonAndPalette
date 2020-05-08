using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ColorPalette
{
    public partial class FormPallete : Form
    {
        //Всплывающая подсказка при наведении мыши на объект
        ToolTip tooltip = new ToolTip();

        public FormPallete()
        {
            InitializeComponent();
            OnColorChanged(this, new EventArgs());
        }

        Color color;
        //Изменение цвета индикатора
        private void OnColorChanged(object sender, EventArgs e)
        {
            // Изменение цвета индикатора при каждом изменении
            byte red = (byte)sliderRed.Value;
            byte green = (byte)sliderGreen.Value;
            byte blue = (byte)sliderBlue.Value;
            color = Color.FromArgb(red, green, blue);
            pnlColorPreview.BackColor = color;
        }
        private void OnColorChangeOver(object sender, EventArgs e)
        {
            // Шестнадцатиричные значения в буфере обмена и в подсказке
            Clipboard.SetText(HexColor(color));
            tooltip.SetToolTip(pnlColorPreview, Clipboard.GetText());
        }
        //Возвращает код цвета в шестнадцатиричной системе счисления
        private string HexColor(Color c) => "#" + c.R.ToString("X2") + c.G.ToString("X2") + c.B.ToString("X2");
    }
}
