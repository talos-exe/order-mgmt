public class SummaryItem
{
    public string Title { get; set; }
    public List<LabelValue> LabelValues { get; set; }
}

public class LabelValue
{
    public string Label { get; set; }
    public int Value { get; set; }
}
