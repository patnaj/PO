// See https://aka.ms/new-console-template for more information
using System.Collections;
using System.Data.Common;
using System.Net.Http.Json;

Console.WriteLine("Hello, World!");

var library = new Library()
{
    Catalogs = new List<Catalog>(){
        new Catalog(){ThematicDepartment="IT C# development",
            Items = new List<Item>(){
                new Jurnal(){Title="JAISCR",Publisher="Springer", Number=1},
                new Jurnal(){Title="IEEE",Publisher="Neurocomputing", Number=1}
            }
        }
    },
    Librarians = new List<Person>(){
        new Person(){FirstName="Robert", LastName="Cook"},
        new Person(){FirstName="Albert", LastName="Nowak"}
    }

};
ISearch serch = library;
Console.WriteLine("Znajdz zawierajace 'a'");
serch.Find("a").ToList().ForEach(i => Console.WriteLine(i));
Console.WriteLine("Wszystkie 'Pozycje'");
serch.Find("Pozycja").ToList().ForEach(i => Console.WriteLine(i));

// Console.ReadKey();
 

public interface ISearch
{
    object[] Searchable { get; }

    IList<object> Find(string query, List<object>? start = null)
    {
        var result = start ?? new List<object>();
        foreach (var sitem in Searchable)
        {
            if (sitem is IEnumerable list)
            {
                foreach (var item in list)
                {
                    if ($"{item}".Contains(query))
                        result.Add(item);
                    if (item is ISearch serch_item)
                        serch_item.Find(query, result);
                }
            }
        }
        return result;
    }
}


public class Jurnal : Item
{
    public int Number { get; set; }

    public override string ToString()
    {
        return base.ToString() + $" Jurnal {Number}";
    }
}

public abstract class Item
{
    private int _id;
    private static int _idNext = 0;

    protected Item()
    {
        _id = Item._idNext++;
    }

    public int Id { get { return _id; } }
    public string Title { get; set; } = "";
    public string Publisher { get; set; } = "";

    public override string ToString()
    {
        return $"Pozycja: {Id} {Title} {Publisher}";
    }
}

public class Catalog : ISearch
{
    public string ThematicDepartment { get; set; } = "";
    public override string ToString()
    {
        return $"Katalog: {ThematicDepartment} ";
    }
    public IList<Item> Items { get; set; } = new List<Item>();
    public object[] Searchable { get { return [Items]; } }
}

public class Library : ISearch
{
    public IList<Catalog> Catalogs { get; set; } = new List<Catalog>();
    public IList<Person> Librarians { get; set; } = new List<Person>();

    public object[] Searchable { get { return [Catalogs, Librarians]; } }

}

public class Person
{
    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";

    public override string ToString()
    {
        return $"Person: {FirstName} {LastName}";
    }
}