public struct Sheet1 : IConfigData
{
public string Id { get; set;}
public string Chinese { get; set;}
public string English { get; set;}
public Sheet1(string Id,string Chinese,string English)
{
this.Id = Id;
this.Chinese = Chinese;
this.English = English;
}
}
