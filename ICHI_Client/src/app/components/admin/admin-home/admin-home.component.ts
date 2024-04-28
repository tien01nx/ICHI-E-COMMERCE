import {
  AfterViewInit,
  Component,
  ElementRef,
  OnInit,
  ViewChild,
} from '@angular/core';
import Chart from 'chart.js/auto';
import { TrxTransactionService } from '../../../service/trx-transaction.service';
import flatpickr from 'flatpickr';
import { ProductsService } from '../../../service/products.service';
import { ProductDTO } from '../../../dtos/product.dto';
import { Environment } from '../../../environment/environment';
@Component({
  selector: 'app-admin-home',
  templateUrl: './admin-home.component.html',
  styleUrl: './admin-home.component.css',
})
export class AdminHomeComponent implements OnInit {
  constructor(
    private transactionService: TrxTransactionService,
    private productService: ProductsService
  ) {}
  Environment = Environment;

  ngOnInit(): void {
    this.createYears();
    this.onYearSelect({ target: { value: this.yearNow } });
    this.createPieChart();
    this.getData();
    this.createLineChart();
  }
  years: number[] = [];
  chooseYear: number = new Date().getFullYear();

  yearNow: number = new Date().getFullYear();
  monthNow: string = ('0' + (new Date().getMonth() + 1)).slice(-2); // Đảm bảo có hai chữ số cho tháng

  piechart: any;
  lineChart: any;
  orderStatus: any;
  moneyTotal: any;
  productdto: ProductDTO[] = [];
  colors = [
    'rgb(56, 116, 255)', // Màu cho 'pending'
    'rgb(38, 176, 4)', // Màu cho 'onHold'
    'rgb(0, 152, 235)', // Màu cho 'waitingForPickup'
    'rgb(97, 197, 255)', // Màu cho 'waitingForDelivery'
    'rgb(250, 189, 180)', // Màu cho 'delivered'
    'rgb(255, 204, 133)', // Màu cho 'cancelled'
  ];

  createYears() {
    // Tạo 5 năm trước
    for (let i = 5; i > 0; i--) {
      this.years.push(this.yearNow - i);
    }

    // Thêm năm hiện tại
    this.years.push(this.yearNow);

    // Tạo 5 năm sau
    for (let i = 1; i <= 5; i++) {
      this.years.push(this.yearNow + i);
    }
  }

  onYearSelect(event: any) {
    this.chooseYear = event.target.value;
    // data doanh thu và chi phí
    this.transactionService.getGetMonneyMoth(this.chooseYear).subscribe({
      next: (res: any) => {
        console.log(res);
        const data = res.data;
        this.updateChartData(data);
      },
      error: (err) => {
        console.log(err);
      },
    });
  }
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

    const dateTime = this.yearNow + '-' + this.monthNow;
    this.productTopFive(dateTime);
  }

  productTopFive(dateTime: string) {
    this.productService.ProductTopFive(dateTime).subscribe({
      next: (res: any) => {
        this.productdto = res.data;
        console.log('products', this.productdto);
      },
      error: (err) => {
        console.log(err);
      },
    });
  }

  updateChartData(data: any) {
    const totalOrderAmountData = data.map((item: any) => item.totalOrderAmount);
    const totalRealAmountData = data.map((item: any) => item.totalRealAmount);

    this.lineChart.data.datasets[0].data = totalOrderAmountData;
    this.lineChart.data.datasets[1].data = totalRealAmountData;

    this.lineChart.update();
  }

  createLineChart() {
    this.lineChart = new Chart('lineChart', {
      data: {
        labels: [
          'Tháng',
          'Tháng 1',
          'Tháng 2',
          'Tháng 3',
          'Tháng 4',
          'Tháng 5',
          'Tháng 6',
          'Tháng 7',
          'Tháng 8',
          'Tháng 9',
          'Tháng 10',
          'Tháng 11',
          'Tháng 12',
        ],
        datasets: [
          {
            type: 'line',
            label: 'Doanh thu: ',
            data: [],
            borderColor: 'green', // Màu viền của đường
            backgroundColor: 'lightgreen', // Màu nền của đường
            borderWidth: 2, // Độ dày của đường
            pointRadius: 5, // Kích thước của điểm
            pointBackgroundColor: 'green', // Màu của điểm
          },
          {
            type: 'line',
            label: 'Chi phí: ',
            data: [],
            borderColor: 'red', // Màu viền của đường
            backgroundColor: 'lightcoral', // Màu nền của đường
            borderWidth: 2, // Độ dày của đường
            pointRadius: 5, // Kích thước của điểm
            pointBackgroundColor: 'red', // Màu của điểm
          },
        ],
      },
      options: {
        scales: {
          y: {
            beginAtZero: true,
            // Tùy chỉnh các thuộc tính của trục y ở đây
          },
          x: {
            beginAtZero: true,
            // Tùy chỉnh các thuộc tính của trục x ở đây
          },
        },
      },
    });
  }
  changeDateBestSeller(event: any) {
    const selectedDate: string = event.target.value; // Lấy giá trị của control chọn tháng và năm
    this.productTopFive(selectedDate);
  }
}
