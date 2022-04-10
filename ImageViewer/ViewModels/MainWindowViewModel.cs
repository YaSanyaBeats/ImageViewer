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

namespace ImageViewer.ViewModels
{
    public class Node
    {
        public ObservableCollection<Node> Subfolders { get; set; }
        public string Name { get; }
        public string StrPath { get; }
        public Bitmap Image { get; set; }
        public Node Parent { get; set; }
        public Node(string strPath, bool isImage = false, Node parent = null)
        {
            StrPath = strPath;
            Name = Path.GetFileName(strPath);
            if (isImage)
            {
                Image = new Bitmap(strPath);
                Parent = parent;
            }
        }
    }
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
        public MainWindowViewModel()
        {
            string root = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            Folders = new ObservableCollection<Node>();
            SelectedImages = new ObservableCollection<Node>();
            if (withImages(root))
            {
                Node rootNode = new Node(root);
                rootNode.Subfolders = GetSubfolders(root);
                GetImages(root, rootNode);
                Folders.Add(rootNode);
            }
        }
        public ObservableCollection<Node> GetSubfolders(string strPath)
        {
            ObservableCollection<Node> subfolders = new ObservableCollection<Node>();
            string[] subdirs = Directory.GetDirectories(strPath, "*", SearchOption.TopDirectoryOnly);

            foreach (string dir in subdirs)
            {
                if (withImages(dir))
                {
                    Node currentNode = new Node(dir);
                    currentNode.Subfolders = new ObservableCollection<Node>();
                    currentNode.Subfolders = GetSubfolders(dir);
                    GetImages(dir, currentNode);
                    subfolders.Add(currentNode);
                }
            }

            return subfolders;
        }
        public void GetImages(string imagePath, Node node)
        {
            List<string> images = new List<string>();
            images.AddRange(Directory.GetFiles(imagePath, "*.*", SearchOption.TopDirectoryOnly)
                             .Where(f => f.EndsWith(".jpg") || f.EndsWith(".png")).ToArray());
            if (images.Count > 0)
            {
                if(node.Subfolders == null)
                {
                    node.Subfolders = new ObservableCollection<Node>();
                }
                foreach (string image in images)
                {
                    Node imageNode = new Node(image, true, node);
                    node.Subfolders.Add(imageNode);
                }
            }
        }
        public bool withImages(string srcPath)
        {
            //Да это долго, да мне стыдно(
            List<string> images = new List<string>();
            try
            {
                images.AddRange(Directory.GetFiles(srcPath, "*.*", SearchOption.AllDirectories)
                             .Where(f => f.EndsWith(".jpg") || f.EndsWith(".png")).ToArray());
            }
            catch (Exception ex)
            {

            }
            if (images.Count > 0)
            {
                return true;
            }
            return false;
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
                foreach (var imageNode in thisNode.Parent.Subfolders)
                {
                    if (imageNode.Image != null)
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
