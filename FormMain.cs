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

        private void FormMain_Load(object sender, EventArgs e)
        {
            this.m_DefaultFontFamili = GetDefaultFont();
            tabControlPrincipal.ContextMenuStrip = contextMenuStripContextDoc;

            CreateNewTab();
            GetFontCollection();
            LoadListFontSize();
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
        private void CreateNewTab()
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
                CreateNewTab();
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
            CreateNewTab();
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

        #region Открыть и сохранить

        /// <summary>
        /// Метод, открывающий существующий файл.
        /// </summary>
        private void OpenDocument()
        {
            openFileDialog_Document.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            openFileDialog_Document.Filter = "Формат текстовых файлов (RTF)|*.rtf";

            if(openFileDialog_Document.ShowDialog() == DialogResult.OK)
            {
                // Если было выбрано допустимое имя файла ...
                if(openFileDialog_Document.FileName.Length > 0)
                {
                    try // Продуем прочитать файл
                    {
                        // Создается новая вкладка.
                        CreateNewTab();
                        // Новая сгенерированная вкладка ищется и выбирается.
                        tabControlPrincipal.SelectedTab = tabControlPrincipal.TabPages["Новый документ - " + this.m_intTabCount];
                        // Загружаем содержимое файла в RichTextBox новой вкладки.
                        GetNewDocument.LoadFile(openFileDialog_Document.FileName, RichTextBoxStreamType.RichText);
                        // Имя файла указывается в заголовке вкладки и ее имени.
                        string NameOpenDocument = Path.GetFileName(openFileDialog_Document.FileName);
                        tabControlPrincipal.SelectedTab.Text = NameOpenDocument;
                        tabControlPrincipal.SelectedTab.Name = NameOpenDocument;
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.Message);
                    }
                }
            }
        }

        /// <summary>
        /// Метод, сохраняющий активный документ.
        /// </summary>
        private void SaveDocument()
        {
            saveFileDialog_Document.FileName = tabControlPrincipal.SelectedTab.Name;
            saveFileDialog_Document.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            saveFileDialog_Document.Filter = "Формат текстового файла (RTF)|*.rtf";
            saveFileDialog_Document.Title = "Сохранить";

            if(saveFileDialog_Document.ShowDialog() == DialogResult.OK)
            {
                // Если было выбрано допустимое имя файла ...
                if(saveFileDialog_Document.FileName.Length > 0)
                {
                    try
                    {
                        // Сохраняем содержимое RichTextBox в установленном пути к файлу.
                        GetNewDocument.SaveFile(saveFileDialog_Document.FileName, RichTextBoxStreamType.RichText);
                        // Имя файла указывается в заголовке вкладки и ее имени.
                        string FileName = Path.GetFileName(saveFileDialog_Document.FileName);
                        tabControlPrincipal.SelectedTab.Text = FileName;
                        tabControlPrincipal.SelectedTab.Name = FileName;
                    }
                    catch(Exception e)
                    {
                        MessageBox.Show(e.Message);
                    }
                }
            }
        }

        /// <summary>
        /// Метод, сохраняющий документ с новым именем или новым форматом.
        /// </summary>
        private void SaveAsDocument()
        {
            saveFileDialog_Document.FileName = tabControlPrincipal.SelectedTab.Name;
            saveFileDialog_Document.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            saveFileDialog_Document.Filter = "Формат текстового файла (RTF)|*.rtf";
            saveFileDialog_Document.Title = "Сохранить как";

            if(saveFileDialog_Document.ShowDialog() == DialogResult.OK)
            {
                // Если было выбрано допустимое имя файла ...
                if(saveFileDialog_Document.FileName.Length > 0)
                {
                    try
                    {
                        // Сохраняем содержимое RichTextBox в установленном пути к файлу.
                        GetNewDocument.SaveFile(saveFileDialog_Document.FileName, RichTextBoxStreamType.RichText);
                        // Имя файла указывается в заголовке вкладки и ее имени.
                        string FileName = Path.GetFileName(saveFileDialog_Document.FileName);
                        tabControlPrincipal.SelectedTab.Text = FileName;
                        tabControlPrincipal.SelectedTab.Name = FileName;
                    }
                    catch(Exception e)
                    {
                        MessageBox.Show(e.Message);
                    }
                }
            }
        }

        #endregion Открыть и сохранить

        #region Печать

        /// <summary>
        /// Метод, открывающий диалоговое окно для печати текущего желаемого документа.
        /// </summary>
        private void PrintDocument()
        {
            printDocument_Main.DocumentName = tabControlPrincipal.SelectedTab.Name;

            printDialog_Main.Document = printDocument_Main;
            printDialog_Main.AllowSelection = true;
            printDialog_Main.AllowSomePages = true;

            if(printDialog_Main.ShowDialog() == DialogResult.OK)
            {
                printDocument_Main.Print();
            }
        }

        /// <summary>
        /// Метод, открывающий диалоговое окно с предварительным просмотром того, как будет выглядеть документ при печати.
        /// </summary>
        private void PrintPreviwDocument()
        {
            printDocument_Main.DocumentName = tabControlPrincipal.SelectedTab.Name;

            printPreviewDialog_Main.Document = printDocument_Main;

            printPreviewDialog_Main.ShowDialog();
        }
        #endregion Печать

        #region Основные 
        /// <summary>
        ///   Метод получения всех семейств шрифтов, установленных в системе, 
        ///   и их установка в элементе управления toolStripComboBoxFontFamilySet.
        /// </summary>
        private void GetFontCollection()
        {
            InstalledFontCollection InstaledFonts = new InstalledFontCollection();
            foreach(FontFamily item in InstaledFonts.Families)
            {
                toolStripComboBox_FontFamiliSet.Items.Add(item.Name);
            }
            toolStripComboBox_FontFamiliSet.SelectedIndex = toolStripComboBox_FontFamiliSet.FindStringExact(DefaultFontFamili);
        }
        /// <summary>
        ///   Метод установки значений, которые будут представлять размеры шрифта 
        ///   в элементе управления toolStripComboBox_FontSizeSet
        /// </summary>
        private void LoadListFontSize()
        {
            for(int i = 0; i <= 75; i++)
            {
                toolStripComboBox_FontSizeSet.Items.Add(i);
            }
            toolStripComboBox_FontSizeSet.SelectedIndex = DefaultFontSize;
        }
        /// <summary>
        /// Метод получения семейства шрифтов по умолчанию.
        /// </summary>
        private Font GetDefaultFont()
        {
            return new Font(DefaultFontFamili, DefaultFontSize, FontStyle.Regular);
        }
        #endregion Основные

        #endregion Методы

        
    }
}                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                     