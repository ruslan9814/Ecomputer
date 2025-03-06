namespace Infrasctructure.Extensions;

public static class EnumExtensions 
{
    public static string GetName<T>(this T @enum) where T : Enum =>
        Enum.GetName(@enum.GetType(), @enum)!;
}
