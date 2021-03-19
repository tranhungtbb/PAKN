
export class COMMONS {
  constructor() { }

  public static LIST_EMAIL = [
    { text: 'Gmail', value: 1 },
    { text: 'Yahoo', value: 2 },
    { text: 'Hotmail', value: 3 },
  ];

  public static  LIST_TRANG_THAI = [
    { text: 'Hiệu lực', value: true },
    { text: 'Hết hiệu lực', value: false },
  ]

  public static ADD_SUCCESS = "Thêm mới thành công";
  public static DELETE_SUCCESS = "Xóa thành công";
  public static UPDATE_SUCCESS = "Cập nhật thành công";
  public static UNLOCK_SUCCESS = "Mở khóa thành công";
  public static LOCK_SUCCESS = "Khóa thành công";
  public static ADD_FAILED = "Thêm mới thất bại";
  public static DELETE_FAILED = "Xóa thất bại";
  public static UPDATE_FAILED = "Cập nhật thất bại";
  public static PUBLISH_SUCCESS = "Công bố thành công";
  public static WITHDRAW_SUCCESS = "Thu hồi thành công";
}