using System.Collections.Generic;

/// <summary>
/// Member row serialized to the Who's Who directory JSON.
/// Property names stay camelCase so the existing client script can bind them.
/// </summary>
public class WhoisWhoMember
{
    public string id { get; set; }
    public string name { get; set; }
    public string firstName { get; set; }
    public string lastName { get; set; }
    public string initials { get; set; }
    public string organization { get; set; }
    public string orgId { get; set; }
    public string title { get; set; }
    public string pronouns { get; set; }
    public string institution { get; set; }
    public string certification { get; set; }
    public string yearOfGraduation { get; set; }
    public string email { get; set; }
    public string phone { get; set; }
    public string extension { get; set; }
    public string mobile { get; set; }
    public string linkedIn { get; set; }
    public string about { get; set; }
    public string photoUrl { get; set; }
    public List<string> committees { get; set; }
}