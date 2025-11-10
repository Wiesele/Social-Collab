using MRO.SMK.Docu.ApplicationCore.Abstracts;

namespace MRO.SMK.Docu.ApplicationCore.Models;

public class Repository : NamedEntity
{
    public string Location { get; set; }
}