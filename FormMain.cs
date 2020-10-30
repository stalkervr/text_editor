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
        /// <summary>
        /// Обработка события загрузки формы
        /// </summary>
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
        /// Свойство, в котором выполняется связь с элементом управления RichTextBox, 
        /// который находится внутри вкладки, выбранной пользователем в данный момент.
        ///</summary>
        ///
        public RichTextBox ActiveDocument
        {
            get { return (RichTextBox)tabControlPrincipal.SelectedTab.Controls["Cuerpo"]; }
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
                Name = "Cuerpo",
                AcceptsTab = true,
                Dock = DockStyle.Fill,
                ContextMenuStrip = contextMenuStripContextDoc,
                Font = this.m_DefaultFontFamili
            };
            // смещение границ начала и конца строки в окне
            Body.SelectionIndent = 56;
            Body.SelectionRightIndent = 56; 
            
            
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
            // TODO: Дописать позже
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
                        ActiveDocument.LoadFile(openFileDialog_Document.FileName, RichTextBoxStreamType.RichText);
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
                        ActiveDocument.SaveFile(saveFileDialog_Document.FileName, RichTextBoxStreamType.RichText);
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
                        ActiveDocument.SaveFile(saveFileDialog_Document.FileName, RichTextBoxStreamType.RichText);
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
            printDocumentPrincipal.DocumentName = tabControlPrincipal.SelectedTab.Name;

            printDialogPrincipal.Document = printDocumentPrincipal;
            printDialogPrincipal.AllowSelection = true;
            printDialogPrincipal.AllowSomePages = true;

            if (printDialogPrincipal.ShowDialog() == DialogResult.OK)
                printDocumentPrincipal.Print();
        }

        /// <summary>
        /// Метод, открывающий диалоговое окно с предварительным просмотром того, как будет выглядеть документ при печати.
        /// </summary>
        private void PrintPreviwDocument()
        {
            printDocumentPrincipal.DocumentName = tabControlPrincipal.SelectedTab.Name;

            printPreviewDialogPrincipal.Document = printDocumentPrincipal;

            printPreviewDialogPrincipal.ShowDialog();
        }

        #endregion Печать

        #region Обработка текста

        ///<summary>
        /// Метод, отменяющий последнее действие, сделанное пользователем.
        ///</summary>
        private void UndoLastChange()
        {
            ActiveDocument.Undo();
        }

        ///<summary>
        /// Метод, повторяющий последнее действие, сделанное пользователем.
        /// </summary>
        private void RedoLastChange()
        {
            ActiveDocument.Redo();
        }

        ///<summary>
        /// Метод, который вырезает выделение из документа и помещает его в буфер обмена.
        /// </summary>
        private void CutText()
        {
            ActiveDocument.Cut();
        }

        ///<summary>
        /// Метод, который копирует выделение с холста и помещает его в буфер обмена.
        /// </summary>
        private void CopySelectedText()
        {
            ActiveDocument.Copy();
        }

        ///<summary>
        /// Метод, вставляющий содержимое буфера обмена.
        /// </summary>
        private void PasteFromBuf()
        {
            ActiveDocument.Paste();
        }

        ///<summary>
        /// Метод, который выделяет весь текст в текущем документе.
        /// </summary>
        private void SelectAllText()
        {
            ActiveDocument.SelectAll();
        }

        private void InsertImage()
        {
            OpenFileDialog _dialog = new OpenFileDialog();
            _dialog.Filter = "All Image Files|*.bmp;*.ico;*.gif;*.jpeg;*.jpg;*.png;*.tif;*.tiff";

            if(_dialog.ShowDialog() == DialogResult.OK)
            {
                if(_dialog.FileName.Length > 0)
                {
                    try
                    {
                        string lstrFile = _dialog.FileName;
                        Bitmap myBitmap = new Bitmap(lstrFile);
                        // Copy the bitmap to the clipboard.
                        Clipboard.SetDataObject(myBitmap);
                        // Get the format for the object type.
                        DataFormats.Format myFormat = DataFormats.GetFormat(DataFormats.Bitmap);
                        if (ActiveDocument.CanPaste(myFormat))
                        {
                            ActiveDocument.Paste(myFormat);
                        }
                        else
                        {
                            MessageBox.Show("The data format that you attempted site" +
                              " is not supportedby this control.");
                        }
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.Message);
                    }
                }
            }

           
        }

        #endregion Обработка текста

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

        #region Обработка событий menuStrip_Main

        /// <summary>
        /// Обработка нажатия на пункт меню "Создать новый"
        /// </summary>
        private void toolStripMenuItem_NewCreate_Click(object sender, EventArgs e)
        {
            CreateNewTab();
        }

        /// <summary>
        /// Обработка нажатия на пункт меню "Открыть"
        /// </summary>
        private void toolStripMenuItem_OpenFile_Click(object sender, EventArgs e)
        {
            OpenDocument();
        }

        /// <summary>
        /// Обработка нажатия на пункт меню "Сохранить"
        /// </summary>
        private void toolStripMenuItem_SaveFile_Click(object sender, EventArgs e)
        {
            SaveDocument();
        }
        /// <summary>
        /// Обработка нажатия на пункт меню "Сохранить как"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripMenuItem_SaveAs_Click(object sender, EventArgs e)
        {
            SaveAsDocument();
        }
        /// <summary>
        /// Обработка нажатия на пункт меню "Печатать"
        /// </summary>
        private void toolStripMenuItem_Print_Click(object sender, EventArgs e)
        {
            PrintDocument();
        }
        /// <summary>
        /// Обработка нажатия на пункт меню "Просмотр печати"
        /// </summary>
        private void toolStripMenuItem_PrintPreview_Click(object sender, EventArgs e)
        {
            PrintPreviwDocument();
        }
        /// <summary>
        /// Обработка нажатия на пункт меню "Выход"
        /// </summary>
        private void toolStripMenuItem_Exit_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
        }
        #endregion Обработка событий menuStrip_Main

        #region Обработка событий панели интрументов toolStrip_Top

        /// <summary>
        /// Обработка нажатия кнопки "B"
        /// </summary>
        private void toolStripButton_BoldFont_Click(object sender, EventArgs e)
        {
            Font FontRegular = new Font(ActiveDocument.SelectionFont.FontFamily,
                ActiveDocument.SelectionFont.SizeInPoints, FontStyle.Regular);

            Font FontBold = new Font(ActiveDocument.SelectionFont.FontFamily,
                ActiveDocument.SelectionFont.SizeInPoints, FontStyle.Bold);

            if (ActiveDocument.SelectionFont.Bold)
            {
                ActiveDocument.SelectionFont = FontRegular;
                toolStripButton_BoldFont.BackColor = BackColor;
            }
            else
            {
                ActiveDocument.SelectionFont = FontBold;
                toolStripButton_BoldFont.BackColor = Color.LightGray;
            }
        }

        /// <summary>
        /// Обработка нажатия кнопки "I" курсивное начертание
        /// </summary>
        private void toolStripButton_CursivFont_Click(object sender, EventArgs e)
        {
            Font FontRegular = new Font(ActiveDocument.SelectionFont.FontFamily,
                ActiveDocument.SelectionFont.SizeInPoints, FontStyle.Regular);

            Font FontItalica = new Font(ActiveDocument.SelectionFont.FontFamily,
                ActiveDocument.SelectionFont.SizeInPoints, FontStyle.Italic);

            if (ActiveDocument.SelectionFont.Italic)
            {
                ActiveDocument.SelectionFont = FontRegular;
                toolStripButton_CursivFont.BackColor = BackColor;
            }
            else
            {
                ActiveDocument.SelectionFont = FontItalica;
                toolStripButton_CursivFont.BackColor = Color.LightGray;
            }
        }

        /// <summary>
        /// Обработка нажатия кнопки подчёркивания текста
        /// </summary>
        private void toolStripButton_UnderlineFont_Click(object sender, EventArgs e)
        {
            Font FontRegular = new Font(ActiveDocument.SelectionFont.FontFamily,
                ActiveDocument.SelectionFont.SizeInPoints, FontStyle.Regular);

            Font FontUnderline = new Font(ActiveDocument.SelectionFont.FontFamily,
                ActiveDocument.SelectionFont.SizeInPoints, FontStyle.Underline);

            if (ActiveDocument.SelectionFont.Underline)
            {
                ActiveDocument.SelectionFont = FontRegular;
                toolStripButton_UnderlineFont.BackColor = BackColor;
            }
            else
            {
                ActiveDocument.SelectionFont = FontUnderline;
                toolStripButton_UnderlineFont.BackColor = Color.LightGray;
            }
        }
        /// <summary>
        /// Обработка нажатия кнопки пперечёркивания текста
        /// </summary>
        private void toolStripButton_CrostFont_Click(object sender, EventArgs e)
        {
            Font FuenteRegular = new Font(ActiveDocument.SelectionFont.FontFamily,
                ActiveDocument.SelectionFont.SizeInPoints, FontStyle.Regular);

            Font FuenteTachado = new Font(ActiveDocument.SelectionFont.FontFamily,
                ActiveDocument.SelectionFont.SizeInPoints, FontStyle.Strikeout);

            if (ActiveDocument.SelectionFont.Strikeout)
            {
                ActiveDocument.SelectionFont = FuenteRegular;
                toolStripButton_CrostFont.BackColor = BackColor;
            }
            else
            {
                ActiveDocument.SelectionFont = FuenteTachado;
                toolStripButton_CrostFont.BackColor = Color.LightGray;
            }
        }

        /// <summary>
        /// Обработка нажатия кнопки выравнивания текста по левому краю
        /// </summary>
        private void toolStripButton_AlignLeft_Click(object sender, EventArgs e)
        {
            ActiveDocument.SelectionAlignment = HorizontalAlignment.Left;
        }

        /// <summary>
        /// Обработка нажатия кнопки выравнивания текста по центру
        /// </summary>
        private void toolStripButton_AlignCenter_Click(object sender, EventArgs e)
        {
            ActiveDocument.SelectionAlignment = HorizontalAlignment.Center;
        }

        /// <summary>
        /// Обработка нажатия кнопки выравнивания текста по правому краю
        /// </summary>
        private void toolStripButton_AlignRight_Click(object sender, EventArgs e)
        {
            ActiveDocument.SelectionAlignment = HorizontalAlignment.Right;
        }

        /// <summary>
        /// Обработка нажатия кнопки выравнивания текста по ширине страницы
        /// </summary>
        private void toolStripButton_AlignJustify_Click(object sender, EventArgs e)
        {
            ActiveDocument.SelectionAlignment = HorizontalAlignment.Center;
        }

        /// <summary>
        /// Обработка нажатия кнопки смещения выделенного текста влево
        /// </summary>
        private void toolStripButton_TextIndent_Click(object sender, EventArgs e)
        {
            if (ActiveDocument.SelectionIndent != 0)
            {
                ActiveDocument.SelectionIndent -= 25;
            }
        }

        /// <summary>
        /// Обработка нажатия кнопки смещения выделенного текста вправо
        /// </summary>
        private void toolStripButton_TextOutdent_Click(object sender, EventArgs e)
        {
            ActiveDocument.SelectionIndent += 25;
        }

        /// <summary>
        /// Обработка нажатия кнопки список с точками
        /// </summary>
        private void toolStripButton_BulletedList_Click(object sender, EventArgs e)
        {
            ActiveDocument.SelectionBullet = !ActiveDocument.SelectionBullet;
        }

        /// <summary>
        /// Обработка нажатия кнопки нумерованный список
        /// </summary>
        private void toolStripButton_NumberedList_Click(object sender, EventArgs e)
        {
            //TODO: Нужно найти решение
            ActiveDocument.SelectionBullet = !ActiveDocument.SelectionBullet;
        }

        /// <summary>
        /// Обработка нажатия кнопки перевода в верхний регистр
        /// </summary>
        private void toolStripButton_TextToUpper_Click(object sender, EventArgs e)
        {
            ActiveDocument.SelectedText = ActiveDocument.SelectedText.ToUpper();
        }

        /// <summary>
        /// Обработка нажатия кнопки перевода в нижний регистр
        /// </summary>
        private void toolStripButton_TextToLower_Click(object sender, EventArgs e)
        {
            ActiveDocument.SelectedText = ActiveDocument.SelectedText.ToLower();
        }

        /// <summary>
        /// Обработка нажатия кнопки увеличения размера текста
        /// </summary>
        private void toolStripButton_FontUp_Click(object sender, EventArgs e)
        {
            float NewFontSize = ActiveDocument.SelectionFont.SizeInPoints + 2;

            Font NewFont = new Font(ActiveDocument.SelectionFont.Name,
                NewFontSize, ActiveDocument.SelectionFont.Style);

            this.m_DefaultFontFamili = NewFont;
            ActiveDocument.SelectionFont = NewFont;
        }

        /// <summary>
        /// Обработка нажатия кнопки уменьшения размера текста
        /// </summary>
        private void toolStripButton_FontDown_Click(object sender, EventArgs e)
        {
            float NewFontSize = ActiveDocument.SelectionFont.SizeInPoints - 2;

            Font NewFont = new Font(ActiveDocument.SelectionFont.Name,
                NewFontSize, ActiveDocument.SelectionFont.Style);

            this.m_DefaultFontFamili = NewFont;
            ActiveDocument.SelectionFont = NewFont;
        }
        /// <summary>
        /// Обработка нажатия кнопки выбора цвета текста
        /// </summary>
        private void toolStripButton_SetFontColor_Click(object sender, EventArgs e)
        {
            if (colorDialog_FontColor.ShowDialog() == DialogResult.OK)
            {
                ActiveDocument.SelectionColor = colorDialog_FontColor.Color;
            }
        }
        /// <summary>
        /// Обработка нажатия кнопки выделения текста маркером
        /// </summary>
        private void toolStripMenuItem_Yellow_Click(object sender, EventArgs e)
        {
            ActiveDocument.SelectionBackColor = Color.Yellow;
            toolStripSplitButton_MarkText.Image = Properties.Resources.yellow;
        }
        private void toolStripMenuItem_Cyan_Click(object sender, EventArgs e)
        {
            ActiveDocument.SelectionBackColor = Color.Cyan;
            toolStripSplitButton_MarkText.Image = Properties.Resources.cyan;
        }
        private void toolStripSplitButton_MarkText_ButtonClick(object sender, EventArgs e)
        {
            ActiveDocument.SelectionBackColor = Color.White;
            toolStripSplitButton_MarkText.Image = Properties.Resources.black;
        }

        /// <summary>
        /// Обработка нажатия кнопки выбора шрифта
        /// </summary>
        private void toolStripComboBox_FontFamiliSet_SelectedIndexChanged(object sender, EventArgs e)
        {
            Font NewFont = new Font(toolStripComboBox_FontFamiliSet.SelectedItem.ToString(),
                ActiveDocument.SelectionFont.Size,
                ActiveDocument.SelectionFont.Style);

            this.m_DefaultFontFamili = NewFont;
            ActiveDocument.SelectionFont = NewFont;
        }

        /// <summary>
        /// Обработка нажатия кнопки выбора размера шрифта
        /// </summary>
        private void toolStripComboBox_FontSizeSet_SelectedIndexChanged(object sender, EventArgs e)
        {
            float NewSize;

            float.TryParse(toolStripComboBox_FontSizeSet.SelectedItem.ToString(), out NewSize);

            Font NewFont = new Font(ActiveDocument.SelectionFont.Name, NewSize,
                ActiveDocument.SelectionFont.Style);

            this.m_DefaultFontFamili = NewFont;
            ActiveDocument.SelectionFont = NewFont;
        }

        #endregion Обработка событий панели интрументов toolStrip_Top


        #region PrintDocumentPrincipal Eventos

        /// <summary>
        ///   Событие, отвечающее за рисование в <see cref = "PrintDocument" /> содержимого, которое он содержит
        ///   в выбранном документе для печати..
        /// </summary>
        private void printDocumentPrincipal_PrintPage(object sender, PrintPageEventArgs e)
        {
            e.Graphics.DrawString(ActiveDocument.Text, ActiveDocument.Font, Brushes.Black, 100, 20);
            e.Graphics.PageUnit = GraphicsUnit.Inch;
        }



        #endregion PrintDocumentPrincipal Eventos

        private void toolStripButton_InsertImage_Click(object sender, EventArgs e)
        {
            InsertImage();
        }
    }
}                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                     