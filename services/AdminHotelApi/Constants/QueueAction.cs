using System.ComponentModel.DataAnnotations;

namespace AdminHotelApi.Constants;

/// <summary> Mã lỗi </summary>
public enum QueueAction
{
    [Display(Name = "None")] None = 0,
    [Display(Name = "Tạo")] Create = 1,
    [Display(Name = "Cập nhật")] Update = 2,
    [Display(Name = "Xóa")] Delete = 3,
}
