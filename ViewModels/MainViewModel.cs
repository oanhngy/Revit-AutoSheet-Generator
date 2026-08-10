using System;
using System.Collections.ObjectModel;
using System.IO;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RevitAutoSheetGenerator.Models;
using RevitAutoSheetGenerator.Services;

namespace RevitAutoSheetGenerator.ViewModels;

public partial class MainViewModel : ObservableObject
{
    //gọi JsonConfigService (milestone 1)
    private readonly JsonConfigService _configService;

    private readonly string _configFilePath;

    //list bản vẽ
    public ObservableCollection<SheetItem> Sheets {get;}=new();

    //list title block lấy từ revit api(milestone 3), hiển thị trong ComboBox để user chọn
    public ObservableCollection<string> AvailableTitleBlocks {get;}=new();

    [ObservableProperty]
    private SheetItem? _selectedSheet;
    [ObservableProperty]
    private string _defaultTitleBlock=string.Empty;
    [ObservableProperty]
    private bool _isBusy;

    public MainViewModel()
    {
        _configService=new JsonConfigService();

        //lưu 'sheet.json' trong AppData/Roaming (standard)
        string appDataPath=Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        string appFolder=Path.Combine(appDataPath, "RevitAutoSheetGenerator");
        _configFilePath=Path.Combine(appFolder, "sheet.json");
        
        LoadConfigData(); 
    }

    //apply ReplayCommand, tự sinh lệnh sự kiện nút bấm (UI commands)
    //khai báo private, Source Generator tự động sinh ICommand public tương đương có hậu tố Command
    //ADD NEW SHEET
    [RelayCommand]
    private void AddSheet()
    {
        var newSheet=new SheetItem
        {
            SheetNumber= "A-101",
            SheetName= "New Sheet",
            TitleBlockName= DefaultTitleBlock
        };
        //thêm vô ObservableCollection
        Sheets.Add(newSheet);
        SelectedSheet= newSheet;
    }

    //DELETE SELECTED SHEET
    [RelayCommand]
    private void RemoveSheet()
    {
        if(SelectedSheet==null)
            return;
        Sheets.Remove(SelectedSheet);
        SelectedSheet=null; //set lại null sau xóa
    }

    //SAVE to JSON
    [RelayCommand]
    private void SaveConfig()
    {
        var config=new ConfigModel
        {
            DefaultTitleBlockName=DefaultTitleBlock,
            Sheets= new System.Collections.Generic.List<SheetItem>(Sheets)
        };
        bool success= _configService.SaveConfig(_configFilePath, config); //ghi file = service trong milestone 1

        if(success)
        {
            Console.WriteLine("Configuration saved successfully.");
        }
    }

    //TẠO SHEET THẬT TRONG REVIT (CONNECT TO MILESTONE 3 LATER)
    [RelayCommand]
    private void CreateSheetsInRevit()
    {
        //UX defense: khóa giao diện --> tránh việc user click nhiều lần gây các lệnh chạy song song
        IsBusy=true;

        try
        {
            //viết lại later trong milestone 3, gọi Revit API để tạo sheet
            Console.WriteLine($"Creating {Sheets.Count} sheets in Revit...");
            SaveConfig();
        }

        catch (Exception ex)
        {
            Console.WriteLine($"Error creating sheets in Revit: {ex.Message}");
        }

        finally
        {
            IsBusy=false; //mở khóa UX
        }
    }

    //hàm nội bộ
    //tải data từ json lên bảng hiển thị
    private void LoadConfigData()
    {
        ConfigModel config= _configService.LoadConfig(_configFilePath);
        Sheets.Clear();

        //load từng sheet vào UI
        foreach(var sheet in config.Sheets)
        {
            Sheets.Add(sheet);
        }
        DefaultTitleBlock= config.DefaultTitleBlockName;
    }
}