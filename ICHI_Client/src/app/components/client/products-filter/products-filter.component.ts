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
import { CategoryProduct } from '../../../models/category.product';
import { CategoryService } from '../../../service/category-product.service';
import { identifierName } from '@angular/compiler';

@Component({
  selector: 'app-products-filter',
  templateUrl: './products-filter.component.html',
  styleUrl: './products-filter.component.css',
})
export class ProductsFilterComponent implements OnInit {
  paginationDTO: PaginationDTO<ProductDTO> = PaginationDTO.createEmpty();
  protected readonly Utils = Utils;
  Environment = Environment;
  trademarks: TrademarkModel[] = [];
  categories: CategoryProduct[] = [];
  colors!: { name: string }[];
  valueColors: string[] = [];
  valueCategories: string[] = [];
  valueTrademarks: string[] = [];
  valueFilterPriceMin: any;
  valueFilterPriceMax: any;

  constructor(
    private traremarkService: TrademarkService,
    private productService: ProductsService,
    private categoryService: CategoryService,
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

    this.categoryService
      .findAllByParentId(this.activatedRoute.snapshot.params['categoryName'])
      .subscribe((response: any) => {
        // this.categories = data.data;
        // console.log('category', this.categories);
        this.categories = response.data.filter(
          (item: CategoryProduct) =>
            item.categoryName !==
            this.activatedRoute.snapshot.params['categoryName']
        );
      });

    this.colors = Utils.createColorList();
    console.log(this.colors);

    this.activatedRoute.queryParams.subscribe((params) => {
      const pageSize = +params['page-size'] || 2;
      const pageNumber = +params['page-number'] || 1;

      let colors: string[] = [];
      let trademarks: string[] = [];
      let categories: string[] = [];

      params['colors']?.split(',').forEach((color: any) => {
        colors.push(color);
      });

      console.log('url', colors);

      params['category-parent']?.split(',').forEach((category: any) => {
        categories.push(category);
      });

      params['trademark']?.split(',').forEach((trademark: any) => {
        trademarks.push(trademark);
      });
      const priceMin = +params['gia-thap-nhat'] || '';
      const priceMax = +params['gia-cao-nhat'] || '';

      this.findByProductInCategory(
        this.activatedRoute.snapshot.params['categoryName'],
        colors,
        categories,
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
    categories: string[],
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
        categories,
        trademarks,
        priceMin,
        priceMax,
        pageSize,
        pageNumber
      )
      .subscribe((response: any) => {
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
        console.log(response.data);
        console.log('pageSize', this.paginationDTO.pageSize);
      });
  }

  filter(key: string, event: any): void {
    let values: any;
    if (event.target.checked) {
      if (key === 'colors') {
        this.valueColors.push(event.target.value);
        values = this.valueColors;
      } else if (key === 'category-parent') {
        this.valueCategories.push(event.target.value);
        values = this.valueCategories;
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
      } else if (key === 'category-parent') {
        this.valueCategories = this.valueCategories.filter(
          (value) => value !== event.target.value
        );
        values = this.valueCategories;
      } else if (key === 'trademark') {
        this.valueTrademarks = this.valueTrademarks.filter(
          (value) => value !== event.target.value
        );
        values = this.valueTrademarks;
      }
    }
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

  changePageNumber(pageNumber: number): void {
    this.router
      .navigate(
        [
          `/product_filter/` +
            this.activatedRoute.snapshot.params['categoryName'],
        ],
        {
          queryParams: { 'page-number': pageNumber },
          queryParamsHandling: 'merge',
        }
      )
      .then((r) => {});
  }

  changePageSize(pageSize: number): void {
    this.router
      .navigate(
        [
          '/product_filter' +
            this.activatedRoute.snapshot.params['categoryName'],
        ],
        {
          queryParams: { 'page-size': pageSize, 'page-number': 1 },
          queryParamsHandling: 'merge',
        }
      )
      .then((r) => {});
  }
}
