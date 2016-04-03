using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
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

namespace websitecsharp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        //gereftane source htmle page ba proxy
        private string GetPageSourcebyproxy(string url,string address,int port,string username,string password )
        {
            string htmlSource = string.Empty;
            try
            {
                System.Net.WebProxy myProxy = new System.Net.WebProxy(address, port);
                using (System.Net.WebClient client = new System.Net.WebClient())
                {
                    client.Proxy = myProxy;
                    client.Proxy.Credentials = new System.Net.NetworkCredential(username, password);
                    htmlSource = client.DownloadString(url);
                }
            }
            catch (WebException ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
            return htmlSource;
        }

        //gereftane source htmle page bedune proxy
        private string GetPageSource(string url)
        {
            string htmlSource = string.Empty;
            try
            {
                using (System.Net.WebClient client = new System.Net.WebClient())
                {
                    htmlSource = client.DownloadString(url);
                }
            }
            catch (WebException ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
            return htmlSource;
        }


        //in tabe tag ha ra hazf mikonad va fagat matne mofide page ra midahad
        string GetPageContent(string source)
        {
            //Regex regexscript = new Regex(@"<script(.*?)</script>");
            Regex regexscript = new Regex(@"<script[^>]*>[\s\S]*?</script>");
            source = regexscript.Replace(source, " ");

            string noHTML = Regex.Replace(source, @"<[^>]+>|&nbsp;", "").Trim();
            string noHTMLNormalised = Regex.Replace(noHTML, @"\s{2,}", " ");

            Regex regalphabetic = new Regex("[^a-zA-Z -]");
            noHTMLNormalised = regalphabetic.Replace(noHTMLNormalised, " ");

            Regex regexspace = new Regex("\\s+");
            noHTMLNormalised = regexspace.Replace(noHTMLNormalised, " ");

            return noHTMLNormalised;
        }

        private void Buttonproxy_OnClick(object sender, RoutedEventArgs e)
        {

            string source1 = GetPageSource(TextBox1.Text);
            string source2 = GetPageSource(TextBox2.Text);


            site1source.Document.Blocks.Add(new Paragraph(new Run(source1)));
            site2source.Document.Blocks.Add(new Paragraph(new Run(source2)));

            string content1 = GetPageContent(source1);
            string content2 = GetPageContent(source2);

            content1=content1.ToLower();
            content2=content2.ToLower();

            site1content.Document.Blocks.Add(new Paragraph(new Run(content1)));
            site2content.Document.Blocks.Add(new Paragraph(new Run(content2)));

            string[] site1 = content1.Split(' ');
            string[] site2 = content2.Split(' ');

            var dic1 = new Dictionary<string, int>();
            var dic2 = new Dictionary<string, int>();

            foreach (var item  in site1)
            {
                if (dic1.ContainsKey(item))
                    dic1[item]++;
                else
                {
                    dic1.Add(item, 0);
                }
            }

            foreach (var item in site2)
            {
                if (dic2.ContainsKey(item))
                    dic2[item]++;
                else
                {
                    dic2.Add(item, 0);
                }
            }

            string temp =
                "a, about, above, across, after, again, against, all, almost, alone, along, already, also, although, always, am, among, an, and, another, any, anybody, anyone, anything, anywhere, are, area, areas, aren't, around, as, ask, asked, asking, asks, at, away, b, back, backed, backing, backs, be, became, because, become, becomes, been, before, began, behind, being, beings, below, best, better, between, big, both, but, by, c, came, can, cannot, can't, case, cases, certain, certainly, clear, clearly, come, could, couldn't, d, did, didn't, differ, different, differently, do, does, doesn't, doing, done, don't, down, downed, downing, downs, during, e, each, early, either, end, ended, ending, ends, enough, even, evenly, ever, every, everybody, everyone, everything, everywhere, f, face, faces, fact, facts, far, felt, few, find, finds, first, for, four, from, full, fully, further, furthered, furthering, furthers, g, gave, general, generally, get, gets, give, given, gives, go, going, good, goods, got, great, greater, greatest, group, grouped, grouping, groups, h, had, hadn't, has, hasn't, have, haven't, having, he, he'd, he'll, her, here, here's, hers, herself, he's, high, higher, highest, him, himself, his, how, however, how's, i, i'd, if, i'll, i'm, important, in, interest, interested, interesting, interests, into, is, isn't, it, its, it's, itself, i've, j, just, k, keep, keeps, kind, knew, know, known, knows, l, large, largely, last, later, latest, least, less, let, lets, let's, like, likely, long, longer, longest, m, made, make, making, man, many, may, me, member, members, men, might, more, most, mostly, mr, mrs, much, must, mustn't, my, myself, n, necessary, need, needed, needing, needs, never, new, newer, newest, next, no, nobody, non, noone, nor, not, nothing, now, nowhere, number, numbers, o, of, off, often, old, older, oldest, on, once, one, only, open, opened, opening, opens, or, order, ordered, ordering, orders, other, others, ought, our, ours, ourselves, out, over, own, p, part, parted, parting, parts, per, perhaps, place, places, point, pointed, pointing, points, possible, present, presented, presenting, presents, problem, problems, put, puts, q, quite, r, rather, really, right, room, rooms, s, said, same, saw, say, says, second, seconds, see, seem, seemed, seeming, seems, sees, several, shall, shan't, she, she'd, she'll, she's, should, shouldn't, show, showed, showing, shows, side, sides, since, small, smaller, smallest, so, some, somebody, someone, something, somewhere, state, states, still, such, sure, t, take, taken, than, that, that's, the, their, theirs, them, themselves, then, there, therefore, there's, these, they, they'd, they'll, they're, they've, thing, things, think, thinks, this, those, though, thought, thoughts, three, through, thus, to, today, together, too, took, toward, turn, turned, turning, turns, two, u, under, until, up, upon, us, use, used, uses, v, very, w, want, wanted, wanting, wants, was, wasn't, way, ways, we, we'd, well, we'll, wells, went, were, we're, weren't, we've, what, what's, when, when's, where, where's, whether, which, while, who, whole, whom, who's, whose, why, why's, will, with, within, without, won't, work, worked, working, works, would, wouldn't, x, y, year, years, yes, yet, you, you'd, you'll, young, younger, youngest, your, you're, yours, yourself, yourselves, you've, z";
            string temp2 = temp.Replace(", ", " ");
            string[] stopwords = temp2.Split(' ');

            foreach (var item in stopwords)
            {
                if (dic1.ContainsKey(item))
                    dic1.Remove(item);
                if (dic2.ContainsKey(item))
                    dic2.Remove(item);
            }


            double d1d2 = 0;
            foreach (var item in dic1)
            {
                if (dic2.ContainsKey(item.Key))
                    d1d2 += item.Value*dic2[item.Key];
            }
            double sumd1 = 0;
            foreach (var item in dic1)
            {
                if (item.Value != 0)
                    sumd1 += item.Value*item.Value;
            }
            double sumd2 = 0;
            foreach (var item in dic2)
            {
                if (item.Value != 0)
                    sumd2 += item.Value*item.Value;
            }
            double d1 = Math.Sqrt(sumd1);
            double d2 = Math.Sqrt(sumd2);

            double cosine = (double)d1d2/(d1*d2);
            cosine *= 100;

            Labelanswer.Content = cosine.ToString();
        }
    }
}
