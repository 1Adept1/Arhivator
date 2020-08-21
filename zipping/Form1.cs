using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Ionic.Zip;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace zipping
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

        }
   
        private const string FILE = "selectedDirectory.txt";

        FolderBrowserDialog bd = new FolderBrowserDialog();
        private void button1_Click(object sender, EventArgs e)
        {           
            if (bd.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = bd.SelectedPath;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Zip files (*.zip)|*.zip";
            if (textBox1.Text!="" && sfd.ShowDialog() == DialogResult.OK)
            {
                ZipFile zf = new ZipFile(sfd.FileName);
                zf.AddDirectory(bd.SelectedPath);
                zf.Save();
                MessageBox.Show("Архивация прошла успешно.", "Выполнено");
            }           
        }

        private void Delete_Click(object sender, EventArgs e)
        {
            string directoryPath = textBox1.Text; // заносим в переменную выбранный каталог
            if (directoryPath != string.Empty) //проверяем что путь до каталога не пустой
            {
                if (DialogResult.Yes == MessageBox.Show(string.Format("Подтвердите очистку каталога {0}", directoryPath),
                    "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question)) //подтверждение об очистке.
                {
                    DeleteAllFilesOnDirectory(directoryPath); // выполняем очистку
                    MessageBox.Show("Операция завершена", "Результат", MessageBoxButtons.OK, MessageBoxIcon.Information); // сообщаем о завершении очистки
                }
            }
        }
        private void ErrorBox(Exception e)
        {
            string exception = string.Format("{0}\n{1}", e.Message, e.StackTrace);
            MessageBox.Show(exception, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void Main(string[] args)
        {
            string directoryPath = textBox1.Text;
            string _del = comboBox1.Text;
            string[] _files = Directory.GetFiles(directoryPath, _del);
            foreach (string fl in _files)
            {
                File.Delete(fl);         
            }
        }

        private void DeleteAllFilesOnDirectory(string directoryPath)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(directoryPath); //загружаем в экземпляр класса DirectroyInfo информацию о всех файлах
            foreach (DirectoryInfo directory in directoryInfo.GetDirectories()) //получаем список всех подкаталогов в выбранном каталоге
            {
                try
                {
                    directory.Delete(true); // пытаемся удалить каталог
                }
                catch (Exception e)
                {
                    ErrorBox(e); // если не удалось, выводим сообщение об ошибке
                }
            }
            foreach (FileInfo file in directoryInfo.GetFiles()) //получаем список всех файлов в выбранном каталоге
            {
                try
                {
                    file.Delete(); //пытаемся удалить файл
                }
                catch (Exception e)
                {
                    ErrorBox(e); // если не удалось, выводим сообщение об ошибке
                }
            }
        }

        
    }
}
