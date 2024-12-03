namespace GylleneDroppen.Api.Exceptions;

public class EmailAlreadyConfirmedException(string email) : Exception($"Email {email} is already confirmed.");
public class ConfirmationLinkAlreadySentException() : Exception($"A confirmation link has already been sent to this email. Please check your inbox.");