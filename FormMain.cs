using System;
using System.Drawing;
using System.Drawing.Printing;
using System.Drawing.Text;
using System.IO;
//using System.Reflection;
using System.Windows.Forms;
//using PrintCtrl;
using System.Runtime.InteropServices;
using DAudio;
using TagLib;
using System.Linq;

namespace text_editor
{
    public partial class FormMain : Form
    {
        #region MyRegion



        #endregion

        Image CloseImage;

        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wp, IntPtr lp);
        private const int TCM_SETMINTABWIDTH = 0x1300 + 49;
        private void tabControl1_HandleCreated(object sender, EventArgs e)
        {
            SendMessage(this.tabControlPrincipal.Handle, TCM_SETMINTABWIDTH, IntPtr.Zero, (IntPtr)16);
        }


        #region Константы

        /// <summary>
        ///   Семейство шрифтов по умолчанию.
        /// </summary>
        private const string DefaultFontFamili = "Calibri";

        /// <summary>
        /// Размер шрифта по умолчанию.
        /// </summary>
        private const int DefaultFontSize = 15;

        private AudioPlayer Player;

        #endregion Константы

        #region Притные поля

        /// <summary>
        /// Путь к файлу.
        /// </summary
        private string FilePath = null;

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

            Player = new AudioPlayer();
            // подписываемся на событие изменения статуса
            //Player.PlayingStatusChanged += (s, e) => button_Play.Text = e == Status.Playing ? "Pause" : "Play";
            //st_Button_Play
            Player.PlayingStatusChanged += (s, e) => st_Button_Play.Text = e == Status.Playing ? "Pause" : "Play";

            Player.AudioSelected += (s, e) =>
            {
                //приравниваем максимум трекбара к продолжительности трека
                if((int)e.Duration > 0)
                {
                    trackBar_Duration.Maximum = (int)e.Duration;
                    //выводим длину трека в правый лэбел
                    label_Duration.Text = e.DurationTime.ToString(@"mm\:ss");
                }
                else
                {
                    // обработка костыльная ошибки чтения длительности файла
                    trackBar_Duration.Maximum = 1200;
                    //выводим длину трека в правый лэбел
                    label_Duration.Text = ((AudioPlayer)s).PositionTime.ToString(@"mm\:ss");
                }
                
                //выводим название трека в верхний лэбел 
                label_TrakName.Text = e.Name;
                
                listBox_Playlist.SelectedItem = e.Name;
            };

            Player.ProgressChanged += (s, e) =>
            {
                trackBar_Duration.Value = (int)e;
                label_CurentPos.Text = ((AudioPlayer)s).PositionTime.ToString(@"mm\:ss");
            };
        }
        /// <summary>
        /// Обработка события загрузки формы
        /// </summary>
        private void FormMain_Load(object sender, EventArgs e)
        {
            this.m_DefaultFontFamili = GetDefaultFont();
            tabControlPrincipal.ContextMenuStrip = contextMenuStripContextTab;

            CreateNewTab();
            GetFontCollection();
            LoadListFontSize();

            // отрисовка кнопок закрытия вкладок
            tabControlPrincipal.DrawMode = TabDrawMode.OwnerDrawFixed;
            tabControlPrincipal.DrawItem += TabControlPrincipal_DrawItem;
            CloseImage = text_editor.Properties.Resources.close_tab;
            tabControlPrincipal.Padding = new Point(10, 3);
        }

        //private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    // If the last TabPage is selected then Create a new TabPage
        //    if (tabControlPrincipal.SelectedIndex == tabControlPrincipal.TabPages.Count - 1)
        //        CreateNewTab();
        //}

        private void TabControlPrincipal_DrawItem(object sender, DrawItemEventArgs e)
        {
            try
            {
                var tabPage = this.tabControlPrincipal.TabPages[e.Index];
                var tabRect = this.tabControlPrincipal.GetTabRect(e.Index);
                tabRect.Inflate(2, -3);
                // рисуем кнопку добавления вкладки
                //if (e.Index == this.tabControlPrincipal.TabCount - 1)
                //{
                //    var addImage = new Bitmap(text_editor.Properties.Resources.add_tab);
                //    e.Graphics.DrawImage(addImage,
                //        tabRect.Left + (tabRect.Width - addImage.Width) / 2,
                //        tabRect.Top + (tabRect.Height - addImage.Height) / 2);
                //}
                // рисум кнопку закрытия для всех вкладок
                // else
                //{
                var closeImage = new Bitmap(text_editor.Properties.Resources.close_tab);
                e.Graphics.DrawImage(closeImage,
                    (tabRect.Right - closeImage.Width - 3),
                    tabRect.Top + (tabRect.Height + 1 - closeImage.Height) / 2);
                TextRenderer.DrawText(e.Graphics, tabPage.Text, tabPage.Font,
                    tabRect, tabPage.ForeColor, TextFormatFlags.Left);
                //}
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        #region Свойства
        ///<summary>
        /// Свойство, в котором выполняется связь с элементом управления RichTextBox, 
        /// который находится внутри вкладки, выбранной пользователем в данный момент.
        ///</summary>
        ///
        public RichTextBox ActiveDocument
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

            // смещение границ начала и конца строки в окне и отступа сверху
            const int dist = 24;
            Body.SetInnerMargins(dist, dist, dist, 0);
            // вставка картинки перетаскиванием
            Body.AllowDrop = true;
            Body.DragDrop += new DragEventHandler(Body_DragDrop);

            // Количество вкладок увеличивается ...
            this.m_intTabCount++;
            // Для нового документа создается имя.
            string NameDocument = "  Новый документ - " + this.m_intTabCount;
            //Новая вкладка (TabPage) создается с именем нового документа в 
            //качестве заголовка и именем нового элемента управления.
            TabPage NewTab = new TabPage
            {
                Name = NameDocument,
                Text = NameDocument,
                ContextMenuStrip = contextMenuStripContextTab
            };
            // Новый RichTextBox добавляется внутри новой вкладки (TabPage).
            NewTab.Controls.Add(Body);
            // Новая вкладка (TabPage) добавляется внутри элемента управления вкладкой (TabControl).
            tabControlPrincipal.TabPages.Add(NewTab);
            FilePath = null;
            //Console.WriteLine("Method CreateNewTab Path to file -> " + FilePath);
        }
        /// <summary>
        ///   Метод, удаляющий текущую выбранную вкладку.
        /// </summary>
        private void DeleteTab()
        {
            if (tabControlPrincipal.TabPages.Count != 1)
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
            foreach (TabPage Page in tabControlPrincipal.TabPages)
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
            foreach(TabPage Page in tabControlPrincipal.TabPages)
            {
                if(Page.Name != tabControlPrincipal.SelectedTab.Name)
                {
                    tabControlPrincipal.TabPages.Remove(Page);
                }
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
        /// Метод открыть с помощью приложения из папки
        /// </summary>
        /// <param name="path">путь к файлу</param>
        public void OpenDocumentContext(string path)
        {
            // Если было выбрано допустимое имя файла ...
            if (path != string.Empty && Path.GetExtension(path).ToLower() == ".rtf")
            {
                try // Пробуем прочитать файл
                {
                    // Создается новая вкладка.
                    CreateNewTab();
                    FilePath = path;
                    Console.WriteLine("Method OpenDocument Path to file -> " + FilePath);
                    // Новая сгенерированная вкладка ищется и выбирается.
                    tabControlPrincipal.SelectedTab = tabControlPrincipal.TabPages["  Новый документ - " + this.m_intTabCount];
                    // Загружаем содержимое файла в RichTextBox новой вкладки.
                    ActiveDocument.LoadFile(FilePath, RichTextBoxStreamType.RichText);
                    // Имя файла указывается в заголовке вкладки и ее имени.
                    string NameOpenDocument = Path.GetFileName(FilePath);
                    tabControlPrincipal.SelectedTab.Text = "  " + NameOpenDocument;
                    tabControlPrincipal.SelectedTab.Name = FilePath;
                    toolStripStatusLabel_DocPath.Text = "  " + FilePath;
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }
            else
            {
                MessageBox.Show("Попытка отрыть файл неизвестного формата !", "Ошибка типа файла !", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        /// <summary>
        /// Метод, открывающий существующий файл.
        /// </summary>
        private void OpenDocument()
        {
            openFileDialog_Document.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            openFileDialog_Document.Filter = "Формат текстовых файлов (RTF)|*.rtf";

            if (openFileDialog_Document.ShowDialog() == DialogResult.OK)
            {
                // Если было выбрано допустимое имя файла ...
                if (openFileDialog_Document.FileName.Length > 0)
                {
                    
                    try // Продуем прочитать файл
                    {
                        // Создается новая вкладка.
                        CreateNewTab();
                        FilePath = openFileDialog_Document.FileName;
                        Console.WriteLine("Method OpenDocument Path to file -> " + FilePath);
                        // Новая сгенерированная вкладка ищется и выбирается.
                        tabControlPrincipal.SelectedTab = tabControlPrincipal.TabPages["  Новый документ - " + this.m_intTabCount];
                        // Загружаем содержимое файла в RichTextBox новой вкладки.
                        ActiveDocument.LoadFile(openFileDialog_Document.FileName, RichTextBoxStreamType.RichText);
                        // Имя файла указывается в заголовке вкладки и ее имени.
                        string NameOpenDocument = Path.GetFileName(openFileDialog_Document.FileName);
                        tabControlPrincipal.SelectedTab.Text = "  " + NameOpenDocument;
                        tabControlPrincipal.SelectedTab.Name = FilePath;
                        toolStripStatusLabel_DocPath.Text = "  " + FilePath;
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
            if (string.IsNullOrEmpty(FilePath))
            {
                saveFileDialog_Document.FileName = tabControlPrincipal.SelectedTab.Name;
                saveFileDialog_Document.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                saveFileDialog_Document.Filter = "Формат текстового файла (RTF)|*.rtf";
                saveFileDialog_Document.Title = "Сохранить";
                if (saveFileDialog_Document.ShowDialog() == DialogResult.OK)
                {
                    
                    if (saveFileDialog_Document.FileName.Length > 0)
                    {
                        // Если было выбрано допустимое имя файла ...
                        FilePath = saveFileDialog_Document.FileName;
                        Console.WriteLine("Method SaveDocument (if) Path to file -> " + FilePath);
                        try
                        {
                            // Сохраняем содержимое RichTextBox в установленном пути к файлу.
                            ActiveDocument.SaveFile(saveFileDialog_Document.FileName, RichTextBoxStreamType.RichText);
                            // Имя файла указывается в заголовке вкладки и ее имени.
                            string FileName = Path.GetFileName(saveFileDialog_Document.FileName);
                            tabControlPrincipal.SelectedTab.Text = FileName;
                            tabControlPrincipal.SelectedTab.Name = FilePath;
                        }
                        catch (Exception e)
                        {
                            MessageBox.Show(e.Message);
                        }
                    }
                }
            }
            else
            {
                System.IO.File.WriteAllText(FilePath, ActiveDocument.Rtf);
                Console.WriteLine("Method SaveDocument(else) Path to file -> " + FilePath);
            }
            //FilePath = tabControlPrincipal.SelectedTab.Name;
        }

        /// <summary>
        /// Метод, сохраняющий документ с новым именем или новым форматом.
        /// </summary>
        private void SaveAsDocument()
        {
            saveFileDialog_Document.FileName = tabControlPrincipal.SelectedTab.Name;
            saveFileDialog_Document.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            saveFileDialog_Document.Filter = "Формат текстового файла (RTF)|*.rtf";
            saveFileDialog_Document.Title = "Сохранить как";

            if (saveFileDialog_Document.ShowDialog() == DialogResult.OK)
            {
                // Если было выбрано допустимое имя файла ...
                if (saveFileDialog_Document.FileName.Length > 0)
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
                    catch (Exception e)
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
                ActiveDocument.Print();
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

        #region События операции печати

        /// <summary>
        ///   Событие, отвечающее за рисование в <see cref = "PrintDocument" /> содержимого, которое он содержит
        ///   в выбранном документе для печати..
        /// </summary>
        //private void printDocumentPrincipal_PrintPage(object sender, PrintPageEventArgs e)
        //{
        //    e.Graphics.DrawString(ActiveDocument.Text, ActiveDocument.Font, Brushes.Black, 100, 20);
        //    e.Graphics.PageUnit = GraphicsUnit.Inch;
        //}

        #endregion События операции печати

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

        #endregion Обработка текста

        #region Вставка объектов
        /// <summary>
        /// Метод вставки изображения
        /// </summary>
        private void InsertImage()
        {
            OpenFileDialog _dialog = new OpenFileDialog();
            _dialog.Filter = "All Image Files|*.bmp;*.ico;*.gif;*.jpeg;*.jpg;*.png;*.tif;*.tiff";

            if (_dialog.ShowDialog() == DialogResult.OK)
            {
                if (_dialog.FileName.Length > 0)
                {
                    try
                    {
                        string lstrFile = _dialog.FileName;
                        Bitmap myBitmap = new Bitmap(lstrFile);
                        // Копируем изображение в буфер обмена .
                        Clipboard.SetDataObject(myBitmap);
                        // Определяем формат объекта вставки.
                        DataFormats.Format myFormat = DataFormats.GetFormat(DataFormats.Bitmap);
                        if (ActiveDocument.CanPaste(myFormat))
                        {
                            // Вставляем изображение из буфера
                            ActiveDocument.Paste(myFormat);
                        }
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show("Ошибка вставки изображения ! " + e.Message);
                    }
                }
            }
        }
        /// <summary>
        /// Вставка изображения перетаскиванием из папки в поле документа.
        /// Открытие документа перетаскиванием в поле вкладки.
        /// Обработка события перетаскивания в поле активного документа.
        /// </summary>
        private void Body_DragDrop(object sender, DragEventArgs e)
        {
            object fileName = e.Data.GetData("FileDrop");
            
            if(fileName != null)
            {
                var list = fileName as string[];
                // Console.WriteLine(list[0]);
                // Если в поле файла перетаскиваем файл .rtf
                if(list != null && !string.IsNullOrWhiteSpace(list[0]) && Path.GetExtension(list[0]).ToLower() == ".rtf")
                {
                    OpenDocumentContext(list[0]);
                }
                // Если в поле файла перетаскиваем файл .txt
                else if (list != null && !string.IsNullOrWhiteSpace(list[0]) && Path.GetExtension(list[0]).ToLower() == ".txt")
                {
                    // Загружка содержимого в текущую вкладку
                    ActiveDocument.Clear();
                    ActiveDocument.LoadFile(list[0], RichTextBoxStreamType.PlainText);
                }
                else
                {
                    // для вставки изображения перетаскиванием
                    string[] droppedFiles = e.Data.GetData(DataFormats.FileDrop) as string[];
                    foreach (string droppedFile in droppedFiles)
                    {
                        using (Bitmap image = new Bitmap(droppedFile))
                        {
                            Clipboard.SetDataObject(image);
                            DataFormats.Format MyFormat = DataFormats.GetFormat(DataFormats.Bitmap);
                            if (ActiveDocument.CanPaste(MyFormat))
                            {
                                ActiveDocument.Paste(MyFormat);
                            }
                        }
                    }
                }
            }
        }

        #endregion Вставка объектов

        #region Основные 
        /// <summary>
        ///   Метод получения всех семейств шрифтов, установленных в системе, 
        ///   и их установка в элементе управления toolStripComboBoxFontFamilySet.
        /// </summary>
        private void GetFontCollection()
        {
            InstalledFontCollection InstaledFonts = new InstalledFontCollection();
            foreach (FontFamily item in InstaledFonts.Families)
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
            for (int i = 0; i <= 75; i++)
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

        #region Поиск слов в тексте
        /// <summary>
        /// Метод выполняет поиск слов в тексте активного документа
        /// </summary>
        private void SearchWorld()
        {
            string[] words = toolStripTextBox_textSearch.Text.Split(',');
            foreach (string word in words)
            {
                int startIndex = 0;
                while (startIndex < ActiveDocument.TextLength)
                {
                    int wordStartIndex = ActiveDocument.Find(word, startIndex, RichTextBoxFinds.None);
                    if (wordStartIndex != -1)
                    {
                        ActiveDocument.SelectionStart = wordStartIndex;
                        ActiveDocument.SelectionLength = word.Length;
                        ActiveDocument.SelectionBackColor = Color.Yellow;
                    }
                    else
                    {
                        break;
                    }
                    startIndex += wordStartIndex + word.Length;
                }
            }
            //throw new NotImplementedException();
        }
        /// <summary>
        /// Метод выполнят очистку результатов поиска в активном документе
        /// </summary>
        private void SearchClear()
        {
            toolStripTextBox_textSearch.Text = "";
            ActiveDocument.SelectionStart = 0;
            ActiveDocument.SelectAll();
            ActiveDocument.SelectionBackColor = Color.White;
            ActiveDocument.SelectionStart = ActiveDocument.TextLength;
        }

        #endregion Поиск слов в тексте

        #endregion Методы

        #region Обработка событий

        #region Обработка событий главного меню

        #region Пунк меню "Файл"

        /// <summary>
        /// Обработка нажатия на пункт меню "Создать новый"
        /// </summary>
        private void toolStripMenuItem_NewCreate_Click(object sender, EventArgs e) => CreateNewTab();

        /// <summary>
        /// Обработка нажатия на пункт меню "Открыть"
        /// </summary>
        private void toolStripMenuItem_OpenFile_Click(object sender, EventArgs e) => OpenDocument();

        /// <summary>
        /// Обработка нажатия на пункт меню "Сохранить"
        /// </summary>
        private void toolStripMenuItem_SaveFile_Click(object sender, EventArgs e) => SaveDocument();
        /// <summary>
        /// Обработка нажатия на пункт меню "Сохранить как"
        /// </summary>
        private void toolStripMenuItem_SaveAs_Click(object sender, EventArgs e) => SaveAsDocument();
        /// <summary>
        /// Обработка нажатия на пункт меню "Печатать"
        /// </summary>
        private void toolStripMenuItem_Print_Click(object sender, EventArgs e) => PrintDocument();
        /// <summary>
        /// Обработка нажатия на пункт меню "Просмотр печати"
        /// </summary>
        private void toolStripMenuItem_PrintPreview_Click(object sender, EventArgs e) => PrintPreviwDocument();
        /// <summary>
        /// Обработка нажатия на пункт меню "Выход"
        /// </summary>
        private void toolStripMenuItem_Exit_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
        }

        #endregion Пунк меню "Файл"

        #region Пунк меню "Редактировать" и контекстное меню

        private void toolStripMenuItem_UndoLast_Click(object sender, EventArgs e) => UndoLastChange();

        private void toolStripMenuItem_RedoLast_Click(object sender, EventArgs e) => RedoLastChange();

        private void toolStripMenuItem_SelectAllText_Click(object sender, EventArgs e) => SelectAllText();

        private void toolStripMenuItem_CopySelect_Click(object sender, EventArgs e) => CopySelectedText();

        private void toolStripMenuItem_PasteInPlace_Click(object sender, EventArgs e) => PasteFromBuf();

        private void toolStripMenuItem_Undo_Click(object sender, EventArgs e) => UndoLastChange();

        private void toolStripMenuItem_Redo_Click(object sender, EventArgs e) => RedoLastChange();

        private void toolStripMenuItem_Cut_Click(object sender, EventArgs e) => CutText();

        private void toolStripMenuItem_Copy_Click(object sender, EventArgs e) => CopySelectedText();

        private void toolStripMenuItem_Paste_Click(object sender, EventArgs e) => PasteFromBuf();

        private void toolStripMenuItem_SelectAll_Click(object sender, EventArgs e) => SelectAllText();

        #endregion Пунк меню "Редактировать" и контекстное меню

        #region Пункты меню "Поиск" "Очистка" и поле ввода текста

        /// <summary>
        /// Обработка нажатия на пункт меню "Поиск"
        /// </summary>
        private void toolStripMenuItem_Search_Click(object sender, EventArgs e) => SearchWorld();
        /// <summary>
        /// Обработка клика в поле для ввода слов 
        /// </summary>
        private void toolStripTextBox_textSearch_Click(object sender, EventArgs e) => toolStripTextBox_textSearch.Text = "";
        /// <summary>
        /// Обработка нажатия на пункт меню "Очистить"
        /// </summary>
        private void ToolStripMenuItem_ClearSearch_Click(object sender, EventArgs e) => SearchClear();

        #endregion Пункты меню "Поиск" "Очистка" и поле ввода текста

        #region Пунк меню справка
        private void toolStripMenuItem_Help_Click(object sender, EventArgs e)
        {
            OpenDocumentContext("help.rtf");
        }

        private void toolStripMenuItem_License_Click(object sender, EventArgs e)
        {
            OpenDocumentContext("license.rtf");
        }

        private void toolStripMenuItem_About_Click(object sender, EventArgs e)
        {

        }
        #endregion Пунк меню справка

        #endregion Обработка событий главного меню

        #region Обработка событий контекстного меню клик на вкладке

        private void toolStripMenuItem_Close_Click(object sender, EventArgs e) => DeleteTab();

        private void toolStripMenuItem_CloseOthers_Click(object sender, EventArgs e) => RemoveAllActiveTabUnselected();

        private void toolStripMenuItem_CloseAll_Click(object sender, EventArgs e) => RemoveAllTabs();

        #endregion Обработка событий контекстного меню клик на вкладке

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
        private void toolStripButton_AlignLeft_Click(object sender, EventArgs e) => ActiveDocument.SelectionAlignment = HorizontalAlignment.Left;

        /// <summary>
        /// Обработка нажатия кнопки выравнивания текста по центру
        /// </summary>
        private void toolStripButton_AlignCenter_Click(object sender, EventArgs e) => ActiveDocument.SelectionAlignment = HorizontalAlignment.Center;

        /// <summary>
        /// Обработка нажатия кнопки выравнивания текста по правому краю
        /// </summary>
        private void toolStripButton_AlignRight_Click(object sender, EventArgs e) => ActiveDocument.SelectionAlignment = HorizontalAlignment.Right;

        /// <summary>
        /// Обработка нажатия кнопки выравнивания текста по ширине страницы
        /// </summary>
        private void toolStripButton_AlignJustify_Click(object sender, EventArgs e) => ActiveDocument.SelectionAlignment = HorizontalAlignment.Center;

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
        private void toolStripButton_TextOutdent_Click(object sender, EventArgs e) => ActiveDocument.SelectionIndent += 25;

        /// <summary>
        /// Обработка нажатия кнопки список с точками
        /// </summary>
        private void toolStripButton_BulletedList_Click(object sender, EventArgs e) => ActiveDocument.SelectionBullet = !ActiveDocument.SelectionBullet;

        /// <summary>
        /// Обработка нажатия кнопки нумерованный список
        /// </summary>
        private void toolStripButton_NumberedList_Click(object sender, EventArgs e) =>
            //TODO: Нужно найти решение
            ActiveDocument.SelectionBullet = !ActiveDocument.SelectionBullet;

        /// <summary>
        /// Обработка нажатия кнопки перевода в верхний регистр
        /// </summary>
        private void toolStripButton_TextToUpper_Click(object sender, EventArgs e) => ActiveDocument.SelectedText = ActiveDocument.SelectedText.ToUpper();

        /// <summary>
        /// Обработка нажатия кнопки перевода в нижний регистр
        /// </summary>
        private void toolStripButton_TextToLower_Click(object sender, EventArgs e) => ActiveDocument.SelectedText = ActiveDocument.SelectedText.ToLower();

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
                DefaultFontSize, m_DefaultFontFamili.Style);

           // ActiveDocument.SelectedText.
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

        private void toolStripButton_InsertImage_Click(object sender, EventArgs e) => InsertImage();


        #endregion Обработка событий панели интрументов toolStrip_Top

        #region Обработка событий кликов на вкладках

        /// <summary>
        /// Обработка нажания кнопки закрытия вкладки
        /// </summary>
        private void tabControlPrincipal_MouseDown(object sender, MouseEventArgs e)
        {
            // Process MouseDown event only till (tabControl.TabPages.Count - 1) excluding the last TabPage
            for (var i = 0; i <= this.tabControlPrincipal.TabPages.Count - 1; i++)
            {
                var tabRect = this.tabControlPrincipal.GetTabRect(i);
                tabRect.Inflate(2, -3);

                var closeImage = new Bitmap(text_editor.Properties.Resources.close_tab);
                var imageRect = new Rectangle(
                    (tabRect.Right - closeImage.Width),
                    tabRect.Top + (tabRect.Height - closeImage.Height) / 2,
                    closeImage.Width,
                    closeImage.Height);
                //
                if (imageRect.Contains(e.Location))
                {
                    //сохраняем документ перед закрытием
                    if (string.IsNullOrEmpty(FilePath))
                    {
                        SaveDocument();
                    }
                    //удаляем вкладку
                    this.tabControlPrincipal.TabPages.RemoveAt(i);
                    //если закрыты все вкладки создаем новую пустую вкладку
                    if (this.tabControlPrincipal.TabPages.Count == 0)
                    {
                        this.m_intTabCount = 0;
                        CreateNewTab();
                    }
                    break;
                }
            }
        }
        /// <summary>
        /// Обработка события переключения по вкладкам
        /// </summary>
        private void tabControlPrincipal_Click(object sender, EventArgs e)
        {
            //Записываем в поле путь к файлу при переключении по вкладкам
            FilePath = tabControlPrincipal.SelectedTab.Name;
            //прорверяем содержится ли в переменной FilePath символ \ если нет то файл не открыт с диска и не сохранялся
            bool checkPath = FilePath.Contains(@"\");
            if (!checkPath)
            {
                FilePath = null;
            }
            if(FilePath == null)
            {
                toolStripStatusLabel_DocPath.Text = "  Не сохранённый документ !";
            }
            else
            {
                toolStripStatusLabel_DocPath.Text = "  " + FilePath;
            }
            
            Console.WriteLine("Method tabControlPrincipal_Click Path to file -> " + FilePath);
        }

        #endregion Обработка событий кликов на вкладках

        #region Обработка событий панели интрументов работа с файлами

        private void ToolStripButtonFiles_Create_Click(object sender, EventArgs e) => CreateNewTab();
        private void ToolStripButtonFiles_Open_Click(object sender, EventArgs e) => OpenDocument();
        private void ToolStripButtonFiles_Save_Click(object sender, EventArgs e) => SaveDocument();
        private void ToolStripButtonFiles_Cut_Click(object sender, EventArgs e) => CutText();
        private void ToolStripButtonFiles_Copy_Click(object sender, EventArgs e) => CopySelectedText();
        private void ToolStripButtonFiles_Paste_Click(object sender, EventArgs e) => PasteFromBuf();
        private void ToolStripButtonFiles_Print_Click(object sender, EventArgs e)
        {
            ActiveDocument.Print();
        }
        private void ToolStripButton_Help_Click(object sender, EventArgs e)
        {

        }

        #endregion Обработка событий панели интрументов работа с файлами

        #endregion Обработка событий

        #region Музыкальный плеер

        private void музыкаToolStripMenuItem_Click(object sender, EventArgs e) => panel3.Visible = !panel3.Visible;

        private void button_Add_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dialog = new OpenFileDialog() { Multiselect = true, Filter = "Audio files|*.wav;*.aac;*.mp4;*.m4a;*.mp3;" })
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    Player.LoadAudio(dialog.FileNames);
                    listBox_Playlist.Items.Clear();
                    listBox_Playlist.Items.AddRange(Player.Playlist);
                }
            }
        }

        private void listBox_Playlist_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (((ListBox)sender).SelectedItem == null) return;

            Player.SelectAudio(((ListBox)sender).SelectedIndex);
            // Читаем теги из файла в объект tfile
            var tfile = TagLib.File.Create(Player.CurrentAudio.SourceUrl);
            // Устанавливаем имя трека
            label_TrakName.Text = tfile.Tag.Title;
            // Устанавливаем название альбома
            string new_album = tfile.Tag.Album + " - " + tfile.Tag.Year;
            label_Album.Text = new_album;
            // Пишем  информацию в статус-бар
            toolStripStatusLabel_NowPlay.Text = tfile.Tag.FirstPerformer + " :: " + tfile.Tag.Album + " :: " + tfile.Tag.Year + " :: " + tfile.Tag.Title;
            
            // Устанавливаем обложку из файла
            try
            {
                byte[] bin = (byte[])(tfile.Tag.Pictures[0].Data.Data);
                pictureBox_MuzikCover.Image = Image.FromStream(new MemoryStream(bin));
            }
            catch 
            {
                Console.WriteLine("Нет обложки установлена def_cover");
                pictureBox_MuzikCover.Image = text_editor.Properties.Resources.def_cover;
            }
        }

        private void trackBar_Duration_Scroll(object sender, EventArgs e) => Player.Position = ((TrackBar)sender).Value;
        private void trackBar1_Scroll(object sender, EventArgs e) => Player.Volume = ((TrackBar)sender).Value;
        
        
       
        #endregion Музыкальный плеер

        private void toolStripMenuItem_GitHub_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(@"https://github.com/stalkervr");
        }

        private void st_Button_Prev_Click(object sender, EventArgs e)
        {
            if (listBox_Playlist.SelectedIndex < listBox_Playlist.Items.Count)
            {
                Player.SelectAudio(listBox_Playlist.SelectedIndex - 1);
            }
            else
            {
                Player.SelectAudio(0);
            }
        }

        private void st_Button_Play_Click(object sender, EventArgs e)
        {
            if (((Button)sender).Text == "Play") Player.Play();

            else if (((Button)sender).Text == "Pause") Player.Pause();
        }

        private void st_Button_next_Click(object sender, EventArgs e)
        {
            if (listBox_Playlist.SelectedIndex < listBox_Playlist.Items.Count)
            {
                Player.SelectAudio(listBox_Playlist.SelectedIndex + 1);
            }
            else
            {
                Player.SelectAudio(0);
            }
        }

        private void st_Button_Clear_Click(object sender, EventArgs e)
        {
            Player.Stop();
            label_CurentPos.Text = "0.00";
            label_Duration.Text = "0.00";
            trackBar_Duration.Value = 0;
            listBox_Playlist.Items.Clear();
            Player.ClearPlaylist();
            pictureBox_MuzikCover.Image = Properties.Resources.def_cover;
            label_TrakName.Text = "Track name";
            label_Album.Text = "Album - year";        }

        private void st_Button_Add_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dialog = new OpenFileDialog() { Multiselect = true, Filter = "Audio files|*.wav;*.aac;*.mp4;*.m4a;*.mp3;" })
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    Player.LoadAudio(dialog.FileNames);
                    listBox_Playlist.Items.Clear();
                    listBox_Playlist.Items.AddRange(Player.Playlist);
                }
            }
        }

        //private void button1_Click(object sender, EventArgs e)
        //{
        //    if (((Button)sender).Text == "Play") Player.Play();

        //    else if (((Button)sender).Text == "Pause") Player.Pause();
        //}

        private void pictureBox_Mute_Click(object sender, EventArgs e)
        {
            if (Player.Volume <= 0)
            {
                Player.Volume = trackBar_Volume.Value; ;
                pictureBox_Mute.Image = Properties.Resources.mute_off;
                pictureBox_Vol100.Image = Properties.Resources.vol_lev_cur;
            }
            else
            {
                Player.Volume = 0;
                pictureBox_Mute.Image = Properties.Resources.mute;
                pictureBox_Vol100.Image = Properties.Resources.vol_lev_cur;
            }
        }

        private void pictureBox_Vol100_Click(object sender, EventArgs e)
        {
            if (Player.Volume == 90)
            {
                Player.Volume = trackBar_Volume.Value;
                pictureBox_Vol100.Image = Properties.Resources.vol_lev_cur;
            }
            else
            {
                Player.Volume = 90;
                pictureBox_Vol100.Image = Properties.Resources.vol_lev_90;
                pictureBox_Mute.Image = Properties.Resources.mute_off;
            }     
        }
    }
}                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                         