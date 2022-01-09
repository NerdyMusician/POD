using POD.ViewModels;
using System.Windows;

namespace POD.Toolbox
{
    public static class Configuration
    {
        public static FrameworkElement AppFramework = new FrameworkElement();
        public static MainViewModel MainModelRef;

        public static readonly string ItemDataFilePath = "Data/PersonalObjectData.xml";

        public static readonly string XmlFileFilter = "XML files (*.xml)|*.xml";
        public static readonly string ImageFileFilter = "Image files|*.bmp;*.jpg;*.gif;*.png;*.tif";
        public static readonly string DocFileFilter = "Document files|*.docx;*.pdf;";
        public static readonly string AllFileFilter = "All files|*.*";

    }
}
