using App.Core.Enums;

namespace App.Core.Results;
public class RegistrationResult
{
    public bool IsSuccess { get; set; }
    public RegistrationError Error { get; set; }
}
