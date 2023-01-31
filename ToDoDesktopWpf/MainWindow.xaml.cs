using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Policy;
using System.Text;
using System.Text.Json.Serialization;
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

namespace ToDoDesktopWpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string url = "http://localhost:8000/api/todo";
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            AdatokLekerdezese();
        }

        private void AdatokLekerdezese()
        {
            using (var client = new HttpClient())
            {
                var result = client.GetStringAsync(url).Result;
                List<ToDo> toDos = JsonConvert.DeserializeObject<List<ToDo>>(result);
                toDoList.Items.Clear();
                foreach (var todo in toDos)
                {
                    toDoList.Items.Add(todo);
                }
            }
        }

        private void Hozzad_Click(object sender, RoutedEventArgs e)
        {
            string title = teendo.Text;
            ToDo todo = new ToDo(title);
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                string data = JsonConvert.SerializeObject(todo);
                var content = new StringContent(data, UnicodeEncoding.UTF8, "application/json");
                var response = client.PostAsync(url, content).Result;
                if (response.StatusCode == HttpStatusCode.Created)
                {
                    AdatokLekerdezese();
                } 
                else
                {
                    MessageBox.Show(response.Content.ReadAsStringAsync().Result);
                }
            }
        }

        private void Elkeszult_Click(object sender, RoutedEventArgs e)
        {
            int selectedIndex = toDoList.SelectedIndex;
            if (selectedIndex == -1)
            {
                MessageBox.Show("Előbb válasszon ki elemet");
                return;
            }

            ToDo selected = (ToDo)toDoList.SelectedItem;
            ToDo toDo = new ToDo(selected.Id, selected.Title, !selected.Done);
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                string data = JsonConvert.SerializeObject(toDo);
                var content = new StringContent(data, UnicodeEncoding.UTF8, "application/json");
                var response = client.PutAsync(url + "/" + selected.Id, content).Result;
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    AdatokLekerdezese();
                }
                else
                {
                    MessageBox.Show(response.Content.ReadAsStringAsync().Result);
                }
            }
        }

        private void Torles_Click(object sender, RoutedEventArgs e)
        {
            int selectedIndex = toDoList.SelectedIndex;
            if (selectedIndex == -1)
            {
                MessageBox.Show("Előbb válasszon ki elemet");
                return;
            }

            ToDo selected = (ToDo)toDoList.SelectedItem;
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                var response = client.DeleteAsync(url + "/" + selected.Id).Result;
                if (response.StatusCode == HttpStatusCode.OK || 
                    response.StatusCode == HttpStatusCode.NoContent)
                {
                    AdatokLekerdezese();
                }
                else
                {
                    MessageBox.Show(response.Content.ReadAsStringAsync().Result);
                }
            }
        }
    }
}
