using System;
using System.Drawing;
using System.Drawing.Printing;
using System.Drawing.Text;
using System.IO;
using System.Windows.Forms;

namespace text_editor
{
    public partial class FormMain : Form
{
    #region Constantes

    /// <summary>
    ///   Establece la familia fuente predeterminada con que iniciara el editor.
    /// </summary>
    private const string FamiliaFuentePredeterminada = "Calibri";

    /// <summary>
    ///   Establece el tamaño de la fuente predeterminada con que iniciara el editor.
    /// </summary>
    private const int TamanoFuentePredeterminada = 15;

    #endregion Constantes

        #region Miembros Privados

        /// <summary>
        ///   Miembro privado donde almacena la cantidad de pestañas activas en el editor.
        /// </summary>
        private int m_intConteoPestanas = 0;

        /// <summary>
        ///   Miembro privado donde almacena la familia fuente utilizada para su uso posterior.
        /// </summary>
        private Font m_fontFamiliaFuenteSeleccionada;

        #endregion

        public FormMain()
        {
            InitializeComponent();
        }
    }
}                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                     