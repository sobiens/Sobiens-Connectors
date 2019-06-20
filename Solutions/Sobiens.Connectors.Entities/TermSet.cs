using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sobiens.Connectors.Entities
{
    public class TermSet
    {
        // TS – Term Set 

        /// a12 –
        public string Name;// a11 – TermSet or Term Description
        public Guid Id;// a9 – Term ID or TermSet ID 
        public bool Open;// a16 – Submission Policy (Is the termset open or closed)
        public bool Enabled;// a17 – Term or TermSet is Enabled (can be used for tagging)
        public string ContactEmail;// a68 – TermSet contact email

        public TermCollection Terms=new TermCollection();

    }

    public class TermCollection : List<Term>
    {
        public List<Term> GetEnableTerms()
        {
            List<Term> terms = new List<Term>();
            foreach (Term term in this)
            {
                if (term.Enabled&&!term.Deprecated)
                    continue;
                terms.Add(term);
            }
            return terms;
        }
    }

    public class Term
    {
        // T – The Term. LS – Label Set TL – Term Label. Potentially a Term can have several labels, for different Languages
        
        public string Name;// T/LS/TL/a32 – Label Text 
        public Guid Id;// T/a9 – Term ID or TermSet ID 
        public string Description;// T/DS/TD/a11 – TermSet or Term Description
        public bool Enabled;// T/TMS/TM/a17 – Term or TermSet is Enabled (can be used for tagging)
        public bool Deprecated;// T/a21 – (boolean) False if the term is not deprecated 
        public Guid ParentTermSetID; // T/TMS/TM/a24 – Term Set ID the Term belongs to 
        public string ParentTermSetName;// T/TMS/TM/a12 – Term Set or Term Name the Term belongs to
        public string ParentTermName;// T/TMS/TM/a40 – Parent Term Name
        public string TermPath;// T/TMS/TM/a45 – A semicolon separated list of the IDs of the Terms to the current term’s location in the taxonomy. (I.e. its path!) 
        public TermCollection Terms;

        public override string ToString()
        {
            return this.Name;
        }

        //"/Sobiens.Connectors.WPF.Controls;component/Images/EMMCopyTerm.png"
        // T/LS/TL/a31 – (boolean) Not sure, I think this is true if it’s the default term label?
        // T/a61 – (integer) ? 
        // T/TMS/TM/a67 – ? 
    }
}
