using System;
using PowerPointAddinRibbon.Properties;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Microsoft.Office.Core;
using Microsoft.Office.Interop.PowerPoint;
using Application = Microsoft.Office.Interop.PowerPoint.Application;
using Shape = Microsoft.Office.Interop.PowerPoint.Shape;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.IO.Compression;

namespace PowerPointAddinRibbon
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
                                        <button id='extract_images' label='Extract Images' size='large' getImage='OnGetImage' onAction='OnExtractImage'/>                                        
                                    </group>
                                </tab>
                            </tabs>
                        </ribbon>
                    </customUI>";

        public void OnLoad(Microsoft.Office.Core.IRibbonUI ribbonUI)
        {
            _ribbonUi = ribbonUI;
        }

        private string GetSaveDir()
        {
            using (var dialog = new CommonOpenFileDialog())
            {
                dialog.IsFolderPicker = true;

                var result = dialog.ShowDialog();

                if (result == CommonFileDialogResult.Ok)
                {
                    return dialog.FileName;
                }
            }

            return null;
        }

        private void ExtractWithZip(string pptxFile, string directory)
        {
            var zipDir = "";

            using (var arh = ZipFile.Open(pptxFile, ZipArchiveMode.Read))
            {
                zipDir = Path.Combine(directory, "temp_zip");
                Directory.CreateDirectory(zipDir);
                arh.ExtractToDirectory(zipDir);

                foreach (var f in Directory.GetFiles(Path.Combine(zipDir, @"ppt\media")))
                    File.Copy(f, Path.Combine(directory, Path.GetFileName(f)));
            }

            try
            {
                var dirToDelete = new DirectoryInfo(zipDir);
                dirToDelete.Delete(true);
            }
            catch
            {
                //
            }
        }

        public void OnExtractImage(Microsoft.Office.Core.IRibbonControl control)
        {
            var app = Globals.ThisAddIn.Application;

            var saveDirectory = GetSaveDir();

            if (string.IsNullOrWhiteSpace(saveDirectory))
            {
                return;
            }

            ExtractWithZip(app.ActivePresentation.FullName, saveDirectory);
            
            var i = 1;
            foreach (Slide slide in app.ActivePresentation.Slides)
            {
                foreach (Shape shape in slide.Shapes)
                {
                    var doExport = false;

                    if (shape.Type == MsoShapeType.msoPicture ||
                        shape.Type == MsoShapeType.msoLinkedPicture)
                    {
                        doExport = true;
                    }
                    else if (shape.Type == MsoShapeType.msoPlaceholder)
                    {
                        if (shape.PlaceholderFormat.ContainedType == MsoShapeType.msoPicture ||
                            shape.PlaceholderFormat.ContainedType == MsoShapeType.msoLinkedPicture)
                        {
                            doExport = true;
                        }
                    }
                    else
                    {
                        try
                        {
                            // this is just a dummy code. In case there is no picture in the
                            // shape, any attempt to read the CropBottom variable will throw 
                            // an exception
                            var test = shape.PictureFormat.CropBottom > -1;
                            doExport = true;
                        }
                        catch
                        {
                            doExport = false;
                        }
                    }

                    if(doExport) 
                        shape.Export(Path.Combine(saveDirectory, $"{i++}.png"), PpShapeFormat.ppShapeFormatPNG);
                }
            }
        }

        public Bitmap OnGetImage(Microsoft.Office.Core.IRibbonControl control) => Resources.Do1_128px;
    }
}
