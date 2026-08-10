using System.Collections.Generic;
namespace RevitAutoSheetGenerator.Models;
//cấu hình dự án, lưu trữ/đọc từ file json
public class ConfigModel
{
    public List<SheetItem> Sheets {get; set;}=new();
    public string DefaultTitleBlockUniqueId {get; set;}=string.Empty;
    public string DefaultTitleBlockName {get; set;}=string.Empty;
}