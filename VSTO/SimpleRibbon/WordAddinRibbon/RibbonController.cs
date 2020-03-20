using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using WordAddinRibbon.Properties;

namespace WordAddinRibbon
{
    [ComVisible(true)]
    public class RibbonController : Microsoft.Office.Core.IRibbonExtensibility
    {
        private Microsoft.Office.Core.IRibbonUI _ribbonUi;

        public string GetCustomUI(string ribbonID) =>
            @"<customUI xmlns='http://schemas.microsoft.com/office/2009/07/customui'>
                        <ribbon>
                           <tabs>
                                <tab id='sample_tab' label='GoTask'>
                                    <group id='sample_group' label='Operations'>
                                        <button id='do_1' label='Do 1' size='large' getImage='OnDo1GetImage' onAction='OnDo1Click'/>
                                    </group>
                                </tab>
                            </tabs>
                        </ribbon>
                    </customUI>";

        public void OnLoad(Microsoft.Office.Core.IRibbonUI ribbonUI)
        {
            _ribbonUi = ribbonUI;
        }

        public void OnDo1Click(Microsoft.Office.Core.IRibbonControl control)
        {
            MessageBox.Show(Resources.Do1Action);
        }

        public Bitmap OnDo1GetImage(Microsoft.Office.Core.IRibbonControl control) => Resources.Do1_128px;
    }
}
