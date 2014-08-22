//#define DEBUG

//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.Linq;
//using System.Web;
//using System.Web.Mvc;

//namespace MvcApplication1.Controllers
//{

//    public class ExemplosLivroController : Controller
//    {
//        //
//        // GET: /ExemplosLivro/


//        public void ActionTeste()
//        {
//            Action[] actions = new Action[3];
//            for (int i = 0; i < 3; i++)
//            {
//                int loop = i;
//                actions[i] = () => Console.Write(loop);
//            }

//            foreach (Action a in actions)
//                a();
//        }

//        /*
//         * Teste enumerator
//         */
//        IEnumerable<string> Foo()
//        {
//            try
//            {
//                yield return "One";
//            }
//            finally { }
//        }
//        public void IteratorTeste()
//        {
//            string firstElement = null;
//            var sequence = Foo();
//            using (var enumerator = sequence.GetEnumerator())
//            {
//                if (enumerator.MoveNext())
//                    firstElement = enumerator.Current;
//            }
//        }

//        //IEnumerable
//        /*
//         * Fibonnaci
//         */
//        public static IEnumerable<int> Fibs(int fibCount)
//        {
//            for (int i = 0, prevFib = 1, curFib = 1; i < fibCount; i++)
//            {
//                yield return prevFib;
//                int newFib = prevFib + curFib;
//                prevFib = curFib;
//                curFib = newFib;
//            }
//        }


//        public static IEnumerable<string> Teste()
//        {
//            yield return "One";
//            yield return "Two";
//            yield return "Three";
//        }
//        /// <summary>
//        ///     Método Main da classe
//        /// </summary>
//        public static void main()
//        {
//            #pragma warning disable 414 
//            foreach (string s in Teste())
//                Console.WriteLine(s);

//            foreach (int f in Fibs(10))
//                Console.WriteLine(f);


//            int i = 7;
//            dynamic d = i;
//            long l = d;
             

            
//                var teste11 = "teste";
//                dynamic teste12 = "teste";

//                Console.WriteLine();


//                string[] array = "esse é um teste".Split();
//                string inteira = string.Join(" ", array);

//                System.IO.File.WriteAllText("data.txt", "Testando...", System.Text.Encoding.ASCII);
                

//            /*
//            int x = teste11; // Erro de compilação
//            int y = teste12; // Erro de runtime, não acusando erro durante a compilação
//            */
//        }
        


        


//    }
//}
