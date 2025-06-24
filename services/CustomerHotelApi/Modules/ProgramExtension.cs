using System.Globalization;

namespace CustomerHotelApi.Modules;

public static class ProgramExtension
{
    /// <summary>
    /// Cấu hình văn hóa mặc định của ứng dụng là tiếng Việt ("vi-VN") và thiết lập định dạng ngày giờ tùy chỉnh.
    /// Đặt định dạng ngày ngắn là "yyyy-MM-dd" và định dạng giờ dài là "HH:mm:ss".
    /// Áp dụng các thiết lập này cho tất cả các luồng trong ứng dụng.
    /// </summary>
    /// <param name="_">
    /// Đối tượng <see cref="WebApplicationBuilder"/> được mở rộng. Tham số này không được sử dụng.
    /// </param>
    public static void AddCultureInfo(this WebApplicationBuilder _)
    {
        // * Config ISO8601 CultureInfo, với format dd-MM-yyyy HH:mm:ss mặc định
        CultureInfo cultureInfo = new("vi-VN"); // chỉnh lại culture thành vietnam
        cultureInfo.DateTimeFormat.ShortDatePattern = "yyyy-MM-dd"; // định dạng ngày default
        cultureInfo.DateTimeFormat.LongTimePattern = "HH:mm:ss"; // định dạng thời gian default
        CultureInfo.DefaultThreadCurrentCulture = cultureInfo; // chỉnh lại trên source
        CultureInfo.DefaultThreadCurrentUICulture = cultureInfo; // chỉnh lại trên source
    }
}
