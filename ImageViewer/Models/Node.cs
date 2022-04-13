using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Avalonia.Media.Imaging;
using System.IO;

namespace ImageViewer.Models
{
    public class Node
    {
        public ObservableCollection<Node> Subfolders { get; set; }
        public string Name { get; }
        public string StrPath { get; set; }
        public Bitmap Image { get; set; }
        public Node Parent { get; set; }
        public Node(string strPath, bool isImage = false, Node parent = null)
        {
            Subfolders = new ObservableCollection<Node>();
            StrPath = strPath;
            Name = Path.GetFileName(strPath);
            if(Name == "")
            {
                Name = strPath;
            }
            if (isImage)
            {
                Image = new Bitmap(strPath);
                Parent = parent;
            }
        }
        public void LoadSubfolders()
        {
            try
            {
                string[] subdirs = Directory.GetDirectories(StrPath, "*", SearchOption.TopDirectoryOnly);

                foreach (string dir in subdirs)
                {
                    Node currentNode = new Node(dir);
                    Subfolders.Add(currentNode);
                }
                LoadImages();
            }
            catch
            {

            }
        }
        public void LoadImages()
        {
            List<string> images = new List<string>();
            images.AddRange(Directory.GetFiles(StrPath, "*.*", SearchOption.TopDirectoryOnly)
                             .Where(f => f.EndsWith(".jpg") || f.EndsWith(".png")).ToArray());
            if (images.Count > 0)
            {
                foreach (string image in images)
                {
                    Node imageNode = new Node(image, true, this);
                    Subfolders.Add(imageNode);
                }
            }
        }
    }
}
