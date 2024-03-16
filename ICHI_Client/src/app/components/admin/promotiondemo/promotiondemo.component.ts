import { PromotionModel } from '../../../models/promotion.model';
import { Component, Inject, OnInit, inject } from '@angular/core';
import { PromotiondemoService } from '../../../service/promotiondemo.service';
import { Router } from '@angular/router';
import { DOCUMENT } from '@angular/common';
import { PaginationDTO } from '../../../dtos/pagination.dto';

@Component({
  selector: 'app-promotiondemo',
  templateUrl: './promotiondemo.component.html',
  styleUrl: './promotiondemo.component.css',
})
export class PromotiondemoComponent implements OnInit {
  paginationModel: PaginationDTO<PromotionModel[]> =
    PaginationDTO.createEmpty();
  products: PromotionModel[] = [];
  private productService = inject(PromotiondemoService);
  private router = inject(Router);
  // private location = inject(Location);
  // localStorage?: Storage;

  constructor(@Inject(DOCUMENT) private document: Document) {
    // this.localStorage = document.defaultView?.localStorage;
    debugger;
  }
  ngOnInit() {
    this.getProducts(1, 10, 'desc', 'id', '');
  }

  getProducts(
    pageSize: number,
    pageNumber: number,
    sortBy: string,
    sortDir: string,
    search: string
  ) {
    debugger;
    this.productService
      .getPromotiom(pageSize, pageNumber, sortBy, sortDir, search)
      .subscribe({
        next: (response: any) => {
          debugger;
          this.paginationModel.content = response.data.items;
          this.paginationModel.totalPages = response.data.pageCount;
          this.paginationModel.totalElements = response.data.totalCount;
          this.paginationModel.numberOfElements = response.numberOfElements;
          this.paginationModel.pageSize = response.data.pageSize;
          this.paginationModel.pageNumber = response.data.currentPage;
          this.paginationModel.firstElementOnPage = response.firstElementOnPage;
          this.paginationModel.lastElementOnPage = response.lastElementOnPage;
          this.paginationModel.sortBy = response.sortBy;
          this.paginationModel.sortDirection = response.sortDirection;
          console.log('Products:', this.paginationModel.content);
        },
        complete: () => {
          debugger;
        },
        error: (error: any) => {
          debugger;
          console.error('Error fetching products:', error);
        },
      });
  }
}
