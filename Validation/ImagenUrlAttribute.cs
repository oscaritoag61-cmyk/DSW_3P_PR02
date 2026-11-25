using System.ComponentModel.DataAnnotations;

namespace DSW_3P_PR02.Validation;

/// <summary>
/// Valida que el valor sea una URL absoluta con esquema HTTP o HTTPS.
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class ImagenUrlAttribute : ValidationAttribute
{
    public ImagenUrlAttribute()
        : base("ImagenUrl debe ser una URL absoluta válida (http o https).") { }

    public override bool IsValid(object? value)
    {
        if (value is not string texto || string.IsNullOrWhiteSpace(texto))
            return false;

        return Uri.TryCreate(texto.Trim(), UriKind.Absolute, out var uri)
               && (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps)
               && !string.IsNullOrEmpty(uri.Host);
    }
}
