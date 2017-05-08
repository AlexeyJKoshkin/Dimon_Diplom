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
                for (int i = 0; i < 15; i++)
                {
                    result.Add(new BaseDataForSelectWindow()
                    {
                        Name = "Dima " +  i.ToString(),
                        AvatarSprite = GameCore.LoadSprite("Sprite"),
                        Price = "Blabala " + i.ToString()
                    });
                }
                
                
                break;
            case MenuItemType.Music:
                break;
            case MenuItemType.Fotographer:
                break;
            case MenuItemType.Decorator:
                break;
            case MenuItemType.Videograph:
                break;
            case MenuItemType.ShowProgramm:
                break;
            default:
                throw new ArgumentOutOfRangeException("type", type, null);
        }
        return result;
    }
}
