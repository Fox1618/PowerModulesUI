using System.Windows;
using System.Windows.Controls;
using Telemetria.UI;

namespace PowerModulesManager
{
    public class BottomPanel : ControlPanelBase, IBottomPanel
    {
        public BottomPanel()
        {
            AddSeparator();
        }
        public IImageButton StartButton { get; set; }
        public IBarcodeBox BarCodeBox { get; set; }
    }
     public interface IBottomPanel : IInputElement
    {
        IImageButton StartButton { get; set; }
        IBarcodeBox BarCodeBox { get; set; }
    }
}