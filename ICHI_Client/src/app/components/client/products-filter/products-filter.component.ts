import { ProductsService } from './../../../service/products.service';
import { TrademarkService } from './../../../service/trademark.service';
import { Component, OnInit } from '@angular/core';
import { ClientFooterComponent } from '../client-footer/client-footer.component';
import { ClientMenuComponent } from '../client-menu/client-menu.component';
import { ClientHeaderComponent } from '../client-header/client-header.component';
import { TrademarkModel } from '../../../models/trademark.model';
import { Utils } from '../../../Utils.ts/utils';

@Component({
  selector: 'app-products-filter',
  templateUrl: './products-filter.component.html',
  styleUrl: './products-filter.component.css',
})
export class ProductsFilterComponent implements OnInit {
  trademarks: TrademarkModel[] = [];
  colors!: { name: string }[];
  constructor(
    private traremarkService: TrademarkService,
    private productService: ProductsService
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
  }
}
