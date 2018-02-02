using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostAndFoundGUI
{
    class Page
    {
        public Page(string html, string address)
        {
            this.html = html;
            this.address = address;
        }

        private string html;
        private string address;

        public void setHTML(string HTML)
        {
            html = HTML;
        }

        public string getHTML()
        {
            return html;
        }

        public void setAddress(string inAddress)
        {
            address = inAddress;
        }

        public string getAddress()
        {
            return address;
        }
    }
}
