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
  static text = /^[^\\d\\W_ ]+$/;
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

  static handleKeyPress(
    event: KeyboardEvent,
    minValue: number,
    maxValue: number
  ) {
    const keyPressed = event.key;

    // Kiểm tra xem phím được nhấn có phải là số từ 0-9 hoặc không
    const isNumberKey = /^\d$/.test(keyPressed);

    if (!isNumberKey) {
      event.preventDefault(); // Ngăn chặn sự kiện keypress nếu không phải là số từ 0-9
      return;
    }

    const currentValue = +(event.target as HTMLInputElement).value;
    const newValue = +(currentValue.toString() + keyPressed);

    // Kiểm tra xem newValue có nằm trong khoảng min và max không
    if (newValue < minValue || newValue > maxValue) {
      event.preventDefault(); // Ngăn chặn sự kiện keypress
    }
  }

  // nguyên dương

  static handleKeyPressNguyen(event: KeyboardEvent) {
    const keyPressed = event.key;

    // Kiểm tra xem phím được nhấn có phải là số từ 0-9 hoặc không
    const isNumberKey = /^\d$/.test(keyPressed);

    if (!isNumberKey) {
      event.preventDefault(); // Ngăn chặn sự kiện keypress nếu không phải là số từ 0-9
      return;
    }

    // Kiểm tra xem giá trị sau khi nhập vào có phải là số nguyên dương hay không
    const currentValue = +(event.target as HTMLInputElement).value;
    const newValue = +(currentValue.toString() + keyPressed);

    if (newValue <= 0) {
      event.preventDefault(); // Ngăn chặn sự kiện keypress nếu không phải là số nguyên dương
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
    { name: 'PAYMENTONDELIVERY', value: 'Thanh toán khi nhận hàng' },
    { name: 'PAYMENTVIACARD', value: 'Thanh toán qua thẻ' },
    { name: 'CASH', value: 'Tiền mặt' },
  ];

  static PaymentOnDelivery = 'PAYMENTONDELIVERY';
  static PaymentViaCard = 'PAYMENTVIACARD';

  // Trạng thái đơn hàng
  // Chưa xác nhận - Pending
  // Chờ xác nhận - On hold
  // Chờ lấy hàng - WaitingForPickup
  // Chờ giao hàng - WaitingForDelivery
  // Đã giao hàng - Delivered
  // Đã hủy - Cancelled
  // sửa thành name có có giá trị viết hoa  hết
  static statusOrder = [
    { name: 'PENDING', value: 'Chưa xác nhận' },
    { name: 'ONHOLD', value: 'Đã xác nhận' },
    { name: 'WAITINGFORPICKUP', value: 'Chờ lấy hàng' },
    { name: 'WAITINGFORDELIVERY', value: 'Chờ giao hàng' },
    { name: 'DELIVERED', value: 'Đã giao hàng' },
    { name: 'CANCELLED', value: 'Đã hủy' },
  ];

  static paymentStatus = [
    { name: 'PENDING', value: 'Chưa thanh toán' },
    { name: 'APPROVED', value: 'Đã thanh toán' },
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
    const paymentTypes = this.paymentTypes.find((x) => x.name === paymentType);
    return paymentTypes ? paymentTypes.value : '';
  }
}
