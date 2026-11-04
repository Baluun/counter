using System.Collections.ObjectModel;
using Projekt.Models;

namespace Projekt.Views
{
    public partial class MainPage : ContentPage
    {
        private ObservableCollection<CounterData> counters = new();
        private string saveFile = Path.Combine(FileSystem.AppDataDirectory, "counters.txt");
        private PageTexts texts = new();

        public MainPage()
        {
            InitializeComponent();
            BindingContext = texts;
            CountersView.ItemsSource = counters;
            LoadCounters();
        }

        private void LoadCounters()
        {
            try
            {
                if(File.Exists(saveFile))
                {
                    string[] lines = File.ReadAllLines(saveFile);
                    foreach(string line in lines)
                    {
                        int position = line.LastIndexOf(';');
                        if(position == -1)
                        {
                            continue;
                        }
                        string name = line.Substring(0, position);
                        int.TryParse(line.Substring(position + 1), out int value);
                        counters.Add(new CounterData { Name = name, Value = value });
                    }
                }
            }
            catch
            {
                StatusLabel.Text = "Could not load counters.";
            }

            if(counters.Count == 0)
            {
                counters.Add(new CounterData { Name = "Main Counter", Value = 0 });
                SaveCounters();
            }
        }

        private void SaveCounters()
        {
            try
            {
                List<string> lines = new List<string>();
                foreach(CounterData counter in counters)
                {
                    lines.Add(counter.Name + ";" + counter.Value);
                }
                File.WriteAllLines(saveFile, lines);
                StatusLabel.Text = "Saved.";
            }
            catch
            {
                StatusLabel.Text = "Could not save.";
            }
        }

        private void OnSaveClicked(object? sender, EventArgs e)
        {
            SaveCounters();
        }

        private void OnAddCounterClicked(object? sender, EventArgs e)
        {
            string name = NameEntry.Text?.Trim() ?? "";
            if(name == "")
            {
                name = "Counter";
            }
            int.TryParse(StartValueEntry.Text, out int startValue);
            counters.Add(new CounterData { Name = name, Value = startValue });
            NameEntry.Text = "";
            StartValueEntry.Text = "";
            SaveCounters();
        }

        private void OnPlusClicked(object? sender, EventArgs e)
        {
            if(sender is Button button && button.BindingContext is CounterData counter)
            {
                counter.Value++;
                SaveCounters();
            }
        }

        private void OnMinusClicked(object? sender, EventArgs e)
        {
            if(sender is Button button && button.BindingContext is CounterData counter)
            {
                counter.Value--;
                SaveCounters();
            }
        }
    }
}
