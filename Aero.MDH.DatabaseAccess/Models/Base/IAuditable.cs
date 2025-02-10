namespace Aero.MDH.DatabaseAccess.Models.Base;

public interface IAuditable
{
    DateTime AuditCreateDate { get; set; }
    string AuditCreateUser { get; set; }
    DateTime? AuditUpdateDate { get; set; }
    string? AuditUpdateUser { get; set; }
}