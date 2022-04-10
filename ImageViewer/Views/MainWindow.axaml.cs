using Avalonia.Controls;
using ImageViewer.ViewModels;
using System.Collections.ObjectModel;

namespace ImageViewer.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var carousel = this.FindControl<Carousel>("carousel");
            this.FindControl<Button>("next").Click += delegate
            {
                carousel.Next();
                ObservableCollection<Node> items = carousel.Items as ObservableCollection<Node>;
                var currentItem = carousel.SelectedItem;
                if(currentItem == items[items.Count - 1])
                {
                    var context = this.DataContext as MainWindowViewModel;
                    context.EnableNext = false;
                }
                if (currentItem != items[0])
                {
                    var context = this.DataContext as MainWindowViewModel;
                    context.EnableBack = true;
                }

            };
            this.FindControl<Button>("back").Click += delegate
            {
                carousel.Previous();
                ObservableCollection<Node> items = carousel.Items as ObservableCollection<Node>;
                var currentItem = carousel.SelectedItem;
                if (currentItem == items[0])
                {
                    var context = this.DataContext as MainWindowViewModel;
                    context.EnableBack = false;
                }
                if (currentItem != items[items.Count - 1])
                {
                    var context = this.DataContext as MainWindowViewModel;
                    context.EnableNext = true;
                }
            };
        }
        public void OnTreeViewSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var context = this.DataContext as MainWindowViewModel;
            context.ChangeSelectedImages(e.AddedItems[0]);
        }
    }
}
