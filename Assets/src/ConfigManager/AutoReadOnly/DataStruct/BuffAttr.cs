public struct BuffAttr : IConfigData
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string ChinName { get; set; }
    public string Introduce { get; set; }
    public bool Immediately { get; set; }
    public bool Repeat { get; set; }
    public float Value_0 { get; set; }
    public float Value_1 { get; set; }
    public float Value_2 { get; set; }
    public BuffAttr(string Id, string Name, string ChinName, string Introduce, bool Immediately, bool Repeat, float Value_0, float Value_1, float Value_2)
    {
        this.Id = Id;
        this.Name = Name;
        this.ChinName = ChinName;
        this.Introduce = Introduce;
        this.Immediately = Immediately;
        this.Repeat = Repeat;
        this.Value_0 = Value_0;
        this.Value_1 = Value_1;
        this.Value_2 = Value_2;
    }
}
