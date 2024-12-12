using System.Globalization;

namespace GylleneDroppen.Api.Utilities;

public static class LocalizationHelper
{
    public static string GetLocalizedString(string key, System.Resources.ResourceManager resourceManager, params object[] args)
    {
        var localizedString = resourceManager.GetString(key, CultureInfo.CurrentCulture)
                              ?? resourceManager.GetString(key, new CultureInfo("en"));

        if (localizedString == null)
        {
            return key; 
        }

        try
        {
            return string.Format(localizedString, args);
        }
        catch (FormatException)
        {
            return key;
        }
    }
}