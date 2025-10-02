namespace AutomotiveApp.BlazorUI.Models
{
    public class ColumnDefinition<T>
    {
        public string Header { get; set; }
        public Func<T, object> CellTemplate { get; set; }
        public bool IsSearchable { get; set; } = false;
        public bool IsStatusColumn { get; set; }
        public bool IsActionColumn { get; set; }
    }
}
