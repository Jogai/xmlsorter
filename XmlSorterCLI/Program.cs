using CommandLine;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XmlSorterCLI
{
    class Options
    {
        [Option('i', Required = true, HelpText = "Input file.")]
        public string SourceFilePath { get; set; }

        [Option('o', Required = false, HelpText = "Output file.")]
        public string TargetFilePath { get; set; }

        [Option('s', "sort-attr", Required = false, HelpText = "Sort attributes.")]
        public bool SortAttributes { get; set; }

        [OptionList('a', "sorting-attr", Required = false, HelpText = "Sort by specific attributes.")]
        public IList<string> FilteredSortingAttibutes { get; set; }

        // Omitting long name, default --verbose
        [Option(HelpText = "Prints all messages to standard output.")]
        public bool Verbose { get; set; }

    }

    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var options = new Options();
                if (Parser.Default.ParseArguments(args, options))
                {
                    if (options.FilteredSortingAttibutes == null)
                        options.FilteredSortingAttibutes = new List<string>();
                    bool sortBySpecificAttributes = options.FilteredSortingAttibutes.Count > 0;
                    bool stdout = false;
                    if (options.TargetFilePath == null)
                    {
                        options.TargetFilePath = Path.Combine(Path.GetTempPath(), "Target.xml");
                        stdout = true;
                    }
                    XmlSorterLib.Sorter.SortElementHelper(options.SourceFilePath, options.TargetFilePath, options.SortAttributes, sortBySpecificAttributes, options.FilteredSortingAttibutes);
                    if (stdout)
                        Console.WriteLine(File.ReadAllText(options.TargetFilePath));
                }
                else
                    Console.Error.WriteLine("ERROR: cannot parse arguments");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("ERROR: {0}", ex.Message);
            }

        }
    }
}
