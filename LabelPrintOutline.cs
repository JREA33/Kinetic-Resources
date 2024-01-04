//Create Header Text
string btHeader = "%BTW% " + "/AF=" + @"\\ecm-docstar\Users\bartend_svc\Documents\BarTender\LabelFormats\2X4\HdwLbl.btw " + "/D=\"<Trigger File Name>\" /PRN=\"" + @"\\spi-prt-srv\PRT487" + "\" /DBTEXTHEADER=3 /R=3 /P \n%END%\n";


//Create columnNames
string columnNames = "";

int i = 0;

while (i < result.Results.Columns.Count)
{
  if (i == (result.Results.Columns.Count - 1))
  {
    columnNames = columnNames + "\"_" + result.Results.Columns[i].ColumnName.ToString() + "\"\n";
    i++;
  }
  else
  {
    columnNames = columnNames + "\"_" + result.Results.Columns[i].ColumnName.ToString() + "\"" + ", ";
    i++;
  }
}

//Create Records
string records = "";

foreach (var row in result.Results)
{
  foreach (var column in result.Results.Columns)
  {
    if (row.GetColumnIndex(column.ColumnName) == result.Results.Columns.Count - 1)
    {
      records = records + "\"" + row[column] + "\"";
    }
    else
    {
      records = records + "\"" + row[column] + "\", ";
    }
  }
  
  records = records + "\n";
}

//Combine Output Data
string finalData = btHeader + columnNames + records;

//Define file output path

string orderNum = result.Results.FirstOrDefault().SerialNo_OrderNum.ToString();
string timeStamp = DateTime.Now.ToString("(yyyy-MM-dd_HHmm)");
string path = @"\\161839.file.core.windows.net\161839\Bartender\" + orderNum + "_FabricTicket" + timeStamp + ".bt";

//PublishInfoMessage(path, Ice.Common.BusinessObjectMessageType.Information, Ice.Bpm.InfoMessageDisplayMode.Individual, "", "");

//Create File
using (FileStream fs = File.Create(path))
{
    byte[] info = new UTF8Encoding(true).GetBytes(finalData);
    fs.Write(info, 0, info.Length);
}
