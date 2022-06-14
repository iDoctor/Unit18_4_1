using YoutubeExplode;
using YoutubeExplode.Videos.Streams;

// создадим отправителя
var sender = new Sender();

// создадим получателя
var receiver = new Receiver();

// создадим команду
var commandOne = new CommandOne(receiver);

// инициализация команды
sender.SetCommand(commandOne);

//  выполнение
sender.GetInfo("qq");
sender.DownloadVideo("ww");


Console.WriteLine("Введите ссылку на видео с Youtube:");
string link = Console.ReadLine();

var youtube = new YoutubeClient();
var video = await youtube.Videos.GetAsync(link);

while (true)
{
    Console.WriteLine($"{Environment.NewLine}Введите команду:");
    Console.WriteLine("Информация о видео = i");
    Console.WriteLine("Скачивание видео = d");
    string command = Console.ReadLine();

    if (command == "i")
    {
        var videoInfo = $"{video.Author.ChannelTitle} | {video.Title} | {video.Duration}";
        Console.WriteLine($"{Environment.NewLine}Информация о видео:{Environment.NewLine}{videoInfo}");
    }
    else if (command == "d")
    {
        var streamManifest = await youtube.Videos.Streams.GetManifestAsync(link);
        var streamInfo = streamManifest
            .GetVideoOnlyStreams()
            .Where(s => s.Container == Container.Mp4)
            .GetWithHighestVideoQuality();

        await youtube.Videos.Streams.DownloadAsync(streamInfo, $"video.{streamInfo.Container}");

        Console.WriteLine($"{Environment.NewLine}Видео скачено!");
    }
}

/// <summary>
/// Базовый класс команды
/// </summary>
abstract class Command
{
    public abstract void GetInfo(string link);
    public abstract void DownloadVideo(string link);
}

/// <summary>
/// Отправитель команды
/// </summary>
class Sender
{
    Command _command;

    public void SetCommand(Command command)
    {
        _command = command;
    }

    // Получить информацию
    public void GetInfo(string link)
    {
        _command.GetInfo(link);
    }

    // Скачать видео
    public void DownloadVideo(string link)
    {
        _command.DownloadVideo(link);
    }
}

/// <summary>
/// Класс-получатель команды
/// </summary>
class Receiver
{
    public void Operation()
    {
        Console.WriteLine("Процесс запущен");
    }
}

/// <summary>
/// Конкретная реализация команды.
/// </summary>
class CommandOne : Command
{
    Receiver receiver;

    public CommandOne(Receiver receiver)
    {
        this.receiver = receiver;
    }

    public override void GetInfo(string link)
    {
        Console.WriteLine("Команда отправлена [1]");
        receiver.Operation();
    }

    public override void DownloadVideo(string link)
    {
        Console.WriteLine("Команда отправлена [2]");
        receiver.Operation();
    }
}