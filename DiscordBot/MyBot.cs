using Discord;
//using DiscordBot;
using Discord.Net;
using Discord.Commands;


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YSQ.core.Quotes;
using Discord.Audio;

namespace DiscordBot
{

    class MyBot
    {

        
        DiscordClient discord;
        public MyBot()
        {


            discord = new DiscordClient(x =>
            {
                x.LogLevel = Discord.LogSeverity.Info;
                x.LogHandler = Log;
            });

            discord.UsingCommands(x =>
           {
               x.PrefixChar = '.';
               x.AllowMentionPrefix = true;
           });

            var commands = discord.GetService<CommandService>();

            commands.CreateCommand("type")
                .Do(async (e) =>
                {
                    await e.Channel.SendIsTyping();
                   // await e.Channel.DeleteMessages();
                });

            commands.CreateCommand("poop")
                .Do(async (e) =>
                {
                    await e.Channel.SendMessage("POOP");
                });

            commands.CreateCommand("del")
                .Do(async (e) =>
                {
             
                    Message[] messagestoDelete;
                    messagestoDelete = await e.Channel.DownloadMessages(2);
                    await e.Channel.DeleteMessages(messagestoDelete);
                  
                });

            commands.CreateCommand("triggered")
               .Do(async (e) =>
               {
                   await e.Channel.SendIsTyping();
                   await e.Channel.SendFile("gif/trig.webp");
               });

            commands.CreateCommand("eg")
              .Do(async (e) =>
              {
                  await e.Channel.SendIsTyping();
                  await e.Channel.SendFile("gif/bear.png");
              });

            commands.CreateCommand("stocks")
              .Do(async (e) =>
              {
                  //Create the quote service
                  var quote_service = new QuoteService();

                  //Get a quote
           
                  var quotes = quote_service.Quote("COST","").Return(QuoteReturnParameter.Symbol,
                                                                          QuoteReturnParameter.Name,
                                                                          QuoteReturnParameter.LatestTradePrice,
                                                                          QuoteReturnParameter.ChangeAsPercent,
                                                                          QuoteReturnParameter.LatestTradeTime);
                  //Get info from the quotes
                  foreach (var quote in quotes)
                  {
                      //Console.WriteLine("{0} - {1} - {2} - {3}", quote.Symbol, quote.Name, quote.LatestTradePrice, quote.LatestTradeTime);
                      await e.Channel.SendMessage(quote.Name);
                      await e.Channel.SendMessage(quote.ChangeAsPercent);
                  }
                  
              });
            //Audio Portion//
            discord.UsingAudio(x =>
            {
                x.Mode = AudioMode.Outgoing;
            });

            commands.CreateCommand("joinc")
             .Do(async (e) =>
             {
                 var voiceChannel = discord.FindServers("C++").FirstOrDefault().VoiceChannels.FirstOrDefault(); // Finds the first VoiceChannel on the server 'Music Bot Server'

                 var _vClient = await discord.GetService<AudioService>() // We use GetService to find the AudioService that we installed earlier. In previous versions, this was equivelent to _client.Audio()
                         .Join(voiceChannel); // Join the Voice Channel, and return the IAudioClient.                
             });
            commands.CreateCommand("joine")
            .Do(async (e) =>
            {
                var voiceChannel = discord.FindServers("The Great Eagle Bear").FirstOrDefault().VoiceChannels.FirstOrDefault(); // Finds the first VoiceChannel on the server 'Music Bot Server'

                 var _vClient = await discord.GetService<AudioService>() // We use GetService to find the AudioService that we installed earlier. In previous versions, this was equivelent to _client.Audio()
                        .Join(voiceChannel); // Join the Voice Channel, and return the IAudioClient.                
             });
            commands.CreateCommand("join")
           .Do(async (e) =>
           {
               Channel voiceChan = e.User.VoiceChannel;
               // await voiceChan.JoinAudio();
               var _vClient = await discord.GetService<AudioService>() // We use GetService to find the AudioService that we installed earlier. In previous versions, this was equivelent to _client.Audio()
                       .Join(voiceChan); // Join the Voice Channel, and return the IAudioClient.           
           });

            commands.CreateCommand("leave")
            .Do(async (e) =>
            {
                Channel voiceChan = e.User.VoiceChannel;
                await voiceChan.LeaveAudio();
            });
            commands.CreateCommand("kys")
            .Do(async (e) =>
            {
                await discord.Disconnect();
            });
            /////Audio Portion///

            //GREETING
            discord.GetService<CommandService>().CreateCommand("greet") //create command greet
          .Alias(new string[] { "gr", "hi" }) //add 2 aliases, so it can be run with ~gr and ~hi
          .Description("Greets a person.") //add description, it will be shown when ~help is used
          .Parameter("GreetedPerson", ParameterType.Required) //as an argument, we have a person we want to greet
          .Do(async e =>
            {
              await e.Channel.SendMessage($"{e.User.Name} greets {e.GetArg("GreetedPerson")}");
            //sends a message to channel with the given text
            });
            //GREETING

            //Stocks///////////////////////////////////////////
            discord.GetService<CommandService>().CreateCommand("price") //create command greet
        .Alias(new string[] { "p", "quote" }) //add 2 aliases, so it can be run with ~gr and ~hi
        .Description("Shows stock price.") //add description, it will be shown when ~help is used
        .Parameter("TickerSymbol", ParameterType.Required) //as an argument, we have a person we want to greet
        .Do(async e =>
        {
            //Create the quote service
            var quote_service = new QuoteService();
            //Get a quote
            var quotes = quote_service.Quote($"{e.GetArg("TickerSymbol")}", "").Return(QuoteReturnParameter.Symbol,
                                                                    QuoteReturnParameter.Name,
                                                                    QuoteReturnParameter.LatestTradePrice,
                                                                    QuoteReturnParameter.LatestTradeTime);

            //Get info from the quotes
            
            foreach (var quote in quotes)
           
            {
                //Console.WriteLine("{0} - {1} - {2} - {3}", quote.Symbol, quote.Name, quote.LatestTradePrice, quote.LatestTradeTime);
                await e.Channel.SendMessage(quote.Name);
                await e.Channel.SendMessage("$"+quote.LatestTradePrice);
            }
            //await e.Channel.SendMessage($"{e.User.Name} greets {e.GetArg("GreetedPerson")}");
            //sends a message to channel with the given text
            
        })
        
        ;
            //STOCKS^/////////////////////////////////////

            //FLIP//
            commands.CreateCommand("flipcoin")
              .Do(async (e) =>
              {
                  Random random = new Random();
                  int randomNumber = random.Next(1, 3);
                  if(randomNumber == 1)
                  {
                      await e.Channel.SendMessage("HEADS");
                  }
                 else if(randomNumber == 2)
                  {
                      await e.Channel.SendMessage("TAILS");
                  }
              });
            commands.CreateCommand("flipdie")
             .Do(async (e) =>
             {
                 Random random = new Random();
                 int randomNumber = random.Next(1, 7);
                 await e.Channel.SendMessage("" + randomNumber);
             });


            //CONNECT BOT
            discord.ExecuteAndWait(async () =>
            {
                await discord.Connect("MzM1MjM1NjU2NjAxMDQyOTQ1.DEnZhw.ANvKVSPmrLfEaEwdzhmjdTbVdtE", TokenType.Bot);
            });
        }
        private void Log(object sender, LogMessageEventArgs e)
        {
            Console.WriteLine(e.Message);
        }

    }
}
