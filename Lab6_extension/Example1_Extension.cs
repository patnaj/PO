
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Lab6_extension.extension
{

    /// <summary>
    /// Statyczna klasa z extension, jej statyczne metody dołączą się do typów okreslonych przez this <typ> 
    /// </summary>
    public static class Example1_Extension
    {
        /// <summary>
        /// Metoda podłączy się pod każdy int 
        /// </summary>
        /// <param name="a">Referencja do obiektu int, na z którym wywołano metodę</param>
        public static void PrintInt(this int a)
        {
            Console.WriteLine($"PrintInt: {a}");
        }

        /// <summary>
        /// Podłączy się pod wszystko, ponieważ wszystko dziedziczy po object
        /// </summary>
        /// <param name="a"></param>
        public static void PrintObj(this object a)
        {
            Console.WriteLine($"PrintObj: {a}");
        }

    }

    public class User
    {
        public string Name { get; set; } = "";
        public int Id { get; set; }

        override public string ToString()
        {
            return $"{Id}:{Name}";
        }
    }

    public static class Example2_Extension
    {

        /// <summary>
        /// Funkcja podłączy się pod wszytko i przyjmie predykat dla obiektu, który ma zrucić int
        /// </summary>
        /// <param name="a"></param>
        /// <param name="f"></param>
        /// <returns></returns>
        public static int Operacja(this object a, Func<object, int> f)
        {
            return f(a);
        }

        /// <summary>
        /// Funkcja podłaczy się pod wszystko, i przyjmuje predykat. Składania predykatu podpowiadana jest zgodnie z typem.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="f"></param>
        /// <typeparam name="O"></typeparam>
        /// <returns></returns>
        public static int Operacja2<O>(this O a, Func<O, int> f)
        {
            return f(a);
        }

        public static IEnumerable<O> BubbleSort1<O>(this IEnumerable<O> source, Func<O, O, int> fun)
        {
            var _out = source.ToList();
            for (int i = _out.Count; i > 1; i--)
            {
                // Console.WriteLine($"i = {i}");
                for (int ii = 0; ii < i - 1; ii++)
                {
                    // Console.WriteLine($" ii = {ii}");
                    if (fun(_out[ii], _out[ii + 1]) > 0)
                    {
                        var tmp = _out[ii];
                        _out[ii] = _out[ii + 1];
                        _out[ii + 1] = tmp;
                        // Console.WriteLine($" \tl = {String.Join(',', _out)}");
                    }
                }
            }
            return _out;
        }
    }
}

