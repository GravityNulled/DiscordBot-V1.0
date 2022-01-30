using System;
using System.Linq;
using System.Net;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Newtonsoft.Json;

namespace XLR8_Bot.Core.Commands
{
    public class Commands : ModuleBase<SocketCommandContext>
    {
        [Command("chat")]
        [Alias("talk")]
        public async Task TalkToBot([Remainder]string messege)
        {
            
            var encode = WebUtility.UrlDecode(messege);
            Console.WriteLine(encode);
            string response;
            using (var web = new WebClient())
            {
                response = web.DownloadString($"https://some-random-api.ml/chatbot/?message={encode}");
            }

            var json = JsonConvert.DeserializeObject<dynamic>(response);
            var UserName = Context.User.Username;
            string ResponseMessege = json.response;
            await Context.Channel.SendMessageAsync($"{UserName}: {messege}");
            await Context.Channel.SendMessageAsync($"Bot: {ResponseMessege}");
            
        }
        

        [Command("whois")]
        [Alias("user")]
        public async Task Whois(IGuildUser user)
        {
            string s;
            var UserNicks = user.Nickname;
            if (UserNicks != "")
            {
                s = UserNicks;
            }

            s = UserNicks;
            var CreatedAt = user.CreatedAt;
            var Avatar = user.GetAvatarUrl();
            var DateJoined = user.JoinedAt;
            var Status = user.Status;
            var UserID = user.Id;
            var Roles = user.Guild.Roles.Count;
            var name = user.Username;

            var embed = new EmbedBuilder();
            embed.WithThumbnailUrl(Avatar);
            embed.WithColor(0, 122, 204);
            embed.WithAuthor(Context.User.Username);
            embed.AddField("Name", name);
            embed.AddField("NickName", s);
            embed.AddField("UserID", UserID);
            embed.AddField("DateJoined", DateJoined);
            embed.AddField("CreatedAt", CreatedAt);
            embed.AddField("Roles", Roles);
            embed.AddField("Status", Status);
            await Context.Channel.SendMessageAsync("", false, embed.Build());
        }

        [Command("meme")]
        [Alias("Meme", "Plsmeme")]
        [Summary("Sends a random meme")]
        public async Task Meme()
        {
            string json;
            using (var cl = new WebClient())
            {
                json = cl.DownloadString("https://some-random-api.ml/meme");
            }

            var Data = JsonConvert.DeserializeObject<dynamic>(json);
            string Url = Data.url;
            string Text = Data.text;
            await Context.Channel.SendMessageAsync(Text);
            await Context.Channel.SendMessageAsync(Url);
        }

        [Command("Person")]
        [Alias("person")]
        public async Task Person()
        {
            string response;
            using (var web = new WebClient())
            {
                response = web.DownloadString("https://randomuser.me/api/");
            }

            var json = JsonConvert.DeserializeObject<dynamic>(response);
            string FirstName = json.results[0].name.first;
            string LastName = json.results[0].name.last;
            string location = json.results[0].location.street;
            string city = json.results[0].location.city;
            string state = json.results[0].location.state;
            string postcode = json.results[0].location.postcode;
            string picture = json.results[0].picture.large;
            var embed = new EmbedBuilder();
            embed.WithColor(Color.Blue);
            embed.AddField("FirstName", FirstName);
            embed.AddField("LastName", LastName);
            embed.AddField("location", location);
            embed.AddField("city", city);
            embed.AddField("state", state);
            embed.AddField("postcode", postcode);
            embed.WithThumbnailUrl(picture);
            await Context.Channel.SendMessageAsync("", false, embed.Build());
        }

        [Command("lyrics")]
        [Alias("Lyrics")]
        [Summary("Returns Lyrics of a given Song")]
        public async Task Lyrics(string song)
        {
            string response;
            using (var webClient = new WebClient())
            {
                response = webClient.DownloadString($"https://some-random-api.ml/lyrics?title={song}");
            }

            var Data = JsonConvert.DeserializeObject<dynamic>(response);
            string title = Data.title;
            string author = Data.author;
            string lyrics = Data.links.genius;
            string thumbnail = Data.thumbnail.genius;
            var embed = new EmbedBuilder();
            embed.WithColor(154, 0, 137);
            embed.AddField("Title", title);
            embed.AddField("Author", author);
            embed.WithThumbnailUrl(thumbnail);
            embed.AddField("Lyrics Link", lyrics);
            await Context.Channel.SendMessageAsync("", false, embed.Build());
        }

        [Command("fact")]
        [Alias("Fact")]
        public async Task Fact()
        {
            string downloaded;
            using (var cl = new WebClient())
            {
                downloaded = cl.DownloadString("http://numbersapi.com/random/year?json");
            }

            var json = JsonConvert.DeserializeObject<dynamic>(downloaded);
            string text = json.text;

            var embed = new EmbedBuilder();
            embed.AddField("Fact", text);
            embed.WithColor(Color.Blue);
            await Context.Channel.SendMessageAsync("", false, embed.Build());
        }

        [Command("ip")]
        [Alias("Ip")]
        public async Task GetIP(string ip)
        {
            string response;
            using (var web = new WebClient())
            {
                response = web.DownloadString(
                    $"https://geo.ipify.org/api/v1?apiKey=at_g1NDYmmsAB9GaiN4NsIukUa71pbAD&ipAddress={ip}");
            }

            var json = JsonConvert.DeserializeObject<dynamic>(response);
            string Country = json.location.country;
            string Region = json.location.region;
            string City = json.location.city;
            string Postal_Code = json.location.postalCode;
            if (Postal_Code == "") Postal_Code = "Null";
            var embed = new EmbedBuilder();
            embed.WithColor(Color.Blue);
            embed.AddField("Country", Country);
            embed.AddField("Region", Region);
            embed.AddField("City", City);
            embed.AddField("Postal_Code", Postal_Code);
            await Context.Channel.SendMessageAsync("", false, embed.Build());
        }

        [Command("Trivia")]
        [Alias("trivia")]
        public async Task Trivia()
        {
            string Response;
            using (var Webclient = new WebClient())
            {
                Response = Webclient.DownloadString(
                    "https://opentdb.com/api.php?amount=1&type=boolean&encode=base64");
            }

            var json = JsonConvert.DeserializeObject<dynamic>(Response);
            string category = json.results[0].category;
            string difficulty = json.results[0].difficulty;
            string question = json.results[0].question;
            string correct_answer = json.results[0].correct_answer;
            var BytesCat = Convert.FromBase64String(category);
            var catg = Encoding.UTF8.GetString(BytesCat);
            var BytesDif = Convert.FromBase64String(difficulty);
            var Diff = Encoding.UTF8.GetString(BytesDif);
            var BytesQuestion = Convert.FromBase64String(question);
            var Quiz = Encoding.UTF8.GetString(BytesQuestion);
            var BytesAnswer = Convert.FromBase64String(correct_answer);
            var Answer = Encoding.UTF8.GetString(BytesAnswer);
            var embed = new EmbedBuilder();
            embed.AddField("Category", catg);
            embed.AddField("Difficulty", Diff);
            embed.AddField("Question", Quiz);
            embed.AddField("Correct Answer", Answer);
            embed.WithColor(154, 0, 137);
            await Context.Channel.SendMessageAsync("", false, embed.Build());
        }

        [Command("help"), Alias("Help")]
        public async Task help()
        {
            EmbedBuilder Embed = new EmbedBuilder();
            Embed.AddField("chat | talk", "Talk to A.I Robot");
            Embed.AddField("Meme | meme", "Random Weird Memes :cry:");
            Embed.AddField("Help | help", "Access Help Menu");
            Embed.AddField("whois | user", "Who is the Tagged Person");
            Embed.AddField("Urban | dick | urban", "Gets the meaning of the word from urban disctionary");
            Embed.AddField("fact | Fact ", "Random Fact");
            Embed.AddField("Lyrics | lyrics", "Genius.com Lyrics Link (Discord is Gay!)");
            Embed.AddField("Trivia | trivia", "Random Trivia");
            Embed.AddField("`` Mod | Admin Commands``", "``--------------------------``");
            Embed.AddField("kick | Kick", "Kick a user of the server");
            Embed.AddField("Ban | ban", "Ban a user from the Server");
            Embed.AddField("Add | add", "Give Role to a user");
            Embed.AddField("Uban | uban", "Uban a user(Requires user Id)");
            Embed.AddField("Purge | Delete", "Delete a specific ammount of messeges (Max 100)");
            Embed.AddField("Remove | remove", "Remove User Roles");
            Embed.WithColor(Color.Blue);
            Embed.WithFooter("Made by One#7878");
            Embed.WithTitle("Commands Available Still on Beta Testing");
            var GuildIcon = Context.Guild.IconUrl;
            Embed.WithThumbnailUrl(GuildIcon);
            await Context.Channel.SendMessageAsync("", false, Embed.Build());
        }

        [Command("urban")]
        [Alias("dic", "Urban")]
        public async Task Ubran(string KeyWord)
        {
            string Response;
            using (var WebClient = new WebClient())
            {
                Response = WebClient.DownloadString($"http://api.urbandictionary.com/v0/define?term={KeyWord}");
            }

            var Json = JsonConvert.DeserializeObject<dynamic>(Response);
            var Lis = Json.list[0];
            string definition = Lis.definition;
            string example = Lis.example;
            EmbedBuilder embed = new EmbedBuilder();
            embed.AddField("Defination", definition);
            embed.AddField("Example", example);
            embed.WithColor(0, 122, 204);
            await ReplyAsync("", false, embed.Build());
        }
    }
    
}