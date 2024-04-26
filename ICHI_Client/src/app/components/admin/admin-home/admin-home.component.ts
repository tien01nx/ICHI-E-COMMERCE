import { Component, OnInit } from '@angular/core';
import Chart from 'chart.js/auto';
import { TrxTransactionService } from '../../../service/trx-transaction.service';
@Component({
  selector: 'app-admin-home',
  templateUrl: './admin-home.component.html',
  styleUrl: './admin-home.component.css',
})
export class AdminHomeComponent implements OnInit {
  constructor(private transactionService: TrxTransactionService) {}
  ngOnInit(): void {
    this.createPieChart();
    this.getData();
  }
  piechart: any;
  orderStatus: any;
  moneyTotal: any;
  colors = [
    'rgb(56, 116, 255)', // Màu cho 'pending'
    'rgb(38, 176, 4)', // Màu cho 'onHold'
    'rgb(0, 152, 235)', // Màu cho 'waitingForPickup'
    'rgb(97, 197, 255)', // Màu cho 'waitingForDelivery'
    'rgb(250, 189, 180)', // Màu cho 'delivered'
    'rgb(255, 204, 133)', // Màu cho 'cancelled'
  ];
  createPieChart() {
    this.transactionService.getOrderStatus().subscribe({
      next: (res: any) => {
        console.log(res);
        this.orderStatus = res.data;
        this.piechart = new Chart('piechart', {
          type: 'pie',
          data: {
            datasets: [
              {
                data: [
                  this.orderStatus.pending,
                  this.orderStatus.onHold,
                  this.orderStatus.waitingForPickup,
                  this.orderStatus.waitingForDelivery,
                  this.orderStatus.delivered,
                  this.orderStatus.cancelled,
                ],
                backgroundColor: this.colors,
              },
            ],
          },
        });
      },
      error: (err) => {
        console.log(err);
      },
    });
  }

  getData() {
    this.transactionService.getGetMonneyTotal().subscribe({
      next: (res: any) => {
        console.log(res);
        this.moneyTotal = res.data;
        console.log(this.moneyTotal);
      },
      error: (err) => {
        console.log(err);
      },
    });
  }
}
