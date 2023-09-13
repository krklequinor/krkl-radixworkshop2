namespace web_app.Models;

public class ItemViewModel
{
    public List<ItemModel> Items { get; set; } = new List<ItemModel>();
    public bool UseApiClient { get; set; }
}

public class ItemModel
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Data { get; set; } = "";
    public bool IsSelected { get; set; }
}