import { Environment } from './../../../environment/environment';
import { ProductsService } from './../../../service/products.service';
import { TrademarkService } from './../../../service/trademark.service';
import { Component, OnInit } from '@angular/core';
import { ClientFooterComponent } from '../client-footer/client-footer.component';
import { ClientMenuComponent } from '../client-menu/client-menu.component';
import { ClientHeaderComponent } from '../client-header/client-header.component';
import { TrademarkModel } from '../../../models/trademark.model';
import { Utils } from '../../../Utils.ts/utils';
import { ActivatedRoute, Router } from '@angular/router';
import { PaginationDTO } from '../../../dtos/pagination.dto';
import { ProductDTO } from '../../../dtos/product.dto';

@Component({
  selector: 'app-products-filter',
  templateUrl: './products-filter.component.html',
  styleUrl: './products-filter.component.css',
})
export class ProductsFilterComponent implements OnInit {
  paginationDTO: PaginationDTO<ProductDTO> = PaginationDTO.createEmpty();
  Environment = Environment;
  trademarks: TrademarkModel[] = [];
  colors!: { name: string }[];
  valueColors: string[] = [];
  valueTrademarks: string[] = [];
  valueFilterPriceMin: any;
  valueFilterPriceMax: any;

  constructor(
    private traremarkService: TrademarkService,
    private productService: ProductsService,
    private router: Router,
    protected activatedRoute: ActivatedRoute
  ) {}
  ngOnInit(): void {
    this.getDataTrademark();
  }

  getDataTrademark() {
    this.traremarkService.findAll().subscribe((data: any) => {
      this.trademarks = data.data.items;
      console.log(this.trademarks);
    });

    this.colors = Utils.createColorList();
    console.log(this.colors);

    this.activatedRoute.queryParams.subscribe((params) => {
      const pageSize = +params['page-size'] || 10;
      const pageNumber = +params['trang'] || 1;

      let colors: string[] = [];
      let trademarks: string[] = [];

      params['colors']?.split(',').forEach((color: any) => {
        colors.push(color);
      });

      console.log('url', colors);
      params['trademark']?.split(',').forEach((trademark: any) => {
        trademarks.push(trademark);
      });
      const priceMin = +params['gia-thap-nhat'] || '';
      const priceMax = +params['gia-cao-nhat'] || '';

      this.findByProductInCategory(
        this.activatedRoute.snapshot.params['categoryName'],
        colors,
        trademarks,
        priceMin,
        priceMax,
        pageSize,
        pageNumber
      );
    });
  }

  findByProductInCategory(
    categoryName: string,
    colors: string[],
    trademarks: string[],
    priceMin: any,
    priceMax: any,
    pageSize: number,
    pageNumber: number
  ) {
    this.productService
      .findProductToCategory(
        categoryName,
        colors,
        trademarks,
        priceMin,
        priceMax,
        pageSize,
        pageNumber
      )
      .subscribe((response: any) => {
        console.log(response.data);
        this.paginationDTO.content = response.data.items;
        this.paginationDTO.totalPages = response.data.pageCount;
        this.paginationDTO.totalElements = response.data.totalCount;
        this.paginationDTO.numberOfElements = response.numberOfElements;
        this.paginationDTO.pageSize = response.data.pageSize;
        this.paginationDTO.pageNumber = response.data.currentPage;
        this.paginationDTO.firstElementOnPage = response.firstElementOnPage;
        this.paginationDTO.lastElementOnPage = response.lastElementOnPage;
        this.paginationDTO.sortBy = response.sortBy;
        this.paginationDTO.sortDirection = response.sortDirection;
      });
  }

  filter(key: string, event: any): void {
    let values: any;
    if (event.target.checked) {
      if (key === 'colors') {
        this.valueColors.push(event.target.value);
        values = this.valueColors;
      } else if (key === 'trademark') {
        this.valueTrademarks.push(event.target.value);
        values = this.valueTrademarks;
      }
    } else {
      if (key === 'colors') {
        this.valueColors = this.valueColors.filter(
          (value) => value !== event.target.value
        );
        values = this.valueColors;
      } else if (key === 'trademark') {
        this.valueTrademarks = this.valueTrademarks.filter(
          (value) => value !== event.target.value
        );
        values = this.valueTrademarks;
      }
    }
    console.log('colors', this.valueColors);
    console.log('trademark', this.valueTrademarks);
    console.log(this.valueFilterPriceMin);
    console.log(this.valueFilterPriceMax);
    this.router
      .navigate(
        [
          '/product_filter/' +
            this.activatedRoute.snapshot.params['categoryName'],
        ],
        {
          queryParams: { [key]: values.toString() },
          queryParamsHandling: 'merge',
        }
      )
      .then();
  }

  filterPrice(): void {
    this.router
      .navigate(
        [
          '/product_filter/' +
            this.activatedRoute.snapshot.params['categoryName'],
        ],
        {
          queryParams: {
            'gia-thap-nhat': this.valueFilterPriceMin,
            'gia-cao-nhat': this.valueFilterPriceMax,
          },
          queryParamsHandling: 'merge',
        }
      )
      .then();
  }
  productDetail(id: number) {
    this.router.navigate(['/product_detail/' + id]);
  }
}
