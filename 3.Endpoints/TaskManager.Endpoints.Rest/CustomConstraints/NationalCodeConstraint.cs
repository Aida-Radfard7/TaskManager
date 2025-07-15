using System.Text.RegularExpressions;

namespace TaskManager.Endpoints.Rest.CustomConstraints
{
    public class NationalCodeConstraint:IRouteConstraint
    {
        public bool Match(HttpContext? httpContext, IRouter? route, 
                            string value , RouteValueDictionary values,  
                            RouteDirection routeDirection)
        {
            if (!values.TryGetValue(value, out var nationalCode) || value == null)
                return false;

            var code = nationalCode.ToString();

            if (!Regex.IsMatch(code, @"^\d{10}$"))
                return false;

            if (new string(code[0], 10) == code)
                return false;

            var check = int.Parse(code[9].ToString());
            var sum = 0;

            for (int i = 0; i < 9; i++)
            {
                sum += int.Parse(code[i].ToString()) * (10 - i);
            }

            var remainder = sum % 11;
            return (remainder < 2 && check == remainder) || (remainder >= 2 && check == 11 - remainder);
        } 
    
    }
}
 