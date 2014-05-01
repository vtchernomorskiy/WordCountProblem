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


namespace WordCountClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = WordCountMVVM.Instance;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                var client = new WordCountServiceReference.WordCountClient();
                WordCountMVVM.Instance.FirstUserResponse = client.CountWordsInStatement(txtFirstUserRequest.Text, WordCountMVVM.Instance.RemovePunctuation1);
            }
            catch (Exception ex)
            {
                WordCountMVVM.Instance.FirstUserResponse = new WordCountLibrary.Interfaces.UserResponse { HasError = true, Error = ex.Message.Substring(0, 100) };
            }
            
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            try
            {
                var client = new WordCountServiceReference.WordCountClient();
                WordCountMVVM.Instance.SecondUserResponse = client.CountWordsInStatement(txtSecondUserRequest.Text, WordCountMVVM.Instance.RemovePunctuation2);
            }
            catch (Exception ex)
            {
                WordCountMVVM.Instance.SecondUserResponse = new WordCountLibrary.Interfaces.UserResponse { HasError = true, Error = ex.Message.Substring(0, 100) };
            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            var tsks = new List<Task>();
            WordCountLibrary.Interfaces.UserResponse[] resps = new WordCountLibrary.Interfaces.UserResponse[2];

            tsks.Add(Task.Factory.StartNew(new Action<object>((o) => 
            {
                var args = (object[])o;
                var statement = (string)args[0];
                var removePunctuation = (bool)args[1];
                try
                {
                    var client = new WordCountServiceReference.WordCountClient();
                    resps[1] = client.CountWordsInStatement(statement, removePunctuation);
                }
                catch (Exception ex)
                {
                    resps[1] = new WordCountLibrary.Interfaces.UserResponse { HasError = true, Error = ex.Message };
                }
            }), new object[] { txtSecondUserRequest.Text, WordCountMVVM.Instance.RemovePunctuation2 }));

            tsks.Add(Task.Factory.StartNew(new Action<object>((o) => 
            {
                var args = (object[])o;
                var statement = (string)args[0];
                var removePunctuation = (bool)args[1];
                try
                {
                    var client = new WordCountServiceReference.WordCountClient();

                    resps[0] = client.CountWordsInStatement(statement, removePunctuation);
                }
                catch (Exception ex)
                {
                    resps[0] = new WordCountLibrary.Interfaces.UserResponse { HasError = true, Error = ex.Message };
                }
            }), new object[] { txtFirstUserRequest.Text, WordCountMVVM.Instance.RemovePunctuation1 }));

            Task.WaitAll(tsks.ToArray());

            WordCountMVVM.Instance.FirstUserResponse = resps[0];
            WordCountMVVM.Instance.SecondUserResponse = resps[1];
        }
    }
}
