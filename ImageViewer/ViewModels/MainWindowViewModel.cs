using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI;
using System.Reactive;
using Avalonia.Media.Imaging;
using System.IO;
using ImageViewer.Models;

namespace ImageViewer.ViewModels
{
    
    public class MainWindowViewModel : ViewModelBase
    {
        public ObservableCollection<Node> Folders { get; }
        private ObservableCollection<Node> selectedImages;
        public ObservableCollection<Node> SelectedImages
        {
            get => selectedImages;
            set
            {
                this.RaiseAndSetIfChanged(ref selectedImages, value);
            }
        }
        List<string> allDrivesNames;
        public MainWindowViewModel()
        {
            string root = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            Folders = new ObservableCollection<Node>();
            SelectedImages = new ObservableCollection<Node>();

            allDrivesNames = new List<string>();
            DriveInfo[] allDrives = DriveInfo.GetDrives();

            foreach (DriveInfo drive in allDrives)
            {
                allDrivesNames.Add(drive.Name);
                Node rootNode = new Node(drive.Name);
                Folders.Add(rootNode);
            }
        }
        public void ChangeSelectedImages(object obj)
        {
            var thisNode = obj as Node;
            if(thisNode.Image != null)
            {
                SelectedImages.Clear();
                int count = 0;
                foreach (var node in thisNode.Parent.Subfolders)
                {
                    if(node.Image != null)
                    {
                        count++;
                    }
                }
                EnableNext = count != 1;

                SelectedImages.Add(thisNode);
                foreach (var imageNode in thisNode.Parent.Subfolders)
                {
                    if (imageNode.Image != null && imageNode != thisNode)
                    {
                        SelectedImages.Add(imageNode);
                    }
                }
            }
            
        }
        private bool enableNext = false;
        public bool EnableNext
        {
            get { return enableNext; }
            set
            {
                this.RaiseAndSetIfChanged(ref enableNext, value);
            }
        }
        private bool enableBack = false;
        public bool EnableBack
        {
            get { return enableBack; }
            set
            {
                this.RaiseAndSetIfChanged(ref enableBack, value);
            }
        }
    }
}
