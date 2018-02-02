using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Web;

namespace LostAndFoundGUI
{
    public partial class MainWindow : Window
    {
        private static string locationUrl = "http://sacramento.craigslist.org";

        private static List<string> allLinks = new List<string>();
        private static List<string> goodLinks = new List<string>();
        private static List<string> searchLinks = new List<string>();
        private static List<string> toParseText = new List<string>();
        private static List<string> queryList = new List<string>();
        private static List<Page> pageList = new List<Page>();

        public static ItemType itemType;

        public enum ItemType
        {
            antiques, appliances, arts_and_crafts, ATV_UTV_SNO, baby_kid, beauty_health,
            bikes, boats, books, buisness, cds_dvd_vhs, cell_phones, chothes_accesories,
            collectables, computers, computer_parts, electronics, farm_garden, furniture, general, heavy_equipment,
            household, jewelry, materials, motorcycles, music_industry, photo_video, rvs_camp,
            sporting, tickets, tools, toys_games, video_gaming
        }

        public MainWindow()
        {
            InitializeComponent();
        }

        public static void startScraping()
        {
            var webGet = new HtmlWeb();
            string test = getCLPath(locationUrl, itemType);
            var document = webGet.Load(getCLPath(locationUrl, itemType));

            var linksOnPage = from links in document.DocumentNode.Descendants()
                              where links.Name == "a" && links.Attributes["href"] != null && links.InnerText.Trim().Length > 0
                              select links.Attributes["href"].Value;

            allLinks = linksOnPage.ToList();

            foreach(string scrapedURL in allLinks)
            {
                if (scrapedURL.Contains("search")) searchLinks.Add(scrapedURL);
                else if(!scrapedURL.Contains("about") && !scrapedURL.Contains("#") && !scrapedURL.Contains("post") && !scrapedURL.Contains("account") && !scrapedURL.Contains("forum"))
                {
                    goodLinks.Add(locationUrl + scrapedURL);
                }
            }
            
            goodLinks = goodLinks.Distinct().ToList();
            goodLinks.RemoveAt(0);
            goodLinks.RemoveAt(0);
            searchLinks = searchLinks.Distinct().ToList();

            foreach (string goodURL in goodLinks)
            {
                scrapePage(goodURL);
            }
        }

        public static void scrapePage(string url)
        {
            WebClient wc = new WebClient();

            List<string> tempLinks = new List<string>();
            List<string> tempText = new List<string>();

            try
            {
                var page = wc.DownloadString(url);
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(page);

                Page p = new Page(doc.DocumentNode.OuterHtml, url);

                pageList.Add(p);
            }
            catch (Exception e)
            {
            
            }
        }

        public static string getCLPath(string baseURL, ItemType itemtype)
        {
            if (itemtype == ItemType.antiques)
            {
                return baseURL + "/search/ata";
            }
            if (itemtype == ItemType.appliances)
            {
                return baseURL + "/search/ppa";
            }
            if (itemtype == ItemType.arts_and_crafts)
            {
                return baseURL + "/search/ara";
            }
            if (itemtype == ItemType.ATV_UTV_SNO)
            {
                return baseURL + "/search/sna";
            }
            if (itemtype == ItemType.baby_kid)
            {
                return baseURL + "/search/baa";
            }
            if (itemtype == ItemType.beauty_health)
            {
                return baseURL + "/search/haa";
            }
            if (itemtype == ItemType.bikes)
            {
                return baseURL + "/search/bik";
            }
            if (itemtype == ItemType.boats)
            {
                return baseURL + "/search/boa";
            }
            if (itemtype == ItemType.books)
            {
                return baseURL + "/search/bka";
            }
            if (itemtype == ItemType.cds_dvd_vhs)
            {
                return baseURL + "/search/ema";
            }
            if (itemtype == ItemType.cell_phones)
            {
                return baseURL + "/search/moa";
            }
            if (itemtype == ItemType.chothes_accesories)
            {
                return baseURL + "/search/cla";
            }
            if (itemtype == ItemType.collectables)
            {
                return baseURL + "/search/cba";
            }
            if (itemtype == ItemType.computer_parts)
            {
                return baseURL + "/search/sop";
            }
            if (itemtype == ItemType.computers)
            {
                return baseURL + "/search/sys";
            }
            if (itemtype == ItemType.electronics)
            {
                return baseURL + "/search/ela";
            }
            if (itemtype == ItemType.farm_garden)
            {
                return baseURL + "/search/gra";
            }
            if (itemtype == ItemType.furniture)
            {
                return baseURL + "/search/fua";
            }
            if (itemtype == ItemType.general)
            {
                return baseURL + "/search/foa";
            }
            if (itemtype == ItemType.heavy_equipment)
            {
                return baseURL + "/search/hva";
            }
            if (itemtype == ItemType.household)
            {
                return baseURL + "/search/hsa";
            }
            if (itemtype == ItemType.jewelry)
            {
                return baseURL + "/search/jwa";
            }
            if (itemtype == ItemType.materials)
            {
                return baseURL + "/search/maa";
            }
            if (itemtype == ItemType.motorcycles)
            {
                return baseURL + "/search/mcy";
            }
            if (itemtype == ItemType.music_industry)
            {
                return baseURL + "/search/msa";
            }
            if (itemtype == ItemType.photo_video)
            {
                return baseURL + "/search/pha";
            }
            if (itemtype == ItemType.rvs_camp)
            {
                return baseURL + "/search/rva";
            }
            if (itemtype == ItemType.sporting)
            {
                return baseURL + "/search/sga";
            }
            if (itemtype == ItemType.tickets)
            {
                return baseURL + "/search/tia";
            }
            if (itemtype == ItemType.tools)
            {
                return baseURL + "/search/tla";
            }
            if (itemtype == ItemType.toys_games)
            {
                return baseURL + "/search/taa";
            }
            if (itemtype == ItemType.video_gaming)
            {
                return baseURL + "/search/vga";
            }
            return null;
        }

        private void ComboBoxCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem typeItem = (ComboBoxItem)categoryComboBox.SelectedItem;
            string value = typeItem.Content.ToString();
            ParseAndAssignComboBoxItemValue(value);
        }

        private void ParseAndAssignComboBoxItemValue(string value)
        {
            if (value.Contains("Antiqu")) itemType = ItemType.antiques;
            if (value.Contains("Applian")) itemType = ItemType.appliances;
            if (value.Contains("Crafts")) itemType = ItemType.arts_and_crafts;
            if (value.Contains("ATVs")) itemType = ItemType.ATV_UTV_SNO;
            if (value.Contains("Baby a")) itemType = ItemType.baby_kid;
            if (value.Contains("Beauty")) itemType = ItemType.beauty_health;
            if (value.Contains("Bikes")) itemType = ItemType.bikes;
            if (value.Contains("Boats")) itemType = ItemType.boats;
            if (value.Contains("Books")) itemType = ItemType.books;
            if (value.Contains("Buisn")) itemType = ItemType.buisness;
            if (value.Contains("CDs,")) itemType = ItemType.cds_dvd_vhs;
            if (value.Contains("Cell P")) itemType = ItemType.cell_phones;
            if (value.Contains("Clothes")) itemType = ItemType.chothes_accesories;
            if (value.Contains("Collecta")) itemType = ItemType.collectables;
            if (value.Contains("Computers")) itemType = ItemType.computers;
            if (value.Contains("Computer P")) itemType = ItemType.computer_parts;
            if (value.Contains("Electronics")) itemType = ItemType.electronics;
            if (value.Contains("Garden")) itemType = ItemType.farm_garden;
            if (value.Contains("Furnit")) itemType = ItemType.furniture;
            if (value.Contains("General")) itemType = ItemType.general;
            if (value.Contains("Heavy Eq")) itemType = ItemType.heavy_equipment;
            if (value.Contains("Househo")) itemType = ItemType.household;
            if (value.Contains("Jewe")) itemType = ItemType.jewelry;
            if (value.Contains("Material")) itemType = ItemType.materials;
            if (value.Contains("Motorcy")) itemType = ItemType.motorcycles;
            if (value.Contains("Music In")) itemType = ItemType.music_industry;
            if (value.Contains("Photo a")) itemType = ItemType.photo_video;
            if (value.Contains("Camping")) itemType = ItemType.rvs_camp;
            if (value.Contains("Sporting")) itemType = ItemType.sporting;
            if (value.Contains("Tickets")) itemType = ItemType.tickets;
            if (value.Contains("Tools")) itemType = ItemType.tools;
            if (value.Contains("Toys and")) itemType = ItemType.toys_games;
            if (value.Contains("Video Gam")) itemType = ItemType.video_gaming;
        }

        private void searchButton_Click(object sender, RoutedEventArgs e)
        {
            getUserQueryInput();
            startScraping();
            compareUserInputList();
            Console.WriteLine("DONE");
        }

        private void getUserQueryInput()
        {
            string input = queryTextBox.Text;
            queryList = input.Split(',').AsEnumerable().ToList();
        }

        private void compareUserInputList()
        {
            resultsTextBox.Text = "";
            foreach (Page p in pageList)
            {
                string temp = "";
                foreach(string s in queryList)
                {
                    if(p.getHTML().Contains(s))
                    {
                        temp += s + ", ";
                    }
                }
                if(temp.Length > 2)
                {
                    resultsTextBox.Text += p.getAddress() + " contains your keyword(s): " + temp + "\n";
                }
            }
            pageList.Clear();
        }
    }
}