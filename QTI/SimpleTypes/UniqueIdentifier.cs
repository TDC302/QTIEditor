using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;

namespace QTIEditor.QTI.SimpleTypes
{
    /// <summary>
    /// A unique identifier is simply an identifier that is unique within the scope of the instance. An identifier is a string of characters that must start
    /// with a Letter or an underscore ('_') and contain only Letters, underscores, hyphens ('-'), period ('.', a.k.a. full-stop), Digits, CombiningChars and
    /// Extenders. Identifiers containing the period character are reserved for use in prefixing, as described in the definition of variable. The character classes
    /// Letter, Digit, CombiningChar and Extender are defined in the Extensible Markup Language (XML) 1.0 (Second Edition) [XML, 00]. Note particularly that identifiers
    /// may not contain the colon (':') character. Identifiers should have no more than 32 characters for compatibility with version 1. They are always compared case-sensitively.
    /// </summary>
    public class UniqueIdentifier
    {

        static readonly Dictionary<Type, ulong> typeCounters = [];


        public string ID { get; init; }

        public override string ToString()
        {
            return ID;
        }

        internal UniqueIdentifier()
        {
        }


        public UniqueIdentifier(Type self)
        {
            StringBuilder sb = new();
            sb.Append(self.Name + "-");


            bool hasCounter = typeCounters.TryGetValue(self, out ulong counter);

            if (hasCounter)
            {
                typeCounters[self] = ++counter;
            }
            else
            {
                typeCounters.Add(self, counter);
            }

            sb.AppendFormat("{0:x4}", counter);
            ID = sb.ToString();
        }

        public Value ToValue()
        {
            return new Value()
            {
                value = ID
            };
        }




    }
}
