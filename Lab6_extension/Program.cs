using Lab6_extension.extension;





// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!dd");

// przykład Example1_Extension
1.PrintInt();

1.PrintObj();
('a' + 2).PrintObj();
('a' + 2).PrintInt();
("a" + 2).PrintObj();
'a'.PrintObj();
(new { i = 1, a = "" }).PrintObj();




// przykład Example2_Extension
// Operacja nie zadziała, poniewaz jest object
// new { a = 1, b = 3, }.Operacja(i => i.a + i.a);
// Operacja2 działa ponieważ użyliśmy template <O> 
new { a = 1, b = 3, }.Operacja2(i => i.a + i.a);


var l = new List<int>(){7, 3, 1, 2, 3, 4, 5, 6, 0};
// uniwersalne sortowanie bombelkowe, predykat służy do porównywania obiektów w liscie
var l2 = l.BubbleSort1((a,b)=>a-b);
String.Join(',', l).PrintObj();
String.Join(',', l2).PrintObj();

var lu = new List<User>(){
    new User{Name="Jarek", Id=0},
    new User{Name="Bartek", Id=1},
    new User{Name="Tomek", Id=2},
};
var lu2 = lu.BubbleSort1((a,b)=>a.Name.CompareTo(b.Name));
String.Join(',', lu).PrintObj();
String.Join(',', lu2).PrintObj();



