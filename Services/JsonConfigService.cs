using System;
using System.IO;
using Newtonsoft.Json;
using RevitAutoSheetGenerator.Models;
namespace RevitAutoSheetGenerator.Services;

// xử lý IO, input=link, đọc json -->chuyển thành ConfigModel (deserialization), output=ConfigModel-->ghi xuống json (serialization)
public class JsonConfigService
{
    private readonly JsonSerializerSettings _settings;
    public JsonConfigService()
    {
        _settings=new JsonSerializerSettings
        {
            Formatting=Formatting.Indented,
            NullValueHandling=NullValueHandling.Ignore
        };
    }

    public ConfigModel LoadConfig(string filePath)
    {
        if(string.IsNullOrWhiteSpace(filePath))
            return new ConfigModel();

        try
        {
            if(!File.Exists(filePath))
            {
                return new ConfigModel();
            }

            string jsonContent=File.ReadAllText(filePath);
            var config=JsonConvert.DeserializeObject<ConfigModel>(jsonContent,_settings);
            return config ?? new ConfigModel();
        }

        catch(Exception ex)
        {
            Console.WriteLine($"Error loading config from {filePath}: {ex.Message}");
            return new ConfigModel();
        }
    }

    //save
    public bool SaveConfig(string filePath, ConfigModel config)
    {
        if(string.IsNullOrEmpty(filePath))
            return false;

        if(config==null)
            return false;

        try
        {
            //tìm thư mục cha chứa file
            string? directoryPath= Path.GetDirectoryName(filePath);

            //nếu chưa có--> tạo
            if(!string.IsNullOrEmpty(directoryPath) && !Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            string jsonContent=JsonConvert.SerializeObject(config, _settings);
            File.WriteAllText(filePath, jsonContent);
            return true;
        }

        catch (Exception ex)
        {
            Console.WriteLine($"Error saving config to {filePath}: {ex.Message}");
            return false;
        }
    }
}