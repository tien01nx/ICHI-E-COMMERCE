import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Title } from '@angular/platform-browser';
import { ToastrService } from 'ngx-toastr';
import { TrxTransactionService } from '../../../../service/trx-transaction.service';
import { ShoppingCartDTO } from '../../../../dtos/shopping.cart.dto.';
import { Utils } from '../../../../Utils.ts/utils';

@Component({
  selector: 'app-order-notification',
  templateUrl: './order-notification.component.html',
  styleUrl: './order-notification.component.css',
})
export class OrderNotificationComponent implements OnInit {
  orderId: number = 0;
  shoppingcartdto!: ShoppingCartDTO;

  isSuccessful: boolean = true;

  dataOrder: any;
  titleNotification: string = 'Đặt hàng thành công';
  descriptionNotification: string =
    'Cảm ơn bạn đã lựa chọn mua hàng tại Văn phòng phẩm ICHI.';

  constructor(
    private trxTransaction: TrxTransactionService,
    private title: Title,
    private router: Router,
    private toastr: ToastrService,
    private activatedRoute: ActivatedRoute
  ) {}

  ngOnInit(): void {
    this.title.setTitle('Đặt hàng thành công');
    this.orderId = this.activatedRoute.snapshot.params['id'];
    this.findOrderById();
  }

  // orderStatus:0 Trạng thái đơn hàng
  // paymentMethod:1 Phương thức thanh toán
  // paymentStatus:true Trạng thái thanh toán

  findOrderById() {
    this.trxTransaction
      .GetTrxTransactionFindById(this.activatedRoute.snapshot.params['id'])
      .subscribe({
        next: (respon: any) => {
          this.shoppingcartdto = respon.data;
          // vnpay va da thanh toan
          if (
            this.shoppingcartdto.trxTransaction.paymentTypes ===
              'PAYMENTVIACARD' &&
            this.shoppingcartdto.trxTransaction.paymentStatus === 'APPROVED'
          ) {
            this.isSuccessful = true;
            this.titleNotification = 'Thanh toán thành công';
          } else if (
            this.shoppingcartdto.trxTransaction.paymentTypes ===
              'PAYMENTVIACARD' &&
            this.shoppingcartdto.trxTransaction.paymentStatus === 'APPROVED'
          ) {
            this.isSuccessful = false;
            this.titleNotification = 'Thanh toán thất bại';
            this.descriptionNotification =
              'Thanh toán thất bại, vui lòng thử lại sau.';
          }
        },
        error: (error: any) => {
          if (error.status == 404) {
            console.log(error.error);
          } else {
            this.toastr.error('Lỗi thực hiện, vui lòng thử lại sau');
          }
        },
      });
  }
  goToHome() {
    return this.router.navigate(['/']);
  }

  goToOrderTracking(id: number) {
    return this.router.navigate(['/order_tracking/' + id]);
  }
}
