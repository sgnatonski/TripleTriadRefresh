using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Xml.Linq;
using TripleTriadRefresh.Server.Models.System;

namespace TripleTriadRefresh.Server.Models
{
    public class CardImage
    {
        private static readonly Dictionary<int, CardMetadata> cardMap = new Dictionary<int, CardMetadata>();

        public static void Initialize()
        {
            var appPath = HttpContext.Current.Server.MapPath("~/Images/Cards/");

            var files = Directory.GetFiles(appPath, "*.png", SearchOption.TopDirectoryOnly);

            var index = 0;
            var document = XDocument.Load(HttpContext.Current.Server.MapPath("~/Data/Cards/cards.xml"));
            if (document.Root != null)
            {
                var xmlCards = document.Root.Elements("card").Where(x => x != null);
                foreach (var xmlCard in xmlCards)
                {
                    var cardname = xmlCard.Attribute("name").Value;
                    var card = new CardMetadata();

                    var img = files.Select(Path.GetFileName).Where(fn => fn != null && fn.EndsWith(cardname)).First();
                    card.Image = Path.Combine(VirtualPathUtility.ToAbsolute("~/Images/Cards/"), img);
                    card.Level = int.Parse(xmlCard.Attribute("lvl").Value);
                    card.Strength = new CardStrength(int.Parse(xmlCard.Attribute("top").Value) ,
                        int.Parse(xmlCard.Attribute("right").Value) ,
                        int.Parse(xmlCard.Attribute("bottom").Value) ,
                        int.Parse(xmlCard.Attribute("left").Value));
                    card.Elemental = (Elemental)Enum.Parse(typeof(Elemental), xmlCard.Attribute("element").Value);
                    cardMap.Add(++index, card);
                }
            }
        }

        public static string GetImagePath(int id)
        {
            return cardMap[id].Image;
        }

        public static CardStrength GetStrength(int id)
        {
            return cardMap[id].Strength;
        }

        public static int[] GetUpToStrength(int strength)
        {
            return cardMap.Where(c => c.Value.Strength.ToArray().Average() <= strength).Select(c => c.Key).ToArray();
        }

        public static Elemental GetElemental(int id)
        {
            return cardMap[id].Elemental;
        }
    }
}