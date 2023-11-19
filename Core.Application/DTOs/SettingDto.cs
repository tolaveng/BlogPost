namespace Core.Application.DTOs
{
    public class SettingDto
    {
        public string Name { get; set; }
        public string Value { get; set; }

        public SettingDto()
        {
        }

        public SettingDto(string name, string value)
        {
            Name = name;
            Value = value;
        }
    }
}
