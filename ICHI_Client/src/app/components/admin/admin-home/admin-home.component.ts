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
import { InventorryReceiptsService } from '../../../service/inventory.receipts.service';
@Component({
  selector: 'app-admin-home',
  templateUrl: './admin-home.component.html',
  styleUrl: './admin-home.component.css',
})
export class AdminHomeComponent implements OnInit, AfterViewInit {
  constructor(
    private transactionService: TrxTransactionService,
    private productService: ProductsService,
    private InventoryService: InventorryReceiptsService
  ) {}
  ngAfterViewInit(): void {
    setTimeout(() => {
      this.createLineChart();
    }, 0);
  }
  Environment = Environment;

  ngOnInit(): void {
    this.createYears();
    this.onYearSelect({ target: { value: this.yearNow } });
    this.createPieChart();
    this.getData();
    this.getMoneyTotal();
    this.getdataInventoryModel();
  }
  titleShow: string = 'Doanh thu';
  show = true;

  intDisplay: number = 1;
  years: number[] = [];
  chooseYear: number = new Date().getFullYear();

  yearNow: number = new Date().getFullYear();
  monthNow: string = ('0' + (new Date().getMonth() + 1)).slice(-2); // Đảm bảo có hai chữ số cho tháng

  piechart: any;
  lineChart: any;
  orderStatus: any;
  moneyTotal: any;
  productdto: ProductDTO[] = [];
  dataDoanhThu: any;
  dataLoiNhuan: any;

  dataInventoryModel: any;

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
    if (this.intDisplay === 1) {
      this.transactionService.getGetMonneyMoth(this.chooseYear).subscribe({
        next: (res: any) => {
          console.log(res);
          this.dataDoanhThu = res.data;
          this.updateChartData(this.dataDoanhThu);
        },
        error: (err) => {
          console.log(err);
        },
      });
    }
    debugger;
    // data lợi nhuận
    if (this.intDisplay === 2) {
      this.transactionService.getCost(this.chooseYear).subscribe({
        next: (res: any) => {
          console.log('Lợi nhuân:', res.data);
          this.dataLoiNhuan = res.data;
          this.updateChartData(this.dataLoiNhuan);
        },
        error: (err) => {
          console.log(err);
        },
      });
    }

    this.getdataInventoryModel();
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
        // chỉ lấy 5 sản phẩm đầu tiên
        this.productdto = res.data.slice(0, 5);
        // this.productdto = res.data;
        console.log('products', this.productdto);
      },
      error: (err) => {
        console.log(err);
      },
    });
  }

  // Lợi nhuận

  getMoneyTotal() {
    this.transactionService.getCost(1).subscribe({
      next: (res: any) => {
        console.log(res);
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
    if (this.lineChart) {
      this.lineChart.destroy();
    }

    if (this.intDisplay === 1) {
      this.lineChart = new Chart('lineChart', {
        data: {
          labels: [
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

    if (this.intDisplay === 2) {
      this.lineChart = new Chart('lineChart', {
        data: {
          labels: [
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
              label: 'Lợi nhuận: ',
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
  }
  changeDateBestSeller(event: any) {
    const selectedDate: string = event.target.value; // Lấy giá trị của control chọn tháng và năm
    this.productTopFive(selectedDate);
  }

  changeDateLoiNhuan(event: any) {
    const selectedDate: string = event.target.value; // Lấy giá trị của control chọn tháng và năm
    // this.productTopFive(selectedDate);
  }

  showDisplay(int: number) {
    this.intDisplay = int;
    // Hủy biểu đồ hiện tại nếu có
    if (this.lineChart) {
      this.lineChart.destroy();
      this.lineChart = null; // Đặt lại biến lineChart
    }

    // Xử lý hiển thị dựa trên giá trị của intDisplay
    if (int === 1 || int === 2) {
      this.show = true; // Cho phép hiển thị biểu đồ
      if (int === 1) {
        this.titleShow = 'Doanh thu';
        this.createLineChart();
        this.updateChartData(this.dataDoanhThu);
      } else if (int === 2) {
        this.titleShow = 'Lợi nhuận';
        this.createLineChart();
        this.updateChartData(this.dataLoiNhuan);
      }
    } else {
      this.show = false; // Ẩn biểu đồ
      this.titleShow = 'Báo cáo nhập hàng';
    }
  }

  clickDownloadExcel() {
    this.transactionService.getExcel(this.chooseYear).subscribe(
      (blob: Blob) => {
        // Ensure blob is of type Blob
        const url = window.URL.createObjectURL(blob);
        const a = document.createElement('a');
        a.href = url;
        a.download = `report${this.chooseYear}.xlsx`;
        a.click();
        window.URL.revokeObjectURL(url);
        a.remove(); // Optionally remove the element after use
      },
      (error) => {
        console.error('Download error:', error); // Handle errors appropriately
      }
    );
  }

  getdataInventoryModel() {
    this.InventoryService.InventoryModel(this.chooseYear).subscribe({
      next: (res: any) => {
        console.log('respondata', res.data);
        this.dataInventoryModel = res.data;
      },
      error: (err) => {
        console.log(err);
      },
    });
  }
}
