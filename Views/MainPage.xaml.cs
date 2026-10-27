using Projekt.Models;

namespace Projekt.Views
{
    public partial class MainPage : ContentPage
    {
        private List<CounterData> counters = new();
        private string saveFile = Path.Combine(FileSystem.AppDataDirectory, "counters.txt");
        public MainPage()
        {
            InitializeComponent();
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
                        int value = 0;
                        int.TryParse(line.Substring(position + 1), out value);
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
            foreach(CounterData counter in counters)
            {
                ShowCounter(counter);
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
            if (name == "")
            {
                name = "Counter";
            }
            int startValue = 0;
            int.TryParse(StartValueEntry.Text, out startValue);
            CounterData counter = new CounterData { Name = name, Value = startValue };
            counters.Add(counter);
            ShowCounter(counter);
            NameEntry.Text = "";
            StartValueEntry.Text = "";
            SaveCounters();
        }
        private void ShowCounter(CounterData counter)
        {
            Label nameLabel = new Label
            {
                Text = counter.Name,
                FontSize = 20,
                FontAttributes = FontAttributes.Bold,
                HorizontalOptions = LayoutOptions.Center,
                TextColor = Color.FromArgb("#202535")
            };

            Label valueLabel = new Label
            {
                Text = counter.Value.ToString(),
                FontSize = 48,
                FontAttributes = FontAttributes.Bold,
                HorizontalOptions = LayoutOptions.Center,
                TextColor = Color.FromArgb("#202535")
            };

            Button minusButton = new Button
            {
                Text = "−",
                FontSize = 28,
                BackgroundColor = Color.FromArgb("#E75B5B"),
                TextColor = Colors.White,
                CornerRadius = 12
            };

            Button plusButton = new Button
            {
                Text = "+",
                FontSize = 28,
                BackgroundColor = Color.FromArgb("#4B79E8"),
                TextColor = Colors.White,
                CornerRadius = 12
            };

            minusButton.Clicked += (s, e) =>
            {
                counter.Value--;
                valueLabel.Text = counter.Value.ToString();
                SaveCounters();
            };

            plusButton.Clicked += (s, e) =>
            {
                counter.Value++;
                valueLabel.Text = counter.Value.ToString();
                SaveCounters();
            };

            Grid buttons = new Grid
            {
                ColumnDefinitions =
                {
                    new ColumnDefinition { Width = GridLength.Star },
                    new ColumnDefinition { Width = GridLength.Star }
                },
                ColumnSpacing = 12
            };

            buttons.Add(minusButton, 0, 0);
            buttons.Add(plusButton, 1, 0);
            VerticalStackLayout content = new VerticalStackLayout
            {
                Spacing = 12,
                Children = { nameLabel, valueLabel, buttons }
            };

            Frame card = new Frame
            {
                BackgroundColor = Colors.White,
                CornerRadius = 16,
                Padding = 18,
                HasShadow = true,
                Content = content
            };
            CountersLayout.Children.Add(card);
        }
    }
}