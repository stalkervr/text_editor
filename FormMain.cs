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
    #region Константы

    /// <summary>
    ///   Семейство шрифтов по умолчанию.
    /// </summary>
     private const string DefaultFontFamili = "Calibri";

    /// <summary>
    /// Размер шрифта по умолчанию.
    /// </summary>
    private const int DefaultFontSize = 15;

    #endregion Константы

    #region Притные поля

    /// <summary>
    /// Количество активных вкладок редактора.
    /// </summary>
    private int m_intTabCount = 0;

    /// <summary>
    ///  Шрифт установленный по умолчанию.
    /// </summary>
    private Font m_DefaultFontFamili;

    #endregion Притные поля

        public FormMain()
        {
            InitializeComponent();
        }

        #region Свойства
        ///<summary>
        ///Свойство, в котором выполняется связь с элементом управления RichTextBox, 
        ///который находится внутри вкладки, выбранной пользователем в данный момент.
        ///</summary>
        ///
        public RichTextBox GetNewDocument
        {
            get { return (RichTextBox)tabControlPrincipal.SelectedTab.Controls["Body"]; }
        }
        #endregion Свойства

        #region Методы

        #region Вкладки
        ///<summary>
        ///Метод, который создает новый документ без содержимого на новой вкладке.
        ///</summary>
        private void NewDocument()
        {
            //Создается экземпляр нового RichTextBox со следующими значениями, установленными в его свойствах.
            RichTextBox Body = new RichTextBox
            {
                Name = "Body",
                AcceptsTab = true,
                Dock = DockStyle.Fill,
                ContextMenuStrip = contextMenuStripContextDoc,
                Font = this.m_DefaultFontFamili
            };
            // Количество вкладок увеличивается ...
            this.m_intTabCount++;
            // Для нового документа создается имя.
            string NameDocument = "Новый документ - " + this.m_intTabCount;
            //Новая вкладка (TabPage) создается с именем нового документа в 
            //качестве заголовка и именем нового элемента управления.
            TabPage NewTab = new TabPage
            {
                Name = NameDocument,
                Text = NameDocument
            };
            // Новый RichTextBox добавляется внутри новой вкладки (TabPage).
            NewTab.Controls.Add(Body);
            // Новая вкладка (TabPage) добавляется внутри элемента управления вкладкой (TabControl).
            tabControlPrincipal.TabPages.Add(NewTab);
        }
        /// <summary>
        ///   Метод, удаляющий текущую выбранную вкладку.
        /// </summary>
        private void DeleteTab()
        {
            if(tabControlPrincipal.TabPages.Count != 1)
            {
                tabControlPrincipal.TabPages.Remove(tabControlPrincipal.SelectedTab);
            }
            else
            {
                tabControlPrincipal.TabPages.Remove(tabControlPrincipal.SelectedTab);
                this.m_intTabCount = 0;
                NewDocument();
            }
        }
        /// <summary>
        ///  Метод, удаляющий все активные вкладки в редакторе.
        /// </summary>
        private void RemoveAllTabs()
        {
            foreach(TabPage Page in tabControlPrincipal.TabPages)
            {
                tabControlPrincipal.TabPages.Remove(Page);
            }
            this.m_intTabCount = 0;
            NewDocument();
        }
        /// <summary>
        ///   Метод, удаляющий все активные вкладки, кроме текущей вкладки, выбранной пользователем.
        /// </summary>
        private void RemoveAllActiveTabUnselected()
        {
            for (int i = tabControlPrincipal.TabCount - 1; i > tabControlPrincipal.SelectedIndex; i--)
            {
                tabControlPrincipal.TabPages.RemoveAt(i);
            }
        }
        /// <summary>
        ///   Метод, удаляющий все вкладки, где их содержимое уже было сохранено пользователем..
        /// </summary>
        private void RemoveAllSavedTab()
        {
            // todo
        }
        

        #endregion Вкладки

        #endregion Методы
    }
}                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                     