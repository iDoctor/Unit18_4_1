using YoutubeExplode;
using YoutubeExplode.Videos.Streams;

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