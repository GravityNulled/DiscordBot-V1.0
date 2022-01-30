using System;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace XLR8_Bot.Core.ModCommands
{
    public class ModCommands : ModuleBase<SocketCommandContext>
    {
        [Command("Kick")]
        [Alias("kick")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task Kickuser(SocketGuildUser user, string Reason)
        {
            var KickDmMessege = await user.GetOrCreateDMChannelAsync();
            var ServerName = Context.Guild.Name;
            await KickDmMessege.SendMessageAsync($"You Have Been Kicked From {ServerName}");

            await user.KickAsync(Reason);
            await Context.Channel.SendMessageAsync(user.Username + " Has Been Kicked");
        }

        [Command("Ban")]
        [Alias("ban")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task Ban(SocketGuildUser user, string Reason = null)
        {
            var Dm = await user.GetOrCreateDMChannelAsync();
            await Dm.SendMessageAsync($"You have been Banned For: {Reason}");
            await user.BanAsync(5, Reason);
            await Context.Channel.SendFileAsync(@"D:\Banned.gif");
        }

        [Command("Add")]
        [Alias("add")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task Add(IRole role, IGuildUser user)
        {
            var msg = await Context.Channel.SendMessageAsync("Adding Roles");
            await user.AddRoleAsync(role);
            var msg2 = await Context.Channel.SendMessageAsync("Roles Added :cookie:");
        }

        [Command("UnBan")]
        [Alias("unban")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task Unban(IUser userId)
        {
            var DmChannel = await userId.GetOrCreateDMChannelAsync();
            var ServerName = Context.Guild.Name;
            await Context.Guild.RemoveBanAsync(userId);
            await DmChannel.SendMessageAsync($"The Managers of Server: {ServerName} have Unbanned You :smiley:");
        }

        [RequireUserPermission(GuildPermission.ManageMessages)]
        [Command("Purge")]
        [Alias("Delete", "delete", "purge")]
        [Summary("Deletes Amount of Messeges")]
        public async Task Purge(int Amount)
        {
            var UserName = Context.User.Username;
            var messages = await Context.Channel.GetMessagesAsync(Amount).FlattenAsync();
            await ((SocketTextChannel)Context.Channel).DeleteMessagesAsync(messages);

            //var messages = await Context.Channel.GetMessagesAsync(Amount).FlattenAsync();
            //foreach (var message in messages)
            //{
            //    var s = message.Id;
            //    await Context.Channel.DeleteMessageAsync(s);
            //}
            var STD = await Context.Channel.SendMessageAsync($"{UserName} Deleted {Amount} Messeges");
            Thread.Sleep(2000);
            await Context.Channel.DeleteMessageAsync(STD);
        }

       

        [Command("Remove")]
        [Alias("remove")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task Rrl(IRole role, SocketGuildUser user)
        {
            await user.RemoveRoleAsync(role);
            await Context.Channel.SendMessageAsync("Roles Removed");
        }


    }
}