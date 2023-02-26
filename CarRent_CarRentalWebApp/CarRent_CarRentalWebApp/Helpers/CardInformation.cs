using System.Text.RegularExpressions;

namespace CarRent_CarRentalWebApp.Helpers;
public static class CardInformation
{
    public static bool IsCreditCardInfoValid(string cardNumber, string endDate, string cvc)
    {
        var cardCheck = new Regex(@"^([\-\s]?[0-9]{4}){4}$");
        var monthCheck = new Regex(@"^(0[1-9]|1[0-2])$");
        var yearCheck = new Regex(@"^[0-9]{2}$");
        var cvcCheck = new Regex(@"^\d{3}$");

        if (!cardCheck.IsMatch(cardNumber)) return false;
        if (!cvcCheck.IsMatch(cvc)) return false;

        var dateParts = endDate.Split('/');
        if (!monthCheck.IsMatch(dateParts[0]) || !yearCheck.IsMatch(dateParts[1])) return false;

        return true;
    }
}
