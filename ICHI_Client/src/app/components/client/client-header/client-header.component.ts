import { Environment } from './../../../environment/environment';
import { Component } from '@angular/core';
import { TokenService } from '../../../service/token.service';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { TrxTransactionService } from '../../../service/trx-transaction.service';
import { ProductsService } from '../../../service/products.service';
import { ProductDTO } from '../../../dtos/product.dto';

@Component({
  selector: 'app-client-header',
  templateUrl: './client-header.component.html',
  styleUrl: './client-header.component.css',
})
export class ClientHeaderComponent {
  cartItemCount: number = 0;
  productdto: ProductDTO[] = [];
  email = this.tokenService.getUserEmail();
  Environment = Environment;
  constructor(
    private tokenService: TokenService,
    private router: Router,
    private toastr: ToastrService,
    private cartService: TrxTransactionService,
    private productService: ProductsService
  ) {}

  ngOnInit(): void {
    this.cartService
      .getCartItemCount(this.tokenService.getUserEmail())
      .subscribe((count) => {
        this.cartItemCount = count;
      });
  }

  signout() {
    this.tokenService.removeToken();
    // this.router.navigate(['/']);
    window.location.href = '/';
    this.toastr.success('Đăng xuất thành công');
  }

  login() {
    this.router.navigate(['login']);
  }

  resetPassword() {
    this.router.navigate(['reset-password']);
  }
  search: string = '';
  searchIndex: number = 0;
  products: any[] = [];
  searchProduct(event: KeyboardEvent) {
    // Kiểm tra nếu phím nhấn là Backspace (keyCode = 8) và độ dài của chuỗi search là 0
    if (event.key === 'Backspace' && this.search.length === 0) {
      this.search = ''; // Đặt giá trị của biến search về ''
    }

    this.searchIndex++;

    if (this.search === '') {
      this.productdto = [];
      return;
    }
    this.productService.searchProductName(this.search).subscribe({
      next: (response: any) => {
        console.log(response);
        this.productdto = response.data;
        console.log('search:', this.productdto);
        this.products = response;
      },
      error: (error) => {
        console.log(error);
      },
    });
  }

  onSearch() {
    // lưu vào localStorage
    // Lấy danh sách tìm kiếm đã lưu từ localStorage, hoặc tạo một mảng mới nếu không tồn tại
    let searches: { value: string; timestamp: number }[] = JSON.parse(
      localStorage.getItem('searches') || '[]'
    );

    //kiểm tra xem giá trị tìm kiếm đã tồn tại trong mảng chưa không phân biệt hoa thường
    const index = searches.findIndex(
      (item) => item.value.toLowerCase() === this.search.toLowerCase()
    );
    // Nếu tồn tại thì xóa giá trị tìm kiếm đó khỏi mảng và lưu giá trị mới vào vị trí đầu tiên
    if (index !== -1) {
      searches.splice(index, 1);
    }

    // Thêm giá trị tìm kiếm mới vào mảng với thời gian hiện tại
    searches.push({ value: this.search, timestamp: Date.now() });

    // Lưu lại mảng tìm kiếm vào localStorage
    localStorage.setItem('searches', JSON.stringify(searches));

    //this.router.navigate(['/product'], { queryParams: { search: this.search } });

    // dùng href giữ nguyên param trên url
    this.router.navigate(['//product_filter/', this.search]);
    // window.location.href = `/product?search=${this.search}`;
  }
}
