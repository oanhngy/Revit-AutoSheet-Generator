namespace RevitAutoSheetGenerator.Models;
//Độc lập vs Revit API, chỉ là class lưu trữ dữ liệu
public class SheetItem
{
    public string SheetNumber { get; set; }=string.Empty;
    public string SheetName { get; set; }=string.Empty;
    public string TitleBlockName { get; set; }=string.Empty;
    public string TitleBlockUniqueId { get; set; }=string.Empty;
}