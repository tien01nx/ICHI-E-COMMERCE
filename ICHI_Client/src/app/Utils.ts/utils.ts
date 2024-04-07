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
  static textPattern1 = /^[a-zA-ZÀ-ỹ\s]*$/;

  static phoneNumberPattern = /^[0-9]{10}$/;

  static numberPattern = /^[0-9]*$/;

  static textPhoneNumber = /^0\d{9}$/;

  static cartList = 'cartList';

  static checkEmail = /^\w+([.-]?\w+)*@\w+([.-]?\w+)*(\.\w{2,3})+$/;

  static onKeyPressNumber(event: KeyboardEvent) {
    const charCode = event.key; // Lấy mã ký tự từ sự kiện
    // Kiểm tra nếu ký tự không phải số (0-9) hoặc không phải phím điều hướng
    if (
      isNaN(Number(charCode)) &&
      !['ArrowLeft', 'ArrowRight', 'Delete', 'Backspace'].includes(charCode)
    ) {
      event.preventDefault(); // Ngăn chặn hành động mặc định của phím
    }
  }

  static onKeyPressText(event: KeyboardEvent) {
    const charCode = event.key; // Lấy mã ký tự từ sự kiện
    const isText = /^[a-zA-Z\s]*$/.test(charCode); // Kiểm tra xem ký tự là văn bản
    // Nếu không phải là văn bản, ngăn chặn hành động mặc định của phím
    if (!isText) {
      event.preventDefault();
    }
  }

  static onKeyPressTextNumberNoSpacebar(event: KeyboardEvent) {
    const charCode = event.key; // Lấy mã ký tự từ sự kiện
    const isAlphanumeric = /^[a-zA-Z0-9]*$/.test(charCode); // Kiểm tra xem ký tự là văn bản hoặc số
    // Nếu không phải là văn bản hoặc số, ngăn chặn hành động mặc định của phím
    if (!isAlphanumeric) {
      event.preventDefault();
    }
  }

  static onKeyPressTextNumber(event: KeyboardEvent) {
    const charCode = event.key; // Lấy mã ký tự từ sự kiện
    const isAllowedChar = /^[a-zA-Z0-9\s]*$/.test(charCode); // Kiểm tra xem ký tự là văn bản, số hoặc khoảng trắng
    // Nếu không phải là văn bản, số hoặc khoảng trắng, ngăn chặn hành động mặc định của phím
    if (!isAllowedChar) {
      event.preventDefault();
    }
  }
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

  // Pending: Đơn hàng chưa được xử lý hoặc chưa được xác nhận.
  // Processing: Đơn hàng đang được xử lý.
  // Shipped: Đơn hàng đã được gửi đi.
  // Delivered: Đơn hàng đã được giao thành công.
  // Cancelled: Đơn hàng đã bị hủy.
  // Refunded: Đơn hàng đã được hoàn lại tiền.
  // On hold: Đơn hàng đang bị tạm dừng.
  // Completed: Đơn hàng đã được hoàn thành.
  // Failed: Đơn hàng thất bại trong quá trình xử lý.
  // Returned: Đơn hàng đã được trả lại.

  // Các loại thanh toán
  static paymentTypes = [
    { paymentTypes: 'PaymentOnDelivery', name: 'Thanh toán khi nhận hàng' },
    { paymentTypes: 'PaymentViaCard', name: 'Thanh toán qua thẻ' },
    { paymentTypes: 'Cash', name: 'Tiền mặt' },
  ];

  static PaymentOnDelivery = 'PaymentOnDelivery';
  static PaymentViaCard = 'PaymentViaCard';

  static statusOrder = [
    { name: 'Pending', value: 'Chưa xác nhận' }, // Chưa xác nhận
    { name: 'Processing', value: 'Đang xử lý' }, // Đang xử lý
    { name: 'Shipped', value: 'Đã vận chuyển' }, // Đã vận chuyển
    { name: 'Delivered', value: 'Đã giao hàng' },
    { name: 'Cancelled', value: 'Đã hủy' },
    { name: 'Refunded', value: 'Đã hoàn lại' },
    { name: 'On hold', value: 'Đang chờ' },
    { name: 'Completed', value: 'Đã hoàn thành' }, // Đã hoàn thành
    { name: 'Failed', value: 'Thất bại' },
    { name: 'Returned', value: 'Đã trả lại' },
  ];

  static paymentStatus = [
    { name: 'Pending', value: 'Chưa thanh toán' },
    { name: 'Approved', value: 'Đã thanh toán' },
    { name: 'Cancelled', value: 'Đã hủy' },
  ];

  static getOrdersStatus(status: string): string {
    const orderStatus = this.statusOrder.find((x) => x.name === status);
    return orderStatus ? orderStatus.value : '';
  }

  static getPaymentStatus(status: string): string {
    const paymentStatus = this.paymentStatus.find((x) => x.name === status);
    return paymentStatus ? paymentStatus.value : '';
  }
  static getPaymentType(paymentType: string): string {
    const paymentTypes = this.paymentTypes.find(
      (x) => x.paymentTypes === paymentType
    );
    return paymentTypes ? paymentTypes.name : '';
  }
}
