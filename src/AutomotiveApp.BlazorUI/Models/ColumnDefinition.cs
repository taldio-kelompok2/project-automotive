namespace AutomotiveApp.BlazorUI.Models
{
    public class ColumnDefinition<T>
    {
        public string Header { get; set; }
        public Func<T, object> CellTemplate { get; set; }
        public bool IsSearchable { get; set; } = false;
        public bool IsStatusColumn { get; set; } = false;
        public bool IsActionColumn { get; set; } = false;
        public bool IsImageColumn { get; set; } = false;

        public bool ShowEdit { get; set; } = false;
        public bool ShowDetails { get; set; } = false;
    }
}
