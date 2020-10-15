using System;
using System.Drawing;
using System.Runtime.InteropServices;
using ExcelDna.Integration;
using ExcelDna.Integration.CustomUI;
using ExcelRibbonUDF.Properties;
using Microsoft.Office.Interop.Excel;

namespace ExcelAddInRibbon
{
    [ComVisible(true)]
    public class RibbonController : ExcelRibbon, IDisposable
    {
        private Microsoft.Office.Core.IRibbonUI _ribbonUi;

        private Application App
        {
            get => (Application)ExcelDnaUtil.Application;
        }            

        public override string GetCustomUI(string ribbonID) =>
                @"<customUI xmlns='http://schemas.microsoft.com/office/2009/07/customui'>
                        <ribbon>
                           <tabs>
                                <tab id='sample_tab' label='GoTask'>
                                    <group id='sample_group' label='Operations'>                                        
                                        <button id='do_reverse_range' label='Reverse' size='large' getImage='OnDoReverseGetImage' onAction='OnDoReverse'/>
                                    </group>
                                </tab>
                            </tabs>
                        </ribbon>
                    </customUI>";

        public void OnLoad(Microsoft.Office.Core.IRibbonUI ribbonUI)
        {
            _ribbonUi = ribbonUI;          
        }        
       
        public void Dispose()
        {            
        }

        public void OnDoReverse(Microsoft.Office.Core.IRibbonControl control)
        {
            var selectedRange = App.Selection;

            if (selectedRange == null) return;

            foreach (Range cell in selectedRange)
            {
                var next = cell.Offset[0, 1];
                next.Formula = $"=ReverseString({cell.Address})";
            }
        }

        public Bitmap OnDoReverseGetImage(Microsoft.Office.Core.IRibbonControl control) => Resources.Reverse_128px;
    }
}
