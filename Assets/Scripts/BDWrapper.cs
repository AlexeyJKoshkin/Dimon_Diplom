using System;
using System.Collections.Generic;
using ShutEye.Core;

public static class BDWrapper
{
    public static IList<BaseDataForSelectWindow> GetAllInfoAbout(MenuItemType type)
    {
        List<BaseDataForSelectWindow> result = new List<BaseDataForSelectWindow>();

        switch (type)
        {
            case MenuItemType.Leading:
                result.Add(new BaseDataForSelectWindow()
                {
                   Id= 1,
                    Name = "Дмитрий Кошкин",
                    AvatarSprite = GameCore.LoadSprite(@"Iconc\avatar_leading\Koshkin"),
                    Info = "Blabala"
                });
                result.Add(new BaseDataForSelectWindow()
                {
                    Id= 2,
                    Name = "Владимир Пахомов",
                    AvatarSprite = GameCore.LoadSprite(@"Iconc\avatar_leading\Pakhomov"),
                    Info = "Blabala"
                });
                result.Add(new BaseDataForSelectWindow()
                {
                    Id= 3,
                    Name = "Максим Лобов",
                    AvatarSprite = GameCore.LoadSprite(@"Iconc\avatar_leading\Lobov"),
                    Info = "Blabala"
                });
                break;
            case MenuItemType.Music:
                result.Add(new BaseDataForSelectWindow()
                {
                    Id= 1,
                    Name = "French",
                    AvatarSprite = GameCore.LoadSprite(@"Iconc\avatar_music\French"),
                    Info = "Blabala"
                });
                result.Add(new BaseDataForSelectWindow()
                {
                    Id= 2,
                    Name = "Dj_Lime",
                    AvatarSprite = GameCore.LoadSprite(@"Iconc\avatar_music\Dj_Lime"),
                    Info = "Blabala"
                });
                result.Add(new BaseDataForSelectWindow()
                {
                    Id= 3,
                    Name = "Hobots",
                    AvatarSprite = GameCore.LoadSprite(@"Iconc\avatar_music\Hobots"),
                    Info = "Blabala"
                });
                break;
            case MenuItemType.Fotographer:
                result.Add(new BaseDataForSelectWindow()
                {
                    Id= 1,
                    Name = "Вероника Чернявская",
                    AvatarSprite = GameCore.LoadSprite(@"Iconc\avatar_Fotographer\Chernyavskaya"),
                    Info = "Blabala"
                });
                result.Add(new BaseDataForSelectWindow()
                {
                    Id= 2,
                    Name = "Христя Мармонов",
                    AvatarSprite = GameCore.LoadSprite(@"Iconc\avatar_Fotographer\Marmarov"),
                    Info = "Blabala"
                });
                result.Add(new BaseDataForSelectWindow()
                {
                    Id= 3,
                    Name = "Александр Сеоев",
                    AvatarSprite = GameCore.LoadSprite(@"Iconc\avatar_Fotographer\Seoev"),
                    Info = "Blabala"
                });
                break;
            case MenuItemType.Decorator:
                result.Add(new BaseDataForSelectWindow()
                {
                    Id= 1,
                    Name = "Сад",
                    AvatarSprite = GameCore.LoadSprite(@"Iconc\avatar_Decorator\Sad"),
                    Info = "Blabala"
                });
                result.Add(new BaseDataForSelectWindow()
                {
                    Id= 2,
                    Name = "Пронина Анастасия ",
                    AvatarSprite = GameCore.LoadSprite(@"Iconc\avatar_Decorator\Pronina"),
                    Info = "Blabala"
                });
                result.Add(new BaseDataForSelectWindow()
                {
                    Id= 3,
                    Name = "Чудное Мгновение",
                    AvatarSprite = GameCore.LoadSprite(@"Iconc\avatar_Decorator\Mgnovenie"),
                    Info = "Blabala"
                });
                break;
            case MenuItemType.Videograph:
                result.Add(new BaseDataForSelectWindow()
                {
                    Id = 1,
                    Name = "Алексей Иванов",
                    AvatarSprite = GameCore.LoadSprite(@"Iconc\avatar_Videograph\Ivanov"),
                    Info = "Blabala"
                });
                result.Add(new BaseDataForSelectWindow()
                {
                    Id = 2,
                    Name = "Артем Дрягин",
                    AvatarSprite = GameCore.LoadSprite(@"Iconc\avatar_Videograph\Draygin"),
                    Info = "Blabala"
                });
                result.Add(new BaseDataForSelectWindow()
                {
                    Id = 3,
                    Name = "Сергей Студеникин",
                    AvatarSprite = GameCore.LoadSprite(@"Iconc\avatar_Videograph\Studenikin"),
                    Info = "Blabala"
                });
                break;
            case MenuItemType.ShowProgramm:
                result.Add(new BaseDataForSelectWindow()
                {
                    Id = 1,
                    Name = "Старт Наука",
                    AvatarSprite = GameCore.LoadSprite(@"Iconc\avatar_ShowProgramm\Start"),
                    Info = "Blabala"
                });
                result.Add(new BaseDataForSelectWindow()
                {
                    Id = 2,
                    Name = "Арт-Бар",
                    AvatarSprite = GameCore.LoadSprite(@"Iconc\avatar_ShowProgramm\ArtBar"),
                    Info = "Blabala"
                });
                result.Add(new BaseDataForSelectWindow()
                {
                    Id = 3,
                    Name = "Бар-Молекула",
                    AvatarSprite = GameCore.LoadSprite(@"Iconc\avatar_ShowProgramm\Molecula"),
                    Info = "Blabala"
                });
                break;
            default:
                throw new ArgumentOutOfRangeException("type", type, null);
        }
        return result;
    }
}
