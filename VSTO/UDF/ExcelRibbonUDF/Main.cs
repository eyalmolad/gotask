using ExcelDna.Integration;

namespace ExcelRibbonUDF
{
    // The class here implements the ExcelDna.Integration.IExcelAddIn interface.
    // This allows the add-in to run code at start-up and shutdown.
    public class ExcelRibbonUDFAddin : IExcelAddIn
    {
        public void AutoOpen()
        {
            // startup code
        }

        public void AutoClose()
        {
            // clean up
        }
    }
}
