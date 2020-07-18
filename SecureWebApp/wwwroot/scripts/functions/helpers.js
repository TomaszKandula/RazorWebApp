// This module should not manipulate DOM/Virtual DOM

"use strict";


export default class Helpers
{

    ClearSelectElement(ASelectElement)
    {

        let Index, Length = ASelectElement.options.length - 1;

        for (Index = Length; Index >= 0; Index--)
        {
            ASelectElement.remove(Index);
        }

    }

    FormatPhoneNumber(ANumber)
    {
        ANumber = ANumber.replace(/[^\d]+/g, '').replace(/(\d{2})(\d{3})(\d{3})(\d{3})/, '($1) $2 $3 $4');

        if (isEmpty(ANumber))
        {
            return false;
        }
        else
        {
            return ANumber;
        };

    }

    HasSpecialChar(AText)
    {
        let LFormat = /[!@#$%^&*()_+\-=\[\]{};':"\\|,.<>\/?]/

        if (LFormat.test(AText))
        {
            return true;
        }
        else
        {
            return false;
        }

    }

    HasLowerCase(AText)
    {
        if (AText.toUpperCase() != AText)
        {
            return true;
        }

        return false;
    }

    HasUpperCase(AText)
    {
        if (AText.toLowerCase() != AText)
        {
            return true;
        }

        return false;
    }

    IsEmpty(AValue)
    {
        return typeof AValue === 'string' && !AValue.trim() || typeof AValue === undefined || AValue === null;
    }

    IsNumeric(AValue)
    {
        return !isNaN(parseFloat(AValue)) && isFinite(AValue);
    }

    ValidateEmail(AEmail)
    {
        let LRegex = /\S+@\S+\.\S+/
        return LRegex.test(AEmail);
    }

    ValidatePasswordField(AValue)
    {

        let LCheck = 0;

        if (AValue.length < 8) { LCheck++; };
        if (this.IsEmpty(AValue)) { LCheck++; };
        if (!this.HasLowerCase(AValue)) { LCheck++; };
        if (!this.HasUpperCase(AValue)) { LCheck++; };
        if (!this.HasSpecialChar(AValue)) { LCheck++; };

        if (LCheck != 0)
        {
            return false;
        }
        else
        {
            return true;
        }

    }

}
