using System.ComponentModel;
using System.Windows.Forms.Design;

namespace ImageBox
{
    public class MiniMapControlDesigner : ControlDesigner
    {
        public override void Initialize(IComponent component)
        {  
            base.Initialize(component);
            EnableDesignMode(((ImageBox)Control).MiniMap, "MiniMap");
        }
    }
}