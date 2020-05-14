"# Crystal Report Dynamic Image" 
1. Create DataSet

<?xml version="1.0" encoding="UTF-8"?>
<xs:schema xmlns:xs="http://www.w3.org/2001/XMLSchema" elementFormDefault="qualified">

  <xs:element name="DataSets" type="dataSets"/>

  <xs:complexType name="dataSets">
    <xs:sequence>
      <xs:element name="image_stream" type="xs:base64Binary" minOccurs="0" />
    </xs:sequence>
  </xs:complexType>

</xs:schema>  

2. Add DataSet to Crystal Report by Database Expert
3. Drag field name "image_stream" to Report
4. Create DataTable in C# code and add data to this DataTable as binary stream


        private void LoadReport()
        {
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
                image.Save(stream, ImageFormat.Png);
                return stream.ToArray();
            }
        }

