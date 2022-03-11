using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MenuWindow : Window
    {
        private BitmapImage _createBtnUsualImg, _createBtnHoverImg, _connectBtnUsualImg, _connectBtnHoverImg;
        private bool _isFirstNameBoxClick = true;
        public MenuWindow()
        {
            InitializeComponent();            
            _createBtnUsualImg = new BitmapImage(new Uri("Images/createGame.png", UriKind.Relative));
            _createBtnHoverImg = new BitmapImage(new Uri("Images/createGameHover.png", UriKind.Relative));
            _connectBtnUsualImg = new BitmapImage(new Uri("Images/connect.png", UriKind.Relative));
            _connectBtnHoverImg = new BitmapImage(new Uri("Images/connectHover.png", UriKind.Relative));
        }

        private void createGameBtn_MouseEnter(object sender, MouseEventArgs e)
        {
            createGameBtn.Source = _createBtnHoverImg;
        }

        private void NameField_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (_isFirstNameBoxClick)
            {
                NameField.Text = String.Empty;
                NameField.Foreground = Brushes.Black;
                _isFirstNameBoxClick = false;
            }
        }

        private void createGameBtn_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (checkTextInNameField())
            {
                this.Hide();
                GameWindow gameWindow = new GameWindow(true, NameField.Text);
                gameWindow.Show();                
                                    
            }  
        }

        private void connectToGameBtn_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (checkTextInNameField())
            {
                GameWindow gameWindow = new GameWindow(false, NameField.Text);
                gameWindow.Show();
                this.Hide();
            }       
        }

        private void createGameBtn_MouseLeave(object sender, MouseEventArgs e)
        {
            createGameBtn.Source = _createBtnUsualImg;
        }

  

        private void connectToGameBtn_MouseEnter(object sender, MouseEventArgs e)
        {
            connectToGameBtn.Source = _connectBtnHoverImg;
        }

        private void connectToGameBtn_MouseLeave(object sender, MouseEventArgs e)
        {
            connectToGameBtn.Source = _connectBtnUsualImg;
        }

        private bool checkTextInNameField()
        {
            if (NameField.Text.Length < 3)
            {
                MessageBox.Show("Длина имени должна составлять минимум 3 символа!");
                return false;
            }
            else if (_isFirstNameBoxClick)
            {
                MessageBox.Show("Введите, пожалуйста, своё имя!");
                return false;
            }
            else return true;
        }

      
    }
}
