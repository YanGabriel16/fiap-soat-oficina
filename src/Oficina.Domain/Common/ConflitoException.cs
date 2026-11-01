namespace Oficina.Domain.Common;

public sealed class ConflitoException(string message) : Exception(message);
