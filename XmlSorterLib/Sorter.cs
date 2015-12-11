using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace XmlSorterLib
{
    public static class Sorter
    {
        public static void SortElementHelper(string sourceFilePath, string targetFilePath, bool sortAttributes, bool sortBySpecificAttributes, IEnumerable<string> FilteredSortingAttibutes)
        {
            XElement xe = XElement.Load(sourceFilePath);
            SortElement(xe, sortAttributes, sortBySpecificAttributes, FilteredSortingAttibutes);
            xe.Save(targetFilePath);

        }
        private static void SortElement(XElement xe, bool sortAttributes, bool sortBySpecificAttributes, IEnumerable<string> FilteredSortingAttibutes)
        {
            IEnumerable<XNode> NodesToBePreserved = xe.Nodes().Where(P => P.GetType() != typeof(XElement));
            if (sortAttributes)
            {
                xe.ReplaceAttributes(xe.Attributes().OrderBy(x => x.Name.LocalName));
            }
            if (!sortBySpecificAttributes || FilteredSortingAttibutes.Count() == 0)
            {
                xe.ReplaceNodes((xe.Elements().OrderBy(x => x.Name.LocalName).Union((NodesToBePreserved).OrderBy(P => P.ToString()))).OrderBy(N => N.NodeType.ToString()));
            }
            else
            {
                foreach (string Att in FilteredSortingAttibutes)
                {
                    xe.ReplaceNodes((xe.Elements().OrderBy(x => x.Name.LocalName).ThenBy(x => (string)x.Attribute(Att)).Union((NodesToBePreserved).OrderBy(P => P.ToString()))).OrderBy(N => N.NodeType.ToString()));
                }
            }
            foreach (XElement xc in xe.Elements())
            {
                SortElement(xc, sortAttributes, sortBySpecificAttributes, FilteredSortingAttibutes);
            }
        }

    }
}
