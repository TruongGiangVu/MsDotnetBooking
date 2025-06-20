using System.Globalization;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Modules;

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


    /// <summary>
    /// Gán một section cấu hình vào một class settings kiểu mạnh và đăng ký nó với service collection.
    /// </summary>
    /// <typeparam name="T">Kiểu class settings để gán. Phải là class có constructor không tham số.</typeparam>
    /// <param name="configuration">Đối tượng cấu hình của ứng dụng.</param>
    /// <param name="services">Service collection để đăng ký cấu hình.</param>
    /// <param name="sectionName">Tên section cấu hình cần gán.</param>
    /// <returns>
    /// Một instance của <typeparamref name="T"/> đã được gán giá trị từ section cấu hình chỉ định,
    /// hoặc <c>null</c> nếu section không tồn tại hoặc không thể gán.
    /// </returns>
    public static T? BindAppsettings<T>(IConfiguration configuration, IServiceCollection services, string? sectionName) where T : class, new()
    {
        IConfigurationSection section = configuration.GetSection(sectionName ?? typeof(T).Name);
        services.Configure<T>(section);
        return section.Get<T>();
    }
    // var appSettings = ConfigurationHelper.BindAppsettings<AppSettings>(builder.Configuration, builder.Services, nameof(AppSettings));

}
