export class Utils {
  static getVisiblePages(
    currentPage: number,
    totalPages: number,
    visibleCount: number
  ): number[] {
    // start = Giá trị nhỏ nhất của trang hiển thị
    let start = Math.max(
      1,
      Math.min(
        currentPage - Math.floor(visibleCount / 2),
        totalPages - visibleCount + 1
      )
    );
    // end = Giá trị lớn nhất của trang hiển thị
    let end = Math.min(totalPages, start + visibleCount - 1);
    return Array.from({ length: end - start + 1 }, (_, index) => start + index);
  }
  static createColorList() {
    return [
      { name: 'Đen' },
      { name: 'Trắng' },
      { name: 'Đỏ' },
      { name: 'Xanh' },
      { name: 'Vàng' },
      { name: 'Hồng' },
      { name: 'Xám' },
      { name: 'Nâu' },
      { name: 'Cam' },
      { name: 'Tím' },
      { name: 'Xanh dương' },
    ];
  }

  static passwordPattern =
    /^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[!@#$%^&*])[A-Za-z\d!@#$%^&*]{8,}$/;

  static textPattern = /^[a-zA-ZÀ-ỹ\s]*$/;

  static phoneNumberPattern = /^[0-9]{10}$/;

  static cartList = 'cartList';

  // Các loại thanh toán
  static paymentTypes = [
    { paymentTypes: 'PaymentOnDelivery', name: 'Thanh toán khi nhận hàng' },
    { paymentTypes: 'PaymentViaCard', name: 'Thanh toán qua thẻ' },
  ];

  static PaymentOnDelivery = 'PaymentOnDelivery';
  static PaymentViaCard = 'PaymentViaCard';
  //   sách https://theme.hstatic.net/1000230347/1000782290/14/menu_icon_8.png?v=20386
  // buts https://theme.hstatic.net/1000230347/1000782290/14/menu_icon_1.png?v=20386
  // dungj cuj hoc tap
  // https://theme.hstatic.net/1000230347/1000782290/14/menu_icon_2.png?v=20386
  // vanw phong
  // https://theme.hstatic.net/1000230347/1000782290/14/menu_icon_9.png?v=20386

  static categories = [
    {
      name: 'Giấy in',
      image:
        'https://theme.hstatic.net/1000230347/1000782290/14/menu_icon_8.png?v=20386',
    },
    {
      name: 'Bút',
      image:
        'https://theme.hstatic.net/1000230347/1000782290/14/menu_icon_1.png?v=20386',
    },
    {
      name: 'Đồ dùng học tập',
      image:
        'https://theme.hstatic.net/1000230347/1000782290/14/menu_icon_2.png?v=20386',
    },
    {
      name: 'Đồ dùng văn phòng',
      image:
        'https://theme.hstatic.net/1000230347/1000782290/14/menu_icon_7.png?v=20386',
    },
    {
      name: 'Thiết bị văn phòng',
      image:
        'https://theme.hstatic.net/1000230347/1000782290/14/menu_icon_6.png?v=20387',
    },
    {
      name: 'Băng keo - Dao - Kéo',
      image:
        'https://theme.hstatic.net/1000230347/1000782290/14/menu_icon_9.png?v=20386',
    },
  ];
}
