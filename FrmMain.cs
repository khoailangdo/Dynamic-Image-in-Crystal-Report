using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Windows.Forms;
using QRCoder;

namespace DynamicImage
{
    public partial class FrmMain : Form
    {
        public int ReportType { get; set; }
        public decimal PatientId { get; set; }
        public string DeclarationCode { get; set; }
        public string QrCode { get; set; }
        public FrmMain()
        {
            InitializeComponent();

            PatientId = 1332;
            DeclarationCode = "DS12DE";
            QrCode = "1332|0989477292|DS12DE";
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            if ((PatientId < 1) || (string.IsNullOrWhiteSpace(DeclarationCode))) return;

            QRCodeGenerator _qrCode = new QRCodeGenerator();
            QRCodeData _qrCodeData = _qrCode.CreateQrCode(QrCode, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(_qrCodeData);
            Bitmap qrCodeImage = qrCode.GetGraphic(20);
            var qrCodeBytes = BitmapToBytesCode(qrCodeImage);


            var dataTableImage = new DataTable();
            var objDataColumn = new DataColumn("image_stream", Type.GetType("System.Byte[]"));
            dataTableImage.Columns.Add(objDataColumn);
            dataTableImage.Rows.Add(qrCodeBytes);

            var temp = new DeclarationReport();
            temp.Database.Tables[0].SetDataSource(dataTableImage);
            temp.SetParameterValue("DeclarationCode", DeclarationCode);

            crystalReportViewer1.ToolPanelView = ToolPanelViewType.None;
            crystalReportViewer1.ReportSource = temp;

        }
        private static byte[] BitmapToBytesCode(Bitmap image)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                image.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                return stream.ToArray();
            }
        }


    }
}
