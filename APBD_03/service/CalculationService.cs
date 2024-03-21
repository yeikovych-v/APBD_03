namespace APBD_03.service;

public static class CalculationService
{
    public static decimal TonsToKg(decimal tons)
    {
        return tons * 1000;
    }

    public static bool IsIdInListRange(int num, int size)
    {
        return num >= 0 && num + 1 <= size;
    }

    public static bool IsInt(string str)
    {
        try
        {
            int.Parse(str);
            return true;
        }
        catch (FormatException)
        {
            return false;
        }
    }
}