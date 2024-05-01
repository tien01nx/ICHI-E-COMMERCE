import { TrxTransactionDetailModel } from './../../../models/trx.transaction.detail.model';
import { Component, OnInit } from '@angular/core';
import { TrxTransactionService } from '../../../service/trx-transaction.service';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Title } from '@angular/platform-browser';
import { Utils } from '../../../Utils.ts/utils';
import { Environment } from '../../../environment/environment';
import { OrderTrackingDTO } from '../../../dtos/order.tracking.dto';
import { VnPaymentRequestDTO } from '../../../dtos/vn.payment.request.dto';

@Component({
  selector: 'app-order-tracking',
  templateUrl: './order-tracking.component.html',
  styleUrl: './order-tracking.component.css',
})
export class OrderTrackingComponent implements OnInit {
  protected readonly Utils = Utils;
  protected readonly Environment = Environment;
  orderTrackingDTO: OrderTrackingDTO[] = [];
  priceDiscount: number = 0;
  priceShip = 0;
  data: VnPaymentRequestDTO = {
    trxTransactionId: 0,
    fullName: '',
    amount: 0,
    orderStatus: '',
    createDate: new Date(),
  };

  constructor(
    private toastr: ToastrService,
    private orderService: TrxTransactionService,
    private router: Router,
    private activatedRoute: ActivatedRoute,
    private titleService: Title
  ) {}
  ngOnInit(): void {
    this.getData();
  }

  getData() {
    this.orderService
      .getOrderTracking(this.activatedRoute.snapshot.params['id'])
      .subscribe({
        next: (response: any) => {
          console.log('data', response.data);
          if (response.code === 200) {
            this.orderTrackingDTO = response.data;

            this.orderTrackingDTO.forEach((item) => {
              // nếu discount > 0 thì tính ra số tiền giảm giá của sản phẩm đó
              if (item.discount > 0) {
                this.priceDiscount +=
                  item.price * item.quantity * (item.discount / 100);
              }
            });
            this.priceShip = this.orderTrackingDTO[0].trxTransaction.priceShip;
          } else {
            this.toastr.error(response.message);
          }
        },
        error: (error) => {
          this.toastr.error('Lỗi hệ thống');
        },
      });
  }
  getTotalPrice(): number {
    let totalPrice = 0;
    this.orderTrackingDTO.forEach((item) => {
      totalPrice += item.price * item.quantity;
    });
    totalPrice = totalPrice - this.priceDiscount + this.priceShip;
    return totalPrice;
  }
  getStatusClass(orderStatus: string): string {
    switch (orderStatus) {
      case 'Pending':
        return 'bg-success';
      case 'Processing':
        return 'bg-success';
      case 'Shipped':
        return 'bg-success';
      case 'Delivered':
        return 'bg-success';
      case 'Cancelled':
        return 'bg-success';
      default:
        return 'bg-warning';
    }
  }
  // thanh toán lại
  repayment() {
    let data: VnPaymentRequestDTO = {
      trxTransactionId: this.orderTrackingDTO[0].trxTransaction.id,
      fullName: this.orderTrackingDTO[0].trxTransaction.fullName,
      amount: this.getTotalPrice(),
      orderStatus: 'Pending',
      createDate: new Date(),
    };

    this.orderService.createPaymentUrl(data).subscribe({
      next: (response: any) => {
        if (response.code === 200) {
          window.location.href = response.data;
        }
      },
    });
  }

  getReportBill() {
    this.orderService
      .getReportBill(this.orderTrackingDTO[0].trxTransactionId)
      .subscribe(
        (blob: Blob) => {
          // Ensure blob is of type Blob
          const url = window.URL.createObjectURL(blob);
          const a = document.createElement('a');
          a.href = url;
          a.download = `report${this.orderTrackingDTO[0].trxTransactionId}.pdf`;
          a.click();
          window.URL.revokeObjectURL(url);
          a.remove(); // Optionally remove the element after use
        },
        (error) => {
          console.error('Download error:', error); // Handle errors appropriately
        }
      );
  }
}
