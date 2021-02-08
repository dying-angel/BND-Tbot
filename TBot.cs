using System;
using System.IO;
using System.Linq;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.ReplyMarkups;

namespace TGBot
{
    class Program
    {
        static FileInfo fi = new FileInfo("log.txt");
        static TelegramBotClient BND_Bot = new TelegramBotClient(
            "");

        static InlineKeyboardMarkup firstKB = new InlineKeyboardMarkup(new[]
        {
            new[]
            {
                InlineKeyboardButton.WithCallbackData(text: "Информация", callbackData: "Stuff"),
                InlineKeyboardButton.WithCallbackData(text: "Полезное", callbackData: "UsefulStuff"),
                InlineKeyboardButton.WithCallbackData(text: "Справочник", callbackData: "PhoneBook"),
            },
            new[]
            {
                InlineKeyboardButton.WithUrl(text: "Правила", url: "https://telegra.ph/Polnyj-svod-pravil-BND-06-26")
            }
        }
        );

        static InlineKeyboardMarkup InfoKB = new InlineKeyboardMarkup(new[]
        {
            new[]
            {
                InlineKeyboardButton.WithCallbackData(text: "Миграции", callbackData: "Migration"),
                InlineKeyboardButton.WithCallbackData(text: "Звезды замка", callbackData: "Stars"),
                InlineKeyboardButton.WithCallbackData(text: "Герои в бастионах", callbackData: "Darknest"),
            },
            new[]
            {
                InlineKeyboardButton.WithCallbackData(text: "Появление мобов на карте", callbackData: "CurrentMap"),
                InlineKeyboardButton.WithCallbackData(text: "Мобы в лабиринте", callbackData: "Labirynth"),
                InlineKeyboardButton.WithCallbackData(text: "Лут в сокровищнице", callbackData: "Treasure"),
            },
            new[]
            {
                InlineKeyboardButton.WithCallbackData(text: "Донат", callbackData: "Donate"),
            }
        }
        );

        static InlineKeyboardMarkup UsefulKB = new InlineKeyboardMarkup(new[]
        {
            new[]
            {
                InlineKeyboardButton.WithCallbackData(text: "Пачки на мобов", callbackData: "Monster teams"),
                InlineKeyboardButton.WithCallbackData(text: "Гайды", callbackData: "Guides"),
            },
            new[]
            {
                InlineKeyboardButton.WithCallbackData(text: "Расписание охот WHG", callbackData: "Schedule"),
            }
        }
        );

        static InlineKeyboardMarkup DonateKB = new InlineKeyboardMarkup(new[]
        {
            new[]
            {
                InlineKeyboardButton.WithCallbackData(text: "Ветка сигилов", callbackData: "Sigils"),
            },
            new[]
            {
                InlineKeyboardButton.WithCallbackData(text: "Боевые навыки фамильяров", callbackData: "Fams"),
            }
        }
        );

        static InlineKeyboardMarkup GuideKB = new InlineKeyboardMarkup(new[]
        {
            new[]
            {
                InlineKeyboardButton.WithCallbackData(text: "Рыбалка на грифона", callbackData: "Gryphon"),
            },
            new[]
            {
                InlineKeyboardButton.WithCallbackData(text: "Калькулятор", callbackData: "Calc"),
            },
        });


        static void Main(string[] args)
        {
            var bot_check = BND_Bot.GetMeAsync().Result;
            Console.WriteLine($"Bot name: {bot_check.FirstName}\r\nBot's ID: {bot_check.Id}\r\nBot's username: {bot_check.Username}");

            BND_Bot.OnMessage += BND_Bot_OnMessage;
            BND_Bot.OnCallbackQuery += BND_Bot_AnswerCallbackQuery;
            BND_Bot.StartReceiving();

            Console.ReadLine();
            BND_Bot.StopReceiving();
        }

        private static async void BND_Bot_OnMessage(object sender, Telegram.Bot.Args.MessageEventArgs e)
        {
            if (e.Message.Type != Telegram.Bot.Types.Enums.MessageType.Text)
            {
                await BND_Bot.SendTextMessageAsync(
                    chatId: e.Message.Chat,
                    text: "I can handle only text messages.");
                return;
            }

            using (StreamWriter sr = fi.AppendText())
            {
                await sr.WriteAsync($"Message: {e.Message.MessageId}\r\nMessage Type: {e.Message.Type}\r\nFrom user: {e.Message.From}\r\nIn chat: {e.Message.Chat.Id}\r\nChat type: {e.Message.Chat.Type}\r\nDate: {e.Message.Date}\r\nContent: {e.Message.Text}\r\n");
                await sr.WriteAsync("----------------------------------------------\r\n");
            }

            await BND_Bot.SendTextMessageAsync(
                chatId: e.Message.Chat,
                text: "Выбери раздел: ",
                replyMarkup: firstKB);
        }

        private static async void BND_Bot_AnswerCallbackQuery(object User, CallbackQueryEventArgs args)
        {
            var result = args.CallbackQuery.Data;
            var chatID = args.CallbackQuery.Message.Chat.Id;
            using (StreamWriter sr = fi.AppendText())
            {
                await sr.WriteAsync($"Query data: {args.CallbackQuery.Data}\r\nFrom user: {args.CallbackQuery.From}\r\nIn chat: {chatID}\r\nChat type: {args.CallbackQuery.Message.Chat.Type}\r\nDate: {args.CallbackQuery.Message.Date}\r\n");
                await sr.WriteAsync("----------------------------------------------\r\n");
            }

            switch (result)
            {
                case "Stuff":
                    {
                        await BND_Bot.SendTextMessageAsync(chatId: chatID,
                                                           text: "Информация:",
                                                           replyMarkup: InfoKB);
                        await BND_Bot.AnswerCallbackQueryAsync(args.CallbackQuery.Id);

                        return;
                    }

                case "PhoneBook":
                    {

                        await BND_Bot.AnswerCallbackQueryAsync(args.CallbackQuery.Id);
                        return;
                    }
            }
        }
    }
}