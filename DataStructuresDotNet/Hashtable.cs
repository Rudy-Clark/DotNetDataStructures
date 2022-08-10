using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStructuresDotNet
{
    public class Hashtable
    {
        public static void Print()
        {
            Dictionary<string, string> openWith = new();

            openWith.Add("txt", "notepad.exe");
            openWith.Add("bmp", "paint.exe");
            openWith.Add("dib", "paint.exe");
            openWith.Add("rtf", "wordpad.exe");

            try
            {
                openWith.Add("txt", "notepad.exe");

            }
            catch (ArgumentException)
            {
                Console.WriteLine("An element with key = \"txt\" already exists.");

            }

            Console.WriteLine("For key = 'rtf', value = {0}", openWith["rtf"]);

            // if key doesn't exist, set indexer for that key
            // add a new key/value pair
            openWith["doc"] = "winword.exe";

            try
            {
                Console.WriteLine("For key = 'tif', value = {0}", openWith["tif"]);

            } catch (KeyNotFoundException)
            {
                Console.WriteLine("Argument 'tif' doesn't exist!");

            }

            string value = "";
            if (openWith.TryGetValue("tif", out value))
            {
                Console.WriteLine("For key = 'tif', value = {0}", value);

            }
            else
            {
                Console.WriteLine("key = 'tif' doesn't exists.!");

            }

            if (!openWith.ContainsKey("ht"))
            {
                openWith.Add("ht", "hypertrm.exe");
                Console.WriteLine("Value added for key = 'ht', value = {0}", openWith["ht"]);

            }
            
            Console.WriteLine();
            foreach (KeyValuePair<string, string> item in openWith)
            {
                Console.WriteLine("Key = {0}, Value = {1}", item.Key, item.Value);

            }

            Dictionary<string, string>.ValueCollection values = openWith.Values;

            Console.WriteLine();
            foreach (string s in values)
            {
                Console.WriteLine("openWith Value = {0}", s);

            }

            Dictionary<string, string>.KeyCollection keys = openWith.Keys;
            Console.WriteLine();
            foreach (string k in keys)
            {
                Console.WriteLine("openWith Key = {0}", k);

            }

            Console.WriteLine("\nRemove key ['doc']");
            openWith.Remove("doc");

            if (!openWith.ContainsKey("doc"))
            {
                Console.WriteLine("Key = 'doc' is not found!");
            }






        }
    }
}
